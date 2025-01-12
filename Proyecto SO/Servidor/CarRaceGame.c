#include <mysql.h>
#include <sys/types.h> 
#include <sys/socket.h> 
#include <netinet/in.h> 
#include <stdio.h>
#include <string.h>
#include <stdlib.h>
#include <pthread.h>
#include <unistd.h>

// Definiciones
#define MAX_JUGADORES 100
#define MAX_NOMBRE 20
#define TAM_BUFFER 512
#define PORT 50050
#define MAX 200

// Estructuras
typedef struct {
    int sock;
    char nombre[MAX_NOMBRE];
	int en_partida; // Nuevo campo: 0 si no está en partida, 1 si está en partida
} Jugador;

typedef struct {
    Jugador jugador[MAX_JUGADORES];
    int numero_jugadores;
} Lista_Jugadores;

typedef struct {
    int sock_conn;
    char nombre[MAX_NOMBRE];
    char contrasenya[MAX_NOMBRE];
} DatosLogin;

// Estado del juego
typedef struct {
    int player1_position;
    int player2_position;
    int finish_line;
    int player1_socket;
    int player2_socket;
} GameState;

GameState game_state;
pthread_mutex_t mtx_game_state = PTHREAD_MUTEX_INITIALIZER;
Lista_Jugadores Jugadores;
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
MYSQL *conn;

// Función para actualizar las posiciones
void update_position(int player, int increment) {
    pthread_mutex_lock(&mtx_game_state);

    if (player == 1) {
        game_state.player1_position += increment;
        if (game_state.player1_position >= game_state.finish_line) {
            printf("¡Jugador 1 gana!\n");
            send_winner_notification(1);
            reset_game();
        }
    } else if (player == 2) {
        game_state.player2_position += increment;
        if (game_state.player2_position >= game_state.finish_line) {
            printf("¡Jugador 2 gana!\n");
            send_winner_notification(2);
            reset_game();
        }
    }

    pthread_mutex_unlock(&mtx_game_state);
}

// Función para notificar a los clientes sobre el ganador
void send_winner_notification(int winner) {
    char msg[100];
    sprintf(msg, "WINNER/%d", winner);

    // Enviar el mensaje a ambos jugadores
    if (game_state.player1_socket != -1) {
        write(game_state.player1_socket, msg, strlen(msg));
    }
    if (game_state.player2_socket != -1) {
        write(game_state.player2_socket, msg, strlen(msg));
    }
}

// Función para reiniciar el juego
void reset_game() {
    game_state.player1_position = 0;
    game_state.player2_position = 0;
}



// Funciￃﾳn para conectar a la base de datos
void conectarBD() {
    conn = mysql_init(NULL);
    if (conn == NULL) {
        printf("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
        exit(1);
    }
    
    conn = mysql_real_connect(conn, "shiva2.upc.es", "root", "mysql", "M9_BBDDSERVIDOR", 0, NULL, 0);
    if (conn == NULL) {
        printf("Error al inicializar la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
        exit(1);
    }
}

  

// Funciￃﾳn para ejecutar consultas SQL
void ejecutarConsultaSQL(char *sql) {
    if (mysql_query(conn, sql) != 0) {
        fprintf(stderr, "Error en la consulta: %s\n", mysql_error(conn));
        return -1; // Retorna un valor de error
    }
    return 0; // ￉xito
}
// Funciￃﾳn para notificar lista jugadores conectados
void notificarListaConectados() {

	char mensaje[1024] = "4/Jugadores conectados:"; // Usamos el formato completo
	pthread_mutex_lock(&mutex); // Bloqueamos el acceso a la lista de jugadores
	
	// Concatenar cada nombre de jugador al mensaje
	for (int i = 0; i < Jugadores.numero_jugadores; i++) {
		if (Jugadores.jugador[i].nombre[0] != '\0') { // Verificar que el nombre no sea vacￃﾭo
			snprintf(mensaje + strlen(mensaje), sizeof(mensaje) - strlen(mensaje) - 1, " %s", Jugadores.jugador[i].nombre);
		}
	}
	pthread_mutex_unlock(&mutex); // Liberamos el acceso
	
	// Enviar el mensaje a todos los jugadores conectados
	for (int i = 0; i < Jugadores.numero_jugadores; i++) {
		int sock = Jugadores.jugador[i].sock;
		if (sock > 0) { // Verificar que el socket es vￃﾡlido
			if (write(sock, mensaje, strlen(mensaje)) < 0) {
				perror("Error al enviar lista a un cliente");
			}
		}
	}
	
	printf("Lista enviada a todos los jugadores: %s\n", mensaje); // Debug


}
// Verifica si un jugador ya estￃﾡ conectado
int jugador_ya_conectado(const char *nombre) {
    for (int i = 0; i < Jugadores.numero_jugadores; i++) {
        if (strcmp(Jugadores.jugador[i].nombre, nombre) == 0 && Jugadores.jugador[i].en_partida) {
            return 1; // Jugador ya está en una partida
        }
    }
    return 0;
}


// Buscar el socket de un jugador en la lista de jugadores conectados
int BuscarSocketJugador(Lista_Jugadores *ListaJugadoresConectados, const char *NombreaBuscar) {
    pthread_mutex_lock(&mutex);
    for (int i = 0; i < ListaJugadoresConectados->numero_jugadores; i++) {
        if (strcmp(ListaJugadoresConectados->jugador[i].nombre, NombreaBuscar) == 0) {
            pthread_mutex_unlock(&mutex);
            return ListaJugadoresConectados->jugador[i].sock;
        }
    }
    pthread_mutex_unlock(&mutex);
    return -1; // No encontrado
}
// Buscar el nombre de un jugador por su socket
const char* BuscarNombrePorSocket(Lista_Jugadores *ListaJugadoresConectados, int sock) {
    pthread_mutex_lock(&mutex); // Bloquear el acceso a la lista
    for (int i = 0; i < ListaJugadoresConectados->numero_jugadores; i++) {
        if (ListaJugadoresConectados->jugador[i].sock == sock) {
            pthread_mutex_unlock(&mutex); // Liberar el acceso
            return ListaJugadoresConectados->jugador[i].nombre;
        }
    }
    pthread_mutex_unlock(&mutex); // Liberar el acceso
    return NULL; // No encontrado
}


// Manejo de la desconexiￃﾳn
void manejarDesconexion(int sock_conn) {
    pthread_mutex_lock(&mutex); // Bloqueamos el acceso a la lista de jugadores

    int jugador_eliminado = 0;

    // Buscar el jugador por su socket
    for (int j = 0; j < Jugadores.numero_jugadores; j++) {
        if (Jugadores.jugador[j].sock == sock_conn) {
            // Eliminamos al jugador desplazando los elementos hacia la izquierda
            for (int k = j; k < Jugadores.numero_jugadores - 1; k++) {
                Jugadores.jugador[k] = Jugadores.jugador[k + 1];
            }
            // Reducimos el n￺mero de jugadores conectados
            Jugadores.numero_jugadores--;
            jugador_eliminado = 1;
            printf("Jugador con socket %d desconectado.\n", sock_conn);
            break;
        }
    }

    // Si el jugador no fue encontrado
    if (!jugador_eliminado) {
        printf("Jugador con socket %d no encontrado.\n", sock_conn);
    }

    pthread_mutex_unlock(&mutex); // Liberamos el acceso a la lista de jugadores

    // Notificamos a los dem￡s jugadores
    notificarListaConectados();

    // Aqu￭ podr￭as cerrar el socket del cliente de forma segura
    close(sock_conn);
    printf("Socket %d cerrado.\n", sock_conn);
}

// Registro de jugador
void manejarRegistro(int sock_conn, char *nombre, char *contrasenya) {
    char comando[512];
    char respuesta[512];

    // Validaci￳n simple de entrada (no permitir cadenas vac￭as)
    if (nombre == NULL || contrasenya == NULL || strlen(nombre) == 0 || strlen(contrasenya) == 0) {
        sprintf(respuesta, "1/Nombre y/o contrase￱a inv￡lidos.");
        write(sock_conn, respuesta, strlen(respuesta));
        return;
    }

    // Consulta para verificar si el jugador ya existe
    snprintf(comando, sizeof(comando), "SELECT Name FROM Player WHERE Name='%s'", nombre);

    if (mysql_query(conn, comando) != 0) {
        sprintf(respuesta, "1/Error al realizar la consulta: %s", mysql_error(conn));
        write(sock_conn, respuesta, strlen(respuesta));
        return;
    }

    MYSQL_RES* result = mysql_store_result(conn);
    if (result == NULL) {
        sprintf(respuesta, "1/Error al procesar los resultados: %s", mysql_error(conn));
        write(sock_conn, respuesta, strlen(respuesta));
        return;
    }

    if (mysql_num_rows(result) > 0) {
        // El jugador ya existe
        sprintf(respuesta, "1/El jugador %s ya existe.", nombre);
    }
    else {
        // El jugador no existe, proceder a registrar
        snprintf(comando, sizeof(comando), "INSERT INTO Player (Name, Password) VALUES ('%s', '%s')", nombre, contrasenya);

        if (mysql_query(conn, comando) != 0) {
            sprintf(respuesta, "1/Error al registrar el jugador: %s", mysql_error(conn));
        }
        else {
            sprintf(respuesta, "1/Registro exitoso para %s", nombre);
        }
    }

    // Liberar resultados y enviar respuesta
    mysql_free_result(result);
    write(sock_conn, respuesta, strlen(respuesta));
}

void* manejarLoginThread(void* arg) {
    DatosLogin* datos = (DatosLogin*)arg;
    int sock_conn = datos->sock_conn;
    char* nombre = datos->nombre;
    char* contrasenya = datos->contrasenya;
    char comando[256];
    char respuesta[256];

    // Realizar la consulta SQL para verificar el login
    sprintf(comando, "SELECT Name FROM Player WHERE Name='%s' AND Password='%s'", nombre, contrasenya);
    if (mysql_query(conn, comando) == 0) {
        MYSQL_RES* result = mysql_store_result(conn);
        if (mysql_num_rows(result) > 0) {
            pthread_mutex_lock(&mutex);
            if (jugador_ya_conectado(nombre)) {
                sprintf(respuesta, "2/El jugador %s ya est￡ conectado.", nombre);
            }
            else {
                sprintf(respuesta, "2/OK");
                strcpy(Jugadores.jugador[Jugadores.numero_jugadores].nombre, nombre);
                Jugadores.jugador[Jugadores.numero_jugadores].sock = sock_conn;
                Jugadores.numero_jugadores++;
            }
            pthread_mutex_unlock(&mutex);
        }
        else {
            sprintf(respuesta, " 2/Usuario o contrase￱a incorrectos");
        }
        mysql_free_result(result);
    }
    else {
        sprintf(respuesta, "2/Error al realizar la consulta");
    }

    // Notificar a los jugadores conectados
    notificarListaConectados();
	
   
    write(sock_conn, respuesta, strlen(respuesta));

    // Liberar la memoria de los datos del hilo
    free(datos);
    return NULL;
}
// Inicio de sesiￃﾳn
void manejarLogin(int sock_conn, char *nombre, char *contrasenya) {
    // Crear una estructura de datos para pasar al hilo
    DatosLogin* datos = (DatosLogin*)malloc(sizeof(DatosLogin));
    datos->sock_conn = sock_conn;
    strcpy(datos->nombre, nombre);
    strcpy(datos->contrasenya, contrasenya);

    // Crear un hilo para manejar el login
    pthread_t thread;
    if (pthread_create(&thread, NULL, manejarLoginThread, (void*)datos) != 0) {
        perror("2/Error al crear el hilo de login");
        free(datos);
    }

    // Detach el hilo para que se libere autom￡ticamente cuando termine
    pthread_detach(thread);
	
}

// Lista de jugadores conectados
void manejarListaConectados(int sock_conn) {
	// Bloqueo para asegurar acceso seguro a la lista de jugadores
	char mensaje[1024] = "4/Jugadores conectados:"; // Usamos el mismo formato
	pthread_mutex_lock(&mutex); // Bloqueamos el acceso a la lista de jugadores
	
	// Concatenar cada nombre de jugador al mensaje
	for (int i = 0; i < Jugadores.numero_jugadores; i++) {
		if (Jugadores.jugador[i].nombre[0] != '\0') { // Verificar que el nombre no sea vacￃﾭo
			snprintf(mensaje + strlen(mensaje), sizeof(mensaje) - strlen(mensaje) - 1, " %s", Jugadores.jugador[i].nombre);
		}
	}
	pthread_mutex_unlock(&mutex); // Liberamos el acceso
	
	// Enviar el mensaje solo al cliente conectado (sock_conn)
	if (write(sock_conn, mensaje, strlen(mensaje)) < 0) {
		perror("Error al enviar lista a un cliente");
	}
	
	printf("Lista enviada a un solo jugador: %s\n", mensaje); // Debug
}



	
	
// Funciￃﾳn para atender jugadores
void *AtenderJugador(void *socket) {
    int sock_conn = *(int*)socket;
	int player = 0;
    char peticion[512];
    int ret, terminar = 0;
	// Asignar jugador al conectarse
    pthread_mutex_lock(&mtx_game_state);
    if (game_state.player1_socket == 0) {
        game_state.player1_socket = sock_conn;
        player = 1;
        printf("Jugador 1 conectado.\n");
    } else if (game_state.player2_socket == 0) {
        game_state.player2_socket = sock_conn;
        player = 2;
        printf("Jugador 2 conectado.\n");
    } else {
        // Rechazar conexión si ya hay dos jugadores
        printf("Conexión rechazada: ya hay dos jugadores conectados.\n");
        pthread_mutex_unlock(&mtx_game_state);
        close(sock_conn);
        return NULL;
    }
    pthread_mutex_unlock(&mtx_game_state);

    while (!terminar) {
    ret = read(sock_conn, peticion, sizeof(peticion) - 1);  // Dejamos espacio para el '\0'

    if (ret < 0) {
        perror("Error al leer del socket");
        break;  // Salir si hay un error
    }

    if (ret == 0) {
        // El cliente se ha desconectado (retorno 0 de `read` significa desconexión)
        printf("El cliente ha cerrado la conexión\n");
        
       

        // Cerrar la conexión del jugador desconectado
        close(sock_conn);
        return NULL;  // Salir de la función
    }

    peticion[ret] = '\0';  // Aseguramos que la cadena esté terminada en '\0'
    printf("Petición recibida: %s\n", peticion);

        // Usamos strtok para separar el c￳digo de la solicitud
        char* p = strtok(peticion, "/");
        if (p == NULL) {
            printf("Error en la solicitud: c￳digo no encontrado\n");
            break;
        }

        int codigo = atoi(p);  // Convertimos el c￳digo a entero
        char nombre[20], contrasenya[20], respuesta[30];char query[1024];

        switch (codigo) {
            case 0: // Desconexiￃﾳn
				printf("Jugador %d solicitó desconexión. Socket: %d\n", player, sock_conn);
				
				// Bloquear el estado del juego para liberar el slot
				pthread_mutex_lock(&mtx_game_state);
				if (player == 1) {
					game_state.player1_socket = 0;  // Liberar slot del jugador 1
					printf("Jugador 1 desconectado. Slot liberado. Socket: %d\n", sock_conn);
				} else if (player == 2) {
					game_state.player2_socket = 0;  // Liberar slot del jugador 2
					printf("Jugador 2 desconectado. Slot liberado. Socket: %d\n", sock_conn);
				} else {
					printf("Desconexión recibida, pero el jugador no tiene un slot asignado. Socket: %d\n", sock_conn);
				}
				pthread_mutex_unlock(&mtx_game_state);
				
				// Preparar la respuesta al cliente
				sprintf(respuesta, "0/Jugador desconectado\n");
				write(sock_conn, respuesta, strlen(respuesta));
				
				// Manejar desconexión
                terminar = 1;  // Salir del bucle
                manejarDesconexion(sock_conn);  // Llamar a la funci￳n de desconexi￳n
                close(sock_conn);  // Cerrar el socket
				printf("Socket cerrado para el jugador %d.\n", player);
                sprintf(respuesta, "0/Jugador desconectado\n");
				write(sock_conn, respuesta, strlen(respuesta));
                break;
            case 1: // Registro
                p = strtok(NULL, "/");
                if (p == NULL) {
                    sprintf(respuesta, "1/Nombre no proporcionado para registro\n");
					write(sock_conn, respuesta, strlen(respuesta));
                    break;  // Error si no se proporciona el nombre
                }
                strncpy(nombre, p, sizeof(nombre) - 1);  // Usar strncpy para evitar desbordamientos
                nombre[sizeof(nombre) - 1] = '\0';  // Asegurar la terminaci￳n de la cadena

                p = strtok(NULL, "/");
                if (p == NULL) {
                    sprintf(respuesta, "1/Contrase￱a no proporcionada para registro\n");
					write(sock_conn, respuesta, strlen(respuesta));
                    break;  // Error si no se proporciona la contrase￱a
                }
                strncpy(contrasenya, p, sizeof(contrasenya) - 1);  // Usar strncpy para evitar desbordamientos
                contrasenya[sizeof(contrasenya) - 1] = '\0';  // Asegurar la terminaci￳n de la cadena

                manejarRegistro(sock_conn, nombre, contrasenya);  // Registrar al jugador
                break;
            case 2: // Iniciar sesion
				p = strtok(NULL, "/");
				if (p == NULL) {
					sprintf(respuesta, "2/Nombre no proporcionado para login\n");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				strncpy(nombre, p, sizeof(nombre) - 1);
				nombre[sizeof(nombre) - 1] = '\0';
				p = strtok(NULL, "/");
				if (p == NULL) {
					sprintf(respuesta, "2/Contrase￱a no proporcionada para login\n");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				strncpy(contrasenya, p, sizeof(contrasenya) - 1);
				contrasenya[sizeof(contrasenya) - 1] = '\0';
				
				// Solo llama a manejarLogin, no envￃﾭes mensajes aquￃﾭ
				manejarLogin(sock_conn, nombre, contrasenya);
				break;
				
			case 3: { // Baja del sistema
				char nombre[20];
				char respuesta[256];
				
				p = strtok(NULL, "/");
				if (p == NULL) {
					sprintf(respuesta, "3/ERROR/Faltan parￃﾡmetros");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				strncpy(nombre, p, sizeof(nombre) - 1);
				nombre[sizeof(nombre) - 1] = '\0';
				
				pthread_mutex_lock(&mutex);
				int conectado = jugador_ya_conectado(nombre);
				pthread_mutex_unlock(&mutex);
				if (!conectado) {
					sprintf(respuesta, "3/ERROR/El jugador %s no estￃﾡ conectado", nombre);
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				
				if (conn == NULL) {
					sprintf(respuesta, "3/ERROR/No se pudo conectar a la base de datos");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				char comando[256];
				sprintf(comando, "DELETE FROM Player WHERE Name='%s'", nombre);
				printf("Ejecutando comando SQL: %s\n", comando);
				
				if (mysql_query(conn, comando) == 0) {
					manejarDesconexion(sock_conn); // Eliminar el jugador de la lista conectada
					sprintf(respuesta, "3/BAJA OK/El jugador %s ha sido eliminado del sistema", nombre);
				} else {
					printf("Error al eliminar al jugador %s: %s\n", nombre, mysql_error(conn));
					sprintf(respuesta, "3/ERROR/Error al eliminar al jugador %s: %s", nombre, mysql_error(conn));
					printf(respuesta);
				}
				
				if (send(sock_conn, respuesta, strlen(respuesta), MSG_NOSIGNAL) == -1) {
					perror("Error al enviar la respuesta");
				}
				
				break;
			}
				
			
            case 4: // Lista de jugadores conectados
                manejarListaConectados(sock_conn);
				break;
				
			case 7: {
				char Gestion[20];
				char UsuarioContrincante[MAX_NOMBRE];
				char Respuesta[TAM_BUFFER];

				p = strtok(NULL, "/"); // Gestión a realizar
				if (p == NULL) {
					sprintf(Respuesta, "7/ERROR/Faltan parámetros");
					write(sock_conn, Respuesta, strlen(Respuesta));
					break;
				}
				strncpy(Gestion, p, sizeof(Gestion) - 1);
				Gestion[sizeof(Gestion) - 1] = '\0';

				p = strtok(NULL, "/"); // Nombre del contrincante
				if (p == NULL) {
					sprintf(Respuesta, "7/ERROR/Faltan parámetros");
					write(sock_conn, Respuesta, strlen(Respuesta));
					break;
				}
				strncpy(UsuarioContrincante, p, sizeof(UsuarioContrincante) - 1);
				UsuarioContrincante[sizeof(UsuarioContrincante) - 1] = '\0';

				int SocketContrincante = BuscarSocketJugador(&Jugadores, UsuarioContrincante);
				if (SocketContrincante == -1) {
					sprintf(Respuesta, "7/ERROR/Usuario %s no conectado", UsuarioContrincante);
					write(sock_conn, Respuesta, strlen(Respuesta));
					break;
				}

				const char* NombreSolicitante = BuscarNombrePorSocket(&Jugadores, sock_conn);
				if (NombreSolicitante == NULL) {
					sprintf(Respuesta, "7/ERROR/No se pudo identificar al jugador solicitante");
					write(sock_conn, Respuesta, strlen(Respuesta));
					break;
				}

				pthread_mutex_lock(&mutex);

				if (strcmp(Gestion, "ENVIAR") == 0) {
					// Verificar que el jugador contrincante no esté en una partida
					if (jugador_ya_conectado(UsuarioContrincante)) {
						sprintf(Respuesta, "7/ERROR/El jugador %s ya está en una partida", UsuarioContrincante);
						write(sock_conn, Respuesta, strlen(Respuesta));
					} else {
						// Enviar invitación al contrincante
						sprintf(Respuesta, "7/RECIBIR/%s", NombreSolicitante);
						write(SocketContrincante, Respuesta, strlen(Respuesta));
						printf("Enviado a %d: %s\n", SocketContrincante, Respuesta);
					}
				} else if (strcmp(Gestion, "ACEPTAR") == 0) {
					// Registrar los sockets en el GameState
					

					// Avisar a ambos jugadores que comienza la partida
					char mensaje[TAM_BUFFER];
					sprintf(mensaje, "9/START_GAME/%s/%s", NombreSolicitante, UsuarioContrincante);

					// Enviar a ambos jugadores
					write(sock_conn, mensaje, strlen(mensaje)); // Al jugador que aceptó
					write(SocketContrincante, mensaje, strlen(mensaje)); // Al contrincante
					printf("Partida aceptada entre %s y %s\n", NombreSolicitante, UsuarioContrincante);

				} else if (strcmp(Gestion, "RECHAZAR") == 0) {
					// Notificar al contrincante que fue rechazado
					sprintf(Respuesta, "7/RECHAZADO/%s", NombreSolicitante);
					write(SocketContrincante, Respuesta, strlen(Respuesta));
					printf("Partida rechazada por %s\n", NombreSolicitante);
				}

				pthread_mutex_unlock(&mutex);
				break;
			}




				
			case 8: { // Código para manejar el chat
				char destinatario[MAX_NOMBRE];
				char mensaje[TAM_BUFFER];
				char respuesta[TAM_BUFFER];
				int socket_destinatario;

				// Extraer el destinatario del mensaje
				p = strtok(NULL, "/");
				if (p == NULL || strlen(p) == 0) {  // Validar que el destinatario no esté vacío
					snprintf(respuesta, sizeof(respuesta), "8/ERROR/Faltó el destinatario");
					write(sock_conn, respuesta, strlen(respuesta));
					printf("8/ERROR/Faltó el destinatario\n");
					break;
				}
				snprintf(destinatario, sizeof(destinatario), "%s", p);

				// Extraer el mensaje
				p = strtok(NULL, "/");
				if (p == NULL || strlen(p) == 0) {  // Validar que el mensaje no esté vacío
					snprintf(respuesta, sizeof(respuesta), "8/ERROR/Mensaje vacío");
					write(sock_conn, respuesta, strlen(respuesta));
					printf("8/ERROR/Mensaje vacío\n");
					break;
				}
				snprintf(mensaje, sizeof(mensaje), "%s", p);
				printf("Mensaje recibido para enviar: %s\n", mensaje);
            

				// Buscar el socket del destinatario
				socket_destinatario = BuscarSocketJugador(&Jugadores, destinatario);
				// Buscar el nombre del remitente
				const char* remitente = BuscarNombrePorSocket(&Jugadores, sock_conn);
				// Liberar el mutex antes de enviar mensajes
				
				snprintf(respuesta, sizeof(respuesta), "8/%s/%s", remitente, mensaje);
				write(socket_destinatario, respuesta, strlen(respuesta));
				printf("Mensaje enviado de %s a %s: %s\n", remitente, destinatario, mensaje);
				snprintf(respuesta, sizeof(respuesta), "8/OK/Mensaje enviado a %s", destinatario);
				printf(respuesta);
				break;
			}
            
            
            case 10: {
				p = strtok(NULL, "/");
				if (p == NULL) {
					sprintf(respuesta, "10/Nombre del jugador no proporcionado\n");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				strncpy(nombre, p, sizeof(nombre) - 1);
				nombre[sizeof(nombre) - 1] = '\0';
				p = strtok(NULL, "/");
				if (p == NULL) {
					sprintf(respuesta, "10/Nombre del oponente no proporcionado\n");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				char oponente[20];
				strncpy(oponente, p, sizeof(oponente) - 1);
				oponente[sizeof(oponente) - 1] = '\0';
				sprintf(query, 
						"SELECT g.Identifier, g.Winner, g.EndDateTime "
						"FROM Game g "
						"JOIN Participation p1 ON g.Identifier = p1.Game "
						"JOIN Participation p2 ON g.Identifier = p2.Game "
						"WHERE p1.Player = '%s' AND p2.Player = '%s';",
						nombre, oponente);
				if (mysql_query(conn, query) == 0) {
					MYSQL_RES *res = mysql_store_result(conn);
					MYSQL_ROW row;
					strcpy(respuesta, "10/");
					while ((row = mysql_fetch_row(res)) != NULL) {
						strcat(respuesta, "Partida ");
						strcat(respuesta, row[0]);  // ID
						strcat(respuesta, ": Ganador ");
						strcat(respuesta, row[1]);  // Winner
						strcat(respuesta, " en ");
						strcat(respuesta, row[2]);  // Fecha
						strcat(respuesta, "\n");
					}
				} else {
					sprintf(respuesta, "10/Error al realizar la consulta\n");
				}
				write(sock_conn, respuesta, strlen(respuesta));
				break;
            }
            case 11: {
				p = strtok(NULL, "/");
				if (p == NULL) {
					sprintf(respuesta, "11/Fecha de inicio no proporcionada\n");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				char fecha_inicio[20];
				strncpy(fecha_inicio, p, sizeof(fecha_inicio) - 1);
				fecha_inicio[sizeof(fecha_inicio) - 1] = '\0';
				p = strtok(NULL, "/");
				if (p == NULL) {
					sprintf(respuesta, "11/Fecha de fin no proporcionada\n");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				char fecha_fin[20];
				strncpy(fecha_fin, p, sizeof(fecha_fin) - 1);
				fecha_fin[sizeof(fecha_fin) - 1] = '\0';
				sprintf(query, 
						"SELECT Identifier, Winner, EndDateTime "
						"FROM Game "
						"WHERE EndDateTime BETWEEN '%s' AND '%s';",
						fecha_inicio, fecha_fin);
				
				if (mysql_query(conn, query) == 0) {
					MYSQL_RES *res = mysql_store_result(conn);
					MYSQL_ROW row;
					strcpy(respuesta, "11/");
					while ((row = mysql_fetch_row(res)) != NULL) {
						strcat(respuesta, "Partida ");
						strcat(respuesta, row[0]);  // ID
						strcat(respuesta, ": Ganador ");
						strcat(respuesta, row[1]);  // Winner
						strcat(respuesta, " en ");
						strcat(respuesta, row[2]);  // Fecha
						strcat(respuesta, "\n");
					}
				} else {
					sprintf(respuesta, "5/Error al realizar la consulta\n");
				}
				write(sock_conn, respuesta, strlen(respuesta));
				break;
            }
           case 13: { // Movimiento del jugador
				pthread_mutex_lock(&mtx_game_state);

				// Actualizar la posición del jugador
				if (player == 1) {
					game_state.player1_position += 10;
					printf("Jugador 1 se movia a la posicion %d\n", game_state.player1_position);
				} else if (player == 2) {
					game_state.player2_position += 10;
					printf("Jugador 2 se movia a la posicion %d\n", game_state.player2_position);
				}

				// Generar respuesta para el jugador
				char move_msg[64];
				snprintf(move_msg, sizeof(move_msg), "10/MOVE/%d\n", player);
				printf(move_msg);

				// Enviar respuesta al jugador correspondiente
				if (game_state.player1_socket != 0) {
					write(game_state.player1_socket, move_msg, strlen(move_msg));
				}
				if (game_state.player2_socket != 0) {
					write(game_state.player2_socket, move_msg, strlen(move_msg));
				}

				// Comprobar si el jugador cruzó la línea de meta
				if ((player == 1 && game_state.player1_position >= game_state.finish_line) ||
					(player == 2 && game_state.player2_position >= game_state.finish_line)) {
					printf("Jugador %d ha cruzado la línea de meta.\n", player);

					char win_msg[64];
					snprintf(win_msg, sizeof(win_msg), "11/WIN/%d\n", player);

					// Notificar a ambos jugadores
					if (game_state.player1_socket != 0) {
						write(game_state.player1_socket, win_msg, strlen(win_msg));
					}
					if (game_state.player2_socket != 0) {
						write(game_state.player2_socket, win_msg, strlen(win_msg));
					}

					// Reiniciar el juego (opcional)
					game_state.player1_position = 0;
					game_state.player2_position = 0;
				}

				pthread_mutex_unlock(&mtx_game_state);
				break;
				}
				case 14: { // Obtener la lista de jugadores con los que ha jugado el jugador especificado
				p = strtok(NULL, "/");
				if (p == NULL) {
					sprintf(respuesta, "14/Nombre no proporcionado\n");
					write(sock_conn, respuesta, strlen(respuesta));
					break;
				}
				strncpy(nombre, p, sizeof(nombre) - 1);
				nombre[sizeof(nombre) - 1] = '\0';
				sprintf(query, 
						"SELECT DISTINCT p2.Player "
						"FROM Participation p1 "
						"JOIN Participation p2 ON p1.Game = p2.Game "
						"WHERE p1.Player = '%s' AND p2.Player != '%s';",
						nombre, nombre);
				if (mysql_query(conn, query) == 0) {
					MYSQL_RES *res = mysql_store_result(conn);
					MYSQL_ROW row;
					strcpy(respuesta, "14/");
					while ((row = mysql_fetch_row(res)) != NULL) {
						strcat(respuesta, row[0]);
						strcat(respuesta, ",");
					}
					respuesta[strlen(respuesta) - 1] = '\n';  // Quitar la última coma y añadir salto de línea
				} else {
					sprintf(respuesta, "14/Error al realizar la consulta\n");
				}
				write(sock_conn, respuesta, strlen(respuesta));
				break;
                
            }


            default:
                snprintf(respuesta, sizeof(respuesta), "16/Codigo no reconocido", mysql_error(conn));
                write(sock_conn, respuesta, strlen(respuesta));
                break;
        }
    }

    close(sock_conn);
    return NULL;
}




// Manejo de se￱ales para cerrar adecuadamente el servidor
void signal_handler(int signum) {
    printf("Cerrando servidor...\n");
    mysql_close(conn);
    exit(0);
}
int main(int argc, char *argv[]) {
	int sockfd, newsockfd;
	struct sockaddr_in server_addr, client_addr;
	socklen_t clilen;
	pthread_t thread_id;
	
	// Conectar a la base de datos
	conectarBD();
	
	// Inicialización del estado del juego
    game_state.player1_position = 0;
    game_state.player2_position = 0;
    game_state.finish_line = 625; // Línea de meta
    game_state.player1_socket = 0;
    game_state.player2_socket = 0;
	
	// Crear socket
	sockfd = socket(AF_INET, SOCK_STREAM, 0);
	if (sockfd < 0) {
		perror("Error al abrir el socket");
		exit(1);
	}
	
	
	memset((char *)&server_addr, 0, sizeof(server_addr));
	server_addr.sin_family = AF_INET;
	server_addr.sin_addr.s_addr = INADDR_ANY;
	server_addr.sin_port = htons(PORT);
	
	// Enlazar el socket a una direcci￯﾿ﾃ￯ﾾﾳn
	if (bind(sockfd, (struct sockaddr *)&server_addr, sizeof(server_addr)) < 0) {
		perror("Error en el bind");
		close(sockfd);
		exit(1);
	}
	
	// Poner el servidor en modo escucha (hasta 5 conexiones en espera)
	if (listen(sockfd, 5) < 0) {
		perror("Error al poner el servidor a escuchar");
		close(sockfd);
		exit(1);
	}
	
	printf("Servidor escuchando en el puerto %d...\n", PORT);
	
	// Aceptar conexiones entrantes
	clilen = sizeof(client_addr);
	
	while (1) {
		// Aceptar una nueva conexi￯﾿ﾃ￯ﾾﾳn
		newsockfd = accept(sockfd, (struct sockaddr *)&client_addr, &clilen);
		if (newsockfd < 0) {
			perror("Error en la aceptacion de la conexion");
			continue; // Si hay un error en la aceptaci￯﾿ﾃ￯ﾾﾳn, continuar escuchando
		}
		
		// Crear un hilo para manejar la conexi￯﾿ﾃ￯ﾾﾳn del cliente
		if (pthread_create(&thread_id, NULL, AtenderJugador, (void *)&newsockfd) != 0) {
			perror("Error al crear hilo para el cliente");
			continue; // Si hay un error al crear el hilo, continuar aceptando conexiones
		}
		
		// Detener el hilo para que no se bloquee
		pthread_detach(thread_id);  // Esto permite que el hilo se limpie autom￯﾿ﾃ￯ﾾﾡticamente al finalizar
	}
	
	// Cerrar el socket del servidor (esto no se alcanza nunca en este flujo de trabajo)
	close(sockfd);
	mysql_close(conn);  // Cerrar la conexi￯﾿ﾃ￯ﾾﾳn a la base de datos cuando se termine
	return 0;
}


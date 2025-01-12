using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class InicioSesion : Form
    {
        private Socket server;
        private readonly List<string> jugadoresConectados = new List<string>();
        private const int PUERTO_SERVIDOR = 50050;
            
        private const string IP_SERVIDOR = "10.4.119.5";
        public InicioSesion()
        {
            InitializeComponent();
            ConfigurarUI();
            ConectarServidor();
           

        }
        private void ConfigurarUI()
        {
            panelSalaEspera.Visible = false; // Panel que contiene elementos de Sala de Espera
            btnDesconectar.Enabled = false;
            buttonPartidas.Enabled = false;
            buttonResultados.Enabled = false;
            buttonAdversarios.Enabled = false;
        }


        private void ConectarServidor()
        {
            IPAddress direccionIP = IPAddress.Parse(IP_SERVIDOR);
            IPEndPoint puntoConexion = new IPEndPoint(direccionIP, PUERTO_SERVIDOR);

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(puntoConexion);
                this.BackColor = Color.Green;
                MessageBox.Show("Conectado al servidor.");
                Task.Run(AtenderServidor);
            }
            catch (SocketException)
            {
                MessageBox.Show("No se pudo conectar al servidor.");
                this.BackColor = Color.Red;
            }
        }

        private async Task AtenderServidor()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[500];
                    int bytesRecibidos = await server.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

                    if (bytesRecibidos == 0)
                    {
                        MessageBox.Show("Conexión cerrada por el servidor.");
                        break;
                    }

                    string mensaje = Encoding.ASCII.GetString(buffer, 0, bytesRecibidos).Trim();

                    ProcesarMensajeServidor(mensaje);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en la conexión con el servidor: " + ex.Message);
            }
        }

        private void ProcesarMensajeServidor(string mensaje)
        {
            string[] partesMensaje = mensaje.Split('/');
            if (partesMensaje.Length == 0 || !int.TryParse(partesMensaje[0], out int codigo))
            {
                MessageBox.Show("Mensaje inválido recibido del servidor.");
                return;
            }

            switch (codigo)
            {
                case 0:
                    ProcesarCerrarSesion(partesMensaje);
                    break;
                case 1: 
                    ProcesarRegistro(partesMensaje);
                    break;
                case 2:
                    ProcesarInicioSesion(partesMensaje);
                    
                    break;
                case 4:
                    ActualizarListaJugadores(partesMensaje);
                    break;
                case 7:
                    ProcesarInvitacion(partesMensaje);
                    break;
                case 8:
                    MostrarMensajeChat(partesMensaje);
                    break;
                case 9:
                    AbrirCarRaceGame();
                    break;
                case 10:
                    if (partesMensaje.Length > 1)
                    {
                        string resultados = partesMensaje[1]; // La lista de resultados recibidos
                        MessageBox.Show($"Resultados: {resultados}");
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron resultados para este par de jugadores.");
                    }
                    break;
                case 11:
                    if (partesMensaje.Length > 1)
                    {
                        string partidas = partesMensaje[1];  // Las partidas recibidas
                        MessageBox.Show($"Partidas: {partidas}");
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron partidas en ese rango de fechas.");
                    }
                    break;
                case 14:
                    if (partesMensaje.Length > 1)
                    {
                        string listaAdversarios = partesMensaje[1];  // La lista de adversarios recibida
                        MessageBox.Show($"Adversarios: {listaAdversarios}");
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron adversarios para este jugador.");
                    }

                    break;
           
                case 16:
                    MessageBox.Show(mensaje);
                    break;
                default:
                    MessageBox.Show("Código no reconocido: " + codigo);
                    break;
            }
        }
        private void AbrirCarRaceGame()
        {
            CarRaceGame carRaceGame = new CarRaceGame(server);
            carRaceGame.ShowDialog();
        }
        private void ProcesarRegistro(string[] partesMensaje)
        {
            if (partesMensaje.Length >= 2) // Validar que haya al menos dos partes en la respuesta
            {
                string estado = partesMensaje[0]; // Código de respuesta (debería ser "1" para registro)
                string mensaje = partesMensaje[1]; // Mensaje enviado por el servidor

                // Mostrar el mensaje según el contenido recibido
                if (estado == "1")
                {
                    MessageBox.Show($"Respuesta del servidor: {mensaje}");
                }
                else
                {
                    MessageBox.Show("Código de respuesta inesperado: " + estado);
                }
            }
            else
            {
                MessageBox.Show("Respuesta del servidor no válida.");
            }
        }



        private void ProcesarInicioSesion(string[] partesMensaje)
        {
            if (partesMensaje.Length >= 2 && partesMensaje[1] == "OK")
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show("Inicio de sesión exitoso.");
                    panelnicioSesion.Visible = false; // Oculta los controles de inicio de sesión
                    panelSalaEspera.Visible = true;    // Muestra la Sala de Espera
                    btnDesconectar.Enabled = true;
                    buttonAdversarios.Enabled = true;
                    buttonResultados.Enabled = true;
                    buttonPartidas.Enabled = true;
                }));
            }
            else
            {
                MessageBox.Show("Inicio de sesión fallido.");
                MessageBox.Show($"Respuesta del servidor: {partesMensaje}");
            }
        }
        private void ProcesarCerrarSesion(string[] partesMensaje)
        {
            if (partesMensaje.Length >= 2) // Validar que haya al menos dos partes en la respuesta
            {
                string estado = partesMensaje[0]; // Código de respuesta (debería ser "0" para registro)
                string mensaje = partesMensaje[1]; // Mensaje enviado por el servidor

                // Mostrar el mensaje según el contenido recibido
                if (estado == "0")
                {
                    MessageBox.Show($"Respuesta del servidor: {mensaje}");
                }
                else
                {
                    MessageBox.Show("Código de respuesta inesperado: " + estado);
                }
            }
            else
            {
                MessageBox.Show("Respuesta del servidor no válida.");
            }
        }

        private void ActualizarListaJugadores(string[] partesMensaje)
        {
            if (partesMensaje.Length < 2) return;

            jugadoresConectados.Clear();
            jugadoresConectados.AddRange(partesMensaje[1].Split(' ', (char)StringSplitOptions.RemoveEmptyEntries));

            this.Invoke(new Action(() =>
            {
                listJugadoresConectados.Items.Clear();
                listJugadoresConectados.Items.AddRange(jugadoresConectados.ToArray());
            }));
        }

        private void ProcesarInvitacion(string[] partesMensaje)
        {
            if (partesMensaje.Length >= 3 && partesMensaje[1] == "RECIBIR")
            {
                string jugadorRival = partesMensaje[2];
                DialogResult resultado = MessageBox.Show(
                    $"Has recibido una invitación de {jugadorRival}. ¿Aceptar?",
                    "Invitación recibida",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                string respuesta = resultado == DialogResult.Yes ? $"7/ACEPTAR/{jugadorRival}" : $"7/RECHAZAR/{jugadorRival}";
                _ = EnviarMensaje(respuesta);
            }
            else if (partesMensaje[1] == "RECHAZADO")
            {
                string rechazador = partesMensaje[2];
                MessageBox.Show($"{rechazador} ha rechazado tu invitación.");
            }
            else if (partesMensaje[1] == "ACEPTADO")
            {
                string aceptador = partesMensaje[2];
                MessageBox.Show($"{aceptador} ha aceptado tu invitación. Iniciando partida...");
                // Aquí puedes mostrar la interfaz del juego
            }
        }


        private void MostrarMensajeChat(string[] partesMensaje)
        {
            // Validar que el mensaje tenga al menos 3 partes (código, remitente, mensaje)
            if (partesMensaje == null || partesMensaje.Length < 3)
            {
                MessageBox.Show("El mensaje recibido es inválido.");
                return;
            }

            // Extraer el remitente y el mensaje
            string remitente = partesMensaje[1].Trim();
            string mensaje = partesMensaje[2].Trim();

            // Validar que no estén vacíos
            if (string.IsNullOrEmpty(remitente) || string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show("El mensaje o remitente recibido está vacío.");
                return;
            }
            listMensajesChat.Invoke(new Action(() =>
            {
                // Mostrar el mensaje en la lista de chat
                listMensajesChat.Items.Add($"{remitente}: {mensaje}");
            }));
            
        }


        private async Task EnviarMensaje(string mensaje)
        {
            try
            {
                if (server != null && server.Connected)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(mensaje);
                    await Task.Run(() => server.Send(buffer));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el mensaje: {ex.Message}");
            }
        }

        private async void buttonEnviarmsg_Click(object sender, EventArgs e)
        {
            // Obtener y validar el mensaje a enviar
            string mensaje = txtMensajeChat.Text.Trim();
            if (string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show("El mensaje no puede estar vacío.");
                return;
            }

            // Verificar que haya un jugador seleccionado en la lista
            if (listJugadoresConectados.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un jugador para enviar el mensaje.");
                return;
            }

            // Obtener el nombre del jugador seleccionado
            string jugadorSeleccionado = listJugadoresConectados.SelectedItem.ToString();

            // Construir el mensaje con el formato adecuado para el protocolo
            string mensajeFormato = $"8/{jugadorSeleccionado}/{mensaje}";

            // Enviar el mensaje al servidor
            try
            {
                await EnviarMensaje(mensajeFormato);

                // Mostrar confirmación al usuario
                listMensajesChat.Items.Add($"Yo -> {jugadorSeleccionado}: {mensaje}");
                MessageBox.Show($"Mensaje enviado a {jugadorSeleccionado}.");
                //MessageBox.Show(mensaje);

                // Limpiar el cuadro de texto del mensaje
                txtMensajeChat.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el mensaje: {ex.Message}");
            }
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            mensaje = "0/"; // Código 0 para desconexión
            Task enviarMensajeTask = EnviarMensaje(mensaje);

            try
            {
                if (server != null && server.Connected)
                {
                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                }

                panelSalaEspera.Visible = false;
                panelnicioSesion.Visible = true;
                btnDesconectar.Enabled = false;
                this.BackColor = Color.Gray;

                MessageBox.Show("Desconectado del servidor.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al desconectar: {ex.Message}");
            }
        }

        private void InicioSesion_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnDesconectar_Click(sender, e);
        }

        private async void buttonEnviar_Click(object sender, EventArgs e)
        {
            if (server == null || !server.Connected)
            {
                MessageBox.Show("No hay conexión con el servidor.");
                return;
            }

            string mensaje = string.Empty;

            if (Registrar.Checked)
            {
                mensaje = $"1/{Usuario.Text}/{Contraseña.Text}"; // Código 1 para registro
            }
            else if (IniciarSesion.Checked)
            {
                mensaje = $"2/{Usuario.Text}/{Contraseña.Text}"; // Código 2 para inicio de sesión
                usuariolbl.Text = Usuario.Text;
            }
          

            if (!string.IsNullOrEmpty(mensaje))
            {
                await EnviarMensaje(mensaje);
            }
        }

        private void buttonConectar_Click(object sender, EventArgs e)
        {
            ConectarServidor();
        }

        private void buttonBaja_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            mensaje = $"3/{Usuario.Text}/{Contraseña.Text}"; // Código 3 para darse de baja
            Task enviarMensajeTask = EnviarMensaje(mensaje);
            btnDesconectar_Click(sender, e);

        }

        private async void button_Invitar_Click(object sender, EventArgs e)
        {
            if (listJugadoresConectados.SelectedItem == null)
            {
                MessageBox.Show("Selecciona un jugador para invitar.");
                return;
            }

            string jugadorSeleccionado = listJugadoresConectados.SelectedItem.ToString();
            string mensaje = $"7/ENVIAR/{jugadorSeleccionado}"; // Código 7 para enviar invitación

            await EnviarMensaje(mensaje);
            MessageBox.Show($"Has enviado una invitación a {jugadorSeleccionado}.");
        }

        private async void buttonAdversarios_Click(object sender, EventArgs e)
        {

            // Obtener y validar el mensaje a enviar
            string mensaje = textBox.Text.Trim();
            if (string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show("El mensaje no puede estar vacío.");
                return;
            }

           

            // Construir el mensaje con el formato adecuado para el protocolo
            string mensajeFormato = $"14/{mensaje}";

            // Enviar el mensaje al servidor
            try
            {
                await EnviarMensaje(mensajeFormato);
                //MessageBox.Show(mensajeFormato);
                // Limpiar el cuadro de texto del mensaje
                txtMensajeChat.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el mensaje: {ex.Message}");
            }
        }

        private async void buttonResultados_Click(object sender, EventArgs e)
        {
            // Obtener y validar el mensaje a enviar
            string mensaje = textBox.Text.Trim();
            if (string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show("El mensaje no puede estar vacío.");
                return;
            }



            // Construir el mensaje con el formato adecuado para el protocolo
            string mensajeFormato = $"10/{mensaje}";

            // Enviar el mensaje al servidor
            try
            {
                await EnviarMensaje(mensajeFormato);
                //MessageBox.Show(mensajeFormato);
                // Limpiar el cuadro de texto del mensaje
                txtMensajeChat.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el mensaje: {ex.Message}");
            }
        }

        private async void buttonPartidas_Click(object sender, EventArgs e)
        {
            // Obtener y validar el mensaje a enviar
            string mensaje = textBox.Text.Trim();
            if (string.IsNullOrEmpty(mensaje))
            {
                MessageBox.Show("El mensaje no puede estar vacío.");
                return;
            }



            // Construir el mensaje con el formato adecuado para el protocolo
            string mensajeFormato = $"11/{mensaje}";

            // Enviar el mensaje al servidor
            try
            {
                await EnviarMensaje(mensajeFormato);
                // Limpiar el cuadro de texto del mensaje
                //MessageBox.Show(mensajeFormato);
                txtMensajeChat.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el mensaje: {ex.Message}");
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class CarRaceGame : Form
    {
        
        
        private Socket server;
        public CarRaceGame(Socket clienteSocket)
        {
            InitializeComponent();
            this.server = clienteSocket; // Reutiliza el socket existente
        }
        private void btnMove_Click(object sender, EventArgs e)
        {
            EnviarMensaje("13/MOVE");
        }



        private async void EnviarMensaje(string mensaje)
        {
            try
            {
                if (server != null && server.Connected)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(mensaje);
                    await Task.Run(() => server.Send(buffer));
                    //MessageBox.Show(mensaje);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el mensaje: {ex.Message}");
            }
        }

        private void ListenToServer()
        {
            while (true)
            {
                try
                {
                    // Leer el mensaje del servidor
                    byte[] data = new byte[256];
                    int bytes = server.Receive(data);
                    string response = Encoding.ASCII.GetString(data, 0, bytes);

                    // Verifica el mensaje recibido
                    //MessageBox.Show("Mensaje recibido del servidor: " + response);
                    // Procesar el mensaje recibido
                    ProcesarMensajeServidor(response);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al leer del servidor: " + ex.Message);
                    break;
                }
            }
        }

        private void ProcesarMensajeServidor(string mensaje)
        {
            // Elimina caracteres de salto de línea o espacios al final del mensaje
            mensaje = mensaje.Trim();

            // Dividir el mensaje en partes
            string[] partesMensaje = mensaje.Split('/');
            if (partesMensaje.Length == 0 || !int.TryParse(partesMensaje[0], out int codigo))
            {
                MessageBox.Show("Mensaje inválido recibido del servidor: " + mensaje);
                return;
            }

            // Verificar si es un mensaje de movimiento o de ganador
            switch (codigo)
            {
                case 10:
                    ProcesarMovimiento(partesMensaje);
                    break;
                case 11:
                    ProcesarGanador(partesMensaje);
                    break;
                case 16:
                    //MessageBox.Show(mensaje);
                    break;
                default:
                    MessageBox.Show("Código no reconocido: " + codigo);
                    break;
            }
        }


        private void ProcesarMovimiento(string[] partesMensaje)
        {
            // El formato del mensaje es "MOVE/JugadorId"
            if (partesMensaje.Length >= 2)
            {
                int playerId = int.Parse(partesMensaje[2]);

                // Actualizar la posición del coche en el hilo principal
                Invoke((MethodInvoker)delegate
                {
                    if (playerId == 1)
                    {
                        car1.Left += 10; // Mueve el coche del Jugador 1
                    }
                    else if (playerId == 2)
                    {
                        car2.Left += 10; // Mueve el coche del Jugador 2
                    }
                });
            }
        }

        private void ProcesarGanador(string[] partesMensaje)
        {
            // El formato del mensaje es "WINNER/JugadorId"
            if (partesMensaje.Length >= 2)
            {
                int winnerId = int.Parse(partesMensaje[2]);

                // Mostrar quién ganó
                Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show($"¡Jugador {winnerId} gana!");
                    ResetGame();
                });
            }
        }

       


        // Resetea la posición de los coches
        private void ResetGame()
        {
            // Reinicia las posiciones de los autos
            car1.Left = 10;
            car2.Left = 10;

            // Mensaje opcional al usuario
            MessageBox.Show("¡El juego está listo! Pulsa las teclas para mover tu auto.");
        }

        private void CarRaceGame_Load(object sender, EventArgs e)
        {
            Task.Run(ListenToServer);

        }

        


        private void CarRaceGame_FormClosing(object sender, FormClosingEventArgs e)
        {
             server.Close();    
        }
    }
}

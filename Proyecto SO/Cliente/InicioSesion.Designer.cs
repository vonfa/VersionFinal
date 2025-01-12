namespace Cliente
{
    partial class InicioSesion
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonEnviarmsg = new System.Windows.Forms.Button();
            this.Registrar = new System.Windows.Forms.RadioButton();
            this.IniciarSesion = new System.Windows.Forms.RadioButton();
            this.Contraseña = new System.Windows.Forms.TextBox();
            this.Usuario = new System.Windows.Forms.TextBox();
            this.listMensajesChat = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMensajeChat = new System.Windows.Forms.TextBox();
            this.button_Invitar = new System.Windows.Forms.Button();
            this.btnDesconectar = new System.Windows.Forms.Button();
            this.panelnicioSesion = new System.Windows.Forms.Panel();
            this.buttonConectar = new System.Windows.Forms.Button();
            this.buttonEnviar = new System.Windows.Forms.Button();
            this.panelSalaEspera = new System.Windows.Forms.Panel();
            this.buttonPartidas = new System.Windows.Forms.Button();
            this.buttonResultados = new System.Windows.Forms.Button();
            this.buttonAdversarios = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.usuariolbl = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonBaja = new System.Windows.Forms.Button();
            this.listJugadoresConectados = new System.Windows.Forms.ListBox();
            this.textBox = new System.Windows.Forms.TextBox();
            this.panelnicioSesion.SuspendLayout();
            this.panelSalaEspera.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "Usuario";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Contraseña";
            // 
            // buttonEnviarmsg
            // 
            this.buttonEnviarmsg.BackColor = System.Drawing.SystemColors.Window;
            this.buttonEnviarmsg.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonEnviarmsg.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEnviarmsg.Location = new System.Drawing.Point(21, 280);
            this.buttonEnviarmsg.Name = "buttonEnviarmsg";
            this.buttonEnviarmsg.Size = new System.Drawing.Size(203, 37);
            this.buttonEnviarmsg.TabIndex = 16;
            this.buttonEnviarmsg.Text = "Enviar mensaje";
            this.buttonEnviarmsg.UseVisualStyleBackColor = false;
            this.buttonEnviarmsg.Click += new System.EventHandler(this.buttonEnviarmsg_Click);
            // 
            // Registrar
            // 
            this.Registrar.AutoSize = true;
            this.Registrar.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Registrar.Location = new System.Drawing.Point(15, 211);
            this.Registrar.Name = "Registrar";
            this.Registrar.Size = new System.Drawing.Size(109, 27);
            this.Registrar.TabIndex = 15;
            this.Registrar.TabStop = true;
            this.Registrar.Text = "Registarse";
            this.Registrar.UseVisualStyleBackColor = true;
            // 
            // IniciarSesion
            // 
            this.IniciarSesion.AutoSize = true;
            this.IniciarSesion.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IniciarSesion.Location = new System.Drawing.Point(15, 173);
            this.IniciarSesion.Name = "IniciarSesion";
            this.IniciarSesion.Size = new System.Drawing.Size(130, 27);
            this.IniciarSesion.TabIndex = 13;
            this.IniciarSesion.TabStop = true;
            this.IniciarSesion.Text = "Iniciar Sesión";
            this.IniciarSesion.UseVisualStyleBackColor = true;
            // 
            // Contraseña
            // 
            this.Contraseña.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Contraseña.Location = new System.Drawing.Point(115, 102);
            this.Contraseña.Name = "Contraseña";
            this.Contraseña.Size = new System.Drawing.Size(172, 26);
            this.Contraseña.TabIndex = 19;
            // 
            // Usuario
            // 
            this.Usuario.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Usuario.Location = new System.Drawing.Point(115, 56);
            this.Usuario.Name = "Usuario";
            this.Usuario.Size = new System.Drawing.Size(172, 26);
            this.Usuario.TabIndex = 18;
            // 
            // listMensajesChat
            // 
            this.listMensajesChat.FormattingEnabled = true;
            this.listMensajesChat.Location = new System.Drawing.Point(21, 34);
            this.listMensajesChat.Name = "listMensajesChat";
            this.listMensajesChat.Size = new System.Drawing.Size(298, 173);
            this.listMensajesChat.TabIndex = 38;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(431, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 23);
            this.label4.TabIndex = 37;
            this.label4.Text = "Lista de Conectados";
            // 
            // txtMensajeChat
            // 
            this.txtMensajeChat.Location = new System.Drawing.Point(94, 225);
            this.txtMensajeChat.Margin = new System.Windows.Forms.Padding(2);
            this.txtMensajeChat.Name = "txtMensajeChat";
            this.txtMensajeChat.Size = new System.Drawing.Size(225, 20);
            this.txtMensajeChat.TabIndex = 35;
            // 
            // button_Invitar
            // 
            this.button_Invitar.BackColor = System.Drawing.SystemColors.Window;
            this.button_Invitar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_Invitar.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Invitar.Location = new System.Drawing.Point(433, 268);
            this.button_Invitar.Name = "button_Invitar";
            this.button_Invitar.Size = new System.Drawing.Size(164, 29);
            this.button_Invitar.TabIndex = 34;
            this.button_Invitar.Text = "Invitar";
            this.button_Invitar.UseVisualStyleBackColor = false;
            this.button_Invitar.Click += new System.EventHandler(this.button_Invitar_Click);
            // 
            // btnDesconectar
            // 
            this.btnDesconectar.BackColor = System.Drawing.Color.Red;
            this.btnDesconectar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDesconectar.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDesconectar.Location = new System.Drawing.Point(239, 280);
            this.btnDesconectar.Name = "btnDesconectar";
            this.btnDesconectar.Size = new System.Drawing.Size(135, 37);
            this.btnDesconectar.TabIndex = 39;
            this.btnDesconectar.Text = "Desconectar";
            this.btnDesconectar.UseVisualStyleBackColor = false;
            this.btnDesconectar.Click += new System.EventHandler(this.btnDesconectar_Click);
            // 
            // panelnicioSesion
            // 
            this.panelnicioSesion.BackColor = System.Drawing.Color.Gray;
            this.panelnicioSesion.Controls.Add(this.buttonConectar);
            this.panelnicioSesion.Controls.Add(this.buttonEnviar);
            this.panelnicioSesion.Controls.Add(this.label2);
            this.panelnicioSesion.Controls.Add(this.Contraseña);
            this.panelnicioSesion.Controls.Add(this.label1);
            this.panelnicioSesion.Controls.Add(this.Usuario);
            this.panelnicioSesion.Controls.Add(this.IniciarSesion);
            this.panelnicioSesion.Controls.Add(this.Registrar);
            this.panelnicioSesion.Location = new System.Drawing.Point(12, 12);
            this.panelnicioSesion.Name = "panelnicioSesion";
            this.panelnicioSesion.Size = new System.Drawing.Size(341, 310);
            this.panelnicioSesion.TabIndex = 40;
            // 
            // buttonConectar
            // 
            this.buttonConectar.BackColor = System.Drawing.Color.PaleGreen;
            this.buttonConectar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonConectar.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConectar.Location = new System.Drawing.Point(0, -3);
            this.buttonConectar.Name = "buttonConectar";
            this.buttonConectar.Size = new System.Drawing.Size(218, 37);
            this.buttonConectar.TabIndex = 41;
            this.buttonConectar.Text = "Conectarse al servidor";
            this.buttonConectar.UseVisualStyleBackColor = false;
            this.buttonConectar.Click += new System.EventHandler(this.buttonConectar_Click);
            // 
            // buttonEnviar
            // 
            this.buttonEnviar.BackColor = System.Drawing.SystemColors.Window;
            this.buttonEnviar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonEnviar.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEnviar.Location = new System.Drawing.Point(185, 168);
            this.buttonEnviar.Name = "buttonEnviar";
            this.buttonEnviar.Size = new System.Drawing.Size(84, 37);
            this.buttonEnviar.TabIndex = 40;
            this.buttonEnviar.Text = "Enviar";
            this.buttonEnviar.UseVisualStyleBackColor = false;
            this.buttonEnviar.Click += new System.EventHandler(this.buttonEnviar_Click);
            // 
            // panelSalaEspera
            // 
            this.panelSalaEspera.BackColor = System.Drawing.Color.DimGray;
            this.panelSalaEspera.Controls.Add(this.textBox);
            this.panelSalaEspera.Controls.Add(this.buttonPartidas);
            this.panelSalaEspera.Controls.Add(this.buttonResultados);
            this.panelSalaEspera.Controls.Add(this.buttonAdversarios);
            this.panelSalaEspera.Controls.Add(this.label6);
            this.panelSalaEspera.Controls.Add(this.usuariolbl);
            this.panelSalaEspera.Controls.Add(this.label5);
            this.panelSalaEspera.Controls.Add(this.label3);
            this.panelSalaEspera.Controls.Add(this.buttonBaja);
            this.panelSalaEspera.Controls.Add(this.listJugadoresConectados);
            this.panelSalaEspera.Controls.Add(this.txtMensajeChat);
            this.panelSalaEspera.Controls.Add(this.btnDesconectar);
            this.panelSalaEspera.Controls.Add(this.button_Invitar);
            this.panelSalaEspera.Controls.Add(this.label4);
            this.panelSalaEspera.Controls.Add(this.listMensajesChat);
            this.panelSalaEspera.Controls.Add(this.buttonEnviarmsg);
            this.panelSalaEspera.Location = new System.Drawing.Point(359, 12);
            this.panelSalaEspera.Name = "panelSalaEspera";
            this.panelSalaEspera.Size = new System.Drawing.Size(626, 489);
            this.panelSalaEspera.TabIndex = 41;
            // 
            // buttonPartidas
            // 
            this.buttonPartidas.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPartidas.Location = new System.Drawing.Point(302, 434);
            this.buttonPartidas.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPartidas.Name = "buttonPartidas";
            this.buttonPartidas.Size = new System.Drawing.Size(72, 29);
            this.buttonPartidas.TabIndex = 49;
            this.buttonPartidas.Text = "Partidas  ";
            this.buttonPartidas.UseVisualStyleBackColor = true;
            this.buttonPartidas.Click += new System.EventHandler(this.buttonPartidas_Click);
            // 
            // buttonResultados
            // 
            this.buttonResultados.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonResultados.Location = new System.Drawing.Point(166, 433);
            this.buttonResultados.Margin = new System.Windows.Forms.Padding(2);
            this.buttonResultados.Name = "buttonResultados";
            this.buttonResultados.Size = new System.Drawing.Size(97, 31);
            this.buttonResultados.TabIndex = 48;
            this.buttonResultados.Text = "Resultados ";
            this.buttonResultados.UseVisualStyleBackColor = true;
            this.buttonResultados.Click += new System.EventHandler(this.buttonResultados_Click);
            // 
            // buttonAdversarios
            // 
            this.buttonAdversarios.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAdversarios.Location = new System.Drawing.Point(34, 433);
            this.buttonAdversarios.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAdversarios.Name = "buttonAdversarios";
            this.buttonAdversarios.Size = new System.Drawing.Size(102, 30);
            this.buttonAdversarios.TabIndex = 47;
            this.buttonAdversarios.Text = "Adversarios";
            this.buttonAdversarios.UseVisualStyleBackColor = true;
            this.buttonAdversarios.Click += new System.EventHandler(this.buttonAdversarios_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(30, 386);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 23);
            this.label6.TabIndex = 44;
            this.label6.Text = "CONSULTAS";
            // 
            // usuariolbl
            // 
            this.usuariolbl.AutoSize = true;
            this.usuariolbl.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usuariolbl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.usuariolbl.Location = new System.Drawing.Point(542, 440);
            this.usuariolbl.Name = "usuariolbl";
            this.usuariolbl.Size = new System.Drawing.Size(68, 23);
            this.usuariolbl.TabIndex = 43;
            this.usuariolbl.Text = "usuario";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(17, 225);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 23);
            this.label5.TabIndex = 42;
            this.label5.Text = "Escribe:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(17, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 23);
            this.label3.TabIndex = 41;
            this.label3.Text = "Chat";
            // 
            // buttonBaja
            // 
            this.buttonBaja.BackColor = System.Drawing.Color.Black;
            this.buttonBaja.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonBaja.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonBaja.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonBaja.Location = new System.Drawing.Point(21, 327);
            this.buttonBaja.Name = "buttonBaja";
            this.buttonBaja.Size = new System.Drawing.Size(203, 37);
            this.buttonBaja.TabIndex = 40;
            this.buttonBaja.Text = "Darse de baja";
            this.buttonBaja.UseVisualStyleBackColor = false;
            this.buttonBaja.Click += new System.EventHandler(this.buttonBaja_Click);
            // 
            // listJugadoresConectados
            // 
            this.listJugadoresConectados.FormattingEnabled = true;
            this.listJugadoresConectados.Location = new System.Drawing.Point(419, 63);
            this.listJugadoresConectados.Name = "listJugadoresConectados";
            this.listJugadoresConectados.Size = new System.Drawing.Size(191, 199);
            this.listJugadoresConectados.TabIndex = 39;
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(166, 389);
            this.textBox.Margin = new System.Windows.Forms.Padding(2);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(225, 20);
            this.textBox.TabIndex = 50;
            // 
            // InicioSesion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(1001, 611);
            this.Controls.Add(this.panelSalaEspera);
            this.Controls.Add(this.panelnicioSesion);
            this.Name = "InicioSesion";
            this.Text = "Form1";
            this.panelnicioSesion.ResumeLayout(false);
            this.panelnicioSesion.PerformLayout();
            this.panelSalaEspera.ResumeLayout(false);
            this.panelSalaEspera.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonEnviarmsg;
        private System.Windows.Forms.RadioButton Registrar;
        private System.Windows.Forms.RadioButton IniciarSesion;
        private System.Windows.Forms.TextBox Contraseña;
        private System.Windows.Forms.TextBox Usuario;
        private System.Windows.Forms.ListBox listMensajesChat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMensajeChat;
        private System.Windows.Forms.Button button_Invitar;
        private System.Windows.Forms.Button btnDesconectar;
        private System.Windows.Forms.Panel panelnicioSesion;
        private System.Windows.Forms.Panel panelSalaEspera;
        private System.Windows.Forms.ListBox listJugadoresConectados;
        private System.Windows.Forms.Button buttonEnviar;
        private System.Windows.Forms.Button buttonConectar;
        private System.Windows.Forms.Button buttonBaja;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label usuariolbl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonPartidas;
        private System.Windows.Forms.Button buttonResultados;
        private System.Windows.Forms.Button buttonAdversarios;
        private System.Windows.Forms.TextBox textBox;
    }
}


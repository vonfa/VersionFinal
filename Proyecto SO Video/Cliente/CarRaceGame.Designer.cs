namespace Cliente
{
    partial class CarRaceGame
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CarRaceGame));
            this.btnMove = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.finishLine = new System.Windows.Forms.PictureBox();
            this.car1 = new System.Windows.Forms.PictureBox();
            this.car2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.finishLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.car1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.car2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMove
            // 
            this.btnMove.Location = new System.Drawing.Point(31, 439);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(115, 45);
            this.btnMove.TabIndex = 6;
            this.btnMove.Text = "CLICK";
            this.btnMove.UseVisualStyleBackColor = true;
            this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1, -6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(645, 504);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // finishLine
            // 
            this.finishLine.Image = ((System.Drawing.Image)(resources.GetObject("finishLine.Image")));
            this.finishLine.Location = new System.Drawing.Point(642, -6);
            this.finishLine.Name = "finishLine";
            this.finishLine.Size = new System.Drawing.Size(163, 504);
            this.finishLine.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.finishLine.TabIndex = 7;
            this.finishLine.TabStop = false;
            // 
            // car1
            // 
            this.car1.Image = ((System.Drawing.Image)(resources.GetObject("car1.Image")));
            this.car1.Location = new System.Drawing.Point(12, 150);
            this.car1.Name = "car1";
            this.car1.Size = new System.Drawing.Size(100, 50);
            this.car1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.car1.TabIndex = 8;
            this.car1.TabStop = false;
            // 
            // car2
            // 
            this.car2.Image = ((System.Drawing.Image)(resources.GetObject("car2.Image")));
            this.car2.Location = new System.Drawing.Point(12, 282);
            this.car2.Name = "car2";
            this.car2.Size = new System.Drawing.Size(100, 50);
            this.car2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.car2.TabIndex = 9;
            this.car2.TabStop = false;
            // 
            // CarRaceGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 496);
            this.Controls.Add(this.car2);
            this.Controls.Add(this.car1);
            this.Controls.Add(this.finishLine);
            this.Controls.Add(this.btnMove);
            this.Controls.Add(this.pictureBox1);
            this.Name = "CarRaceGame";
            this.Text = "CarRaceGame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CarRaceGame_FormClosing);
            this.Load += new System.EventHandler(this.CarRaceGame_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.finishLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.car1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.car2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox finishLine;
        private System.Windows.Forms.PictureBox car1;
        private System.Windows.Forms.PictureBox car2;
    }
}
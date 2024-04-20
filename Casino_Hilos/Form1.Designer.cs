namespace Casino_Hilos
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnPalanca = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(139, 392);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(108, 341);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(295, 392);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(110, 341);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Location = new System.Drawing.Point(446, 392);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(117, 331);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Location = new System.Drawing.Point(604, 392);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(116, 330);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Location = new System.Drawing.Point(760, 392);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(114, 330);
            this.panel5.TabIndex = 4;
            // 
            // btnPalanca
            // 
            this.btnPalanca.BackColor = System.Drawing.Color.Transparent;
            this.btnPalanca.FlatAppearance.BorderSize = 0;
            this.btnPalanca.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPalanca.Location = new System.Drawing.Point(927, 469);
            this.btnPalanca.Name = "btnPalanca";
            this.btnPalanca.Size = new System.Drawing.Size(82, 319);
            this.btnPalanca.TabIndex = 5;
            this.btnPalanca.UseVisualStyleBackColor = false;
            this.btnPalanca.Click += new System.EventHandler(this.btnPalanca_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Casino_Hilos.Properties.Resources.CasinoImg;
            this.ClientSize = new System.Drawing.Size(1021, 992);
            this.Controls.Add(this.btnPalanca);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Slot Machine";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnPalanca;
    }
}


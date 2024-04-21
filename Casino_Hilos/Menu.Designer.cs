namespace Casino_Hilos
{
    partial class Menu
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
            this.BtnJugar = new System.Windows.Forms.Button();
            this.BtnInstrucciones = new System.Windows.Forms.Button();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnJugar
            // 
            this.BtnJugar.BackColor = System.Drawing.Color.Transparent;
            this.BtnJugar.FlatAppearance.BorderSize = 0;
            this.BtnJugar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnJugar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnJugar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnJugar.Location = new System.Drawing.Point(201, 369);
            this.BtnJugar.Name = "BtnJugar";
            this.BtnJugar.Size = new System.Drawing.Size(619, 153);
            this.BtnJugar.TabIndex = 0;
            this.BtnJugar.UseVisualStyleBackColor = false;
            this.BtnJugar.Click += new System.EventHandler(this.BtnJugar_Click);
            // 
            // BtnInstrucciones
            // 
            this.BtnInstrucciones.BackColor = System.Drawing.Color.Transparent;
            this.BtnInstrucciones.FlatAppearance.BorderSize = 0;
            this.BtnInstrucciones.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnInstrucciones.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnInstrucciones.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnInstrucciones.Location = new System.Drawing.Point(197, 544);
            this.BtnInstrucciones.Name = "BtnInstrucciones";
            this.BtnInstrucciones.Size = new System.Drawing.Size(623, 155);
            this.BtnInstrucciones.TabIndex = 1;
            this.BtnInstrucciones.UseVisualStyleBackColor = false;
            this.BtnInstrucciones.Click += new System.EventHandler(this.BtnInstrucciones_Click);
            // 
            // BtnSalir
            // 
            this.BtnSalir.BackColor = System.Drawing.Color.Transparent;
            this.BtnSalir.FlatAppearance.BorderSize = 0;
            this.BtnSalir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnSalir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSalir.Location = new System.Drawing.Point(211, 732);
            this.BtnSalir.Name = "BtnSalir";
            this.BtnSalir.Size = new System.Drawing.Size(609, 157);
            this.BtnSalir.TabIndex = 2;
            this.BtnSalir.UseVisualStyleBackColor = false;
            this.BtnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Casino_Hilos.Properties.Resources.Menu_Principal;
            this.ClientSize = new System.Drawing.Size(1021, 992);
            this.ControlBox = false;
            this.Controls.Add(this.BtnSalir);
            this.Controls.Add(this.BtnInstrucciones);
            this.Controls.Add(this.BtnJugar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnJugar;
        private System.Windows.Forms.Button BtnInstrucciones;
        private System.Windows.Forms.Button BtnSalir;
    }
}
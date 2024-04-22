namespace Casino_Hilos
{
    partial class Manual
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
            this.BtnMenu = new System.Windows.Forms.Button();
            this.BtnSalir = new System.Windows.Forms.Button();
            this.panelMover = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // BtnMenu
            // 
            this.BtnMenu.BackColor = System.Drawing.Color.Transparent;
            this.BtnMenu.FlatAppearance.BorderSize = 0;
            this.BtnMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMenu.Location = new System.Drawing.Point(6, 4);
            this.BtnMenu.Name = "BtnMenu";
            this.BtnMenu.Size = new System.Drawing.Size(144, 40);
            this.BtnMenu.TabIndex = 0;
            this.BtnMenu.UseVisualStyleBackColor = false;
            this.BtnMenu.Click += new System.EventHandler(this.BtnMenu_Click);
            // 
            // BtnSalir
            // 
            this.BtnSalir.BackColor = System.Drawing.Color.Transparent;
            this.BtnSalir.FlatAppearance.BorderSize = 0;
            this.BtnSalir.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnSalir.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSalir.Location = new System.Drawing.Point(874, 2);
            this.BtnSalir.Name = "BtnSalir";
            this.BtnSalir.Size = new System.Drawing.Size(146, 41);
            this.BtnSalir.TabIndex = 1;
            this.BtnSalir.UseVisualStyleBackColor = false;
            this.BtnSalir.Click += new System.EventHandler(this.BtnSalir_Click);
            // 
            // panelMover
            // 
            this.panelMover.BackColor = System.Drawing.Color.Transparent;
            this.panelMover.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMover.Location = new System.Drawing.Point(0, 0);
            this.panelMover.Name = "panelMover";
            this.panelMover.Size = new System.Drawing.Size(1021, 55);
            this.panelMover.TabIndex = 2;
            this.panelMover.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMover_Paint);
            this.panelMover.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelMover_MouseDown);
            // 
            // Manual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Casino_Hilos.Properties.Resources.Tabla_de_precios;
            this.ClientSize = new System.Drawing.Size(1021, 992);
            this.ControlBox = false;
            this.Controls.Add(this.BtnSalir);
            this.Controls.Add(this.BtnMenu);
            this.Controls.Add(this.panelMover);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Manual";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manual";
            this.Load += new System.EventHandler(this.Manual_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnMenu;
        private System.Windows.Forms.Button BtnSalir;
        private System.Windows.Forms.Panel panelMover;
    }
}
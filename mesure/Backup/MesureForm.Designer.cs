namespace mesure
{
    partial class MesureForm
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
            this.components = new System.ComponentModel.Container();
            this.ButValid = new System.Windows.Forms.Button();
            this.vistaMenu1 = new wyDay.Controls.VistaMenu(this.components);
            this.EdEchantillon = new System.Windows.Forms.TextBox();
            this.ButSav = new System.Windows.Forms.Button();
            this.ButUndo = new System.Windows.Forms.Button();
            this.lblEchantillon = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.vistaMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // ButValid
            // 
            this.ButValid.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ButValid.Location = new System.Drawing.Point(14, 58);
            this.ButValid.Name = "ButValid";
            this.ButValid.Size = new System.Drawing.Size(90, 59);
            this.ButValid.TabIndex = 0;
            this.ButValid.Text = "Valid";
            this.ButValid.UseVisualStyleBackColor = true;
            this.ButValid.Click += new System.EventHandler(this.butValid_Click);
            // 
            // vistaMenu1
            // 
            this.vistaMenu1.ContainerControl = this;
            // 
            // EdEchantillon
            // 
            this.EdEchantillon.Location = new System.Drawing.Point(14, 14);
            this.EdEchantillon.Name = "EdEchantillon";
            this.EdEchantillon.Size = new System.Drawing.Size(100, 20);
            this.EdEchantillon.TabIndex = 2;
            // 
            // ButSav
            // 
            this.ButSav.Location = new System.Drawing.Point(14, 164);
            this.ButSav.Name = "ButSav";
            this.ButSav.Size = new System.Drawing.Size(90, 35);
            this.ButSav.TabIndex = 3;
            this.ButSav.Text = "sauve Resultats";
            this.ButSav.UseVisualStyleBackColor = true;
            this.ButSav.Click += new System.EventHandler(this.SauveRes_Click);
            // 
            // ButUndo
            // 
            this.ButUndo.Location = new System.Drawing.Point(14, 123);
            this.ButUndo.Name = "ButUndo";
            this.ButUndo.Size = new System.Drawing.Size(90, 35);
            this.ButUndo.TabIndex = 4;
            this.ButUndo.Text = "Annule";
            this.ButUndo.UseVisualStyleBackColor = true;
            this.ButUndo.Click += new System.EventHandler(this.delete_Click);
            // 
            // lblEchantillon
            // 
            this.lblEchantillon.AutoSize = true;
            this.lblEchantillon.Location = new System.Drawing.Point(127, 20);
            this.lblEchantillon.Name = "lblEchantillon";
            this.lblEchantillon.Size = new System.Drawing.Size(55, 13);
            this.lblEchantillon.TabIndex = 5;
            this.lblEchantillon.Text = "Paramètre";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 21);
            this.button1.TabIndex = 6;
            this.button1.Text = "Quitter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MesureForm
            // 
            this.AcceptButton = this.ButValid;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 236);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblEchantillon);
            this.Controls.Add(this.ButUndo);
            this.Controls.Add(this.ButSav);
            this.Controls.Add(this.EdEchantillon);
            this.Controls.Add(this.ButValid);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(143, 102);
            this.Name = "MesureForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Mesure";
            this.VisibleChanged += new System.EventHandler(this.MesureForm_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.vistaMenu1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButValid;
        private wyDay.Controls.VistaMenu vistaMenu1;
        private System.Windows.Forms.TextBox EdEchantillon;
        private System.Windows.Forms.Button ButUndo;
        private System.Windows.Forms.Button ButSav;
        private System.Windows.Forms.Label lblEchantillon;
        private System.Windows.Forms.Button button1;
    }
}
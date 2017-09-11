namespace mesure
{
    partial class GesCodForm
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
            this.textMessage = new System.Windows.Forms.TextBox();
            this.textExpli = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textNomUser = new System.Windows.Forms.TextBox();
            this.but_genkey = new System.Windows.Forms.Button();
            this.but_ActLic = new System.Windows.Forms.Button();
            this.butferm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textMessage
            // 
            this.textMessage.Location = new System.Drawing.Point(28, 39);
            this.textMessage.Name = "textMessage";
            this.textMessage.ReadOnly = true;
            this.textMessage.Size = new System.Drawing.Size(361, 20);
            this.textMessage.TabIndex = 0;
            // 
            // textExpli
            // 
            this.textExpli.Location = new System.Drawing.Point(28, 65);
            this.textExpli.Multiline = true;
            this.textExpli.Name = "textExpli";
            this.textExpli.ReadOnly = true;
            this.textExpli.Size = new System.Drawing.Size(361, 130);
            this.textExpli.TabIndex = 1;
            this.textExpli.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 226);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Utilisateur :";
            // 
            // textNomUser
            // 
            this.textNomUser.Location = new System.Drawing.Point(138, 223);
            this.textNomUser.Name = "textNomUser";
            this.textNomUser.Size = new System.Drawing.Size(197, 20);
            this.textNomUser.TabIndex = 3;
            // 
            // but_genkey
            // 
            this.but_genkey.Location = new System.Drawing.Point(65, 280);
            this.but_genkey.Name = "but_genkey";
            this.but_genkey.Size = new System.Drawing.Size(107, 35);
            this.but_genkey.TabIndex = 4;
            this.but_genkey.Text = "Générer une clé";
            this.but_genkey.UseVisualStyleBackColor = true;
            this.but_genkey.Click += new System.EventHandler(this.but_genkey_Click);
            // 
            // but_ActLic
            // 
            this.but_ActLic.Location = new System.Drawing.Point(65, 330);
            this.but_ActLic.Name = "but_ActLic";
            this.but_ActLic.Size = new System.Drawing.Size(107, 35);
            this.but_ActLic.TabIndex = 5;
            this.but_ActLic.Text = "Activer une licence";
            this.but_ActLic.UseVisualStyleBackColor = true;
            this.but_ActLic.Click += new System.EventHandler(this.but_ActLic_Click);
            // 
            // butferm
            // 
            this.butferm.Location = new System.Drawing.Point(228, 382);
            this.butferm.Name = "butferm";
            this.butferm.Size = new System.Drawing.Size(107, 35);
            this.butferm.TabIndex = 6;
            this.butferm.Text = "Fermer";
            this.butferm.UseVisualStyleBackColor = true;
            this.butferm.Click += new System.EventHandler(this.butferm_Click);
            // 
            // GesCodForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 462);
            this.ControlBox = false;
            this.Controls.Add(this.butferm);
            this.Controls.Add(this.but_ActLic);
            this.Controls.Add(this.but_genkey);
            this.Controls.Add(this.textNomUser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textExpli);
            this.Controls.Add(this.textMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GesCodForm";
            this.ShowInTaskbar = false;
            this.Text = "GesCodForm";
            this.Load += new System.EventHandler(this.GesCodForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textMessage;
        private System.Windows.Forms.TextBox textExpli;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textNomUser;
        private System.Windows.Forms.Button but_genkey;
        private System.Windows.Forms.Button but_ActLic;
        private System.Windows.Forms.Button butferm;
    }
}
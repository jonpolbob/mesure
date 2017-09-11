namespace mesure
{
    partial class EnregFichierParam
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
            this.checkAutoSauve = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ChkClearAuto = new System.Windows.Forms.CheckBox();
            this.choixdirectory = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.butOK = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.combosuffixe = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxdirectory = new System.Windows.Forms.TextBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.folderBrowserDial = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkAutoSauve
            // 
            this.checkAutoSauve.AutoSize = true;
            this.checkAutoSauve.Location = new System.Drawing.Point(15, 28);
            this.checkAutoSauve.Name = "checkAutoSauve";
            this.checkAutoSauve.Size = new System.Drawing.Size(143, 17);
            this.checkAutoSauve.TabIndex = 0;
            this.checkAutoSauve.Text = "sauvegarde automatique";
            this.checkAutoSauve.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ChkClearAuto);
            this.groupBox1.Controls.Add(this.checkAutoSauve);
            this.groupBox1.Location = new System.Drawing.Point(18, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(262, 92);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sauve resultats";
            // 
            // ChkClearAuto
            // 
            this.ChkClearAuto.AutoSize = true;
            this.ChkClearAuto.Location = new System.Drawing.Point(15, 60);
            this.ChkClearAuto.Name = "ChkClearAuto";
            this.ChkClearAuto.Size = new System.Drawing.Size(111, 17);
            this.ChkClearAuto.TabIndex = 1;
            this.ChkClearAuto.Text = "Clear automatique";
            this.ChkClearAuto.UseVisualStyleBackColor = true;
            // 
            // choixdirectory
            // 
            this.choixdirectory.Location = new System.Drawing.Point(324, 154);
            this.choixdirectory.Name = "choixdirectory";
            this.choixdirectory.Size = new System.Drawing.Size(38, 19);
            this.choixdirectory.TabIndex = 3;
            this.choixdirectory.Text = "...";
            this.choixdirectory.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Dossier sauvegarde auto";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 252);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "format";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(234, 206);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "index";
            // 
            // butOK
            // 
            this.butOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.butOK.Location = new System.Drawing.Point(285, 291);
            this.butOK.Name = "butOK";
            this.butOK.Size = new System.Drawing.Size(61, 28);
            this.butOK.TabIndex = 11;
            this.butOK.Text = "OK";
            this.butOK.UseVisualStyleBackColor = true;
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Location = new System.Drawing.Point(217, 291);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(62, 27);
            this.butCancel.TabIndex = 12;
            this.butCancel.Text = "Abandon";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(81, 223);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 14;
            // 
            // combosuffixe
            // 
            this.combosuffixe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combosuffixe.FormattingEnabled = true;
            this.combosuffixe.Items.AddRange(new object[] {
            "essai1",
            "essai2"});
            this.combosuffixe.Location = new System.Drawing.Point(81, 198);
            this.combosuffixe.Name = "combosuffixe";
            this.combosuffixe.Size = new System.Drawing.Size(121, 21);
            this.combosuffixe.TabIndex = 15;
            this.combosuffixe.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 205);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "suffixe";
            // 
            // textBoxdirectory
            // 
            this.textBoxdirectory.Location = new System.Drawing.Point(0, 154);
            this.textBoxdirectory.Name = "textBoxdirectory";
            this.textBoxdirectory.Size = new System.Drawing.Size(348, 20);
            this.textBoxdirectory.TabIndex = 18;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(284, 206);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(62, 20);
            this.numericUpDown1.TabIndex = 17;
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Items.AddRange(new object[] {
            "Excel (.xls)",
            "Text (.csv)",
            "Tab (.tsv)"});
            this.listBox1.Location = new System.Drawing.Point(74, 252);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(128, 13);
            this.listBox1.TabIndex = 16;
            // 
            // EnregFichierParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 329);
            this.Controls.Add(this.textBoxdirectory);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.combosuffixe);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butOK);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.choixdirectory);
            this.Controls.Add(this.groupBox1);
            this.Name = "EnregFichierParam";
            this.Text = "parametres enregistrement fichier";
            this.Load += new System.EventHandler(this.EnregFichierParam_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EnregFichierParam_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkAutoSauve;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ChkClearAuto;
        private System.Windows.Forms.Button choixdirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button butOK;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox combosuffixe;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxdirectory;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDial;
    }
}
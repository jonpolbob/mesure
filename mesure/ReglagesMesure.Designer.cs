namespace mesure
{
    partial class ReglagesMesure
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReglagesMesure));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxObj = new System.Windows.Forms.CheckBox();
            this.checkBoxParam = new System.Windows.Forms.CheckBox();
            this.checkClearImmediat = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkAutoClear = new System.Windows.Forms.CheckBox();
            this.chkAutoSauve = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ButCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panelsepar = new System.Windows.Forms.Panel();
            this.radioDefault = new System.Windows.Forms.RadioButton();
            this.radioPoint = new System.Windows.Forms.RadioButton();
            this.radiovirgule = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.UpDnDecimales = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.UpDownIndex = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.combosuffixe = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxdirectory = new System.Windows.Forms.TextBox();
            this.listFormat = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.choixdirectory = new System.Windows.Forms.Button();
            this.folderBrowserDial = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panelsepar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDnDecimales)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxObj);
            this.groupBox1.Controls.Add(this.checkBoxParam);
            this.groupBox1.Controls.Add(this.checkClearImmediat);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 189);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(340, 172);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Colonnes de Résultats";
            // 
            // checkBoxObj
            // 
            this.checkBoxObj.AutoSize = true;
            this.checkBoxObj.Location = new System.Drawing.Point(28, 63);
            this.checkBoxObj.Name = "checkBoxObj";
            this.checkBoxObj.Size = new System.Drawing.Size(102, 17);
            this.checkBoxObj.TabIndex = 1;
            this.checkBoxObj.Text = "Objectif Courant";
            this.checkBoxObj.UseVisualStyleBackColor = true;
            // 
            // checkBoxParam
            // 
            this.checkBoxParam.AutoSize = true;
            this.checkBoxParam.Location = new System.Drawing.Point(28, 28);
            this.checkBoxParam.Name = "checkBoxParam";
            this.checkBoxParam.Size = new System.Drawing.Size(134, 17);
            this.checkBoxParam.TabIndex = 0;
            this.checkBoxParam.Text = "Parametres Echantillon";
            this.checkBoxParam.UseVisualStyleBackColor = true;
            // 
            // checkClearImmediat
            // 
            this.checkClearImmediat.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkClearImmediat.AutoSize = true;
            this.checkClearImmediat.Location = new System.Drawing.Point(82, 132);
            this.checkClearImmediat.Name = "checkClearImmediat";
            this.checkClearImmediat.Size = new System.Drawing.Size(109, 23);
            this.checkClearImmediat.TabIndex = 2;
            this.checkClearImmediat.Text = "Effacer les résultats";
            this.checkClearImmediat.UseVisualStyleBackColor = true;
            this.checkClearImmediat.CheckedChanged += new System.EventHandler(this.checkClearImmediat_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(317, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ces réglages ne sont accessibles que si les résultats sont effacés.";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkAutoClear);
            this.groupBox2.Controls.Add(this.chkAutoSauve);
            this.groupBox2.Location = new System.Drawing.Point(363, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(164, 107);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "En fin de mesure";
            // 
            // chkAutoClear
            // 
            this.chkAutoClear.AutoSize = true;
            this.chkAutoClear.Location = new System.Drawing.Point(22, 70);
            this.chkAutoClear.Name = "chkAutoClear";
            this.chkAutoClear.Size = new System.Drawing.Size(115, 17);
            this.chkAutoClear.TabIndex = 1;
            this.chkAutoClear.Text = "Reset automatique";
            this.chkAutoClear.UseVisualStyleBackColor = true;
            this.chkAutoClear.CheckedChanged += new System.EventHandler(this.checkAutoClear_CheckedChanged);
            // 
            // chkAutoSauve
            // 
            this.chkAutoSauve.AutoSize = true;
            this.chkAutoSauve.Location = new System.Drawing.Point(22, 31);
            this.chkAutoSauve.Name = "chkAutoSauve";
            this.chkAutoSauve.Size = new System.Drawing.Size(118, 17);
            this.chkAutoSauve.TabIndex = 0;
            this.chkAutoSauve.Text = "Enreg. automatique";
            this.chkAutoSauve.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(454, 377);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 39);
            this.button1.TabIndex = 3;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ButCancel
            // 
            this.ButCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButCancel.Location = new System.Drawing.Point(346, 377);
            this.ButCancel.Name = "ButCancel";
            this.ButCancel.Size = new System.Drawing.Size(95, 39);
            this.ButCancel.TabIndex = 4;
            this.ButCancel.Text = "Abandon";
            this.ButCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panelsepar);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.UpDnDecimales);
            this.groupBox3.Location = new System.Drawing.Point(378, 172);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(149, 189);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Précision";
            // 
            // panelsepar
            // 
            this.panelsepar.Controls.Add(this.radioDefault);
            this.panelsepar.Controls.Add(this.radioPoint);
            this.panelsepar.Controls.Add(this.radiovirgule);
            this.panelsepar.Location = new System.Drawing.Point(18, 95);
            this.panelsepar.Name = "panelsepar";
            this.panelsepar.Size = new System.Drawing.Size(107, 88);
            this.panelsepar.TabIndex = 7;
            // 
            // radioDefault
            // 
            this.radioDefault.AutoSize = true;
            this.radioDefault.Location = new System.Drawing.Point(10, 8);
            this.radioDefault.Name = "radioDefault";
            this.radioDefault.Size = new System.Drawing.Size(75, 17);
            this.radioDefault.TabIndex = 0;
            this.radioDefault.TabStop = true;
            this.radioDefault.Text = "par Defaut";
            this.radioDefault.UseVisualStyleBackColor = true;
            // 
            // radioPoint
            // 
            this.radioPoint.AutoSize = true;
            this.radioPoint.Location = new System.Drawing.Point(10, 54);
            this.radioPoint.Name = "radioPoint";
            this.radioPoint.Size = new System.Drawing.Size(54, 17);
            this.radioPoint.TabIndex = 2;
            this.radioPoint.TabStop = true;
            this.radioPoint.Text = ". point";
            this.radioPoint.UseVisualStyleBackColor = true;
            this.radioPoint.Click += new System.EventHandler(this.OnClkSepar);
            // 
            // radiovirgule
            // 
            this.radiovirgule.AutoSize = true;
            this.radiovirgule.Location = new System.Drawing.Point(10, 31);
            this.radiovirgule.Name = "radiovirgule";
            this.radiovirgule.Size = new System.Drawing.Size(62, 17);
            this.radiovirgule.TabIndex = 3;
            this.radiovirgule.TabStop = true;
            this.radiovirgule.Text = ", virgule";
            this.radiovirgule.UseVisualStyleBackColor = true;
            this.radiovirgule.Click += new System.EventHandler(this.OnClkSepar);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Séparateur";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Nombre de décimales";
            // 
            // UpDnDecimales
            // 
            this.UpDnDecimales.Location = new System.Drawing.Point(18, 43);
            this.UpDnDecimales.Name = "UpDnDecimales";
            this.UpDnDecimales.Size = new System.Drawing.Size(55, 20);
            this.UpDnDecimales.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Controls.Add(this.textBoxdirectory);
            this.groupBox4.Controls.Add(this.listFormat);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.choixdirectory);
            this.groupBox4.Location = new System.Drawing.Point(18, 5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(334, 167);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Enregistrement auto.";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.UpDownIndex);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.textBox1);
            this.groupBox5.Controls.Add(this.combosuffixe);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Location = new System.Drawing.Point(13, 64);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(315, 45);
            this.groupBox5.TabIndex = 29;
            this.groupBox5.TabStop = false;
            // 
            // UpDownIndex
            // 
            this.UpDownIndex.Location = new System.Drawing.Point(247, 12);
            this.UpDownIndex.Name = "UpDownIndex";
            this.UpDownIndex.Size = new System.Drawing.Size(62, 20);
            this.UpDownIndex.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(209, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "index";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(84, 103);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(110, 20);
            this.textBox1.TabIndex = 2;            
            // 
            // combosuffixe
            // 
            this.combosuffixe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combosuffixe.FormattingEnabled = true;
            this.combosuffixe.Items.AddRange(new object[] {
            "essai1",
            "essai2"});
            this.combosuffixe.Location = new System.Drawing.Point(71, 12);
            this.combosuffixe.Name = "combosuffixe";
            this.combosuffixe.Size = new System.Drawing.Size(121, 21);
            this.combosuffixe.TabIndex = 3;
            this.combosuffixe.TabStop = false;
            this.combosuffixe.SelectedIndexChanged += new System.EventHandler(this.combosuffixe_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "suffixe";
            // 
            // textBoxdirectory
            // 
            this.textBoxdirectory.Location = new System.Drawing.Point(6, 38);
            this.textBoxdirectory.Name = "textBoxdirectory";
            this.textBoxdirectory.Size = new System.Drawing.Size(289, 20);
            this.textBoxdirectory.TabIndex = 28;
            // 
            // listFormat
            // 
            this.listFormat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listFormat.FormattingEnabled = true;
            this.listFormat.Items.AddRange(new object[] {
            "Excel (.xls)",
            "Text (.csv)",
            "Tab (.tsv)"});
            this.listFormat.Location = new System.Drawing.Point(76, 139);
            this.listFormat.Name = "listFormat";
            this.listFormat.Size = new System.Drawing.Size(128, 13);
            this.listFormat.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "format";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(2, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Dossier sauvegarde auto";
            // 
            // choixdirectory
            // 
            this.choixdirectory.Location = new System.Drawing.Point(301, 38);
            this.choixdirectory.Name = "choixdirectory";
            this.choixdirectory.Size = new System.Drawing.Size(27, 19);
            this.choixdirectory.TabIndex = 19;
            this.choixdirectory.Text = "...";
            this.choixdirectory.UseVisualStyleBackColor = true;
            this.choixdirectory.Click += new System.EventHandler(this.choixdirectory_Click);
            // 
            // ReglagesMesure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 428);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.ButCancel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ReglagesMesure";
            this.Text = "ReglagesMesure";
            this.Load += new System.EventHandler(this.ReglagesMesure_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReglagesMesure_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panelsepar.ResumeLayout(false);
            this.panelsepar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDnDecimales)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpDownIndex)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxObj;
        private System.Windows.Forms.CheckBox checkBoxParam;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkAutoClear;
        private System.Windows.Forms.CheckBox chkAutoSauve;
        private System.Windows.Forms.CheckBox checkClearImmediat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ButCancel;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown UpDnDecimales;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxdirectory;
        private System.Windows.Forms.ListBox listFormat;
        private System.Windows.Forms.ComboBox combosuffixe;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button choixdirectory;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown UpDownIndex;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDial;
        private System.Windows.Forms.Panel panelsepar;
        private System.Windows.Forms.RadioButton radioPoint;
        private System.Windows.Forms.RadioButton radiovirgule;
        private System.Windows.Forms.RadioButton radioDefault;
    }
}
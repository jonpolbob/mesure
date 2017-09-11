using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mesure
{
    /// <summary>
    /// enum des formats de fichier resultat
    /// </summary>
    public enum resuformat{xls,csv,tsv};


    public partial class EnregFichierParam : Form
    {
        public paramsavres m_parametres;

        /// <summary>
        /// constructeur
        /// </summary>
        public EnregFichierParam()
        {
         InitializeComponent();
         int dropDownButtonWidth = 14;
         this.textBox1.Bounds = this.combosuffixe.Bounds;
         textBox1.Width -= dropDownButtonWidth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttselectdir_Click(object sender, EventArgs e)
        {
            textBoxdirectory.Text = m_parametres.m_autoSaveResPath; 
            if (folderBrowserDial.ShowDialog() == DialogResult.OK)
                this.textBoxdirectory.Text = folderBrowserDial.SelectedPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combosuffixe_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// execute sur le close de la boite de dialogue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnregFichierParam_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK) // fermeture par click sur ok
                {// A FAIRE : on verifie que le fichier est possible et on checkera l'effacement de la serie
                 m_parametres.m_autoSaveResPath = textBoxdirectory.Text;
                 m_parametres.m_autoSaveResSuffix = this.textBox1.Text;
                 m_parametres.m_autoSaveResIdx= (int)this.numericUpDown1.Value;
                
                 // on force en xls il faudra decoder la position de la scrollbox
                 m_parametres.m_autoSaveResFormat = resuformat.xls;
                 m_parametres.m_autosaveonsave= this.checkAutoSauve.Checked;
                 m_parametres.m_clearautosave= this.ChkClearAuto.Checked;
                }

        }

        // init le dialogue avec les parametres de la structure de parametres
        private void EnregFichierParam_Load(object sender, EventArgs e)
        {
            textBoxdirectory.Text = m_parametres.m_autoSaveResPath;
            textBox1.Text = m_parametres.m_autoSaveResSuffix;
            numericUpDown1.DecimalPlaces = 0;
            numericUpDown1.Value = m_parametres.m_autoSaveResIdx;

            // on force en xls il faudra decoder la position de la scrollbox
            m_parametres.m_autoSaveResFormat=resuformat.xls;
            checkAutoSauve.Checked = m_parametres.m_autosaveonsave;
            ChkClearAuto.Checked = m_parametres.m_clearautosave;
        }

        private void EnregFichierParam_FormClosing_1(object sender, FormClosingEventArgs e)
        {

        }

    }
}

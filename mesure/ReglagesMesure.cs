using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mesure
{
    public partial class ReglagesMesure : Form
    {
        public paramsavres m_parametres;

        int m_RadioSepar = 0; // bouton actuellement check

        public ReglagesMesure()
        {
            InitializeComponent();
            int dropDownButtonWidth = 14;
            this.textBox1.Bounds = this.combosuffixe.Bounds;
            textBox1.Width -= dropDownButtonWidth;
        }

        /// <summary>
        /// gestionnaire de clic commaun a tous les radiobuttons
        /// positionne la variable m_radioChecked chaqu fois qu'un bouton est chcked
        /// pour checked un bouton il faut faire
        /// RadioButton r = (RadioButton)panelsepar.Controls[n];
        /// r.Checked = true; et encore c'est pas dit que ca uncheck les autres. a verifier
        /// et positionner m_radio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClkSepar(object sender, EventArgs e)
        {
            int n = 0;
            
            
            // pfff il faut se taper la lecture de tous les radiobuttons du panel
            foreach (Object o in panelsepar.Controls)
            {
                if (o is RadioButton)
                {
                    RadioButton r = (RadioButton)o;
                    if (r.Checked)
                        m_RadioSepar = n;
                    n++;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReglagesMesure_Load(object sender, EventArgs e)
        {
            
            // sectionnom fichier
            textBoxdirectory.Text = m_parametres.m_autoSaveResPath;
            textBox1.Text = m_parametres.m_autoSaveResSuffix;
            UpDownIndex.DecimalPlaces = 0;
            UpDownIndex.Value = m_parametres.m_autoSaveResIdx;
            

            // on force en xls il faudra decoder la position de la scrollbox
            m_parametres.m_autoSaveResFormat=resuformat.xls;
            
            // action en fin de mesure : save et clear
            chkAutoSauve.Checked = m_parametres.m_autosaveonsave;
            chkAutoClear.Checked = m_parametres.m_clearautosave;

            // getstion des champs param et obj et le clear des resultats
            checkClearImmediat.Checked = false; // toujours a false en entrant
            if (m_parametres.m_EmptyResuStatus == 0)// les resultats sont empty
                checkClearImmediat.Enabled = false; // ce bouton ne sert a rien
            else
            {// on gris les boutons si m_EmptyResuStatus !=0 
                checkBoxObj.Enabled= false; // disabled mais check comme c'est dans le param
                checkBoxParam.Enabled = false;
            }

            checkBoxObj.Checked = m_parametres.m_SavObj;
            checkBoxParam.Checked = m_parametres.m_SavParam;
            
            // init du manel de separator 
            // le tralala pour cocher un radio bouton dans un panel
            // et gaffe ac e qu'il y ai pas un erreur d'indice
            m_RadioSepar = m_parametres.m_ResSepar ;
            RadioButton r = (RadioButton)panelsepar.Controls[m_RadioSepar];
            r.Checked = true;
            
            // nombre de decimales
            UpDnDecimales.DecimalPlaces = 0;
            UpDnDecimales.Value = m_parametres.m_ResNbDecim;
            

        }

         /// <summary>
         /// on superpose le textbox d'edit de nom suffixe avec le sroll des valeurs acceptees
         /// </summary>
         public void EnregFichierParam()
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
        private void butOK_Click(object sender, EventArgs e)
        {
         m_parametres.m_autoSaveResPath = textBoxdirectory.Text;
         m_parametres.m_autoSaveResSuffix = this.textBox1.Text;
         m_parametres.m_autoSaveResIdx = (int)this.UpDownIndex.Value;

         // on force en xls il faudra decoder la position de la scrollbox
         m_parametres.m_autoSaveResFormat = resuformat.xls;
         
         m_parametres.m_autosaveonsave = this.chkAutoSauve.Checked;
         m_parametres.m_clearautosave = this.chkAutoClear.Checked;
        
         m_parametres.m_ResSepar = m_RadioSepar;
         m_parametres.m_ResNbDecim = (int)UpDnDecimales.Value;
         m_parametres.m_SavObj = checkBoxObj.Checked;
         m_parametres.m_SavParam = checkBoxParam.Checked;

            // il rest a cleare les resultats
         CoreSystem.Instance.RebuildParamSauvRes(m_parametres); // on copie la structure paramtres dans le core
        }

        private void choixdirectory_Click(object sender, EventArgs e)
        {
            textBoxdirectory.Text = m_parametres.m_autoSaveResPath; 
            if (folderBrowserDial.ShowDialog() == DialogResult.OK)
                this.textBoxdirectory.Text = folderBrowserDial.SelectedPath;
        }

        private void ReglagesMesure_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK) // fermeture par click sur ok
                {m_parametres.m_autoSaveResPath = textBoxdirectory.Text;
                 m_parametres.m_autoSaveResSuffix = this.textBox1.Text;
                 m_parametres.m_autoSaveResIdx = (int)this.UpDownIndex.Value;

                 // on force en xls il faudra decoder la position de la scrollbox
                 m_parametres.m_autoSaveResFormat = resuformat.xls;

                 m_parametres.m_autosaveonsave = this.chkAutoSauve.Checked;
                 m_parametres.m_clearautosave = this.chkAutoClear.Checked;

                 m_parametres.m_ResSepar = m_RadioSepar; // separateur a utililser
                 m_parametres.m_ResNbDecim = (int)UpDnDecimales.Value; // nb decimales
                 
                 if (checkClearImmediat.Checked)
                    m_parametres.m_EmptyResuStatus=2; // vider les resultats en sortant
                 
                 if (m_parametres.m_EmptyResuStatus != 1) // resultats soit vides, soit a vider : on modifie les champs
                    {
                    m_parametres.m_SavObj = checkBoxObj.Checked; 
                    m_parametres.m_SavParam = checkBoxParam.Checked;
                    }

                 // il rest a cleare les resultats
                 CoreSystem.Instance.RebuildParamSauvRes(m_parametres); // on copie la structure paramtres dans le core
                }

        }

        private void checkAutoClear_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkClearImmediat_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkClearImmediat.Checked)
            {
                checkBoxObj.Enabled = true;
                checkBoxParam.Enabled = true;
            }
            else
            {
                checkBoxObj.Enabled = false;
                checkBoxParam.Enabled = false;            
                
            }
            
        }

        private void combosuffixe_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = this.combosuffixe.Text;

        }

        
       
    }
}

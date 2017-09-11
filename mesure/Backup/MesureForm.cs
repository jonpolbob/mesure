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
    /// form pilotant la mesure
    /// </summary>
    public partial class MesureForm : Form
    {
        /// <summary>
        /// constructeur
        /// </summary>
        public MesureForm()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// event declenché quand on clique sur valid : ca declenche un evenement comme un changement de GUI
        /// </summary>
        public event ChangeVariablehandler ChangingVariable; //fonction appelee qui declenche un evenement ailleurs

        /// <summary>
        /// bouton valid : lance l'enregistremetn de la mesure courant a l'aide d'un changingvariable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butValid_Click(object sender, EventArgs e)
        {
            if (ChangingVariable == null) // pas de getionnaire d'evenement : on fait rien
                return;
            
            string curechantillon;
                
            // on lit l'echantillon
            if (EdEchantillon.Enabled)
               curechantillon = EdEchantillon.Text;
            else
               curechantillon = null;
                
            // on le passe au core
            CoreSystem.Instance.CurEchName = curechantillon;

             ChangeVariable ev = new ChangeVariable(ChgVarNum.sendresu, 0, 0); // 2 à : fin de mesure
             ChangingVariable(this, ev);                
            
        }

        /// <summary>
        /// clic sur bouton efface derniere mesure :
        /// ca appelle changingvariable avec delresu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delete_Click(object sender, EventArgs e)
        {
            if (ChangingVariable == null) // pas de gestionnaire : on sort
                return;

            ChangeVariable ev = new ChangeVariable(ChgVarNum.delresu, 0, 0); // 2 à : fin de mesure
            ChangingVariable(this, ev);   
        }

        /// <summary>
        /// bouton sauve
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SauveRes_Click(object sender, EventArgs e)
        {
            //this.DialogResult = DialogResult.OK;
            this.Hide();
            MainForm mainform = (MainForm)Application.OpenForms[0]; // export des resultats
            mainform.EndMesure();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MesureForm_VisibleChanged(object sender, EventArgs e)
        {
            if (CoreSystem.Instance.ParamSauvRes.m_SavParam)
            {
                EdEchantillon.Enabled = true;
                this.lblEchantillon.Enabled = true;
            }
            else
            {
                EdEchantillon.Text = "";
                EdEchantillon.Enabled = false;
                this.lblEchantillon.Enabled = false;
            }
            
        }
        

        /// <summary>
        /// bouton abandonner la mesure 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
         this.Hide();         
        }
        
    }
}

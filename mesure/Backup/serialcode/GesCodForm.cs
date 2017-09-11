using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using serialcode;
using System.IO;

namespace mesure
{
    public partial class GesCodForm : Form
    {
        int m_codemessage;

        public int CodeMessage
        { set { 
                m_codemessage = value;
               }
        }


        public GesCodForm()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void GesCodForm_Load(object sender, EventArgs e)
        {
            switch (this.m_codemessage)
            { case 1 : // fichier inexistant
                    this.textExpli.ForeColor=Color.Red;
                    this.textExpli.Text = "générez un fichier de cle \r\n en cliquant sur 'générer une clé'\r\n apres avoir saisi le nom de l'utilisateur à enregistrer pour ce logiciel";
                    this.textMessage.Text = "Motion n'a trouvé aucune licence";
                    break;

                case 2 : 
                    this.textExpli.ForeColor=Color.Red;
                    this.textExpli.Text = "Générez un nouveau fichier de cle \r\n en cliquant sur 'générer une clé'\r\n apres avoir saisi le nom de l'utilisateur à enregistrer pour ce logiciel";
                    this.textMessage.Text = "Le fichier de licence n'est pas valide";
                    break;


                }
        }
        
        
        /// <summary>
        ///  generer une cle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void but_genkey_Click(object sender, EventArgs e)
        {
            string licname;// = Application.ExecutablePath.Clone(); // path de l'exe            
            licname = Application.ExecutablePath.ToLower().Replace(".exe", ".key"); // nom du fichier key
            string username = this.textNomUser.Text;

            protectionclass laprotection = new protectionclass();
            laprotection.setnomuser(username);
            laprotection.savkeyfile(licname);

            this.textMessage.Text = "Cle générée";
            this.textExpli.Text = "Un fichier de cle a été généré dans \r\n " + licname + " \r\n Vos pouvez le charger en piece jointe dans un mail\r\n pour recevoir votre fichier de licence";
            
        }
        
        // bouton activer le lic
        private void but_ActLic_Click(object sender, EventArgs e)
        {
            FileDialog ledial = new OpenFileDialog();

            ledial.DefaultExt = "lic";
            ledial.FileName = "*.lic";
            ledial.Filter = "fichier de licence (*.lic)|lic||";
            if (ledial.ShowDialog() != DialogResult.OK)
                {
                this.DialogResult = DialogResult.OK; // ok : on reboote pas
                Close();
                return;
                }


            string nomfile = ledial.FileName;
            
            // on reconstruit le nom de ce fichier dans le repertoire de l'exe
            
            // si le fichier de licence existe deja : on le sauvearde
            string licname;// = Application.ExecutablePath.Clone(); // path de l'exe            
            licname = Application.ExecutablePath.ToLower().Replace(".exe", ".lic"); // nom du fichier key
            if (File.Exists(licname))
                {
                string dstname = Application.ExecutablePath.ToLower().Replace(".exe", ".li$"); // nom du fichier key  File.Copy(licname,}
                if (File.Exists(dstname))
                    File.Delete(dstname);

                File.Copy(licname, dstname);
                File.Delete(licname);
                }

            File.Copy(nomfile,licname );

            // on reboot
            this.DialogResult = DialogResult.Cancel; // cancel : on reboote
            Close();

            return;            
        }
        
        // bouton fermer
        private void butferm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK; // OK : on sort
            Close();
        }
    }
}

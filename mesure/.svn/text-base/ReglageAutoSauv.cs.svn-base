using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace mesure
{
    public partial class ReglageAutoSauvImg : Form
    {
        public paramsavimg m_paramsauv;
        

        public ReglageAutoSauvImg()
        {
            InitializeComponent();
            int dropDownButtonWidth = 14;
            this.textBox1.Bounds = this.combosuffixe.Bounds;
            textBox1.Width -= dropDownButtonWidth;
        }

        private void buttselectdir_Click(object sender, EventArgs e)
        {
            textBoxdirectory.Text = m_paramsauv.m_ResPath; 
            if (folderBrowserDial.ShowDialog() == DialogResult.OK)
                this.textBoxdirectory.Text = folderBrowserDial.SelectedPath;
        }

        private void combosuffixe_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = this.combosuffixe.Text;

        }

        private void ReglageAutoSauv_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK) // fermeture par click sur ok
                {// A FAIRE : on verifie que le fichier est possible et on checkera l'effacement de la serie
                    m_paramsauv.m_ResPath = textBoxdirectory.Text;
                    m_paramsauv.m_ResSuffix = this.textBox1.Text;
                    m_paramsauv.m_ResIdx = (int)this.numericUpDown1.Value;


                    System.Drawing.Imaging.ImageFormat imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;

                switch(this.listBox1.SelectedIndex )
                    {case 0 : imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                     case 1 : imgformat = System.Drawing.Imaging.ImageFormat.Tiff;
                        break;
                        
                     case 2 :imgformat = System.Drawing.Imaging.ImageFormat.Bmp;
                           break;
                    }
                        
                 // on force en jpeg il faudra decoder la position de la scrollbox
                    m_paramsauv.m_Format = imgformat;
                }

        }

        private void ReglageAutoSauvImg_Load(object sender, EventArgs e)
        {
            textBoxdirectory.Text = m_paramsauv.m_ResPath;
            this.textBox1.Text =m_paramsauv.m_ResSuffix;
            this.numericUpDown1.Value = m_paramsauv.m_ResIdx;

            System.Drawing.Imaging.ImageFormat imgformat = m_paramsauv.m_Format;
            if (imgformat == System.Drawing.Imaging.ImageFormat.Jpeg)
                this.listBox1.SelectedIndex = 0;

            if (imgformat == System.Drawing.Imaging.ImageFormat.Tiff)
                this.listBox1.SelectedIndex= 1;
            
            if (imgformat == System.Drawing.Imaging.ImageFormat.Bmp)
                this.listBox1.SelectedIndex = 2;

            // on force en jpeg il faudra decoder la position de la scrollbox
            
        }

        
    }
}
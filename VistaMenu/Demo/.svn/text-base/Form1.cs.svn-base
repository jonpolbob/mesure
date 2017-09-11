using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;

namespace VistaMenuDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Font = SystemFonts.MessageBoxFont;

            InitializeComponent();
        }

        private void btnSetOptionsImage_Click(object sender, EventArgs e)
        {
            vistaMenu.SetImage(mnuOptions, vistaMenu.GetImage(mnuAddFiles));
        }

        private void btnClearOptionsImage_Click(object sender, EventArgs e)
        {
            vistaMenu.SetImage(mnuOptions, null);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
                contextMenu.Show(this, e.Location);
        }
    }
}

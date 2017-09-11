using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace mesure
{
    public partial class Résultats : Form
    {
        public Résultats()
        {
            InitializeComponent();
        }

        public int LoadResu(DataGridView source)
        {// pour l'instant : on copie les resultats dans ce tableau
            dataGridResult.Rows.Clear();
            dataGridResult.ColumnCount = source.ColumnCount;
            dataGridResult.ColumnHeadersVisible = source.ColumnHeadersVisible;
            int dstcol =0;
            
            foreach (DataGridViewColumn colhead in source.Columns)
                dataGridResult.Columns[dstcol++].HeaderText = colhead.HeaderText;

            
            foreach (DataGridViewRow row in source.Rows)
            {
                DataGridViewRow newrow = (DataGridViewRow)row.Clone(); // clone ne coppie pas les cell values
                foreach (DataGridViewCell cell in row.Cells)
                    newrow.Cells[cell.ColumnIndex].Value = cell.Value;
                this.dataGridResult.Rows.Add(newrow);
            }
        
            return 0; // pas de souci
        }
        
        
        // icon impression
        private void printToolStripButton_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// clic sur icon enregistrer sous xls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog ledial = new SaveFileDialog();

            string path = Directory.GetCurrentDirectory(); // chamin d'ouverture
            ledial.InitialDirectory = path;
            ledial.DefaultExt = "xls";
            ledial.FileName = "sans nom";
            ledial.Filter = "Excel Files|.xls||";
            if (ledial.ShowDialog(this) == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(ledial.FileName); ;
            

            int iColCount = dataGridResult.ColumnCount;
            
            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dataGridResult.Columns[i].Name); 
                if (i < iColCount - 1) 
                    sw.Write("\t"); 
            
            }
            sw.Write(sw.NewLine);
            foreach (DataGridViewRow dr in dataGridResult.Rows)	
                {for (int i = 0; i < iColCount; i++)		
                    {if (!Convert.IsDBNull(dr.Cells[i]))			
                        {sw.Write(dr.Cells[i].Value);
                        }
                    if ( i < iColCount - 1)			
                        {sw.Write("\t");			
                        }		
                    }		
                sw.Write("\r");	
                }

                sw.Close();
                }

            this.Close();
        }
        


        /// <summary>
        ///  clic sur icon copier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            this.dataGridResult.SelectAll();
            DataObject dataObj = dataGridResult.GetClipboardContent();
            Clipboard.SetDataObject(dataObj,true); // true car reste apres la fin de l'appli
            this.Close();
        }




    }
}

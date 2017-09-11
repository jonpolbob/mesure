using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace mesure
{
    class logger
    {
        public static void log(string strLogText)
        {
            // Create a writer and open the file:
            StreamWriter log;
            return; // neutralise pour cause de bug.

            //string path = Application.StartupPath; // path de l'exe
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            

            if (!File.Exists(path + "\\logfile.txt"))
            {
                log = new StreamWriter(path + "\\logfile.txt");
            }
            else
            {
                log = File.AppendText(path + "\\logfile.txt");
            }

            // Write to the file:
            log.WriteLine(DateTime.Now+"\t"+strLogText);
            
            // Close the stream:
            log.Close();
            }

    }
}

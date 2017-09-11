using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Win32;
 
namespace serialcode
{
    
    
    class Sample_ManagementObjectSearcher
    {
        public string GetSerialVista2(ref string dskserial)
    {
        RegistryKey thekey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\");
        //RegistryKey thekey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\");
            string txt =  (string)thekey.GetValue("ProductId");
            return txt;
            
    }
        public string GetSerialVista(ref string dskserial)
        {
            //String Model;
            //String Type;
            String Serial;
            //String DeviceID;

            ConnectionOptions options =
            new ConnectionOptions();


            ManagementScope scope = new ManagementScope("\\\\127.0.0.1\\root\\cimv2", options);
            scope.Connect();

            //Query system for Operating System information
            ObjectQuery query = new ObjectQuery("select * from Win32_WindowsProductActivation");
            ManagementObjectSearcher searcherphysical = new ManagementObjectSearcher(scope, query);


            Serial = "00";
            foreach (ManagementObject share2 in searcherphysical.Get())
            {
                if (share2["ProductID"] == null)
                    Serial = "00";
                else
                    Serial = share2["ProductID"].ToString();
            }

            return Serial;

        }

        
        public string GetSerial(ref string dskserial)
        {
            //String Model ;
            //String Type;
            String Serial;
            //String DeviceID;

            /* --- suppression du code inutils pour le serial
            ManagementObjectSearcher searcherlogical =
                new ManagementObjectSearcher("select * from Win32_LogicalDisk Where DriveType = 3");
            foreach (ManagementObject share3 in searcherlogical.Get())
            {
                DeviceID = share3["DeviceID"].ToString();
                string sysemname = share3["Name"].ToString();   
                
            }

            // lecture de l'information presente dans win32diskdrive
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (ManagementObject share in searcher.Get())
            {
                string txtout = share["DeviceID"].ToString();
                //if (txtout.CompareTo("c:") == 0)
                {
                    Model = share["Model"].ToString();
                    Type = share["InterfaceType"].ToString();
                }
            }
            --- fin code inutile
             * */

            // lecture de l'info du disque dur

            // lecture de l'information presente dans win32physicalmedia
            return GetSerialVista2(ref dskserial);

            try
            {

                ManagementObjectSearcher searcherphysical =
                    new ManagementObjectSearcher("select * from Win32_WindowsProductActivation");
                Serial = "00";
                foreach (ManagementObject share2 in searcherphysical.Get())
                {
                    if (share2["ProductID"] == null)
                        Serial = "00";
                    else
                        Serial = share2["ProductID"].ToString();
                }

            }catch (Exception e)
            {// il y a un os en xp : on essaie en vista
                //MessageBox labox = new MessageBox();
                MessageBox.Show("vista detected");
                
                return GetSerialVista(ref dskserial);
            }

            return Serial;            
        }

    }

    public class protectionclass
    {
        protected string m_md5;
        protected bool m_md5_valid = false;
        protected bool m_wcode_present = false;
        protected string m_wcode = "";
        protected string m_registereduser;
        protected string m_version;
        protected string m_nomfile;

        protected List<licdoublon> m_liste = new List<licdoublon>();

        string GetVariable(string variable)
        {
            return "";
        }

        /// <summary>
        /// constructeur
        /// </summary>
        public protectionclass()
        {
            Sample_ManagementObjectSearcher searcher = new Sample_ManagementObjectSearcher();
            string serial = "00";
            string txtout = searcher.GetSerial(ref serial); // lecture du numero de dserie windows
            m_md5 = computehash(txtout); // hash local
            m_version = "0.0";
            m_registereduser = "unknown";
        }


        /// <summary>
        /// sauvegarde le fichier de clef
        /// si le md5 est ok : on l'enregistre et on enregistrez toutes les clefs validees
        /// dans tous les cas on enregistre le numero de serie aussi
        /// </summary>

        /// <param name="name"></param>
        /// <param name="md5cod"></param>
        /// <returns></returns>
        public int savkeyfile(string name)
        {// si on a un md5 correct c a d on ac charge un fichier valide (cas d'une mise a jour de clef temporaire)
            StreamWriter lefile;

            try
            {
                lefile = File.CreateText(name);
            }
            catch (Exception e)
            {
                return 1;
            }

            // si fichier de protection : on reenregistre le hashcode qui a ete lu lors de l'ouverture du fichier
            // sinon, on enregistre la clef windows qui sera convertie en hashcode par le vendeur
            string txtout;
            string txttowrite;

            // on enregistre d'abord la version du fichier
            txtout = ("numver:" + m_version);
            txttowrite = Encryption25(txtout, "170505");
            lefile.WriteLine(txttowrite);

            // et le useruser
            txtout = ("reguser:" + m_registereduser);
            txttowrite = Encryption25(txtout, "170505");
            lefile.WriteLine(txttowrite);


            // on enregistre le serial de windows dans tous les cas
            // avant le md5 
            {
                Sample_ManagementObjectSearcher searcher = new Sample_ManagementObjectSearcher();
                string serial = "00";
                txtout = "ws:" + searcher.GetSerial(ref serial); // lecture du numero de dserie windows             
                txttowrite = Encryption25(txtout, "170505");
                lefile.WriteLine(txttowrite);
            }


            if (m_md5_valid) // le mdcode est valide : on l'enregistre
            {   // md5 du code serial
                txtout = "wcode:" + m_md5; // on enregistre le code md5
                txttowrite = Encryption25(txtout, "170505");
                lefile.WriteLine(txttowrite);

                savevariables(lefile);
            }


            lefile.Close();
            return 0;

        }

        /// <summary>
        /// fonction sachant lire et ecrire un fichier
        /// // sait ecrire un fichier de 2 manieres : 
        /// // soit avec le numero de serie windows
        /// dans ce cas le fichier n'est pas utilisable
        /// sot avec un md5 pour utilisation apres decodage
        /// s'il y a un md5 en plus du numero de serie windows ca le verifie et ca charge.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cansavmd5"></param>
        /// <returns></returns>
        protected int savevariables(StreamWriter lefile)
        {
            string txtout;
            string txttowrite;

            // application et ses variables
            txtout = ("@app0:010110:" + "valeurofappli0");
            txttowrite = Encryption25(txtout, "170505");
            lefile.WriteLine(txttowrite);
            String valeurofvariable1 = "1515";

            txtout = ("@var0:000000" + "xxyyzz:" + valeurofvariable1);
            txttowrite = Encryption25(txtout, "170505");
            lefile.WriteLine(txttowrite);

            return 0;
        }


        /// <summary>
        /// charge un fichier lic
        /// on lit le md5, on le compare avec le numero de licence :
        /// si c'est bon on lit tout
        /// sinon on sort
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int loadlicfile(string name)
        {
            m_wcode_present = false;
            m_liste.Clear();
            if (System.IO.File.Exists(name) == false)
                return 1; // 

            // lecture ligne a ligne du fichier
            int counter = 0;
            string line;

            // on lit le fichier ligne opar ligne
            //System.IO.StreamReader file = new System.IO.StreamReader(@"c:\test.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(name);
            while ((line = file.ReadLine()) != null)
            {
                string resu = Decryption25(line, "170505");
                string[] tabmots = resu.Split(':');
                if (tabmots[0] == "version")
                {//decodage de la version de lic
                    //int version = System. tabmots[1].
                    //ici pour les versions suivantes : On rappelera Le code DES versions precedentes
                }

                // on a le code md5 dedans : ca se decode tout seul
                if (tabmots[0] == "wcode")
                {//decodage du code windows
                    string code = tabmots[1];
                    // on calcule le md5 du numero windows
                    if (code == m_md5)
                        m_md5_valid = true;
                    else
                        {
                        file.Close();
                        return 2; // code non valide
                        }
                }

                if (tabmots[0] == "reguser")
                {//decodage du code windows
                    m_registereduser = tabmots[1];
                }

                // nom d'origine du fichier de licence
                if (tabmots[0] == "filename")
                {// on enregistre le md5 qui a ete lu
                    m_nomfile = tabmots[1]; // on enregistre le nom du fichier trransmis par le net
                }

                if (tabmots[0][0] == '@')
                {//decodage du code windows
                    string nomvar = tabmots[0];
                    string valeur = tabmots[1];
                    licdoublon doubl = new licdoublon();
                    doubl.nomvar = tabmots[0].Substring(2);
                    doubl.valeur = tabmots[2];
                    doubl.dluo = tabmots[1];
                    m_liste.Add(doubl);
                }

            }

            file.Close();
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private TripleDES CreateDES(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = md5.ComputeHash(Encoding.Unicode.GetBytes(key));
            des.IV = new byte[des.BlockSize / 8];
            return des;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="PlainText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string Encryption25(string PlainText, string key)
        {

            Encoding ascii = Encoding.Unicode;
            //PlainText.PadRight(25, 'µ');

            //UTF8Encoding utf8 = new UTF8Encoding();
            System.Text.UnicodeEncoding unicd = new UnicodeEncoding();

            TripleDES des = CreateDES(key);
            ICryptoTransform ct = des.CreateEncryptor();
            byte[] input = unicd.GetBytes(PlainText);
            byte[] output = ct.TransformFinalBlock(input, 0, input.Length);
            string resu = BitConverter.ToString(output);
            return resu.Replace("-", " ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtin"></param>
        /// <returns></returns>
        protected byte[] cutstring(string txtin)
        {
            int nboctets = (txtin.Length + 1) / 3;

            byte[] outbyte = new byte[nboctets];
            // on nettoie le txtin de tout ce qui n'es pas dans l'intervalle
            for (int i = 0; i < txtin.Length; i += 3)
            {
                string octet = txtin.Substring(i, 2);
                outbyte[i / 3] = Convert.ToByte(octet, 16);
            }

            return outbyte;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CypherText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        protected string Decryption25(string CypherText, string key)
        {
            byte[] b = cutstring(CypherText);
            UTF8Encoding utf8 = new UTF8Encoding();
            string input = utf8.GetString(b);
            // ici on a refait un tabeau de bytes il, resste a le decrypter

            TripleDES des = CreateDES(key);
            ICryptoTransform ct = des.CreateDecryptor();
            byte[] output = ct.TransformFinalBlock(b, 0, b.Length);
            string txtacouper = Encoding.Unicode.GetString(output);
            return txtacouper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="txtin"></param>
        /// <returns></returns>
        protected string computehash(string txtin)
        {
            byte[] input = System.Text.ASCIIEncoding.Unicode.GetBytes(txtin);

            MD5 md5 = new MD5CryptoServiceProvider();
            try
            {
                byte[] result = md5.ComputeHash(input);

                // Build the final string by converting each byte
                // into hex and appending it to a StringBuilder
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    sb.Append(result[i].ToString("X2"));
                }

                // And return it
                return sb.ToString();
            }
            catch (ArgumentNullException ane)
            {
                //If something occurred during serialization, 
                //this method is called with a null argument. 
                Console.WriteLine("Hash has not been generated.");
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomuser"></param>
        /// <returns></returns>
        public int setnomuser(string nomuser)
        {
            m_registereduser = nomuser;
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getnomuser()
        {
            return m_registereduser;
        }

    }

    // classe uniqiement utilisee par le generateur de licence
    class protectionclasswrite : protectionclass
    {
        /// <summary>
        /// enregisre un ficheir de licence a partir du code windows lu dans le fichier
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// ///lecture d'un fichier de clef 
        /// // on lit le wcode
        /// et on le compare au md5 du fichier
        /// si c'est ok : on charge les variables du fichier
        /// sinon on ne charge rien
        public int loadkeyfile(string name)
        {
            m_wcode_present = false;

            if (System.IO.File.Exists(name) == false)
                return 1; // 

            m_liste.Clear();

            // lecture ligne a ligne du fichier
            int counter = 0;
            string line;

            // on lit le fichier ligne opar ligne
            //System.IO.StreamReader file = new System.IO.StreamReader(@"c:\test.txt");
            System.IO.StreamReader file = new System.IO.StreamReader(name);
            while ((line = file.ReadLine()) != null)
            {
                string resu = Decryption25(line, "170505");
                string[] tabmots = resu.Split(':');
                if (tabmots[0] == "numver")
                {//decodage de la version de lic
                    //int version = System. tabmots[1].
                    //ici pour les versions suivantes : On rappelera Le code DES versions precedentes
                }

                // on a un serial windows dedans 
                // attention il faut toujours que le serial windows soit avant le md5
                if (tabmots[0] == "ws")
                {//decodage du code windows
                    string code = tabmots[1];
                    // on calcule le md5 du numero windows
                    m_md5 = computehash(code); // hash du serial lu dans le fichier
                    m_wcode_present = true;
                    m_wcode = code;
                }

                // on a le code windows md5 dedans : on le lit
                if (tabmots[0] == "wcode")
                {//decodage du code windows
                    string code = tabmots[1];
                    // on verifie que le md5 du fichier correspond a l'ordi du fichier
                    if (code == m_md5)
                        m_md5_valid = true;
                }

                // lecture de l'utilisateur
                if (tabmots[0] == "reguser")
                {//decodage du code windows
                    m_registereduser = tabmots[1];
                }

                if (m_md5_valid && tabmots[0][0] == '@') // lecture d'une variable ou d'un programme
                {//decodage du code windows
                    string nomvar = tabmots[0];
                    string valeur = tabmots[1];
                    licdoublon doubl = new licdoublon();
                    doubl.nomvar = tabmots[0];
                    doubl.valeur = tabmots[2];
                    doubl.dluo = tabmots[1];
                    m_liste.Add(doubl);
                }
            }

            file.Close();

            return 0;

        }

        /// <summary>
        /// creee u n fichier de licence
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>        
        public int savlicfile(string name)
        {// si on a un md5 correct c a d on ac charge un fichier valide (cas d'une mise a jour de clef temporaire)

            StreamWriter lefile = File.CreateText(name);
            string txtout;
            string txttowrite;

            // si fichier de protection : on reenregistre le hashcode qui a ete lu lors de l'ouverture du fichier
            // sinon, on enregistre la clef windows qui sera convertie en hashcode par le vendeur

            if (m_version != "0.0")
            {// cas ou c'est pas la bonne vesion : on appelle un ancien programme de version
            }

            // on enregistre d'abord la version du fichier
            // il faut peut etre enregistrer avec l'ancienne version pour eviter que le programme installe soit perdu ?
            txtout = ("numver:" + m_version);
            txttowrite = Encryption25(txtout, "170505");
            lefile.WriteLine(txttowrite);

            // et le useruser
            txtout = ("reguser:" + m_registereduser);
            txttowrite = Encryption25(txtout, "170505");
            lefile.WriteLine(txttowrite);

            // on enregistre le md5 qui a ete lu
            txtout = "wcode:" + m_md5; // on enregistre le code md5
            txttowrite = Encryption25(txtout, "170505");
            lefile.WriteLine(txttowrite);

            // on n'enregistre jamais le serial de windows dans u fichier de licence
            /*{
                  Sample_ManagementObjectSearcher searcher = new Sample_ManagementObjectSearcher();
                  string serial = "00";
                  txtout = "ws:" + searcher.GetSerial(ref serial); // lecture du numero de dserie windows             
                  txttowrite = Encryption25(txtout, "170505");
                  lefile.WriteLine(txttowrite);
            }*/

            // on enregistre le md5 qui a ete lu
            txtout = "filename:" + this.m_nomfile; // on enregistre le nom du fichier trransmis par le net
            txttowrite = Encryption25(txtout, "170505");
            lefile.WriteLine(txttowrite);


            // on n'enregistre jamais le serial de windows dans u fichier de licence

            // on enregistre le user
            savevariables(lefile);

            lefile.Close();

            return 0;
        }


      
        public string setnomfile(string nomfile)
        {

            int posname = nomfile.LastIndexOf("\\");
            m_nomfile = nomfile.Substring(posname + 1);

            //m_nomfile
            return m_nomfile;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomuser"></param>
        /// <returns></returns>
        public int setversion(string numver)
        {
            m_version = numver;
            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getversion()
        {
            return m_version;
        }


        /// <summary>
        /// renvoie le wcode s'il y en a 1
        /// c a dire le code windows lu dans le fichier
        /// </summary>
        /// <param name="wcode"></param>
        /// <returns></returns>
        public int getwcode(ref string wcode)
        {
            int retour = 1;
            if (m_wcode_present)
                wcode = m_wcode;
            else
                retour = 0;

            return retour;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class licdoublon
    {
        string m_nomvar;
        string m_valeur;
        string m_dluo;

        internal string nomvar
        {
            get
            { return m_nomvar; }

            set
            { m_nomvar = value; }
        }

        internal string valeur
        {
            get
            { return m_valeur; }
            set
            { m_valeur = value; }
        }

        internal string dluo
        {
            get
            { return m_dluo; }

            set
            { m_dluo = value; }

        }

    }

}

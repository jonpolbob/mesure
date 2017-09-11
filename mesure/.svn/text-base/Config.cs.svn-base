using System;
using System.Collections.Generic;
using System.Text;

namespace mesure
{
    public class Config : IBagSavXml
    {
        private bool m_UrlJpgAvail = false; // presence camera urljpg
        private bool m_UrlMpgAvail = false; // presence camera urlmpg
        private bool m_UrlTwainAvail = false; // presence entree twain
        private bool m_TwainAvail = false;
        // element du savxml
        public string getNomType()
            { return "settings"; 
            } //// renvoie le nom de ce type de bag

        public string[] getTabVers()
             {
             string[] tabout = { "1.0" };
             return tabout;
             }
        
        /// <summary>
        /// savedisk ne fait rien. on verra plus tard s'il enregistre qqchose en code
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public int SaveDisk(XMLAvElement elem)
        { return 0;
        }
        
        /// <summary>
        /// lecture de la config dans son fichier xml
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public int LoadDisk(XMLAvElement elem)
        {// en tete standard d'un loaddisk
            string strtType = "";
            string[] tabout = { "1.0" };

            int ok = XMLAvElement.chkelement(elem,"settings", tabout);

            if (ok != 0) // pas bonne section
                return 1;

            // on verifie que c'est une camer et on lit le numero de version pour bien decoder ca
            String nomconfig;
         
            XMLAvElement sources = elem.GetFirstElement("sources");

            // lecture des sources
            LoadSources(sources);
            return 0;
        }

        /// <summary>
        /// lecture des configs ds sourcesv 
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        int LoadSources(XMLAvElement elem)
        {// en tete standard d'un loaddisk
         string strtType = "";
         string[] tabout = { "1.0" };

         try
         {
             int ok = XMLAvElement.chkelement(elem, "sources", tabout); // tout a la meme version 

         }catch (XmlAvException a)
         { return 1;  // il y a un gos problemedans le fichier settings : 
         }

         ///-------- fin du debut standard de lecture d'un xmlbag
         ///

            // chargement des settings des sources
            try
            {
            XMLAvElement urljpg = elem.GetFirstElement("UrlJpg");
            LoadUrlJpg(urljpg); // on lit la section urljpg


            // chargement des settings des mesures
            // chargement des settings des sources
            XMLAvElement urlmpg = elem.GetFirstElement("UrlMpg");
            LoadUrlMpg(urlmpg); // on lit la section urljpg
            }
            catch (XmlAvException a)
            { }
            // chargement des settings des resultats

            return 0;
        }

        /// <summary>
        /// charle une section urljpg contenant 
        /// available pour dire si cette sorte de source est dispo
        /// et la liste des raccourcis ves les urls
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        int LoadUrlJpg(XMLAvElement elem)
        {// en tete standard d'un loaddisk
            string strtType = "";
            string[] tabout = { "1.0" };

            int ok = XMLAvElement.chkelement(elem, "UrlJpg", tabout); // tout a la meme version 
            if (ok != 0) // probleme de format
                return 1;
            ///-------- fin du debut standard de lecture d'un xmlbag

            ///on lit les parametres de urljpg : available et liste des raccourcis

            int avail;
            elem.GetAttribute("available", out avail);

            if (avail == 1)
            {
                m_UrlJpgAvail = true;
                // ici mon lit les raccourcis et on les met dansla liste de chargement du dialogue
            }
            
            return 0;

           }



    int LoadUrlMpg(XMLAvElement elem)
        {// en tete standard d'un loaddisk
            string strtType = "";
            string[] tabout = { "1.0" };
            int ok = XMLAvElement.chkelement(elem, "UrlMpg", tabout); // tout a la meme version 
            if (ok != 0) // probleme de format
                return 1;
            ///-------- fin du debut standard de lecture d'un xmlbag

            ///on lit les parametres de urljpg : available et liste des raccourcis

            int avail=0;
            elem.GetAttribute("available", out avail);

            if (avail ==1)
                {m_UrlMpgAvail = true;
                 // ici mon lit les raccourcis et on les met dansla liste de chargement du dialogue
           }

            return 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SrcUrlMpgAvail
        {get {return (m_UrlMpgAvail);}
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SrcUrlJpgAvail
        {get {return (m_UrlJpgAvail);}
        }

        public bool SrcTwainAvail
        {get {return (m_TwainAvail);}
        }

            
    }
}

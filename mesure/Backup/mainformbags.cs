 
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using VideoSource;
using mesure.vfw;
using Direct3D = Microsoft.DirectX.Direct3D;
using DirectX = Microsoft.DirectX;
using System.Drawing.Printing;
//using TwainLib;

using System.Windows;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using mesure;
using System.Diagnostics;
using System.IO; // pour bitmapinfoheader

namespace mesure
{


    // une classe pour faire un enum de strings
    public sealed class xmlavlabels
    {
        private xmlavlabels() { }

        public const string numver = "Ver";
        public const string type = "Typ";
        public const string chktyp = "ChkTyp";
        public const string param = "Param";

    }

    public class paramGUI : IBagSavXml
    {
        public int modewindow = 0; 
        
        // fenetre client
        public int mainleft;
        public int maintop;
        public int mainwidth;
        public int mainheight;

        // fenetre de mesure
        public int mesleft; 
        public int mestop;


        public paramGUI()
        { }


        //charge les parametres a partir des parametres de la mainforme
        public int SetParams(MainForm mainform)
        {
            mainleft = mainform.Left;
            maintop = mainform.Top;
            mainwidth = mainform.Width;
            mainheight = mainform.Height;

            Form dialform = mainform.pDialMesure;
            if (dialform ==null)
                    {mesleft=0;
                    mestop=0;
                    return 0;
                    }
            mesleft = dialform.Left;
            mestop = dialform.Top;
            
            return 0;

        }

        // renvoie la position de la fenetre mesure
        
        public int SetMainPosition(MainForm mainform)
        {

            mainform.SetBounds(mainleft, maintop, mainwidth, mainheight, BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Width | BoundsSpecified.Height);
            return 0;
        }
        
        // renvoie la position de la fenetre mesure
        public int SetMesPos(Form MesForm)
        {
            if (MesForm == null) // doit gerer le parametre null
                return 0;

            MesForm.SetBounds(mesleft, mestop, 0, 0, BoundsSpecified.X | BoundsSpecified.Y);
            return 0;
        }



        //-------- elements de ibagxml
        public string[] getTabVers()
        {
            string[] tabvers = { "1.0" };
            return tabvers;
        }
        
        public string getNomType()
        {
            return "GUIparams";
        }


        // ibagxml
        /// <summary>
        /// charge un element 
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public int LoadDisk(XMLAvElement elem)
        {
         //----  contenu standard de tous les bags  -----
         if (elem == null)
              return 2;

          string chktype ;
          try
          {
              elem.GetAttribute(xmlavlabels.chktyp, out chktype);
              if (chktype.CompareTo(this.getNomType()) != 0)
                  return 1;
          }catch(XmlAvException a)
          {return 1;
          }
            XMLAvElement param;

            try{ // on lit le contenu de 'params'
               param = elem.GetFirstElement(xmlavlabels.param);
                }catch (XmlAvException a)
            {return 1;
            }
            //----------------------------------
            
            try{
                XMLAvElement mainwindow = param.GetFirstElement("mainwnd");
                mainwindow.GetAttribute("x",out mainleft);
                mainwindow.GetAttribute("y", out maintop);
                mainwindow.GetAttribute("w", out mainwidth);
                mainwindow.GetAttribute("h", out mainheight);
                }
            catch (XmlAvException a)
            {}

            try{
                XMLAvElement meswindow = param.GetFirstElement("meswnd");
                meswindow.GetAttribute("x", out mesleft);
                meswindow.GetAttribute("y", out mestop);
            }
            catch(XmlAvException a)
            {}
            return 0;
        }

       /// <summary>
       /// sauve les valeurs du bag sur disque
       /// </summary>
       /// <param name="elem"></param>
       /// <returns></returns>
        public int SaveDisk(XMLAvElement elem)
        {elem.SetAttribute(xmlavlabels.chktyp, this.getNomType());
         elem.SetAttribute(xmlavlabels.numver, XMLAvElement.getxmlver(this.getTabVers())); 
         XMLAvElement param = elem.CreateNode(xmlavlabels.param);
         // sauve les paramters
         
         XMLAvElement mainwindow = param.CreateNode("mainwnd");
         mainwindow.SetAttribute("x",mainleft);
         mainwindow.SetAttribute("y",maintop);
         mainwindow.SetAttribute("w",mainwidth);
         mainwindow.SetAttribute("h",mainheight);

         XMLAvElement meswindow = param.CreateNode("meswnd");
         meswindow.SetAttribute("x",mesleft);
         meswindow.SetAttribute("y",mestop);
            
         return 0;
        }

    }
    
    #region paramsavres
    public class paramsavres : IBagSavXml, ICloneable
    {// element pas a sauvegarder dans fichier
        public int m_EmptyResuStatus = 0; // dit si es resultats sont vides ou a vider au conurs de creation/destruction d dialogue
        
        // elements de donnees
        public string m_autoSaveResPath = "."; 
        public string m_autoSaveResSuffix = "image";
        public int m_autoSaveResIdx = 0;
        public resuformat m_autoSaveResFormat = resuformat.xls;
        public bool m_clearautosave = false; // clear apres autosave
        public bool m_autosaveonsave = false; // autosave en mesure
        public bool m_SavParam = false; //enregistrer les parametres dans les resultats
        public bool m_SavObj = false;  // enregistrer les objectifs dans le ficxhier resultat
        
        public int m_ResNbDecim = 2; // nombre de decimales des resultats
        public int m_ResSepar = 0; // separateur de decimales 0 = dafuat, 1 = point 2 = virgule
        
            
        // interface ibagsavxml
        public string getNomType()
        {
            return (string)"paramsavres";
        }

        public string[] getTabVers()
        {
            string[] outtab ={ "1.0" };
            return outtab;
        }

        /// <summary>
        /// renvoie la propriete disant si il faut sauver les parametres avec els resultats
        /// </summary>
        public bool SavParam
        {
            get { return m_SavParam;
            }
        }

        /// <summary>
        /// renvoie la propriete disant si il faut sauver l'objecti avec els resultats
        /// </summary>
        public bool SavObj
        {
            get
            {
                return m_SavObj;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int SaveDisk(XMLAvElement elem)
        {
            elem.SetAttribute(xmlavlabels.chktyp, this.getNomType());
            elem.SetAttribute(xmlavlabels.numver, XMLAvElement.getxmlver(this.getTabVers()));
            XMLAvElement param = elem.CreateNode(xmlavlabels.param);
            // sauve les paramters

            XMLAvElement autosaveelem = param.CreateNode("autosave");
            autosaveelem.SetAttribute("idx", this.m_autoSaveResIdx);
            autosaveelem.SetAttribute("path", this.m_autoSaveResPath);
            autosaveelem.SetAttribute("suffix", this.m_autoSaveResSuffix);
            
            string formattxt = "jpg";
            switch (m_autoSaveResFormat)
            {case resuformat.csv: formattxt = "csv"; break;
             case resuformat.xls: formattxt = "xls"; break;
             case resuformat.tsv: formattxt = "tsv"; break; 
            }

            autosaveelem.SetAttribute("format", formattxt);
            
            XMLAvElement onmesureelem = param.CreateNode("onmesure");
            onmesureelem.SetAttribute("autosav", this.m_autosaveonsave ? "1" : "0");
            onmesureelem.SetAttribute("autoclear", this.m_clearautosave ? "1" : "0");

            XMLAvElement resuformatelem = param.CreateNode("resformat");
            resuformatelem.SetAttribute("decimal", this.m_ResNbDecim);
            resuformatelem.SetAttribute("separ", this.m_ResSepar);
            
            resuformatelem.SetAttribute("savlens", this.m_SavObj?1:0);
            resuformatelem.SetAttribute("savparam", this.m_SavParam?1:0);
            
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int LoadDisk(XMLAvElement elem)
        {//----  contenu standard de tous les bags  -----
         if (elem == null)
              return 2;

          string chktype ;
          if (elem.GetAttribute(xmlavlabels.chktyp, out chktype) == 0)
             return 1;
          
          if (chktype.CompareTo(this.getNomType()) != 0)
                return 1;


           // on lit le contenu de 'params'
           XMLAvElement param = elem.GetFirstElement(xmlavlabels.param);
           if (param == null)
                {// initialisation en cas d'echec 
                // ici on fait rien
                return 0;
                }

           try
           {
               XMLAvElement autosaveelem = param.GetFirstElement("autosave");
               autosaveelem.GetAttribute("idx", out m_autoSaveResIdx);
               autosaveelem.GetAttribute("path", out m_autoSaveResPath); // valeur pa defaut = celle actuelle
               autosaveelem.GetAttribute("suffix", out m_autoSaveResSuffix); // valeur par defaut = celle actuelle

               string formattxt;
               autosaveelem.GetAttribute("format", out formattxt, "xls");
               if (formattxt.CompareTo("csv") == 0)
                   m_autoSaveResFormat = resuformat.csv;
               if (formattxt.CompareTo("tsv") == 0)
                   m_autoSaveResFormat = resuformat.tsv;
           }
           catch (XmlAvException a)
           { }

            try{
            XMLAvElement onmesureelem = param.GetFirstElement("onmesure");
            string strbin;
            onmesureelem.GetAttribute("autosav", out strbin,"0");
            m_autosaveonsave = (strbin.CompareTo("1")==0);
            onmesureelem.GetAttribute("autoclear", out strbin,"0");
            this.m_clearautosave = (strbin.CompareTo("1") == 0);
            }
            catch (XmlAvException a)
            { };


            try
            {
                XMLAvElement resuformatelem = param.GetFirstElement("resformat");
                resuformatelem.GetAttribute("decimal", out m_ResNbDecim, 2);
                resuformatelem.GetAttribute("separ", out m_ResSepar, 0);
                int valint;
                resuformatelem.GetAttribute("savlens", out valint, 0);
                m_SavObj = (valint == 1);
                resuformatelem.GetAttribute("savparam", out valint, 0);
                m_SavParam = (valint == 1);
            }
            catch (XmlAvException a)
            { };


            return 0;
        }

        // interface icloneable
        // interface icloneable
        public object Clone()
        {
            paramsavres output = new paramsavres();
            output.m_autosaveonsave = this.m_autosaveonsave;
            output.m_autoSaveResFormat = this.m_autoSaveResFormat;
            output.m_autoSaveResIdx = this.m_autoSaveResIdx;
            output.m_autoSaveResPath = this.m_autoSaveResPath;
            output.m_autoSaveResSuffix = this.m_autoSaveResSuffix;
            output.m_clearautosave = this.m_clearautosave;
            output.m_SavParam = this.m_SavParam;
            output.m_SavObj = this.m_SavObj;  // enregistrer les objectifs dans le ficxhier resultat
            output.m_ResNbDecim = this.m_ResNbDecim; // nombre de decimales des resultats
            output.m_ResSepar = this.m_ResSepar; // separateur de decimales 0 = dafuat, 1 = point 2 = virgule
            output.m_EmptyResuStatus = this.m_EmptyResuStatus;
            return output;
        }
    }
#endregion

    //////////////////////////////////////////////////////////////////////////////

    #region paramsavimg
    /// <summary>
    /// 
    /// </summary>
    public class paramsavimg : IBagSavXml, ICloneable
    {// elements de donnees
        public string m_ResPath = ".";
        public string m_ResSuffix = "image";
        public int m_ResIdx = 0;
        public ImageFormat m_Format = ImageFormat.Jpeg;

        // interface ibagsavxml
        public string getNomType()
        {
            return (string)"paramsavimg";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] getTabVers()
        {
            string[] tabout = { "1.0" };
            return tabout;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int SaveDisk(XMLAvElement elem)
        {
            elem.SetAttribute(xmlavlabels.chktyp, getNomType());
            elem.SetAttribute(xmlavlabels.numver, (string)XMLAvElement.getxmlver(getTabVers())); // numero de version de ce bag

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int LoadDisk(XMLAvElement elem)
        {  // controle type et version
            string strtype;
            if (elem.GetAttribute(xmlavlabels.chktyp, out strtype) != 0)
                return 1;

            if (strtype.CompareTo(getNomType()) != 0)
                return 1;

            elem.GetAttribute(xmlavlabels.numver, out strtype); // numero de version de ce bag
            if (strtype.CompareTo("1.0") != 0)
                return 1;

            return 0;
        }

        // interface icloneable
        public object Clone()
        {
            paramsavimg output = new paramsavimg();
            output.m_ResPath = this.m_ResPath;
            output.m_ResSuffix = this.m_ResSuffix;
            output.m_ResIdx = this.m_ResIdx;
            output.m_Format = this.m_Format;
            return output;
        }

    }
    #endregion



    //////////////////////////////////////////////////////////////////////////////

    #region parammesure
    /// <summary>
    /// 
    /// </summary>
    public class parammesure : IBagSavXml, ICloneable
    {// elements de donnees
        public bool  m_SavOnValid = true;
        public bool m_ClearOnValid = true;
        public bool m_ClearOnQuit = false; // variable non sauvegardee
        public bool m_ClearedResu = true; // pas de resultats
        public bool m_saveParam=false; // sauvegarde de parametre
        public bool m_saveObj=true; // sauvegarde de objetif

        // interface ibagsavxml
        public string getNomType()
        {
            return (string)"parammesure";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] getTabVers()
        {
            string[] tabout = { "1.0" };
            return tabout;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int SaveDisk(XMLAvElement elem)
        {
            elem.SetAttribute(xmlavlabels.chktyp, getNomType());
            elem.SetAttribute(xmlavlabels.numver, (string)XMLAvElement.getxmlver(getTabVers())); // numero de version de ce bag
           
            XMLAvElement param = elem.CreateNode(xmlavlabels.param);
           
            XMLAvElement onmesureelem = param.CreateNode("onmesure");
            onmesureelem.SetAttribute("autosav", m_SavOnValid ?1:0);
            onmesureelem.SetAttribute("autoclear", m_ClearOnValid ?1:0);

            XMLAvElement onmesureeitem = param.CreateNode("saveitem");
           // onmesureelem.SetAttribute("param", m_SavParam ?1:0);
            //onmesureelem.SetAttribute("scale", m_saveObj ?1:0);

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int LoadDisk(XMLAvElement elem)
        {
            if (elem == null)
                return 2;

            string chktype;
            if (elem.GetAttribute(xmlavlabels.chktyp, out chktype) == 0)
                return 1;

            if (chktype.CompareTo(this.getNomType()) != 0)
                return 1;


            // on lit le contenu de 'params'
            XMLAvElement param = elem.GetFirstElement(xmlavlabels.param);
            if (param == null)
            {// initialisation en cas d'echec 
                // ici on fait rien
                return 0;
            }


            // et la on lit le bouzin
            XMLAvElement onmesureelem = param.CreateNode("onmesure");
            string strbin;
            onmesureelem.GetAttribute("autosav", out strbin, "0");
            this.m_SavOnValid = (strbin.CompareTo("1") == 0);

            onmesureelem.GetAttribute("autoclear", out strbin, "0");
            this.m_ClearOnValid = (strbin.CompareTo("1") == 0);

            XMLAvElement onmesureeitem = param.CreateNode("saveitem");
            onmesureelem.GetAttribute("param", out strbin, "0");
            this.m_saveParam = (strbin.CompareTo("1") == 0);
            onmesureelem.GetAttribute("scale", out strbin, "0");
            this.m_saveObj = (strbin.CompareTo("1") == 0);

            return 0;
        }

        // interface icloneable
        public object Clone()
        {
            parammesure output = new parammesure();
            output.m_ClearedResu = this.m_ClearedResu;
            //output.m_ClearOnQuit = this.m_ResSuffix.m_ClearOnQuit;
            //output.m_ClearOnValid = this.m_ResIdx.m_ClearOnValid;
            output.m_saveObj = this.m_saveObj;  // on enregistre les objectifs dans les res
            output.m_saveParam = this.m_saveParam; // on enregistre le parametre dans les res

            return output;
        }

    }
    #endregion
}
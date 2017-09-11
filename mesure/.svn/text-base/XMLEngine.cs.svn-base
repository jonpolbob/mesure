using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using mesure;



namespace mesure
{enum xmlavexceptiontype {noattrib, formaterror,badtype,missingnode,none};
    
    /// <summary>
    /// exception lgestion des fichiers XML
    /// </summary>
    class XmlAvException : Exception
    {
        xmlavexceptiontype numexception = xmlavexceptiontype.none;

        /// <summary>
        /// renvoie le type dexception
        /// </summary>
        public xmlavexceptiontype XmlAvType
        {
            get { return numexception; }
        }
        
        /// <summary>
        /// constructeur de l'exception
        /// </summary>
        /// <param name="i"></param>
        public XmlAvException(xmlavexceptiontype i)
        {
             numexception = i;
        }
    }
    
    /// <summary>
    /// element pour lire/enregistrer le xml
    /// </summary>
    public class XMLAvElement
    {// cree un node dans cet element
     XmlElement m_element;
     XmlDocument m_document;
     XMLAvElement m_parent;
     XmlNodeList m_liste=null;
     int m_indexElement = 0;

     /// <summary>
     /// checke l'en tete d'un element : verification de type et de version
     /// renvoie l'indice dans le tableau des types passes
     /// </summary>
     /// <param name="?"></param>
     /// <returns></returns>
        public static int chkelement(XMLAvElement elem, string nomtyp, string[] tabvers)
        {
         String nomconfig;
         String numver;
         
         try
         {
             elem.GetAttribute(xmlavlabels.chktyp, out nomconfig);
             elem.GetAttribute(xmlavlabels.numver, out numver); // numero de version de ce bag
         }
         catch (XmlAvException e)
         {
             if (e.XmlAvType == xmlavexceptiontype.noattrib)
                 return 1; // pas de section camera


             return 2; //autre erreur
         };

         // on verifie que c'est bien ce type la
         if (nomconfig.CompareTo(nomtyp) != 0)
             return 3; // pas bon type

         // reste a tester le numero de version
         // plus tard
         ///-------- fin du debut standard de lecture d'un xmlbag
         ///
         
         return 0; // pas de probleme
     }

     // renvoie la version courante de ce bag (=deriere version de la liste des versions
     /// <summary>
     /// renvoie la derniere version disponible pour le tableau tabver
     /// </summary>
     /// <param name="tabver">tableau de la liste des versions existantes</param>
     /// <returns>la version la plus recente (la derniere)</returns>
     public static string getxmlver(string[] tabver)
     { return tabver[tabver.Length - 1];
     }
     
      
        

    /// <summary>
    /// fonctions utiles pour le xml : renvoie l'index1 de stringver dans le tablea tabver 
    /// 0 si pas trouve
    /// </summary>
    /// <param name="tabver">tableau de strings contenant toutes les versions du bag</param>
    /// <param name="stringver">string de la version a trouver</param>
/// <returns></returns>
      public static int decodever(string []tabver, string stringver)
        {
         int i = 0;
         for (i = 0; i < tabver.Length; i++)
            if (tabver[i].CompareTo(stringver) == 0) // trouvé
               return i+1;
            
         return 0; // pas trouve
        }


        
        /// <summary>
     ///  positionne le texte de l'element
     /// </summary>
     /// <param name="?"></param>
     /// <returns></returns>
     public string Text
     {set {m_element.InnerText = value;}
      get {return m_element.InnerText;}
      }

        /// <summary>
        /// get le XML document
        /// </summary>
    public XmlDocument Document
        {get{
            return m_document;
            }
        }

        /// <summary>
        /// cree un xmlavelement a partir d'un element lu dans le doc
        /// </summary>
        /// <param name="ledoc"></param>
        /// <param name="Parent"></param>
        /// <param name="newelement"></param>
    public XMLAvElement(XmlDocument ledoc, XMLAvElement Parent, XmlElement newelement)
    {m_document = ledoc;
     m_element = newelement;
     m_parent = Parent;        // null si c'est le root
    }

      /// <summary>
      /// constructeur
      /// </summary>
      /// <param name="ledoc"></param>
      public XMLAvElement(XmlDocument ledoc)
      {
       m_document = ledoc;
       XmlElement element = m_document.CreateElement("root","");
       // ici on rajoutera les infs de version etc etc

       ledoc.AppendChild(element);
       
       m_parent = null; // pas de parent c'est le top
       m_element = element;       
      }
     

     
     /// constructeur en arborescence : recoit un tr sur le doc
     /// 
     public XMLAvElement(XMLAvElement leparent, string nomnode)            

     {
      m_document = leparent.m_document;

      XmlElement element = m_document.CreateElement(nomnode); //on cree l'element
      leparent.m_element.AppendChild(element); // on le colle comme enfant de son parent
      m_parent = leparent;
      m_element = element;         
     }


     
   #region lecture ecriture des string
   /// <summary>
/// 
/// </summary>
/// <param name="nomattrib"></param>
/// <param name="valeur"></param>
/// <returns></returns>
      public int SetAttribute(string nomattrib, string valeur)
     {
      m_element.SetAttribute(nomattrib, "",":"+valeur);
      return 1;
     }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nomattrib"></param>
        /// <param name="valeur"></param>
        /// <returns></returns>
    public int GetAttribute(string nomattrib, out string valeur)
    {return GetAttribute(nomattrib, out valeur, null);
    }


        /// <summary>
        /// le meme avec une valeur par defaut possible
        /// </summary>
        /// <param name="nomattrib"></param>
        /// <param name="valeur"></param>
        /// <param name="valdef"></param>
        /// <returns></returns>
    public int GetAttribute(string nomattrib, out string valeur, string valdef)
     {if (valdef != null)
         valeur = valdef;

      string txtout = m_element.GetAttribute(nomattrib);
        if (txtout.Length == 0)
            throw (new XmlAvException(xmlavexceptiontype.noattrib));
        // compare a : ou -temporairement - a S pour compatibilite avec qq versions 
        if (txtout[0].CompareTo(':') == 0 || txtout[0].CompareTo('S') == 0) // ca commence par un i
         valeur = txtout.Substring(1);
      else
          throw (new XmlAvException(xmlavexceptiontype.formaterror));

      return 1;
  }

  #endregion

  #region lescture-ecriture des double

  /// <summary>
        /// ecriture d'un double        
        /// </summary>
        /// <param name="nomattrib">nom attribut</param>
        /// <param name="valeur">valeur a ecrire</param>
        /// <returns></returns>
      public int SetAttribute(string nomattrib, double valeur)
      {string txtout = System.Convert.ToBase64String(BitConverter.GetBytes(valeur));
       
       m_element.SetAttribute(nomattrib, "","D"+txtout);
       return 1;
     }

        /// <summary>
        /// lit la valeur d'un attribut si pas trouve, la valeur est inchangee
        /// </summary>
        /// <param name="nomattrib">nom de l'attribut</param>
        /// <param name="valeur">valeur double a remplir</param>
        /// <returns></returns>
        /// 
        public int GetAttribute(string nomattrib, out double valeur)
        { return GetAttribute(nomattrib, out valeur,0,false);
        }
        /// <summary>
        /// lit la valeur double d'un attribut, si pas trouve  : ca lui donne la valeur valdef
        /// </summary>
        /// <param name="nomattrib">nom de l'attribut</param>
        /// <param name="valeur">double out a remplir</param>
        /// <param name="valdef">valeur par defaut</param>
        /// <returns></returns>
        public int GetAttribute(string nomattrib, out double valeur, double valdef)
        { return GetAttribute(nomattrib, out valeur,valdef,true);
        }

       /// <summary>
       /// get attribute prive : 
       /// </summary>
       /// <param name="nomattrib"></param>
       /// <param name="valeur"></param>
       /// <param name="valdef"></param>
       /// <param name="setvaldef"></param>
       /// <returns></returns>
 
      private int GetAttribute(string nomattrib, out double valeur, double valdef, bool setvaldef)
      {int valeurin=0;
      
       if (setvaldef)
          valeur = valdef;
   
      string txtin = m_element.GetAttribute(nomattrib); // lecture de la valeur
      if (txtin.Length ==0)
          throw (new XmlAvException(xmlavexceptiontype.noattrib));

      if (txtin[0].CompareTo('D')==0) // verifie le I au debut
        { byte [] tableau = System.Convert.FromBase64String(txtin.Substring(1)); // convertit le string en byte[]
          valeur = BitConverter.ToDouble(tableau,0); // convertit le byte[] en int
         }
      else
          throw (new XmlAvException(xmlavexceptiontype.formaterror));

       return 1;
   }

   #endregion
   
  #region lecture ecriture des int
   /// <summary>
      ///lecture ecritutre d'un int
    /// </summary>
    /// <param name="nomattrib"></param>
    /// <param name="valeur"></param>
    /// <returns></returns>
      public int SetAttribute(string nomattrib, int valeur)
      {string txtout = System.Convert.ToBase64String(BitConverter.GetBytes(valeur));
       
       m_element.SetAttribute(nomattrib, "","I"+txtout);
       return 1;
     }
        /// <summary>
        /// get attribute d'un int sans modifier valeur en cas d'echec
        /// genere xmlavexceptiontype.noattrib si le nom est pas trouve
        /// xmlavexceptiontype.formaterror si mauvais codage
        /// </summary>
        /// <param name="nomattrib"></param>
        /// <param name="valeur"></param>
        /// <returns>1 si ok</returns>
      public int GetAttribute(string nomattrib, out int valeur)
        {int dummy=0;
         return GetAttribute(nomattrib, out valeur, dummy, false);
        }
          
      /// <summary>
      /// get attribute d'un int 
      /// en cas d'echec :
      /// genere xmlavexceptiontype.noattrib si le nom est pas trouve
      /// valeur recoit defval
      /// </summary>
      /// <param name="nomattrib"></param>
      /// <param name="valeur"></param>
      /// <param name="defval"></param>
      /// <returns></returns>
    
     public int GetAttribute(string nomattrib, out int valeur, int defval)
        {
         return GetAttribute(nomattrib, out valeur,defval,true);
        }
        
      /// <summary>
      /// 
      /// </summary>
      /// <param name="nomattrib"></param>
      /// <param name="valeur"></param>
      /// <param name="defval"></param>
      /// <param name="usedef"></param>
      /// <returns></returns>
      private int GetAttribute(string nomattrib, out int valeur, int defval, bool usedef)
      {int valeurin=0;
             
        if (usedef) // utilisation de la valeur par defaut
           valeur = defval;

       string txtin = m_element.GetAttribute(nomattrib); // lecture de la valeur
          if (txtin.Length ==0)
              throw (new XmlAvException(xmlavexceptiontype.noattrib));

      if (txtin[0].CompareTo('E') == 0) // verifie le E  = entier non code
          {
           valeur = System.Convert.ToInt16(txtin.Substring(1), 10); // 
          } 
      else
        if (txtin[0].CompareTo('I')==0) // verifie le I au debut
        {
          byte[] tableau = System.Convert.FromBase64String(txtin.Substring(1)); // convertit le string en byte[]
          valeur = BitConverter.ToInt32(tableau,0); // convertit le byte[] en int
         }
      else
          throw (new XmlAvException(xmlavexceptiontype.formaterror));

       return 1;
     }

#endregion          

     #region lecture ecriture de la propriete texte des elements

     /// <summary>
     /// positionne la propriete texte d'un elemnt
     /// </summary>
     /// <param name="letext">texte</param>
     /// <returns>1 toujours </returns>
     public int SetText(string letext)
     {
         XmlText text = m_document.CreateTextNode(letext);
         m_element.AppendChild(text);
         return 1;

     }

     /// <summary>
     /// renvoie la propriete texte d'un elemnt
     /// </summary>
     /// <returns>renvoie le texte de l'element vide si pas de texte</returns>
     public string GetText()
     {
         return m_element.InnerText;
     }
     #endregion

     #region lecture ecriture des node

     /// <summary>
     /// cree un nouveau node sous cet element 
     /// </summary>
     /// <param name="nomnode"></param>
     /// <returns></returns>
     public XMLAvElement CreateNode(string nomnode)
     {
         XMLAvElement retour = new XMLAvElement(this, nomnode);
         return retour;
     }

        
     
     
     /// <summary>
     /// renvoie le premier element fils portant ce nom
     /// lance une exception xmlavexceptiontype.missingnode si node pas trouve
     /// </summary>
     /// <param name="Name">nom du node a trouver</param>
     /// <returns></returns>
     public XMLAvElement GetFirstElement(string Name)
     { int nbelems;
      return GetFirstElement(Name, out nbelems);
     }

        /// <summary>
    /// renvoie le premier element avec ce nom et positionne le nombre de'elements dans cette liste
    /// </summary>
    /// <param name="Name">nmo de l'element a trouver</param>
    /// <param name="nbelems">nbombre d'elments dans cette section</param>
    /// <returns>retourne null ou l'element</returns>
     
public XMLAvElement GetFirstElement(string Name, out int nbelems)
     {
         nbelems = 0;
         
         if (m_liste != null)
             m_liste = null;
         
         XmlElement lelement;

         m_liste = m_element.GetElementsByTagName(Name);
         m_indexElement = 0;
         nbelems = m_liste.Count;
         if (nbelems == 0)
             throw new XmlAvException(xmlavexceptiontype.missingnode);

         if (m_liste.Count != m_indexElement)
             lelement = (XmlElement)m_liste[m_indexElement++];
         else
             return null;

         XMLAvElement avElement = new XMLAvElement(m_document,this,lelement);
         return avElement;
        }

     /// <summary>
     ///  renvoie l'element suivant le precedent getnext ou getfirst et de meme nom
     /// lance une exception xmlavexceptiontype.missingnode si node pas trouve
     /// </summary>
     /// <param name="parent"></param>
     /// <param name="Name"></param>
     /// <returns></returns>
     public XMLAvElement GetNextElement()
     {
         try
         {
             if (m_liste.Count != m_indexElement)
                 return new XMLAvElement(m_document, this, (XmlElement)m_liste[m_indexElement++]);
             else
                 m_liste = null;
         }
         catch (Exception erreur)
         { m_liste = null; // probleme : on clear la liste
         }
         
         return null;
     }

     #endregion

 }


/// <summary>
/// XMLEngine : tout le stuff qui permet de creer un document xml et de l'enregistrer, le lire etc etc
/// </summary>
class XMLEngine
    {
        XmlDocument m_doc=null;
        XMLAvElement m_root = null;
    /// <summary>
    /// 
    /// </summary>
        XmlDocument Document
        {get {return m_doc;}
        }
    /// <summary>
    /// 
    /// </summary>
        XMLAvElement Root
        {
            get { return m_root; }
        }

    /// <summary>
    /// init le doc et cree la base de l'arborescence        
    /// </summary>
    /// <returns></returns>
        public XMLAvElement initDoc()
        {m_doc = new XmlDocument();
         XMLAvElement retour = new XMLAvElement(m_doc); // cree le root
         m_root = retour;
         return retour;
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nomfich"></param>
    /// <returns></returns>
        public int SaveDoc(string nomfich)
            {m_doc.Save(nomfich);
             return 0;
            }

        // charge un doc depuis le disque

    /// <summary>
    /// charge un doc sur le disque et renvoie son root
    /// </summary>
    /// <param name="nomfich"></param>
    /// <returns></returns>
        public XMLAvElement LoadDoc(string nomfich)
        {
            m_doc = new XmlDocument();
            try
            {
                m_doc.Load(nomfich);
            }
            catch
            { return null;  }// erreur : pas de fichier config par defaut
            XmlNodeList rootelem = m_doc.GetElementsByTagName("root");
            if (rootelem.Count != 1)
                m_root = null;
            else
                m_root = new XMLAvElement(m_doc, null, (XmlElement)rootelem[0]);
            
            return m_root;
        }
       
}
     
}

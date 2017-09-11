using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace mesure
{
    class ExceptionNoEtal : System.Exception 
    {// on implemente les 3 sortes d'exceptions recommandees
        public ExceptionNoEtal()
      {
      }
   public ExceptionNoEtal(string message)
      : base(message)
      {
      }
   public ExceptionNoEtal(string message, Exception inner)
      : base(message, inner)
      {
      }
 }


    public class Etalonnage 
    {
        private double dScalex = 1.0;
        private double dScaley = 1.0;
        private bool bEtalok = true;
        private double dRapxy = 1.0;
        private string sUnite;
        private string sName;


        //-----------------------------------------------
        //@brief constructeur par defaut : rapport ecran de 1;
        //-----------------------------------------------
        
      public Etalonnage()
        {
            dRapxy = 1.0;
        }

      //-----------------------------------------------
      //@brief constructeur comlet : rapport ecran a deffinir
      //-----------------------------------------------
      public Etalonnage(double rapxy)
       {
          dRapxy = rapxy;
        }

        public string Name
        {
            get { return sName; }   
            set { sName = value; }   
        }

        // enregistre l'etalonnage dans le moteur xml
        public int savedisk(XMLAvElement element)
        {int retour = 0; 
         element.SetAttribute("scalex",dScalex);
         element.SetAttribute("scaley",dScaley);
         element.SetAttribute(xmlavlabels.chktyp,"etal");
         element.SetAttribute("unit",this.sUnite);
         element.SetText(this.sName);
         return 0;
        }

    /// <summary>
    /// charge l'etalonnage avec les donnees incluses dans element
    /// </summary>
    /// <param name="element"></param>
    /// 
        public int LoadDisk(XMLAvElement element)
        {string txtin;
        try
        {
            element.GetAttribute(xmlavlabels.chktyp, out txtin);
            if (txtin.CompareTo("etal") != 0)
                throw new XmlAvException(xmlavexceptiontype.badtype); // mauvais foramt donnees
        }
        catch (XmlAvException e)
            { return 0;
            }
         double valin=0;
         string txtlu;
         try {
            element.GetAttribute("scalex",out valin);
            dScalex = valin;
            element.GetAttribute("scaley",out valin);
             dScaley = valin;
            sName = element.GetText();
         }catch (XmlAvException e)
         {
         }

         try
         {
             element.GetAttribute("unit", out txtlu);
             sUnite = txtlu;
         }
         catch (XmlAvException e)
         {
             if (e.XmlAvType == xmlavexceptiontype.noattrib)
                 sUnite = "mm";
         }
          return 1;
        }

        /// <summary>
        /// init de l'etalonnage, pour l'instant on fait tout en rapport d'ecran dde 1
        /// </summary>
        /// <param name="debx"></param>
        /// <param name="deby"></param>
        /// <param name="finx"></param>
        /// <param name="finy"></param>
        /// <param name="mesure"></param>
        /// <param name="unit"></param>
        public void InitEtal(double debx, double deby, double finx, double finy, double mesure, string unit)
        {double distpix = Math.Sqrt((finx-debx)*(finx-debx)+(finy-deby)*(finy-deby));
         bEtalok = false;
         dScalex=1.0;
         dScaley=1.0;

         if (distpix == 0)
             throw new ExceptionNoEtal();

          bEtalok = true;
          dScalex = mesure / distpix;
          dScaley = mesure / distpix;
          sUnite = unit;
        }


        /// <summary>
        ///renvoie un point en coordonees etalonnees a partir d'un point en coords source 
        /// </summary>
        /// <param name="ptin"></param>
        /// <returns></returns>
        public PointF Conversion(PointF ptin)
        {
            PointF ptout = new PointF(ptin.X, ptin.Y);
            float X=ptin.X;
            float Y = ptin.Y;

            Conversion(ptin.X, ptin.Y, ref X, ref Y);
            ptout.X = X;
            ptout.Y = Y;
            return ptout;
        }


        /// <summary>
        /// fait la conversion d'un point coordonnees source è-> coordonnees client
        ///  si le point est en dehors du client : ca renboie 2 et ca positionne les valurs max dans le point
        /// renvoie un point en coordonees etalonnees en coordonnees clipper
        /// renvoie 0 si un probleme. la valeur sortie est toujours mise a jour mais peut etre 
        /// fausee si ca depasse le cadre du clipper
        /// </summary>
        /// <param name="ptin"></param>
        /// <param name="ptclient"></param>
        /// <returns></returns>
        public int ConversionInv(PointF ptin, ref PointF ptclient)
        {
            float X = ptin.X;
            float Y = ptin.Y;

            int retour = ConversionInv(ptin.X, ptin.Y, ref X, ref Y);
            ptclient.X = X;
            ptclient.Y = Y;
            
            return retour;
        }

        /// <summary>
        ///  la meme fonction avec les coordonnees x,y du point        
        /// </summary>
        /// <param name="x">x du point en coords source</param>
        /// <param name="y">y du point en coords source</param>
        /// <param name="xunit">x du ploint en coords clipper</param>
        /// <param name="yunit">ydu point en coords clipper</param>
        /// <returns>0 si pas d'erreur, sinon des erreurs</returns>
        public int ConversionInv(double x, double y, ref float xunit, ref float yunit)
        {
            int erreur = 0;

            if (bEtalok)
            {
                xunit = (float)(x / dScalex); // calcul coversion source -> fenetre
                yunit = (float)(y / dScaley);
                
                // est ce qu'on depasse les llimites de la fenetre ?
                /*if (xunit < 0)
                {
                    xunit = 0;
                    erreur = 2;
                }
                
                if (xunit > m_maxx)
                {
                    xunit = m_maxx;
                    erreur = 2;
                }
                
                if (yunit > m_maxy)
                {
                    yunit = m_maxy;
                    erreur = 2;
                }

                if (yunit <0)
                {
                    yunit = 0;
                    erreur = 2;
                }
                */
                return erreur; 
            }
            else
            {
                xunit = (float)x;
                yunit = (float)y;
                return 1; // erreur : etalonnage pas fait
            }
        }


        /// <summary>
        /// converti un pt x,y en un point d'unites x,y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="xunit"></param>
        /// <param name="yunit"></param>
        /// <returns></returns>
        public int Conversion(double x, double y, ref float xunit, ref float yunit)
        {if (bEtalok)
            {xunit = (float)(x*dScalex);
            yunit = (float)(y * dScaley);
             return 0; // pas d'erreur
            }
        else
        {
            xunit = (float)x;
            yunit = (float)y;
            return 1; // erreur : etalonnage pas fait
            }
        }


        /// <summary>
        /// brief renvoie la legende des unites
        /// </summary>
        public string Unite
            {get {
                if (bEtalok)
                    return sUnite; 
                else 
                    return "";
                }
                set 
                { sUnite = value; }
            }

        }    
}

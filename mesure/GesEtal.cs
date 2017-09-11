// fichier correspondant aux etalonnages de la camera

using System;
using System.Drawing;
using System.Threading;
using VideoSource;
using System.Windows.Forms;
using System.Collections;

namespace mesure
{
    /// <summary>
    /// classe gerant les etalonnages
    /// les etalonnages etaient dans la camera maismaintenant ils sont sortis pour eviter l'effacement des etalonnages en cas de 
    /// destruction/reconstruction de camera
    /// </summary>
    public class GesEtals
    {
      private ArrayList m_Etalonnages=new ArrayList(); //
      private Etalonnage m_curetalonnage; //
      private ClippingEngine m_Clipper = new ClippingEngine();


      /// <summary>
      /// clipping engine utilise par la camera : mis a jour a chaque modif du clipping de la camera
      /// </summary>
      public ClippingEngine Clipper
        {
         get { return m_Clipper; }
        }

      /// <summary>
      /// 
      /// </summary>
      public ArrayList ListEtals
        {
          get { return m_Etalonnages; }
        }

      /// <summary>
      /// renvoie le valeur etalonnee d'un point exprime en coords source
      /// attention, il faudrait renvoyer des messages d'erreur ici
      /// </summary>
      /// <param name="pointsrc">point en ccord source</param>
      /// <returns>point en coordonnees etalonnees</returns>
      public PointF etalonne(PointF pointsrc)
        {
            if (m_curetalonnage != null)
                return m_curetalonnage.Conversion(pointsrc);
            else
            {// pas d'etalonnage : on renvoie les coords source
             PointF retour = new PointF(pointsrc.X, pointsrc.Y);
             return retour;
            }
        }

      /// <summary>
      /// renvoie le valeur clipping d'un point en coordonnees source 
      /// </summary>
      /// <param name="pointsrc">point en coordonnees etalonnees</param>
      /// <param name="pointCli">point en coordonnees source</param>
      /// <returns>0 si pas d'erreur</returns>
      public int invEtalonne(PointF pointsrc, ref PointF pointCli)
        {
            if (m_curetalonnage != null)
                return m_curetalonnage.ConversionInv(pointsrc,ref pointCli);
            else
            {// pas d'etalonnage : on convertit en coordonnees source
                pointCli.X = pointsrc.X * m_Clipper.VisuSize.Width/ m_Clipper.SourceRect.Width;
                pointCli.Y = pointsrc.Y * m_Clipper.VisuSize.Height / m_Clipper.SourceRect.Height;
                return 0; // pas d'erreur mais pas d'etalonnage
            }
        }

      /// <summary>
      /// calcule une echelle possible l'argument est une lonngueur en pixels qui sera celle utilisse a 10% pres pour dessiner une echelle correcte
      /// </summary>
      /// <returns></returns>
      public Echelledrawing GetCurrentEchelle(int nbpixels)
        {// on commence par cacluler la longueur reelle du trait de pixels
        
            PointF OrgCli = new PointF(0.0F,0.0F);
            PointF FinCli = new PointF(0.0F,(float)nbpixels);

            PointF OrgSrc = this.etalonne(OrgCli);
            PointF FinSrc = etalonne(FinCli);
            double SrcLen = Math.Sqrt((double)((OrgSrc.X - FinSrc.X) * (OrgSrc.X - FinSrc.X) + (OrgSrc.Y - FinSrc.Y) * (OrgSrc.Y - FinSrc.Y)));
            
            // a partir de cette longueur, on cherche une longueur voisine qui soit un multiple qui de qqch
            // comme on le fait pour les rulers
            int nbticks =0;
            double RealLen=0.0;
            
            // modifie la longueur relle de l'echelle en coords source
            // pour que ca fasse une valeur affichee convenable
            // et le nombre de ticks correspondant a cette echelle

            nbticks = genTicks(SrcLen , ref RealLen);

            // on recalcule le longueur en pixels dansl'autre sens
            OrgSrc.X=0.0F;
            OrgSrc.Y=0.0F;
            FinSrc.Y = 0.0F;
            FinSrc.X = (float)RealLen;
            
            // on ramene cette longueur en pixels pour savoir quel trait tirer
            invEtalonne(OrgSrc,ref OrgCli);
            invEtalonne(FinSrc,ref FinCli);

            int nbpixreal = (int)(Math.Sqrt((OrgCli.X - FinCli.X)*(OrgCli.X - FinCli.X) + (OrgCli.Y - FinCli.Y)*(OrgCli.Y - FinCli.Y)));

            // on cree l'echelle avec tout ca.
            Echelledrawing leretour = new Echelledrawing(nbpixreal, nbticks, RealLen);
            
            return leretour;
        }

      /// <summary>
      /// calcule les ticks de l'etalonnage pour une longueur donnee
      /// </summary>
      /// <param name="tableau"></param>
      /// <returns></returns>
      private int genTicks(double RealRequested , ref double RealLen)
        {// on regarde le min et le max et on regarde comment faire des marques qui ressemblent a qq chose
            double normaldelta = RealRequested * 1.10; // on s'autiorise 10% de plus que la longueur demandee
            double facteur = 1.0;
            int nbticks=0;

            if (normaldelta == 0)
                return 0;

            // on cherche un facteur qui ramene delata entre O et 10
            while (normaldelta >= 10)
            {
                facteur *= 10;
                normaldelta = normaldelta / 10;
            }

            while (normaldelta <= 1)
            {
                facteur *= 10;
                normaldelta /= 10;
            }

          // ici on a trouve un facteur multipliccatif qui ramene la difference entre 1 et 10
          //etdouble tick; normaldelta qui es cette difference entre 0 et 1
          double tick = .2;
            
          // on tronque la valeur en forcant un peu sur le plus grand : au lieu de rajouter .5 pour passer a celui d'au dessus, on rajoute .8
          // ca tronque au superieur a partir de .2
          int trunkdelta = (int)Math.Truncate(normaldelta + .8);

          // on caclule le nombre de ticks
          // et on force certaines longueurs qui sont des multiples pas pratiques 
            
          switch (trunkdelta)
                {
                    case 1: nbticks = 1; trunkdelta = 1; break;
                    case 2: nbticks = 2; trunkdelta = 2; break;
                    case 3: nbticks = 3; trunkdelta = 3; break;
                    case 4: nbticks = 2; trunkdelta = 4; break;
                    case 5: nbticks = 1; trunkdelta = 5; break;
                    case 6: nbticks = 3; trunkdelta = 6; break;
                    case 7: nbticks = 4; trunkdelta = 8; break; // on n'accepte pas le 7
                    case 8: nbticks = 4; trunkdelta = 8; break;
                    case 9: nbticks = 5; trunkdelta = 10; break; // on n'accetpe pas le 9
               }

            RealLen = trunkdelta * facteur;

            return nbticks;
        }

      /// <summary>
      /// renvoie true si cet etalonnage est le courant
      /// </summary>
      /// <param name="nometal">nom de l'etalonnage</param>
      /// <returns>bool true si c'est le courant</returns>
      public bool iscurrentetal(string nometal)
        {
            foreach (Etalonnage etal in m_Etalonnages)
            {
                if (etal == m_curetalonnage)// on a trouve le courant : on inserera ici
                    if (etal.Name == nometal)
                       return true;
            }
            return false;
          }
        
      /// <summary>
      /// Ajoute un etalonnage apres l'etalonnage courant
      /// </summary>
      /// <param name="newetal">nexetal : nouvel etalonnage a inserer</param>
      /// <returns>false si erreur</returns>
      public bool InsertEtal(Etalonnage newetal)
        {int index=0;
         int toinsert = -1;

         foreach(Etalonnage etal in m_Etalonnages)
         {
          if (etal == m_curetalonnage)// on a trouve le courant : on inserera ici
                toinsert = index;                
            
             index++;
            
             if (etal.Name == newetal.Name) //un homonyme !!!
                return false; // erreur deux etals homonymes
            }
            
         // pas d'etal courant trouve : on insere a la fin
         if (m_curetalonnage == null)
            toinsert = index;
         else
            toinsert++; // on ajoute PARES le courant
            
         m_Etalonnages.Insert( toinsert,newetal);

         // le nouveau devient le courant
         m_curetalonnage = newetal;
           
         return true;
        }


        
      /// <summary>
      /// elemine l'etalonnage courant de la liste des etlaonnages 
      /// et revient en mode pixel
      /// </summary>
      /// <returns></returns>
      public int DelCurEtal()
        {Etalonnage todel = m_curetalonnage;
         m_curetalonnage = null;

         if (todel!=null)
            DelEtal(todel.Name);

         return 0;
        }
        
      /// <summary>
      /// 
      /// </summary>
      /// <returns></returns>
      public int ClearEtals()
        {
            m_Etalonnages.Clear();
            m_curetalonnage = null;
            return 0;
        }

      /// <summary>
      /// destruction de l'etalonnnage de nopm etalname 
      /// </summary>
      /// <param name="etalname"></param>
      /// <returns></returns>
      public bool DelEtal(string etalname)
        {int i=0;
         bool found =false;

         foreach (Etalonnage etal in m_Etalonnages)
            {if (etal.Name == etalname)
                {found = true;
                 break;
                }

             i++;
            }
         
            //-----------------------
            //-----------------------

            if (!found)
                return false; // delete pas fait
                
            if (m_Etalonnages[i].Equals(m_curetalonnage))
                m_curetalonnage =null; // on vire l'etalonnage courant

            m_Etalonnages.RemoveAt(i); // on enelve cet etalonnage de la liste
            
            return true; // delete ok
        }

      /// <summary>
      ///renvoie la liste des etalonnages dispos 
      /// </summary>
      /// <param name="indexcur"></param>
      /// <returns></returns>
      public ArrayList GetListEtal(ref int indexcur)
        {ArrayList liste = new ArrayList();
         indexcur=-1;
            
         int i=0;

         foreach (Etalonnage etal in m_Etalonnages)
            {liste.Add(etal.Name);
             if (etal == m_curetalonnage)
                 indexcur =i;
             i++;
            }
            
          return liste;
        }

      /// <summary>
      /// renvoie le nom de l'etalonnage courant
      /// </summary>
      /// <returns></returns>
      public string getCurEtalName()
        {if (m_curetalonnage!=null)
            return m_curetalonnage.Name;
         else
            return "pixels";
        }

        /// <summary>
        /// renvoie le nom de l'etalonnage courant
        /// </summary>
        /// <returns></returns>
        public string getCurEtalUnit()
        {
            if (m_curetalonnage != null)
                return m_curetalonnage.Unite;
            else
                return "pixels";
        }

      /// <summary>
      /// positionne l'etalonnage courant sur l'etalonnage dont on met le nom
      /// si l'etalonnage n'est pas trouve : rien ne change
      /// </summary>
      /// <param name="etalname"></param>
      /// <returns></returns>
      public bool SetCurEtal(string etalname)
        {
            if (etalname == null)
            {
                m_curetalonnage = null; // pas d'etalonnage si on est en pixels
                return true;
            }

            foreach (Etalonnage etal in m_Etalonnages)
            {if (etal.Name == etalname)
                {
                m_curetalonnage = etal;
                 return true;
                }
            }
       
         return false; // commutation a rate
        }
      
    }
    
    /// <summary>
    /// classe contenant les elements de dessin d'une echelle correcte
    /// </summary>
    public class Echelledrawing
    { int m_nbpix = 0;
      int m_nbticks = 0;
      double m_reallen =0.0;
        string m_unit;

      /// <summary>
      /// nbpix = nb de pixels du trait d'echelle
      /// reallen = longueur reelle       
      /// </summary>
      /// <param name="nbpix"></param>
      /// <param name="nbticks"></param>
      /// <param name="reallen"></param>
      public Echelledrawing(int nbpix, int nbticks, double reallen)
        {m_nbpix = nbpix;
         m_nbticks = nbticks;
         m_reallen = reallen;
        }

      /// <summary>
      /// 
      /// </summary>
      public string unitname
        {
            get
            { return m_unit; }
        }
    
      /// <summary>
      /// 
      /// </summary>
      public int nbpix
        {get 
            {return m_nbpix;}
         }

      /// <summary>
      /// 
      /// </summary>
      public int nbticks
        {get 
            {return m_nbticks;}
         }

      /// <summary>
      /// 
      /// </summary>
      public double reallen
        {get 
            {return m_reallen;}
         }

    }
}
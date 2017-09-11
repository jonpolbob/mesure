using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;

namespace mesure
{	
 /// <summary>
 /// delegate pour la conversion de client a source
 /// </summary>
 /// <param name="sender">controle emettant l'event</param>
 /// <param name="e">argument contenant les coords en vis(x/y) et qui recevra les resultats caclules en cal(x/y)</param>
   
 public delegate void MesureEventHandler(object sender, ref MesureEventArgs e);
    
    /// <summary>
    /// arguments pour la conversion de client a resultats calibres et vice versa
    /// </summary>
 public class MesureEventArgs : EventArgs
  { private double inputX;      /// x a covertir
      private double inputY;    /// y a convertir
      private double outputX;   /// resultat X converti
      private double outputY;   /// resultata Y converti
      private bool convAvail;   /// la conversion etait possible (un etalonnage existait)
      private bool scaleok;     /// la conversion est dans la gamme de conversion possible
      private int codeerreur=0; /// il y a une erreur qui fait que le resutat sorti n'est pas explitable

     public int CodeErreur
     { get { return codeerreur;}
         set { codeerreur = value;}
     }
     
     /// <summary>
     /// constructuer remplissant les points client de la structure
     /// initialise la structure avec les points client a convertir.
     /// les points calibres sont init egaux aux points clients
     /// </summary>
     /// <param name="X">x a convertir</param>
     /// <param name="cliy">y a convertir</param>
     public MesureEventArgs(double X, double Y)
        {this.inputX = X;
         this.inputY = Y;
         convAvail =false;
         scaleok = false;
         outputX = X; 
         outputY = Y;
        }

        /// <summary>
        /// get le point a convertir
        /// </summary>
        public PointF inputPt
        {get
            {
                return new PointF((float)inputX, (float)inputY);
             }
        }

        
        /// <summary>
        /// get/set le point converti 
        /// le set est fait pas copie des x,y
        /// </summary>
        public PointF outPt
            {get
                {
                 return new PointF((float)outputX, (float)outputY);
                }
            set
                {outputX = (double)value.X;
                 outputY = (double)value.Y;
                }
            }


        public void out2inp()
        {
            inputX = outputX;
            inputY = outputY;
        }
     /// <summary>
     /// renvoie si la calibration est ok
     /// </summary>
    
     public bool CalOk
        {
            set { convAvail = value; }
            get { return convAvail; }        
        }

     /// <summary>
     /// renvoie si l'echelle est ok
     /// </summary>
        public  bool ScaleOk
        {set {scaleok = value;}
            get {return scaleok;}
        }
    }

    /// <summary>
    /// elements pour le retour des resultats du mesureur vers l'affichage ou les tableaux de resultats
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
     public delegate void ResuEventHandler(object sender, ref ResuEventArg e);

    
    /// <summary>
    /// class permettant la communication des resultats entre le mesureur et l'appli
    /// </summary>
     public class ResuEventArg
     {
         ArrayList m_IDResus = new System.Collections.ArrayList();
         bool m_bfinal;
         ICalculator lecalculateur;


         public ICalculator Calculator
         { get { return lecalculateur; } 
         }
         /// <summary>
         ///constructeur, dit si c'est un envoi de resultat final ou en cours de trace 
         /// </summary>
         /// <param name="final"></param>
         public ResuEventArg(ICalculator calculateur,bool final) // avec argument : final ou pas
         {
             lecalculateur = calculateur;
             m_bfinal = final; 
         }

         /// <summary>
         /// constructeur de base, pour des resultats non finaux
         /// </summary>
         public ResuEventArg(ICalculator calculateur) // constructeur de base 
         {
             lecalculateur = calculateur;             
             m_bfinal = false;
         }
        
         /// <summary>
         /// arraylist des id de resultats proposes par la mesureur
         /// </summary>
         public ArrayList IDResus
         { get { return m_IDResus;}
         }
         
         
         /// <summary>
         /// get sile resultat de ce resuarg est final ou non
         /// </summary>
         public bool isFinal
         {
             get { return m_bfinal; }
         }

         /// <summary>
         /// ajoute un ID a la liste des resultats proposes
         /// </summary>
         /// <param name="ID"></param>
         /// <returns></returns>
         public int AddResu(int ID)
         {
             m_IDResus.Add(ID);
             return 0;
         }
         
     }
}


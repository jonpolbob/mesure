using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

// il va y avoir un probleme d'init si on demarre le truc avec la souris appuyee....

namespace mesure
{
    /// <summary>
    /// implemente Icalculator dans une classe mesure verticale
    /// </summary>
        class LineCalculator : GenericCalculator, ICalculator
        {
            //public event MesureEventHandler VisuToSrcConv;
            //public event MesureEventHandler SrcToVisuConv;
            //public event MesureEventHandler SrcToEtalConv;
            //public event ResuEventHandler ToResu;
      
            bool bMouseDown = false; /// dit si on est en mode souris appuyee ou non
            float prvx;
            float prvy;
            private float m_dMesDebX; // position x point deb etalonne
            private float m_dMesDebY;
            private float m_dMesFinX;
            private float m_dMesFinY;
            protected float m_dSrcDebX; // coords source des deux points
            protected float m_dSrcDebY;
            protected float m_dSrcFinX;
            protected float m_dSrcFinY;
            
            protected float m_dVisDebX; // pt x a l'ecran
            protected float m_dVisDebY; // pt y a l'ecran
            
            Control leclient=null;
            Cursor m_curs = null;

            // resultats
            double m_longueur = 0;

            /// <summary>
            /// constructeur par defaut
            /// </summary>
            public LineCalculator()
            { }


            public override void repaint()
            {}

            /// <summary>
            /// fonction traitant les changement de taille de la visu
            /// appelle par l'evenemetn de la visu changement de taille
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// EventHandler
            public void OnChgVisuSize(object sender, EventArgs e)
            { }
       
            /// <summary>
            /// sauve les parametres dans l'arobrescence XML de sauvegarde
            /// </summary>
            /// <param name="elemnt">element a remplir </param>
            /// <returns></returns>
            public int SaveDisk(XMLAvElement elemnt)
            { return 0;
            }


            /// <summary>
            /// charge les parametres dans l'element xml de sauvegarde
            /// </summary>
            /// <param name="elemnt">element a lire</param>
            /// <returns></returns>
            public int LoadDisk(XMLAvElement elemnt)
            { return 0;
            }

            /// <summary>
            /// rend ce calcuateur actif pour les mesures actuelles
            /// </summary>
            /// <param name="control"></param>
            /// <returns></returns>
            public int SetActive(Control client)
            {
                m_curs = new Cursor(GetType(), "Resources.periscop.cur");

                
                lepanel.Releasegraph(lepanel.GetGraph(true)); // efface l'ecran

                leclient = client;
                leclient.Cursor = m_curs;
                return 0;
            }

            /// <summary>
            /// renvoie la valeur et l'unite d'une mesure du calculateur
            /// </summary>
            /// <param name="ID">ID demandee</param>
            /// <param name="unit">unite de cette mesure</param>
            /// <returns>valeur de la mesure</returns>
            
            
            /// <summary>
            /// renvoie la legende a utiliser pour cette mesure 
            /// </summary>
            /// <param name="ID"></param>
            /// <returns></returns>
            public virtual string GetLegende(int ID)
            {
                return "longueur";
            }

            
            
            /// <summary>
            /// traite un changement de variable de la camerawindow (mouvement souris etc etc
            /// </summary>
            /// <param name="user"></param>
            /// <param name="e"></param>
            /// <returns>1 si la mesure continue, 0 si la mesure est finie</returns>
            public virtual int OnChangeVariable(object user,ChangeVariable e)
            {// on cherche un graphic utilisable
                
             if (e.num == ChgVarNum.none)
                 return 1;
                
                // effece si mouse down
             Graphics graph = lepanel.GetGraph(e.num == ChgVarNum.mousedown); // graphics sur le palnel

             switch (e.num)
                {case ChgVarNum.mousedown :
                    onmousedown(e,graph);
                    break;
             
                 case ChgVarNum.mousmov :// suite de trace
                    onmousemove(e, graph);
                    break;
                

                 case ChgVarNum.mousup : // finde trace
                    onmousup(e, graph);
                    break;

                 case ChgVarNum.sendresu : // envoi resultat
                    ResuEventArg resargs = sendresuline(true); // final
                    //if (ToResu != null)
                    OnToResu(this, ref resargs);
                    break;
                }
                
             // on libere le graph
             lepanel.Releasegraph(graph);

             return 1; // on continue a mesure ca s'arrete avec un stop
            }



            

            /// <summary>
            /// action sur fin clic souris dans fenetre video
            /// </summary>
            /// <param name="e"></param>

            #region mouvements souris
            /// <summary>
            /// action sur clic souris dans la fenetre video
            /// </summary>
            /// <param name="e"></param>
            /// <param name="graph"></param>
            private void onmousedown(ChangeVariable e, Graphics graph)
            {
                bMouseDown = true;
                m_dVisDebX = prvx = (float)e.valeurx;  // coordonnees a l'ecran
                m_dVisDebY = prvy = (float)e.valeury;

                // on sauvegarde les coords source des points
                MesureEventArgs mesargs = new MesureEventArgs(prvx, prvy);
                OnVisuToSrcConv(this, ref mesargs);
                m_dSrcDebX = m_dSrcFinX = (float)mesargs.outPt.X; // sauvees dans m_dsrcdebx/y
                m_dSrcDebY = m_dSrcFinY = (float)mesargs.outPt.Y;
                
/*                mesargs.out2inp(); /// 2 eme conversion 
                this.SrcToEtalConv(this, ref mesargs);
                m_dMesDebX = mesargs.outPt.X; // position x point deb etalonne
                m_dMesDebY = mesargs.outPt.Y; // position x point deb etalonne
  */              
                // on dessine la ligne : en fait on dessine rien ya rien a dessiner tant que ca a pas bougé
                graph.DrawLine(new Pen(Color.FromArgb(254, m_Color), 2), (float)m_dVisDebX, (float)m_dVisDebY, (float)prvx, (float)prvy);
            }

            /// <summary>
            /// action sur mouvement souris dans fenetre video
            /// </summary>
            /// <param name="e"></param>
            private void onmousemove(ChangeVariable e, Graphics graph)
            {
                if (!bMouseDown)
                    return;

                // on efface l'ancienne ligne
                Color EffColor = Color.FromArgb(255, Color.Magenta);
                graph.DrawLine(new Pen(EffColor, 2), (float)m_dVisDebX, (float)m_dVisDebY, (float)prvx, (float)prvy);

                // on prend la nouvelle coord de point courant
                prvx = (float)e.valeurx;
                prvy = (float)e.valeury;

                // calcul des coords source
                MesureEventArgs mesargs = new MesureEventArgs(prvx, prvy);
                OnVisuToSrcConv(this, ref mesargs);
                m_dSrcFinX = mesargs.outPt.X; // points en coordonnees source sauvegardees pour l'etalonnage
                m_dSrcFinY = mesargs.outPt.Y;

                mesargs.out2inp(); // on renvoie la sortie dans l'entree pour calcul de coords etalonnees
                OnSrcToEtalConv(this, ref mesargs);
                double calx2 = mesargs.outPt.X;
                double caly2 = mesargs.outPt.Y;

                // on refait la mme operation pour les coords origine
                // on recalcule a chaque fois au cas ou un zoom en cours de trace aurait fait qqch
                
                mesargs = new MesureEventArgs(m_dSrcDebX, m_dSrcDebY); // coords sourdce du debut
                OnSrcToEtalConv(this, ref mesargs);
                double calx1 = mesargs.outPt.X; // coods etalonnnees du debut
                double caly1 = mesargs.outPt.Y;

                // on calcule juste m_ longueur mais on pourrait faire differents calculs suivant le setting
                // et n'evoyer que le calcul chois dans la dialogue de configuration
                m_longueur = Math.Sqrt((calx2 - calx1) * (calx2 - calx1) + (caly2 - caly1) * (caly2 - caly1));

                // envoi de resultat 105 a l'affichage temps reel
                ResuEventArg resargs = new ResuEventArg(this,false); // pas resultat final
                resargs.AddResu(105); // on ne rajoute que le 105 (distance) en cours de trace par le toresu
                /// mais c'est peut etre debile : si le toresu met a jour le tableau des resultats intercatif 
                /// il n'y aura que le 101 mis a jour


                //on declanche ToResu qui recoit le handler des fenetres ayant as tenir a jour un affichage de Label valeur
                //if (ToResu != null)
                OnToResu(this, ref resargs); // envoie de la distance a l'afficahge interactif

                // dessin de la ligne
                //m_dVisDebX  est inchangé mais il faudra le recalculer quand il y aura le zoom
                graph.DrawLine(new Pen(Color.FromArgb(254, m_Color), 2), (float)m_dVisDebX, (float)m_dVisDebY, prvx, prvy);
            }

            /// <summary>
            /// envoi de reultats a la fin de la mesure
            /// </summary>
            /// <param name="e"></param>
            /// <param name="graph"></param>
            private void onmousup(ChangeVariable e, Graphics graph)
            {
                if (bMouseDown) // inutile mais pourtatn parfois ca sert....
                {// on efface la line d'avant
                    Color color = Color.FromArgb(255, Color.Magenta);
                    graph.DrawLine(new Pen(color, 2), (float)m_dVisDebX, (float)m_dVisDebY, (float)prvx, (float)prvy);

                    // mise a jour de prvx et prvy avec les nouvelles coords souris
                    prvx = (float)e.valeurx;
                    prvy = (float)e.valeury;

                    // on dessine le trait sur les nouvelles coordonnees
                    graph.DrawLine(new Pen(Color.FromArgb(254, m_Color), 2), (float)m_dVisDebX, (float)m_dVisDebY, prvx, prvy);
                }
                bMouseDown = false;

                // calcul de la longueur etalonnee
                MesureEventArgs mesargs = new MesureEventArgs(m_dSrcDebX, m_dSrcDebY);
                OnSrcToEtalConv(this, ref mesargs);
                double calx1 = mesargs.outPt.X;
                double caly1 = mesargs.outPt.Y;

                mesargs = new MesureEventArgs(m_dSrcFinX, m_dSrcFinY);
                OnSrcToEtalConv(this, ref mesargs);
                double calx2 = mesargs.outPt.X;
                double caly2 = mesargs.outPt.Y;

                // on sauvegarde ces valeurs etalonnees pour les sortir enr resultats
                // ca a deja ete calcule en fait lors du mousedown et mousemove
                // mais au debut ca le faisaitr pas... on le sauvearde que maintenant dans les vars
                m_dMesDebX = (float)calx1;
                m_dMesDebY = (float)caly1;
                m_dMesFinX = (float)calx2;
                m_dMesFinY = (float)caly2;

                m_longueur = Math.Sqrt((calx2 - calx1) * (calx2 - calx1) + (caly2 - caly1) * (caly2 - caly1));

                /// envoi de resultat instantané
                ResuEventArg resargs = newresuline(false); // genere un ligne de resultat pas final
                //if (ToResu != null)
                    OnToResu(this, ref resargs);

            }
            #endregion

            #region gestion des resultats

            /// <summary>
            /// renvoie des information sur la gestion de l'unite d'une messure
            /// </summary>
            /// <param name="ID">ID de la mesure</param>
            /// <param name="flag">flags de traitement de la mesure</param>
            /// <returns>nom de la mesure</returns>
            public virtual string GetUnit(int ID, ref mesure.etalflagstyp flag)
            {
                flag = etalflagstyp.none;
                return "mm";
            }



            /// <summary>
            /// genere un eventarg correct pour tous les resultats d'une mesure
            /// renvoie uniquement un resultat : la longueuru
            /// </summary>
            /// <param name="final">a true si c'est une mesure finale</param>
            /// <returns>resueventarg pret a etre passe a l'event</returns>
            protected virtual ResuEventArg newresuline(bool final)
            {
                ResuEventArg resargs = new ResuEventArg(this, final); // resultat final
                resargs.AddResu(105); // sortir un resultat 105
                return resargs;
            }

            /// <summary>
            /// fonction construisant la liste des resultats disponibles ou actuellement selectionnes
            /// pour le resultat final
            /// </summary>
            /// <param name="final"></param>
            /// <returns></returns>
            protected virtual ResuEventArg sendresuline(bool final)
            {
                ResuEventArg resargs = new ResuEventArg(this, true); // resultat final
                resargs.AddResu(105); // sortir un resultat 101
                return resargs;
            }


            /// <summary>
            /// genere la liste des resultats pouvant etre generes pour preparer le tableau des resultats de rsultat final
            /// on ne renvoie que la longueur qui est 105 ici
            /// </summary>
            /// <returns></returns>
            public ArrayList GetListResu() 
                    {ArrayList laliste = new ArrayList();
                     laliste.Add(105); // longueur etalonnee
                     return laliste;
                    }

            public virtual double GetValeur(int ID, int unit)
            {
                switch (ID)
                {
                    case 105:
                        return m_longueur; // longueur mesuree
                        break;
                }
                return 0;
            }

            #endregion

        }



    /// <summary>
    /// calculator utilise pour le dessin de al ligne d'etalonnage
    /// comme la ligne mais renvoie les x y des extremites 
    /// </summary>
    class EtalCalculator : LineCalculator
      { 
        /// <summary>
        /// construction des resultats : surcharge de linecalcultaor pour sortir les xy des deux extremites en coords source
        /// le calculateur d'etalonnage renvoie 4 resultats
        /// </summary>
        /// <param name="final"></param>
        /// <returns></returns>
            protected override ResuEventArg newresuline(bool final)
            {
               ResuEventArg resargs = new ResuEventArg(this,final); // resultat final
               resargs.AddResu(101); // xdeb source
               resargs.AddResu(102); // ydeb
               resargs.AddResu(103); // xfin
               resargs.AddResu(104); // yfin
               
               return resargs;
            }

            /// <summary>
            /// renvoie la valeur de la variable de ID id
            /// en ligne etalonnage, on renvoie les 4 valeurs x et y de debut et fin de la ligne
            /// l'etalonnage ne renvoie que les coords pixel source
            /// </summary>
            /// <param name="ID"></param>
            /// <param name="unit"></param>
            /// <returns></returns>
            public override double GetValeur(int ID, int unit)
            {
                switch (ID)
                {
                    case 101:
                        return m_dSrcDebX; // position x source
                        break;

                    case 102:
                        return m_dSrcDebY; // position y source
                        break;

                    case 103:
                        return m_dSrcFinX; // source
                        break;

                    case 104:
                        return m_dSrcFinY; // source
                        break;

                    case 105:
                        return Math.Sqrt((m_dSrcFinY-m_dSrcDebY)*(m_dSrcFinY-m_dSrcDebY)+(m_dSrcFinX-m_dSrcDebX)*(m_dSrcFinX-m_dSrcDebX)); // source
                    default:
                        return 0.0;
                }


            }

        }
}

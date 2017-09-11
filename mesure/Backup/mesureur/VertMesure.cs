﻿using System;
using System.Collections.Generic;
using System.Text;
using mesure;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Collections;

namespace mesure
{
    
    /// <summary>
    /// classe derivee de icalculator faisant une mesure de longueur verticale
    /// </summary>
    public class VertMesure : GenericCalculator, ICalculator
    {
        //public event MesureEventHandler VisuToSrcConv;
        //public event MesureEventHandler SrcToVisuConv;
        //public event MesureEventHandler SrcToEtalConv;
        //public event ResuEventHandler ToResu;
      
     private int[] legID = new int[]{101};
     
     private string[] legres= new string[]{"hauteur"}; // type de resultat 101

     private Cursor m_cursup = null;
     private Cursor m_cursdn = null;
     private Cursor m_cursdef = null;
        
        /// <summary>
        /// constructeur
        /// </summary>
     public VertMesure()
        {//Cursorhaut = new Cursor(GetType(), "Resources.dnmove.cur"); // chargement d'un curseur depuis la resource
         //Cursorbas = Cursorhaut;
        }
           
       
        // resultats
        double m_Longueur=0;

        bool mousedown = false;

        enum ligne { none, haut, bas };
        ligne tomove = ligne.none;

        private double ylignehaut = -1; // coordonees (client) de la ligne du haut
        private double ylignebas = -1;  // coordonees (client) de la ligne du bas
        private double ysourcehaut;
        private double ysourcebas;  /// y source de l ligne basse
        private double yCalHaut;    /// y calibre de la ligne haute
        private double yCalBas;     /// y calibre de la ligne basse

        Control leclient = null;
        private Rectangle m_prvrect;
        double prvxmous = -1.0; // derniere coordonnee de la souris
        double prvymous = -1.0;
        double srcylignebas = 0.0;
        double srcylignehaut = 0.0;


        #region serialisation de ce calcul
        /// <summary>
        /// necessaire a Icalculator
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int SaveDisk(XMLAvElement element)
        {
            element.SetAttribute("lineup", ysourcehaut);
            element.SetAttribute("linedn", ysourcebas);

            return 0;
        }

        /// <summary>
        /// necessaire a Icalculator
        /// lit les reglages dans las ection de la config
        /// </summary>
        /// <param name="elemnt"></param>
        /// <returns></returns>
        public int LoadDisk(XMLAvElement element)
        {
            try
            {
                element.SetAttribute("lineup", ysourcehaut);
                element.SetAttribute("linedn", ysourcebas);                
            }
            catch (XmlAvException e)
            {
                if (e.XmlAvType != xmlavexceptiontype.none) // histoire de dire qqch
                // pas lu les valeurs : on reinit
                 inity();
            }
            return 0;
        }
        #endregion


        #region gestion des resultats caclules
        

        /// <summary>
        /// fonction construisant la liste des resultats
        /// </summary>
        /// <param name="final">true si c'est le resultat destine a etre enregistre</param>
        /// <returns></returns>
        protected virtual ResuEventArg sendresuline(bool final)
        {
            tomove = ligne.none;

            ResuEventArg resargs = new ResuEventArg(this, final); // resultat final
            resargs.AddResu(101); // sortir un resultat 101
            return resargs;
        }

        /// <summary>
        /// renvoie la valeur du calul de numero ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public double GetValeur(int ID, int unit)
        {switch(ID)
            {case 101:
                    return m_Longueur;
                break;
            }    
           
         return 0.0;
        }

        /// <summary>
        /// necessaire a Icalculator
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
         public string GetLegende(int ID)
        {switch(ID)
            {case 101 :
                return legres[0];
             default:
                return "";
            }
        }

        /// <summary>
        /// necessaire a Icalculator
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="etalflags"></param>
        /// <returns></returns>
        public string GetUnit(int ID, ref etalflagstyp  etalflags)
        {switch(ID)
            {case 101:
                etalflags = etalflagstyp.distancey; // distance y
                return "mm";

             default:
                etalflags = etalflagstyp.none; // distance y
                return "";
            }
        }


        /// <summary>
        /// genere la listr des id de resultats disponibles pour ce calcul
        /// </summary>
        /// <returns></returns>
        public ArrayList GetListResu()
        {
            ArrayList laliste = new ArrayList();
            laliste.Add(101); // que la longueur
            return laliste;
        }

        #endregion


        /// <summary>
        /// reinitialise les coordonnees lignes a l'ecran en les recalculant a partir des coords source et du clipper
        /// </summary>
        /// <param name="sender">objet envoyant le message</param>
        private void reloadlignes(object sender)
        {
            MesureEventArgs mesargs = new MesureEventArgs(0, ysourcehaut); // conversion uniquement en y
            // appelle la conversio des coords par un evenement a l'etalonnage
            //if (SrcToVisuConv != null)
            OnSrcToVisuConv(sender, ref mesargs); // conversion source -> visu

            if (mesargs.CodeErreur == 0) // pas d'erreur
                ylignehaut = mesargs.outPt.Y; // ou tracer la ligne

            mesargs = new MesureEventArgs(0, ysourcebas); // conversion uniquement en y

            // appelle la conversio des coords par un evenement a l'etalonnage
            //if (SrcToVisuConv != null)
                OnSrcToVisuConv(sender, ref mesargs);

            if (mesargs.CodeErreur == 0) // pas d'erreur
                ylignebas = mesargs.outPt.Y;

            // cas ou une erreur : on fait quoi ?
            Debug.WriteLine("haut :" + ysourcehaut + "->" + ylignehaut + "-" + ysourcebas + "->" + ylignebas);
        }
        
        /// <summary>
        /// fonction traitant les changement de taille de la visu
        /// le but est de reconvertir les coords source de ce calculateur en nouvelles coords clipp
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnChgVisuSize(object sender, EventArgs e)
        {// on recupere les coord source des points et on les converit en coords clipper
         reloadlignes(sender);
         repaint();    
         
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void repaint()
        { // on repeint les traits
            if (leclient != null)
            {
                Graphics gr = lepanel.GetGraph(true); // avec clear
                gr.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                Pen lepen = new Pen(Color.FromArgb(254, m_Color), 1);

                Rectangle rect = leclient.ClientRectangle;
                //gr.Clear(Color.FromArgb(255, Color.Magenta)); // effece le rectangle
                gr.DrawLine(lepen, (float)0, (float)ylignebas, (float)rect.Width, (float)ylignebas); // on efface
                gr.DrawLine(lepen, (float)0, (float)ylignehaut, (float)rect.Width, (float)ylignehaut); // on repeint en bleu
                lepanel.Releasegraph(gr); // 

            }
        }

        /// <summary>
        /// rend cette mesure active (et lza dessine a l'ecran)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public int SetActive(Control client)
        {leclient = client;
            
        // on positionn les lignes sur le centre de la fenetre si elles sont a -1
        // il faudra aussi les repositionner si la taille change
        //on init lea position des traits 
        bool init = inity(); 
        
        // init ce qui est a initialiser : les curseurs
        m_cursup = new Cursor(GetType(),"Resources.upmove.cur");
        m_cursdn = new Cursor(GetType(), "Resources.dnmove.cur");
        m_cursdef = new Cursor(GetType(), "Resources.regle.cur");

        client.Cursor = m_cursdef;
        // init vaut 1 si c'est une initialisation, sinon on doit recacluer la position des lignes
        // ou on les reinit si quelque chose a change
        reloadlignes(client);
            
         // on repeint les traits
         if (leclient != null)
            {
             Pen redpen = new Pen(Color.FromArgb(254, m_Color), 1);
             Graphics gr = lepanel.GetGraph(true); // avec clear
             gr.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
             Rectangle rect = leclient.ClientRectangle;
             //gr.Clear(Color.FromArgb(255, Color.Magenta)); // effece le rectangle
             gr.DrawLine(redpen, (float)0, (float)ylignebas, (float)rect.Width, (float)ylignebas); // on efface
             gr.DrawLine(redpen, (float)0, (float)ylignehaut, (float)rect.Width, (float)ylignehaut); // on repeint en bleu
             lepanel.Releasegraph(gr); //        
            }
         
        // mesure de longueur et envoie pour affichage
        m_Longueur = srcylignebas - srcylignehaut;
        ResuEventArg resargs = new ResuEventArg(this);
        resargs.AddResu(101); // sortir un resultat 101
        //if(ToResu != null)
        OnToResu(this, ref resargs);
        
        return 0;
        }
        
        /// <summary>
        /// initialise la position des traits si c'est pas deja fait
        /// ne'init pas les valeurs de ycalibre
        /// </summary>
        /// <returns></returns>
        private bool inity()
        {Rectangle rectdraw;

        if (leclient != null)
        {
            rectdraw = leclient.ClientRectangle;
            
            if (ylignehaut == -1 || ylignebas == -1)
            {
                ylignehaut = rectdraw.Height / 2 - 1; // init
                ylignebas = rectdraw.Height / 2 + 1;
                m_prvrect = rectdraw;
                // il reste a repositionner correctemetn les coords source de ces deux lignes
                MesureEventArgs mesargsh = new MesureEventArgs(0, ylignehaut);
                //if (VisuToSrcConv != null)
                    OnVisuToSrcConv(this, ref mesargsh);
                ysourcehaut = mesargsh.outPt.Y;
                MesureEventArgs mesargsb = new MesureEventArgs(0, ylignebas);
                //if (VisuToSrcConv != null)
                    OnVisuToSrcConv(this, ref mesargsb);
                ysourcebas = mesargsb.outPt.Y;
                
                // reinit la longueur calibree
                m_Longueur=0;
                return true;  // les lignes ont ete redefinies
            }

            // ici il faut tracer
            return false;
        }
        return false; // rien a faire leclient existe pas
        }

        
        /// <summary>
        /// traite un changement de variable
        /// renvoie 0 si resultat ok
        /// renvoie 1 si attend encore qqchose                
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public int OnChangeVariable(object sender, ChangeVariable e)
        {
            ligne possouris = ligne.none;
            Control source = (Control)sender;
            double y = e.valeury;
            double x = e.valeurx;
            switch (e.num)
            {
                /// deplacement de souris au dessus de la fentre visu : on gere le curseur
                case ChgVarNum.none:
                    if (Math.Abs(y - ylignehaut) < 8)
                        possouris = ligne.haut;

                    if (Math.Abs(y - ylignebas) < 8)
                        possouris = ligne.bas;

                    if (possouris != ligne.none)
                    {
                        if (Math.Abs(y - ylignehaut) < Math.Abs(y - ylignebas))
                            possouris = ligne.haut; // c'est la ligne d'en haut qui bouge
                        else
                            possouris = ligne.bas; // c'est la ligne d'en haut qui bouge
                    }

                    Cursor newCursor=null;
                    switch (possouris)
                    {
                        case ligne.haut:
                            newCursor = this.m_cursup;
                            break;
                        case ligne.bas:
                            newCursor= this.m_cursdn;
                            break;
                        default:
                            newCursor = this.m_cursdef;
                            break;
                    }
                    if (newCursor != source.Cursor)
                        leclient.Cursor = newCursor;
                    return 1;

                // mouse down : debut de trace
                case ChgVarNum.mousedown: //debut de trace
                    if (onmousedown(e.valeurx, e.valeury) == 0)
                        return 0;
                    else
                    {
                        mousedown = true;
                        source.Cursor = null;
                    }
                    break;

                // mouse move avec le bouton appuye
                case ChgVarNum.mousmov: //move de trace
                    if (mousedown)
                        onmousemove(e.valeurx, e.valeury);
                    //else
                        //source.Cursor;
                    break;
                
                // bouton relache
                case ChgVarNum.mousup: //finde trace
                    if (mousedown)
                        onmouseup(e.valeurx, e.valeury);
                    mousedown = false;
                    
                    newCursor = this.m_cursdef;
                    leclient.Cursor = newCursor;
                    break;  
                
                case ChgVarNum.sendresu: //envoi de resultat final
                    ResuEventArg resargs = sendresuline(true); // final
                    //if (ToResu != null)
                    OnToResu(this, ref resargs);
                    
                    break;
            }

        return 0;
        }


        #region gestion mouvements souris
        /// <summary>
    /// traite les coordonnees de la souris quand on l'appuie
    /// </summary>
    /// <param name="x">coord souris dans la fenetre visu video</param>
    /// <param name="y"></param>
    /// <returns>0 si u probleme</returns>
        private int onmousedown(double x, double y)
            {Graphics gr =null;
             Rectangle rect;
             tomove = ligne.none;

             if (leclient != null)
                 rect = leclient.ClientRectangle;
             else
                 return 0;

            prvxmous = x; // derniere coordonnee de la souris
            prvymous = y;

            if (Math.Abs(y - ylignehaut) < 8)
                tomove =ligne.haut;

            if (Math.Abs(y - ylignebas) < 8)
                tomove =ligne.bas;
                                
              if (tomove != ligne.none)
                {if (Math.Abs(y - ylignehaut) < Math.Abs(y - ylignebas))
                    tomove =ligne.haut; // c'est la ligne d'en haut qui bouge
                }
              else 
                  return 0; // none : on fait rien
                
                gr = lepanel.GetGraph();
                gr.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                Pen lepen = new Pen(Color.FromArgb(255,Color.Magenta),4);
                Pen lepenR = new Pen(Color.FromArgb(254, m_Color), 4);
                Pen lepenB = new Pen(Color.FromArgb(254, Color.MediumBlue), 2);
                
                switch(tomove) // quelle est la ligne qui bouge ?
                    {case ligne.bas:
                        // on efface la ligne
                        gr.DrawLine(lepen, (float)0, (float)ylignebas, (float)rect.Width, (float)ylignebas); // on efface
                        // on repaint la ligne du haut en gros
                        gr.DrawLine(lepenR, (float)0, (float)ylignehaut, (float)rect.Width, (float)ylignehaut); 
                
                        gr.DrawLine(lepenB, (float)0, (float)ylignebas, (float)rect.Width, (float)ylignebas); // on repeint en bleu                        
                        break;

                    case ligne.haut:
                        // on efface la ligne 
                         gr.DrawLine(lepen, (float)0, (float)ylignehaut, (float)rect.Width, (float)ylignehaut); // on efface
                         // on repaint la ligne du haut en gros car elle est effacee si les lignes se touchent
                         gr.DrawLine(lepenR, (float)0, (float)ylignebas, (float)rect.Width, (float)ylignebas);
                         gr.DrawLine(lepenB, (float)0, (float)ylignehaut, (float)rect.Width, (float)ylignehaut); // on repeint en bleu                        
                        break;
                    }
                
                // libere le graphics (et lance le present)
                lepen.Dispose();
                lepenB.Dispose();
                lepenR.Dispose();
                lepanel.Releasegraph(gr); //release et transfert dans le panel
                
                return 1; // ca marche !
            }


        /// <summary>
        /// action realisee au mouvement de souris
        /// on lit les y et on les convertit en source mais pas en calibre
        /// on sauvegardera les coordonnees source une fois la souris relachee
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        int onmousemove(double x, double y)
        {
            Rectangle rect;

            if (tomove == ligne.none) // pas de ligne selectionne
            {
                if (leclient != null)
                    rect = leclient.ClientRectangle;
                else
                    return 0;
            }


            if (leclient != null)
                rect = leclient.ClientRectangle;
            else
                return 0;

            double deltay = (y - prvymous);
            prvxmous = x; // derniere coordonnee de la souris
            prvymous = y;
            if (tomove == ligne.none)
                return 0;


            // trace du curseur transparent

            double savligne;
            switch (tomove)
            {
                case ligne.bas:
                    {// on efface la ligne
                        savligne = ylignebas;
                        ylignebas += deltay;
                        if (ylignebas < ylignehaut)
                        {
                            ylignebas = savligne;
                            break;
                        }

                        MesureEventArgs mesargsb = new MesureEventArgs(0, ylignebas);
                        //if (VisuToSrcConv != null)
                            OnVisuToSrcConv(this, ref mesargsb);
                        ysourcebas = mesargsb.outPt.Y;
                    }
                    break;

                case ligne.haut:
                    {// on efface la ligne
                        savligne = ylignehaut;
                        ylignehaut += deltay;
                        if (ylignebas < ylignehaut)
                        {
                            ylignehaut = savligne;
                            break;
                        }

                        MesureEventArgs mesargsh = new MesureEventArgs(0, ylignehaut);
                        //if (VisuToSrcConv != null)
                            OnVisuToSrcConv(this, ref mesargsh);
                        ysourcehaut = mesargsh.outPt.Y;

                    }
                    break;

                default:
                    return 0;

            }


            Graphics gr = lepanel.GetGraph();
            gr.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            Pen lepen = new Pen(Color.FromArgb(255, Color.Magenta), 2); // efface
            Pen lepen2 = new Pen(Color.FromArgb(254, Color.MediumBlue), 2); // peinr en bleu

            gr.DrawLine(lepen, (float)0, (float)savligne, (float)rect.Width, (float)savligne); // on efface le prec
            gr.DrawLine(lepen2, (float)0, (float)ylignebas, (float)rect.Width, (float)ylignebas); // on repeint en bleu
            gr.DrawLine(lepen2, (float)0, (float)ylignehaut, (float)rect.Width, (float)ylignehaut); // on repeint en rouge l'autre ligne

            lepen.Dispose();
            lepen2.Dispose();
            lepanel.Releasegraph(gr);

            return 0;
        }

        /// <summary>
        /// action realisee au lacher de souris:
        /// on calcule le resu etalonne
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        int onmouseup(double x, double y)
        {

            // on recalcule les coords sources des 2 lignes
            MesureEventArgs mesargsh = new MesureEventArgs(0, ylignehaut);
            //if (VisuToSrcConv != null)
            OnVisuToSrcConv(this, ref mesargsh);

            ysourcehaut = mesargsh.outPt.Y;

            // deuxieme conversion : en valeurs etalonnees
            mesargsh.out2inp(); // on repare le deuxieme coversion
            //if (SrcToEtalConv != null)
                OnSrcToEtalConv(this, ref mesargsh);

            double ycalhaut = mesargsh.outPt.Y;


            MesureEventArgs mesargsb = new MesureEventArgs(0, ylignebas);
            //if (VisuToSrcConv != null)
            OnVisuToSrcConv(this, ref mesargsb);
            ysourcebas = mesargsb.outPt.Y; // coocrdonnees du bas e Source

            // deuxieme conversion : en valeurs etalonnees
            mesargsb.out2inp(); // on remet le out dans le in
            //if (SrcToEtalConv != null)
            OnSrcToEtalConv(this, ref mesargsb);

            double ycalbas = mesargsb.outPt.Y;

            // calcul dela longueur en calibré
            m_Longueur = ycalbas - ycalhaut;

            Graphics gr = lepanel.GetGraph(true); // avec clear
            gr.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            Pen lepen = new Pen(Color.FromArgb(254, m_Color), 2);

            ResuEventArg resargs = sendresuline(false); // pas final
            //if (ToResu != null)
            OnToResu(this, ref resargs);

            Rectangle rect;
            if (leclient != null)
            {
                rect = leclient.ClientRectangle;

                gr.DrawLine(lepen, (float)0, (float)ylignebas, (float)rect.Width, (float)ylignebas); // on efface
                gr.DrawLine(lepen, (float)0, (float)ylignehaut, (float)rect.Width, (float)ylignehaut); // on repeint en bleu
            }

            lepanel.Releasegraph(gr); // avec clear

            return 1;
        }



        #endregion

        /// <summary>
        ///  teste xy pour savoir si on est pres des lignes et donc si on doit
        ///  changer le curseur
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        int tstcursor(double x, double y)
            {
                Rectangle rect;

                if (leclient != null)
                    rect = leclient.ClientRectangle;
                else
                    return 0;

                prvxmous = x; // derniere coordonnee de la souris
                prvymous = y;

                if (Math.Abs(y - ylignehaut) < 8)
                    tomove = ligne.haut;

                if (Math.Abs(y - ylignebas) < 8)
                    tomove = ligne.bas;

                if (tomove != ligne.none)
                {
                    if (Math.Abs(y - ylignehaut) < Math.Abs(y - ylignebas))
                        tomove = ligne.haut; // c'est la ligne d'en haut qui bouge
                    else
                        tomove = ligne.bas; // c'est la ligne d'en haut qui bouge
                }
                else
                    return 0; // none : on fait rien

                switch (tomove)
                {
                    case ligne.none: leclient.Cursor= this.m_cursdef; break;
                    case ligne.haut: leclient.Cursor = this.m_cursup; break;
                    case ligne.bas: leclient.Cursor = this.m_cursdn; break;

                }
                return 0;
            }
           

        
            

        
    }
}

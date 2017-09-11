﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Resources;


namespace mesure
{   /// <summary>
    /// delegate est une sorte de typedef pour l'event
    /// ici on envoie au form un ok quand on quitte le dial modal
    /// e.ok = vrai : l'etal est correct  : validation
    /// = false : on n'ajout pas l'etalonnage
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    public delegate void EtalOkhandler(object source, ClickOkEvent e);

    /// <summary>
    /// form d'etalonnage
    /// </summary>
    public partial class EtalForm : Form
    {
        private double m_dDistPix;
        private double m_dSaisie;
        private string m_sUnit;
        private string m_sObjName;
        Etalonnage m_curetal;
        PointF m_deb;
        PointF m_fin;
        bool m_bDrawDistOk = false; // trace distance etlonnage ok
        bool m_bSaisDistOk = false; // saisie definition distance de reference ok
        bool m_bSaisNamOk = false; // definition nom objectif ok
        bool m_bSaisUnitOk = false; // definition unie ok
        Timer letimer = new Timer();
        private int m_countdown = 10;


        /// <summary>
        /// event declenche au clique sur la fin
        /// </summary>
        [Browsable(true)] // visible dans l'editeur graphique
        public event EtalOkhandler CliqueFin;

        /// <summary>
        /// fonction appelee par la mainform lors du mouvement de la souris etalonnage
        /// doit copier les xy cliques dans les xy mesures, pour que ca ressorte dans les variables 
        /// du mesureru etalonnage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainFrmConvHandler(object sender, ref MesureEventArgs e)
        {
            return; // rien a faire : la conversion source-unite est faite dans le constructeur de mesureeventsargs
            // donc on recupere en sortie les cooedonnees non etalonnees des points cliquees
        }


        /// <summary>
        /// ticks de decompte du timer
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        void trttimer(object source, EventArgs e)
        {
            if (m_countdown == 10) // on n'es pas en decompte : on l'affiche
                return;

            m_countdown--;

            if (m_countdown == 0)
                SmoothHide(true); // on cache
        }

        /// <summary>
        /// ferme la fenetre avec un petit delai pour peviter le clignotement
        /// </summary>
        /// <param name="hide"></param>
        public void SmoothHide(bool hide)
        {
            if (m_countdown == 0 && hide) // fin du timer 
                this.Hide(); // on cache la fenetre

            if (hide == false) // montrer
            {
                if (m_countdown == 10) // on n'es pas en decompte : on l'affiche
                    this.Show();
                else // on est en decompte : on raz le timer sans rien faire
                    m_countdown = 0; // on arrete le countdown mais la fenetre n'avait pas ete fermee
            }

            // ordre de cacher et pas de decompte en cours : on demarre le decompte
            if (hide == true && m_countdown == 10)
            {
                m_countdown = 9; // on commence tt de suite

            }

            if (m_countdown == 0) // on est arrive au bout 
            {
                //letimer.Stop();
                m_countdown = 10;
                //letimer.Tick -= new EventHandler(trttimer);

            }
        }


        /// <summary>
        /// fonction appelee par l'evenement a chaque nouveau resultat du calcuteur
        /// on recoit une liste de vriables conteenant les 4 points de la ligne : 
        /// ce sont les coords des 2 points cliques
        /// euh il faudrait decider si c'est ici que ca se fait ou dasn setpoints ....
        /// </summary>
        /// <param name="sender">objet ayant envoye l'event</param>
        /// <param name="e">eventarg contenant la liste des ID des resultats disponbles en resultat</param>
        public void GetResu(object sender, ref ResuEventArg e)
        {
            ArrayList list = e.IDResus;
            ICalculator calcul = (ICalculator)sender;
            double debX = 0;
            double debY = 0;
            double finX = 0;
            double finY = 0;

            bool savDrawDistOk = m_bDrawDistOk;

            foreach (int ID in list)
            {
                double valeur = calcul.GetValeur(ID, 0);

                switch (ID)
                {
                    case 101: debX = valeur; break;
                    case 102: debY = valeur; break;
                    case 103: finX = valeur; break;
                    case 104: finY = valeur; break;
                }

            }


            m_deb.X = (float)debX;
            m_deb.Y = (float)debY;
            m_fin.X = (float)finX;
            m_fin.Y = (float)finY;
            m_dDistPix = Math.Sqrt((debX - finX) * (debX - finX) + (debY - finY) * (debY - finY));
            m_bDrawDistOk = true;
            // ici on pourrait donner cette distance dans la fenete
            if (m_bDrawDistOk != savDrawDistOk) // aachose a change dans la validite
                updateGUI(); // saisie ok on peut quitter

         
        }


        /// <summary>
        /// get /set de la distance saisie par l'utilisateur
        /// </summary>
        public double DistSaisie
        {
            get
            {
                return m_dSaisie;
            }
        }

        /// <summary>
        /// get set de l'etalonnage en cours d'edition
        /// </summary>
        public Etalonnage etalonnage
        {
            set { m_curetal = value; }
            get { return m_curetal; }
        }

        /// <summary>
        /// defintion des deux points d'etalonnage
        /// appele par l'event du trace de ligne
        /// </summary>
        /// <param name="deb">point de debut</param>
        /// <param name="fin">point de fin</param>
        public void SetPoints(PointF deb, PointF fin)
        {
            m_deb = deb; // on conserve les deux points dans l'etalonnage
            m_fin = fin;

            double dx = (double)(deb.X - fin.X);
            double dy = (double)(deb.Y - fin.Y);
            m_dDistPix = Math.Sqrt(dx * dx + dy * dy); // calcul de la distance d'etalonnage
            m_bDrawDistOk = true;// ici il faudrait mettre un bool distok qui valide l'etalonnage
        }

        

        /// <summary>
        /// creation de la form
        /// </summary>
        public EtalForm()
        {
            InitializeComponent();
        }

        Bitmap m_imagedone = null;
        Bitmap m_imagetodo = null;

        /// <summary>
        /// load de la forme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EtalForm_Load(object sender, EventArgs e)
        {
            Icon licon;
            Size sizeicon = new Size(16,16);
            // on charge les images
            //ResourceManager resources = new ResourceManager(typeof(MainForm));
            m_imagedone = new Bitmap(GetType(), "Resources.ok-green.ico");
            m_imagetodo = new Bitmap(GetType(), "Resources.RepeatBlue.ico");


            //resources.ReleaseAllResources();


            letimer.Interval = 50;
            letimer.Tick += new EventHandler(trttimer);
            letimer.Start();

            updateGUI(); // init des icones
        }


        /// <summary>
        /// teste l'etat de l'etalonnage tel qu'il est saisi et valide le ok ou l'invalide 
        /// </summary>
        void updateGUI()
        {
            
            bool isok = true;

            butQuit.Enabled = false;
            if (!m_bSaisDistOk)
            {
                isok = false;
                this.checkBox2.Image = m_imagetodo;
            }
            else
                this.checkBox2.Image = m_imagedone;

            if (!m_bDrawDistOk)
            {
                isok = false;
                this.checkBox1.Image = m_imagetodo;
            }
            else
                this.checkBox1.Image = m_imagedone;

            if (!m_bSaisNamOk)
            {
                isok = false;
                this.checkBox4.Image = m_imagetodo;
            }
            else
                this.checkBox4.Image = m_imagedone;

            if (!m_bSaisUnitOk)
            {
                //isok = false; pas bloquant
                this.checkBox3.Image = m_imagetodo;
            }
            else
                this.checkBox3.Image = m_imagedone;


            butQuit.Enabled = isok;
        }

        /// <summary>
        /// saisie longueur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edLongueur_TextChanged(object sender, EventArgs e)
        {
            double valmesuree = 0;
            bool savsaiok = m_bSaisDistOk;

            try
            {
                double.TryParse(edLongueur.Text, out valmesuree); // decodage du double saisi
                if (valmesuree != 0.0)
                    m_bSaisDistOk = true;
            }
            catch
            {
                m_bSaisDistOk = false;
            }


            if (m_bSaisDistOk)
                m_dSaisie = valmesuree; // distance saisie

            if (m_bSaisDistOk != savsaiok) // aachose a change dans la validite
                updateGUI(); // saisie ok on peut quitter
        }

        /// <summary>
        /// saisie nom objectif
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void edNamObj_TextChanged(object sender, EventArgs e)
        {
            bool savsaisnam = m_bSaisNamOk;

            if (edNamObj.Text.Length < 3) // 3 lettres mini
                this.m_bSaisNamOk = false;
            else
                this.m_bSaisNamOk = true;

            this.m_sObjName = edNamObj.Text;

            if (savsaisnam != m_bSaisNamOk) // changelement de validation
                updateGUI(); // saisie ok on peut quitter
        }


        /// <summary>
        /// fermeture de la form
        /// si on a clique ok et que c'est pas valide : on cancel le quit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EtalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool canquit = true;

            ClickOkEvent eventout = new ClickOkEvent();
            // lecture de la distance
            eventout.ok = false; // cacnel par defaut

            if (this.DialogResult == DialogResult.OK) // clique sur ok
            {
                eventout.ok = true; // on a clique sur ok
                try
                {
                    m_curetal.InitEtal(m_deb.X, m_deb.Y, m_fin.X, m_fin.Y, this.m_dSaisie, "cm");
                    m_curetal.Unite = m_sUnit;
                    m_curetal.Name = m_sObjName;
                }
                catch (ExceptionNoEtal except)
                {// on fait rien pour l'excaption 
                    canquit = false; // etalonnage pas correct : on n'ajoutera pas cet etalonnage
                }
            }

            if (!canquit)
            {
                e.Cancel = true; // on abandonne la cancel
                return; // on quitte sans rien faire, le e.cancel est a true : ca continue
            }

            m_imagedone.Dispose();
            m_imagetodo.Dispose();

            // on va quitter le dialogue : arret du timer
            letimer.Stop();
            letimer.Tick -= new EventHandler(trttimer);

            // on previent l'appli que le dial est ferme 
            if (CliqueFin != null) //cet evenement a ete attache par qqun
                CliqueFin(this, eventout);

        }

        /// <summary>
        /// controle l'etalonnage apres changement du texte du combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            bool prev = m_bSaisUnitOk;
            if (comboBox1.Text.Length > 0)
                m_bSaisUnitOk = true;
            else
                m_bSaisUnitOk = false;

            m_sUnit = comboBox1.Text;

            if (prev != m_bSaisUnitOk)
                updateGUI(); // saisie ok on peut quitter

        }

        /// <summary>
        /// action sur le bouton click : close de la fenetre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butQuit_Click(object sender, EventArgs e)
        {
            this.Close(); // il faut mettre qqch dans ce bouton car le dial est pas modal : un clique sur ok ne fait pas un close automatiquemùent
            // mais dialogresult est correctement rempli
        }

        /// <summary>
        ///  action sur le bouton cancel : close de la fenetre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butcancel_Click(object sender, EventArgs e)
        {// il faut mettre qqch dans ce bouton car le dial est pas modal : un clique sur ok ne fait pas un close automatiquemùent
         // mais dialogresult est correctement rempli
            this.Close();
        }
    }

    //---------------------------------------------------
    // definition de l'argument de l'event clickokevent
    //---------------------------------------------------
    public class ClickOkEvent : System.EventArgs
    {
        public ClickOkEvent() { }

        bool bOk;
        public bool ok
        {
            get { return bOk; }
            set { bOk = value; }
        }
    }

}
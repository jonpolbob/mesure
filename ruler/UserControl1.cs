using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ruler
{

    public enum direction { vertical=0, horizontal=1 };

    public partial class UserControl1 : UserControl
    {
        double m_deb = 0;
        double m_fin = 100;
        direction m_direction = direction.horizontal;


        [Description("Sets the Direction"),
        Category("Values"),
        DefaultValue(direction.horizontal),
        Browsable(true)]
        public direction Direction
        { set { m_direction = value; }
            get { return m_direction; }
        }
        public double MinVal
        { get { return m_deb; }
            set { m_deb = value;
            }
        }

        public double MaxVal
        {
            get { return m_fin; }
            set { m_fin = value;
            }
        }
        
        public UserControl1()
        {
            m_deb = 0;
            m_fin = m_deb; // bloque le draw des tics

            InitializeComponent();
            this.BackColor = Color.LightBlue;
            
        }


        // calcul les ticks pour une dynamique comprise entre m_deb et m_fin
        private int changescale(ref double[] tableau) 
        {// on regarde le min et le max et on regarde comment faire des marques qui ressemblent a qq chose
         double normaldelta = m_fin - m_deb;
         double facteur =1.0;
         
         if (normaldelta == 0)
             return 0;

         while (normaldelta >= 10)
         { facteur *= 10;
         normaldelta = normaldelta / 10;
         }

         while (normaldelta <= 1)
         {facteur *= 10;
          normaldelta /= 10;
         }

        // ici on a trouve un facteur multipliccatif qui ramene la difference entre 1 et 10
         //etdouble tick; normaldelta qui es cette difference entre 0 et 1
         double tick=.2;
         int nbticks=0;

         if (m_deb >= m_fin)
            return 0;

         for (int i = 0; i < 3; i++)
            {
                 switch (i) // on fait les essais pour differents ecarts de tick en valeur normalisee
                    { case 0 : tick = 1; break;
                      case 1 : tick = 0.5; break;
                      case 2: tick = 0.25; break;
                      case 3 : tick = 0.2; break;
                    }

             // si on a trouve un tick qui affiche plus de 5 ticks, c'est bon
                    nbticks = (int)(normaldelta / tick);
             if (nbticks >= 5)
                 break;
            }
        
            
            // on calule la valeur non normalisee du tic
            double unormtick = (int)(tick*facteur); 

            double mintick = (double)((int)(m_deb/unormtick))*unormtick;
            
            for (int i=0;i<=nbticks;i++)
                tableau[i] = mintick+i*unormtick;
                

            return nbticks;
         }


        //--------------------------------------------------
        // convertit un nombre du reel en coord zone client
        //--------------------------------------------------
        private int reeltoclienthor(double reel)
        {
            double pourcent; 
            
        if ((m_fin - m_deb) == 0)
            pourcent = 100;
        else 
            pourcent = (reel-m_deb)/(m_fin-m_deb);
         Rectangle clientrect;
         clientrect = this.ClientRectangle;
         int valeur = (int)((double)clientrect.Width*pourcent);
         
         return valeur;
        }

        //--------------------------------------------------
        //
        //--------------------------------------------------
        private int reeltoclientver(double reel)
        {
            double pourcent; 
       
            if ((m_fin - m_deb) == 0)
                pourcent = 100;
            else
                pourcent = (reel - m_deb) / (m_fin - m_deb);
            Rectangle clientrect;
            clientrect = this.ClientRectangle;
            int valeur = (int)((double)clientrect.Height * pourcent);

            return valeur;
        }


        //--------------------------------------------------
        // convertit un nombre de la zone client au reel
        //--------------------------------------------------
        private double clienttoreel(double reel)
        {
            return 0;
        }

        //-------------------------------------------------
        // peinture du controle 
        //-------------------------------------------------

        private void ctrlpaint(Graphics gr)
        {
            if (m_direction == direction.vertical)
                ctrlverpaint(gr);
            else
                ctrlhorpaint(gr);
        }

        //-------------------------------------------------
        //-------------------------------------------------
        private void ctrlverpaint(Graphics gr)
        {
            double[] tabtics = new double[11];
            int nbtics;
            Rectangle clientrect;
            clientrect = this.ClientRectangle;
            Brush degrade = new LinearGradientBrush(clientrect, Color.LightGray, Color.LightBlue, LinearGradientMode.Horizontal);
            gr.FillRectangle(degrade, clientrect);
       
            nbtics = changescale(ref tabtics);

            int i;
            for (i = 0; i <= nbtics; i++)
            {
                int x = reeltoclientver(tabtics[i]);
                gr.DrawLine(new Pen(Color.DarkSlateGray), clientrect.Left, x, clientrect.Right - clientrect.Width * 2 / 3, x);
                string txtout = tabtics[i].ToString("0.0");
                Brush brush = new SolidBrush(Color.DarkSlateGray);
                Font font = new Font("arial", 7);
                gr.DrawString(txtout, font, brush, new PointF(clientrect.Right - clientrect.Width *3/ 4,x));
            }

        }

        //-------------------------------------------
        // peinture du controle su horizontal
        //-------------------------------------------
        private void ctrlhorpaint(Graphics gr)
        {double [] tabtics = new double[11];
         int nbtics;
         Rectangle clientrect;
         clientrect = this.ClientRectangle;
         Brush degrade = new LinearGradientBrush(clientrect, Color.LightGray, Color.LightBlue, LinearGradientMode.Vertical);
         gr.FillRectangle(degrade, clientrect);
         nbtics = changescale(ref tabtics);
            
            int i;
            for (i = 0; i <= nbtics; i++)
            {
                int x = reeltoclienthor(tabtics[i]);
                gr.DrawLine(new Pen(Color.DarkSlateGray), x, clientrect.Top, x, clientrect.Bottom - clientrect.Height / 2);
                string txtout = tabtics[i].ToString("0.0");
                Brush brush = new SolidBrush(Color.DarkSlateGray);
                Font font = new Font("arial",7);
                gr.DrawString(txtout, font, brush, new PointF(x, clientrect.Bottom - clientrect.Height / 2));
            }

        }


        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            ctrlpaint(e.Graphics);
        }
    }
}
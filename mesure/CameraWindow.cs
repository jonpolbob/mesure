using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ruler;




namespace mesure
{
    /// --------------  delegates ----------------------------------
    // delegate est une sorte de typedef pour l'event
    public delegate void ChangeVariablehandler(object source, ChangeVariable e);


    /// <summary>
    /// camerawindow : control comprenant un panel, la fenetre ou s'affiche la video et les rulers. 
    /// le panel remplit sa fenetre mere, quand il se redimensionne on recaclule la position de la video en fonction des parametres actuels
    /// </summary>
    public partial class CameraWindow : UserControl
    {Object picrectobj = new Object();

     private Camera m_Camera = null;
     private bool autosize = true;
     private bool needSizeUpdate = false;
     private bool firstFrame = true;

     private double m_xmin=0;
     private double m_xmax=100;
     private double m_ymin=0;
     private double m_ymax=100;

     private System.Timers.Timer timer;
     private int flash = 0;
     private Color rectColor = Color.Black;

     // points de debut et fin 
     private Point startPoint = new Point(0, 0);
     private Point endPoint = new Point(0, 0);
     private bool m_IsTarget = false;

     // bitmap de dessin
     /*private Bitmap backBuffer = null;*/
     private bool mouseDown = false;
     Object thislock = new Object();
     private int m_prvwidth = 0;
     private int m_prvheight = 0;
     private Panel panelsize;
     private UserControl1 pictRuleLeft;
     private UserControl1 pictRuleTop;
     private Panel picvideo;
     public event EventHandler ChgClipp;
     // pour utiliser le .cur il faut le mettre en embedded dasn ses proprietes (action)
     Cursor m_regle=null;
     private wyDay.Controls.VistaMenu vistaMenu1;
     private IContainer components;

     Rectangle pictrect = new Rectangle(0, 0, 0, 0);
         
     /// <summary>
     /// ne pinte plus sur une ppict mais directemetn sur le clientrect
     /// </summary>
            public Control PicVideo
            {
                get {
                    return (Control)picvideo; 
                    }
            }

            //[Browsable(false)] // pas visible en mode editeur
            // event du control
            [Category("Property Changed")]
            [Description("Actif quand une variable a changé")]

            // declaration du event memebre de  la classe du controle
            [Browsable(true)] // visible dans l'editeur graphique
            public event ChangeVariablehandler ChangingVariable;


            [Category("Property")]
            [Description("Echelle Y")]
            [Browsable(true)] // visible dans l'editeur graphique
            public RulerAxis YScale
            {
                set 
                {   m_ymin = value.min;
                    m_ymax = value.max;
                    this.pictRuleLeft.MinVal= m_ymin;
                    this.pictRuleLeft.MaxVal = m_ymax;
                }
                get 
                {   RulerAxis retour = new RulerAxis();
                    retour.min = m_ymin;
                    retour.max = m_ymax;
                    return retour;
                }
            }

        [Category("Property")]
         [Description("Echelle X")]

            // declaration du event memebre de  la classe du controle
            [Browsable(true)] // visible dans l'editeur graphique
            public RulerAxis XScale
            {
                set 
                {   m_xmin = value.min;
                    m_xmax = value.max;
                    this.pictRuleTop.MinVal = m_xmin;
                    this.pictRuleTop.MaxVal = m_xmax;
                }
                get 
                {   RulerAxis retour = new RulerAxis();
                    retour.min = m_xmin;
                    retour.max = m_xmax;
                    return retour;
                }
            }

        /// <summary>
        /// 
        /// </summary>
           /* [Browsable(false)] // pas visible dans l'editeur graphique
            public Bitmap DrawBitMap
            {
                get
                {
                    System.Diagnostics.Debug.WriteLine("get drawb\r\n");
                    lock (thislock)
                    {
                        if (backBuffer != null)
                            return (Bitmap)backBuffer.Clone();
                        else
                            return null;
                    }
                }
                set
                {
                    System.Diagnostics.Debug.WriteLine("Set drawb\r\n");

                    lock (thislock)
                    {
                        if (backBuffer != null)
                            backBuffer.Dispose();
                        if (value != null)
                            backBuffer = (Bitmap)value.Clone();
                        else
                            backBuffer = null;
                    }
                }
            }
        */

            // AutoSize property
            [DefaultValue(false)]
            override public bool AutoSize
            {
                get { return autosize; }
                set
                {
                    autosize = value;
                    UpdatePosition();
                }
            }


            /// <summary>
            /// Camera property
            /// </summary>  
            [Browsable(false)]
            public Camera Camera
            {
                get { return m_Camera; }
                set
                {
                    Debug.Write("setcamIN");
                    // lock
                    Monitor.Enter(this);

                    // detach event
                    if (m_Camera != null)
                    {
                        //m_camera.CamNewFrame -= new EventHandler(camera_NewFrame);
                        //m_camera.Alarm -= new EventHandler(camera_Alarm);
                        timer.Stop();
                    }

                    m_Camera = value;
                    needSizeUpdate = true; // le timer devra redimensionner des choses
                    firstFrame = true;
                    flash = 0;


                    // atach event
                    if (m_Camera != null)
                    {// recalcul de la pictrect
                        //clipper.SourceRect = m_camera.SourceRect;

                        m_Camera.CamNewFrame += new EventHandler(camera_NewFrame);
                        //m_Camera.Alarm += new EventHandler(camera_Alarm);
                        timer.Start();
                        
                        // backbuffer = taille de la fenete video
                        // egale a la zone clippee
                        /*backBuffer = new Bitmap(m_camera.Clipper.ClipRect.Width, m_camera.Clipper.ClipRect.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        Graphics gr = Graphics.FromImage(backBuffer);
                        gr.FillRectangle(new SolidBrush(Color.FromArgb(255, Color.Magenta)), 0, 0, backBuffer.Width, backBuffer.Height);
                        gr.Dispose();
                         * */

                    }
                    Monitor.Exit(this);
                    Debug.Write("setcamOUT");
                }
            }



            
         /// <summary>
         /// 
         /// </summary>
            public CameraWindow()
            {
                InitializeComponent();
                //m_regle = new Cursor(GetType(), "Resources.regle.cur");
                m_regle = Cursors.NoMove2D;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer |
                    ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
                panelsize.Cursor = m_regle;
                
            }

            /// <summary>
            /// 
            /// </summary>
            #region Windows Form Designer generated code
            private void InitializeComponent()
            {
                this.components = new System.ComponentModel.Container();
                this.timer = new System.Timers.Timer();
                this.panelsize = new System.Windows.Forms.Panel();
                this.pictRuleTop = new ruler.UserControl1();
                this.pictRuleLeft = new ruler.UserControl1();
                this.picvideo = new System.Windows.Forms.Panel();
                this.vistaMenu1 = new wyDay.Controls.VistaMenu(this.components);
                ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
                this.panelsize.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.vistaMenu1)).BeginInit();
                this.SuspendLayout();
                // 
                // timer
                // 
                this.timer.SynchronizingObject = this;
                // 
                // panelsize
                // 
                this.panelsize.BackColor = System.Drawing.Color.LightBlue;
                this.panelsize.Controls.Add(this.pictRuleTop);
                this.panelsize.Controls.Add(this.pictRuleLeft);
                this.panelsize.Controls.Add(this.picvideo);
                this.panelsize.Location = new System.Drawing.Point(19, 29);
                this.panelsize.Name = "panelsize";
                this.panelsize.Size = new System.Drawing.Size(440, 320);
                this.panelsize.TabIndex = 0;
                // 
                // pictRuleTop
                // 
                this.pictRuleTop.BackColor = System.Drawing.SystemColors.Info;
                this.pictRuleTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                this.pictRuleTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.pictRuleTop.Location = new System.Drawing.Point(0, 283);
                this.pictRuleTop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
                this.pictRuleTop.MaxVal = 100;
                this.pictRuleTop.MinVal = 10;
                this.pictRuleTop.Name = "pictRuleTop";
                this.pictRuleTop.Size = new System.Drawing.Size(412, 37);
                this.pictRuleTop.TabIndex = 4;
                this.pictRuleTop.TabStop = false;
                // 
                // pictRuleLeft
                // 
                this.pictRuleLeft.BackColor = System.Drawing.SystemColors.Info;
                this.pictRuleLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                this.pictRuleLeft.Direction = ruler.direction.vertical;
                this.pictRuleLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.pictRuleLeft.Location = new System.Drawing.Point(390, 0);
                this.pictRuleLeft.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
                this.pictRuleLeft.MaxVal = 100;
                this.pictRuleLeft.MinVal = 10;
                this.pictRuleLeft.Name = "pictRuleLeft";
                this.pictRuleLeft.Size = new System.Drawing.Size(47, 283);
                this.pictRuleLeft.TabIndex = 5;
                this.pictRuleLeft.TabStop = false;
                // 
                // picvideo
                // 
                this.picvideo.AccessibleRole = System.Windows.Forms.AccessibleRole.Cursor;
                this.picvideo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
                this.picvideo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                this.picvideo.Location = new System.Drawing.Point(-3, 0);
                this.picvideo.Name = "picvideo";
                this.picvideo.Size = new System.Drawing.Size(377, 277);
                this.picvideo.TabIndex = 6;
                this.picvideo.MouseLeave += new System.EventHandler(this.CameraWindow_MouseLeave);
                this.picvideo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picvideo_MouseMove);
                this.picvideo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picvideo_MouseDown);
                this.picvideo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picvideo_MouseUp);
                this.picvideo.MouseEnter += new System.EventHandler(this.CameraWindow_MouseEnter);
                // 
                // vistaMenu1
                // 
                this.vistaMenu1.ContainerControl = this;
                // 
                // CameraWindow
                // 
                this.BackColor = System.Drawing.Color.LightBlue;
                this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                this.Controls.Add(this.panelsize);
                this.Cursor = System.Windows.Forms.Cursors.No;
                this.Name = "CameraWindow";
                this.Size = new System.Drawing.Size(570, 383);
                this.Load += new System.EventHandler(this.CameraWindow_Load);
                this.Paint += new System.Windows.Forms.PaintEventHandler(this.CameraWindow_Paint);
                this.Resize += new System.EventHandler(this.CameraWindow_Resize);
                this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CameraWindow_KeyPress);
                this.SizeChanged += new System.EventHandler(this.CameraWindow_SizeChanged);
                ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
                this.panelsize.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.vistaMenu1)).EndInit();
                this.ResumeLayout(false);

            }
            #endregion

            
        /// <summary>
        /// Paint control
        /// </summary>
        /// <param name="pe"></param>
    
        protected override void OnPaint(PaintEventArgs pe)
            {
                CameraWindow_Resize(null, null); // on fait juste un resize (bidon) ce qui provoque le reaffichage des regles et reclacul des rectangles
              
            }

         
        /// <summary>
         /// fonction a appeler quand on modifie la position deu control
         /// elle se charge de recacluler les positrion de ses constituants
         /// /// en cas de modofication de la taille de la camwin
         /// </summary>
        public void UpdatePosition()
        {if (m_prvwidth == Width && m_prvheight == Height)
                    return;
        WinCamPaint();
        }
        
        
        /// <summary>
        /// relais pour updateposition
        /// recalule la taille de la wincam et son positionnement
        /// a prtir de la taille de la femetre parrent
        /// </summary>
        public void WinCamPaint()
            {
                
                // lock
                Monitor.Enter(this);
                m_prvheight = Height;
                m_prvwidth = Width;



                if ((autosize) && (this.Parent != null))
                {// tant que m_camera est nul on n'a pas de backbuffer fabraique....
                 //
                 this.SuspendLayout();

                 // positionne la visurect
                 if (m_Camera != null)
                    {
                        Size visusize = CoreSystem.Instance.Etals.Clipper.processChangeVisuRect(Parent.ClientRectangle);  // taille de la fenetre client du container de cette fenetre
                        this.Location = CoreSystem.Instance.Etals.Clipper.VisuLocation;
                        this.Size = CoreSystem.Instance.Etals.Clipper.VisuSize;
                        //m_camera.ChgVisuRect(); // on signale a la cam que le visurect a change
                    }
                 
                this.ResumeLayout();

                }
                // unlock
                Monitor.Exit(this);
                
                if (ChgClipp != null)
                    ChgClipp(this,new EventArgs()); // evenement envoye ailleurs disant qu'on a modifie la ^position de qqch dans le clipper
            }
         
         
         /// <summary>
         /// eappele sur evenement sizechanged de la camerawindow
         /// recalcule la taille de picvideo a partir de la taille de camerwindow et des curseurs
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
           private void CameraWindow_SizeChanged(object sender, EventArgs e)
           {
               /*Control quid = (Control)sender;

               Debug.Write("[camerawindowresize]");
               if (m_Camera != null)
               {//NOTE : Rectangle est une structure donc le = fait une copie
                   Rectangle rectPar = new Rectangle();
                   //Control CSender = (Control)sender;
                   rectPar = this.Parent.ClientRectangle; // sender est en fait this
                   Rectangle savrect = rectPar;
                   rectPar.Width -= pictRuleLeft.Width; // on diminue le rect de la taille des regles pour recalculer la taille de la video
                   rectPar.Height -= pictRuleTop.Height;

                   if (rectPar.Width <= 0 || rectPar.Height <= 0)
                       return;

                   //rectPar est La taille Dispose pour afficher la VideoSource avec les regles endPoint plus
                   Size visusize = CoreSystem.Instance.Etals.Clipper.processChangeVisuRect(rectPar); // on recoit la taille de la fenetre de visu de la video
                   Rectangle videorect = new Rectangle(0, 0, visusize.Width, visusize.Height);

                   picvideo.SetBounds(0, 0, videorect.Width, videorect.Height);
                   pictRuleTop.SetBounds(videorect.Left, videorect.Bottom + 2, videorect.Width, 0, BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Width);
                   pictRuleLeft.SetBounds(videorect.Right + 2, videorect.Top, 0, videorect.Height, BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Height);

                   // on redimensionne le panel video
                   int newwidth = pictRuleLeft.Right + 2; // width de l'ensemble video + ruler
                   int newheight = pictRuleTop.Bottom + 2; // height de l'ensemble video + ruler

                   panelsize.SetBounds((this.Width - newwidth) / 2, (this.Height - newheight) / 2, newwidth, newheight);

                   pictRuleTop.Invalidate(); // pour faire repeindre
                   pictRuleLeft.Invalidate();
               }*/
            }


            /// <summary>
            /// On new frame ready
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void camera_NewFrame(object sender, System.EventArgs e)
            {
                //Debug.Write("camnewframe");
                //Invalidate();
            }

        /// <summary>
        /// On alarm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
    
        private void camera_Alarm(object sender, System.EventArgs e)
            {
                // flash for 2 seconds
                flash = (int)(2 * (1000 / timer.Interval));
            }

           /// <summary>
           /// traitement de l'event resize de la camerawindow
           /// </summary>
           /// <param name="sender"></param>
           /// <param name="e"></param>
            private void CameraWindow_Resize(object sender, EventArgs e)
            {
                Control quid = (Control)sender;

                Debug.Write("[camerawindowresize]");
                if (m_Camera != null)
                {//NOTE : Rectangle est une structure donc le = fait une copie
                    Rectangle rectPar = new Rectangle();
                    //Control CSender = (Control)sender;
                    rectPar = this.Parent.ClientRectangle; // sender est en fait this
                    Rectangle savrect = rectPar;
                    rectPar.Width -= pictRuleLeft.Width; // on diminue le rect de la taille des regles pour recalculer la taille de la video
                    rectPar.Height -= pictRuleTop.Height;

                    if (rectPar.Width <= 0 || rectPar.Height <= 0)
                        return;

                    //rectPar est La taille Dispose pour afficher la VideoSource avec les regles endPoint plus
                    Size visusize = CoreSystem.Instance.Etals.Clipper.processChangeVisuRect(rectPar); // on recoit la taille de la fenetre de visu de la video
                    Rectangle videorect = new Rectangle(0, 0, visusize.Width, visusize.Height);

                    picvideo.SetBounds(0, 0, videorect.Width, videorect.Height);
                    pictRuleTop.SetBounds(videorect.Left, videorect.Bottom + 2, videorect.Width, 0, BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Width);
                    pictRuleLeft.SetBounds(videorect.Right + 2, videorect.Top, 0, videorect.Height, BoundsSpecified.X | BoundsSpecified.Y | BoundsSpecified.Height);

                    // on redimensionne le panel video
                    int newwidth = pictRuleLeft.Right + 2; // width de l'ensemble video + ruler
                    int newheight = pictRuleTop.Bottom+ 2; // height de l'ensemble video + ruler

                    panelsize.SetBounds((this.Width-newwidth)/2,(this.Height-newheight)/2,newwidth,newheight);
                   
                    pictRuleTop.Invalidate(); // pour faire repeindre
                    pictRuleLeft.Invalidate();
                }
            }
           
         
         
/// <summary>
/// 
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
           
    private void picvideo_MouseDown(object sender, MouseEventArgs e)
     {
        // on convertit la coord souris depuis camerawindow vers picvideo
        Point ptvisu = picvideo.PointToClient(Cursor.Position);
        

                mouseDown = true;
                 int xbitmap = ptvisu.X;
                int ybitmap = ptvisu.Y;
                startPoint.X = xbitmap; // start et end sont confondus
                startPoint.Y = ybitmap; // start et end sont confondus
                endPoint.X = xbitmap; // start et end sont confondus
                endPoint.Y = ybitmap; // start et end sont confondus

                // un evenement 
                if (ChangingVariable != null)
                    ChangingVariable(this, new ChangeVariable(ChgVarNum.mousedown, xbitmap, ybitmap)); // declanchement de l'event 1

            }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
            private void picvideo_MouseMove(object sender, MouseEventArgs e)
            {Debug.Write("[x="+e.X+"Y="+e.Y+"]");
             Point ptvisu = picvideo.PointToClient(Cursor.Position);
        
                if (mouseDown)
                {   int xbitmap = ptvisu.X; // (int)(e.X / (double)pictrect.Width * (double)locbuffer.Width);
                    int ybitmap = ptvisu.Y; // (int)(e.Y / (double)pictrect.Height * (double)locbuffer.Height);

                    endPoint.X = xbitmap; // start et end sont confondus
                    endPoint.Y = ybitmap; // start et end sont confondus

                    if (ChangingVariable != null)
                        ChangingVariable(this, new ChangeVariable(ChgVarNum.mousmov, endPoint.X, endPoint.Y)); // declanchement de l'event 2 : move
                }
                else
                {//idem car on transmet a souris a la fenetre aussi
                    int xbitmap = ptvisu.X; // (int)(e.X / (double)pictrect.Width * (double)locbuffer.Width);
                    int ybitmap = ptvisu.Y; // (int)(e.Y / (double)pictrect.Height * (double)locbuffer.Height);

                    if (ChangingVariable != null)
                        ChangingVariable(this, new ChangeVariable(ChgVarNum.none, e.X, e.Y)); // declanchement de l'event 2 : move
                    }

            }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
            private void picvideo_MouseUp(object sender, MouseEventArgs e)
            {Point ptvisu = picvideo.PointToClient(Cursor.Position);
        
                if (ChangingVariable != null)
                    //ChangingVariable(this, new ChangeVariable(3, ClientRectangle.Width - e.X, endPoint.Y)); // declanchement de l'event 3 : fin de trace
                    ChangingVariable(this, new ChangeVariable(ChgVarNum.mousup, ptvisu.X, endPoint.Y)); // declanchement de l'event 3 : fin de trace

                mouseDown = false;
            

            }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
            private void picvideo_Click(object sender, EventArgs e)
            {

            }

            

        /// <summary>
        /// la souris arrive au dessus de la fenetre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraWindow_MouseEnter(object sender, EventArgs e)
        {
            // variable 10 : la souris entre au dessus de la zone de trace
            if (ChangingVariable != null)
                ChangingVariable(this, new ChangeVariable(ChgVarNum.mousin, 0, 0)); // declanchement de l'event 10 : entree dans la fenetre

            m_IsTarget = true;  // la fenetre est la destination des choses

        }

        /// <summary>
        /// la souris quitte la fenetre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraWindow_MouseLeave(object sender, EventArgs e)
        {// ici il faudrait gerer eventuellement le deplacement du zoom

            // variable 10 : la souris entre au dessus de la zone de trace
            if (ChangingVariable != null)
                ChangingVariable(this, new ChangeVariable(ChgVarNum.mousout,0, 0)); // declanchement de l'event 11 : sortie de fenetre

            m_IsTarget = false ; //la fenetre n'est plus la destination
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.CompareTo(' ')==0)
                if (ChangingVariable != null)
                    ChangingVariable(this, new ChangeVariable(ChgVarNum.sendresu, 0, 0)); // declanchement de l'event 20 : envoyer resultat

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraWindow_Paint(object sender, PaintEventArgs e)
        {
            UpdatePosition();

        }

        private void CameraWindow_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
   
        }// ------- fin de def classe ser control

        

    public enum ChgVarNum {
        none=0, // move pas presse
        mousup, // mous clic up
        mousedown, // mous clic down
        mousmov, // mouse move avec bouton presse
        mousin=10, // mouse entre dans fenetre video
        mousout, // mouse sort de fenetre video
        sendresu=20, // envoi des resultats
        delresu=21 // efface dernier resultat
    };
    
    /// <summary>
    /// eventarg utilise par l'event changevariable 
    /// cet event est utilise quand qqchose d'interactif a ete modifie (mouvement de souris par exemple)
    /// </summary>
    public class ChangeVariable : System.EventArgs
        {
        /// <summary>
        /// attributs 
        /// </summary>
            private ChgVarNum numVariable = 0; // valeur 0 : impossible
            private double valValeurX = 0;
            private double valValeurY = 0;

            /// <summary>
            /// 
            /// </summary>
            public ChangeVariable()
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="num">numero de vraiable</param>
            /// <param name="valeurx">premiere valeur (x en general)</param>
            /// <param name="valeury"duexieme valeur (y en general)></param>
            public ChangeVariable(ChgVarNum num, double valeurx, double valeury)
            {
                setvalue(num, valeurx, valeury);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="numvariable"></param>
            /// <param name="valeurx"></param>
            /// <param name="valeury"></param>
            public void setvalue(ChgVarNum numvariable, double valeurx, double valeury)
            {
                numVariable = numvariable;
                valValeurX = valeurx;
                valValeurY = valeury;
            }


            /// <summary>
            /// variable get renvoyant le num
            /// </summary>
            public ChgVarNum num
            {
                get { return numVariable; }
            }

        /// <summary>
        /// @brief variable get renvoyant la valeur
        /// </summary>
        public double valeurx
            {
                get { return valValeurX; }
            }
/// <summary>
/// 
/// </summary>
            public double valeury
            {
                get { return valValeurY; }
            }
        
    }
}


 
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
using System.IO;
using clsimgutils;
using System.Threading;
using SplashScreenThreaded;
using serialcode;

namespace mesure
{

   
    /// <summary>
    /// Summary description for MainForm
    /// </summary>
    /// on implement imessagefilter a cause des messages du twain
    public partial class  MainForm : System.Windows.Forms.Form
    {
        // conteneur global 
        //static CoreSystem m_Core = new CoreSystem();

        static bool _Runing = true;  // rebouclage pour reboot
        static bool m_nodefconfig = false; // reboot sans relecture de la config

         // statistics
        private const int statLength = 15;
        private int statIndex = 0, statReady = 0;
        private int[] statCount = new int[statLength];

      
        private double dMesDebX = 0.0;
        private double dMesDebY = 0.0;
        private double dMesFinX = 0.0;
        private double dMesFinY = 0.0;

        private AVIWriter writer = null;
        private bool saveOnMotion = false;

        private System.Windows.Forms.MenuItem fileItem;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem exitFileItem;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Timers.Timer timer;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.StatusBarPanel fpsPanel;
        private System.Windows.Forms.MenuItem motionItem;
        private System.Windows.Forms.MenuItem openURLFileItem;
        private System.Windows.Forms.MenuItem menuopenDShowPeriph;
        private System.Windows.Forms.MenuItem openMJEPGFileItem;
        private System.Windows.Forms.MenuItem helpItem;
        private System.Windows.Forms.MenuItem aboutHelpItem;
        //private CameraWindow cameraWindow;
        private MenuItem menuOpenImage;
        private DataGridViewTextBoxColumn objectif;
        private DataGridViewImageColumn status;
        //private DataGridViewButtonColumn butetal;
        private IContainer components;
        private bool notprinting = true;


        private paramGUI m_GUIParam = new paramGUI();
        protected ArrayList panelList = new ArrayList();
        private System.Windows.Forms.Timer timer1;
        
         // splashscreen
        const int kSplashUpdateInterval_ms = 50;
        const int kMinAmountOfSplashTime_ms = 8000;

        // timer si pas de flux d'image
        object dummy;
        System.Threading.Timer m_Timer; 
        

        // impression
        bool toprint = false;
        bool ontimerprint = false;
        Bitmap toprintbmp = null;
#if DEBUG
#else

        SplashScreenForm sf = new SplashScreenForm();
#endif
       
        //// archivage automatique : a enregistrer dans le default
        //private string m_autoSaveImaPath = ".";
        //private string m_autoSaveImaSuffix = "image";
        //private int m_autoSaveImaIdx = 0;
        //private System.Drawing.Imaging.ImageFormat m_autoSaveImaFormat = System.Drawing.Imaging.ImageFormat.Jpeg;

        
        private Color m_DrawColor = Color.Red; // couleur de trace

        MesureForm m_dialmesure = null;

        // boite de dialogue etalonnage
        private EtalForm fEtalForm = null;


        //fields
        //the Direct3D device
        public Microsoft.DirectX.Direct3D.Device device;
       
        private MenuItem MenuCalculLigne;
        private MenuItem MenuCalculHor;
        private MenuItem MenuCalculVert;
        private MenuItem menuReglages;
        private MenuItem menuPrintrapport;
        private MenuItem openTwain;
        private StatusBarPanel StatusbarResult;
        private StatusBarPanel statusbarobj;
        private ImageList imagelist;
        private Panel panel;
        private SplitContainer splitFenPPale;
        private Panel panel2;
        private CameraWindow camwin;
        private FlowLayoutPanel flowLayoutPanel1;
        private ToolStrip toolStripGeneral;
        private ToolStrip toolStripMotionTools;
        private MenuItem menuOpenConfig;
        private MenuItem menuSaveConfig;
        private wyDay.Controls.VistaMenu vistaMenu1;
        private MenuItem menuSource;
        private MenuItem menuOpenFilm;
        private MenuItem menuEnregistrerSous;
        private MenuItem menusaveauto;
        private MenuItem menuItem13;
        private MenuItem menuResultats;
        private MenuItem menuResuInit;
        private MenuItem menuSavXls;
        private MenuItem MenuSaveRapport;
        private MenuItem menuItem18;
        private MenuItem menuModifRapport;
        private ToolStripButton saveToolStripButton;
        private ToolStripButton printToolStripButton;
        private ToolStripSeparator toolStripSeparator;
        //private ToolStripDropDownButton toolStripDropEtal;
        private ToolStripDropDownButton toolStripDropEtal;
        
        
        private ToolStripSplitButton toolStripSplitButton;
        private ToolStripMenuItem pixelToolStripMenuItem;
        private ToolStripMenuItem ligneToolStripMenuItem;
        private ToolStripMenuItem verticalToolStripMenuItem;
        private ToolStripMenuItem horizontalToolStripMenuItem;
        
        private MenuItem menuPrintImage;
        private MenuItem menuOptions;
        private ColorDialog colorDialog1;
        private MenuItem menuSelColorGenerale;
        private ImageList imageListEtal;
        private MenuItem menuParamEnregAuto;
        private TabControl mesuretab; /// tabcontrol contenant les 2 pages
        private TabPage microtab; // page reglage objectifs
        private TabPage mestab;  // page afficahge mesure
        
        private SplitContainer splitTabReglages;
        private MenuItem menuNewConfig;
        private MenuItem menuFichier;

        private string m_echantillon;
        private GroupBox GrpBoxObj;
        private DataGridView dataGridEtal;
        private Panel panAddDel;
        private Button supobj;
        private Button addobj;
        private GroupBox groupBoxMesure;
        private FlowLayoutPanel flowLayoutPanel2;
        private DataGridView dataGridResuImmediat;
        private DataGridViewTextBoxColumn mesure;
        private DataGridViewTextBoxColumn valeur;
        private GroupBox groupdatarecord;
        private DataGridView dataGridViewResuRecord;
        private DataGridViewImageColumn objstatus;
        private DataGridViewTextBoxColumn objObjectif;
        private MenuItem menuEnregXls;
        private MenuItem menuEnregAuto;
        private MenuItem menuReglEnregImage;
        private MenuItem menuItem4;
        private MenuItem menuRecordAVIOpen;
        private MenuItem menuRecordAviDemarre;
        private MenuItem menuItem7;
        private ToolStripDropDownButton toolStripDropOutils;
        private ToolStripMenuItem toolStripMenuLongueur;
        private ToolStripMenuItem ToolStripMenuItemHauteur;
        private ToolStripMenuItem ToolStripMenuItemLargeur;
        //private ToolStripButton toolStripButRegl;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem ToolStripMenuItemEtalPix;
        private ToolStripButton ToolStripButOpen;
        private ToolStripButton ToolStripButSave;
        //private ToolStripButton ToolStripButPrint;
        private ToolStripSeparator toolStripSeparator3;
        private MenuItem menuReglEnregResu;
        private MenuItem menuImage;
        private MenuItem menuImageLive;
        private MenuItem menuImagePhoto;
        //private MenuItem menuItem6;
        private ToolStripButton toolStripButFreeze;
        private ToolStripButton toolStripButSnap;
        private ToolStripButton toolStripReglage;
        private ToolStripButton ToolStripButNew;
        private ToolStripButton toolStripButPrint;
        static bool m_chklock=true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// _Runin

        
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 0)
                if (args[0] == "pqscsq2snt")
                    m_chklock = false;


            try
            {
                ImgUtil imgutil = new ImgUtil();
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Le programme est mal installé. DLL manquante", "Motion", MessageBoxButtons.OK);
                return;
            }

          
            //MessageBox.Show("Demarrage", "Motion", MessageBoxButtons.YesNo);
        
            while (_Runing)
            {
                _Runing = false; 
                try
                {Application.Run(new MainForm());
                }
                catch (SecurityException e)
                    {
                     if (e.NumException == 1)
                        _Runing = true;
                    }
            }

        }

        /// <summary>
        /// renvoie la fenetre dialmesure pour la repositionner
        /// </summary>
        public Form pDialMesure
        {
            get { return m_dialmesure; }
        }


        ///GESTION DU SPLASHSCREEN
        ///

        /// <summary>
        /// Instance of the splash form.
        /// </summary>
        

       

	
        /// <summary>
        /// Paint the form background only if the splash screen is gone
        /// </summary>
        /// <param name="e">Paint event arguments</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
       
            base.OnPaintBackground(e);
        }


        /// <summary>
        /// constructuer de la form
        /// </summary>
        public MainForm()
        {
            //MessageBox.Show("Demarrage 1", "Motion", MessageBoxButtons.YesNo);
            // demarrage du splash
            this.Hide();
            CoreSystem.Instance.initGui(this);
            Thread splashthread = new Thread(new ThreadStart(SplashScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
#if DEBUG
#else
            splashthread.Start();
#endif
            // fin du splash
           
            // test de la licence
#if DEBUG
#else

            SplashScreen.UdpateStatusText("Vérification de la license");
#endif

#if DEBUG
#else
            Thread.Sleep(400);
#endif

            string licname;// = Application.ExecutablePath.Clone(); // path de l'exe            
            licname = Application.ExecutablePath.ToLower().Replace(".exe", ".lic");
            //licname = licname.Replace(".exe", ".lic");
            
            // on charge le fichier de licence 
            protectionclass laprotectionclass = new protectionclass();
            int retour=0;
#if DEBUG
#else

            if (m_chklock)            
                retour = laprotectionclass.loadlicfile(licname);
#endif
            

            // onteste la valeur retournee
            if (retour !=0)
                {
#if DEBUG
#else
SplashScreen.CloseSplashScreen();
#endif
                GesCodForm DialCode = new GesCodForm();
                 DialCode.CodeMessage = retour;
                 if (DialCode.ShowDialog()== DialogResult.Cancel)
                    throw new SecurityException(1); // 1 : on reboot                 
                 else
                     throw new SecurityException(0); // 0 : on quitte
             }

#if DEBUG
#else
            SplashScreen.UdpateLicence("Licence permanente accordée à " + laprotectionclass.getnomuser());
            Thread.Sleep(400);
#endif

             InitSettingsXml(); // configuration generale du programme
            //MessageBox.Show("Demarrage 2", "Motion", MessageBoxButtons.YesNo);
        
            //
            // Required for Windows Form Designer support
            //
            Application.EnableVisualStyles();
            //            MessageBox.Show("Demarrage 3", "Motion", MessageBoxButtons.YesNo);

#if DEBUG
#else
            SplashScreen.UdpateStatusText("Initialisation Graphique");
            Thread.Sleep(200);
#endif

            InitializeComponent();
            //          MessageBox.Show("Demarrage 4", "Motion", MessageBoxButtons.YesNo);

#if DEBUG
#else
            SplashScreen.UdpateStatusText("Initialisation des filtres DirectShow");
            Thread.Sleep(200);
#endif

            InitializeTDPanels();
            #if DEBUG
#else

            SplashScreen.UdpateStatusText("initialisation Direct X");
#endif
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
          //  MessageBox.Show("Demarrage 5", "Motion", MessageBoxButtons.YesNo);
          
            InitializeGraphics();
            //MessageBox.Show("Demarrage 6", "Motion", MessageBoxButtons.YesNo);
            m_Timer = new System.Threading.Timer(new TimerCallback(tcallback), dummy, 0, 100);            
         }
        
        void tcallback(object o)
        {bool hastimer =false;

        if (CoreSystem.Instance.Camera != null)
            if (CoreSystem.Instance.Camera.source != null)
                if (!CoreSystem.Instance.Camera.source.isfreeze())
                    hastimer = false;


        if (!hastimer) // pas de flux d'image : on lance les operations periodiques
        {
            timeroperation(); 
        }

        }
        


        /// <summary>
        /// fermeture de mainform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {// Determine if text has changed in the textbox by comparing to original text.
         // Display a MsgBox asking the user to save changes or abort.
            logger.log("avt question");
            e.Cancel=false; // on ferme vraiment on n'abandonne pas le close
            if (_Runing)
                {
                CloseFile();
                return;
                }



            if (MessageBox.Show("Voulez vous vraiment quitter ?", "Motion",
            MessageBoxButtons.YesNo) ==  DialogResult.No)
         {
            // Cancel the Closing event from closing the form.
            e.Cancel = true;
            _Runing = false; // on annule le rebouclage
            // Call method to save file...
         }

            //foreach (TDPanel tdPanel in panelList)
            //    panelList[0] = null;
            //    panelList[1] = null;
            
            logger.log("mainfrm clos");

            logger.log("mainfrm close");
            this.SaveDefaultXml(); // enregistre la config
            logger.log("savxml ok");
//             MessageBox.Show("10 : Xml ok", "Motion",MessageBoxButtons.OK); 
            CloseFile();
  //          MessageBox.Show("20 : CloseFile ok", "Motion",MessageBoxButtons.OK);
         
            logger.log("closefile ok");
        }

    
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region evenements menus

        /// <summary>
        /// reponse menu auoclic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menusaveauto_Click(object sender, EventArgs e)
        {
            autoSaveImage();
        }


        /// <summary>
        /// menu config new : fait un reboot sasn lecture du default.xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuNewConfig_Click(object sender, EventArgs e)
        {
            m_nodefconfig = true; // pas de relecture de la config
            this.reboot(); // reboot : tous parametres remais a zero
            // passe directement en mode choix camera mais ca peut etre modifie
        }

        
        /// <summary>
        /// menu file save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSaveFile(object sender, EventArgs e)
        {
            saveconfig();
        }


        /// <summary>
        /// icone enregistrer sous : idema a menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            menuEnregistrerImgSous_Click(sender, e);
        }


        /// <summary>
        /// menu enregistrer sous
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuEnregistrerImgSous_Click(object sender, EventArgs e)
        {
            SaveFileDialog ledial = new SaveFileDialog();
            ledial.DefaultExt = "jpg";
            ledial.FileName = "sansnom";
            ledial.Filter = "Images JPG (*.jpg)|*.jpg|Images BMP (*.bmp)|*.bmp|Images TIFF (*.tif)|*.tif||";
            ledial.InitialDirectory = CoreSystem.Instance.ParamSauvImg.m_ResPath;

            if (ledial.ShowDialog(this) == DialogResult.OK)
            {
                string ext = ledial.DefaultExt; // lecture de l'extension choisie
                string nomfic = ledial.FileName;
                saveImage(nomfic, ext); // pas de verifi si default est charge
            };
        }

        /// <summary>
        ///reponse menu reglage autoclic : ouverture de la boite de dialogue reglage autosave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuParamEnregImgAuto_Click(object sender, EventArgs e)
        { /// creation de la boite de dialogue autosave
            ReglageAutoSauvImg ledial = new ReglageAutoSauvImg();

            // chargement des valeurs courantes
            ledial.m_paramsauv = (paramsavimg)CoreSystem.Instance.ParamSauvImg.Clone();


            // execution de la boite de dialogue
            if (ledial.ShowDialog(this) == DialogResult.OK)
            {
             CoreSystem.Instance.RebuildParamSauvImg((paramsavimg)ledial.m_paramsauv.Clone());                
             }
            
            ledial.Dispose();

        }


        /// <summary>
        /// menu parametres reglage enregistremetn automatique fichier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuParamEnregResuAuto_Click(object sender, EventArgs e)
        { /// creation de la boite de dialogue autosave
            if (m_dialmesure.Visible)
                m_dialmesure.Hide(); // masque le dial de mesure, ce qui provoquera la mise a jour de param enabled ou non au reaffichage

            ReglagesMesure ledial = new ReglagesMesure();

            // chargement des valeurs courantes dans le dial
            ledial.m_parametres = (paramsavres)CoreSystem.Instance.ParamSauvRes.Clone();
            ledial.m_parametres.m_EmptyResuStatus = IsEmptyResurecord() ?0:1; 

            // execution de la boite de dialogue
            if (ledial.ShowDialog(this) == DialogResult.OK)
            {
                CoreSystem.Instance.RebuildParamSauvRes((paramsavres)ledial.m_parametres.Clone());
            }
            
            ledial.Dispose(); // on libere tout de suite

            if (CoreSystem.Instance.ParamSauvRes.m_EmptyResuStatus == 2)  // il faut clearer les resultats
                ClearTabResuRecord();


            // on ajoute ou supprime les colonnes tab et obj
            // c'est pas top de faire ca ici il faudrait le faire aussi a la lecture du xml et donc
            // avoir des fonctions qui s'en chargent au niveau du rescumul
            if (CoreSystem.Instance.ParamSauvRes.m_SavObj)
                CoreSystem.Instance.ResCumul.AddColonne(-1,coltype.legendefin,"Obj");
            else
                CoreSystem.Instance.ResCumul.DelColonne("Obj");

            if (CoreSystem.Instance.ParamSauvRes.m_SavParam)
                CoreSystem.Instance.ResCumul.AddColonne(-1, coltype.legendedeb, "Nom");
            else
                CoreSystem.Instance.ResCumul.DelColonne("Nom");

            // dans tous les cas on reaffiche le tableau de resultats
            updateresdatagrid();
        }


        /// <summary>
        /// meun impression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuPrtRapport_Click(object sender, EventArgs e)
        {
            if (m_dialmesure.Visible)
                m_dialmesure.Hide();
            toprint = true;
        }


        /// <summary>
        /// reponse au menu file exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitFileItem_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// menu reboot (a supprimer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem5_Click(object sender, EventArgs e)
        {
        reboot();
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        private void reboot()
        {_Runing = true;
         this.Close();
        }
        
        
        /// <summary>
        /// reponse au menu about
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutHelpItem_Click(object sender, System.EventArgs e)
        {
            AboutForm form = new AboutForm();

            form.ShowDialog();
        }

        /// <summary>
        /// menu selection couleur ligne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSelColorGenerale_Click(object sender, EventArgs e)
        {
            ColorDialog MyDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            MyDialog.AllowFullOpen = false;
            // Allows the user to get help. (The default is false.)
            MyDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
            MyDialog.Color = m_DrawColor;

            // Update the text box color if the user clicks OK 
            if (MyDialog.ShowDialog(this) == DialogResult.OK)
            {
                m_DrawColor = MyDialog.Color;
                if (CoreSystem.Instance.Calculator != null)
                    CoreSystem.Instance.Calculator.SetColor(m_DrawColor);
            }

        }

        /// <summary>
        /// bouton toobox print rapport
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            if (m_dialmesure.Visible)
                m_dialmesure.Hide();
            toprint = true;
        }


        /// <summary>
        /// sauvegarde les resultats mesure en xls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuVisuExport_Click(object sender, EventArgs e)
        {
         visuExport();            
        }

        #endregion


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.SplitContainer splitToolBar;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            mesure.RulerAxis rulerAxis1 = new mesure.RulerAxis();
            mesure.RulerAxis rulerAxis2 = new mesure.RulerAxis();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.toolStripGeneral = new System.Windows.Forms.ToolStrip();
            this.ToolStripButNew = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButOpen = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            //this.ToolStripButPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripReglage = new System.Windows.Forms.ToolStripButton();
            this.toolStripButFreeze = new System.Windows.Forms.ToolStripButton();
            this.toolStripButSnap = new System.Windows.Forms.ToolStripButton();
            this.toolStripMotionTools = new System.Windows.Forms.ToolStrip();
            this.toolStripDropEtal = new System.Windows.Forms.ToolStripDropDownButton();
            this.ToolStripMenuItemEtalPix = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropOutils = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuLongueur = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemHauteur = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLargeur = new System.Windows.Forms.ToolStripMenuItem();
            this.panel = new System.Windows.Forms.Panel();
            this.splitFenPPale = new System.Windows.Forms.SplitContainer();
            this.mesuretab = new System.Windows.Forms.TabControl();
            this.microtab = new System.Windows.Forms.TabPage();
            this.splitTabReglages = new System.Windows.Forms.SplitContainer();
            this.GrpBoxObj = new System.Windows.Forms.GroupBox();
            this.panAddDel = new System.Windows.Forms.Panel();
            this.supobj = new System.Windows.Forms.Button();
            this.addobj = new System.Windows.Forms.Button();
            this.dataGridEtal = new System.Windows.Forms.DataGridView();
            this.objstatus = new System.Windows.Forms.DataGridViewImageColumn();
            this.objObjectif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxMesure = new System.Windows.Forms.GroupBox();
            this.mestab = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.dataGridResuImmediat = new System.Windows.Forms.DataGridView();
            this.mesure = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valeur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupdatarecord = new System.Windows.Forms.GroupBox();
            this.dataGridViewResuRecord = new System.Windows.Forms.DataGridView();
            this.camwin = new mesure.CameraWindow();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.ligneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.horizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.fileItem = new System.Windows.Forms.MenuItem();
            this.menuNewConfig = new System.Windows.Forms.MenuItem();
            this.menuOpenConfig = new System.Windows.Forms.MenuItem();
            this.menuSaveConfig = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuSource = new System.Windows.Forms.MenuItem();
            this.menuopenDShowPeriph = new System.Windows.Forms.MenuItem();
            this.menuOpenFilm = new System.Windows.Forms.MenuItem();
            this.menuOpenImage = new System.Windows.Forms.MenuItem();
            this.openURLFileItem = new System.Windows.Forms.MenuItem();
            this.openMJEPGFileItem = new System.Windows.Forms.MenuItem();
            this.openTwain = new System.Windows.Forms.MenuItem();
            this.menuFichier = new System.Windows.Forms.MenuItem();
            this.menuEnregistrerSous = new System.Windows.Forms.MenuItem();
            this.menusaveauto = new System.Windows.Forms.MenuItem();
            this.menuItem13 = new System.Windows.Forms.MenuItem();
            this.menuPrintImage = new System.Windows.Forms.MenuItem();
            this.exitFileItem = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuRecordAVIOpen = new System.Windows.Forms.MenuItem();
            this.menuRecordAviDemarre = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuImage = new System.Windows.Forms.MenuItem();
            this.menuImageLive = new System.Windows.Forms.MenuItem();
            this.menuImagePhoto = new System.Windows.Forms.MenuItem();
            this.motionItem = new System.Windows.Forms.MenuItem();
            this.MenuCalculLigne = new System.Windows.Forms.MenuItem();
            this.MenuCalculHor = new System.Windows.Forms.MenuItem();
            this.MenuCalculVert = new System.Windows.Forms.MenuItem();
            this.menuResultats = new System.Windows.Forms.MenuItem();
            this.menuResuInit = new System.Windows.Forms.MenuItem();
            this.menuSavXls = new System.Windows.Forms.MenuItem();
            this.menuEnregXls = new System.Windows.Forms.MenuItem();
            this.menuEnregAuto = new System.Windows.Forms.MenuItem();
            this.menuItem18 = new System.Windows.Forms.MenuItem();
            this.MenuSaveRapport = new System.Windows.Forms.MenuItem();
            this.menuPrintrapport = new System.Windows.Forms.MenuItem();
            this.menuModifRapport = new System.Windows.Forms.MenuItem();
            this.menuOptions = new System.Windows.Forms.MenuItem();
            this.menuSelColorGenerale = new System.Windows.Forms.MenuItem();
            this.menuParamEnregAuto = new System.Windows.Forms.MenuItem();
            this.menuReglEnregImage = new System.Windows.Forms.MenuItem();
            this.menuReglEnregResu = new System.Windows.Forms.MenuItem();
            this.menuReglages = new System.Windows.Forms.MenuItem();
            this.helpItem = new System.Windows.Forms.MenuItem();
            this.aboutHelpItem = new System.Windows.Forms.MenuItem();
            this.timer = new System.Timers.Timer();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.fpsPanel = new System.Windows.Forms.StatusBarPanel();
            this.StatusbarResult = new System.Windows.Forms.StatusBarPanel();
            this.statusbarobj = new System.Windows.Forms.StatusBarPanel();
            this.objectif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewImageColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.imagelist = new System.Windows.Forms.ImageList(this.components);
            this.pixelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.vistaMenu1 = new wyDay.Controls.VistaMenu(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.imageListEtal = new System.Windows.Forms.ImageList(this.components);
            splitToolBar = new System.Windows.Forms.SplitContainer();
            splitToolBar.Panel1.SuspendLayout();
            splitToolBar.Panel2.SuspendLayout();
            splitToolBar.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.toolStripGeneral.SuspendLayout();
            this.toolStripMotionTools.SuspendLayout();
            this.panel.SuspendLayout();
            this.splitFenPPale.Panel1.SuspendLayout();
            this.splitFenPPale.Panel2.SuspendLayout();
            this.splitFenPPale.SuspendLayout();
            this.mesuretab.SuspendLayout();
            this.microtab.SuspendLayout();
            this.splitTabReglages.Panel1.SuspendLayout();
            this.splitTabReglages.Panel2.SuspendLayout();
            this.splitTabReglages.SuspendLayout();
            this.GrpBoxObj.SuspendLayout();
            this.panAddDel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridEtal)).BeginInit();
            this.mestab.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResuImmediat)).BeginInit();
            this.groupdatarecord.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResuRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusbarResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusbarobj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vistaMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitToolBar
            // 
            splitToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            splitToolBar.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitToolBar.IsSplitterFixed = true;
            splitToolBar.Location = new System.Drawing.Point(0, 0);
            splitToolBar.Name = "splitToolBar";
            splitToolBar.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitToolBar.Panel1
            // 
            splitToolBar.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitToolBar.Panel2
            // 
            splitToolBar.Panel2.Controls.Add(this.panel);
            splitToolBar.Size = new System.Drawing.Size(799, 51);
            splitToolBar.SplitterDistance = 25;
            splitToolBar.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("flowLayoutPanel1.BackgroundImage")));
            this.flowLayoutPanel1.Controls.Add(this.toolStripGeneral);
            this.flowLayoutPanel1.Controls.Add(this.toolStripMotionTools);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(799, 25);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // toolStripGeneral
            // 
            this.toolStripGeneral.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripButNew,
            this.ToolStripButOpen,
            this.ToolStripButSave,
            this.toolStripButPrint,
            this.toolStripSeparator2,
            //this.ToolStripButPrint,
            //this.toolStripSeparator3,
            this.toolStripReglage,
            this.toolStripButFreeze,
            this.toolStripButSnap});
            this.toolStripGeneral.Location = new System.Drawing.Point(0, 0);
            this.toolStripGeneral.Name = "toolStripGeneral";
            this.toolStripGeneral.Size = new System.Drawing.Size(271, 25);
            this.toolStripGeneral.TabIndex = 0;
            this.toolStripGeneral.Text = "toolStrip1";
            // 
            // ToolStripButNew
            // 
            this.ToolStripButNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButNew.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButNew.Image")));
            this.ToolStripButNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButNew.Name = "ToolStripButNew";
            this.ToolStripButNew.Size = new System.Drawing.Size(23, 22);
            this.ToolStripButNew.Text = "&New";
            this.ToolStripButNew.Click += new System.EventHandler(this.ToolStripButNew_Click);
            // 
            // ToolStripButOpen
            // 
            this.ToolStripButOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButOpen.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButOpen.Image")));
            this.ToolStripButOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButOpen.Name = "ToolStripButOpen";
            this.ToolStripButOpen.Size = new System.Drawing.Size(23, 22);
            this.ToolStripButOpen.Text = "&Open";
            this.ToolStripButOpen.Click += new System.EventHandler(this.ToolStripButOpen_Click);
            // 
            // ToolStripButSave
            // 
            this.ToolStripButSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButSave.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButSave.Image")));
            this.ToolStripButSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButSave.Name = "ToolStripButSave";
            this.ToolStripButSave.Size = new System.Drawing.Size(23, 22);
            this.ToolStripButSave.Text = "&Save";
            this.ToolStripButSave.Click += new System.EventHandler(this.ToolStripButSave_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButPrint.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButPrint.Name = "toolStripButPrint";
            this.toolStripButPrint.Size = new System.Drawing.Size(23, 22);
            this.toolStripButPrint.Text = "&Print";            
            this.toolStripButPrint.Click += new System.EventHandler(this.toolStripButPrint_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ToolStripButPrint
            // 
            //this.ToolStripButPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            //this.ToolStripButPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            //this.ToolStripButPrint.Name = "ToolStripButPrint";
            //this.ToolStripButPrint.Size = new System.Drawing.Size(23, 22);
            //this.ToolStripButPrint.Text = "&Print";
            //this.ToolStripButPrint.Click += new System.EventHandler(this.ToolStripButPrint_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripReglage
            // 
            this.toolStripReglage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripReglage.Image = ((System.Drawing.Image)(resources.GetObject("toolStripReglage.Image")));
            this.toolStripReglage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripReglage.Name = "toolStripReglage";
            this.toolStripReglage.Size = new System.Drawing.Size(55, 22);
            this.toolStripReglage.Text = "Reglages";
            this.toolStripReglage.Click += new System.EventHandler(this.toolStripReglage_Click);
            // 
            // toolStripButFreeze
            // 
            this.toolStripButFreeze.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButFreeze.Name = "toolStripButFreeze";
            this.toolStripButFreeze.Size = new System.Drawing.Size(44, 22);
            this.toolStripButFreeze.Text = "Freeze";
            this.toolStripButFreeze.Click += new System.EventHandler(this.toolStripButFreeze_Click);
            // 
            // toolStripButSnap
            // 
            this.toolStripButSnap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButSnap.Name = "toolStripButSnap";
            this.toolStripButSnap.Size = new System.Drawing.Size(35, 22);
            this.toolStripButSnap.Text = "Snap";
            this.toolStripButSnap.Click += new System.EventHandler(this.toolStripButSnap_Click);
            // 
            // toolStripMotionTools
            // 
            this.toolStripMotionTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropEtal,
            this.toolStripDropOutils});
            this.toolStripMotionTools.Location = new System.Drawing.Point(271, 0);
            this.toolStripMotionTools.Name = "toolStripMotionTools";
            this.toolStripMotionTools.Size = new System.Drawing.Size(155, 25);
            this.toolStripMotionTools.TabIndex = 1;
            // 
            // toolStripDropEtal
            // 
            this.toolStripDropEtal.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemEtalPix});
            this.toolStripDropEtal.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripDropEtal.Name = "toolStripDropEtal";
            this.toolStripDropEtal.Size = new System.Drawing.Size(74, 22);
            this.toolStripDropEtal.Text = "Etalonnage";
            this.toolStripDropEtal.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripDropDn_DropDownItemClicked);
            this.toolStripDropEtal.Click += new System.EventHandler(this.toolStripDropEtal_Click);
            // 
            // ToolStripMenuItemEtalPix
            // 
            this.ToolStripMenuItemEtalPix.Name = "ToolStripMenuItemEtalPix";
            this.ToolStripMenuItemEtalPix.Size = new System.Drawing.Size(107, 22);
            this.ToolStripMenuItemEtalPix.Text = "Pixel";
            // 
            // toolStripDropOutils
            // 
            this.toolStripDropOutils.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuLongueur,
            this.ToolStripMenuItemHauteur,
            this.ToolStripMenuItemLargeur});
            this.toolStripDropOutils.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropOutils.Image")));
            this.toolStripDropOutils.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropOutils.Name = "toolStripDropOutils";
            this.toolStripDropOutils.Size = new System.Drawing.Size(71, 22);
            this.toolStripDropOutils.Text = "mesure";
            // 
            // toolStripMenuLongueur
            // 
            this.toolStripMenuLongueur.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuLongueur.Image")));
            this.toolStripMenuLongueur.Name = "toolStripMenuLongueur";
            this.toolStripMenuLongueur.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuLongueur.Text = "longueur";
            this.toolStripMenuLongueur.Click += new System.EventHandler(this.toolStripMenuLongueur_Click);
            // 
            // ToolStripMenuItemHauteur
            // 
            this.ToolStripMenuItemHauteur.Name = "ToolStripMenuItemHauteur";
            this.ToolStripMenuItemHauteur.Size = new System.Drawing.Size(127, 22);
            this.ToolStripMenuItemHauteur.Text = "hauteur";
            this.ToolStripMenuItemHauteur.Click += new System.EventHandler(this.hauteurToolStripMenuItem_Click);
            // 
            // ToolStripMenuItemLargeur
            // 
            this.ToolStripMenuItemLargeur.Name = "ToolStripMenuItemLargeur";
            this.ToolStripMenuItemLargeur.Size = new System.Drawing.Size(127, 22);
            this.ToolStripMenuItemLargeur.Text = "largeur";
            this.ToolStripMenuItemLargeur.Click += new System.EventHandler(this.largeurToolStripMenuItem_Click);
            // 
            // panel
            // 
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel.Controls.Add(this.splitFenPPale);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(799, 25);
            this.panel.TabIndex = 3;
            // 
            // splitFenPPale
            // 
            this.splitFenPPale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitFenPPale.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitFenPPale.IsSplitterFixed = true;
            this.splitFenPPale.Location = new System.Drawing.Point(0, 0);
            this.splitFenPPale.Name = "splitFenPPale";
            // 
            // splitFenPPale.Panel1
            // 
            this.splitFenPPale.Panel1.Controls.Add(this.mesuretab);
            // 
            // splitFenPPale.Panel2
            // 
            this.splitFenPPale.Panel2.BackColor = System.Drawing.Color.LightBlue;
            this.splitFenPPale.Panel2.Controls.Add(this.camwin);
            this.splitFenPPale.Size = new System.Drawing.Size(795, 21);
            this.splitFenPPale.SplitterDistance = 256;
            this.splitFenPPale.TabIndex = 2;
            // 
            // mesuretab
            // 
            this.mesuretab.Controls.Add(this.microtab);
            this.mesuretab.Controls.Add(this.mestab);
            this.mesuretab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mesuretab.Location = new System.Drawing.Point(0, 0);
            this.mesuretab.Name = "mesuretab";
            this.mesuretab.SelectedIndex = 0;
            this.mesuretab.Size = new System.Drawing.Size(256, 21);
            this.mesuretab.TabIndex = 0;
            this.mesuretab.SizeChanged += new System.EventHandler(this.mesuretab_SizeChanged);
            // 
            // microtab
            // 
            this.microtab.Controls.Add(this.splitTabReglages);
            this.microtab.Location = new System.Drawing.Point(4, 22);
            this.microtab.Name = "microtab";
            this.microtab.Padding = new System.Windows.Forms.Padding(3);
            this.microtab.Size = new System.Drawing.Size(248, 0);
            this.microtab.TabIndex = 0;
            this.microtab.Text = "Réglages";
            this.microtab.UseVisualStyleBackColor = true;
            // 
            // splitTabReglages
            // 
            this.splitTabReglages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitTabReglages.Location = new System.Drawing.Point(3, 3);
            this.splitTabReglages.Name = "splitTabReglages";
            this.splitTabReglages.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitTabReglages.Panel1
            // 
            this.splitTabReglages.Panel1.Controls.Add(this.GrpBoxObj);
            // 
            // splitTabReglages.Panel2
            // 
            this.splitTabReglages.Panel2.Controls.Add(this.groupBoxMesure);
            this.splitTabReglages.Size = new System.Drawing.Size(242, 0);
            this.splitTabReglages.SplitterDistance = 25;
            this.splitTabReglages.TabIndex = 0;
            // 
            // GrpBoxObj
            // 
            this.GrpBoxObj.Controls.Add(this.panAddDel);
            this.GrpBoxObj.Controls.Add(this.dataGridEtal);
            this.GrpBoxObj.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrpBoxObj.Location = new System.Drawing.Point(0, 0);
            this.GrpBoxObj.Name = "GrpBoxObj";
            this.GrpBoxObj.Size = new System.Drawing.Size(242, 25);
            this.GrpBoxObj.TabIndex = 1;
            this.GrpBoxObj.TabStop = false;
            this.GrpBoxObj.Text = "objectifs";
            // 
            // panAddDel
            // 
            this.panAddDel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panAddDel.Controls.Add(this.supobj);
            this.panAddDel.Controls.Add(this.addobj);
            this.panAddDel.Location = new System.Drawing.Point(194, 16);
            this.panAddDel.Name = "panAddDel";
            this.panAddDel.Size = new System.Drawing.Size(42, 110);
            this.panAddDel.TabIndex = 12;
            // 
            // supobj
            // 
            this.supobj.FlatAppearance.BorderSize = 0;
            this.supobj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.supobj.Image = ((System.Drawing.Image)(resources.GetObject("supobj.Image")));
            this.supobj.Location = new System.Drawing.Point(3, 69);
            this.supobj.Name = "supobj";
            this.supobj.Size = new System.Drawing.Size(31, 29);
            this.supobj.TabIndex = 13;
            this.supobj.UseVisualStyleBackColor = true;
            this.supobj.Click += new System.EventHandler(this.supobj_Click);
            // 
            // addobj
            // 
            this.addobj.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.addobj.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.addobj.FlatAppearance.BorderSize = 0;
            this.addobj.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addobj.Image = ((System.Drawing.Image)(resources.GetObject("addobj.Image")));
            this.addobj.Location = new System.Drawing.Point(3, 15);
            this.addobj.Name = "addobj";
            this.addobj.Size = new System.Drawing.Size(31, 34);
            this.addobj.TabIndex = 12;
            this.addobj.UseVisualStyleBackColor = true;
            this.addobj.Click += new System.EventHandler(this.butaddetal_Click);
            // 
            // dataGridEtal
            // 
            this.dataGridEtal.AllowUserToAddRows = false;
            this.dataGridEtal.AllowUserToDeleteRows = false;
            this.dataGridEtal.AllowUserToResizeColumns = false;
            this.dataGridEtal.AllowUserToResizeRows = false;
            this.dataGridEtal.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dataGridEtal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridEtal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.objstatus,
            this.objObjectif});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridEtal.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridEtal.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridEtal.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGridEtal.Location = new System.Drawing.Point(3, 16);
            this.dataGridEtal.MultiSelect = false;
            this.dataGridEtal.Name = "dataGridEtal";
            this.dataGridEtal.ReadOnly = true;
            this.dataGridEtal.RowHeadersVisible = false;
            this.dataGridEtal.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridEtal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridEtal.Size = new System.Drawing.Size(188, 6);
            this.dataGridEtal.TabIndex = 8;
            this.dataGridEtal.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridEtal_CellContentClick);
            // 
            // objstatus
            // 
            this.objstatus.HeaderText = "Actif";
            this.objstatus.Name = "objstatus";
            this.objstatus.ReadOnly = true;
            this.objstatus.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.objstatus.Width = 40;
            // 
            // objObjectif
            // 
            this.objObjectif.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.objObjectif.HeaderText = "Nom";
            this.objObjectif.Name = "objObjectif";
            this.objObjectif.ReadOnly = true;
            // 
            // groupBoxMesure
            // 
            this.groupBoxMesure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxMesure.Location = new System.Drawing.Point(0, 0);
            this.groupBoxMesure.Name = "groupBoxMesure";
            this.groupBoxMesure.Size = new System.Drawing.Size(242, 25);
            this.groupBoxMesure.TabIndex = 0;
            this.groupBoxMesure.TabStop = false;
            this.groupBoxMesure.Text = "Mesure";
            this.groupBoxMesure.Visible = false;
            // 
            // mestab
            // 
            this.mestab.Controls.Add(this.flowLayoutPanel2);
            this.mestab.Location = new System.Drawing.Point(4, 22);
            this.mestab.Margin = new System.Windows.Forms.Padding(0);
            this.mestab.Name = "mestab";
            this.mestab.Size = new System.Drawing.Size(248, 12);
            this.mestab.TabIndex = 1;
            this.mestab.Text = "mesure";
            this.mestab.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.dataGridResuImmediat);
            this.flowLayoutPanel2.Controls.Add(this.groupdatarecord);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(248, 12);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // dataGridResuImmediat
            // 
            this.dataGridResuImmediat.AllowUserToAddRows = false;
            this.dataGridResuImmediat.AllowUserToDeleteRows = false;
            this.dataGridResuImmediat.AllowUserToResizeColumns = false;
            this.dataGridResuImmediat.AllowUserToResizeRows = false;
            this.dataGridResuImmediat.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridResuImmediat.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridResuImmediat.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dataGridResuImmediat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridResuImmediat.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.mesure,
            this.valeur});
            this.dataGridResuImmediat.Location = new System.Drawing.Point(3, 3);
            this.dataGridResuImmediat.Name = "dataGridResuImmediat";
            this.dataGridResuImmediat.ReadOnly = true;
            this.dataGridResuImmediat.RowHeadersVisible = false;
            this.dataGridResuImmediat.RowHeadersWidth = 20;
            this.dataGridResuImmediat.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridResuImmediat.Size = new System.Drawing.Size(248, 101);
            this.dataGridResuImmediat.TabIndex = 7;
            // 
            // mesure
            // 
            this.mesure.HeaderText = "legende";
            this.mesure.MaxInputLength = 20;
            this.mesure.Name = "mesure";
            this.mesure.ReadOnly = true;
            this.mesure.Width = 80;
            // 
            // valeur
            // 
            this.valeur.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.valeur.HeaderText = "valeur";
            this.valeur.Name = "valeur";
            this.valeur.ReadOnly = true;
            // 
            // groupdatarecord
            // 
            this.groupdatarecord.Controls.Add(this.dataGridViewResuRecord);
            this.groupdatarecord.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupdatarecord.Location = new System.Drawing.Point(3, 110);
            this.groupdatarecord.Name = "groupdatarecord";
            this.groupdatarecord.Size = new System.Drawing.Size(246, 100);
            this.groupdatarecord.TabIndex = 8;
            this.groupdatarecord.TabStop = false;
            this.groupdatarecord.Text = "cumul";
            // 
            // dataGridViewResuRecord
            // 
            this.dataGridViewResuRecord.AllowUserToAddRows = false;
            this.dataGridViewResuRecord.AllowUserToDeleteRows = false;
            this.dataGridViewResuRecord.AllowUserToResizeColumns = false;
            this.dataGridViewResuRecord.AllowUserToResizeRows = false;
            this.dataGridViewResuRecord.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridViewResuRecord.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResuRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewResuRecord.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewResuRecord.MultiSelect = false;
            this.dataGridViewResuRecord.Name = "dataGridViewResuRecord";
            this.dataGridViewResuRecord.ReadOnly = true;
            this.dataGridViewResuRecord.RowHeadersVisible = false;
            this.dataGridViewResuRecord.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewResuRecord.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewResuRecord.Size = new System.Drawing.Size(240, 81);
            this.dataGridViewResuRecord.TabIndex = 0;
            // 
            // camwin
            // 
            this.camwin.AutoSize = true;
            this.camwin.BackColor = System.Drawing.Color.LightBlue;
            this.camwin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.camwin.Camera = null;
            this.camwin.Cursor = System.Windows.Forms.Cursors.No;
            this.camwin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camwin.Location = new System.Drawing.Point(0, 0);
            this.camwin.Name = "camwin";
            this.camwin.Size = new System.Drawing.Size(535, 21);
            this.camwin.TabIndex = 0;
            this.camwin.XScale = rulerAxis1;
            this.camwin.YScale = rulerAxis2;
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripButton.Name = "printToolStripButton";
            this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printToolStripButton.Text = "&Print";
            this.printToolStripButton.Click += new System.EventHandler(this.printToolStripButton_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSplitButton
            // 
            this.toolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ligneToolStripMenuItem,
            this.horizontalToolStripMenuItem,
            this.verticalToolStripMenuItem});
            this.toolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton.Name = "toolStripSplitButton";
            this.toolStripSplitButton.Size = new System.Drawing.Size(58, 22);
            this.toolStripSplitButton.Text = "Mesure";
            this.toolStripSplitButton.ButtonClick += new System.EventHandler(this.toolStripSplitButton_ButtonClick);
            // 
            // ligneToolStripMenuItem
            // 
            this.ligneToolStripMenuItem.Checked = true;
            this.ligneToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ligneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("ligneToolStripMenuItem.Image")));
            this.ligneToolStripMenuItem.Name = "ligneToolStripMenuItem";
            this.ligneToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.ligneToolStripMenuItem.Text = "longueur";
            this.ligneToolStripMenuItem.Click += new System.EventHandler(this.ligneToolStripMenuItem_Click);
            // 
            // horizontalToolStripMenuItem
            // 
            this.horizontalToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("horizontalToolStripMenuItem.Image")));
            this.horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem";
            this.horizontalToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.horizontalToolStripMenuItem.Text = "largeur";
            this.horizontalToolStripMenuItem.Click += new System.EventHandler(this.horizontalToolStripMenuItem_Click);
            // 
            // verticalToolStripMenuItem
            // 
            this.verticalToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("verticalToolStripMenuItem.Image")));
            this.verticalToolStripMenuItem.Name = "verticalToolStripMenuItem";
            this.verticalToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.verticalToolStripMenuItem.Text = "hauteur";
            this.verticalToolStripMenuItem.Click += new System.EventHandler(this.verticalToolStripMenuItem_Click);
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.fileItem,
            this.menuItem4,
            this.menuImage,
            this.motionItem,
            this.menuResultats,
            this.menuOptions,
            this.helpItem});
            // 
            // fileItem
            // 
            this.fileItem.Index = 0;
            this.fileItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuNewConfig,
            this.menuOpenConfig,
            this.menuSaveConfig,
            this.menuItem1,
            this.menuSource,
            this.menuFichier,
            this.menuEnregistrerSous,
            this.menusaveauto,
            this.menuItem13,
            this.menuPrintImage,
            this.exitFileItem});
            this.fileItem.Text = "&Fichier";
            // 
            // menuNewConfig
            // 
            this.menuNewConfig.Index = 0;
            this.menuNewConfig.Text = "&Nouvelle configuration ";
            this.menuNewConfig.Click += new System.EventHandler(this.menuNewConfig_Click);
            // 
            // menuOpenConfig
            // 
            this.menuOpenConfig.Index = 1;
            this.menuOpenConfig.Text = "&Ouvrir configuration ";
            this.menuOpenConfig.Click += new System.EventHandler(this.menuOpenFile);
            // 
            // menuSaveConfig
            // 
            this.menuSaveConfig.Index = 2;
            this.menuSaveConfig.Text = "&Enregistrer Configuration ";
            this.menuSaveConfig.Click += new System.EventHandler(this.menuSaveFile);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // menuSource
            // 
            this.menuSource.Index = 4;
            this.menuSource.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuopenDShowPeriph,
            this.menuOpenFilm,
            this.menuOpenImage,
            this.openURLFileItem,
            this.openMJEPGFileItem,
            this.openTwain});
            this.menuSource.Text = "&Source image";
            // 
            // menuopenDShowPeriph
            // 
            this.menuopenDShowPeriph.Index = 0;
            this.menuopenDShowPeriph.Text = "&Periphérique";
            this.menuopenDShowPeriph.Click += new System.EventHandler(this.menuSourcePeriph_Click);
            // 
            // menuOpenFilm
            // 
            this.menuOpenFilm.Index = 1;
            this.menuOpenFilm.Text = "&Film";
            this.menuOpenFilm.Click += new System.EventHandler(this.menuOpenFilm_Click);
            // 
            // menuOpenImage
            // 
            this.menuOpenImage.Index = 2;
            this.menuOpenImage.Text = " &Image";
            this.menuOpenImage.Click += new System.EventHandler(this.menuSourcImg_Click);
            // 
            // openURLFileItem
            // 
            this.openURLFileItem.Index = 3;
            this.openURLFileItem.Text = "&JPEG URL";
            this.openURLFileItem.Visible = false;
            this.openURLFileItem.Click += new System.EventHandler(this.menuSourcURLJPGFile_Click);
            // 
            // openMJEPGFileItem
            // 
            this.openMJEPGFileItem.Index = 4;
            this.openMJEPGFileItem.Text = "&MJPEG URL";
            this.openMJEPGFileItem.Visible = false;
            this.openMJEPGFileItem.Click += new System.EventHandler(this.menuSourceURLMJEPGItem_Click);
            // 
            // openTwain
            // 
            this.openTwain.Index = 5;
            this.openTwain.Text = "Scanner &Twain";
            this.openTwain.Click += new System.EventHandler(this.menuSourcTwain_Click);
            // 
            // menuFichier
            // 
            this.menuFichier.Index = 5;
            this.menuFichier.Text = "-";
            // 
            // menuEnregistrerSous
            // 
            this.menuEnregistrerSous.Index = 6;
            this.menuEnregistrerSous.Text = "Enregistrer &Image";
            this.menuEnregistrerSous.Click += new System.EventHandler(this.menuEnregistrerImgSous_Click);
            // 
            // menusaveauto
            // 
            this.menusaveauto.Index = 7;
            this.menusaveauto.Text = "Enregistrement &Automatique";
            this.menusaveauto.Click += new System.EventHandler(this.menusaveauto_Click);
            // 
            // menuItem13
            // 
            this.menuItem13.Index = 8;
            this.menuItem13.Text = "-";
            // 
            // menuPrintImage
            // 
            this.menuPrintImage.Index = 9;
            this.menuPrintImage.Text = "Imprimer &Image";
            this.menuPrintImage.Click += new System.EventHandler(this.menuPrintImage_Click);
            // 
            // exitFileItem
            // 
            this.exitFileItem.Index = 10;
            this.exitFileItem.Text = "&Quitter";
            this.exitFileItem.Click += new System.EventHandler(this.exitFileItem_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuRecordAVIOpen,
            this.menuRecordAviDemarre,
            this.menuItem7});
            this.menuItem4.Text = "Enreg. Video";
            this.menuItem4.Visible = false;
            // 
            // menuRecordAVIOpen
            // 
            this.menuRecordAVIOpen.Index = 0;
            this.menuRecordAVIOpen.Text = "Ouvrir Fichier";
            this.menuRecordAVIOpen.Click += new System.EventHandler(this.menuRecordAVIOpen_Click);
            // 
            // menuRecordAviDemarre
            // 
            this.menuRecordAviDemarre.Index = 1;
            this.menuRecordAviDemarre.Text = "Demarrer";
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 2;
            this.menuItem7.Text = "Arreter";
            // 
            // menuImage
            // 
            this.menuImage.Index = 2;
            this.menuImage.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuImageLive,
            this.menuImagePhoto});
            this.menuImage.Text = "Image";
            // 
            // menuImageLive
            // 
            this.menuImageLive.Checked = true;
            this.menuImageLive.Index = 0;
            this.menuImageLive.Text = "Live";
            this.menuImageLive.Click += new System.EventHandler(this.menuImageLive_Click);
            // 
            // menuImagePhoto
            // 
            this.menuImagePhoto.Index = 1;
            this.menuImagePhoto.Text = "Photo";
            this.menuImagePhoto.Click += new System.EventHandler(this.menuImagePhoto_Click);
            // 
            // motionItem
            // 
            this.motionItem.Index = 3;
            this.motionItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuCalculLigne,
            this.MenuCalculHor,
            this.MenuCalculVert});
            this.motionItem.Text = "&Mesure";
            // 
            // MenuCalculLigne
            // 
            this.vistaMenu1.SetImage(this.MenuCalculLigne, ((System.Drawing.Image)(resources.GetObject("MenuCalculLigne.Image"))));
            this.MenuCalculLigne.Index = 0;
            this.MenuCalculLigne.Text = "&Longueur";
            this.MenuCalculLigne.Click += new System.EventHandler(this.menuCalcLigne_Click);
            // 
            // MenuCalculHor
            // 
            this.vistaMenu1.SetImage(this.MenuCalculHor, ((System.Drawing.Image)(resources.GetObject("MenuCalculHor.Image"))));
            this.MenuCalculHor.Index = 1;
            this.MenuCalculHor.Text = "L&argeur";
            this.MenuCalculHor.Click += new System.EventHandler(this.MenuCalculHor_Click);
            // 
            // MenuCalculVert
            // 
            this.vistaMenu1.SetImage(this.MenuCalculVert, ((System.Drawing.Image)(resources.GetObject("MenuCalculVert.Image"))));
            this.MenuCalculVert.Index = 2;
            this.MenuCalculVert.Text = "&Hauteur";
            this.MenuCalculVert.Click += new System.EventHandler(this.menuCalcVert_Click);
            // 
            // menuResultats
            // 
            this.menuResultats.Index = 4;
            this.menuResultats.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuResuInit,
            this.menuSavXls,
            this.menuEnregXls,
            this.menuEnregAuto,
            this.menuItem18,
            this.MenuSaveRapport,
            this.menuPrintrapport,
            this.menuModifRapport});
            this.menuResultats.Text = "&Resultats";
            // 
            // menuResuInit
            // 
            this.menuResuInit.Index = 0;
            this.menuResuInit.Text = "Reinit";
            this.menuResuInit.Click += new System.EventHandler(this.menuResuInit_Click);
            // 
            // menuSavXls
            // 
            this.menuSavXls.Index = 1;
            this.menuSavXls.Text = "Visualiser résultats";
            this.menuSavXls.Click += new System.EventHandler(this.menuVisuExport_Click);
            // 
            // menuEnregXls
            // 
            this.menuEnregXls.Index = 2;
            this.menuEnregXls.Text = "Enregistrer résultats";
            this.menuEnregXls.Click += new System.EventHandler(this.menuEnregresSous_Click);
            // 
            // menuEnregAuto
            // 
            this.menuEnregAuto.Index = 3;
            this.menuEnregAuto.Text = "Enregistrement automatique";
            this.menuEnregAuto.Click += new System.EventHandler(this.menuEnregAuto_Click);
            // 
            // menuItem18
            // 
            this.menuItem18.Index = 4;
            this.menuItem18.Text = "-";
            // 
            // MenuSaveRapport
            // 
            this.MenuSaveRapport.Enabled = false;
            this.MenuSaveRapport.Index = 5;
            this.MenuSaveRapport.Text = "Enregistrer rapport";
            // 
            // menuPrintrapport
            // 
            this.menuPrintrapport.Enabled = false;
            this.menuPrintrapport.Index = 6;
            this.menuPrintrapport.Text = "&Imprimer";
            this.menuPrintrapport.Click += new System.EventHandler(this.menuPrtRapport_Click);
            // 
            // menuModifRapport
            // 
            this.menuModifRapport.Enabled = false;
            this.menuModifRapport.Index = 7;
            this.menuModifRapport.Text = "Modifier rapport";
            // 
            // menuOptions
            // 
            this.menuOptions.Index = 5;
            this.menuOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuSelColorGenerale,
            this.menuParamEnregAuto,
            this.menuReglages});
            this.menuOptions.Text = "Options";
            // 
            // menuSelColorGenerale
            // 
            this.menuSelColorGenerale.Index = 0;
            this.menuSelColorGenerale.Text = "Couleur tracé";
            this.menuSelColorGenerale.Click += new System.EventHandler(this.menuSelColorGenerale_Click);
            // 
            // menuParamEnregAuto
            // 
            this.menuParamEnregAuto.Index = 1;
            this.menuParamEnregAuto.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuReglEnregImage,
            this.menuReglEnregResu});
            this.menuParamEnregAuto.Text = "Réglages Enreg. ";
            // 
            // menuReglEnregImage
            // 
            this.menuReglEnregImage.Index = 0;
            this.menuReglEnregImage.Text = "Image";
            this.menuReglEnregImage.Click += new System.EventHandler(this.menuParamEnregImgAuto_Click);
            // 
            // menuReglEnregResu
            // 
            this.menuReglEnregResu.Index = 1;
            this.menuReglEnregResu.Text = "Résultats";
            this.menuReglEnregResu.Click += new System.EventHandler(this.menuParamEnregResuAuto_Click);
            // 
            // menuReglages
            // 
            this.menuReglages.Index = 2;
            this.menuReglages.Text = "&Reglages Source";
            this.menuReglages.Click += new System.EventHandler(this.menuReglages_Click);
            // 
            // helpItem
            // 
            this.helpItem.Index = 6;
            this.helpItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.aboutHelpItem});
            this.helpItem.Text = "&Aide";
            // 
            // aboutHelpItem
            // 
            this.aboutHelpItem.Index = 0;
            this.aboutHelpItem.Text = "&A propos";
            this.aboutHelpItem.Click += new System.EventHandler(this.aboutHelpItem_Click);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.SynchronizingObject = this;
            this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Elapsed);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 51);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.fpsPanel,
            this.StatusbarResult,
            this.statusbarobj});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(799, 30);
            this.statusBar.TabIndex = 1;
            // 
            // fpsPanel
            // 
            this.fpsPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.fpsPanel.Name = "fpsPanel";
            this.fpsPanel.Width = 583;
            // 
            // StatusbarResult
            // 
            this.StatusbarResult.Name = "StatusbarResult";
            // 
            // statusbarobj
            // 
            this.statusbarobj.Name = "statusbarobj";
            this.statusbarobj.Text = "Objectif";
            // 
            // objectif
            // 
            this.objectif.HeaderText = "Obj.";
            this.objectif.Name = "objectif";
            // 
            // status
            // 
            this.status.HeaderText = "+";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 25;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // imagelist
            // 
            this.imagelist.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagelist.ImageStream")));
            this.imagelist.TransparentColor = System.Drawing.Color.Transparent;
            this.imagelist.Images.SetKeyName(0, "");
            this.imagelist.Images.SetKeyName(1, "");
            this.imagelist.Images.SetKeyName(2, "addobj.bmp");
            this.imagelist.Images.SetKeyName(3, "supobj.bmp");
            this.imagelist.Images.SetKeyName(4, "supobj-ina.bmp");
            // 
            // pixelToolStripMenuItem
            // 
            this.pixelToolStripMenuItem.Name = "pixelToolStripMenuItem";
            this.pixelToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(519, 262);
            this.panel2.TabIndex = 0;
            // 
            // vistaMenu1
            // 
            this.vistaMenu1.ContainerControl = this;
            // 
            // imageListEtal
            // 
            this.imageListEtal.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListEtal.ImageStream")));
            this.imageListEtal.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListEtal.Images.SetKeyName(0, "mespix.bmp");
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(799, 81);
            this.Controls.Add(splitToolBar);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Video Mesureur";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_Closing);
            splitToolBar.Panel1.ResumeLayout(false);
            splitToolBar.Panel2.ResumeLayout(false);
            splitToolBar.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.toolStripGeneral.ResumeLayout(false);
            this.toolStripGeneral.PerformLayout();
            this.toolStripMotionTools.ResumeLayout(false);
            this.toolStripMotionTools.PerformLayout();
            this.panel.ResumeLayout(false);
            this.splitFenPPale.Panel1.ResumeLayout(false);
            this.splitFenPPale.Panel2.ResumeLayout(false);
            this.splitFenPPale.Panel2.PerformLayout();
            this.splitFenPPale.ResumeLayout(false);
            this.mesuretab.ResumeLayout(false);
            this.microtab.ResumeLayout(false);
            this.splitTabReglages.Panel1.ResumeLayout(false);
            this.splitTabReglages.Panel2.ResumeLayout(false);
            this.splitTabReglages.ResumeLayout(false);
            this.GrpBoxObj.ResumeLayout(false);
            this.panAddDel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridEtal)).EndInit();
            this.mestab.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridResuImmediat)).EndInit();
            this.groupdatarecord.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResuRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpsPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StatusbarResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusbarobj)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vistaMenu1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

      
       

        /// <summary>
        /// eventhandler avfecte a camnewframe de la camera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void camera_NewFrame(object sender, System.EventArgs e)
        {

            Bitmap newbitmap=null;

            if (CoreSystem.Instance.Camera.LastFrame == null)
                newbitmap = new Bitmap(CoreSystem.Instance.Camera.Width, CoreSystem.Instance.Camera.Height, PixelFormat.Format32bppArgb);
            
            
            //if (camera.LastFrame != null)
                foreach (TDPanel tdPanel in panelList)
                {
                    tdPanel.TDDevice = device;
                    if (tdPanel.IsVideo)
                    {
                        CoreSystem.Instance.Camera.Lock();
                        if (CoreSystem.Instance.Camera.LastFrame != null)
                            tdPanel.PaintFrame(CoreSystem.Instance.Camera.LastFrame); // peint la voideo dans le plan video
                        else
                            tdPanel.PaintFrame(newbitmap); // peint la voideo dans le plan video
                        CoreSystem.Instance.Camera.Unlock();
                    }
                }

                if (newbitmap != null)
                    newbitmap.Dispose();

            timeroperation(); 
        }


        // operations a realiser sous timer ou syncrones aux images
        //-------------------------------------------------------------
        void timeroperation()
        {
            if (toprint)
            {
                toprint = false;

                if (ontimerprint)
                {// on a deja une image en attente
                    //toprint = false;
                }
                else
                {
                    //toprint = false; // emepeche la reentrance
                    toprintbmp = (Bitmap)CoreSystem.Instance.Camera.LastFrame.Clone();
                    toprintbmp = MakeCartouche(toprintbmp);
                    ontimerprint = true; // le prochain timer lancera une impression
                }
            }
        }
        

        
        bool bMouseDown = false;
        float prvx;
        float prvy;

        int cartlen = 200;

        /// <summary>
        /// cree un cartouche a l'image
        /// // la taille du cartouche est ajuste pour que la reduction de l'image a l'ecran donne des caracteres lisibles
        /// </summary>
        private Bitmap MakeCartouche(Bitmap Srcimg)
        {// le cartouche rajoute 10% d'image
         cartlen = Srcimg.Height/10; // 10% de cartouche
         Bitmap cartbitmap = new Bitmap(Srcimg.Width, Srcimg.Height+cartlen, PixelFormat.Format32bppPArgb);
         Graphics largeGraphics = Graphics.FromImage(cartbitmap); 
         largeGraphics.DrawImage(Srcimg, 0 , 0);

         // ici on essaie de dessiner la date
         Font font = new Font("Arial", cartlen/4);
         // on ecrit le titre
         DateTime ladate = DateTime.Now;
         ladate.ToShortDateString();
         //largeGraphics.DrawString(ladate.ToShortDateString() + ":" + ladate.ToShortTimeString(), font, Brushes.Black, 10, Srcimg.Height + cartlen / 9 * 4); // au 4/9 du cadre


        // ici on calcule l'echelle
        Echelledrawing lechelle = CoreSystem.Instance.Etals.GetCurrentEchelle(Srcimg.Width / 8); // echelle = 1/8eme de l'image

        string legendeechelle = lechelle.reallen.ToString("0.0");
        Pen lepen = new Pen(Color.Black);

        largeGraphics.DrawLine(lepen,10, Srcimg.Height + cartlen / 9 * 8, 10 + lechelle.nbpix, Srcimg.Height + cartlen / 9 * 8);

        int posy = cartlen / 9 * 8;
        int ticklen = cartlen / 5;

        for (int i=1;i<lechelle.nbticks;i++)
            largeGraphics.DrawLine(lepen, 10 + lechelle.nbpix / lechelle.nbticks * i, Srcimg.Height + posy + ticklen, 10 + lechelle.nbpix / lechelle.nbticks * i, Srcimg.Height + posy - ticklen);

        // terace des extremites
        ticklen = cartlen / 3;
        largeGraphics.DrawLine(lepen, 10, Srcimg.Height + posy + ticklen, 10, Srcimg.Height + posy - ticklen);
        largeGraphics.DrawLine(lepen, 10 + lechelle.nbpix, Srcimg.Height + posy + ticklen, 10 + lechelle.nbpix, Srcimg.Height + posy - ticklen);
            
        // trait
        string unit = CoreSystem.Instance.Etals.getCurEtalUnit();
        largeGraphics.DrawString(legendeechelle+" "+unit, font, Brushes.Black, 20 + lechelle.nbpix, Srcimg.Height + cartlen / 9 * 4); // au 4/9 du cadre        
        
         return cartbitmap;
        }


        /// <summary>
        /// if a paint message is called, Render the scene
        /// </summary>
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            // on ne paeint que si le splash est ferme
            //if (splash != null)
                //return;

          
            this.Render(); // Render on painting
            base.OnPaint(e); // il faut appeler ca aussi ??? (cf exemple splashscreen)

        }

 
        /// <summary>
        /// timer utilise pour la gestion de l'mpression
        /// et pour le render 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ontimerprint)
            {if (notprinting) // anti reentrance
                {notprinting =false;
                 printpage();
                 notprinting = true;
                }
             ontimerprint = false;
                

            }

         Render();  // c'est le timer qui prend en charge le render
        }

        

        /// <summary>
        /// handler d'evenement de conversion source to etalonnage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mainfrmSrc2EtalConvHandler(object sender, ref MesureEventArgs e)
        {
            if (CoreSystem.Instance.Etals != null)
                e.outPt = CoreSystem.Instance.Etals.etalonne(e.inputPt);
            else
                e.outPt = e.inputPt; // on recopie le resultat dans l'argument
        }

        /// <summary>
        /// handler d'events demodif du clipping
        /// appele a chaque modification du clipper : il faut recalculer la position des points des mesureurs a l'ecran
        /// cette fonction fait la conversion source -> client, 
        /// eklle peut echouer si le point sort de la surface de clipping 
        /// mais POUR L'INSTATNT ;  tant qu'on n'a pas de zoom ca peut pas arriver. 
        /// et enfin calcule les ccords source de la souris
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mainfrmSrc2VisConvHandler(object sender, ref MesureEventArgs e)
        {
            e.CodeErreur = 0;

            if (CoreSystem.Instance.Etals != null)
            {
                //PointF pointVisu = new PointF(0,0);
                //int resu = CoreSystem.Instance.Etals.invEtalonne(e.inputPt, ref pointVisu); // conv src->visu
                //e.outPt = pointVisu; // copie 
                PointF pointVisu = CoreSystem.Instance.Etals.Clipper.convSrcToVisu(e.inputPt);
                e.outPt = pointVisu;

                //if (resu != 0) // une erreur
                 //   e.CodeErreur = resu;
            }
            else
            { //si camera nulle
                // on ne fait rien : le constructeur de mesureeventarg a deja copie input dans output
            }

        }

        /// <summary>
        /// handler d'events mesure
        /// convertit lea section visu de e , (en coords visu)
        /// en coords source dans la section src
        /// pour permettre d'afficher en direct les coords souris
        /// et eventuellement de reagir sur la position de la souris
        /// et enfin calcule les ccords source de la souris
        /// </summary>
        /// <param name="sender">argument contenant les coords souris dans sa section visu</param>
        /// <param name="e"></param> 
        void mainfrmVisu2SrcHandler(object sender, ref MesureEventArgs e)
        {
            PointF pointvisu = e.inputPt;
            //StatusbarResult.Text = pointvisu.Y.ToString("G4"); 

            PointF dest;
            if (CoreSystem.Instance.Etals != null)
                {// conversion visu-> source
                    PointF pointsource = CoreSystem.Instance.Etals.Clipper.convVisuToSrc(pointvisu);
                 //statusBarY.Text = pointsource.Y.ToString("G4"); // general 4 chiffre  
                 e.outPt = pointsource; // copie des coords
                }
        }


        /// <summary>
        ///  cet evenement est appeleé par l'algo des qu'il a une mesure disponible
        ///  y compris quand la souris se deplace, et est utilise pour afficher en temps reel l'evolution de la mesure 
        ///  suivant l'action de la souris
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainResuEventHandler(object sender, ref ResuEventArg e)
        {// ici il reste a faire le traitement general des variables provenant d'un caluclateur
            int numligne=0;

            if (e.isFinal) // resultat final : on l'envoie dans le gestionnaire de resultats
            { FinalResuEventHandler(sender, ref e);
            }
            // affiche le premier resultat dans le statusbar
            double valeurresu = CoreSystem.Instance.Calculator.GetValeur((int)e.IDResus[0], 0);
            
            StatusbarResult.Text = CoreSystem.Instance.MiseEnForme(valeurresu,etalflagstyp.none);
            
            dataGridResuImmediat.Rows.Clear();
            // ecriture des resultats, une ligne par resultat sortie par le calculatur
            foreach (int curID in e.IDResus)
            {
                dataGridResuImmediat.Rows.Add(1);
                if (CoreSystem.Instance.Calculator != null)
                {
                    string legende = CoreSystem.Instance.Calculator.GetLegende(curID);
                    etalflagstyp letype = etalflagstyp.none;
                    string unit = CoreSystem.Instance.Calculator.GetUnit(curID, ref letype);
                    double valeur = CoreSystem.Instance.Calculator.GetValeur(curID, 0);
                    dataGridResuImmediat.Rows[numligne].Cells[0].Value = legende;
                    dataGridResuImmediat.Rows[numligne].Cells[1].Value = CoreSystem.Instance.MiseEnForme(valeur,etalflagstyp.none);
                    numligne++;
                }
            }
        }


        /// <summary>
        /// traite l'envoi final de resultats 
        /// dans le tableau de mesre live
        ///
        /// pour tous les resultats dont le id est passe dans la liste resueventarg
        /// envoie un resultat et son nom dans resulstring 
        /// qui servira a remplir le tableau record
        /// resustring contient peut etre deja la param et recavrea eventuelelment l'objectif a la fin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FinalResuEventHandler(object sender, ref ResuEventArg e)
        {
            foreach (int curID in e.IDResus)
            {
                bool prechamp = true;

                if (e.Calculator != null)
                {
                    char tab = (char)9;

                    string legende = e.Calculator.GetLegende(curID);
                    //etalflagstyp letype = etalflagstyp.none;  // ca sert a quoi ca ? ca devrait servie a aider a la mise en fome de resultats
                    double valeur = e.Calculator.GetValeur(curID, 0);

                    CoreSystem.Instance.ResCumul.AddCell(legende, CoreSystem.Instance.MiseEnForme(valeur,etalflagstyp.none));
                    //ici il faut mettre en forme le resultat pour l'affichage live
                    //m_resuString += legende + tab + CoreSystem.Instance.MiseEnForme(valeur,etalflagstyp.none); // le flag est inutilise
                    if (!prechamp)
                        m_resuStringRecord += tab; 
                         
                    prechamp = false;
                    m_resuStringRecord = m_resuStringRecord + CoreSystem.Instance.MiseEnForme(valeur, etalflagstyp.none); // le flag est inutilise                     

                }
            }
        }




        #region fonctions pour export resultats

        // chaine contenant les resultats
        String m_resuStringRecord =new String(' ',1);


        /// <summary>
        /// efface la derniere ligne de resultats du tableau live
        /// en fait on efface la derniere ligne du gesres et on reaffiche
        /// </summary>
        void DelDerResuRecord()
        {
            CoreSystem.Instance.ResCumul.RemoveLigne();

            updateresdatagrid();

            if (dataGridViewResuRecord.RowCount != 0)
                dataGridViewResuRecord.CurrentCell = dataGridViewResuRecord.Rows[dataGridViewResuRecord.RowCount - 1].Cells[0];

        }

        /// <summary>
        /// redessine le resdata grid a partir du removeligne
        /// </summary>
        void updateresdatagrid()
        {dataGridViewResuRecord.Columns.Clear();
        int nbcols = CoreSystem.Instance.ResCumul.ResetColRead();
         int curcol;

         for (curcol = 0; curcol < nbcols; curcol++)
         {
             String nomcol = CoreSystem.Instance.ResCumul.NxtColRead();
             dataGridViewResuRecord.Columns.Add(nomcol, nomcol);
         }
                
         int nblignes = CoreSystem.Instance.ResCumul.ResetLinesRead();
         
         int curligne;
         for (curligne= 0; curligne< nblignes; curligne++)
             {String lineout;
             CoreSystem.Instance.ResCumul.NxtLineRead(out lineout);
             if (lineout != null)
                {
                 String[] tabstring = lineout.Split('\t');
                 dataGridViewResuRecord.Rows.Insert(curligne, tabstring);
                }
             }

        }

        /// <summary>
        /// init la ligne de resultats dans le tableau record
        /// c'est a dire la chaine resustring
        /// </summary>
        void InitLineResuRecord()
        {
            char tab = (char)9;

            CoreSystem.Instance.ResCumul.AddNewLigne();
                 
            if (CoreSystem.Instance.ParamSauvRes.SavParam)  // il faut sauver les parametres            
                CoreSystem.Instance.ResCumul.AddCell("Nom", CoreSystem.Instance.CurEchName);   // on met le nom d'echantillon dans la colonne name          

            if (CoreSystem.Instance.ParamSauvRes.SavObj) // on met l'etalonnage dans la colonne etalonnage
            {
                CoreSystem.Instance.ResCumul.AddCell("Obj", CoreSystem.Instance.Etals.getCurEtalName());
            }

        }
        
        
        /// <summary>
         /// finit la ligne de resultats dasn le tableau des exports
         /// ici il y aura toutes les options de sortie de resultat
         /// </summary>
        void FinLineResuRecord()
        {
            // on ajoute l'objectif en dernier champ a la string des resultats si il le faut
            
            // on recupere la derniere ligne ecrite en texte correct
            String txtligne = CoreSystem.Instance.ResCumul.readformattedline(CoreSystem.Instance.ResCumul.getLigneEcr());
            // on ajoute cette ligne au tableau des resultats record
            String[] tabstring = txtligne.Split('\t');
            this.dataGridViewResuRecord.Rows.Insert(dataGridViewResuRecord.RowCount , tabstring);
            dataGridViewResuRecord.CurrentCell = dataGridViewResuRecord.Rows[dataGridViewResuRecord.RowCount-1].Cells[0];
        }

        #endregion

#region gestion menus mesure
        /// <summary>
        /// clic sur icone mode toolstrip mesure ligne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ligneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setCalcLigne();
            showmesuretab();
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setCalcVert();
            showmesuretab();
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setCalcHorz();
            showmesuretab();
        }
       

        /// <summary>
        /// menu demarrage mesure ligne 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCalcLigne_Click(object sender, EventArgs e)
        {
            setCalcLigne();
            showmesuretab();
        }

        private void MenuCalculHor_Click(object sender, EventArgs e)
        {
            setCalcHorz();
            showmesuretab();
        }

        /// <summary>
        /// menu demarrage de lamesure en ligne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuCalcVert_Click(object sender, EventArgs e)
        {
            setCalcVert();
            showmesuretab();
        }


        /// <summary>
        /// mise a jour GUI des menus et tool bar mesures 
        /// </summary>
        /// <param name="menuitem">menu a cocher</param>
        /// <param name="toolstripitem">item combo toolbar a cocher</param>
        /// <returns></returns>
        private int updatecalculGUI(MenuItem menuitem, ToolStripMenuItem toolstripitem)
        {
            MenuCalculLigne.Checked = false;
            MenuCalculHor.Checked = false;
            MenuCalculVert.Checked = false;

            ligneToolStripMenuItem.Checked = false;
            verticalToolStripMenuItem.Checked = false;
            horizontalToolStripMenuItem.Checked = false;

            if (toolstripitem != null)
            {
                toolstripitem.Checked = true;
                toolStripSplitButton.Image = toolstripitem.Image;
                toolStripSplitButton.Text = toolstripitem.Text;
            }

            if (menuitem != null)
                menuitem.Checked = true;

            return 0;
        }


#endregion
        
        #region gestion choix mesureurs

        /// <summary>
        /// passe le calculateur en mode ligne
        /// </summary>
        private void setCalcLigne()
        {setcalcul(new LineCalculator());
         
         // coche des menus
         updatecalculGUI(MenuCalculLigne, ligneToolStripMenuItem);            
        }

        /// <summary>
        /// passe le caclul courant en vertical
        /// </summary>
        private void setCalcVert()
        {
         setcalcul(new VertMesure());
        
         // coche des menus
         updatecalculGUI(MenuCalculVert, verticalToolStripMenuItem);
        }

        /// <summary>
        /// choisir l'outil de mesure horizontal
        /// et met a jour le gui
        /// </summary>
        private void setCalcHorz()
        {
            setcalcul(new HorMesure());

            // coche des menus
            updatecalculGUI(this.MenuCalculHor, this.horizontalToolStripMenuItem);
        }

        /// <summary>
        /// installe un calcul
        /// installe les events
        /// ouvrel le dialogue calcul
        /// init le tableau de resultats immediats
        /// </summary>
        /// <param name="mesureur"></param>
        void setcalcul(ICalculator mesureur)
        {
            ICalculator Calculator = CoreSystem.Instance.Calculator;
            if (Calculator != null)
            {// ontimerprint defait legend lien Version la fonction de traitement des echelles
                Calculator.VisuToSrcConv -= new MesureEventHandler(mainfrmVisu2SrcHandler);
                Calculator.SrcToVisuConv -= new MesureEventHandler(mainfrmSrc2VisConvHandler);
                Calculator.SrcToEtalConv -= new MesureEventHandler(mainfrmSrc2EtalConvHandler);
                camwin.ChangingVariable -= new ChangeVariablehandler(camwin_ChangingVariable);
                camwin.SizeChanged -= new EventHandler(Calculator.OnChgVisuSize);            
                if (m_dialmesure.Visible)
                    m_dialmesure.Hide();

                CoreSystem.Instance.Calculator = null;
            }

            // cas ouon annule le calculator
            if (mesureur == null)
                return;

            // init les events du calculator
            // on ne fait plus le init des resultats cr ca vide le tableau
            // mais ca posera probleme quand les resultats difereront entre les caclulateurs
            //InitGridResuRecord(mesureur); // init le datagrid tableau des resuls a enregistrer
            mesureur.SetColor(this.m_DrawColor);
            mesureur.Panel = (TDPanel)panelList[1]; // plan 2 = graphic
            mesureur.SetActive(camwin.PicVideo);
            mesureur.VisuToSrcConv += new MesureEventHandler(mainfrmVisu2SrcHandler);
            mesureur.SrcToVisuConv += new MesureEventHandler(mainfrmSrc2VisConvHandler);
            mesureur.SrcToEtalConv += new MesureEventHandler(mainfrmSrc2EtalConvHandler);
            
            
            if (!m_dialmesure.Visible)
                m_dialmesure.Show(this); // boite de dialogue mesure visible et au dessius de la fenetre
            // on met les onglets sur resultat
            mesuretab.SelectedIndex = 1;

            // les resus du caclulateur sont envoyes a l'etalonnage
            mesureur.ToResu += new ResuEventHandler(MainResuEventHandler);
            // definit le calculator dans le core
            CoreSystem.Instance.Calculator = mesureur;

            camwin.ChangingVariable += new ChangeVariablehandler(camwin_ChangingVariable);
            camwin.SizeChanged += new EventHandler(mesureur.OnChgVisuSize);

            // on reinit le tableau s'il est vide
            if (IsEmptyResurecord())
                InitGridResuRecord(mesureur);
            else
                {// on ajoute les colonnes du nouveau calculateur, ca va se debrouiller por pas les mettres en double
                    ArrayList listresu = mesureur.GetListResu(); // liste des id du caclulateur
                    foreach (int resu in listresu)
                    {
                        string txtlegend = mesureur.GetLegende(resu); // legende de cet id
                     CoreSystem.Instance.ResCumul.AddColonne(-1,coltype.result, txtlegend); // on ajoute la colonne                    
                    }
                    updateresdatagrid();
                }
            
        }

        #endregion
        
        /// <summary>
        /// prepare le datagridresu de resultat live pour ce calculateur
        /// </summary>
        /// <param name="calculateur">calculateur envoyant ss resultats dans ce datagridresu</param>
        /// <returns>tojours 0 </returns>
        int InitGridResuRecord(ICalculator calculateur)
        {
           // ArrayList listresu = CoreSystem.Instance.Calculator.GetListResu();
            if (calculateur == null)
                return 0;

            // on iniatialise lus le grid avec le mesureuer global mais avec l'argument
            // car le global n'est pas encore a jour et ca peut faire des effets de bord
            ArrayList listresu = calculateur.GetListResu();

            string resu0;
            int nbcols = 0;
            ArrayList listchamps= new ArrayList();


            // on init les colonnes du resuges
            // on ne le fait plus ici mais a la lecture du xml ou a la mdif par le dialogue resultats
            /*if (CoreSystem.Instance.ParamSauvRes.m_SavParam)
            {
                resu0 = "Nom";  // on affiche le paramatre
                CoreSystem.Instance.ResCumul.AddColonne(-1,coltype.legendedeb,resu0);
            }
            */
            // le datagrid va afficher tous les resultats possibles du calculateur
            foreach (int resu in listresu)
            {
                string txtlegend = calculateur.GetLegende(resu);
                CoreSystem.Instance.ResCumul.AddColonne(-1,coltype.result,txtlegend); 
            }

            /*idem pour obj : on le fait dans le xml ou le dialogue
             * if (CoreSystem.Instance.ParamSauvRes.m_SavObj)
                CoreSystem.Instance.ResCumul.AddColonne(-1,coltype.legendefin,"obj");
            */
            // on met a jour le datagrid aec ces nouvelles colonnes
            updateresdatagrid();
            
            return 0;

        }


        /// <summary>
        /// seialisation du mesureur et de ses parametres
        /// </summary>
        /// <param name="element">element contenant le mesureur</param>
        void savemesureur(XMLAvElement elem)
        {
            if (CoreSystem.Instance.Calculator != null)
            {
                Type letype = CoreSystem.Instance.Calculator.GetType();
                string strtype = letype.ToString();

                XMLAvElement sourcElem = elem.CreateNode("calculator");
                sourcElem.SetAttribute(xmlavlabels.chktyp, strtype);
                CoreSystem.Instance.Calculator.SaveDisk(sourcElem);
            }
         
        }

        /// <summary>
        /// deserialisation du mesureur et de ses parametres (chargement depuis le disque)
        /// </summary>
        /// <param name="element">element contenzant le mesureur a charger</param>
        int loadmesureur(XMLAvElement element)
        {// ici il faut faire le ver et le chktype
            ICalculator Calculator;
            // il faut gerer la element null
            if (element == null)
            {
             return 2; // on fait rien on laisse pareil
            }

            Calculator = CoreSystem.Instance.Calculator;
            if (Calculator!= null)
            {
                Calculator.VisuToSrcConv -= new MesureEventHandler(mainfrmVisu2SrcHandler);
                Calculator.SrcToVisuConv -= new MesureEventHandler(mainfrmSrc2VisConvHandler);
                Calculator.SrcToEtalConv -= new MesureEventHandler(mainfrmSrc2EtalConvHandler);
            
                camwin.ChangingVariable -= new ChangeVariablehandler(camwin_ChangingVariable);
                CoreSystem.Instance.Calculator = null;
            }

            string strtType = "";

                
            // lecture de la section source
            try
            {
                element.GetAttribute("type", out strtType);
            }
            catch (XmlAvException e)
            {
                if (e.XmlAvType != xmlavexceptiontype.none)
                    return 0;
            }

            // alloue un objet a partir de son nom
            Type t = Type.GetType(strtType);
            object o = Activator.CreateInstance(t);

            //videoSource = (IVideoSource)Activator.CreateInstance (ac,tabstr); // alloue dynaiquement le strttype
            Calculator = (ICalculator)o; // alloue dynaiquement le strttype
      
            Calculator.Panel = (TDPanel)panelList[1]; // plan 2 = graphic
            Calculator.SetActive(camwin.PicVideo);
        
            MenuCalculHor.Enabled = false;

            if (Calculator.GetType() == typeof(VertMesure))
                updatecalculGUI(MenuCalculVert,verticalToolStripMenuItem) ;


            if (Calculator.GetType() == typeof(HorMesure))
                updatecalculGUI(MenuCalculHor, horizontalToolStripMenuItem);

            if (Calculator.GetType() == typeof(LineCalculator))
                updatecalculGUI(MenuCalculLigne,ligneToolStripMenuItem) ;


            Calculator.VisuToSrcConv += new MesureEventHandler(mainfrmVisu2SrcHandler);
            Calculator.SrcToVisuConv += new MesureEventHandler(mainfrmSrc2VisConvHandler);
            Calculator.SrcToEtalConv += new MesureEventHandler(mainfrmSrc2EtalConvHandler);

            Calculator.ToResu += new ResuEventHandler(MainResuEventHandler);
            
            CoreSystem.Instance.Calculator = Calculator; // le calculator est dans le coresysteme
            
            return 1;
        }


        
        

        /// <summary>
        /// fonction appelee par l'evenement changingvariable de la fenetre camwin
        /// appelle le calculateur qui va utiliser ces actions sur la souris et autre GUI pour faire cses calculs
        /// </summary>
        /// <param name="source">source des mouvements de souris (en general c'est camwin)</param>
        /// <param name="e">parametre decrivant la nature des changements de variable</param>
        private void camwin_ChangingVariable(object source, ChangeVariable e)
        {if (e.num == ChgVarNum.mousin) // la souris passe au dessus de la fenetre mesure
            {
                if (fEtalForm != null)
                    fEtalForm.SmoothHide(true);
                m_dialmesure.Focus();

                return; // pas passe au calculatore
            }

            if (e.num == ChgVarNum.mousout) // la souris sort au dessus de la fenetre mesure
            {
                if (fEtalForm != null)
                    fEtalForm.SmoothHide(false);

                return; // pas passe au calculatore
            }

            // ici on pourrait passer ca au calculator, mais si un jour il y a plusieurs calculs, 
            // ca serait bien que tous les caclculs sortent d'un coup sur une touche 
            // donc on le fait tout de suite
            // clic sur touche resu du dialogue mesure

            if (e.num == ChgVarNum.sendresu) // message de fin de mesure : clique sur OK de dial mesure
            {// on cree une ligne de resultats
                InitLineResuRecord();
                // on remplit la ligne avec les calculs
                //    foreach (ICalculator leCal in Calculators)
                if (CoreSystem.Instance.Calculator != null)
                    CoreSystem.Instance.Calculator.OnChangeVariable(this, e); // appel de calculator pour un resultat final

                // on envoie la ligne dans le tableau de resultats
                FinLineResuRecord();
            }


            // cas effacement d'une ligne
            if (e.num == ChgVarNum.delresu) // message de del de la derniere mesure
            {
                DelDerResuRecord();
            }
            

            //
            if (CoreSystem.Instance.Calculator != null)
                CoreSystem.Instance.Calculator.OnChangeVariable(this, e);
        }

      
        #region print rapport
        /// <summary>
        /// impression d'une page
        /// </summary>
        private void printpage()
        {
            PrintDocument doc = new PrintDocument(); // on cree u doc
            doc.PrintPage += this.Doc_PrintPage;

            PrintDialog dlgSettings = new PrintDialog();
            dlgSettings.Document = doc;

            if (dlgSettings.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }

            toprintbmp.Dispose();
            toprintbmp = null;
        }

        /// <summary>
        /// event impression : 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Arial", 15);
            
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;

            double x = e.Graphics.VisibleClipBounds.Left + e.Graphics.VisibleClipBounds.Width / 2-150/2;
            double y = e.Graphics.VisibleClipBounds.Top;
           
            // on ecrit le titre
            e.Graphics.DrawString("MOTION", font, Brushes.Black, (float)x, (float)y);

            double  lineHeight = font.GetHeight(e.Graphics);

            y += lineHeight;


            double imageWidth = 150;
            double imageHeight = imageWidth * toprintbmp.Height / toprintbmp.Width;
            
            // on calcule la position de l'image dans la feuille
            // on essaie toujours de la mettre dena les 2/3 inf de la feuille
            //y = (y+ e.Graphics.VisibleClipBounds.Height) / 2; //milieu de reste de page
            y = (y + imageHeight/10) ; //on saute 10% de la taille de l'image

            // on rajoute le cadre autour
            double  rectLine = 0.25;
            double  rectWidth = imageWidth + 2 * rectLine;
            double rectHeight = imageHeight + 2 * rectLine;

            
            // ca depasse en bas
            if (y + rectHeight > e.Graphics.VisibleClipBounds.Height)
            { // premier principe : on reduit l'image
                double sizemax = e.Graphics.VisibleClipBounds.Height - (y + rectHeight);
                sizemax *= .9; 
                imageHeight = sizemax;
                imageWidth = imageHeight / toprintbmp.Height * toprintbmp.Width;
            }
            
            e.Graphics.DrawRectangle(new Pen(Color.Black, (float)rectLine), (float)x, (float)y, (float)rectWidth, (float)rectHeight);

            e.Graphics.DrawImage(toprintbmp, (float)(x + rectLine), (float)(y-rectLine), (float)imageWidth, (float)imageHeight);

            /*double ximage = ((e.MarginBounds.Right - e.MarginBounds.Left)/100*25.4 - 150.0) / 2 + e.MarginBounds.Left/100*25.4;
            RectangleF dstrect = new RectangleF(e.MarginBounds.Right, y, (float)150.0, (float)150.0 * toprintbmp.Height / toprintbmp.Width);
            RectangleF srcrect = new RectangleF((float)0, (float)0, (float)toprintbmp.Width, (float)toprintbmp.Height);
            // voir la question de la mise a l'echelle sur papoier
            e.Graphics.DrawImage(toprintbmp, dstrect, srcrect,GraphicsUnit.Pixel);
             * */
        }
        #endregion

        // GESTION DES OBJECTIFS
        #region etalonnage

        /// <summary>
        /// clique sur une cellule de la grille etalonnage : on rend l'etalonnage actif
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridEtal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           // if (e.ColumnIndex == 0) // colonne 0 : bouton
            {
                string nom = (string)dataGridEtal.Rows[e.RowIndex].Cells[1].Value;
                if (nom.CompareTo("pixels") == 0) // ligne pixels
                    CoreSystem.Instance.Etals.SetCurEtal(null); // on annule l'etalonnnage
                else
                    CoreSystem.Instance.Etals.SetCurEtal(nom);

                updateetals();
            }
        }

        /// <summary>
        /// click sur drop down d'etalonnage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripDropDn_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            string nom = e.ClickedItem.Text;
                if (nom.CompareTo("pixels") == 0) // ligne pixels
                    CoreSystem.Instance.Etals.SetCurEtal(null); // on annule l'etalonnnage
                else
                    CoreSystem.Instance.Etals.SetCurEtal(nom);

                updateetals();

        }


        // fait passer mesuretab devant
        void showmesuretab()
        { this.microtab.Select();
        }

        /// <summary>
        /// cique sur le bouton ajout d'etalonnage
        /// ouvre la boite de dialogue ajout etalonnage
        /// on ajoutera l'etalonnage a la fermeture
        /// on ajoute toujours un etalonnage apres le courant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butaddetal_Click(object sender, EventArgs e)
        {
            if (CoreSystem.Instance.Camera == null)
                return;

            if (m_dialmesure.Visible)
                m_dialmesure.Hide();

            ICalculator Calculator = new EtalCalculator(); // calculateur special pour etalonnage
            
            Calculator.Panel = (TDPanel)panelList[1]; // plan 2 = graphic
            Calculator.SetActive(camwin.PicVideo);
            
            // on installe toconv pour convertir les coords ecran en coords source
            Calculator.VisuToSrcConv += new MesureEventHandler(mainfrmVisu2SrcHandler);
            Calculator.SrcToVisuConv += new MesureEventHandler(mainfrmSrc2VisConvHandler);
            Calculator.SrcToEtalConv += new MesureEventHandler(mainfrmSrc2EtalConvHandler);

            CoreSystem.Instance.Calculator = Calculator;
            // coche des menus
            //updatecalculGUI(null, null);            
            //MenuCalculLigne.Checked = false;
            //MenuCalculHor.Checked = false;
            //MenuCalculVert.Checked = false;

            MenuCalculLigne.Enabled = false;
            MenuCalculHor.Enabled = false;
            MenuCalculVert.Enabled = false;
            
            //updatecalculGUI(null, null);            

            CoreSystem.Instance.Etals.SetCurEtal(null); // pas d'etalonnage courant : on se met en pixels
            
            fEtalForm = new EtalForm();

            // les resus du caclulateur sont envoyes a l'etalonnage
            CoreSystem.Instance.Calculator.ToResu += new ResuEventHandler(fEtalForm.GetResu);
            
            camwin.ChangingVariable += new ChangeVariablehandler(camwin_ChangingVariable);
            camwin.SizeChanged += new EventHandler(CoreSystem.Instance.Calculator.OnChgVisuSize);
              
            // cas ou l'on cree un nouvel etalonnage
            fEtalForm.etalonnage = new Etalonnage();
            // instalaltion de l'event de close
            fEtalForm.CliqueFin += new mesure.EtalOkhandler(OnCloseEtal);
            
            fEtalForm.Show(this);              // non modal
        }


        
        /// <summary>
        /// fonction appelee par l'evenement quit de la form etalonnage
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnCloseEtal(object source, ClickOkEvent e)
        {
            
            // on vire le calculteur de ligne etalonnage
            ICalculator Calculator = CoreSystem.Instance.Calculator;
            if (Calculator != null)
            {
                Calculator.VisuToSrcConv -= new MesureEventHandler(mainfrmVisu2SrcHandler);
                Calculator.SrcToVisuConv -= new MesureEventHandler(mainfrmSrc2VisConvHandler);
                Calculator.SrcToEtalConv -= new MesureEventHandler(mainfrmSrc2EtalConvHandler);

                CoreSystem.Instance.Calculator = null;

            }
            
            /*int numindex = this.dataGridEtal.CurrentRow; // ligne en surbrillance
            if (numindex == -1) // personne en surbrillance
                numindex = this.dataGridEtal.RowCount; // on inserera apres la derniere
            */

            //fEtalForm.Dispose(); // detruit la boite de dialogue
            if (CoreSystem.Instance.Etals == null)
                return;

            if (e.ok)
                CoreSystem.Instance.Etals.InsertEtal(fEtalForm.etalonnage); // ajout de l'etalonnage si il est ok

            // l'etalonnage courant est le nouveau
            fEtalForm = null; // la form est en cours de fermture : on coupe le lien
            
            // ici il faudrait recharger le calculateur qui va bien
            MenuCalculLigne.Enabled = true; // on reactive le gui calcul
            MenuCalculHor.Enabled = true;
            MenuCalculVert.Enabled = true;
            
            
            // on refait pointer le calculateur qui pointait sur etalonnage vers mesure
            camwin.ChangingVariable += new ChangeVariablehandler(camwin_ChangingVariable);
            //camwin.SizeChanged += new EventHandler(Calculator.OnChgVisuSize);
            
            // on met a jour le tableau des etalons
            updateetals();
         }

         /// <summary>
        /// fait la mise a jour du GUI des etalonnages : panel + menus et toostrip
        /// </summary>
        private void updateetals()
        {
            ToolStripMenuItem newitem;

            // refresh la fenetre video pour redessiner les echelles
            this.camwin.Invalidate();

            int indexcur = -1;
            ArrayList list = null;
            if (CoreSystem.Instance.Etals != null)
                list = CoreSystem.Instance.Etals.GetListEtal(ref indexcur);

            dataGridEtal.Rows.Clear();
            //dataGridEtal.RowCount = 0;  // pas de probleme si allowusertoaddrow est a false

            if (list == null) 
                return;
            int selline = 0; // line a selectionner a la fin

            int currow = 0;

            // clear la drop down de etalonnage
            toolStripDropEtal.DropDownItems.Clear();

            // ajout du item de dropdown de toolbar 'pixels' 
            newitem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripDropEtal.DropDownItems.Add(newitem);
            
            newitem.Text=("pixels");
            if (indexcur == -1)
                newitem.Checked = true;
            else
                newitem.Checked = false;

            // on remplit le datagridetal avec les objectifs
            // et on remplit le combobox de la toolbar
            foreach (string etalname in list)
            {
                // ajout d'une ligne avec le nom
               dataGridEtal.Rows.Insert(currow,1);
               dataGridEtal.Rows[currow].Cells[1].Value = etalname;
               
               // ajout d'une toolstrip ligne 
               newitem = new System.Windows.Forms.ToolStripMenuItem();
               toolStripDropEtal.DropDownItems.Add(newitem);
               newitem.Text = etalname;

               if (currow == indexcur) // etalonnage courant
               {
                   dataGridEtal.Rows[currow].Cells[0].Value = imagelist.Images[1];
                   // mise a jour du nom etalonnage dans status
                   this.statusbarobj.Text = etalname;  // mise a jour du statusbar
                   selline = currow;
                   newitem.Checked = true;
                   toolStripDropEtal.Text = etalname; // nom du dropdown
                   newitem.Checked = true;  // c'est le courant : on check la dropdwn de toolbox
               }
               else
                   dataGridEtal.Rows[currow].Cells[0].Value = imagelist.Images[0];
                
                currow++;
            }

            // ajout de la ligne pixels en debut de tableau
            dataGridEtal.Rows.Insert(0, 1);              
            dataGridEtal.Rows[0].Cells[1].Value = "pixels";

            if (CoreSystem.Instance.Etals == null || indexcur == -1) // selection de cette ligne si c'est l'etal courant
            {
                dataGridEtal.Rows[0].Cells[0].Value = imagelist.Images[1]; // etal pixel pixel
                selline = 0;
                toolStripDropEtal.Text = "pixels";

            }
            else
                dataGridEtal.Rows[0].Cells[0].Value = imagelist.Images[0]; // pixel
            
            
            /// affichage icone suppression objectif
            if (list.Count != 0)   // si il y a des etalonnages
                this.supobj.Image = this.imagelist.Images[3];
            else
                this.supobj.Image = this.imagelist.Images[4];

            
            // on ajuste les regles de l'image en fonction de l'etalonnage
            // on  recupere Le cliprect etalflagstyp on lui applique ligneToolStripMenuItem'etalonnage
            Rectangle rect = CoreSystem.Instance.Etals.Clipper.ClipRect; // coords source du clipper
            PointF dstpt = CoreSystem.Instance.Etals.etalonne(new PointF(rect.Left, rect.Top));
            PointF dstpt2 = CoreSystem.Instance.Etals.etalonne(new PointF(rect.Right, rect.Bottom));
            RulerAxis ruler = new RulerAxis();
            ruler.min = dstpt.X;
            ruler.max = dstpt2.X;
            this.camwin.XScale = ruler;
           ruler.min = dstpt.Y;
           ruler.max = dstpt2.Y;
            this.camwin.YScale = ruler;

        }


        /// <summary>
        ///  bouton effacement d'un objectif
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void supobj_Click(object sender, EventArgs e)
        {
            CoreSystem.Instance.Etals.DelCurEtal();
            updateetals();
        
        }

#endregion

        ///FIN GESTION OBJECTIFS
        
        
       

        /// <summary>
        /// fonction faisant la sauvegarde de la config dant le fichier de path nomfic
        /// </summary>
        int savexml(string nomfic)
        {
            XMLEngine lengin = new XMLEngine();
            // en tete document
            XMLAvElement racine = lengin.initDoc();
            racine.Text = "motion";
            racine.SetAttribute("ver", "1.0");
            
            XMLAvElement racine3 = racine.CreateNode("paramsauvimg");
            CoreSystem.Instance.ParamSauvImg.SaveDisk(racine3);

            XMLAvElement racine4 = racine.CreateNode("paramsauvres");
            CoreSystem.Instance.ParamSauvRes.SaveDisk(racine4);
            
            // section configuration
            XMLAvElement racinesource = racine.CreateNode("currentsource");
            int indexcur = 0;
            if (CoreSystem.Instance.Camera != null)
            {
                CoreSystem.Instance.Camera.SaveDisk(racinesource); // sauvegardera le type a allouer
            }

            
            // enregistre la disposition du gui
            // section GUI
            XMLAvElement m_GUIElement = racine.CreateNode("GUISetting");            
            m_GUIParam.SetParams(this); // init le mguiparam
            m_GUIParam.SaveDisk(m_GUIElement);
            
            // on sauve sur disque
            lengin.SaveDoc(nomfic);
            return 0;
        }


        /// <summary>
        /// enregistre le xml par defaut
        /// </summary>
        private void SaveDefaultXml()
        {
            logger.log("savxml in");
            string path = Application.StartupPath; // path de l'exe
            path += "\\default.xml";
            logger.log("call savxml");
            savexml(path);
            logger.log("call savxml ok");
        }
        
        /// <summary>
        /// definit l'echantillon de la mesure
        /// </summary>
        /// <param name="echantillon"></param>
        public void SetEchantillon(string echantillon)
        {m_echantillon = echantillon;
            
        }
        /// <summary>
        /// ouvre la boite de dialogue et enregistre la config
        /// </summary>
        private void saveconfig()
        {
            string nomfic;
            SaveFileDialog ledial = new SaveFileDialog();
            ledial.DefaultExt="xml";
            ledial.FileName="sansnom";
            ledial.Filter="fichiers de configuration (*.xml)| *.xml||";
            
            string path = Directory.GetCurrentDirectory(); // chamin d'ouverture
            ledial.InitialDirectory = path;
            if (ledial.ShowDialog()== DialogResult.OK)
            {
                nomfic = ledial.FileName;
                 savexml(nomfic);
                }

            
        }



        /// <summary>
        /// menu file open
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void menuOpenFile(object sender, EventArgs e)
        {// ouverture dialogue xml
            string path = Directory.GetCurrentDirectory(); // chamin d'ouverture
            string nomfic;

            OpenFileDialog ledial = new OpenFileDialog();
            ledial.InitialDirectory = path;

            ledial.DefaultExt = "xml";
            ledial.Filter= "Configuration files(*.xml)|*.xml||";
            ledial.Title="ouvrir un fichier de configuration";
            
            // showdialog peut modifier le current directory
            if (ledial.ShowDialog() == DialogResult.OK)
            {
                nomfic = ledial.FileName;
                LoadXml(nomfic, false); // pas de verifi si default est charge
            }
        }

        /// <summary>
        /// charge le xml de config par defaut
        /// </summary>
        /// <returns></returns>
        public int LoadDefaultXml()
        { if (m_nodefconfig)
             return 1; // pas de lecture du def config

            string path = Application.StartupPath; // path de l'exe
            path += "\\default.xml";
            LoadXml(path, true);
            return 0;
        }


        /// <summary>
        /// charge le xml de config par defaut
        /// </summary>
        /// <returns></returns>
        public int InitSettingsXml()
        {
            if (m_nodefconfig)
                return 1; // pas de lecture du def config

            string path = Application.StartupPath; // path de l'exe
            path += "\\settings.xml";
            LoadSettingsXml(path, true);
            return 0;
        }

        /// <summary>
        /// charge es sttings du prgramme
        /// </summary>
        /// <param name="pathxml"></param>
        /// <param name="checkConfig"></param>
        /// <returns></returns>
        public int LoadSettingsXml(string pathxml, bool checkConfig)
        {// creation du doc et decodage en tete
            XMLEngine lengin = new XMLEngine();

            XMLAvElement root = lengin.LoadDoc(pathxml);
            if (root == null)
                return 1;

            string numversion;
            if (root.GetAttribute("ver", out numversion) == 0)
                if (numversion.CompareTo("1.0") != 0)
                    return 1;

            XMLAvElement config = root;
            
            // on verra apres pour l'encodage
            XMLAvElement settings = config.GetFirstElement("settings");
            CoreSystem.Instance.Config.LoadDisk(settings);
            
            // ici on init tout ce qui est a a initaliser avec la config


            return 0;
        }
      
        /// <summary>
        /// ouvre un fichier xml de configuration
        /// </summary>
        /// <param name="pathxml"></param>
        /// <param name="checkConfig">si true : ne pas charger la config si on a la variable nodefault a 1</param>
        /// <returns>0 si pas d'erreur</returns>
        public int LoadXml(string pathxml, bool checkConfig)
        {
            XMLEngine lengin = new XMLEngine();

            XMLAvElement root = lengin.LoadDoc(pathxml);
            if (root == null)
                return 1;
         //   if (root.Text.CompareTo("motion") != 0)  on enleve ce texte parce qu'il renvoie 'motion1000!!!)
           //     return 1; // on a lu portnawak

            string numversion; 
            if (root.GetAttribute("ver", out numversion) ==0)
                if (numversion.CompareTo("1.0")!=0)
                    return 1;

            // voila on a un en tete potable
           
            //if (root.GetFirstElement("motion") == null) // on aouvert portnzwak
             //   return 1;
            
            XMLAvElement config = root;

            // lecture de la section sauv img parametres
            XMLAvElement racparamimg = config.GetFirstElement("paramsauvimg");
            CoreSystem.Instance.ParamSauvImg.LoadDisk(racparamimg);

            // lecture de la section sauv resparametres
            XMLAvElement racparamres = config.GetFirstElement("paramsauvres");
            CoreSystem.Instance.ParamSauvRes.LoadDisk(racparamres);
            
            // mise a jour des col du gestionnaire de resultats
            if (CoreSystem.Instance.ParamSauvRes.m_SavObj)
                CoreSystem.Instance.ResCumul.AddColonne(-1, coltype.legendefin, "Obj");
            else
                CoreSystem.Instance.ResCumul.DelColonne("Obj");

            if (CoreSystem.Instance.ParamSauvRes.m_SavParam)
                CoreSystem.Instance.ResCumul.AddColonne(-1, coltype.legendedeb, "Nom");
            else
                CoreSystem.Instance.ResCumul.DelColonne("Nom");


            LoadVideoSource(config.GetFirstElement("currentsource"));
             
                
            // ici le chargement de video source a init l'etalonnage, on upadtre le gui

            try
            {
                XMLAvElement calcElem = config.GetFirstElement("calculator"); // lecture du node

                if (calcElem != null)
                    loadmesureur(calcElem);
                else
                    CoreSystem.Instance.Calculator = null;

                lengin = null;
            }
            catch (XmlAvException a)
            {
                System.Console.WriteLine("xml calculator");
            }

            try
            {
                XMLAvElement GUIElem = config.GetFirstElement("GUISetting"); // lecture du node
                m_GUIParam.LoadDisk(GUIElem);

                // on positionne les fenetres
                m_GUIParam.SetMainPosition(this);
                m_GUIParam.SetMesPos(m_dialmesure); // gere parametre null
            }
            catch (XmlAvException a)
            { }
            return 0;
        }

       
        /// <summary>
        /// chargement de la form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //creation du splash
            // Create a new thread from which to start the splash screen form
            //Thread splashThread = new Thread(new ThreadStart(StartSplash));
            //splashThread.Start();

            // Pretend that our application is doing a bunch of loading and
            // initialization
            Thread.Sleep(kMinAmountOfSplashTime_ms / 2);

            //// --- inits a faire pendant le splashe

            LoadDefaultXml();  // chargement de la config

            m_nodefconfig = true; // on annule un eventuel no relecture du config

            m_dialmesure = new MesureForm();
            m_dialmesure.ChangingVariable += new ChangeVariablehandler(camwin_ChangingVariable);
            m_dialmesure.Hide();

            // ici on met la fenetre mesure au bon endroit
            m_GUIParam.SetMesPos(m_dialmesure); // gere parametre null
            
            ///


            //// --- fin des inits a faire pendant le splashe 

            // Sit and spin while we wait for the minimum timer interval if
            // the interval has not already passed
            //while (splash.GetUpMilliseconds() < kMinAmountOfSplashTime_ms)
            //{
            //    Thread.Sleep(kSplashUpdateInterval_ms / 4);
            //}

            // Close the splash screen
            //CloseSplash();
            
            // fermeture du splashscreen
            this.Show();
#if DEBUG
#else
SplashScreen.CloseSplashScreen();
#endif
            this.Activate();
            
        }


        /// <summary>
        /// menu reglage camera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuReglages_Click(object sender, EventArgs e)
        {
            if (CoreSystem.Instance.Camera == null)
                return;

            if (m_dialmesure.Visible)
                m_dialmesure.Hide(); // masque le dial de mesure, ce qui provoquera la mise a jour de param enabled ou non au reaffichage

            {
                if (m_dialmesure.Visible)
                    m_dialmesure.Hide();
                //CoreSystem.Instance.Camera.Reglage(this.dataGridEtal);            
                CoreSystem.Instance.Camera.Reglage(this);            
            }
        }
        
        
        
        /// <summary>
        /// sauvegarde auto des images
        /// </summary>
        private void autoSaveImage()
        {string extension = "jpg";

        paramsavimg paramsav = CoreSystem.Instance.ParamSauvImg;

        ImageFormat imgformat = paramsav.m_Format;

        if (imgformat == System.Drawing.Imaging.ImageFormat.Jpeg)
            extension = "jpg";

        if (imgformat == System.Drawing.Imaging.ImageFormat.Tiff)
            extension = "tif";


        if (imgformat == System.Drawing.Imaging.ImageFormat.Bmp)
            extension = "bmp";


        string nomfich = paramsav.m_ResPath + "\\" + paramsav.m_ResSuffix + paramsav.m_ResIdx.ToString("D3") + "." + extension; // entier a 3 chiffres avec des zeros a gauche
        saveImage(nomfich, extension); // pas de verifi si default est charge
        paramsav.m_ResIdx++; // index suivant
        }


        /// <summary>
        /// clear les resultats mesure
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuResuInit_Click(object sender, EventArgs e)
        {
            if (m_dialmesure.Visible)
                m_dialmesure.Hide(); // masque le dial de mesure, ce qui provoquera la mise a jour de param enabled ou non au reaffichage

            ClearTabResuRecord();
        }



        /// gestion du tableau des resultats enregistres 
        

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int ClearTabResuRecord()
        {
            if (m_dialmesure.Visible)
                m_dialmesure.Hide(); // masque le dial de mesure, ce qui provoquera la mise a jour de param enabled ou non au reaffichage

            
            CoreSystem.Instance.ResCumul.ClearResul();
            
            // ici il faut le recreer avec les colonnes qui vont bien
            InitGridResuRecord(CoreSystem.Instance.Calculator); //                    
            return 0;
        }

        /// <summary>
        /// dit si le tableau des resultats est vide ou non
        /// </summary>
        /// <returns></returns>
        private bool IsEmptyResurecord()
        {
            return (dataGridViewResuRecord.Rows.Count == 0);
        }













        /// <summary>
        ///enregistremetn de l'image 
        /// </summary>
        /// <param name="nompath">chemin complet d'enregistrement</param>
        /// <param name="extension">extension sur 3 cars</param>
        private void saveImage(string nompath, string extension)
        {
            if (CoreSystem.Instance.Camera == null)
                return;
            if (CoreSystem.Instance.Camera.LastFrame == null)
                return;

            Bitmap tosavebmp = (Bitmap)CoreSystem.Instance.Camera.LastFrame.Clone();

            ImageFormat format = ImageFormat.Jpeg;
            if (extension.CompareTo("jpg") == 0)
                format  = ImageFormat.Jpeg;
            if (extension.CompareTo("bmp") == 0)
                format = ImageFormat.Bmp;
            if (extension.CompareTo("tif")== 0)
                format = ImageFormat.Tiff;
            Image limage;

            tosavebmp.Save(nompath,format);
            tosavebmp.Dispose();

        }


        /// <summary>
        /// appele par clic sur le bouton fin de mesure
        /// </summary>
        public void EndMesure()
        {
            int retour = 1;
            paramsavres paramres = CoreSystem.Instance.ParamSauvRes;

            if (paramres.m_autosaveonsave) // pas de sauv automatique
                retour = autoSaveResu(); // 0 = pas d'erreur
            
            if (retour !=0) // un probleme qq part
                visuExport(); // on sauve en manuel

            if (paramres.m_clearautosave) // il faut faire un clear apres affichage
                ClearTabResuRecord();

        }

        /// <summary>
        /// visualistaion resultats avant exportation
        /// </summary>
        public void visuExport()
        {
            // annule le calcul courant
            this.setcalcul(null);
            updatecalculGUI(MenuCalculLigne, ligneToolStripMenuItem);

            // ouvre la forme de resultats
            Résultats leform = new Résultats();
            leform.LoadResu(dataGridViewResuRecord); // copie les resultats deu tab resu dans la form
            leform.ShowDialog(this);
        }

        
        /// <summary>
        /// sauvegarde automatique de resultats
        /// </summary>
        /// <returns></returns>
        private int autoSaveResu()
        {
            if (IsEmptyResurecord()) 
                return 1; // pas de resultat a sauvegarder
            
            paramsavres paramres = CoreSystem.Instance.ParamSauvRes;

            string extension = "xls";
            resuformat resFormat = paramres.m_autoSaveResFormat;
            if (resFormat == resuformat.tsv)
                extension = "tsv";

            if (resFormat == resuformat.csv)
                extension = "csv";

            if (resFormat == resuformat.xls)
                extension = "xls";

            string nomfich = paramres.m_autoSaveResPath + "\\" + paramres.m_autoSaveResSuffix + paramres.m_autoSaveResIdx.ToString("D3") + "." + extension; // entier a 3 chiffres avec des zeros a gauche
            saveResu(dataGridViewResuRecord,nomfich, extension); // pas de verifi si default est charge
            paramres.m_autoSaveResIdx++; // index suivant

            if (paramres.m_clearautosave)
                ClearTabResuRecord();
            
            return 0;
        }


/// <summary>
/// vide le tableau de resultats dans un fichier
/// </summary>
/// <param name="source"></param>
/// <param name="filename"></param>
/// <param name="extension"></param>
/// <returns></returns>
   private int saveResu(DataGridView source, string filename, string extension)
   {
            
         StreamWriter sw = new StreamWriter(filename); // ouvertre du fichier de sortie
            
         // on copie les resultats de la grille de ressultats dans le fichier de sortie
         string tabulstr = "\t";
                
         if (extension.CompareTo("csv")==0)
                tabulstr = ",";
                
         int iColCount = source.ColumnCount;
            
         for (int i = 0; i < iColCount; i++)
            {
                sw.Write(source.Columns[i].Name); 
                if (i < iColCount - 1) 
                    sw.Write(tabulstr); 
            }

            //sw.Write(sw.NewLine); // fin ligne en tete
            sw.WriteLine("");	

            // lecture des lignes
            foreach (DataGridViewRow dr in source.Rows)	
            {   for (int i = 0; i < iColCount; i++)		
                {   if (!Convert.IsDBNull(dr.Cells[i]))			
                        sw.Write(dr.Cells[i].Value);
                    
                    if ( i < iColCount - 1)			
                        sw.Write(tabulstr); 			
                        		
                 }		
             sw.WriteLine("");	
             }
        
       sw.Close();
    
       return 0; // pas de souci
    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
   private void menuEnregAuto_Click(object sender, EventArgs e)
   {
    autoSaveResu();
   }

        /// <summary>
        /// dialogue menu enregistrer sous : choix fichier et on sauve l contenu du datagridviewrecord
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
   private void menuEnregresSous_Click(object sender, EventArgs e)
   {
       SaveFileDialog ledial = new SaveFileDialog();
       ledial.DefaultExt = "xls";
       ledial.FileName = "sansnom";
       ledial.Filter = "Fichier Excel XLS (*.xls)|*.xls|Text (virgule sep.) (*.csv)|*.csv|Séparat. tab(*.tsv)|*.tsv||";
       ledial.InitialDirectory = CoreSystem.Instance.ParamSauvRes.m_autoSaveResPath;

       if (ledial.ShowDialog(this) == DialogResult.OK)
       {
           string ext = ledial.DefaultExt; // lecture de l'extension choisie
           string nomfic = ledial.FileName;
           saveResu(dataGridViewResuRecord,nomfic, ext); // pas de verifi si default est charge
       };
   }



   private void menuPrintImage_Click(object sender, EventArgs e)
   {
       if (m_dialmesure.Visible)
           m_dialmesure.Hide();
       toprint = true;
   }


   #region relais des menuus vers ailleurs

   // depuis que la propriete visible des menus url est sous contrle de la config, 
   // l'IDE regenere ces fonctions a chaque ouverture de l'editeur de mainform.
   //pour eviter ca et par rapener ces fonctions dans mainform qui en contient deja beaucoup
   // on fait un relai vers les meems fonctions simlement appellees
   //action au lieu de click

        private void menuSourcURLJPGFile_Click(object sender, EventArgs e)
        {
            menuSourcURLJPGFile_Action(sender, e); // relai vers la meme fonction dns un autre fichier parce que ca me plait
        
        }

        private void menuSourceURLMJEPGItem_Click(object sender, EventArgs e)
        {
            menuSourceURLMJEPGItem_Action(sender, e);

        }

        private void menuSourcTwain_Click(object sender, EventArgs e)
        {
         menuSourcTwain_Action(sender, e);
        }

        private void menuSourcePeriph_Click(object sender, EventArgs e)
        {
            menuSourcePeriph_Action(sender, e);
        }

        private void menuSourcImg_Click(object sender, EventArgs e)
        {
            menuSourcImg_Action(sender, e);
        }
        
        #endregion


        // changemùent de taille du panel de resultats
        private void mesuretab_SizeChanged(object sender, EventArgs e)
        {
            int newheight =  this.mesuretab.Height - this.groupdatarecord.Top - 10;
            groupdatarecord.SetBounds(0, 0, 0, newheight, BoundsSpecified.Height);
            //dataGridViewResuRecord.SetBounds(0, 0, 0, newheight-10, BoundsSpecified.Height); inutile : automatique !
        }

        private void toolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            if (CoreSystem.Instance.Calculator == null)
                foreach (ToolStripMenuItem item in toolStripSplitButton.DropDown.Items)
                {
                    if (item.Checked)
                    {
                        item.PerformClick(); // appelle la fonction de cet item
                        break;
                    }
                }

            

            if (!m_dialmesure.Visible)
                m_dialmesure.Show();
       
        }

        
        private void menuRecordAVIOpen_Click(object sender, EventArgs e)
        {

        }


        private void toolStripMenuLongueur_Click(object sender, EventArgs e)
        {
            setCalcLigne();
            showmesuretab();
        }

        /// <summary>
        /// clic sur icone toolstrip mesure hauteur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hauteurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setCalcVert();
            showmesuretab();
        }

        /// <summary>
        /// click sur icone mesure largeur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void largeurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.setCalcHorz();
            showmesuretab();
        }

        
        /// <summary>
        /// clic sur un bouton d'etalonnage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripDropEtal_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripDropEtal_Click(object sender, EventArgs e)
        {

        }
        
        /// <summary>
        /// clique sur icone toolbar file new
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void ToolStripButNew_Click(object sender, EventArgs e)
        {
            menuNewConfig_Click(sender, e);
        }

        private void ToolStripButOpen_Click(object sender, EventArgs e)
        {
            this.menuOpenFile(sender, e);
        }

        private void ToolStripButSave_Click(object sender, EventArgs e)
        {
            this.menuSaveFile(sender, e);
        }

        //private void ToolStripButPrint_Click(object sender, EventArgs e)
        //{
            //if (m_dialmesure.Visible)
                //m_dialmesure.Hide();
            //toprint = true;
        
        //}

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuImageLive_Click(object sender, EventArgs e)
        {if (CoreSystem.Instance.Camera == null)
            return;
         
         if (CoreSystem.Instance.Camera.source == null)
            return;

         if (CoreSystem.Instance.Camera.source.capchglive())
             if (CoreSystem.Instance.Camera.source.isfreeze())
             {
                 CoreSystem.Instance.Camera.source.live();
                 this.menuImageLive.Checked = true;
             }
             else
             {
                 CoreSystem.Instance.Camera.source.freeze();
                 this.menuImageLive.Checked = false;
             }
        }

        public void UpdateMenuGUI()
        {
                    
            if (CoreSystem.Instance.Camera != null)
                if (CoreSystem.Instance.Camera.source != null)
                {
                    menuImage.MenuItems[0].Checked = CoreSystem.Instance.Camera.source.freeze();
                    menuImage.MenuItems[0].Enabled = CoreSystem.Instance.Camera.source.capchglive();
                    menuImage.MenuItems[1].Enabled = CoreSystem.Instance.Camera.source.capgrab();
                    
                    if (CoreSystem.Instance.Camera.source.capgrab())
                        this.menuImagePhoto.Click += new System.EventHandler(this.menuImagePhoto_Click);
            
                }


        }

        private void menuImagePhoto_Click(object sender, EventArgs e)
        {
            if (CoreSystem.Instance.Camera != null)
                if (CoreSystem.Instance.Camera.source != null)
                    if (CoreSystem.Instance.Camera.source.capgrab())
                            CoreSystem.Instance.Camera.source.grab();
                
            this.camwin.Invalidate();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButFreeze_Click(object sender, EventArgs e)
        {
         menuImageLive_Click(sender, e);
        
        }

        /*private void toolStripButRegl_Click(object sender, EventArgs e)
        {
            menuReglages_Click(sender, e); // reglage source

        }*/


        private void toolStripButSnap_Click(object sender, EventArgs e)
        {
            menuImageLive_Click(sender, e);
        }

        private void toolStripReglage_Click(object sender, EventArgs e)
        {
           menuReglages_Click(sender, e); // reglage source

        }

        private void toolStripButPrint_Click(object sender, EventArgs e)
        {
            menuPrintImage_Click(sender, e);
        }

        


        
    }

    /// <summary>
    /// classe exception de securite
    /// </summary>
    public class SecurityException : System.Exception
    {// on implemente les 3 sortes d'exceptions recommandees
        int m_numexception = 0;

        /// <summary>
        /// accesseur au type d'exception
        /// </summary>
        public int NumException
        {
            get
            {
                return m_numexception;
            }
        }

        /// <summary>
        /// surcharge de constructeur avec argument
        /// </summary>
        /// <param name="numexception"></param>
        public SecurityException(int reboot)
        {
            m_numexception = reboot;
        }
    }
 }

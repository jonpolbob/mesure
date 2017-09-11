using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
//using AVISystem;

// premiere modif de barnche

namespace mesure
{
    ///
    /// singleton contenant les elements fonctionnels
    ///


    // deuxieme modif de la branche
    public class CoreSystem
    {

        // l'essentiel de ce qui fonctionne : etalonnage, camera, source, mesureur
        private Camera m_Camera = null;
        private GesEtals m_Etals = null;
        private Gesres m_ResCumul = null;
        private ICalculator m_Calculator = null;
        private Config m_Config;
        private paramsavres m_paramsauvres = null;
        private paramsavimg m_paramsauvimg = null;
        //Private AVIEngine m_avirecorder =null;
        string m_CurEchantillonName;
        private MainForm m_laform;

        public void initGui(MainForm laform)
        { m_laform = laform; 
        }
        
        public bool updateMenu()
        {
            m_laform.UpdateMenuGUI();
            return true;
        }


        // mecanique du singleton
        static CoreSystem instance = null;
        static readonly object padlock = new object();

        /// <summary>
        ///ca c'est assez mysterieux mais necessaire, ce constructeur static 
        /// 
        /// 
        /// </summary>
        static CoreSystem()
        {
            // le reste est cree
        }

        /// <summary>
        ///on accede a coresystem par instance Coresystem.Instance 
        /// </summary>
        public static CoreSystem Instance
        {
            get
            {
                // c'est sans doute pas thread safe
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CoreSystem();
                        instance.m_Etals = new GesEtals();       // alloue les etalonages}
                        instance.m_Config = new Config(); // alloue le gestionnaire de config
                        instance.m_ResCumul = new Gesres();
                        instance.m_paramsauvres = new paramsavres();
                        instance.m_paramsauvimg = new paramsavimg();
                    }
                }
                return instance;
            }
        }
        

        /// <summary>
        /// property Camera get set
        /// </summary>        
        public Camera Camera
        {
            get { return m_Camera; }
            set { m_Camera = value; }
        }

        /// <summary>
        /// membre renvoyant le gestionnaire de resultats
        /// </summary>
        public Gesres ResCumul
        {get {return m_ResCumul;}
        }

        /// <summary>
        /// memebre renvoyant le gestionnaire de config
        /// </summary>
        public Config Config
        {
            get { return m_Config; }
        }

        /// <summary>
        /// property Etals get uniquement
        /// </summary>
        public GesEtals Etals
        {
            get { return m_Etals; }
        }
        /// <summary>
        /// property calculator get set
        /// </summary>
        public ICalculator Calculator
        {
            get { return m_Calculator; }
            set { m_Calculator = value; }
        }


        
        /// <summary>
        /// recreation de paramsavimg a partir d'une nouvelle structure
        /// par exemle cette structure est le clone de la structure courante, modifié par la boite d'edit des parametres
        /// </summary>
        /// <param name="newparamsav">nouvelle classe paramsavimg destinee a remplacer l'existante</param>
        public void RebuildParamSauvImg(paramsavimg newparamsav)
        {
            m_paramsauvimg = null;
            m_paramsauvimg = newparamsav;
        }


        /// <summary>
        /// permet d'acceder a paramsavimg en lecture
        /// </summary>
        public paramsavimg ParamSauvImg
        {
            get
            { return m_paramsauvimg; }
        }


        // recreation de paramsavimg a partir d'une nouvelle structure
        public void RebuildParamSauvRes(paramsavres newparamsav)
        {
            m_paramsauvres = null;
            m_paramsauvres = newparamsav;
        }


        /// <summary>
        /// permet d'acceder a paramsavimg
        /// </summary>
        public paramsavres ParamSauvRes
        {
            get
            {
                return m_paramsauvres;
            }
        }


        /// <summary>
        /// nom actuel de l'ecchantillon
        /// </summary>
        public string CurEchName
        {
            set { m_CurEchantillonName = value; 
                }
            get { return m_CurEchantillonName;
                }
        }
        
        // mise en foreme d'une mesure selon les paramatres
        public string MiseEnForme(double valeur, etalflagstyp flag)
        {// pou l'instant on fait simple

            // un mic mac pour savoir le caratere de separation utilise sur ce systeme, ca peut faire n'importe quoi (= regle tordue) suivant la config du pc
            String FormatString = "0.0";
            double dumy = 0.0;
            char carsep = dumy.ToString(FormatString)[1]; // on choppe la virgule
            
            // on ecrit avec trop de decimales et on trnque
            int nbdeci = 2;
            if (nbdeci == 0)
                FormatString = "0"; // pas de point
            else
                FormatString = FormatString.PadRight(nbdeci + 2, '0'); // cree la chaine avec les decimales +1 car la long totale = 2 chiffres en plus de 0 et point

            String resu = valeur.ToString(FormatString); // 2 decimales 
            resu = resu.Replace(carsep, '.'); // changement de point decimal ici il va falloir arranger ca quand on a des sytemes pas francais
            return resu;
        }

    }
}

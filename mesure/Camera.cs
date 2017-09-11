// 
//
// 
// 
//
namespace mesure
{
	using System;
	using System.Drawing;
	using System.Threading;
	using VideoSource;
    using System.Windows.Forms;

	/// <summary>
	/// Camera class
	/// </summary>
    public partial class Camera : IBagSavXml,IDisposable
    {
        private Control drawcontrol;
        private IVideoSource videoSource = null;
        private Bitmap lastFrame = null;

        // image width and height
        private int width = 0, height = 0;
        public bool refreshImage(ref Bitmap bmp)
        { return videoSource.refreshimage(ref bmp);
        }

        bool m_freezed = false;

        private clsimgutils.ImgUtil m_imgutils = new clsimgutils.ImgUtil();
        // alarm level
        private double alarmLevel = 0.005;

        //@brief appelle le dial de reglage de la source
        public event EventHandler CamNewFrame;
        public event EventHandler Alarm;
        
        private int prvwidth = 0;
        private int prvheight = 0;
        
        /// <summary>
        /// get : renvoie la bitmap de la derniere trame acquise
        /// </summary>
        public Bitmap LastFrame
        {
            get { return lastFrame; }
        }

        /// <summary>
        /// appelle la boite de ialogue reglage de la source
        /// </summary>
        /// <returns></returns>
        public int Reglage(Control control)
        {
         if (videoSource != null && !m_freezed && videoSource.Running)
            videoSource.DoReglage(control);
         return 0;
        }


        public IVideoSource source
        {
            get { return videoSource; }
        }
       

        /// <summary>
        /// get : width de l'image
        /// </summary>
        public int Width
        {
            get { return width; }
        }
        
        /// <summary>
        /// get : height de l'image
        /// </summary>
        public int Height
        {
            get { return height; }
        }
        

        /// <summary>
        /// nombres d'images recues depuis l'ouverture du flux
        /// </summary>
        public int FramesReceived
        {
            get { return (videoSource == null) ? 0 : videoSource.FramesReceived; }
        }
        
        
        /// <summary>
        /// nombre de bytes recus depuis l'ouverture du flux
        /// </summary>
        public int BytesReceived
        {
            get { return (videoSource == null) ? 0 : videoSource.BytesReceived; }
        }
        
        
        /// <summary>
        /// Running get
        /// </summary>
        public bool Running
        {
            get { return (videoSource == null) ? false : videoSource.Running; }
        }
        
        
        
        /// <summary>
        /// constructeur general
        /// </summary>
        /// <param name="control">controle ou est affice la video</param>
        /// <param name="source">dource de la video</param>
        /// <param name="detector">detecteur utilise</param>
        public Camera(Control control, IVideoSource source)
        {
            this.drawcontrol = control;
            this.videoSource = source;
            if (videoSource == null)
                return;

            videoSource.NewFrame += new CameraEventHandler(video_NewFrame);
            videoSource.OnResize += new SourceResizeHandler(video_Resize);
        }

       /// <summary>
       /// demarrage de la video source
       /// </summary>

        public bool IsFreezed()
        {
            Monitor.Enter(this);
            bool resu = m_freezed;
            Monitor.Exit(this);
            return resu;

        }
        

        public void Start(int interactif)
        {
            if (videoSource != null)
            {
                videoSource.Start(interactif);
                m_freezed = false;
                    return;

            }
        }

        /// <summary>
        ///  gele la video
        /// </summary>
        public void Freeze(bool OnOff)
        {
            if (videoSource != null && videoSource.Running)
            {
                Monitor.Enter(this);
                m_freezed = OnOff;
                Monitor.Exit(this);
            }
            
        }


        /// <summary>
        /// ordre envoye a la video source de s'arreter
        /// </summary>
        public void SignalToStop()
        {
            if (videoSource != null)
            {
                videoSource.SignalToStop();
            }
        }

        /// <summary>
        /// attend arret de la video source apres ordre d'arret
        /// </summary>
        public void WaitForStop()
        {
            if (videoSource != null)
            {
                videoSource.WaitForStop();
                videoSource.Dispose();
            }
         }

        /// <summary>
        /// arret immediat d ela video source : jamais utilise ?
        /// </summary>
        public void Stop()
        {
            // lock
            Monitor.Enter(this);

            if (videoSource != null)
            {
                videoSource.Stop();
                videoSource.Dispose();
            }
            // unlock
            Monitor.Exit(this);
        }

        public void Dispose()
        {
            if (videoSource != null)
            {
                videoSource.Stop();
                videoSource.Dispose();
            }         
        }

        /// <summary>
        /// lock de thread pour acces a la camera
        /// </summary>
        public void Lock()
        {
            Monitor.Enter(this);
        }

        /// <summary>
        /// unlock de thread pour acces a la camera
        /// </summary>
        public void Unlock()
        {
            Monitor.Exit(this);
        }

        /// <summary>
        /// handler de onresize de la source video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void video_Resize(object sender, ref SourceResizeArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, e.width, e.height);
            // ici on init le rect video du clipper et on renvoir le cliprect calcule
            if (CoreSystem.Instance.Etals.Clipper !=null)
            {
                CoreSystem.Instance.Etals.Clipper.SourceRect = rect;
                CoreSystem.Instance.Etals.Clipper.processChangeClipRect(rect);
                e.setcliprect(CoreSystem.Instance.Etals.Clipper.ClipRect);
            }

        }


        /// <summary>
        /// action a realiser sur une nouvelle image
        /// en particulier, si la dmension cahnge : on recaclule cliprect et sourcerect
        /// puis on appelle les evenements lies a Newframe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void video_NewFrame(object sender, CameraEventArgs e)
        {
            if (m_freezed)
                return;
            
            try
            {
                // lock
                Monitor.Enter(this);

                // dispose old frame
                /*if (lastFrame != null)
                {
                    lastFrame.Dispose();
                }
                 * */
                // cahrge l'image dans lastframe
                // qui sera utilise lors de l'evenement newframe
                // (dans lequel mainform execute son camera_newframe)

                
                // image dimension
                width = e.Bitmap.Width;
                height = e.Bitmap.Height;
                if (width != prvwidth || height != prvheight)
                {
                    if (lastFrame != null)
                    {
                        lastFrame.Dispose();
                    }

                    lastFrame = (Bitmap)e.Bitmap.Clone();

                    if (CoreSystem.Instance.Etals.Clipper != null)
                    {
                        CoreSystem.Instance.Etals.Clipper.SourceRect = new Rectangle(0, 0, width, height);
                        CoreSystem.Instance.Etals.Clipper.processChangeClipRect(new Rectangle(0, 0, width, height));
                        prvwidth = width;
                        prvheight = height;
                    }
                }
                else
                    // copie pure et simple de la bitmap
                    m_imgutils.copybmp32tobmp(e.Bitmap,lastFrame);
                    
                
            }
            catch (Exception)
            {
                lastFrame = (Bitmap)e.Bitmap.Clone();
            }
            finally
            {
                // unlock
                Monitor.Exit(this);
            }

            // notify client
            // l'event newframe est adresse par la mainform a sa fonction 
            // camera newframe qui vient lire l'image rangee dans lastframe
            if (CamNewFrame != null)
                CamNewFrame(this, new EventArgs());
        }

        //-------------------------------------------------------------------------
        // appele quand le visurect a change et qu'on doit redessiner des choses
        //-------------------------------------------------------------------------


        public string  getNomType()
        { return (string)"camera";
        }
        
        /// <summary>
        /// interface : renvoie La liste des vesions xml         
        /// </summary>
        /// <returns></returns>
        public string[] getTabVers()
        {
         string[] tabout = { "1.0" };
         return tabout;
         }

        
        /// <summary>
        /// enregistre la camera dans le fichier de config
        /// </summary>
        /// <param name="elem"></param>
        /// <returns></returns>
        public int SaveDisk(XMLAvElement elem)
        {
         
         // enregistre le type et le numero de version
         elem.SetAttribute(xmlavlabels.chktyp, "camera");
         elem.SetAttribute(xmlavlabels.numver, (string) XMLAvElement.getxmlver(getTabVers())); // numero de version de ce bag
            
         
         // sauvegarde la source
         Type letype = videoSource.GetType();
         string strtype = letype.ToString();
            
         XMLAvElement sourcElem = elem.CreateNode("source");
         sourcElem.SetAttribute("type",videoSource.GetType().ToString());
         videoSource.SaveDisk(sourcElem);
            
        // sauvegarde les etalonnages
         foreach (Etalonnage etal in CoreSystem.Instance.Etals.ListEtals)
            {
                XMLAvElement etalElem = elem.CreateNode("etalonnage");
                etal.savedisk(etalElem); // on enregistre l'etalonnage dans cet element
            }

         return 0;
        }

        /// <summary>
        /// initialise la camera avec les donnees du fichier de configuration
        /// </summary>
        /// <param name="elem"></param>
        /// <returns>0 si pas d'erreur</returns>
        public int LoadDisk(XMLAvElement elem)
        {
            string strtType="";

            // on verifie que c'est une camer et on lit le numero de version pour bien decoder ca
            String nomcamera;
            try
            {
                elem.GetAttribute(xmlavlabels.chktyp, out nomcamera);
                String numver;
                elem.GetAttribute(xmlavlabels.numver, out numver); // numero de version de ce bag
            }
            catch (XmlAvException e)
            {if (e.XmlAvType == xmlavexceptiontype.noattrib)
                    return 1; // pas de section camera
                

             return 2; //autre erreur
             };
         
           // on verifie que c'est bien une camera
            if (nomcamera.CompareTo("camera") != 0)
                return 3; // pas bon type

            // reste a tester le numero de version
            // plus tard

            // lecture de la section source
            XMLAvElement sourcElem = elem.GetFirstElement("source");
            if (sourcElem == null) // pas trouve
                return 1; // chargemetn impossible
            try
            {
                sourcElem.GetAttribute("type", out strtType);
            }
            catch (XmlAvException exc)
            { return 1; // une erreur 
            }

            // alloue un objet a partir de son nom
            Type t = Type.GetType (strtType);
            object o = null;
            try
            {
                o = Activator.CreateInstance(t);
            }catch (Exception e)
            {// probleme a la creation de la source : 
             // on ccharge qd meme les etalonnages c'est juste que la source ne marche pas la
             LoadDiskEtals(elem); // on cahrge qd meme les etalonnages. 

                return 1; // impossible de creer la source
            }

            
            //videoSource = (IVideoSource)Activator.CreateInstance (ac,tabstr); // alloue dynaiquement le strttype
            videoSource = (IVideoSource)o; // alloue dynaiquement le strttype

            // affectation des evenements de videosource
            videoSource.NewFrame += new CameraEventHandler(video_NewFrame);
            videoSource.OnResize += new SourceResizeHandler(video_Resize);

            videoSource.LoadDisk(sourcElem);

            // une fois la source chargee on charge les etalobnnages
            LoadDiskEtals(elem);
            return 0;
    
        }

      /// <summary>
      /// charge les etalonnages du fichier xml
      /// </summary>
      /// <param name="elem"></param>
      /// <returns></returns>
 
    public int LoadDiskEtals(XMLAvElement elem)
        {
           // lecture de la section etalonnage
            try
            {
                XMLAvElement etalElem = elem.GetFirstElement("etalonnage");
                while (etalElem != null)
                {
                    Etalonnage letal = new Etalonnage();
                    letal.LoadDisk(etalElem);

                    // definit le calculator dans le core
                    CoreSystem.Instance.Etals.ListEtals.Add(letal); // ajout a la liste des etals
                    etalElem = elem.GetNextElement();
                }
            }
            catch (XmlAvException a)
            { }

            return 0; // pas d'erreur
        }

    }
}

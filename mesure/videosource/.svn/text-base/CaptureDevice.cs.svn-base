// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com 
//
namespace VideoSource
{
	using System;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Threading;
	using System.Runtime.InteropServices;
	using System.Net;
    using System.Collections;
    
    //using System.ComponentModel;
    using System.ComponentModel;
  
    using System.Collections.Generic;
    using System.Windows.Forms;
    using DirectShowLib;
    using System.Diagnostics;
    //using CLRClassImgUtil;
    using mesure;
    using clsimgutils;
    
    
	/// <summary>
	/// CaptureDevice - capture video from local device
	/// </summary>
	public class CaptureDevice : IVideoSource
	{
        //[DllImport("ImgUtility.dll", EntryPoint = "?copy32ligne@@YGHPAD0K@Z")]
     //public static extern  void copy32ligne(IntPtr dst, IntPtr src, int bytelen);
       
		private string	m_sSourceName;
		private object	userData = null;
		private int		framesReceived;
        Bitmap m_lstimage = null;

        private int m_InitWidth=0;
        private int m_InitHeight =0;
        private int m_interactive = 0;
        private bool m_isfreezed=false;

		//private Thread	thread = null;
		private ManualResetEvent stopEvent = null;
        //static CImgUtility util = new CImgUtility();
		// new frame event
		public event CameraEventHandler NewFrame;
        public event SourceResizeHandler OnResize;

        static Rectangle m_cliprect;
        Grabber grabber = null;

        public bool capchglive()  // peut etre live/freeze
        {return true;
        }

        public bool capgrab()  // peut etre live/freeze
        {
            return true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool freeze()
        {
            if (!m_isfreezed)
            {
                m_mediaControl.Stop();
                m_isfreezed = true;
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool live()
        {
            if (m_isfreezed)
            {
                m_mediaControl.Run();
                m_isfreezed = false;
                return true;
            }
       
            return false;
        }

        public bool grab()
        {
            m_isfreezed = true;
            live();
            freeze();
            return true;
        }

       /// <summary>
       /// une boite de dialogue regalge est disponible
       /// </summary>
       /// <returns></returns>
        bool IVideoSource.capregl()  // peut etre reglé
        {return true;
        }
        
        bool IVideoSource.isfreeze()
            {return m_isfreezed;
            }
        

        // ibagxml
        public string[] getTabVers()
        {
            string[] tabvers = { "1.0" };
            return tabvers;
        }
        public string getNomType()
        {
            return "DShowVideo";
        }


        public bool refreshimage(ref Bitmap bmp)
        { if (m_isfreezed)
        {
            bmp = m_lstimage;
                return true;
            }
                else
            return false; // pas d'image dispo
        }

        public Rectangle ClipRect
        {set { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int SaveDisk(XMLAvElement element)
        {
            element.SetAttribute(xmlavlabels.chktyp, this.getNomType());
            element.SetAttribute(xmlavlabels.numver, XMLAvElement.getxmlver(this.getTabVers())); 
                
            
            XMLAvElement param = element.CreateNode(xmlavlabels.param);
            param.SetAttribute("nom", m_sSourceName);
            return 0;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int LoadDisk(XMLAvElement element)
        { // on verifie que le type de l'element est bon 
          
          //----  contenu standard de tous les bags  -----
          string chktype ;
          if (element.GetAttribute(xmlavlabels.chktyp, out chktype) == 0)
             return 1;
          
          if (chktype.CompareTo(this.getNomType()) != 0)
                return 1;


           // on lit le contenu de 'params'
           XMLAvElement param = element.GetFirstElement(xmlavlabels.param);
           if (param == null)
                {
                m_sSourceName = null; // opas de source 
                return 0;
                }
     
            //----------------------------------

           // puis lecture des parametres
           string nomsource;
           param.GetAttribute("nom", out nomsource);
           
            // lecture des caracteristiques de la source a reprogrammer a la place du ialogue
           this.m_InitHeight = 960;
           this.m_InitWidth = 1280;

            // init des memebres de la classe
            m_sSourceName = nomsource;
            return 0;
        }
        
        //----------------------------------------
        // ouvre la boite de dialogue de reglage
        //----------------------------------------
        [DllImport("olepro32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int OleCreatePropertyFrame(
            IntPtr hwndOwner, int x, int y,
            string lpszCaption, int cObjects,
            [In, MarshalAs(UnmanagedType.Interface)] ref object ppUnk,
            int cPages, IntPtr pPageClsID, int lcid, int dwReserved, IntPtr pvReserved);


        /// <summary>
        /// ouvre le dial de reglage
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public int DoReglage(Control parent)
        {
            if (m_capFilter != null)
                DisplayPropertyPage(m_capFilter,parent,1);
         return 1;
        }
        
        /// <summary>
        /// initialise le format de capture
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public int DoInit(Control parent)
        {
            return 0; // le doinit ne fait plus l'ouverture de la boite de dialogue
            if (m_capFilter != null)
                DisplayPropertyPage(m_capFilter, parent,2);
            return 1;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dev"></param>
        /// <param name="Parent"></param>
        /// typpage =1 : reglages
        /// typpage = 2 : entree
        private void DisplayPropertyPage(IBaseFilter dev, Control Parent,int typepage)
        {
            //Get the ISpecifyPropertyPages for the filter
            ISpecifyPropertyPages pProp = dev as ISpecifyPropertyPages;
            int hr = 0;
            IPin iPinOutSource=null;

            if (pProp == null)
                return;

            //Get the name of the filter from the FilterInfo struct
            FilterInfo filterInfo;
            hr = dev.QueryFilterInfo(out filterInfo);


            // Get the propertypages from the property bag
            DsCAUUID caGUID;
            hr = pProp.GetPages(out caGUID);
            //DsError.ThrowExceptionForHR(hr);
            if (hr != 0)
                throw new DeviceCreateException(1, "getproppages");

            // Check for property pages on the output pin
            ISpecifyPropertyPages pinProp = null;
            if (m_capFilter != null)
            {
                iPinOutSource = DsFindPin.ByDirection(m_capFilter, PinDirection.Output, 0);
                pinProp = iPinOutSource as ISpecifyPropertyPages;
            }
            DsCAUUID caGUID2 = new DsCAUUID();

            if (pinProp != null)
            {
                hr = pinProp.GetPages(out caGUID2);
                if (hr != 0)
                    throw new DeviceCreateException(1, "getproppages 2");
                //DsError.ThrowExceptionForHR(hr);

                if (caGUID.cElems > 0)
                {
                    int soGuid = Marshal.SizeOf(typeof(Guid));

                    // Create a new buffer to hold all the GUIDs
                    IntPtr p1 = Marshal.AllocCoTaskMem((caGUID.cElems) * soGuid);

                    // Copy over the pages from the Filter
                    for (int x = 0; x < caGUID.cElems * soGuid; x++)
                    {
                        Marshal.WriteByte(p1, x, Marshal.ReadByte(caGUID.pElems, x));
                    }

                    // Release the old memory
                    Marshal.FreeCoTaskMem(caGUID.pElems);

                    // Reset caGUID to include both
                    caGUID.pElems = p1;
                }


                if (caGUID2.cElems > 0)
                {
                    int soGuid2 = Marshal.SizeOf(typeof(Guid));

                    // Create a new buffer to hold all the GUIDs
                    IntPtr p2 = Marshal.AllocCoTaskMem((caGUID.cElems) * soGuid2);

                    // Copy over the pages from the Filter
                    for (int x = 0; x < caGUID.cElems * soGuid2; x++)
                    {
                        Marshal.WriteByte(p2, x, Marshal.ReadByte(caGUID2.pElems, x));
                    }

                    // Release the old memory
                    Marshal.FreeCoTaskMem(caGUID2.pElems);

                    // Reset caGUID to include both
                    caGUID2.pElems = p2;
                }
            }

            // Create and display the OlePropertyFrame
            // ca amrche mais les controles sont grises. il faudrait defaire le lien ?

            //IPin iPinOutSource2 = DsFindPin.ByDirection(m_capFilter, PinDirection.Output, 0);
               
            // essai de deconnection des pins)
                m_mediaControl = (IMediaControl)this.m_FilterGraph;
          

            // stop si on controle une pin pas stop si on controle l'caquisition
               // if (typepage == 2)
                 //   hr = m_mediaControl.Stop(); // arret du filtre (parfois ca empeche la deconnecxion

                //IPin iPinOutSource2 = DsFindPin.ByDirection(capFilter, PinDirection.Output, 0);
                //IBaseFilter baseGrabFlt = sampGrabber as IBaseFilter;
                //iPinInFilter= DsFindPin.ByDirection(baseGrabFlt, PinDirection.Input, 0);
                //hr = m_FilterGraph.Disconnect(iPinOutSource2); // ne marche pas : hr !=0
            

            //SaveSizeInfo(sampGrabber);

            

            object oDevice = (object)dev;
            object oPin = (object)iPinOutSource;
            if (typepage == 2)
                hr = OleCreatePropertyFrame(Application.OpenForms[0].Handle, 0, 0, filterInfo.achName, 1, ref oPin, caGUID2.cElems, caGUID2.pElems, 0, 0, IntPtr.Zero);
            if (typepage == 1)
                hr = OleCreatePropertyFrame(Parent.Handle, 0, 0, filterInfo.achName, 1, ref oDevice, caGUID.cElems, caGUID.pElems, 0, 0, IntPtr.Zero);
            //DsError.ThrowExceptionForHR(hr);
            if (hr != 0)
                throw new DeviceCreateException(1, "olecratepropertyframes");
            
            
            // run si on a fait stop avant
            //if (typepage == 2)
              //  hr = m_mediaControl.Run(); // arret du filtre


            // Release COM objects
            Marshal.FreeCoTaskMem(caGUID.pElems);
            Marshal.FreeCoTaskMem(caGUID2.pElems);
            Marshal.ReleaseComObject(pProp);
            if (filterInfo.pGraph != null)
            {
                Marshal.ReleaseComObject(filterInfo.pGraph);
            }

            if (typepage == 2)
                return;

            SaveSizeInfo(m_sampGrabber as ISampleGrabber);
            // onr ecalcule tout le truc por un cas de changement de size
            //si la taille a change : OnResize passe LeftRightAlignment nouveau Rectangle encombrement AuthenticationManager clipping
            // on passe le rect 
            Rectangle ClipRect = new Rectangle();

            SourceResizeArgs msg = new SourceResizeArgs(CaptureDevice.m_videoWidth, CaptureDevice.m_videoHeight, ClipRect);
            if (OnResize != null)
                OnResize(this, ref msg); // init le clipper

            m_cliprect = msg.cliprect; // recupere le clip rect
            //--- programme le grabber ------
            grabber.ClipRect = m_cliprect;

            ///programme les variables necessaires a la calllback
            // ici on peut ete amene a recopier la bitmap dans une aurtre bitmap pour le cliippping

            //clipper = new ClippingSystem(grabber.Width, grabber.Height);
            Debug.Write("grabber init" + grabber.Width + "-" + grabber.Height);
           
        }


        //----------------------------------------
        // VideoSource property
        //----------------------------------------
        public virtual string VideoSource
		{
            get { return m_sSourceName; }
            set { m_sSourceName = value; }
		}

        //----------------------------------------
        // Login property
        //----------------------------------------
        public string Login
		{
			get { return null; }
			set { }
		}
        //----------------------------------------
        // Password property
        //----------------------------------------
        public string Password
		{
			get { return null; }
			set {  }
		}
        //----------------------------------------
        // FramesReceived property
        //----------------------------------------
        public int FramesReceived
		{
			get
			{
				int frames = framesReceived;
				framesReceived = 0;
				return frames;
			}
		}
        //----------------------------------------
        // BytesReceived property
        //----------------------------------------
        public int BytesReceived
		{
			get { return 0; }
		}

        //----------------------------------------
        // UserData property
        //----------------------------------------
        public object UserData
		{
			get { return userData; }
			set { userData = value; }
		}
        
        //----------------------------------------
        // Get state of the video source thread
        //----------------------------------------
        public bool Running
		{
			get
            {
                return (grabber != null);
			/*	if (thread != null)
				{
					if (thread.Join(0) == false)
						return true;

					// the thread is not running, so free resources
					Free();
				}
				return false;
             * */
                //return false;
			}
		}


        //----------------------------------------
        // Constructor
        //----------------------------------------
        public CaptureDevice()
		{
		}

        public void Dispose()
        { }

        //----------------------------------------
		// Start work
        // lancement de l'acquisition sous forme de thread 
        //----------------------------------------
        public void Start(int interactif)
        {
            Debug.Write("Start");

			//if (thread == null)
			{
				framesReceived = 0;

				// create events
				//stopEvent	= new ManualResetEvent(false);
				
				// create and start new thread
				//thread = new Thread(new ThreadStart(WorkerThread));
				//thread.Name = source;
                //Debug.Write("thrd start");
                //thread.Start();
                startacquisition(interactif);
                m_isfreezed = false;
			}
		}

        //----------------------------------------
        // Signal thread to stop work
        // dit au thread d'areter
        //----------------------------------------
        public void SignalToStop()
        {
            if (m_mediaControl != null)
            {
                m_mediaControl.StopWhenReady();
                m_isfreezed = true;
            }             
		}

        //----------------------------------------
        // Wait for thread stop
        // attend l'arret du thread
        //----------------------------------------
        public void WaitForStop()
		{
            if (m_mediaControl != null)
			{
              Marshal.ReleaseComObject(m_capFilter);
              m_capFilter = null;
              m_FilterGraph = null;
            }
		}

		//-------------------------------------------
        // tue le thread tout simplement
        // Abort thread
        //-------------------------------------------
        public void Stop()
        {if (m_mediaControl != null)
            {
                m_mediaControl.StopWhenReady();
                m_mediaControl = null;
                m_isfreezed = true;
            }			
		}

        //-------------------------------------------
        // Free resources
        //-------------------------------------------
        private void Free()
		{
			// release events
			stopEvent.Close();
			stopEvent = null;
		}
        
        
        IMediaControl m_mediaControl = null;
        IGraphBuilder m_graphBuilder = null;
        IBaseFilter m_capFilter = null;
        ISampleGrabber m_sampGrabber = null;        
        IPin m_iPinInFilter = null;
        IPin m_iPinOutFilter = null;
        bool m_bfirstsample = true;       
        private IFilterGraph2 m_FilterGraph;

        //------------------------------------------
        //------------------------------------------

        public int  GetListSources(ArrayList List)
        {
            Guid iid = typeof(IBaseFilter).GUID;

            // ici on recherche le videoinput qui a le meme nom que celui choisi dans la scrollbox
            // et on l'instancie en le liant a l'objet avec le moniker qui decode son nom

            // Get the collection of video devices
            DsDevice[] capDevices;
            capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            ArrayList filters = new ArrayList();
            for (int i = 0; i < capDevices.Length; i++)
                List.Add(capDevices[i].Name);
            
            return List.Count;
            
        }


        //-----------------------------
        // mise en route du filtre
        //-----------------------------
        public void InitGraph(int width, int height)
        {
            m_InitWidth = width;
            m_InitHeight = height;
            InitGraph(0);
        }

        /// <summary>
        /// pas d'argument : ouverture de la boite de dialogue pour les regler
        /// </summary>
        public void InitGraph()
        {
            InitGraph(1);
        }


        /// <summary>
        /// interactive dit si on regle les argumentadans la boite ou par paramatres
        /// </summary>
        public void InitGraph(int interactive)

        { m_FilterGraph = new FilterGraph() as IFilterGraph2;

            // ouvre le dial de choix de source
            // et cree thedivice correspondant au choix
         	//Release COM objects
			if (m_capFilter != null)
			{
				Marshal.ReleaseComObject(m_capFilter);
				m_capFilter = null;
			}

            logger.log("ouverture de [" + m_sSourceName+"]"); // log le liste des device
       	    //Create the filter for the selected video input device
            //string devicepath = "RZ300C"; // comboBox1.SelectedItem.ToString();
            string devicepath = m_sSourceName; // comboBox1.SelectedItem.ToString();
            object source = null;

            Guid iid = typeof(IBaseFilter).GUID;
            DsDevice[] capDevices;
            capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            bool found = false;
            // on retrouve le device dontle nom a ete choisi
            for (int i = 0; i < capDevices.Length; i++)
                {DsDevice device = capDevices[i];
                logger.log("device = " + device.Name); // log le liste des device
                    if (device.Name.CompareTo(devicepath) == 0)
                        {
                        int hresu;
                        device.Mon.BindToObject(null, null, ref iid, out source);
                        logger.log("bind to [" + source.ToString()+ "]"); // log le liste des device
                        found = true;
                        // lecture des parametres de ce truc
                      /*  Guid bagiid = typeof(IPropertyBag).GUID;
                        object bagobj;
                        device.Mon.BindToStorage(null, null, ref bagiid, out bagobj);
                        IPropertyBag lebag = bagobj as IPropertyBag;
                        object lavaleur;
                        lebag.Read((String)"DeviceDesc", out lavaleur, null);

                        System.String toto = lavaleur.ToString();
                       * */
                        break;
                        }
                }


            if (!found)
                throw new DeviceCreateException(1, "device not found");

            // ici il faut traiter le cas ou on trouve pas la source
            // de meme en cas de rechargement depuis un fichier

        
            m_capFilter = (IBaseFilter)source;
            Type captype = m_capFilter.GetType();
            
            // analyse des pins
            IEnumPins PinEnum;
            int nbpins = m_capFilter.EnumPins( out PinEnum);
            IPin[] tabpins = new IPin[1];
            while (0 == PinEnum.Next(1, tabpins, IntPtr.Zero))
                {string idString;
                tabpins[0].QueryId(out idString);
                
                // liste des medias typs acceptes
                IEnumMediaTypes ppEnum;
                tabpins[0].EnumMediaTypes(out ppEnum);
                AMMediaType[] MType = new AMMediaType[1];
                while (0 == ppEnum.Next(1,MType,IntPtr.Zero))
                    {//string medianame;
                    //MType. .idString(medianame);
                    //logger.log("media" +  MType.medianame+ "[" + pinfo.name+"]"); // log le liste des device                                
             
                    PinInfo pinfo;
                    tabpins[0].QueryPinInfo(out pinfo);
                    logger.log("... pin ID = " + idString + "[" + pinfo.name+"]"); // log le liste des device                                
                    }
                }
            /*
            // ici on lit la pin capture
            hr = pin.ConnectionMediaType(pmt);
            DsError.ThrowExceptionForHR(hr);

            if (pmt.formatType == FormatType.VideoInfo2)
            {
             VideoInfoHeader2 vih2 = (VideoInfoHeader2)Marshal.PtrToStructure(pmt.formatPtr, typeof(VideoInfoHeader2));
            }
            else if (pmt.formatType == FormatType.VideoInfo)
            {
             VideoInfoHeader vih = (VideoInfoHeader)Marshal.PtrToStructure(pmt.formatPtr, typeof(VideoInfoHeader));
            }
            */
            
            // Get the SampleGrabber interface
            m_sampGrabber = new SampleGrabber() as ISampleGrabber;                

            // recupere la pin d'entree du grabber
            IBaseFilter baseGrabFlt = m_sampGrabber as IBaseFilter;
            m_iPinInFilter = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Input, 0);
            logger.log("grab pin in [" +m_iPinInFilter.ToString()+ "]"); // log le liste des device
                        
 
            // configure le samplegrabber
            AMMediaType media;
            int hr;

            // Set the media type to Video/RBG24
            media = new AMMediaType();
            media.majorType = MediaType.Video;
            media.subType = MediaSubType.RGB24;
            media.formatType = FormatType.VideoInfo;
            
            logger.log("setmediatype"); // log le liste des device

            hr = m_sampGrabber.SetMediaType(media);
            if (hr !=0)
                throw new DeviceCreateException(1, "set media type error");
            //DsError.ThrowExceptionForHR(hr);
            
            if (interactive == 0)
            {
                object o;
                IAMVideoControl videoControl = m_capFilter as IAMVideoControl;
                //ICaptureGraphBuilder2 capGraph = m_graphBuilder as ICaptureGraphBuilder2;
                ICaptureGraphBuilder2 capGraph = (ICaptureGraphBuilder2)new CaptureGraphBuilder2(); 
 

                hr = capGraph.FindInterface(PinCategory.Capture, MediaType.Video, m_capFilter, typeof(IAMStreamConfig).GUID, out o);

                IAMStreamConfig videoStreamConfig = o as IAMStreamConfig;
                hr = videoStreamConfig.GetFormat(out media);
                //DsError.ThrowExceptionForHR(hr); 
 
                // copy out the videoinfoheader 
                VideoInfoHeader v = new VideoInfoHeader();
                Marshal.PtrToStructure(media.formatPtr, v);

                // if overriding the framerate, set the frame rate 
                /*if (iFrameRate > 0)
                {
                    v.AvgTimePerFrame = 10000000 / iFrameRate;
                } */

                int iWidth = 640;
                // if overriding the width, set the width 
                if (iWidth > 0)
                {
                    v.BmiHeader.Width = iWidth;
                }

                int iHeight = 380;
                // if overriding the Height, set the Height 
                if (iHeight > 0)
                {
                    v.BmiHeader.Height = iHeight;
                }

                // Copy the media structure back 
                Marshal.StructureToPtr(v, media.formatPtr, false);

                // Set the new format 
                hr = videoStreamConfig.SetFormat(media);
                //DsError.ThrowExceptionForHR(hr); 
 


                /*
                // copy out the videoinfoheader 
                VideoInfoHeader v = new VideoInfoHeader();
                Marshal.PtrToStructure(media.formatPtr, v); 
 

                hr = m_sampGrabber.GetConnectedMediaType(media); // on relit             
                VideoInfoHeader Header = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));

                Header.BmiHeader.BitCount = (short)Marshal.SizeOf(typeof(BitmapInfoHeader)); // recupere le sizeof de la structure correspondant
                Header.BmiHeader.Height = 1280;  //m_InitWidth;
                Header.BmiHeader.Width = 960; // m_InitHeight;
                Header.BmiHeader.BitCount = 24;
                Header.BmiHeader.Planes = 1;
                
                IntPtr infoptr = media.formatPtr;
                Marshal.StructureToPtr(Header, infoptr, false);

                Header.BmiHeader.ImageSize = Marshal.SizeOf(Header.BmiHeader);

                media.formatType = new Guid("05589F80-C356-11CE-BF01-00AA0055595A");
                media.formatSize = (int)Marshal.SizeOf(typeof(VideoInfoHeader)); 

                hr = m_sampGrabber.SetMediaType(media);
                 * */
            }


            DsUtils.FreeAMMediaType(media);
            media = null;
                    
            //Add the Video input device to the graph
            logger.log("add source filter"); // log le liste des device
            hr = m_FilterGraph.AddFilter(m_capFilter, "source filter");
            if (hr != 0)
                throw new DeviceCreateException(1, "add source filter");
            //DsError.ThrowExceptionForHR(hr);

            logger.log("add grabber"); // log le liste des device
            hr = m_FilterGraph.AddFilter(baseGrabFlt, "grabber");
            if (hr != 0)
                throw new DeviceCreateException(1, "add grabber");
            //DsError.ThrowExceptionForHR(hr);


            // Hopefully this will be the video pin
            logger.log("cap pin out 0"); // log le liste des device
            IPin iPinOutSource = DsFindPin.ByDirection(m_capFilter, PinDirection.Output, 0);
            logger.log("grab source pin out [" + iPinOutSource.ToString() + "]"); // log le liste des device

            if (interactive == 1)
            {
                
                DisplayPropertyPage(m_capFilter, null, 2); // pour init la taille
                
            }

            // Connect the graph.  Many other filters automatically get added here
            logger.log("connect pins"); // log le liste des device
            hr = m_FilterGraph.Connect(iPinOutSource, m_iPinInFilter);
            if (hr != 0)
                throw new DeviceCreateException(1, "connect pins");

            SaveSizeInfo(m_sampGrabber);

            logger.log("connection ok"); // log le liste des device
            
            // on positionne le width et height de la video
        //    m_videoWidth = m_cliprect.Width;
         //   m_videoHeight = m_cliprect.Height;
            m_stride = m_videoWidth * 3; //24bpp//(videoInfoHeader.BmiHeader.BitCount / 8);  // faux il faut aligner en dword...

            m_bfirstsample = true;
            // installe la callback
            logger.log("install callback"); // log le liste des device

            hr = m_sampGrabber.SetCallback(grabber as ISampleGrabberCB, 1);
            //DsError.ThrowExceptionForHR(hr);
            if (hr != 0)
                throw new DeviceCreateException(1, "set callback");
            logger.log("install callback ok"); // log le liste des device
            
        }


        


        ///-----------------------------------
        // Thread entry point
        //------------------------------
        IBaseFilter sourceBase = null;
        static int m_videoWidth = 0;
        static int m_videoHeight = 0;
        int m_stride = 0;


        /// <summary>
        /// lit les caracteristique de l'image grabbee et positionne les variables courantes
        /// </summary>
        /// <param name="legrabber"></param>
        private void SaveSizeInfo(ISampleGrabber legrabber)
        {
            int hr;
            if (legrabber == null)
                return;

            // Get the media type from the SampleGrabber
            AMMediaType media = new AMMediaType();
            hr = legrabber.GetConnectedMediaType(media);
            //DsError.ThrowExceptionForHR(hr);
            if (hr != 0)
                throw new DeviceCreateException(2, "get connected media types");


            if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
            {throw new DeviceCreateException(1, "Unknown Grabber Media Format");
             //   throw new NotSupportedException("Unknown Grabber Media Format");
            }

            // Grab the size info
            VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));

            
            // lecture des caracteristiques des images
            m_videoWidth = videoInfoHeader.BmiHeader.Width;
            m_videoHeight = videoInfoHeader.BmiHeader.Height;
            m_stride = m_videoWidth * (videoInfoHeader.BmiHeader.BitCount / 8);


            DsUtils.FreeAMMediaType(media);
            media = null;
        }
        




        //---------------------------------------------------------
        //
        //---------------------------------------------------------        
        void startacquisition(int interactif)
        {
            try
            {
                // grabber
                grabber = new Grabber(this);
                try
                {
                    InitGraph(interactif);
                }
                catch (DeviceCreateException exc)
                    {//ici il faudrait logger lock'excetion
                        logger.log("create device exception : " + exc.Legende); // log le liste des device

                    grabber = null; // on libere le grabber
                     return; // on sort
                    }

                Debug.Write("work init");
                // on demarre le graph
                // on recupere les dimensions du saplegrabber
                SaveSizeInfo(m_sampGrabber as ISampleGrabber);
                // prepare les bitmap de la callback
                //SetupBitmap();

                //utilisation de l'interface mediacontrol pour controller le graph
                //Create the media control for controlling the graph
                //IMediaControl mediaControl;
                m_mediaControl = (IMediaControl)this.m_FilterGraph;
                //SaveSizeInfo(sampGrabber);

                m_mediaControl.Run(); // ca demarre

                Debug.Write("in workthrd");

                
                //si la taille a change : OnResize passe LeftRightAlignment nouveau Rectangle encombrement AuthenticationManager clipping
                // on passe le rect 
                Rectangle ClipRect = new Rectangle();

                SourceResizeArgs msg = new SourceResizeArgs(CaptureDevice.m_videoWidth, CaptureDevice.m_videoHeight, ClipRect);
                if (OnResize != null) 
                    OnResize(this, ref msg); // init le clipper
                
                m_cliprect = msg.cliprect; // recupere le clip rect
                //--- programme le grabber ------
                grabber.ClipRect = m_cliprect;
                
                ///programme les variables necessaires a la calllback
                // ici on peut ete amene a recopier la bitmap dans une aurtre bitmap pour le cliippping
                
                //clipper = new ClippingSystem(grabber.Width, grabber.Height);
                Debug.Write("grabber init" + grabber.Width + "-" + grabber.Height);
            }
            finally
            {
            }
       }

		//--------------------------------------------------------------------
        // event appelé par le grabber a chaque buffercb
        // appelle les events installes dans Newrame si on n'as pas stoppé l'appli
        // en general on a un event installe par la camera dedans 
        // qui va sur videonewframe, lequel va cloner la bitmap 
        // puis declancher le NewFrame de la camera 
        // qui va appeler camera_newframe du mainform qui se charge surtout d'envoyer la bitmap
        // de la video dans la panel video
        //-----------------------------------------------------------------------
        protected void OnNewFrame(Bitmap image)
        {
            m_lstimage = image;
			framesReceived++;
			//if ((!stopEvent.WaitOne(0, true)) && (NewFrame != null))
            if (NewFrame != null)
				NewFrame(this, new CameraEventArgs(image));
		}

		//-------------------------------------
        // Grabber
        // impplementation de isamplegrabbercb 
        //-------------------------------------
		private class Grabber : ISampleGrabberCB
		{
            private ImgUtil imgutil = new ImgUtil();
    
            private CaptureDevice parent;
			private int width, height;
            Rectangle m_cliprect;
            System.Drawing.Bitmap m_img = null;

            int prvwidth = -1;
            int prvheight = -1;

            public System.Drawing.Bitmap img
            {
                get { return m_img; }
            }

            //------------------------------------------------------
            public Rectangle ClipRect
            { 
                set { m_cliprect = value; }
            }

            //------------------------------------------------------
            // Width property
            //------------------------------------------------------
            public int Width
			{
				get { return width; }
				set { width = value; }
			}
            
            //------------------------------------------------------
            // Height property
            //------------------------------------------------------
            public int Height
			{
				get { return height; }
				set { height = value; }
			}
            
            //------------------------------------------------------
            // Constructor
            //------------------------------------------------------
            public Grabber(CaptureDevice parent)
			{
				this.parent = parent;
			}

            //------------------------------------------------------
            //
            //------------------------------------------------------
            public int SampleCB(double SampleTime, IMediaSample pSample)
			{
				return 0;
			}
            
            
            /// <summary>
            /// Callback method that receives a pointer to the sample buffer			
            /// </summary>
            /// <param name="SampleTime"></param>
            /// <param name="pBuffer"></param>
            /// <param name="BufferLen"></param>
            /// <returns></returns>
            public int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
            {

                // create new image
                // copie de la zone clippee 
                Rectangle cliprect = m_cliprect;
                if (m_img == null || prvwidth != cliprect.Width || prvheight != cliprect.Height)
                {
                    if (m_img != null)
                    {
                        m_img.Dispose();
                        m_img = null;
                    }
                    m_img = new Bitmap(cliprect.Width, cliprect.Height, PixelFormat.Format24bppRgb);
                    prvwidth = cliprect.Width ;
                    prvheight = cliprect.Height;
                    }
                // newimg = center.getfreebuffer()
                //imgutil.copybuftobmp(pBuffer, newimg, m_cliprect, 1);
                //parent.OnNewFrame(newimg);
                    try
                    {
                        imgutil.copybuftobmp(pBuffer, m_img, m_cliprect, 1);
                    }
                    catch (Exception e)
                    {
                        if (m_img != null)
                            m_img.Dispose();

                        m_img = new Bitmap(cliprect.Width, cliprect.Height, PixelFormat.Format24bppRgb);
                        return 0;
                    }
				
                // notify parent
                parent.OnNewFrame(m_img);
				return 0;
			}
		}
	}
    
    
    /// <summary>
    /// exception application exception : deice not found
    /// </summary>
    class DeviceCreateException : ApplicationException
    {
        private string legend;
        private int numexception;


        /// <summary>
        /// recupere la legende
        /// </summary>
        public string Legende
        { get { return legend; }
        }
        
        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="numexception"></param>
        /// <param name="legend"></param>
        public DeviceCreateException(int numexception, string legend)
        {this.legend = legend;
         this.numexception = numexception;
        }

        
    }

}

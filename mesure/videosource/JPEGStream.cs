// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//


using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using System.Collections;
using mesure;

namespace VideoSource
{
	
	/// <summary>
	/// JPEGSource - JPEG downloader
	/// </summary>
	public class JPEGStream : IVideoSource
	{
        private string  m_sSourceName;
		private string	login = null;
		private string	password = null;
		private object	userData = null;
		private int		framesReceived;
		private int		bytesReceived;
		private bool	useSeparateConnectionGroup = false;
		private bool	preventCaching = false;
		private int		frameInterval = 0;		// frame interval in miliseconds

		private const int	bufSize = 512 * 1024;	// buffer size
		private const int	readSize = 1024;		// portion size to read

		private Thread	thread = null;
		private ManualResetEvent stopEvent = null;
        // new frame event
        public event CameraEventHandler NewFrame;
        public event SourceResizeHandler OnResize;


        bool IVideoSource.capchglive() { return false; }  // peut etre live ou freeze
        bool IVideoSource.capgrab() { return false; }  // peut saisir une image
        bool IVideoSource.capregl() { return false; }  // peut etre reglé
        bool IVideoSource.isfreeze() { return true; } // est freeze
        bool IVideoSource.freeze() { return false; }
        bool IVideoSource.live() { return false; }
        bool IVideoSource.grab() { return false; }



        public Rectangle ClipRect
        {set { }
        }

        // ibagxml
        public string[] getTabVers()
        {
            string[] tabvers = { "1.0" };
            return tabvers;
        }
        public string getNomType()
        {
            return "JPGStream";
        }


        public bool refreshimage(ref Bitmap bmp)
        { return false; }
        
     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int SaveDisk(XMLAvElement element)
        {// standard de tous les bags
            element.SetAttribute(xmlavlabels.chktyp, this.getNomType());
            element.SetAttribute(xmlavlabels.numver, XMLAvElement.getxmlver(this.getTabVers()));


            XMLAvElement param = element.CreateNode(xmlavlabels.param);
            //----------------------------------

            element.SetAttribute("URL", m_sSourceName);
         
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int LoadDisk(XMLAvElement element)
        {//----  contenu standard de tous les bags  -----
            string chktype;
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
            string sourcename = "";
            element.GetAttribute("URL", out sourcename);
            m_sSourceName = sourcename;
            return 0;
            return 0;
        }
        
        public int DoReglage(Control source)
        { return 0;
        }

        public int DoInit(Control parent)
        { return 0; }
        
        public int GetListSources(ArrayList List)
        { return 0; }
        
		
		// SeparateConnectioGroup property
		// indicates to open WebRequest in separate connection group
		public bool	SeparateConnectionGroup
		{
			get { return useSeparateConnectionGroup; }
			set { useSeparateConnectionGroup = value; }
		}
		// PreventCaching property
		// If the property is set to true, we are trying to prevent caching
		// appneding fake parameter to URL. It's needed is client is behind
		// proxy server.
		public bool	PreventCaching
		{
			get { return preventCaching; }
			set { preventCaching = value; }
		}
		// FrameInterval property - interval between frames
		// If the property is set 100, than the source will produce 10 frames
		// per second if it possible
		public int FrameInterval
		{
			get { return frameInterval; }
			set { frameInterval = value; }
		}
		// VideoSource property
		public virtual string VideoSource
		{
            get { return m_sSourceName; }
            set { m_sSourceName = value; }
		}
		// Login property
		public string Login
		{
			get { return login; }
			set { login = value; }
		}
		// Password property
		public string Password
		{
			get { return password; }
			set { password = value; }
		}
		// FramesReceived property
		public int FramesReceived
		{
			get
			{
				int frames = framesReceived;
				framesReceived = 0;
				return frames;
			}
		}
		// BytesReceived property
		public int BytesReceived
		{
			get
			{
				int bytes = bytesReceived;
				bytesReceived = 0;
				return bytes;
			}
		}
		// UserData property
		public object UserData
		{
			get { return userData; }
			set { userData = value; }
		}
		// Get state of the video source thread
		public bool Running
		{
			get
			{
				if (thread != null)
				{
					if (thread.Join(0) == false)
						return true;

					// the thread is not running, so free resources
					Free();
				}
				return false;
			}
		}

		// Constructor
		public JPEGStream()
		{
		}


        public void Dispose()
        { }

		// Start work
		public void Start(int interactif)
		{
			if (thread == null)
			{
				framesReceived = 0;
				bytesReceived = 0;

				// create events
				stopEvent	= new ManualResetEvent(false);
				
				// create and start new thread
				thread = new Thread(new ThreadStart(WorkerThread));
                thread.Name = m_sSourceName;
				thread.Start();
			}
		}

		// Signal thread to stop work
		public void SignalToStop()
		{
			// stop thread
			if (thread != null)
			{
				// signal to stop
				stopEvent.Set();
			}
		}

		// Wait for thread stop
		public void WaitForStop()
		{
			if (thread != null)
			{
				// wait for thread stop
				thread.Join();

				Free();
			}
		}

		// Abort thread
		public void Stop()
		{
			if (this.Running)
			{
				thread.Abort();
				WaitForStop();
			}
		}

		// Free resources
		private void Free()
		{
			thread = null;

			// release events
			stopEvent.Close();
			stopEvent = null;
		}

		// Thread entry point
		public void WorkerThread()
		{
			byte[]			buffer = new byte[bufSize];	// buffer to read stream
			HttpWebRequest	req = null;
			WebResponse		resp = null;
			Stream			stream = null;
			Random			rnd = new Random((int) DateTime.Now.Ticks);
			DateTime		start;
			TimeSpan		span;

			while (true)
			{
				int	read, total = 0;

				try
				{
					start = DateTime.Now;

					// create request
					if (!preventCaching)
					{
                        req = (HttpWebRequest)WebRequest.Create(m_sSourceName);
					}
					else
					{
                        req = (HttpWebRequest)WebRequest.Create(m_sSourceName + ((m_sSourceName.IndexOf('?') == -1) ? '?' : '&') + "fake=" + rnd.Next().ToString());
					}
					// set login and password
					if ((login != null) && (password != null) && (login != ""))
						req.Credentials = new NetworkCredential(login, password);
					// set connection group name
					if (useSeparateConnectionGroup)
						req.ConnectionGroupName = GetHashCode().ToString();
					// get response
					resp = req.GetResponse();

					// get response stream
					stream = resp.GetResponseStream();
                    
					// loop
					while (!stopEvent.WaitOne(0, true))
					{
						// check total read
						if (total > bufSize - readSize)
						{
							System.Diagnostics.Debug.WriteLine("flushing");
							total = 0;
						}
                        
						// read next portion from stream
						if ((read = stream.Read(buffer, total, readSize)) == 0)
							break;

						total += read;

						// increment received bytes counter
						bytesReceived += read;
					}

					if (!stopEvent.WaitOne(0, true))
					{
						// increment frames counter
						framesReceived++;

						// image at stop
						if (NewFrame != null)
						{
							Bitmap	bmp = (Bitmap) Bitmap.FromStream(new MemoryStream(buffer, 0, total));
                   
                            //si la taille a change : OnResize passe LeftRightAlignment nouveau Rectangle encombrement AuthenticationManager clipping
                            // on passe le rect 
                            Rectangle ClipRect = new Rectangle();

                            SourceResizeArgs msg = new SourceResizeArgs(bmp.Width, bmp.Height, ClipRect);
                            OnResize(this, ref msg); // init le clipper
                            Rectangle rect = msg.cliprect; 
                             // ici on peut ete amene a recopier la bitmap dans une aurtre bitmap pour le cliippping
                   
                            // pour l'instant on le fait pas

                            // notify client
							NewFrame(this, new CameraEventArgs(bmp));

							// release the image
							bmp.Dispose();
							bmp = null;
						}
					}

					// wait for a while ?
					if (frameInterval > 0)
					{
						// times span
						span = DateTime.Now.Subtract(start);
						// miliseconds to sleep
						int msec = frameInterval - (int) span.TotalMilliseconds;

						while ((msec > 0) && (stopEvent.WaitOne(0, true) == false))
						{
							// sleeping ...
							Thread.Sleep((msec < 100) ? msec : 100);
							msec -= 100;
						}
					}
				}
				catch (WebException ex)
				{
					System.Diagnostics.Debug.WriteLine("=============: " + ex.Message);
					// wait for a while before the next try
					Thread.Sleep(250);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine("=============: " + ex.Message);
				}
				finally
				{
					// abort request
					if (req != null)
					{
						req.Abort();
						req = null;
					}
					// close response stream
					if (stream != null)
					{
						stream.Close();
						stream = null;
					}
					// close response
					if (resp != null)
					{
						resp.Close();
						resp = null;
					}
				}

				// need to stop ?
				if (stopEvent.WaitOne(0, true))
					break;
			}
		}
	}
}

// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//
namespace VideoSource
{
	using System;
	using System.Drawing;
	using System.IO;
	using System.Text;
	using System.Threading;
	using System.Net;
    using System.Windows.Forms;
    using System.Collections;
    using mesure;

	/// <summary>
	/// MJPEGSource - MJPEG stream support
	/// </summary>
	public class MJPEGStream : IVideoSource
	{
        private string m_sSourceName;
		private string	login = null;
		private string	password = null;
		private object	userData = null;
		private int		framesReceived;
		private int		bytesReceived;
		private bool	useSeparateConnectionGroup = true;

		private const int	bufSize = 512 * 1024;	// buffer size
		private const int	readSize = 1024;		// portion size to read

		private Thread	thread = null;
		private ManualResetEvent stopEvent = null;
		private ManualResetEvent reloadEvent = null;
        
		// new frame event
		public event CameraEventHandler NewFrame;
        public event SourceResizeHandler OnResize;
        //public event CameraEventHandler ChgSize;
         
        //positionne un cliprect pour cette source
        // pour le cas ou la source peut changer qqchose si le cliprect change
        public Rectangle ClipRect
        {set 
            {// ne fait rien
            }
        }

        bool IVideoSource.capchglive() { return false; }  // peut etre live ou freeze
        bool IVideoSource.capgrab() { return false; }  // peut saisir une image
        bool IVideoSource.capregl() { return false; }  // peut etre reglé
        bool IVideoSource.isfreeze() { return true; } // est freeze
        bool IVideoSource.freeze() { return false; }
        bool IVideoSource.live() { return false; }
        bool IVideoSource.grab() { return false; }


        public bool refreshimage(ref Bitmap bmp)
        { return false; }
        
        public int GetListSources(ArrayList List)
        { return 0; }


        // ibagxml
        public string[] getTabVers()
        {
            string[] tabvers = { "1.0" };
            return tabvers;
        }
        public string getNomType()
        {
            return "MJPEGStream";
        }


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

            string sourcename="";
            element.GetAttribute("URL", out sourcename);
            m_sSourceName = sourcename;         
            return 0;
        }
        
		/// <summary>
        /// SeparateConnectioGroup property
        /// indicates to open WebRequest in separate connection group		
		/// </summary>
        public bool	SeparateConnectionGroup
		{
			get { return useSeparateConnectionGroup; }
			set { useSeparateConnectionGroup = value; }
		}
		
		/// <summary>
        ///VideoSource property 
		/// </summary>
        public string VideoSource
		{
            get { return m_sSourceName; }
			set
			{
                m_sSourceName = value;
                        
                // signal to reload
				if (thread != null)
					reloadEvent.Set();
			}
		}

		
        /// <summary>
        ///Login property 
        /// </summary>
		public string Login
		{
			get { return login; }
			set { login = value; }
		}
		

        /// <summary>
        /// Password property 
        /// </summary>
		public string Password
		{
			get { return password; }
			set { password = value; }
		}
		
        
        
		/// <summary>
        /// FramesReceived property 
		/// </summary>
        public int FramesReceived
		{
			get
			{
				int frames = framesReceived;
				framesReceived = 0;
				return frames;
			}
		}


		
		/// <summary>
        ///BytesReceived property 
		/// </summary>
        public int BytesReceived
		{
			get
			{
				int bytes = bytesReceived;
				bytesReceived = 0;
				return bytes;
			}
		}
		/// <summary>
        /// UserData property         
        /// </summary>
		public object UserData
		{
			get { return userData; }
			set { userData = value; }
		}
		
        
        /// <summary>
        /// Get state of the video source thread
        /// </summary>
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
		public MJPEGStream()
		{
		}


		// Start work
		public void Start(int interactif)
		{
			if (thread == null)
			{
				framesReceived = 0;
				bytesReceived = 0;

				// create events
				stopEvent	= new ManualResetEvent(false);
				reloadEvent	= new ManualResetEvent(false);
				
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


        public int DoReglage(Control parent)
        { return 0;
        }

        public int DoInit(Control parent)
        { return 0; }
        
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

        ///
        public void Dispose()
        {}


		// Free resources
		private void Free()
		{
			thread = null;

			// release events
			stopEvent.Close();
			stopEvent = null;
			reloadEvent.Close();
			reloadEvent = null;
		}

		// Thread entry point
		public void WorkerThread()
		{bool preframe=true;

			byte[]	buffer = new byte[bufSize];	// buffer to read stream

			while (true)
			{
				// reset reload event
				reloadEvent.Reset();

				HttpWebRequest	req = null;
				WebResponse		resp = null;
				Stream			stream = null;
				byte[]			delimiter = null;
				byte[]			delimiter2 = null;
				byte[]			boundary = null;
				int				boundaryLen, delimiterLen = 0, delimiter2Len = 0;
				int				read, todo = 0, total = 0, pos = 0, align = 1;
				int				start = 0, stop = 0;

				// align
				//  1 = searching for image start
				//  2 = searching for image end
				try
				{
					// create request
                    req = (HttpWebRequest)WebRequest.Create(m_sSourceName);
					// set login and password
					if ((login != null) && (password != null) && (login != ""))
						req.Credentials = new NetworkCredential(login, password);
					// set connection group name
					if (useSeparateConnectionGroup)
						req.ConnectionGroupName = GetHashCode().ToString();
					// get response
					resp = req.GetResponse();

					// check content type
					string ct = resp.ContentType;
					if (ct.IndexOf("multipart/x-mixed-replace") == -1)
						throw new ApplicationException("Invalid URL");

					// get boundary
					ASCIIEncoding encoding = new ASCIIEncoding();
					boundary = encoding.GetBytes(ct.Substring(ct.IndexOf("boundary=", 0) + 9));
					boundaryLen = boundary.Length;

					// get response stream
					stream = resp.GetResponseStream();

					// loop
					while ((!stopEvent.WaitOne(0, true)) && (!reloadEvent.WaitOne(0, true)))
					{
						// check total read
						if (total > bufSize - readSize)
						{
							System.Diagnostics.Debug.WriteLine("flushing");
							total = pos = todo = 0;
						}

						// read next portion from stream
						if ((read = stream.Read(buffer, total, readSize)) == 0)
							throw new ApplicationException();

						total += read;
						todo += read;

						// increment received bytes counter
						bytesReceived += read;
				
						// does we know the delimiter ?
						if (delimiter == null)
						{
							// find boundary
							pos = ByteArrayUtils.Find(buffer, boundary, pos, todo);

							if (pos == -1)
							{
								// was not found
								todo = boundaryLen - 1;
								pos = total - todo;
								continue;
							}

							todo = total - pos;

							if (todo < 2)
								continue;

							// check new line delimiter type
							if (buffer[pos + boundaryLen] == 10)
							{
								delimiterLen = 2;
								delimiter = new byte[2] {10, 10};
								delimiter2Len = 1;
								delimiter2 = new byte[1] {10};
							}
							else
							{
								delimiterLen = 4;
								delimiter = new byte[4] {13, 10, 13, 10};
								delimiter2Len = 2;
								delimiter2 = new byte[2] {13, 10};
							}

							pos += boundaryLen + delimiter2Len;
							todo = total - pos;
						}

						// boundary aligned
						/*						if ((align == 0) && (todo >= boundaryLen))
												{
													if (ByteArrayUtils.Compare(buffer, boundary, pos))
													{
														// boundary located
														align = 1;
														todo -= boundaryLen;
														pos += boundaryLen;
													}
													else
														align = 2;
												}*/

						// search for image
						if (align == 1)
						{
							start = ByteArrayUtils.Find(buffer, delimiter, pos, todo);
							if (start != -1)
							{
								// found delimiter
								start	+= delimiterLen;
								pos		= start;
								todo	= total - pos;
								align	= 2;
							}
							else
							{
								// delimiter not found
								todo	= delimiterLen - 1;
								pos		= total - todo;
							}
						}

						// search for image end
						while ((align == 2) && (todo >= boundaryLen))
						{
							stop = ByteArrayUtils.Find(buffer, boundary, pos, todo);
							if (stop != -1)
							{
								pos		= stop;
								todo	= total - pos;

								// increment frames counter
								framesReceived ++;

								// image at stop
								if (NewFrame != null)
								{
									Bitmap	bmp = (Bitmap) Bitmap.FromStream(new MemoryStream(buffer, start, stop - start));
                                    //si la taille a change : OnResize passe LeftRightAlignment nouveau Rectangle encombrement AuthenticationManager clipping
                                    // on passe le rect 
                                    Rectangle ClipRect = new Rectangle();
                                    
                                    SourceResizeArgs msg = new SourceResizeArgs(bmp.Width, bmp.Height, ClipRect);
                                    if (OnResize != null)
                                        OnResize(this, ref msg); // init le clipper
                                    Rectangle rect = msg.cliprect;
                                    // ici on peut ete amene a recopier la bitmap dans une aurtre bitmap pour le cliippping
                                    // notify client
                                    
									NewFrame(this, new CameraEventArgs(bmp));
									// release the image
									bmp.Dispose();
									bmp = null;
								}
//								System.Diagnostics.Debug.WriteLine("found image end, size = " + (stop - start));

								// shift array
								pos		= stop + boundaryLen;
								todo	= total - pos;
								Array.Copy(buffer, pos, buffer, 0, todo);

								total	= todo;
								pos		= 0;
								align	= 1;
							}
							else
							{
								// delimiter not found
								todo	= boundaryLen - 1;
								pos		= total - todo;
							}
						}
					}
				}
				catch (WebException ex)
				{
					System.Diagnostics.Debug.WriteLine("=============: " + ex.Message);
					// wait for a while before the next try
					Thread.Sleep(250);
				}
				catch (ApplicationException ex)
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

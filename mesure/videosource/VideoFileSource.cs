// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//



using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using mesure;
using mesure.vfw;
using System.Windows.Forms;
using System.Collections;

namespace VideoSource
{
	
	/// <summary>
	/// VideoFileSource
	/// </summary>
	public class VideoFileSource : IVideoSource
	{
		private string	source;
		private object	userData = null;
		private int		framesReceived;

		private Thread	thread = null;
		private ManualResetEvent stopEvent = null;

		// new frame event
		public event CameraEventHandler NewFrame;
        public event SourceResizeHandler OnResize;

        public bool refreshimage(ref Bitmap bmp)
        { return false; }
        
        public Rectangle ClipRect
        {set { }
        }

        bool IVideoSource.capchglive() { return false; }  // peut etre live ou freeze
        bool IVideoSource.capgrab() { return false; }  // peut saisir une image
        bool IVideoSource.capregl() { return false; }  // peut etre reglé
        bool IVideoSource.isfreeze() { return true; } // est freeze
        bool IVideoSource.freeze() { return false; }
        bool IVideoSource.live() { return false; }
        bool IVideoSource.grab() { return false; }


        // ibagxml
        public string[] getTabVers()
        {string[] tabvers = { "1.0" };
         return tabvers;
        }

        public string getNomType()
        {
            return "VideoFileSource"; 
        }



        public int GetListSources(ArrayList List)
        { return 0; }
        
        public int DoReglage(Control parent)
        { return 0;
        }
        
        
        public int DoInit(Control parent)
        { return 0; }
        
		// VideoSource property
		public virtual string VideoSource
		{
			get { return source; }
			set { source = value; }
		}
		// Login property
		public string Login
		{
			get { return null; }
			set { }
		}
		// Password property
		public string Password
		{
			get { return null; }
			set { }
		}

        /// <summary>
        /// enregistre la config sur disque
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int SaveDisk(XMLAvElement element)
        {
            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public int LoadDisk(XMLAvElement element)
        {
            return 0;
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
			get { return 0; }
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
		public VideoFileSource()
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

				// create events
				stopEvent	= new ManualResetEvent(false);
				
				// create and start new thread
				thread = new Thread(new ThreadStart(WorkerThread));
				thread.Name = source;
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
			AVIReader	aviReader = new AVIReader();

			try
			{
				// open file
				aviReader.Open(source);
                
                // init des rectangles
                int width = aviReader.Width;
                int height = aviReader.Height;
                
				while (true)
				{
					// start time
					DateTime	start = DateTime.Now;

					// get next frame

					Bitmap	bmp = aviReader.GetNextFrame();
                    
                    Rectangle ClipRect=new Rectangle();

                    SourceResizeArgs msg = new SourceResizeArgs(bmp.Width, bmp.Height, ClipRect);
                    OnResize(this, ref msg); // init le clipper
                    Rectangle rect = msg.cliprect;

                    // creation de la bitmap de sortie
                    Bitmap ClipBmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppRgb);
                    // on copie la birmap du stream dans la bitmap de sortie


					framesReceived++;

					// need to stop ?
					if (stopEvent.WaitOne(0, false))
						break;

					if (NewFrame != null)
						NewFrame(this, new CameraEventArgs(bmp));

					// free image
					bmp.Dispose();

					// end time
					TimeSpan	span = DateTime.Now.Subtract(start);

					// sleep for a while
/*					int			m = (int) span.TotalMilliseconds;

					if (m < 100)
						Thread.Sleep(100 - m);*/
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("exception : " + ex.Message);
			}

			aviReader.Dispose();
			aviReader = null;
		}
	}
}

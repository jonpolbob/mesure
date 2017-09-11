using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using mesure;

// classe image statique
// renvoie une image comme un dtream
// utilisee comme stream par defaut
/// <summary>
/// IMGSource - JPEG downloader
/// </summary>
namespace VideoSource
{
    public class IMGStream : IVideoSource
    {// new frame event
        int framesReceived = 1; // toujours 1
        int bytesReceived = 1;
        private object userData = null;
        private string source;
        private string login = null;
        private string password = null;
        private Thread thread = null;
        private ManualResetEvent reloadEvent = null; // evenement disant  de recharger la source (changment de fichier)
        private ManualResetEvent stopEvent = null;
        public event CameraEventHandler NewFrame;
        public event SourceResizeHandler OnResize;
        Bitmap m_bmp=null;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="List"></param>
        /// <returns></returns>
        public int GetListSources(ArrayList List)
        { return 0; }


        bool IVideoSource.capchglive()  // peut etre live
        {
            return false;
        }

        bool IVideoSource.capgrab()  // peut etre live
        {
            return false;
        }

        bool IVideoSource.capregl()  // peut etre reglé
        {
            return false;
        }

        bool IVideoSource.isfreeze()
        {
            return true;
        }

        bool IVideoSource.grab()
        { return false;
        }

        bool IVideoSource.live()
        {
            return false;
        }

        bool IVideoSource.freeze()
        {
            return false;
        }


        public bool refreshimage(ref Bitmap bmp)
        {if (m_bmp == null)
            return false;

        bmp = m_bmp;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
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
            return "JPGFixImg";
        }


        public int DoInit(Control parent)
        { return 0; }
        
        public int DoReglage(Control parent)
        { return 0; }
        /// <summary>
        /// 
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
        
        
        /// <summary>
        /// VideoSource property        
        /// </summary>
        public string VideoSource
        {
            get { return source; }
            set
            {
                source = value;
                // signal to reload
                if (thread != null)
                    reloadEvent.Set(); // recharger la source
            }
        }
        
        /// <summary>
        /// Login property
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
                framesReceived = 1;
                return frames;
            }
        }
        
        
        /// <summary>
        /// BytesReceived property
        /// </summary>
        public int BytesReceived
        {
            get
            {
                //int bytes = bytesReceived;
                //bytesReceived = 1;
                return bytesReceived; // reevra la taille de l'image
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



        /// <summary>
        /// Constructor
        /// </summary>
        public IMGStream()
        {
        }

        public void Dispose()
        { }
        
        /// <summary>
        /// Start work
        /// </summary>
        public void Start(int interactif)
        {
            if (thread == null)
            {
                framesReceived = 0;
                bytesReceived = 0;

                // create events
                stopEvent = new ManualResetEvent(false);
                reloadEvent = new ManualResetEvent(false);

                // create events
                stopEvent = new ManualResetEvent(false);
                reloadEvent = new ManualResetEvent(false);

                // create and start new thread
                thread = new Thread(new ThreadStart(WorkerThread));
                thread.Name = source;
                thread.Start();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void Free()
        {
            thread = null;

            // release events
            stopEvent.Close();
            stopEvent = null;
            reloadEvent.Close();
            reloadEvent = null;
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

        /// <summary>
        /// Wait for thread stop 
        /// </summary>
        public void WaitForStop()
        {
            if (thread != null)
            {
                // wait for thread stop
                thread.Join();

                Free();
            }
        }

        /// <summary>
        /// Abort thread
        /// </summary>
        public void Stop()
        {
            if (this.Running)
            {
                thread.Abort();
                WaitForStop();
            }
        }

        /// <summary>
        /// Thread entry point 
        /// </summary>
        public void WorkerThread()
        {
            //req = (HttpWebRequest)WebRequest.Create(source);

            if (source != null && NewFrame != null)
                m_bmp = (Bitmap)Bitmap.FromFile(source);
            else
                m_bmp = new Bitmap(320,240); //.FromStream(new MemoryStream(buffer, start, stop - start));

            //si la taille a change : OnResize passe LeftRightAlignment nouveau Rectangle encombrement AuthenticationManager clipping
            // on passe le rect 
            Rectangle ClipRect = new Rectangle();

            SourceResizeArgs msg = new SourceResizeArgs(m_bmp.Width, m_bmp.Height, ClipRect);
            OnResize(this, ref msg); // init le clipper
            Rectangle rect = msg.cliprect;
            // ici on peut ete amene a recopier la bitmap dans une aurtre bitmap pour le cliippping

            NewFrame(this, new CameraEventArgs(m_bmp));
             /*
            while (true)
            {

             //NewFrame(this, new CameraEventArgs(m_bmp));
             Thread.Sleep(40);  
              
              if (stopEvent.WaitOne(0, true)) // ordre d'arret
                  break;
            }

           m_bmp.Dispose();
           m_bmp = null;
              */
        }
    }
}

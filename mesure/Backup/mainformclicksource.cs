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
using TwainLib;

using System.Windows;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using mesure;
using System.Diagnostics; // pour bitmapinfoheader

namespace mesure
{

    public partial class MainForm : System.Windows.Forms.Form
    {

        /// <summary>
        /// click sur le menu file open : ouvrir une video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpenFilm_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "AVI files (*.avi)|*.avi|MPG files (*.mpg)|*.mpg|MPEG files (*.mpeg)|*.mpeg";
            ofd.Title = "Ouvrir Film";
          
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // create video source
                VideoFileSource fileSource = new VideoFileSource();
                fileSource.VideoSource = ofd.FileName;

                // open it
                OpenVideoSource(fileSource);
                
            }
        }

        
        /// <summary>
        /// action sur menu (il faut que le click soit dans mainform.cs !!!)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSourcURLJPGFile_Action(object sender, System.EventArgs e)
        {
            URLForm form = new URLForm();

            form.Description = "Saisir l'URL d'une image JPEG mise a jour par une caméra IP:";
            form.URLs = new string[]
				{   "http://66.147.53.157/axis-cgi/jpg/image.cgi?image=1",
                    "http://213.221.150.136/axis-cgi/jpg/image.cgi?image=1",
                    "http://194.168.163.96/axis-cgi/jpg/image.cgi?image=1",
                    "http://webkamera.kristinehamn.se/axis-cgi/jpg/image.cgi?image=1",
                    "http://80.34.87.7/axis-cgi/jpg/image.cgi?image=1",
                    "http://80.61.30.131:3000/axis-cgi/jpg/image.cgi?image=1",
                    "http://82.239.14.188/axis-cgi/jpg/image.cgi?image=1",
                    "http://trafficcam10.greensboro-nc.gov//axis-cgi/jpg/image.cgi?image=1",
                    "http://61.220.38.10/axis-cgi/jpg/image.cgi?camera=1",
					"http://212.98.46.120/axis-cgi/jpg/image.cgi?resolution=352x240",
					"http://webcam.mmhk.cz/axis-cgi/jpg/image.cgi?resolution=320x240",
					"http://195.243.185.195/axis-cgi/jpg/image.cgi?camera=1"
				};

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // create video source
                JPEGStream jpegSource = new JPEGStream();
                jpegSource.VideoSource = form.URL;

                // open it
                OpenVideoSource(jpegSource);
                // = localSource;
            }
        }


        /// <summary>
        /// menu source ouverture d'une URL mjpg 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSourceURLMJEPGItem_Action(object sender, System.EventArgs e)
        {
            URLForm form = new URLForm();

            form.Description = "Saisir l'URL d'un flux video MJPEG :";
            form.URLs = new string[]
				{"http://210.236.173.198/mjpg/video.mjpg",
                    "http://148.61.139.229/mjpg/video.mjpg",
                    "http://210.236.173.198/mjpg/video.mjpg",
                    "http://165.91.110.101:2010/mjpg/video.mjpg",
                    "http://64.122.208.241:8000/mjpg/video.mjpg",
                    "http://87.139.23.123/mjpg/video.mjpg",
                    "http://200.61.48.74/mjpg/video.mjpg",
                    "http://www.premium-computer.fr:9006/mjpg/video.mjpg",
                    "http://62.160.44.43:84/mjpg/video.mjpg",
                    "http://85.46.64.146/mjpg/video.mjpg",
                    "http://mandelieu.axiscam.net:8000/mjpg/video.mjpg",
					"http://webcam.salisbury.edu/axis-cgi/mjpg/video.cgi?camera=1&1232573954680http://webcam.salisbury.edu/axis-cgi/mjpg/video.cgi?camera=1&1232573954680",
                    "http://192.171.163.3/axis-cgi/mjpg/video.cgi?camera=&showlength=1&resolution=640x480",
					"http://129.186.47.239/axis-cgi/mjpg/video.cgi?resolution=352x240",
					"http://195.243.185.195/axis-cgi/mjpg/video.cgi?camera=3",
					"http://195.243.185.195/axis-cgi/mjpg/video.cgi?camera=4",
                    "http://chipmunk.uvm.edu/cgi-bin/webcam/nph-update.cgi?dummy=garb"
				};

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // create video source
                MJPEGStream mjpegSource = new MJPEGStream();
                mjpegSource.VideoSource = form.URL;

                // open it
                OpenVideoSource(mjpegSource);
                
            }
        }

        /// <summary>
        /// menu ouverture d'un peripherique local
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSourcePeriph_Action(object sender, System.EventArgs e)
        {

            CaptureDeviceForm form = new CaptureDeviceForm();
            
            CaptureDevice localSource = new CaptureDevice();
            ArrayList ListSource = new ArrayList();

            int nbsources = localSource.GetListSources(ListSource);
            form.Filters = ListSource;

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // create video source
                localSource.VideoSource = form.Selected;

                // open it
                OpenVideoSource(localSource);
                localSource.DoInit(this);
                //ivid = localSource;

         
            }
        }



        /// <summary>
        /// appele par menu open twain
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSourcTwain_Action(object sender, EventArgs e)
        {/*
            if (tw == null)
                tw = new Twain(); // cree le twain
            else
                EndingScan(); // arrete le transfert en cours et demonte la gestion des messages

            // init le systeme twain (si c'esst pas deja fait)
            tw.Init(this.Handle); // init le twain et l'accroche a la fenetre form

            // on cree un dialogue de selsction
            tw.Select();
            //tw.Finish(); // on arrete le twain 
            // ici on testera si on a bien un device valide
            */

            TwainDevice letwain = new TwainDevice(this);

            letwain.Select(this); // definition de la source par defaut

            OpenVideoSource(letwain);
//            ivid = letwain;
            Invalidate();

            //tw.Acquire();
            //on redemarre legend twain
            // pour l'instant on imagine que tout baigne : le handle correct sera renvoye


        }

        
    /// <summary>
    /// menu ouverture source image
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
        private void menuSourcImg_Action(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JPG files (*.jpg)|*.jpg|BMP files (*.bmp)|*.bmp| TIFF files (*.tif)|*.tif";
            ofd.Title = "Ouvrir Image";
          
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // create video source
                IMGStream imgSource = new IMGStream();
                imgSource.VideoSource = ofd.FileName;

                // open it
                OpenVideoSource(imgSource);
                //ivid = localSource;
            }
        }

        /// <summary>
        /// cree une camera l'attache a une source et fait demarrer l'ensemble
        /// </summary>
        /// <param name="source">source a connecter et a lancer</param>
        private void OpenVideoSource(IVideoSource source)
        {
         // set busy cursor
         this.Cursor = Cursors.WaitCursor;

         
         // close previous file
         CloseFile();

         if (source == null)
         {
             CoreSystem.Instance.Camera = null;             
             camwin.Camera = null;
             this.Cursor = Cursors.Default;
             return;
         }


         
         // create camera
         Camera laCamera = new Camera(camwin, source);
         camwin.Camera = laCamera;
            
         // reset statistics
         statIndex = statReady = 0;

         // set event handlers
         laCamera.CamNewFrame += new EventHandler(camera_NewFrame);
         //camera.Alarm += new EventHandler(camera_Alarm);

         // start camera
         laCamera.Start(1);
         Debug.Write("cam start");

         CoreSystem.Instance.Camera = laCamera;

         // start timer
         timer.Start();

         camwin.WinCamPaint();
         camwin.Width -= 10;
         camwin.Width += 10; // pour declencher un repaint
         
         this.Cursor = Cursors.Default;
      //   Invalidate(); // force le redessin pour adapation resolution
         updateetals(); // ca permet un mise a jour des echelle
            EventArgs e = new EventArgs();
            this.OnResetDevice(null, e); 
        }
        
        // charge la source et init la camera


        /// <summary>
        /// charge une config complete a prtir du fichier xml
        /// cree la cam, et la charge (ce qui cree et charge la source)
        /// cree les etalonnages, demarre la source
        /// </summary>
        /// <param name="EtalList">XMl element contenant les parametres de la cam</param>
        /// <returns></returns>
        public int LoadVideoSource(XMLAvElement SourcElem)
        {this.Cursor = Cursors.WaitCursor;

        if (SourcElem == null)  // on gere un argument nul
        {
            camwin.Camera = null; ;
            CoreSystem.Instance.Camera = null;
            this.Cursor = Cursors.Default;

            return 1; // probleme lecture camera
        }

        if (CoreSystem.Instance.Camera != null)
            CoreSystem.Instance.Camera = null;

        Camera lacamera = new Camera(camwin, null);
        camwin.Camera = lacamera;
        if (0 != lacamera.LoadDisk(SourcElem))
            {// probleme lecture de la cam dans le xml : on sort
                camwin.Camera = null;;
                lacamera = null;
                this.Cursor = Cursors.Default;

                return 1; // probleme lecture camera
               }        

        // reset statistics
        statIndex = statReady = 0;

        // set event handlers
        lacamera.CamNewFrame += new EventHandler(camera_NewFrame);
        //camera.Alarm += new EventHandler(camera_Alarm);

        lacamera.Start(0); // demarrage de la source

        CoreSystem.Instance.Camera = lacamera;

        updateetals(); // mise a jour des etalonnages
        timer.Start();

        camwin.Width -= 1; 
        camwin.Width += 1; // pour declencher un repaint

        this.Cursor = Cursors.Default;
        return 0;
    }
    
        /// <summary>
        /// ferme la source et delete la camera
        /// </summary>
        private void CloseFile()
        {
            //Camera camera = cameraWindow.Camera;           
            Camera camera = CoreSystem.Instance.Camera;
            if (camera != null)
            {logger.log("close file camera");
                // detach camera from camera window
                //cameraWindow.Camera = null;
                logger.log("signaltostop");
                // signal camera to stop
                camera.SignalToStop();
                logger.log("camera stop");
                // wait for the camera
               // camera.WaitForStop();
                camera.CamNewFrame -= new EventHandler(camera_NewFrame);             
                logger.log("camera stop ok");
                camera.Dispose();
                logger.log("camera null");
                CoreSystem.Instance.Camera = null;
                //
                GC.Collect();
                logger.log("camera null");
                //
            }
            
            // destruction du writer d'AVI
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
            
        }

        
        /// <summary>
        /// timer event - pas utilise
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Camera camera = CoreSystem.Instance.Camera;

            if (camera != null)
            {
                // get number of frames for the last second
                statCount[statIndex] = camera.FramesReceived;

                // increment indexes
                if (++statIndex >= statLength)
                    statIndex = 0;
                if (statReady < statLength)
                    statReady++;

                float fps = 0;

                // calculate average value
                for (int i = 0; i < statReady; i++)
                {
                    fps += statCount[i];
                }
                fps /= statReady;

                statCount[statIndex] = 0;

                fpsPanel.Text = fps.ToString("F2") + " fps";
            }
        }        
    }
}
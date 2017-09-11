using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using VideoSource;
using System.Windows.Forms;
using TwainLib;
using System.Collections;

namespace mesure
{
    class TwainDevice : IVideoSource, IMessageFilter
    {
        public event CameraEventHandler NewFrame;
        public event SourceResizeHandler OnResize;
        private Bitmap m_bitmap=null;


        bool IVideoSource.capchglive() { return false; }  // peut etre live ou freeze
        bool IVideoSource.capgrab() { return true; }  // peut saisir une image
        bool IVideoSource.capregl() { return false; }  // peut etre reglé
        bool IVideoSource.isfreeze() { return true; } // est freeze
        bool IVideoSource.freeze() { return false; }
        bool IVideoSource.live() { return false; }
        bool IVideoSource.grab()
        {
            Start(1);
            return false; 
        }



        // ibagxml
        public string[] getTabVers()
        {
            string[] tabvers = { "1.0" };
            return tabvers;
        }
        public string getNomType()
        {
            return "TwainSource";
        }
        public bool refreshimage(ref Bitmap bmp)
        {bmp = m_bitmap;
        return (m_bitmap != null);
        }
        
        public Rectangle ClipRect
        {set { }
        }

        Twain tw;
        private bool msgfilter = false;

        //
        public TwainDevice(Form Parent)
        {// constructeur : on se met en mode 4
         // pour pouvoir soit acceder aux reglages, a la source ou a l'acquisition
            tw = new Twain();
            tw.Init(Parent.Handle); // il faudra passer la fenetre ppale pour le init
        }

        public void Dispose()
        {tw.unInit(); // desinstallation de twain
        }

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
        

        public int GetListSources(ArrayList List)
        { return 0; }
        
        ~TwainDevice()
        {
            if (!tw.isinacq()) // c'est deja arrete
                return;

            // pas besoin de passer par la boucle de messages : on envoie direct l'orde de stopacquire
            tw.StopAcquire(); // CloseDial(); // on arrete l'acquisition et le DS            

            // on attend que ca soit arrete
            while (tw.isinacq())
                Application.DoEvents();

         tw.unInit(); // desinstallation de twain
        }

        public string Login { get { return "toto"; } set { } }
        public string Password { get { return "toto"; } set { } }
        public int FramesReceived { get { return 1; } }
        public int BytesReceived { get { return 1; } }
        public object UserData
        {
            get
            {
                object toto = null;
                            return toto;}
            set{}
        }

        //-------------------------------------------
        //-------------------------------------------
        public bool Running
        { 
            get { return tw.isinacq(); } 
        }



        //-------------------------------------------
        // ouvrel a boite de dialogue de selection de source
        //-------------------------------------------
        public int Select(Control e)
        { return tw.Select();            
        }

        //-------------------------------------------
        // demarrage de l'acquisition
        //-------------------------------------------
        public void Start(int interactif)
        {// on installe le gestionnaire de messages
         if (!msgfilter)
            {
                //this.Enabled = false; // interdit de repondre aux interactions de l'utilisateur
                msgfilter = true;
                Application.AddMessageFilter(this);
            }

         //on lance le start
         tw.StartAcquire(); // demrrage de l'acquisition
        }
        
        
        //-----------------------------------------
        //
        //-----------------------------------------
        public void Stop()
        {
           //IL faut poster un stoprequest
            // ca cree un stopacquire
            //tw.StopAcquire(); // arret de l'acquisition
            
            if (!tw.isinacq ()) // c'est deja arrete
                return; 

            // pas besoin de passer par la boucle de messages : on envoie direct l'orde de stopacquire
            tw.StopAcquire(); // CloseDial(); // on arrete l'acquisition et le DS            
            
            // on attend que ca soit arrete
          //  while (tw.isinacq())
            //    Application.DoEvents();
         
            // on desinstalle la boucle de messages
            if (msgfilter)
            {
                //this.Enabled = false; // interdit de repondre aux interactions de l'utilisateur
                msgfilter = false;
                Application.RemoveMessageFilter(this);
            }

        }

        //-------------------------------------
        //
        //-------------------------------------

        public string VideoSource {
            get { return GetVideoSource();
            } 
        set{} 
        }



        //----------------------------------------------------
        // ouvre la boite de dialogue de selection de source 
        // il faut etre en mode 1 : on y est car le constructeur le fait
        //----------------------------------------------------
        string GetVideoSource()
        {
            tw.Select(); // ouverture du dialogue de selection
            return "ok";
		 }

		/// <summary>
		/// Stop receiving video frames
		/// </summary>
        public void SignalToStop()
        {}



        //---------------------------------
        //
        //---------------------------------
        public int DoReglage(Control parent)
        {
            return 0;
        }

        public int DoInit(Control parent)
        { return 0; }
        

        //---------------------------------
        //
        //---------------------------------
        public void WaitForStop()
        {
            // boucle des essages en attendant que ca s'arrete
         
        }


        //--------------------------------------------------------------------------------------------------------
        // traitement de la boucle de messages de la mainfoorm transmis a la camera qui le transmet a videosource
        //----------------------------------------------------------------------------------------------------------
        

        // fonction de traitement des messages de cette fenetre
        // renvoie false si message pas traite
        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
        TwainCommand cmd = tw.PassMessage(ref m); // decode m pour savoir si c'est un message twain
            if (cmd == TwainCommand.Not)
                return false; // false : ce prefilter ne traite pas ce message

            // sinon : ce prefilter traite ce message
            switch (cmd)
            {// la source demande a fermer son interface utilisateur
                //case TwainCommand.Null:
                  //  return true;

                case TwainCommand.CloseRequest:
                    {
                        //stopEvent.Reset(); // orede au thraed de s'arreter
                        Stop();
                        //EndingScan(); // ne pas appeler ca ca arrete l'acquisitoin
                        //normalement ici on fait un stopacquire pour tout arreter
                        // mais nous on contniue d'ailleurs la fenetre s'ouvre meme pas
                        // on fera le stopacquire a la demande des menus (demande de nouvelle source
                        // cad dans le destructuer de twaindevice
                         
                        tw.StopAcquire(); // CloseDial(); // on arrete l'acquisition et le DS
                        break;
                    }
                
                // l'utilisateur a clique sur ok et ferme le UI de reglage
                case TwainCommand.CloseOk:
                    {
                        Stop();
                        //EndingScan();
                        tw.StopAcquire();   // CloseDial(); // on arrete l'acquisition et le DS
                        break;
                    }
                case TwainCommand.DeviceEvent:
                    {
                        break;
                    }


                //commande twain : un transfert est pret dans a memeoire
                case TwainCommand.TransferReady:
                    {// transfert des images dans la bitmap a peindre
                        int erreur=0;
                        ArrayList pics = tw.TransferPictures(ref erreur);
                        if (erreur != 0) // ya un os
                            break;

                        //EndingScan();
                       // tw.CloseSrc();
                        //picnumber++;
                        int i = 0;
                        if (pics.Count != 0)
                        //for (int i = 0; i < pics.Count; i++)
                        {
                             Bitmap twimage = (Bitmap)pics[i];
                            /*if (m_lstimage)
                                m_lstimage.Dispose();

                             * 
                            m_lstimage */
                            //Bitmap image = (Bitmap)Bitmap.FromFile("c:\\tmp\\parismatch.jpg");
                             if (m_bitmap != null)
                             { m_bitmap.Dispose();
                             m_bitmap = null;
                             }
                             m_bitmap = (Bitmap)twimage.Clone();

                            // on envoie ca sous forme d'evenement a la cam
                            //NewFrame(this, new CameraEventArgs((Bitmap)m_lstimage.Clone()));
                             NewFrame(this, new CameraEventArgs(m_bitmap));
                            
                            /*PicForm newpic = new PicForm(img);
                            newpic.MdiParent = this;
                            int picnum = i + 1;
                          //  newpic.Text = "ScanPass" + picnumber.ToString() + "_Pic" + picnum.ToString();
                            newpic.Show();
                             */

                        }
                        for (i = 0; i < pics.Count; i++)
                        {
                            Bitmap twimage = (Bitmap)pics[i];
                            twimage.Dispose();
                        }
                        
                        Stop();
                        tw.StopAcquire(); // CloseDial(); // on arrete l'acquisition et le DS
                        
                        break;
                        
                   
                    }
            }

            return true;
        }
    }
        
		}


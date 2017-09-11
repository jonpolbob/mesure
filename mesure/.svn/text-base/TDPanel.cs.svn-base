using System;
using System.Xml.Serialization;
using System.Collections;
using System.Drawing;
using DirectX = Microsoft.DirectX;
using Direct3D = Microsoft.DirectX.Direct3D;

using System.Runtime.InteropServices;
using Microsoft.DirectX.PrivateImplementationDetails;
using System.Diagnostics;
using Microsoft.DirectX.Direct3D;
//using CLRClassImgUtil;
using mesure;
//using CLRClassImgUtil;
using System.Windows.Forms;
using clsimgutils;
using Microsoft.DirectX;
using System.Drawing.Imaging;// pour import dll


namespace mesure
{
	/// <summary>
	/// Class definition of the TDPanel
	/// </summary>	
    public class TDPanel
    {
        
        //the fields of the class		
        private Direct3D.Device tdDevice = null;
        private Direct3D.VertexBuffer panelVertexBuffer = null;
        private Direct3D.Texture panelTexture = null;
        private Direct3D.Texture panelTextureOdd = null;
        private Direct3D.Texture panelTextureEven = null;
        //private CImgUtility util = new CImgUtility();
        private Bitmap m_SurfAlpha = null; // surface contenant le plan graphique tel qu'il est

        bool m_graphmodif = false;
        
        //bool curtextureOdd = true;
        private float xOffset = 0.0f;
        private float yOffset = 0.0f;
        private float zOffset = 0.0f;
        private int rotateDimension = 0;
        private Microsoft.DirectX.Matrix localMatrix;

        private int m_srcWidth;
        private int m_srcHeight;
        private int m_factwidth = 1;
        private int m_factheight = 1;

        private bool video = false;
        
        ImgUtil imgutil;

        // pour le flip flop des buffers
        Object vectorbuffer = new Object();
        
        int tochangetexturetodraw = 0; // dit qu'il faut changer de buffer de transfert du draw
        int tochangetexturetopresent = 0; // dit qu'il faut changer de buffer de present
        Bitmap drawbitmap=null;
        
        Random tot;

        bool curpresenttextureOdd = true; // le presenter se fait actuellement dans le buffer Odd
        bool curdrawtextureOdd = true;

        /// <summary>
        /// TDPanel Constructor
        /// </summary>	
        public TDPanel(bool video)
        {
            
            imgutil = new ImgUtil();
            tot = new Random();
            this.video = video;
            //initialize the local matrix to Identity
            this.localMatrix = Microsoft.DirectX.Matrix.Identity;
            
        }


        #region ------- region des get et set ------------
        /// <summary>
        /// 
        /// </summary>
        public bool IsVideo
        {
            get
            {
                return video;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Direct3D.Device TDDevice
        {
            set { this.tdDevice = value; }
            get { return this.tdDevice; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Direct3D.VertexBuffer PanelVertexBuffer
        {
            set {panelVertexBuffer = value; 
                }

            get {
                 return this.panelVertexBuffer;                  
                }

        }

        /// <summary>
        /// modif : on swap la texture sur une nouvelle texture juste faite 
        /// </summary>
        public Direct3D.Texture PanelTexture
        {
            set
            {
                this.panelTexture = value;                
            }
            get
            {
                //affiche();
                return this.panelTexture;                
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Microsoft.DirectX.Matrix LocalMatrix
        {
            set { this.localMatrix = value; }
            get { return this.localMatrix; }
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        public float XOffset
        {
            set { this.xOffset = value; }
            get { return this.xOffset; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public float YOffset
        {
            set { this.yOffset = value; }
            get { return this.yOffset; }
        }
        /// <summary>
        /// 
        /// </summary>
        public float ZOffset
        {
            set { this.zOffset = value; }
            get { return this.zOffset; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RotateDimension
        {
            set { this.rotateDimension = value; }
            get { return this.rotateDimension; }
        }
        
        #endregion

        //events
        
        //-------------------------------------------------------------------------
        /// <summary>
        /// Create the actual vertext buffer
        /// In this case, all vertext buffers will be square (two triangles)
        /// and the texture will be streched onto this
        /// </summary>	
        /// //-------------------------------------------------------------------------
        
        
        /// <summary>
        /// dessine la bitmap sur ce panel si c'est un panel video
        /// marche comme pour le draw mais on prend simplemtn la bitmap de la video 
        ///  et on la compie dans le buffer de la texture disponible pour ecriture
        /// </summary>
        /// <param name="vBitmap"> bitmap a envoyer dans le plan video</param>
        /// <returns>retourn 0 si une erreur</returns>
        public int PaintFrame(Bitmap vBitmap)
        {
            Direct3D.Texture latexture;
            int pitch;

            if (!video)
                return 0;


            //{
            //    Graphics gr = Graphics.FromImage(vBitmap);
              //  gr.FillRectangle(new SolidBrush(Color.Blue), 0, 0, vBitmap.Width, vBitmap.Height);
              //  gr.Dispose();
            //}

            lock (vectorbuffer) //vectorvbuffer lock les switch decimal Buffer video
            {
               /* switch (tochangetexturetodraw) // on a une nouvelle texture pour dessiner
                {
                    case 0: // pas de changement de texture : on redessine sur le meme
                        break;

                    case 1: // on bascule sur la texture odd
                        curdrawtextureOdd = true;
                        break;

                    case 2: // on bascule sur la texture even
                        curdrawtextureOdd = false;
                        break;
                }


                tochangetexturetodraw = 0; // la bascule est faite                               
                */

                
                // on copie les bits de la draw dans la bitpmap pas en render
                /*if (curdrawtextureOdd) // la odd est en render
                {
                    latexture = panelTextureOdd;
                }
                else
                {
                    latexture = panelTextureEven;
                }*/
                latexture = panelTextureOdd;
                

                if (latexture == null)
                {
                    //Debug.Write("[TXTNUL]\r\n");

                    return 0;
                }

            }
            

            try
            {
            //Surface surface = latexture.GetSurfaceLevel(0);
              /*  
            Graphics gr = surface.GetGraphics(); // possible car on est x8r8g8b8 pas d'alpha
            //gr.DrawImage(vBitmap,new Point(0,0)); // ca marche en video acr on est en format x8r8g8v8
            // utilise cet over
            // on ultra simplifie la copie pour aller + vite : ca change rien...
           // ca rame amort avec une grande imae
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low; // or NearestNeighbour
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            gr.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
                
            Rectangle srcrect = new Rectangle(0,0,vBitmap.Width,vBitmap.Height);
            // eventuellement on reduit la destination dans la texture pour le zoom entier
            Rectangle dstrect = new Rectangle(0, 0, vBitmap.Width / m_factwidth, vBitmap.Height / m_factheight);
            gr.DrawImage(vBitmap, dstrect, srcrect, GraphicsUnit.Pixel);
            surface.ReleaseGraphics();
            gr.Dispose();
            surface.Dispose();

            */
                Microsoft.DirectX.GraphicsStream a = latexture.LockRectangle(0, Microsoft.DirectX.Direct3D.LockFlags.None, out pitch);
                System.Drawing.Imaging.BitmapData bd = vBitmap.LockBits(new System.Drawing.Rectangle(0, 0, vBitmap.Width, vBitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            if (m_factheight != 1 || m_factwidth != 1)
            imgutil.copy32imgincxy(a,bd.Scan0,0,0,(uint)bd.Height,(uint)pitch,(uint)bd.Stride,(uint)bd.Width,m_factwidth,m_factheight);   
                else
            imgutil.copy32img(a,bd.Scan0,0,0,(uint)bd.Height,(uint)pitch,(uint)bd.Stride,(uint)bd.Width);
          
                latexture.UnlockRectangle(0);
                vBitmap.UnlockBits(bd);

                // la nouvelle texture est faite : 
                // signale au present de changer de texture la prochaine fois
                // ca va pas : on risque d'avoir e present qui farfouille dans la texture pendant la opie // il faut swapper les buffers endsembs
                lock (vectorbuffer)
                {
                    if (curdrawtextureOdd) // on a peint dans la odd
                    {
                        tochangetexturetopresent = 2; // on devra presenter la odd
                        //Debug.Write("TOprsODD");
                    }
                    else
                    {
                        tochangetexturetopresent = 1; // on devra presenter la even
                        //Debug.Write("TOprsEVN");
                    }
                 
                }

            }
            catch { 
                Debug.Write("err"); 
            }
            return 0;
        }

        /// <summary>
        /// fonction mettant a jour la texture graphique a partir de la bitmap de dessin en memoire
        /// appelée au moment du render pour eviter de manger le cpu avec des copies jamais affichées
        /// et en plus n'est appelée que si qqchose a ete change dans le graphisme (par un getgraphics/release graphics)
        /// c'est ptet pas tres thread safe tout ca
        /// </summary>
        public void updatetexture()
        {
            if (this.m_graphmodif) // le graph a ete modifie
            {// on recupere la surface
                
                int pitch;
                Microsoft.DirectX.GraphicsStream a = panelTexture.LockRectangle(0, Microsoft.DirectX.Direct3D.LockFlags.DoNotWait, out pitch);
                System.Drawing.Imaging.BitmapData bd = m_SurfAlpha.LockBits(new System.Drawing.Rectangle(0, 0, m_SurfAlpha.Width, m_SurfAlpha.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                imgutil.copy32img(a, bd.Scan0,0,0, (uint)bd.Height, (uint)pitch, (uint)bd.Stride, (uint)bd.Width);
                m_SurfAlpha.UnlockBits(bd);
                panelTexture.UnlockRectangle(0);
                m_graphmodif = false;
               }
        
        }


        /// <summary>
        /// cree et libere un graphic sur le plan graphique de ce panel
        /// sasn effacer le plan
        /// (si c'est un panel graphique)
        /// </summary>
        /// <returns></returns>
        public Graphics GetGraph()
        {
            return GetGraph2(false);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="erase"></param>
        /// <returns></returns>
        public Graphics GetGraph(bool erase)
        { return GetGraph2(erase);
        }



        //Surface m_surface = null;
        //Graphics legraphic = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="erase"></param>
        /// <returns></returns>
        public Graphics GetGraph2(bool erase)
        {
            if (video)
                return null;

           //m_surface = panelTexture.GetSurfaceLevel(0);
            
            Graphics legraphic = Graphics.FromImage(m_SurfAlpha);
            
            legraphic.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
               
            if (erase)
            {
                legraphic.Clear(Color.FromArgb(255, Color.Firebrick)); // on se fout de la couleur seul le alpha compte
                  
            }

            return legraphic;
        }

        
        /// <summary>
        /// cree et libere un graphic sur le plan graphique de ce panel
        /// efface le plan
        /// </summary>
        /// <param name="erase"></param>
        /// <returns></returns>
        public Graphics GetGraph1(bool erase)
        {
            if (video)
                return null;

            if (drawbitmap == null)
            {
                Debug.Write("gbitmap null");
                return null;
            }
            else
            {
                Graphics gr = Graphics.FromImage(drawbitmap);
                if (erase)

                    gr.FillRectangle(new SolidBrush(Color.FromArgb(255, Color.Magenta)), 0, 0, drawbitmap.Width, drawbitmap.Height);

                return gr;
            }
        }

        /// <summary>
        /// dispose le graphe et envoie la bitmap dans le plan graphique 
        /// </summary>
        /// <param name="graph"></param>
       
       public void Releasegraph(Graphics graph)
       {Releasegraph2(graph) ; // avec drawbitmap
       }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
       public void Releasegraph2(Graphics graph)
       {
           if (video)
               return;

           graph.Dispose();
           m_graphmodif = true; // au prochain render il y aura copie de la texture
       }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        public void Releasegraph1(Graphics graph)
        {
            if (video)
                return;

            graph.Dispose();

            lock (vectorbuffer)
            {
                /*switch (tochangetexturetodraw) // on a une nouvelle texture pour dessiner
                {
                    case 0: // pas de changement de texture : on redessine sur le meme
                        break;

                    case 1: // on bascule sur la texture even
                        //Debug.Write("[SetEvnDRW]");
                        curdrawtextureOdd = false;
                        break;

                    case 2: // on bascule sur la texture odd
                        //Debug.Write("[SetOddDRW]");
                        curdrawtextureOdd = true;
                        break;
                }
                 *

                tochangetexturetodraw = 0; // la bascule est faite                
                 * */
            }

            Direct3D.Texture latexture;

            // on copie les bits de la draw dans la bitpmap pas en render
            //if (curdrawtextureOdd) // la odd est en render
                latexture = panelTextureOdd;
            //else
            //    latexture = panelTextureEven;

            int pitch;
            Microsoft.DirectX.GraphicsStream a = latexture.LockRectangle(0, Microsoft.DirectX.Direct3D.LockFlags.None, out pitch);
            System.Drawing.Imaging.BitmapData bd = drawbitmap.LockBits(new System.Drawing.Rectangle(0, 0, drawbitmap.Width, drawbitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            imgutil.copy32img(a, bd.Scan0, 0,0,(uint)bd.Height, (uint)pitch, (uint)bd.Stride, (uint)bd.Width);
            //imgutil.copy32ovl(a, bd.Scan0, (uint)bd.Height, (uint)pitch, (uint)bd.Stride, (uint)bd.Width);
            /*
            unsafe
            {
                byte* to = (byte*)a.InternalDataPointer;
                byte* from = (byte*)bd.Scan0.ToPointer();
                
                uint color = (((uint)255) * (uint)256 * (uint)256 + (uint)255);
                for (int y = 0; y < bd.Height; ++y)
                {
                    
                    //int retour = util.cpy32lovl((int*)(&to[pitch * y]), (&from[y * bd.Stride]), (uint)bd.Width * 4, color);
                    int retour = util.copy32ligneovl((int *)(&to[pitch * y]), (int *)(&from[y * bd.Stride]), (uint)bd.Width * 4, (int)color);
                
                    // copie avec overlay fait par directx
                    //int retour = copy32ligne((int*)(&to[pitch * y]), (int*)(&from[y * bd.Stride]), (uint)bd.Width * 4);                    
                    int toto = retour;
                }

             }*/
            
            latexture.UnlockRectangle(0);
            drawbitmap.UnlockBits(bd);

            // la nouvelle texture est faite : 
            // signale au present de changer de texture la prochaine fois
            // ca va pas : on risque d'avoir e present qui farfouille dans la texture pendant la opie // il faut swapper les buffers endsembs
            /*lock (vectorbuffer)
            {
                if (curdrawtextureOdd) // on a peint dans la odd
                {
                    //Debug.Write("[ToOddPRS]");
                    tochangetexturetopresent = 2; // on devra presenter la odd
                }
                else
                {
                    //Debug.Write("[ToEvnPRS]");
                    tochangetexturetopresent = 1; // on devra presenter la even
                }
            }*/
        }

        /// <summary>
        /// libere les ressources de vertex buffer
        /// </summary>
        public void FreeResourcesVertexBuffer()
        { if (panelVertexBuffer != null)
                {panelVertexBuffer.Created -= new System.EventHandler(this.OnCreateVertexBuffer);
                 panelVertexBuffer.Dispose();
                }

            if (this.panelTextureOdd != null)
                this.panelTextureOdd.Dispose();

            if (this.panelTextureEven != null)
                this.panelTextureEven.Dispose();

            if (m_SurfAlpha != null)
                m_SurfAlpha.Dispose();

            }
        
        /// <summary>
        /// cre les vertex de la texture video        
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CreateVertexBuffer(int width, int height)
        {

            FreeResourcesVertexBuffer();
            m_srcWidth = width;
            m_srcHeight = height;
            
            // image trop grande pour lat texture : on divise la resolution par 2
            m_factwidth =1;
            m_factheight = 1;

            while (m_srcWidth > 2048)
                {
                m_factwidth ++;
                m_srcWidth = width / m_factwidth;
                }
            while (m_srcHeight > 2048)
            {
                m_factheight ++;
                m_srcHeight = height / m_factheight;
            }


            m_SurfAlpha = null;
                
            if (this.video)
                {panelTextureOdd = new Direct3D.Texture(TDDevice, 2048, 2048, 1, Usage.None, Format.X8R8G8B8, Pool.Managed);
                 panelTextureEven = new Direct3D.Texture(TDDevice, 2048, 2048, 1, Usage.None, Format.X8R8G8B8, Pool.Managed);
                }
            else
            {// on cree toujours une texture
                panelTextureOdd = new Direct3D.Texture(TDDevice, 2048, 2048, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
                panelTextureEven = new Direct3D.Texture(TDDevice, 2048, 2048, 1, Direct3D.Usage.None, Direct3D.Format.A8R8G8B8, Direct3D.Pool.Managed);
                // OnCreateVertexBuffer cree une bitmap qui servira a la peinture
                if (width != 0 && height!= 0 )
                    m_SurfAlpha = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);                            
            }
            
            panelTexture = panelTextureOdd;

            curdrawtextureOdd = false; // la texture actuellement utilisee par le render est la odd
            curpresenttextureOdd = true;

            // au lieu de charger une texture on en cree 2 qui seront swappees
            panelVertexBuffer = new Direct3D.VertexBuffer(
                                                 typeof(Direct3D.CustomVertex.PositionTextured),
                                                 4,
                                                 this.tdDevice,
                                                 0,
                                                 Direct3D.CustomVertex.PositionTextured.Format,
                                                 Direct3D.Pool.Default);

            panelVertexBuffer.Created += new System.EventHandler(this.OnCreateVertexBuffer);

            this.OnCreateVertexBuffer(panelVertexBuffer, null);
        }

        /// <summary>
        /// en reponse a l'event createvertexbuffer : creation vertex projection plan video
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnCreateVertexBuffer(object sender, EventArgs e)
        {
             
            Direct3D.VertexBuffer vb = (Direct3D.VertexBuffer)sender;

            //the vertex buffer consists of 2 triangle strips that form a square plane
            //onto which the texture is mapped
            float width = (float)m_srcWidth / 2048.0f;
            float height = (float)m_srcHeight / 2048.0f;

            Direct3D.CustomVertex.PositionTextured[] verts = (Direct3D.CustomVertex.PositionTextured[])vb.Lock(0, 0);
            
            verts[0].X = 1.0f; verts[0].Y = 1.0f; verts[0].Z = 0.0f; verts[0].Tu = width; verts[0].Tv = 0.0f;
            verts[1].X = -1.0f; verts[1].Y = 1.0f; verts[1].Z = 0.0f; verts[1].Tu = 0.0f; verts[1].Tv = 0.0f;
            verts[2].X = 1.0f; verts[2].Y = -1.0f; verts[2].Z = 0.0f; verts[2].Tu = width; verts[2].Tv = height;
            verts[3].X = -1.0f; verts[3].Y = -1.0f; verts[3].Z = 0.0f; verts[3].Tu = 0.0f; verts[3].Tv = height;
            
            vb.Unlock();           
        }


        /// <summary>
        /// prepare le panel pour qu'il soit pret a affciher sa texture
        /// a l abase ca servait a switcher les textures mais ca ne semble plus necessaire depuis qu'on fricotte avec les surfaces direct3D
        /// </summary>
        public void preptexture()
        {   
            int todrawtexture = 0;
            /*
            lock (vectorbuffer)
            {
                switch (tochangetexturetopresent)
                {
                    case 0:
                        break;

                    case 2: // il faut presenter la odd
                        //Debug.Write("[OddPRS]");
                        curpresenttextureOdd = true;
                        //Debug.Write("[ToEvnDRW]");
                        
                        todrawtexture = 2; // dessiner dns le even
                        break;

                    case 1: //il faut presenter la even
                        //Debug.Write("[EvnPRS]");
                        curpresenttextureOdd = false;
                        todrawtexture = 1; // dessiner dans le odd
                        //Debug.Write("[ToOddDRW]");
                        break;
                }
                tochangetexturetopresent = 0; // raz du flag
                tochangetexturetodraw = todrawtexture; // signal au draw de changer de texture si on a swappe les present
            }
            */
            //if (curpresenttextureOdd) // la odd est utilisse en present
            //{
                this.panelTexture = panelTextureOdd; // peindre dans la even
            //}
            //else
            //{
                //this.panelTexture = panelTextureEven;
            //}

        }
    }
}
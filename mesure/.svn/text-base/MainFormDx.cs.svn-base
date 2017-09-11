

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
//using TwainLib;

using System.Windows;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using mesure;
using System.Diagnostics;
using Microsoft.DirectX.Direct3D;


namespace mesure
{


    // strucures pour connection api windows
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER
    {
        public UInt32 biSize;
        public Int32 biWidth;
        public Int32 biHeight;
        public Int16 biPlanes;
        public Int16 biBitCount;
        public UInt32 biCompression;
        public UInt32 biSizeImage;
        public Int32 biXPelsPerMeter;
        public Int32 biYPelsPerMeter;
        public UInt32 biClrUsed;
        public UInt32 biClrImportant;
    }

    //----------------------------


    /// <summary>
    /// Summary description for MainForm
    /// </summary>
    /// on implement imessagefilter a cause des messages du twain
    public partial class  MainForm : System.Windows.Forms.Form
    {

        const int D3DMCMP_NOTEQUAL=6;
        const int D3DMCMP_LESS = 2;

 
        /// <summary>
        /// D3D rendering
        /// </summary>
        private void Render()
        {Render2();
        }

        #region render1
        private void Render1()
        {
            if (this.device == null)
                return;

            //Clear the backbuffer to a blue color 
            this.device.Clear(Direct3D.ClearFlags.Target, Color.FromArgb(255,Color.Gray), 1.0f, 0);

            //Begin the scene
            this.device.BeginScene();
            
            // ici on dessine les choses dans le back buffe
            // on utiise pour ca une matrix stack
            //The creates the position dependencies between each panel
            Microsoft.DirectX.MatrixStack matrixStack = new Microsoft.DirectX.MatrixStack();

            // loop through the entire list of panels
            // Set up the correct Alpha Channel 		
            int nCount = 0;
            foreach (TDPanel tdPanel in panelList)
            {
               
               
                //set up the correct Texture attributes for 
                //correct handling of the alpha channel textures
                tdPanel.preptexture();
                this.device.SetTexture(0, tdPanel.PanelTexture);
                device.SetRenderState(RenderStates.ZEnable, true);
                
              //  device.SetRenderState(RenderStates.AlphaTestEnable,true);
               // device.SetRenderState(RenderStates.AlphaFunction,6); //5=D3DCMP_GREATER);
               // device.SetRenderState(RenderStates.ReferenceAlpha, 255);
                
                //m_Device->SetTextureStageState(0, D3DTSS_ALPHAOP, D3DTOP_SELECTARG1);
                //m_Device->SetTextureStageState(0, D3DTSS_ALPHAARG1, D3DTA_TEXTURE);

                if (tdPanel.IsVideo)
                {
                    this.device.SamplerState[0].MagFilter = Microsoft.DirectX.Direct3D.TextureFilter.GaussianQuad;
                    this.device.SamplerState[0].MinFilter = Microsoft.DirectX.Direct3D.TextureFilter.GaussianQuad;
                    this.device.SamplerState[0].MipFilter = Microsoft.DirectX.Direct3D.TextureFilter.GaussianQuad;

                    //this.device.TextureState[0].AlphaOperation = Direct3D.TextureOperation.SelectArg1;
                    //this.device.TextureState[0].AlphaArgument1 = Direct3D.TextureArgument.TextureColor;

                    this.device.VertexFormat = Direct3D.CustomVertex.PositionTextured.Format;
                    this.device.SetStreamSource(0, tdPanel.PanelVertexBuffer, 0);

                }
                else
                {
                    this.device.SamplerState[0].MagFilter = Microsoft.DirectX.Direct3D.TextureFilter.Point; // point pour voir si ca permet d'afficher meme en tres reduit
                    this.device.SamplerState[0].MinFilter = Microsoft.DirectX.Direct3D.TextureFilter.Point; // sinon none serait suffisant
                    this.device.SamplerState[0].MipFilter = Microsoft.DirectX.Direct3D.TextureFilter.Point;
                    //this.device.TextureState[0].AlphaOperation = Direct3D.TextureOperation.SelectArg1;
                    //this.device.TextureState[0].AlphaArgument1 = Direct3D.TextureArgument.TextureColor;

                    this.device.VertexFormat = Direct3D.CustomVertex.PositionTextured.Format;
                    this.device.SetStreamSource(0, tdPanel.PanelVertexBuffer, 0);

                }

                
                //push the matrix for this panel onto the stack
                matrixStack.Push();

                //Rotate this bitmap
               // if (panelList[2].Equals(tdPanel))
                 //   tdPanel.RotateMatrixTimer(); // on s'en fout c'est pour la demo

                if (panelList[1].Equals(tdPanel))
                {
                    Graphics gr = tdPanel.GetGraph();
                    Brush labrush = new SolidBrush(Color.FromArgb(128, Color.ForestGreen));
                    gr.FillRectangle(labrush, 10, 10, 100, 100);
                    tdPanel.Releasegraph(gr);
                    
                    }
                //compute this bitmaps Local Matrix on the stack, and draw it
                matrixStack.MultiplyMatrixLocal(tdPanel.LocalMatrix);
                this.device.Transform.World = matrixStack.Top;
                //Each panel consists of two triangles 
                this.device.DrawPrimitives(Direct3D.PrimitiveType.TriangleStrip, 0, 2);

                nCount++;
            }
            
            // fin du dessin dans le backbuffer
            //End the scene
            this.device.EndScene();
            
            // on envoie a l'ecran
            //this.device.Present();
        }

        #endregion

        private void Render2()
        {
            if (this.device == null)
                return;

           //Clear the backbuffer to a blue color 
           // this.device.Clear(Direct3D.ClearFlags.Target, Color.FromArgb(255,Color.Gray), 1.0f, 0);

          
            //Enable alpha blending
            //device.SetRenderState(RenderStates.AlphaBlendEnable,true); // autorise le alpha blending
            device.VertexFormat = Direct3D.CustomVertex.PositionTextured.Format;
            
            // tout ca c'est bon pour voir la video, il reste le probleme 
            // de l'affichage du trait quand on repasse sur du transparent
            device.RenderState.ZBufferEnable = false; // indispensable pour le alpha blending ? et alpha compare ?
            device.RenderState.AlphaBlendEnable = true;
            device.RenderState.SourceBlend = Blend.SourceAlpha;
            device.RenderState.DestinationBlend = Blend.InvSourceAlpha;              
            
            //Begin the scene
            this.device.BeginScene();
            
            // ici on dessine les choses dans le back buffe
            // on utiise pour ca une matrix stack
            //The creates the position dependencies between each panel
            //Microsoft.DirectX.MatrixStack matrixStack = new Microsoft.DirectX.MatrixStack();

            // loop through the entire list of panels
            // Set up the correct Alpha Channel 		
            
            // definition des operations sur les etages de texture
            device.SamplerState[0].MagFilter = Microsoft.DirectX.Direct3D.TextureFilter.Point; // point pour voir si ca permet d'afficher meme en tres reduit
            device.SamplerState[0].MinFilter = Microsoft.DirectX.Direct3D.TextureFilter.Point; // sinon none serait suffisant
            device.SamplerState[0].MipFilter = Microsoft.DirectX.Direct3D.TextureFilter.Point;
            

            // etage 0 plan de video
            // la couleur vient completemetn de la texture
   /*         device.SetTextureStageState(0, TextureStageStates.ColorOperation,true); // il ya a une color operation
            device.TextureState[0].ColorOperation =  TextureOperation.SelectArg1; // valeur de La coloroperation

            device.SetTextureStageState(0, TextureStageStates.ColorArgument1, true);
            device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
            
            // ca ca ne sert a rien puisque arg2 n'est pas utilise
            device.SetTextureStageState(0, TextureStageStates.ColorArgument2, false);
            device.TextureState[0].ColorArgument2 = TextureArgument.Diffuse;

            // operatio sur le alpha : on nen met pas
            device.SetTextureStageState(0, TextureStageStates.AlphaOperation, false);
            //device.SetTextureStageState(0, TextureStageStates.AlphaOperation, true);
            //device.TextureState[0].AlphaOperation = TextureOperation.SelectArg2;
            //device.SetTextureStageState(0, TextureStageStates.AlphaArgument2, true);
            //device.TextureState[0].AlphaArgument2 = TextureArgument.Diffuse;

            // etage 1 : plan de trace graphique instantané
            // on affiche la texure d'avant si on est en colorkey
            device.SetTextureStageState(1, TextureStageStates.ColorOperation, false);
            device.TextureState[1].ColorOperation  = TextureOperation.BlendCurrentAlpha;

            device.SetTextureStageState(1, TextureStageStates.ColorArgument1, true);
            device.TextureState[1].ColorArgument1 = TextureArgument.TextureColor;

            device.SetTextureStageState(1, TextureStageStates.ColorArgument2, false);
     */       
            //device.SetTextureStageState(1, TextureStageStates.AlphaOperation, (int)TextureOperation.SelectArg1);
            //device.SetTextureStageState(1, TextureStageStates.AlphaArgument1, (int)TextureArgument.TextureColor);
            

            // etage 2 : plan de trace graphique transparent
            // sortie = arg& * arg 2
            // arg1 = sortie = texture modulee par le alpha de la texture courante
            // 
            
            //device.SetTextureStageState(1, TextureStageStates.ColorOperation, (int)TextureOperation.BlendCurrentAlpha);
            //device.SetTextureStageState(1, TextureStageStates.ColorArgument1, (int)TextureArgument.TextureColor);
            //device.SetTextureStageState(1, TextureStageStates.ColorArgument2, (int)TextureArgument.Current);
            
            // les vertex sont des textures
            int numlevel=0;
 
         //   device.SetRenderState(RenderStates.SourceBlend, true);
         //   device.SetRenderState(RenderStates.DestinationBlend, true);



            /* ;D3DCMPFUNC
Const D3DCMP_NEVER               = 1
Const D3DCMP_LESS                = 2
Const D3DCMP_EQUAL               = 3
Const D3DCMP_LESSEQUAL           = 4
Const D3DCMP_GREATER             = 5
Const D3DCMP_NOTEQUAL            = 6
Const D3DCMP_GREATEREQUAL        = 7
Const D3DCMP_ALWAYS              = 8
Const D3DCMP_FORCE_DWORD         = $7fffffff	;force 32-bit size enum

 */


            // on charge les textures dans les levels
            //device.SetRenderState(RenderStates.TextureFactor, true);
            
            foreach (TDPanel tdPanel in panelList)
               {
             if (tdPanel.IsVideo)
             {
              
                    device.SetRenderState(RenderStates.AlphaTestEnable, true);
                    device.SetRenderState(RenderStates.AlphaFunction, 8); //8 always
                    int threshhold = 0x000000ff;
                    device.SetRenderState(RenderStates.ReferenceAlpha, threshhold);
                    
                }
                else
             {
                 tdPanel.updatetexture();
                    device.SetRenderState(RenderStates.AlphaTestEnable, true);
                    device.SetRenderState(RenderStates.AlphaFunction,6); //
                    int threshhold = 0x000000ff;
                    device.SetRenderState(RenderStates.ReferenceAlpha, threshhold);
                 
                }
      
                tdPanel.preptexture(); // choisit la texture odd ou even
               
                 numlevel++;
                device.SetTexture(0, tdPanel.PanelTexture);
                device.SetStreamSource(0, tdPanel.PanelVertexBuffer, 0);
                // on dessine la primitve des 3 textures
                     device.DrawPrimitives(Direct3D.PrimitiveType.TriangleStrip, 0, 2);
            }
           
            

           
            // fin du dessin dans le backbuffer
            //End the scene
            device.EndScene();
            
            // on envoie a l'ecran
            try
            {this.device.Present();
            }catch(InvalidCallException e)
                {return;
                }
        }

        /// <summary>
        /// ajoute un tdpanel c a d un plan au stack de directX
        /// </summary>
        /// <param name="video"></param>
        /// <param name="rotateDimension"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="zOffset"></param>
        private void AddTDPanel(bool video,
                        int rotateDimension,
                        float xOffset,
                        float yOffset,
                        float zOffset)
        {
            logger.log("1a");
            //create a new TD control
            TDPanel tdPanel=null;
            try
            {
                tdPanel = new mesure.TDPanel(video);
            }
            catch (System.IO.FileLoadException)
             {  }

            logger.log("1b");
            //tdPanel.TextureFile		= textureFile;
            tdPanel.RotateDimension = rotateDimension;
            tdPanel.XOffset = 0.0f;// xOffset;
            tdPanel.YOffset = 0.0f; // yOffset;
            tdPanel.ZOffset = 0.0f;// zOffset;
            logger.log("tdpanel add");
            //add it to the list
            this.panelList.Add(tdPanel);
        }



               /// <summary>
        /// Initialize the Direct3D device, and rendering timer
        /// </summary>

        private void InitializeTDPanels()
        {
            logger.log("init td panels");
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));

            logger.log("1");
            //Add the test bitmaps to render
            //Please ensure these bitmaps are in the correct directory,
            //or an exception will be thrown on rendering!
            AddTDPanel (true, // video dans plan 0
                        0,
                        0.0F,
                        0.0F,
                        0.0F);
            AddTDPanel( false, // plan 1 : colorkey
                        1,
                        0.0F,
                        0.0F,
                        0.0F);
            AddTDPanel( false, // plan 2 transparence
                        2,
                        0.5F,
                        0.5F,
                        -0.5F);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool InitializeGraphics()
        {
            // Create a PresentParameters object
            Direct3D.PresentParameters presentParams = new Direct3D.PresentParameters();

            // Don't run full screen
            presentParams.Windowed = true;
            
            // Discard the frames
            presentParams.SwapEffect = Direct3D.SwapEffect.Flip;
            presentParams.PresentFlag = PresentFlag.LockableBackBuffer; // pour fairemarcher le getgraphics des surfces ?
            presentParams.PresentationInterval = PresentInterval.One;
            // Instantiate a device
            device = new Direct3D.Device(
                                    0,
                                    Direct3D.DeviceType.Hardware,
                                    this.camwin.PicVideo, // s'adresse a tout la fenetre
                                    Direct3D.CreateFlags.SoftwareVertexProcessing,
                                    presentParams);

            int maxheight = device.DeviceCaps.MaxTextureHeight;

            //Create and reset the device
            device.DeviceReset += new System.EventHandler(OnResetDevice);
            device.DeviceLost += new System.EventHandler(OnLostDevice);

            this.OnCreateDevice(device, null);
            this.OnResetDevice(device, null);
            
            return true;
        }

        
        
        //-------------------------------------------------------------------
        // lors de la  creation du D3D device : on cree les transform view matrix, les projections,
        //-------------------------------------------------------------------
        public void OnCreateDevice(object sender, EventArgs e)
        {
            Direct3D.Device dev = (Direct3D.Device)sender;
            // Now Create the VB

            // Set up our view matrix. A view matrix can be defined given an eye point,
            // a point to lookat, and a direction for which way is up. Here, we set the
            // eye five units back along the z-axis and up three units, look at the
            // origin, and define "up" to be in the y-direction.
            device.Transform.View = Microsoft.DirectX.Matrix.LookAtLH(new Microsoft.DirectX.Vector3(0.0f, 0.0f, -5.0f),
                                                              new Microsoft.DirectX.Vector3(0.5f, 0.5f, 0.0f),
                                                              //new DirectX.Vector3(0.0f, 0.0f, 0.0f)
                                                              new Microsoft.DirectX.Vector3(.0f, .5f, 0.0f)
                                                              );

            // For the projection matrix, we set up a perspective transform (which
            // transforms geometry from 3D view space to 2D viewport space, with
            // a perspective divide making objects smaller in the distance). To build
            // a perpsective transform, we need the field of view (1/4 pi is common),
            // the aspect ratio, and the near and far clipping planes (which define at
            // what distances geometry should be no longer be rendered).
            device.Transform.Projection = Microsoft.DirectX.Matrix.PerspectiveFovLH((float)Math.PI / 4,
                                                                            1.0f,
                                                                            1.0f,
                                                                            100.0f);
        }

        //-------------------------------------------------------------------
        //
        //-------------------------------------------------------------------


        public void OnLostDevice(object sender, EventArgs e)
        {
            int i = 0;

        }
        
        /// <summary>
        /// appelé quand le directx reset le device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnResetDevice(object sender, EventArgs e)
        {
            if (sender!=null)
            {
                //Debug.Write("rst\r\n");
                Direct3D.Device device = (Direct3D.Device)sender;

                bool waitencore = false;
                do
                {
                    try
                    {
                        waitencore = false;
                        device.TestCooperativeLevel();
                    }
                    catch (DeviceLostException)
                    {
                        waitencore = true;
                        Debug.Write("err direct3D");
                    }
                } while (waitencore);

                // Set state
                device.RenderState.DitherEnable = false;
                device.RenderState.Clipping = false;
                device.RenderState.CullMode = Direct3D.Cull.None;
                device.RenderState.Lighting = false;
                device.RenderState.ZBufferEnable = false;
                device.RenderState.ZBufferWriteEnable = false;
                // transparence
                //device.RenderState.AlphaBlendEnable = true;
                //device.RenderState.ZBufferFunction = Compare.Always;

            }
            //set up the alpha bending render state.
            // reglage de alpha deneccsaires a l' keycolor
            //device.RenderState.SourceBlend = Direct3D.Blend.SourceAlpha;
            //device.RenderState.DestinationBlend = Direct3D.Blend.InvSourceAlpha;
            //device.RenderState.AlphaBlendEnable = true;
            
            foreach (TDPanel tdPanel in panelList)
            {
                tdPanel.TDDevice = device;
                if (!tdPanel.IsVideo)
                {
                    Size pictsize;
                    if (camwin.Camera != null)
                        pictsize = CoreSystem.Instance.Etals.Clipper.VisuSize;
                    else
                        pictsize = this.camwin.ClientSize; // si pas de cam on n'a pas de dimension pour la visu
                    if (pictsize.Height <= 0) // ca peut arriver si la fenetre est toute petite
                        pictsize.Height = 10;

                    tdPanel.CreateVertexBuffer(pictsize.Width, pictsize.Height);
                    /*Bitmap bmp = null;
                 if (CoreSystem.Instance.Camera != null)
                        if (CoreSystem.Instance.Camera.refreshImage(ref bmp))
                            tdPanel.PaintFrame(bmp);
                 * */

                    }
                else
                {
                    if (CoreSystem.Instance.Camera != null)
                        {
                        Rectangle clip = CoreSystem.Instance.Etals.Clipper.ClipRect;
                        tdPanel.CreateVertexBuffer(clip.Width, clip.Height);
                        }
                    else
                        tdPanel.CreateVertexBuffer(0, 0); // on cree un vertexbuffer vide
            
                    //device.Present();// soi on met un present ici c'est affreux quand on resize
                    Bitmap bmp = null;
                    if (CoreSystem.Instance.Camera != null)
                        if (CoreSystem.Instance.Camera.refreshImage(ref bmp))
                            tdPanel.PaintFrame(bmp);
                }

            }
            
            
        }

        

    }

}
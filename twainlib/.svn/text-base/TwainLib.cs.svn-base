using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;




namespace TwainLib
{
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

   
   public enum TwainCommand
	{
	Not				= -1,
	Null			= 0,
	TransferReady	= 1,
	CloseRequest	= 2,
	CloseOk			= 3,
	DeviceEvent		= 4
	}




public class Twain
	{
	private const short CountryUSA		= 1;
	private const short LanguageUSA		= 13;
    private bool m_inacq=false;
    private clsimgutils.ImgUtil m_ImgUtil;
    //-------------------------------------------------------------------------------------------- 

[DllImport("gdi32.dll", ExactSpelling = true)] 
internal static extern int SetDIBitsToDevice(IntPtr hdc, int xdst, int ydst, 
                    int width, int height, int xsrc, int ysrc, int start, int lines, 
                       IntPtr bitsptr, IntPtr bmiptr, int color); 

//------------------------------------------------------------------------------------------- 
    [DllImport("ImgUtility.dll", EntryPoint = "copy32ligne")]
    public static extern void copy32ligne(IntPtr dst, IntPtr src, int bytelen);
        
	public Twain()
		{
		appid = new TwIdentity();
		appid.Id				= IntPtr.Zero;
		appid.Version.MajorNum	= 1;
		appid.Version.MinorNum	= 1;
		appid.Version.Language	= LanguageUSA;
		appid.Version.Country	= CountryUSA;
		appid.Version.Info		= "basic twain";
		appid.ProtocolMajor		= TwProtocol.Major;
		appid.ProtocolMinor		= TwProtocol.Minor;
		appid.SupportedGroups	= (int)(TwDG.Image | TwDG.Control);
		appid.Manufacturer		= "Helixee";
		appid.ProductFamily		= "Twain Librairie";
		appid.ProductName		= "Helixee twain interface";

		srcds = new TwIdentity();
		srcds.Id = IntPtr.Zero;
        
        // allocation d'une structure non managee winmsg 
		evtmsg.EventPtr = Marshal.AllocHGlobal( Marshal.SizeOf( winmsg ) );
        m_ImgUtil = new clsimgutils.ImgUtil();
		}

	~Twain()
		{// libere la structure non managee
		Marshal.FreeHGlobal( evtmsg.EventPtr );
		}


    //----------------------------------------
    // demarage de scratch du systeme twain
    // init et positionne la source par defaut
	//-------------------------------------------
    public int Init( IntPtr hwndp )
		{
		// demarrage 
        //DG_COONTROL DAT_PARENT MSG_OPENDSM : Initialize source manager
		TwRC rc = DSMparent( appid, IntPtr.Zero, TwDG.Control, TwDAT.Parent, TwMSG.OpenDSM, ref hwndp );
        
        if (rc != TwRC.Success)
            return 1;

		//DG_CONTROL DAT_IDENTITY MSG_GETDEFAULT :get identity info about default source
		rc = DSMident( appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.GetDefault, srcds );
		if( rc == TwRC.Success )
				hwnd = hwndp;
		else
			//DG_COONTROL DAT_PARENT MSG_CLOSEDSM : Prepare sourcemanager for unloading
            {unInit();
             return 2;
            }
		    
        return 0;
        }

    //---------------------------
    // inverse de init
    //---------------------------
    public int unInit()
        {
            TwRC rc;
            CloseSrc(); // ferme la source si c'est pas deja fait

            if (appid.Id == IntPtr.Zero)
                return 1;

            rc = DSMparent(appid, IntPtr.Zero, TwDG.Control, TwDAT.Parent, TwMSG.CloseDSM, ref hwnd);
            if (rc != TwRC.Success)
                return 2; // probleme fermeture

            appid.Id = IntPtr.Zero;
            return 0; // pas de probleme
        }

    	
    //----------------------------------
    // ouvre dial de selection
    //----------------------------------
        public int Select()
		{
		TwRC rc;
		//CloseSrc(); ferme le twain actuel
		if( appid.Id == IntPtr.Zero )
			{
			if( appid.Id == IntPtr.Zero )
				return 1; // twain pas fonctionnel
			}
          
        // DG_CONTROL / DAT_IDENTITY / MSG_USERSELECT
            // l'application s'identifie aupres du dsm et demande a l'utilisateur de choisir
		rc = DSMident( appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.UserSelect, srcds );
        if (rc != TwRC.Success)
            return 2; // erreur opends

        // sinon l'utilisateur a bien clique une entree qui est maintenant la default source
        return 0; // pas d'erreur
		}

    
    //--------------------------------------------------------------------------
    // dit a la source de demarrer l'acquisition avec le device par dfaut
    // une fois le device ouvert par opends : on demarre l'acquisition
    // il faut que le eventloop soit fonctionnel avant d'appeler cette fonction
    //--------------------------------------------------------------------------
    public int StartAcquire()
		{
		TwRC rc;
		//CloseSrc();
		if( appid.Id == IntPtr.Zero )
            return 1; // id par ready
        
        
        m_inacq = false;

        //DG_CONTROL / DAT_IDENTITY / MSG_OPENDS
        // ouvre la source avec le device decrit dans srcds par msg_userselect
        // si srcds.productname est null ca ouvre le device par defaut
        rc = DSMident(appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.OpenDS, srcds);
        if (rc != TwRC.Success)
            return 2; // erreur opends
        
        // set les capabilities : on met le xfercount a -1
		TwCapability cap = new TwCapability( TwCap.XferCount, -1);
        //TwCapability cap = new TwCapability(TwCap.XferCount, 1);
		rc = DScap( appid, srcds, TwDG.Control, TwDAT.Capability, TwMSG.Set, cap );
		if( rc != TwRC.Success )
			{
			//CloseSrc();
			return 3; // pas cap
			}

        // ouverture du user interface
        // qui demarre l'acquisition en appuyant sur capture
		TwUserInterface	guif = new TwUserInterface();
		guif.ShowUI = 1; // 0 : on montre pas le UI
		guif.ModalUI = 1; // 0 : pas modal
        guif.ModalUI = 0; // 0 : pas modal
		guif.ParentHand = hwnd;

        //DG_CONTROL / DAT_USERINTERFACE / MSG_ENABLEDS : request acquisition
        rc = DSuserif( appid, srcds, TwDG.Control, TwDAT.UserInterface, TwMSG.EnableDS, guif );
		if( rc != TwRC.Success )
			{
			CloseSrc();
			return 4;
			}
        m_inacq = true;
            return 0;
  
		}


    //-------------------------------------------------------------
    // arrete la source pour plus qu'elle envoie d'images
    // et eventuellement la boite de dialogue s'il y en a une
    // apres : on revient en etat 4
	//-------------------------------------------------------------
    public bool isinacq()
    { return m_inacq; 
      //  return true; ; 
    }


    //-----------------------------------------
    // fonction fermant l'acquiqsition et le DS
    // appelle par la boucle de message sur un ordre d'arreter l'acquisition
    //-----------------------------------------
    public int StopAcquire()
		{
		TwRC rc;
		if( srcds.Id == IntPtr.Zero )
            return 1;

		TwUserInterface	guif = new TwUserInterface();
        
        
        // DG_CONTROL / DAT_USERINTERFACE / MSG_DISABLEDS
        // arrete le transfert d'images. ca feme eventuellement le dialogue de reglage s'il est ouvert
        
        rc = DSuserif( appid, srcds, TwDG.Control, TwDAT.UserInterface, TwMSG.DisableDS, guif );
       /* if (rc != TwRC.Success)
        {
            return 5;
        }*/
        

        // on ferme et on decharge la source
        rc = DSMident( appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.CloseDS, srcds );
        /*if( rc != TwRC.Success )
	        {
		     return 4;
		     }*/

        m_inacq = false;

         return 0;
		}


    //------------------------------------
    // globales echangees entre fonctions
    //------------------------------------
    TwPendingXfers pxfr; 
    IntPtr hbitmap = IntPtr.Zero;
    TwImageInfo iinf = new TwImageInfo();




    //-------------------------------------------
    // 
    //-------------------------------------------
    public int stoptransfert()
    {// l'application renvoie un endxfer quand la source a envoye son xferdone
        TwRC rc;
       
        // DG_CONTROL / DAT_PENDINGXFERS / MSG_ENDXFER
        // l'application signale a la source que la transfert est fait
        // on peut regarder dasn pxfr.Count la valeur
        // attention si c'es 0 : la source est revenue en etat 5 : faire un disable et close
        // si c'est -1 il reste des choses a transmeettre mais on sait pas combien et la source est en etat 6
        // et n'emttra pas d'autre xfertready, on doit faire comme si on l'avait recu
        // di on veut empecher la prochaine image  :
        // DG_CONTROL / DAT_PENDINGXFERS / MSG_ENDXFER operation. ()Then, check the Count field again
        // si on veut arretr toutes les images : DG_CONTROL / DAT_PENDINGXFERS /MSG_RESET et on revient en etat 5
        

        // si c'est 1 ou plus : nombre d'images restant a transmettre

       rc = DSpxfer( appid, srcds, TwDG.Control, TwDAT.PendingXfers, TwMSG.EndXfer, pxfr );
	    if( rc != TwRC.Success )
	    {
		return 4;
		}

    return 0;
    }

    
    //-----------------------------------------------------------
    // fonction ransferant les images recues a la fin du transfert
    // appelee par la boucle de messages
    // renvoie un array de toutes les images
    // posiitonne erreur a 0 sipas d'erreur
    //-----------------------------------------------------------
    public ArrayList TransferPictures(ref int erreur)
    {
        TwRC rc;
	    erreur =0;

		ArrayList pics = new ArrayList();
		if( srcds.Id == IntPtr.Zero )
            {erreur =1;
             return pics;
            }

        
        // lit l'image info dqui vient d'arriver
        pxfr = new TwPendingXfers();
        pxfr.Count = 0;

        rc = DSiinf(appid, srcds, TwDG.Image, TwDAT.ImageInfo, TwMSG.Get, iinf);
        if (rc != TwRC.Success)
        {
            erreur =2;
            return pics;
        }

        //rc = DSpxfer(appid, srcds, TwDG.Control, TwDAT.PendingXfers, TwMSG.Get, pxfr);

        /*if (pxfr.Count == 0)
        {
            rc = DSpxfer(appid, srcds, TwDG.Control, TwDAT.PendingXfers, TwMSG.Reset, pxfr);
		
            erreur = 2;
            return pics;
        }*/
        Thread.Sleep(100);
		do
        {
            hbitmap = IntPtr.Zero;
            //while (hbitmap == IntPtr.Zero)
            {// on initie le transfert
                rc = DSixfer(appid, srcds, TwDG.Image, TwDAT.ImageNativeXfer, TwMSG.Get, ref hbitmap);
                if (rc != TwRC.XferDone)
                {
                   erreur=3;
                   return pics;
                }
            }
             //on transforme la hbitmap en une bitmap
            IntPtr bitmapok = Twain.GlobalLock(hbitmap);
           // Bitmap bmp = Bitmap.FromHbitmap(hbitmap);
            BITMAPINFOHEADER bih =  new BITMAPINFOHEADER();
            
            // passe le contenu de bitmapok dans le bitmapinfoheader
            bih = (BITMAPINFOHEADER)Marshal.PtrToStructure(bitmapok, bih.GetType());
            // on a charge le handle dans la bitmapinfoheader 

            IntPtr pixptr = (IntPtr)((int)bitmapok+(int)bih.biSize); // adresse du buffer de data

            // on cree une bitmap a la bonne taille
            Bitmap newbmp = new Bitmap(bih.biWidth, bih.biHeight, PixelFormat.Format24bppRgb);	// on cree une bitmap           
          
            //Rectangle bitmaprect = new Rectangle(0, 0, bih.biWidth, bih.biHeight);
            Rectangle bitmaprect = new Rectangle(0, 0, newbmp.Width, newbmp.Height);
            // on verrouille son buffer
            /*BitmapData bmData = newbmp.LockBits(bitmaprect,
                                            ImageLockMode.ReadWrite,
                                           PixelFormat.Format32bppArgb);

            */
          /*  Graphics TempGrap = Graphics.FromImage(newbmp);
            TempGrap.Clear(Color.White);
            TempGrap.DrawRectangle(new Pen(Color.FromArgb(254,Color.Aquamarine)),0,0,100,100);
            IntPtr hdc = TempGrap.GetHdc();
           
        
                // copie les bits de 
                //DIB_RGB_COLORS : 0
            SetDIBitsToDevice(hdc, 0, 0, bih.biWidth, bih.biHeight, 0, 0, bih.biWidth, bih.biHeight, pixptr, bmData.Scan0, 0);
           
            TempGrap.ReleaseHdc(hdc);
            TempGrap.Dispose();

            newbmp.UnlockBits(bmData);
            
           */ 
            // on copie les octests apres bitmapok dans le buffer de la bitmap
           //unsafe{
            //IntPtr bitmapData = bmData.Scan0;
            IntPtr address = (IntPtr)((uint)pixptr + (uint)Marshal.SizeOf(bih));
            
            //copy32ligne(bitmapData, pixptr, (int)bih.biSizeImage);
            switch(iinf.BitsPerPixel)
            {case 24 :             
                m_ImgUtil.copybuftobmp(address, newbmp, new Rectangle(0, 0, newbmp.Width, newbmp.Height),1);
                break;
             case 8:
                //m_ImgUtil.copybuf8tobmp(address, newbmp, new Rectangle(0, 0, newbmp.Width, newbmp.Height), 1);
                break;
            }
            
            Twain.GlobalUnlock(hbitmap);
            
            // on envoie cette bitmap dans l'array
            pics.Add(newbmp);

            
            rc = DSpxfer(appid, srcds, TwDG.Control, TwDAT.PendingXfers, TwMSG.EndXfer, pxfr);
            if (rc != TwRC.Success)
            {
                CloseSrc();
                return pics;
            }
            // le transfert est finit, tout baigne
            //rc = DSpxfer(appid, srcds, TwDG.Control, TwDAT.PendingXfers, TwMSG.Reset, pxfr);
			}while( pxfr.Count != 0 ); // on regarde si d'autres trucs sont a transferer

        rc = DSpxfer(appid, srcds, TwDG.Control, TwDAT.PendingXfers, TwMSG.Reset, pxfr);
		
		
		return pics;
		}


    //-------------------
    // inutilisee
    //-------------------
    protected IntPtr GetPixelInfo(IntPtr bmpptr)
    {

        BITMAPINFOHEADER bmi = new BITMAPINFOHEADER();

        Marshal.PtrToStructure(bmpptr, bmi);
        Rectangle _bmpRect = new Rectangle(); ;

        _bmpRect.X = _bmpRect.Y = 0;

        _bmpRect.Width = bmi.biWidth;

        _bmpRect.Height = bmi.biHeight;

        if (bmi.biSizeImage == 0)

            bmi.biSizeImage = (uint)(((((bmi.biWidth * bmi.biBitCount) + 31) & ~31) >> 3) * bmi.biHeight);

        int p = (int)bmi.biClrUsed;

        if ((p == 0) && (bmi.biBitCount <= 8))

            p = 1 << bmi.biBitCount;

        p = (int)((p * 4) + (uint)bmi.biSize + (int)bmpptr);

        return (IntPtr)p;

    }


    //------------------------------------------------------
    // fonction appelee par la boucle de messages 
    // l'application passe tous ses messages a la source
    // en appelant DG_CONTROL /DAT_EVENT / MSG_PROCESSEVENT
    //------------------------------------------------------
	public TwainCommand PassMessage( ref Message m )
		{
		if( srcds.Id == IntPtr.Zero )
			return TwainCommand.Not;

		int pos = GetMessagePos();

		winmsg.hwnd		= m.HWnd;
		winmsg.message	= m.Msg;
		winmsg.wParam	= m.WParam;
		winmsg.lParam	= m.LParam;
		winmsg.time		= GetMessageTime();
		winmsg.x		= (short) pos;
		winmsg.y		= (short) (pos >> 16);
		
        // copie la winmsg depuis de la structure managee winmsg 
        // dans la structure non manage evtmsg.EventPtr passee a la fonction twain processevent
		Marshal.StructureToPtr( winmsg, evtmsg.EventPtr, false );
		evtmsg.Message = 0;
        
        //DG_CONTROL /DAT_EVENT / MSG_PROCESSEVENT
		TwRC rc = DSevent( appid, srcds, TwDG.Control, TwDAT.Event, TwMSG.ProcessEvent, ref evtmsg );
     //   if (evtmsg.Message == 0)
       //     return TwainCommand.Not;

        if( rc == TwRC.NotDSEvent ) // pas de message traite : retourne on ne fait rien
			return TwainCommand.Not;

        // on regarde le message charge par la source dans evtmsg
		if( evtmsg.Message == (short) TwMSG.XFerReady ) // transfert pret a partir
			return TwainCommand.TransferReady;

		if( evtmsg.Message == (short) TwMSG.CloseDSReq ) // demande de fermer le DS (clic sur cancel du dialog)
			return TwainCommand.CloseRequest;

		if( evtmsg.Message == (short) TwMSG.CloseDSOK ) // demande de fermer le dialog (clic sur ok du dialog)
			return TwainCommand.CloseOk;

		if( evtmsg.Message == (short) TwMSG.DeviceEvent )// devie event quelconque
			return TwainCommand.DeviceEvent;

		return TwainCommand.Null; // sinon rien
		}
    
    
    // ferme la source
    public void CloseSrc()
    {
        TwRC rc;
        if (srcds.Id != IntPtr.Zero)
        {
            TwUserInterface guif = new TwUserInterface();
            rc = DSuserif(appid, srcds, TwDG.Control, TwDAT.UserInterface, TwMSG.DisableDS, guif);
            // on ferme et on decharge la source
            rc = DSMident( appid, IntPtr.Zero, TwDG.Control, TwDAT.Identity, TwMSG.CloseDS, srcds );

        }
    }

    
    


	private IntPtr		hwnd;
	private TwIdentity	appid;
	private TwIdentity	srcds;
	private TwEvent		evtmsg;
	private WINMSG		winmsg;
	


	// ------ DSM entry point DAT_ variants:
		[DllImport("twain_32.dll", EntryPoint="#1")]
	private static extern TwRC DSMparent( [In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, ref IntPtr refptr );

		[DllImport("twain_32.dll", EntryPoint="#1")]
	private static extern TwRC DSMident( [In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwIdentity idds );

		[DllImport("twain_32.dll", EntryPoint="#1")]
	private static extern TwRC DSMstatus( [In, Out] TwIdentity origin, IntPtr zeroptr, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwStatus dsmstat );


	// ------ DSM entry point DAT_ variants to DS:
		[DllImport("twain_32.dll", EntryPoint="#1")]
	private static extern TwRC DSuserif( [In, Out] TwIdentity origin, [In, Out] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, TwUserInterface guif );

		[DllImport("twain_32.dll", EntryPoint="#1")]
	private static extern TwRC DSevent( [In, Out] TwIdentity origin, [In, Out] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, ref TwEvent evt );

		[DllImport("twain_32.dll", EntryPoint="#1")]
	private static extern TwRC DSstatus( [In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwStatus dsmstat );

		[DllImport("twain_32.dll", EntryPoint="#1")]
	private static extern TwRC DScap( [In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwCapability capa );

		[DllImport("twain_32.dll", EntryPoint="#1")]
	private static extern TwRC DSiinf( [In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwImageInfo imginf );

		[DllImport("twain_32.dll", EntryPoint="#1")]
	private static extern TwRC DSixfer( [In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, ref IntPtr hbitmap );

		[DllImport("twain_32.dll", EntryPoint="#1")]
	private static extern TwRC DSpxfer( [In, Out] TwIdentity origin, [In] TwIdentity dest, TwDG dg, TwDAT dat, TwMSG msg, [In, Out] TwPendingXfers pxfr );


		[DllImport("kernel32.dll", ExactSpelling=true)]
	internal static extern IntPtr GlobalAlloc( int flags, int size );
		[DllImport("kernel32.dll", ExactSpelling=true)]
	internal static extern IntPtr GlobalLock( IntPtr handle );
		[DllImport("kernel32.dll", ExactSpelling=true)]
	internal static extern bool GlobalUnlock( IntPtr handle );
		[DllImport("kernel32.dll", ExactSpelling=true)]
	internal static extern IntPtr GlobalFree( IntPtr handle );

		[DllImport("user32.dll", ExactSpelling=true)]
	private static extern int GetMessagePos();
		[DllImport("user32.dll", ExactSpelling=true)]
	private static extern int GetMessageTime();


		[DllImport("gdi32.dll", ExactSpelling=true)]
	private static extern int GetDeviceCaps( IntPtr hDC, int nIndex );

		[DllImport("gdi32.dll", CharSet=CharSet.Auto)]
	private static extern IntPtr CreateDC( string szdriver, string szdevice, string szoutput, IntPtr devmode );

		[DllImport("gdi32.dll", ExactSpelling=true)]
	private static extern bool DeleteDC( IntPtr hdc );




	public static int ScreenBitDepth {
		get {
			IntPtr screenDC = CreateDC( "DISPLAY", null, null, IntPtr.Zero );
			int bitDepth = GetDeviceCaps( screenDC, 12 );
			bitDepth *= GetDeviceCaps( screenDC, 14 );
			DeleteDC( screenDC );
			return bitDepth;
			}
		}


		[StructLayout(LayoutKind.Sequential, Pack=4)]
	internal struct WINMSG
		{
		public IntPtr		hwnd;
		public int			message;
		public IntPtr		wParam;
		public IntPtr		lParam;
		public int			time;
		public int			x;
		public int			y;
		}


	} // class Twain
}

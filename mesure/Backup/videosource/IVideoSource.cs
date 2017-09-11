// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using mesure;

namespace VideoSource
{
	
	/// <summary>
	/// IVideoSource interface
	/// </summary>
	public interface IVideoSource : IBagSavXml, IDisposable
	{
		/// <summary>
		/// New frame event - notify client about the new frame
		/// </summary>
		event CameraEventHandler NewFrame;
        event SourceResizeHandler OnResize;

		/// <summary>
		/// Video source property
		/// </summary>
		string VideoSource{get; set;}

        int GetListSources(ArrayList List);
        
        bool refreshimage(ref Bitmap bmp);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool capchglive();  // peut etre live ou freeze
        bool capgrab();  // peut saisir une image
        bool capregl();  // peut etre reglé
        bool isfreeze(); // est freeze
        bool freeze();
        bool live();
        bool grab();

        
        
        /// <summary>
		/// Login property
		/// </summary>
		string Login{get; set;}

		/// <summary>
		/// Password property
		/// </summary>
		string Password{get; set;}

		/// <summary>
		/// FramesReceived property
		/// get number of frames the video source received from the last
		/// access to the property
		/// </summary>
		int FramesReceived{get;}

		/// <summary>
		/// BytesReceived property
		/// get number of bytes the video source received from the last
		/// access to the property
		/// </summary>
		int BytesReceived{get;}

		/// <summary>
		/// UserData property
		/// allows to associate user data with an object
		/// </summary>
		object UserData{get; set;}

		/// <summary>
		/// Get state of video source
		/// </summary>
		bool Running{get;}

		/// <summary>
		/// Start receiving video frames
		/// </summary>
		void Start(int iteractif);

		/// <summary>
		/// Stop receiving video frames
		/// </summary>
		void SignalToStop();

        int DoReglage(Control parent);
        int DoInit(Control parent);
        
        /// <summary>
        /// ouvre la fenetre de choix des proprietes de la source
        /// appelé apres creation de la source et avnat son demarrage
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
       // int InitChoix(Control parent);


        // selection de la source
        // renvoie 1 si la source est selectionnee
        // renvoie 0 sinon
      
        /// <summary>
		/// Wait for stop
		/// </summary>
		void WaitForStop();

		/// <summary>
		/// Stop work
		/// </summary>
		void Stop();

        /// <summary>
        /// cree ou renvoie le rectangle de clipping
        /// </summary>
     //   Rectangle ClipRect{get; set;}
        Rectangle ClipRect {set;}

        /// <summary>
        /// renvoie le rectangle de video
        /// </summary>
        //Rectangle SourceRect{get;}
        
	}
}

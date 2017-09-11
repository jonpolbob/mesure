// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//
namespace VideoSource
{
	using System;
	using System.Drawing.Imaging;
    using System.Drawing;

	// NewFrame delegate
	public delegate void CameraEventHandler(object sender, CameraEventArgs e);
    public delegate void SourceResizeHandler(object sender, ref SourceResizeArgs e);

	/// <summary>
	/// Camera event arguments
	/// </summary>
	public class CameraEventArgs : EventArgs
	{
		private System.Drawing.Bitmap bmp;

		// Constructor
		public CameraEventArgs(System.Drawing.Bitmap bmp)
		{
			this.bmp = bmp;
		}

		// Bitmap property
		public System.Drawing.Bitmap Bitmap
		{
			get { return bmp; }
		}
	}



    public class SourceResizeArgs: EventArgs
    {
        private bool m_init;
        private int m_width;
        private int m_height;
        private Rectangle m_cliprect;

        // Constructor
        public SourceResizeArgs(int width, int height, Rectangle RectClip)
        {
            m_width = width;
            m_height = height;
            m_cliprect = new Rectangle(0,0,width,height);
            RectClip = m_cliprect;
        }

        // Bitmap property
        public int width
        {
            get { return m_width; }
        }
        
 
        // Bitmap property
        public int height
        {
            get { return m_height; }
        }

        // Bitmap property
        public Rectangle cliprect
        {
            get { return m_cliprect; }   
        }

        // fonction positionnant le rectangle
        // utilisee a la place d'un set parce que c'est jamais appele par les evenements
        public void setcliprect(Rectangle newrect)
        {m_cliprect = newrect;
        }
        
    }
}
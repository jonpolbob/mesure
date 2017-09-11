// Motion Detector
//
// Copyright © Andrew Kirillov, 2005
// andrew.kirillov@gmail.com
//

namespace mesure
{
	using System;
	using System.Drawing;
    using System.Windows.Forms;

	/// <summary>
	/// IMotionDetector interface
	/// </summary>
	public interface IMotionDetector
	{
		/// <summary>
		/// Motion level calculation - calculate or not motion level
		/// </summary>
		bool MotionLevelCalculation{ set; get; }

		/// <summary>
		/// Motion level - amount of changes in percents
		/// </summary>
		double MotionLevel{ get; }

		/// <summary>
		/// Process new frame
		/// </summary>
        void ProcessFrame(ref Bitmap image, ref Control control);

		/// <summary>
		/// Reset detector to initial state
		/// </summary>
		void Reset( );
	}
}

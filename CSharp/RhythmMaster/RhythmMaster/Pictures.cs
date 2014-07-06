using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalEditor.ObjectModels.Multimedia.Picture;

namespace RhythmMaster
{
	public static class Pictures
	{
		private static PictureObjectModel mvarLogo = null;
		public static PictureObjectModel Logo { get { return mvarLogo; } set { mvarLogo = value; } }

		private static PictureObjectModel mvarLearningStatusReady = null;
		public static PictureObjectModel LearningStatusReady { get { return mvarLearningStatusReady; } set { mvarLearningStatusReady = value; } }

		private static PictureObjectModel mvarLearningStatusSuccessful = null;
		public static PictureObjectModel LearningStatusSuccessful { get { return mvarLearningStatusSuccessful; } set { mvarLearningStatusSuccessful = value; } }
	}
}

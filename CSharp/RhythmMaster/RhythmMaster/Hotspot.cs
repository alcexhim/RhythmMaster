using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmMaster
{
	public class Hotspot
	{
		public class HotspotCollection
			: System.Collections.ObjectModel.Collection<Hotspot>
		{

			public Hotspot this[string ID]
			{
				get
				{
					foreach (Hotspot hotspot in this)
					{
						if (hotspot.ID == ID) return hotspot;
					}
					return null;
				}
			}

		}

		private string mvarID = String.Empty;
		public string ID { get { return mvarID; } set { mvarID = value; } }

		private double mvarX = 0.0;
		public double X { get { return mvarX; } set { mvarX = value; } }

		private double mvarY = 0.0;
		public double Y { get { return mvarY; } set { mvarY = value; } }

		private double mvarWidth = 0.0;
		public double Width { get { return mvarWidth; } set { mvarWidth = value; } }

		private double mvarHeight = 0.0;
		public double Height { get { return mvarHeight; } set { mvarHeight = value; } }

		private bool mvarSelected = false;
		public bool Selected { get { return mvarSelected; } set { mvarSelected = value; } }

		private TriggerLearningStatus mvarLearningStatus = TriggerLearningStatus.None;
		public TriggerLearningStatus LearningStatus { get { return mvarLearningStatus; } set { mvarLearningStatus = value; } }
	}
}

using MonoMidi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmMaster
{
	public class Binding
	{
		public class BindingCollection
			: System.Collections.ObjectModel.Collection<Binding>
		{
		}

		private MessageType mvarMessageType = MessageType.ControlChange;
		public MessageType MessageType { get { return mvarMessageType; } set { mvarMessageType = value; } }

		private byte mvarChannel = 0;
		public byte Channel { get { return mvarChannel; } set { mvarChannel = value; } }

		private byte mvarParameter1 = 0;
		public byte Parameter1 { get { return mvarParameter1; } set { mvarParameter1 = value; } }

		private string mvarHotspotID = String.Empty;
		public string HotspotID { get { return mvarHotspotID; } set { mvarHotspotID = value; } }
	}
}

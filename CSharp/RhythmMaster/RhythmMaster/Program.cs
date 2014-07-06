using System;
using System.Collections.Generic;
using System.Linq;
using Caltron;
using UniversalEditor.ObjectModels.FileSystem;
using UniversalEditor.Accessors;
using UniversalEditor.DataFormats.FileSystem.FARC;
using UniversalEditor.ObjectModels.Markup;
using UniversalEditor.DataFormats.Markup.XML;
using UniversalEditor;
using UniversalEditor.ObjectModels.Multimedia.Picture;

namespace RhythmMaster
{
	static class Program
	{
		private static Hotspot.HotspotCollection mvarHotspots = new Hotspot.HotspotCollection();
		public static Hotspot.HotspotCollection Hotspots { get { return mvarHotspots; } }

		private static FileSystemObjectModel mvarFileSystem = new FileSystemObjectModel();
		public static FileSystemObjectModel FileSystem { get { return mvarFileSystem; } }

		private static MarkupObjectModel mvarConfiguration = new MarkupObjectModel();
		public static MarkupObjectModel Configuration { get { return mvarConfiguration; } }

		private static Binding.BindingCollection mvarBindings = new Binding.BindingCollection();
		public static Binding.BindingCollection Bindings { get { return mvarBindings; } }

		private static List<Document> mvarOpenDocuments = new List<Document>();

		private static string mvarDataDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

		private static void InitializeFileSystem()
		{
			string[] farcFiles = System.IO.Directory.GetFiles(mvarDataDirectory, "*.farc", System.IO.SearchOption.AllDirectories);
			FARCDataFormat farc = new FARCDataFormat();
			foreach (string farcFile in farcFiles)
			{
				FileSystemObjectModel fsom = new FileSystemObjectModel();
				Document d = UniversalEditor.Document.Load(fsom, farc, new FileAccessor(farcFile), false);
				mvarOpenDocuments.Add(d);
				fsom.CopyTo(mvarFileSystem);
			}
		}
		private static void InitializeConfiguration()
		{
			File[] configFiles = mvarFileSystem.GetFiles("*.xml");
			XMLDataFormat xml = new XMLDataFormat();
			foreach (File file in configFiles)
			{
				MarkupObjectModel mom = new MarkupObjectModel();
				UniversalEditor.Document.Load(mom, xml, new MemoryAccessor(file.GetDataAsByteArray()));
				mom.CopyTo(mvarConfiguration);
			}

			string[] xmlFiles = System.IO.Directory.GetFiles(mvarDataDirectory, "*.xml", System.IO.SearchOption.AllDirectories);
			foreach (string xmlfile in xmlFiles)
			{
				MarkupObjectModel mom = new MarkupObjectModel();
				UniversalEditor.Document.Load(mom, xml, new FileAccessor(xmlfile));
				mom.CopyTo(mvarConfiguration);
			}
		}
		private static void InitializePictures()
		{
			#region Logo
			{
				File file = Program.FileSystem.FindFile("Images/RhythmMaster Logo.tga");
				Pictures.Logo = UniversalEditor.Common.Reflection.GetAvailableObjectModel<PictureObjectModel>(file.GetDataAsByteArray(), file.Name);
			}
			#endregion
			#region Learning Status Ready
			{
				File file = Program.FileSystem.FindFile("Images/Learning Status/Learning Status Ready.tga");
				Pictures.LearningStatusReady = UniversalEditor.Common.Reflection.GetAvailableObjectModel<PictureObjectModel>(file.GetDataAsByteArray(), file.Name);
			}
			#endregion
			#region Learning Status Ready
			{
				File file = Program.FileSystem.FindFile("Images/Learning Status/Learning Status Successful.tga");
				Pictures.LearningStatusSuccessful = UniversalEditor.Common.Reflection.GetAvailableObjectModel<PictureObjectModel>(file.GetDataAsByteArray(), file.Name);
			}
			#endregion
		}
		private static void InitializeHotspots()
		{
			MarkupTagElement tagHotspots = (Program.Configuration.FindElement("RhythmMaster", "Games", "Game", "Hotspots") as MarkupTagElement);
			foreach (MarkupElement elHotspot in tagHotspots.Elements)
			{
				MarkupTagElement tagHotspot = (elHotspot as MarkupTagElement);
				if (tagHotspot == null) continue;
				if (tagHotspot.FullName != "Hotspot") continue;

				Hotspot hotspot = new Hotspot();
				hotspot.ID = tagHotspot.Attributes["ID"].Value;
				hotspot.X = Double.Parse(tagHotspot.Attributes["X"].Value);
				hotspot.Y = Double.Parse(tagHotspot.Attributes["Y"].Value);
				hotspot.Width = Double.Parse(tagHotspot.Attributes["Width"].Value);
				hotspot.Height = Double.Parse(tagHotspot.Attributes["Height"].Value);
				mvarHotspots.Add(hotspot);
			}

			MarkupTagElement tagDefaultBindings = (Program.Configuration.FindElement("RhythmMaster", "Games", "Game", "Input", "Bindings") as MarkupTagElement);
			foreach (MarkupElement el in tagDefaultBindings.Elements)
			{
				MarkupTagElement tag = (el as MarkupTagElement);
				if (tag == null) continue;
				if (tag.FullName != "Binding") continue;

				MarkupAttribute attMessageType = tag.Attributes["MessageType"];
				if (attMessageType == null) continue;

				MarkupAttribute attChannel = tag.Attributes["Channel"];
				if (attChannel == null) continue;

				MarkupAttribute attParameter1 = tag.Attributes["Parameter1"];
				if (attParameter1 == null) continue;

				MarkupAttribute attHotspotID = tag.Attributes["HotspotID"];
				if (attHotspotID == null) continue;

				Binding binding = new Binding();
				switch (attMessageType.Value)
				{
					case "ControlChange":
					{
						binding.MessageType = MonoMidi.MessageType.ControlChange;
						break;
					}
				}

				binding.Channel = Byte.Parse(attChannel.Value);
				binding.Parameter1 = Byte.Parse(attParameter1.Value);
				binding.HotspotID = attHotspotID.Value;
				mvarBindings.Add(binding);
			}
		}

		private static Window mvarMainWindow = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.Initialize();

			InitializeFileSystem();
			InitializeConfiguration();
			InitializePictures();
			InitializeHotspots();

			mvarLearning = true;
			mvarLearningHotspotIndex = 0;
			mvarHotspots[mvarLearningHotspotIndex].LearningStatus = TriggerLearningStatus.Ready;

			MonoMidi.Listener.Start();
			MonoMidi.Listener.MessageReceived += Listener_MessageReceived;

			mvarMainWindow = new MainWindow();

			Application.Start();

			MonoMidi.Listener.Stop();

			foreach (Document d in mvarOpenDocuments)
			{
				d.Close();
			}
		}

		private static bool mvarLearning = false;
		public static bool Learning { get { return mvarLearning; } }

		private static int mvarLearningHotspotIndex = 0;

		static void Listener_MessageReceived(object sender, MonoMidi.MessageReceivedEventArgs e)
		{
			if (mvarLearning)
			{
				// if (mvarLearningHotspotIndex - 1 >= 0) mvarHotspots[mvarLearningHotspotIndex - 1].LearningStatus = TriggerLearningStatus.None;
				mvarHotspots[mvarLearningHotspotIndex].LearningStatus = TriggerLearningStatus.Successful;
				if (mvarLearningHotspotIndex < mvarHotspots.Count - 1)
				{
					mvarLearningHotspotIndex++;
					mvarHotspots[mvarLearningHotspotIndex].LearningStatus = TriggerLearningStatus.Ready;
				}
				else
				{
					mvarLearning = false;
				}
			}
			else
			{
				foreach (Binding binding in mvarBindings)
				{
					if (e.Message.MessageType == binding.MessageType && e.Message.Parameter1 == binding.Parameter1 && e.Message.Channel == binding.Channel)
					{
						mvarHotspots[binding.HotspotID].Selected = true;
						System.Threading.Thread.Sleep(50);
						mvarHotspots[binding.HotspotID].Selected = false;
					}
				}
			}
			if (mvarMainWindow != null) mvarMainWindow.Refresh();
		}
	}
}

using Caltron;
using Caltron.Controls.Controls2D;
using RhythmMaster.Screens;
using System;
using System.Collections.Generic;
using UniversalEditor;
using UniversalEditor.ObjectModels.FileSystem;
using UniversalEditor.ObjectModels.Markup;
using UniversalEditor.ObjectModels.Multimedia.Picture;

namespace RhythmMaster
{
	public class MainWindow : Window
	{
		public MainWindow()
		{
			base.Text = Program.Configuration.GetElementValue(new string[] { "RhythmMaster", "Information", "Title" }, "RhythmMaster");
			base.AlwaysRender = true;

			SplashScreen scrnSplashScreen = new SplashScreen();
			scrnSplashScreen.Visible = true;
			base.Controls.Add(scrnSplashScreen);

			InputSetupScreen scrnInputSetup = new InputSetupScreen();
			scrnInputSetup.Visible = false;
			base.Controls.Add(scrnInputSetup);

			MainMenuScreen scrnMainMenu = new MainMenuScreen();
			scrnMainMenu.Visible = false;
			base.Controls.Add(scrnMainMenu);

			Timer.SetTimeout(3000, delegate(object sender, EventArgs e)
			{
				scrnSplashScreen.Visible = false;
				scrnMainMenu.Visible = true;
			});
		}
	}
}

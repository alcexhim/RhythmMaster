using Caltron.Controls.Controls2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalEditor;

namespace RhythmMaster.Screens
{
	public class SplashScreen : Screen
	{
		public SplashScreen()
		{
			base.BackgroundColor = Colors.White;
			base.Visible = false;
		}

		protected override void OnCreated(EventArgs e)
		{
			base.OnCreated(e);

			Image imgLogo = new Image(Pictures.Logo);
			imgLogo.Position = new PositionVector2(0, 0);
			imgLogo.Size = base.Size;

			base.Controls.Add(imgLogo);
		}
	}
}

using Caltron.Controls.Controls2D;
using Caltron.Controls.Controls2D.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RhythmMaster.Screens
{
	public class MainMenuScreen : MenuScreen
	{
		public MainMenuScreen()
		{
			base.MenuItems.Add("Start New Game");
			base.MenuItems.Add("Resume Previous Game");
			base.MenuItems.Add("Quit");
		}

		protected override void OnMenuItemActivated(MenuItemActivatedEventArgs e)
		{
			base.OnMenuItemActivated(e);
			switch (e.MenuItem.Name)
			{
				case "Quit":
				{
					Caltron.Application.Stop();
					break;
				}
			}
		}
	}
}

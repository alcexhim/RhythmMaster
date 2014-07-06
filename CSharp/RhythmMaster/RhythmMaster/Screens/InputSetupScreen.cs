using Caltron.Controls.Controls2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniversalEditor;

namespace RhythmMaster.Screens
{
	public class InputSetupScreen : Screen
	{
		protected override void OnRender(Caltron.RenderEventArgs e)
		{
			base.OnRender(e);
			e.Canvas.Clear(Colors.Black);
			
			foreach (Hotspot hotspot in Program.Hotspots)
			{
				e.Canvas.Color = Colors.LightGreen;
				if (hotspot.Selected)
				{
					e.Canvas.FillCircle(hotspot.X, hotspot.Y, hotspot.Width, hotspot.Height);
				}
				else
				{
					e.Canvas.DrawCircle(hotspot.X, hotspot.Y, hotspot.Width, hotspot.Height);
				}

				if (Program.Learning)
				{
					if (hotspot.LearningStatus == TriggerLearningStatus.Ready)
					{
						e.Canvas.DrawImage(hotspot.X, hotspot.Y, hotspot.Width, hotspot.Height, Pictures.LearningStatusReady);
					}
					else if (hotspot.LearningStatus == TriggerLearningStatus.Successful)
					{
						e.Canvas.DrawImage(hotspot.X, hotspot.Y, hotspot.Width, hotspot.Height, Pictures.LearningStatusSuccessful);
					}
				}
			}
		}
	}
}

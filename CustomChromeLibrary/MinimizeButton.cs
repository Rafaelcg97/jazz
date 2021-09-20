using System;
using System.Windows;
using Microsoft.Windows.Shell;

namespace CustomChromeLibrary
{
	public class MinimizeButton : CaptionButton
	{
		static MinimizeButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MinimizeButton), new FrameworkPropertyMetadata(typeof(MinimizeButton)));
		}

		protected override void OnClick()
		{
			base.OnClick();
            Microsoft.Windows.Shell.SystemCommands.MinimizeWindow(Window.GetWindow(this));
		}
	}
}

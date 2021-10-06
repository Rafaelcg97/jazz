using System;
using System.ComponentModel;
using System.Windows;
using Microsoft.Windows.Shell;
using SystemCommands = Microsoft.Windows.Shell.SystemCommands;

namespace CustomChromeLibrary
{
	public class MaximizeButton : CaptionButton
	{
		static MaximizeButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(MaximizeButton), new FrameworkPropertyMetadata(typeof(MaximizeButton)));
		}

		public MaximizeButton()
		{
			DataContext = this;
		}
	}
}

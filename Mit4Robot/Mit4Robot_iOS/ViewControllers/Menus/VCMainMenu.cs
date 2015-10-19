using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Shared;
using System.Drawing;
using CoreGraphics;

namespace Mit4RobotApp
{
	partial class VCMainMenu : UIViewController
	{
		public VCMainMenu (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			GlobalSupport.ButtonHeight = btnStartGame.Frame.Height / 3 * 2 + 5;
		}
			
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			UpdateGUI ();
		}

		public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate (fromInterfaceOrientation);

			UpdateGUI ();
		}
						
		public void UpdateGUI ()
		{
			GlobalSupport.UpdateScreenSize (new CGRect (
				0,
				0,
				UIScreen.MainScreen.Bounds.Width,
				UIScreen.MainScreen.Bounds.Height));

			scrollView.Frame = GlobalSupport.MainCGRect;

			foreach (var item in MainView.Subviews) 
			{
				if (item is UIButton) 
				{
					item.SizeToFit ();
				}
			}

			nfloat yCoordinate = 5;

			btnStartGame.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnStartGame.Frame.Height * 2 + 5;

			btnOptions.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnOptions.Frame.Height + 5;

			btnHelp.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnHelp.Frame.Height + 5;

			btnHighscores.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnHighscores.Frame.Height + 5;

			btnAchievements.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnAchievements.Frame.Height + 5;

			btnCredits.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);
		}
	}
}

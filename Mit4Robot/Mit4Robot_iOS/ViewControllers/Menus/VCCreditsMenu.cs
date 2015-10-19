using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Shared;
using CoreGraphics;

namespace Mit4RobotApp
{
	partial class VCCreditsMenu : UIViewController
	{
		public VCCreditsMenu (IntPtr handle) : base (handle)
		{
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

			textView.Text = NSBundle.MainBundle.LocalizedString("txtCredits", "", "");
			textView.Frame = scrollView.Frame;
			textView.BackgroundColor = UIColor.Black;
			textView.TextColor = UIColor.White;
		}
	}
}

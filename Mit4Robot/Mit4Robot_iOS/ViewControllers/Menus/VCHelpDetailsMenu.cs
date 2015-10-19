using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using System.Drawing;
using Shared;
using CoreGraphics;

namespace Mit4RobotApp
{
	partial class VCHelpDetailsMenu : UIViewController
	{
		public VCHelpDetailsMenu(IntPtr handle) : base(handle)
		{
		}

		public VCHelpDetailsMenu()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			MainView = new UIView();

			scrollView = new UIScrollView();

			imageView = new UIImageView();
			imageView.BackgroundColor = UIColor.Black;

			textView = new UITextView();
			textView.Editable = false;
			textView.BackgroundColor = UIColor.Black;
			textView.TextColor = UIColor.White;

			scrollView.AddSubviews(new UIView[] { imageView, textView });

			this.View.AddSubview(scrollView);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			UpdateGUI();
		}

		public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate(fromInterfaceOrientation);

			UpdateGUI();
		}

		public void UpdateGUI()
		{
			GlobalSupport.UpdateScreenSize(new CGRect(
					0, 
					0,
					UIScreen.MainScreen.Bounds.Width,
					UIScreen.MainScreen.Bounds.Height));

			scrollView.Frame = GlobalSupport.MainCGRect;

			switch (GlobalSupport.MessageIdentifier)
			{
				case 1:
					textView.Text = NSBundle.MainBundle.LocalizedString("txtHelpPython", "", "");
					imageView.Image = UIImage.FromFile("Images/Help/HelpPython");
					break;
				case 2:
					textView.Text = NSBundle.MainBundle.LocalizedString("txtHelpPascal", "", "");
					imageView.Image = UIImage.FromFile("Images/Help/HelpPascal");
					break;
				case 500:
					textView.Text = NSBundle.MainBundle.LocalizedString("txtHelpVariables", "", "");
					imageView.Image = UIImage.FromFile("Images/Help/HelpVariable");
					break;
				case 501:
					textView.Text = NSBundle.MainBundle.LocalizedString("txtHelpFunctions", "", "");
					imageView.Image = UIImage.FromFile("Images/blueEast");
					break;
				case 800:
					textView.Text = NSBundle.MainBundle.LocalizedString("txtHelpMap", "", "");
					imageView.Image = UIImage.FromFile("Images/Help/HelpMap");
					break;
				case 801:
					textView.Text = NSBundle.MainBundle.LocalizedString("txtHelpRoad", "", "");
					imageView.Image = UIImage.FromFile("Images/Help/HelpRoad");
					break;
				case 802:
					textView.Text = NSBundle.MainBundle.LocalizedString("txtHelpShop", "", "");
					imageView.Image = UIImage.FromFile("Images/Help/HelpShop");
					break;
			}

			imageView.Frame = new CGRect(
				5,
				5,
				GlobalSupport.ScreenWidth,
				GlobalSupport.ScreenWidth / 3 * 2);

			textView.Frame = new CGRect(
				0,
				imageView.Frame.Height + 5,
				GlobalSupport.ScreenWidth,
				GlobalSupport.ScreenHeight - imageView.Frame.Height - 65);
		}
	}
}

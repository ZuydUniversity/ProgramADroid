using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Shared;
using CoreGraphics;
using Shared.DataBase;

namespace Mit4RobotApp
{
	partial class VCOptionsMenu : UIViewController
	{
		public VCOptionsMenu(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			lblSlow.TextAlignment = UITextAlignment.Right;
			lblFast.TextAlignment = UITextAlignment.Left;

			GlobalSupport.GameSpeed = 1500;
			lblSpeed.Text = "The gamespeed is: 2 seconds per move.";
			sldrSpeed.Value = 2;

			btnSave.TouchUpInside += (object sender, EventArgs e) =>
			{
				if (GlobalSupport.GameSpeed == 0)
				{
					GlobalSupport.GameSpeed = 500;
					updateDatabase(GlobalSupport.GameSpeed);
				}
				else
				{
					updateDatabase(GlobalSupport.GameSpeed);
				}

				GlobalSupport.ShowPopupMessage("Options saved.");
			};

			sldrSpeed.ValueChanged += (object sender, EventArgs e) =>
			{
				lblSpeed.Text = "The gamespeed is: " + (sender as UISlider).Value.ToString() + " seconds per move.";
				GlobalSupport.GameSpeed = (Int32)(((sender as UISlider).Value * 500) + 500);
			};
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

			foreach (var item in scrollView.Subviews)
			{
				if (item is UILabel)
				{
					item.SizeToFit();
				}
			}

			nfloat yCoordinate = 10;

			lblFast.Frame = new CGRect(
				10,
				yCoordinate,
				lblFast.Frame.Width,
				GlobalSupport.ButtonHeight);

			lblSlow.Frame = new CGRect(
				GlobalSupport.ScreenWidth - lblSlow.Frame.Width - 5,
				yCoordinate,
				lblSlow.Frame.Width,
				GlobalSupport.ButtonHeight);

			sldrSpeed.Frame = new CGRect(
				lblFast.Frame.X + lblFast.Frame.Width + 5,
				yCoordinate,
				GlobalSupport.ScreenWidth - lblFast.Frame.X - lblFast.Frame.Width - lblSlow.Bounds.Width - 15,
				GlobalSupport.ButtonHeight);

			yCoordinate += lblSlow.Frame.Height + 5;

			lblSpeed.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += lblSpeed.Frame.Height + 5;

			btnSave.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);
		}

		private void updateDatabase(int speed)
		{
			DataBase db = DataBase.Instance();
			Settings settings = db.SelectFirst<Settings>();

			if (settings != null)
			{
				settings.GameSpeed = speed;
				db.Update(settings);
			}
			else
			{
				settings = new Settings();
				settings.GameSpeed = speed;
				db.Update(settings);
			}

			GlobalSupport.GameSpeed = settings.GameSpeed;
		}
	}
}

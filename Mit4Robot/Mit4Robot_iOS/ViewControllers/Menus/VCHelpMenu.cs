using System;
using System.CodeDom.Compiler;
using CoreGraphics;
using Foundation;
using Shared;
using Shared.Enums;
using UIKit;

namespace Mit4RobotApp
{
	partial class VCHelpMenu : UIViewController
	{
		public VCHelpMenu(IntPtr handle) : base(handle)
		{
		}

		public VCHelpMenu()
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			btnFunctions.TouchUpInside += (object sender, EventArgs e) =>
			{
				GlobalSupport.MessageIdentifier = 501;
				NavigateToDetails();
			};

			btnGameField.TouchUpInside += (object sender, EventArgs e) =>
			{
				CreateTilesPopUp();
			};

			btnVariables.TouchUpInside += (object sender, EventArgs e) =>
			{
				GlobalSupport.MessageIdentifier = 500;
				NavigateToDetails();
			};

			btnLanguages.TouchUpInside += (object sender, EventArgs e) =>
			{
				if (GlobalSupport.GameLanguage == 0)
				{
					CreateLanguagePopUp();
				}
				else
				{
					GlobalSupport.MessageIdentifier = (int)GlobalSupport.GameLanguage;
					NavigateToDetails();
				}
			};

			btnMainMenu.TouchUpInside += (object sender, EventArgs e) =>
			{
				GlobalSupport.ResetVariables();

				this.NavigationController.PopToRootViewController(true);
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

			foreach (var item in MainView.Subviews)
			{
				if (item is UIButton)
				{
					item.SizeToFit();
				}
			}

			nfloat yCoordinate = 5;

			btnVariables.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnVariables.Frame.Height + 5;

			btnGameField.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnGameField.Frame.Height + 5;

			btnFunctions.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnFunctions.Frame.Height + 5;

			btnLanguages.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnFunctions.Frame.Height * 2 + 5;

			btnMainMenu.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);
		}

		public void CreateTilesPopUp()
		{
			UIActionSheet actionsheet = new UIActionSheet("Selecteer een categorie"){ "Map", "Road", "Shop", "Annuleer" };

			actionsheet.Clicked += (sender, e) =>
			{
				switch (e.ButtonIndex)
				{
					case 0:
						GlobalSupport.MessageIdentifier = 800;
							NavigateToDetails();
						break;
					case 1:
						GlobalSupport.MessageIdentifier = 801;
							NavigateToDetails();
						break;
					case 2:
						GlobalSupport.MessageIdentifier = 802;
							NavigateToDetails();
						break;
				}
			};

			actionsheet.ShowInView(this.View);
		}

		public void CreateLanguagePopUp()
		{
			UIActionSheet actionsheet = new UIActionSheet("Selecteer een taal"){ "Python", "Pascal", "Annuleer" };

			actionsheet.Clicked += (sender, e) =>
			{
				switch (e.ButtonIndex)
				{
					case 0:
						GlobalSupport.MessageIdentifier = 1;
							NavigateToDetails();
						break;
					case 1:
						GlobalSupport.MessageIdentifier = 2;
							NavigateToDetails();
						break;
				}
			};

			actionsheet.ShowInView(this.View);
		}

		public void NavigateToDetails()
		{
			this.NavigationController.PushViewController(new VCHelpDetailsMenu(), true);
		}
	}
}

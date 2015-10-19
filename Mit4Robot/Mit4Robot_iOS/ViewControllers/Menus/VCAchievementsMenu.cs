using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Shared.Achievements;
using Shared;
using CoreGraphics;

namespace Mit4RobotApp
{
	partial class VCAchievementsMenu : UIViewController
	{
		public VCAchievementsMenu(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			AchievementManager achMan = new AchievementManager();

			TBSAchievements source = new TBSAchievements(achMan.GetAchievementsForGUI().ToArray());

			source.OnRowSelected += (object sender, TBSAchievements.RowSelectedEventArgs e) =>
			{
				if (CheckColor(tbAchievements.Source.GetCell(tbAchievements, e.indexPath).BackgroundColor))
				{
					ShareAchievement(tbAchievements.Source.GetCell(tbAchievements, e.indexPath).TextLabel.Text);
				}

				tbAchievements.DeselectRow(e.indexPath, true);
			};

			tbAchievements.Source = source;
			tbAchievements.ReloadData();
		}

		public bool CheckColor(UIColor cellColor)
		{
			UIColor checkColor = GlobalSupport.AchievementUnlocked;

			if (cellColor.ToString() == checkColor.ToString())
			{
				return true;
			}

			else
			{
				return false;
			}
		}

		public void ShareAchievement(string text)
		{
			string toShare = "Look at this achievement from the ProgramADroid app: " + text;

			UIActivityViewController activityShare = new UIActivityViewController(
				                                         new NSObject[]	{ UIActivity.FromObject(toShare) },
				                                         null);
			this.NavigationController.PresentViewController(activityShare, true, null);
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

			tbAchievements.Frame = GlobalSupport.MainCGRect;
		}
	}
}

// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Mit4RobotApp
{
	[Register ("VCAchievementsMenu")]
	partial class VCAchievementsMenu
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView MainView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView scrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView tbAchievements { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (MainView != null) {
				MainView.Dispose ();
				MainView = null;
			}
			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}
			if (tbAchievements != null) {
				tbAchievements.Dispose ();
				tbAchievements = null;
			}
		}
	}
}

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
	[Register ("VCMainMenu")]
	partial class VCMainMenu
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnAchievements { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnCredits { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnHelp { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnHighscores { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnOptions { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnStartGame { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView MainView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView scrollView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnAchievements != null) {
				btnAchievements.Dispose ();
				btnAchievements = null;
			}
			if (btnCredits != null) {
				btnCredits.Dispose ();
				btnCredits = null;
			}
			if (btnHelp != null) {
				btnHelp.Dispose ();
				btnHelp = null;
			}
			if (btnHighscores != null) {
				btnHighscores.Dispose ();
				btnHighscores = null;
			}
			if (btnOptions != null) {
				btnOptions.Dispose ();
				btnOptions = null;
			}
			if (btnStartGame != null) {
				btnStartGame.Dispose ();
				btnStartGame = null;
			}
			if (MainView != null) {
				MainView.Dispose ();
				MainView = null;
			}
			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}
		}
	}
}

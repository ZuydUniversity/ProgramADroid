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
	[Register ("VCGameSetup")]
	partial class VCGameSetup
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnDifficulty { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnLanguage { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnStart { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblGameDetails { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView MainView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView scrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView tbLevelSelect { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnDifficulty != null) {
				btnDifficulty.Dispose ();
				btnDifficulty = null;
			}
			if (btnLanguage != null) {
				btnLanguage.Dispose ();
				btnLanguage = null;
			}
			if (btnStart != null) {
				btnStart.Dispose ();
				btnStart = null;
			}
			if (lblGameDetails != null) {
				lblGameDetails.Dispose ();
				lblGameDetails = null;
			}
			if (MainView != null) {
				MainView.Dispose ();
				MainView = null;
			}
			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}
			if (tbLevelSelect != null) {
				tbLevelSelect.Dispose ();
				tbLevelSelect = null;
			}
		}
	}
}

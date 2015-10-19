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
	[Register ("VCHelpMenu")]
	partial class VCHelpMenu
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnFunctions { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnGameField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnLanguages { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnMainMenu { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnVariables { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView MainView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView scrollView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnFunctions != null) {
				btnFunctions.Dispose ();
				btnFunctions = null;
			}
			if (btnGameField != null) {
				btnGameField.Dispose ();
				btnGameField = null;
			}
			if (btnLanguages != null) {
				btnLanguages.Dispose ();
				btnLanguages = null;
			}
			if (btnMainMenu != null) {
				btnMainMenu.Dispose ();
				btnMainMenu = null;
			}
			if (btnVariables != null) {
				btnVariables.Dispose ();
				btnVariables = null;
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

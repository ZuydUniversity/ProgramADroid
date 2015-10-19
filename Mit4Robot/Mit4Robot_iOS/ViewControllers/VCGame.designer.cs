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
	[Register ("VCGame")]
	partial class VCGame
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnCodeDelete { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnCodeOpen { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnCodeReset { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnCodeSave { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnCodeShare { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnCodeValidate { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnHelp { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnOptions { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView gameField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblError { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblExecutedCode { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblGoals { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView MainView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView scrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextView txtCodeField { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnCodeDelete != null) {
				btnCodeDelete.Dispose ();
				btnCodeDelete = null;
			}
			if (btnCodeOpen != null) {
				btnCodeOpen.Dispose ();
				btnCodeOpen = null;
			}
			if (btnCodeReset != null) {
				btnCodeReset.Dispose ();
				btnCodeReset = null;
			}
			if (btnCodeSave != null) {
				btnCodeSave.Dispose ();
				btnCodeSave = null;
			}
			if (btnCodeShare != null) {
				btnCodeShare.Dispose ();
				btnCodeShare = null;
			}
			if (btnCodeValidate != null) {
				btnCodeValidate.Dispose ();
				btnCodeValidate = null;
			}
			if (btnHelp != null) {
				btnHelp.Dispose ();
				btnHelp = null;
			}
			if (btnOptions != null) {
				btnOptions.Dispose ();
				btnOptions = null;
			}
			if (gameField != null) {
				gameField.Dispose ();
				gameField = null;
			}
			if (lblError != null) {
				lblError.Dispose ();
				lblError = null;
			}
			if (lblExecutedCode != null) {
				lblExecutedCode.Dispose ();
				lblExecutedCode = null;
			}
			if (lblGoals != null) {
				lblGoals.Dispose ();
				lblGoals = null;
			}
			if (MainView != null) {
				MainView.Dispose ();
				MainView = null;
			}
			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}
			if (txtCodeField != null) {
				txtCodeField.Dispose ();
				txtCodeField = null;
			}
		}
	}
}

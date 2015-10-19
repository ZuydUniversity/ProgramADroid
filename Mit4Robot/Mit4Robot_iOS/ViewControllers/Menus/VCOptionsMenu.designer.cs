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
	[Register ("VCOptionsMenu")]
	partial class VCOptionsMenu
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton btnSave { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblFast { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblSlow { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel lblSpeed { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView MainView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIScrollView scrollView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISlider sldrSpeed { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnSave != null) {
				btnSave.Dispose ();
				btnSave = null;
			}
			if (lblFast != null) {
				lblFast.Dispose ();
				lblFast = null;
			}
			if (lblSlow != null) {
				lblSlow.Dispose ();
				lblSlow = null;
			}
			if (lblSpeed != null) {
				lblSpeed.Dispose ();
				lblSpeed = null;
			}
			if (MainView != null) {
				MainView.Dispose ();
				MainView = null;
			}
			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}
			if (sldrSpeed != null) {
				sldrSpeed.Dispose ();
				sldrSpeed = null;
			}
		}
	}
}

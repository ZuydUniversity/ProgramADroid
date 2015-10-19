using System;
using System.Drawing;
using Foundation;
using UIKit;
using Shared;
using CoreGraphics;

namespace Mit4RobotApp
{
	public class KeyboardMoveView
	{
		private nfloat keyboardHeight = 0.0f;

		public UIView ActiveView { get; set; }

		public void RegisterForKeyboardNotifications()
		{ 
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, KeyBoardUpNotification);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidHideNotification, KeyBoardDownNotification);
		}

		private void KeyBoardUpNotification(NSNotification notification)
		{
			CGRect r = UIKeyboard.BoundsFromNotification (notification);

			keyboardHeight = r.Height;

			ScrollTheView (true);
		}

		private void KeyBoardDownNotification(NSNotification notification)
		{
			ScrollTheView (false);
		}

		private void ScrollTheView(bool move)
		{
			UIView.BeginAnimations (string.Empty, System.IntPtr.Zero);
			UIView.SetAnimationDuration (0.3);

			CGRect frame = ActiveView.Frame;

			if (move) 
			{
				frame.Height -= keyboardHeight;
			} 

			else 
			{
				frame.Height = GlobalSupport.ScreenHeight;
			}

			ActiveView.Frame = frame;

			UIView.CommitAnimations();
		}
	}
}


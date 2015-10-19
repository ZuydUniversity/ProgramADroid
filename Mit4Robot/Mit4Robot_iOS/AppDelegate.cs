using Foundation;
using UIKit;
using Shared;
using CoreGraphics;

namespace Mit4RobotApp
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public static UIStoryboard storyboard = UIStoryboard.FromName("StoryboardMain", null);

		public override UIWindow Window 
		{
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			Window = new UIWindow (UIScreen.MainScreen.Bounds);

			Window.RootViewController = storyboard.InstantiateInitialViewController () as UIViewController;

			Window.MakeKeyAndVisible ();

			return true;
		}
	}
}



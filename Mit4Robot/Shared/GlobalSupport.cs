using System;
using Shared.DataBase;
using Shared.Enums;

#if __IOS__
using CoreGraphics;
using UIKit;
#endif

namespace Shared
{
	public partial class GlobalSupport
	{
		public static EDifficulty GameDifficulty { get; set; }

		public static EGameLanguage GameLanguage { get; set; }

		public static EOrientation CurrentRobotOrientation { get; set; }

		public static string GameLevel { get; set; }

		public static int GameSpeed { get; set; }

		public static string LastNameInput { get; set;}

		public static Func<Code,bool> OpenCodeQuery { get { 
				return x => x.LevelName == (GameLevel.Substring (0, GameLevel.LastIndexOf ('.'))) && x.Language == GameLanguage;
			} }

		public static void ResetVariables()
		{
			GameDifficulty = EDifficulty.None;
			GameLanguage = EGameLanguage.None;
			GameLevel = "";
		}

		#if __IOS__
		public static CGRect MainCGRect { get; set; }

		public static UIColor AchievementUnlocked { get { return new UIColor(0, (nfloat)0.75, 0, (nfloat)1.0); } }

		public static int MessageIdentifier { get; set; }
		public static bool EverythingOkay { get; set; }
		public static string ImageIdentifier { get; set; }

		public static nfloat ScreenWidth { get; set; }
		public static nfloat ScreenHeight { get; set; }
		public static nfloat ButtonHeight { get; set; }

		public static CGRect FullWidthCGRect (nfloat yCoordinate)
		{
			return new CGRect (5, yCoordinate, ScreenWidth - 10, ButtonHeight);
		}

		public static void UpdateScreenSize (CGRect rectangle)
		{
			MainCGRect = rectangle;
			ScreenWidth = rectangle.Width;
			ScreenHeight = rectangle.Height;
		}

		public static void ShowPopupMessage (string message)
		{
			UIAlertView alert = new UIAlertView ();
			alert.Title = message;
			alert.AddButton ("Ok");

			alert.Show ();
		}

		public static void ShowPopupMessage (string title, string message)
		{
		UIAlertView alert = new UIAlertView ();
		alert.Title = title;
		alert.Message = message;
		alert.AddButton ("Ok");

		alert.Show ();
		}

		#endif
	}
}


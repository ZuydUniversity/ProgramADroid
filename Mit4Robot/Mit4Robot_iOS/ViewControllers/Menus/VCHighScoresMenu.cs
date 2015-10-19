using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Shared;
using CoreGraphics;
using System.Collections.Generic;
using Shared.DataBase;
using Shared.Enums;
using System.Linq;

namespace Mit4RobotApp
{
	partial class VCHighScoresMenu : UIViewController
	{
		private Dictionary <int, EDifficulty> difficulties;
		private UIActionSheet actionDifficulty;
		private EDifficulty selectedDifficulty;

		public VCHighScoresMenu(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			selectedDifficulty = EDifficulty.Easy;

			#region select difficulty
			difficulties = new Dictionary<int, EDifficulty>();

			actionDifficulty = new UIActionSheet();

			actionDifficulty.Title = "Selecteer de moeilijkheidsgraad";

			int index = 0;

			foreach (var item in Enum.GetValues(typeof(EDifficulty)))
			{
				if ((EDifficulty)item != EDifficulty.Custom && (EDifficulty)item != EDifficulty.None)
				{
					difficulties.Add(index, (EDifficulty)item);

					actionDifficulty.Add(item.ToString());

					index++;
				}
			}

			actionDifficulty.Add("Annuleer");

			actionDifficulty.Clicked += (sender1, e1) =>
			{
				if (e1.ButtonIndex != actionDifficulty.ButtonCount - 1)
				{
					selectedDifficulty = difficulties[(int)e1.ButtonIndex];

					UpdateTableView();
				}
			};
			#endregion

			btnDifficulty.TouchUpInside += (object sender, EventArgs e) =>
			{
				actionDifficulty.ShowInView(scrollView);
			};
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			UpdateGUI();
			UpdateTableView();
		}

		public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate(fromInterfaceOrientation);

			UpdateGUI();
			UpdateTableView();
		}

		public void UpdateGUI()
		{
			GlobalSupport.UpdateScreenSize(new CGRect(
					0, 
					0,
					UIScreen.MainScreen.Bounds.Width,
					UIScreen.MainScreen.Bounds.Height));

			scrollView.Frame = GlobalSupport.MainCGRect;

			nfloat yCoordinate = 5;

			btnDifficulty.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnDifficulty.Frame.Height + 5;

			tbHighscores.Frame = new CGRect(
				5,
				yCoordinate,
				GlobalSupport.ScreenWidth - 10,
				GlobalSupport.ScreenHeight - yCoordinate);
		}

		public void UpdateTableView()
		{
			TBSHighScores source = new TBSHighScores(HighscoreSelectDifficulty(selectedDifficulty.ToString()).ToArray());
			source.OnRowSelected += (object sender, TBSHighScores.RowSelectedEventArgs e) =>
			{
				string lblText = tbHighscores.Source.GetCell(tbHighscores, e.indexPath).TextLabel.Text;
				string[] split = lblText.Split(':');
				if (split[1].Replace(" ", "") == "Nohighscore")
				{
					GlobalSupport.ShowPopupMessage("Sorry, you have to get a highscore to share first!");
				}
				else
				{
					ShareScore(split[0], split[1].Replace(" ", ""));
				}

			};

			tbHighscores.Source = source;
			tbHighscores.ReloadData();
		}

		public void ShareScore(string level, string score)
		{
			string toShare = "I got " + score + " points on " + level + " in the ProgramADroid app. Can you beat me?";

			UIActivityViewController activityShare = new UIActivityViewController(
				                                         new NSObject[]	{ UIActivity.FromObject(toShare) },
				                                         null);
			this.NavigationController.PresentViewController(activityShare, true, null);
		}

		private List<string> HighscoreSelectDifficulty(string difficulty)
		{
			List<string> highscoreStrings = new List<string>();
			DataBase db = DataBase.Instance();
			List<HighScore> highscores = db.SelectAll<HighScore>();

			List<string> levelList = new List<string>();

			int items = 1;
			string toAdd = "";

			switch (selectedDifficulty)
			{
				case EDifficulty.Easy:
					items = 11;
					toAdd = "Easy";
					break;
				case EDifficulty.Medium:
					items = 13;
					toAdd = "Medium";
					break;
				case EDifficulty.Hard:
					items = 12;
					toAdd = "Hard";
					break;
			}

			for (int i = 0; i < items; i++)
			{
				if (i < 9)
				{
					levelList.Add(toAdd + "0" + (i + 1) + ".tmx".ToString());
				}
				else
				{
					levelList.Add(toAdd + (i + 1) + ".tmx".ToString());
				}
			}

			foreach (var item in levelList)
			{
				string levelName = item.Substring (0,item.LastIndexOf("."));

				try
				{

					HighScore highest = highscores.Where(x => x.Level == item).OrderByDescending(x => x.Score).First();
					highscoreStrings.Add(levelName + ": " + highest.Score + " : " + highest.Name + " : " + highest.Date.ToShortDateString());
				}
				catch (Exception)
				{
					// No highscore for this level.
					highscoreStrings.Add(levelName + ": " + "No highscore");
				}
			}

			return highscoreStrings;
		}
	}
}

using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Shared.Enums;
using System.Collections.Generic;
using Shared;
using CoreGraphics;

namespace Mit4RobotApp
{
	partial class VCGameSetup : UIViewController
	{
		private UIActionSheet actionDifficulty, actionLanguage;
		private Dictionary<int, EDifficulty> difficulties;
		private Dictionary<int, EGameLanguage> languages;

		private List<string> levelList;
		private int items;
		private string toAdd;

		public VCGameSetup(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			difficulties = new Dictionary<int, EDifficulty>();
			languages = new Dictionary<int, EGameLanguage>();

			#region difficulty picker
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
					GlobalSupport.GameDifficulty = difficulties[(int)e1.ButtonIndex];

					UpdateGUI();
					UpdateTableView();
				}
			};
			#endregion

			#region language picker
			actionLanguage = new UIActionSheet();

			actionLanguage.Title = "Selecteer de programmeertaal";

			index = 0;

			foreach (var item in Enum.GetValues(typeof(EGameLanguage)))
			{
				if ((EGameLanguage)item != EGameLanguage.None)
				{
					languages.Add(index, (EGameLanguage)item);

					actionLanguage.Add(item.ToString());

					index++;
				}
			}

			actionLanguage.Add("Annuleer");

			actionLanguage.Clicked += (sender1, e1) =>
			{
				if (e1.ButtonIndex != actionLanguage.ButtonCount - 1)
				{
					GlobalSupport.GameLanguage = (EGameLanguage)(languages[(int)e1.ButtonIndex]);

					UpdateGUI();
				}
			};
			#endregion
	
			btnDifficulty.TouchUpInside += (object sender, EventArgs e) =>
			{	
				actionDifficulty.ShowInView(scrollView);

				GlobalSupport.GameDifficulty = EDifficulty.None;
				GlobalSupport.GameLevel = "";
			};

			btnLanguage.TouchUpInside += (object sender, EventArgs e) =>
			{
				actionLanguage.ShowInView(scrollView);
			};

			btnStart.TouchUpInside += (object sender, EventArgs e) =>
			{
				if (GlobalSupport.GameDifficulty != EDifficulty.Custom && GlobalSupport.GameDifficulty != EDifficulty.None)
				{
					if (GlobalSupport.GameLanguage != EGameLanguage.None)
					{
						if (!String.IsNullOrEmpty(GlobalSupport.GameLevel))
						{
							GlobalSupport.EverythingOkay = true;
						}
						else
						{					
							GlobalSupport.ShowPopupMessage("Selecteer een level");
							GlobalSupport.EverythingOkay = false;
						}
					}
					else
					{
						GlobalSupport.ShowPopupMessage("Selecteer een taal");
						GlobalSupport.EverythingOkay = false;
					}
				}
				else
				{
					GlobalSupport.ShowPopupMessage("Selecteer een moeilijkheidsgraad");
					GlobalSupport.EverythingOkay = false;
				}
			};
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			GlobalSupport.GameLanguage = EGameLanguage.Python;
			GlobalSupport.GameDifficulty = EDifficulty.Easy;

			UpdateGUI();
			UpdateTableView();
		}

		public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate(fromInterfaceOrientation);

			UpdateGUI();
		}

		public void UpdateGUI()
		{
			GlobalSupport.UpdateScreenSize(new CGRect(
					0, 
					0,
					UIScreen.MainScreen.Bounds.Width,
					UIScreen.MainScreen.Bounds.Height));

			scrollView.Frame = GlobalSupport.MainCGRect;

			foreach (var item in scrollView.Subviews)
			{
				if (item is UIButton)
				{
					item.SizeToFit();
				}
			}

			nfloat yCoordinate = 5;

			btnDifficulty.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnDifficulty.Frame.Height + 5;

			btnLanguage.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnLanguage.Frame.Height + 5;

			lblGameDetails.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += lblGameDetails.Frame.Height + 5;

			string gameDetails = "";

			if (GlobalSupport.GameLanguage != 0)
			{
				gameDetails += GlobalSupport.GameLanguage.ToString();
				gameDetails += ": ";
			}

			if (GlobalSupport.GameDifficulty != EDifficulty.None && GlobalSupport.GameDifficulty != EDifficulty.Custom)
			{
				gameDetails += GlobalSupport.GameDifficulty.ToString();
				gameDetails += ": ";
			}

			if (!String.IsNullOrEmpty(GlobalSupport.GameLevel))
			{
				gameDetails += GlobalSupport.GameLevel.Split('.')[0];
			}

			lblGameDetails.Text = gameDetails;

			tbLevelSelect.Frame = new CGRect(
				0,
				lblGameDetails.Frame.Y + lblGameDetails.Frame.Height + 5,
				GlobalSupport.ScreenWidth,
				GlobalSupport.ScreenHeight - (lblGameDetails.Frame.Y + lblGameDetails.Frame.Height + 5));

			this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem(btnStart), true);
		}

		public void UpdateTableView()
		{
			levelList = new List<string>();

			items = 1;
			toAdd = "";

			switch (GlobalSupport.GameDifficulty)
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
					levelList.Add(toAdd + "0" + (i + 1).ToString());
				}
				else
				{
					levelList.Add(toAdd + (i + 1).ToString());
				}
			}

			TBSSelectLevelSource source = new TBSSelectLevelSource(levelList.ToArray());
			source.OnRowSelected += (object sender, TBSSelectLevelSource.RowSelectedEventArgs e) =>
			{
				string levelName = levelList[e.indexPath.Row];
				GlobalSupport.GameLevel = levelName + ".tmx";

				UpdateGUI();
			};

			tbLevelSelect.Source = source;
			tbLevelSelect.ReloadData();
		}
	}
}

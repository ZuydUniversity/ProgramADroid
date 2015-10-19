using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;

using Shared;
using Shared.BusinessLayer;
using Shared.CustomEventArgs;
using Shared.DataBase;
using Shared.Enums;
using Shared.Exceptions;
using Shared.Parsers.CodeParsers;
using Shared.Parsers.MapParsers;
using Shared.Achievements;

using System.Linq;
namespace Mit4RobotApp
{
	partial class VCGame : UIViewController
	{
		private KeyboardMoveView keyboardMoveView;
		private DataBase codeDatabase;
		private Robot robot;
		private Map level;
		private AchievementManager achievementManager;

		private UIImageView robotImage;

		private int defRobotX, defRobotY;

		public VCGame()
		{
		}

		public VCGame(IntPtr handle) : base(handle)
		{
		}

		/// <summary>
		/// Fetched the gamespeed from DB and updates the globalsupport
		/// </summary>
		private void SetGameSpeed()
		{
			Settings settings = codeDatabase.SelectFirst<Settings>();
			if (settings == null)
			{
				settings = new Settings();
				settings.GameSpeed = 1000;

				codeDatabase.Insert(settings);
			}
			GlobalSupport.GameSpeed = settings.GameSpeed;
		}


		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (GlobalSupport.EverythingOkay)
			{
				codeDatabase = DataBase.Instance();

				SetGameSpeed();

				robot = TiledParser.Parse(File.Open("Maps/" + GlobalSupport.GameDifficulty.ToString() + "/" + GlobalSupport.GameLevel,
					                                   FileMode.Open,
					                                   FileAccess.Read),
				                          GlobalSupport.GameLevel);

				achievementManager = new AchievementManager();
				achievementManager.AchievementUnlocked += (sender, e) =>
				{
					if (e is AchievementEventArgs)
					{
						var achievementString = (e as AchievementEventArgs).data.message;
						GlobalSupport.ShowPopupMessage("Achievement Unlocked", achievementString);
					}
				};

				if (GlobalSupport.GameLanguage == EGameLanguage.Pascal)
				{
					achievementManager.RegisterEvent(EAchievementType.StartPascal, 0);
				}
				else if (GlobalSupport.GameLanguage == EGameLanguage.Python)
				{
					achievementManager.RegisterEvent(EAchievementType.StartPython, 0);
				}

				level = robot.level;
				defRobotX = robot.xPosition;
				defRobotY = robot.yPosition;

				Robot.UpdateRobotEvent += UpdateRobot;
				Robot.checkGoalsEvent += (object sender, EventArgs e) =>
				{
					CheckGoals(sender,e);
				};

				keyboardMoveView = new KeyboardMoveView();
				keyboardMoveView.RegisterForKeyboardNotifications();
				keyboardMoveView.ActiveView = txtCodeField;

				this.View.AddGestureRecognizer(new UITapGestureRecognizer(tap =>
						{
							if (!txtCodeField.Frame.Contains(tap.LocationInView(this.View)))
							{	
								if (txtCodeField.IsFirstResponder)
								{
									txtCodeField.ResignFirstResponder();	
								}
							}
						}));

				btnCodeSave.TouchUpInside += (object sender, EventArgs e) =>
				{
					if (!String.IsNullOrEmpty(txtCodeField.Text))
					{
						SaveCode();

					//	GlobalSupport.ShowPopupMessage("Code succesvol opgeslagen");
					}
				};

				btnCodeOpen.TouchUpInside += (object sender, EventArgs e) =>
				{
					OpenCode();
				};

				btnCodeShare.TouchUpInside += (object sender, EventArgs e) =>
				{
					ShareCode();
				};

				btnCodeDelete.TouchUpInside += (object sender, EventArgs e) =>
				{
					DeleteCode();
				};

				btnCodeValidate.TouchUpInside += (object sender, EventArgs e) =>
				{
					ValidateCode();
				};

				btnCodeReset.TouchUpInside += (object sender, EventArgs e) =>
				{
					ResetCode(true);
				};

				ResetCode(true);
			}
			else
			{
				this.NavigationController.PopViewController(true);
			}
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			if (!String.IsNullOrEmpty(GlobalSupport.GameLevel)){
				string levelName = GlobalSupport.GameLevel.Substring (0, GlobalSupport.GameLevel.LastIndexOf ('.'));
				this.Title = "Level: " + levelName + "; Lang: " + GlobalSupport.GameLanguage.ToString();
			}

			UpdateGUI();
		}

		public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
		{
			base.DidRotate(fromInterfaceOrientation);

			if (GlobalSupport.EverythingOkay)
			{
				UpdateGUI();
			}
			else
			{
				this.NavigationController.PopViewController(true);
			}
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
				
			nfloat xCoordinate = (nfloat)5;
			nfloat yCoordinate = 5;

			btnOptions.Frame = new CGRect(
			xCoordinate,
			yCoordinate,
			(GlobalSupport.ScreenWidth - 20) / 3,
			GlobalSupport.ButtonHeight);

			btnHelp.Frame = new CGRect(
			(GlobalSupport.ScreenWidth - 20) / 3 * 2 + 15,
			yCoordinate,
			(GlobalSupport.ScreenWidth - 20) / 3,
			GlobalSupport.ButtonHeight);

			yCoordinate += btnOptions.Frame.Height + 5;

			btnCodeOpen.Frame = new CGRect(
				xCoordinate, 
				yCoordinate,
				(GlobalSupport.ScreenWidth - 20) / 3,
				GlobalSupport.ButtonHeight);

			xCoordinate += btnCodeOpen.Frame.Width + 5;

			btnCodeSave.Frame = new CGRect(
				xCoordinate, 
				yCoordinate,
				(GlobalSupport.ScreenWidth - 20) / 3,
				GlobalSupport.ButtonHeight);

			xCoordinate += btnCodeSave.Frame.Width + 5;

			btnCodeDelete.Frame = new CGRect(
				xCoordinate, 
				yCoordinate,
				(GlobalSupport.ScreenWidth - 20) / 3,
				GlobalSupport.ButtonHeight);

			yCoordinate += btnCodeOpen.Frame.Height + 5;

			btnCodeShare.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += btnCodeShare.Frame.Height + 5;

			lblGoals.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += lblGoals.Frame.Height + 5;

			gameField.Frame = new CGRect(
				5, 
				yCoordinate, 
				GlobalSupport.ScreenWidth - 10,
				GlobalSupport.ScreenWidth - 10);

			yCoordinate += gameField.Frame.Height + 5;

			btnCodeValidate.Frame = new CGRect(
				5,
				yCoordinate,
				GlobalSupport.ScreenWidth / 2 - 10,
				GlobalSupport.ButtonHeight);

			btnCodeReset.Frame = new CGRect(
				btnCodeValidate.Frame.X + btnCodeValidate.Frame.Width + 10,
				yCoordinate,
				GlobalSupport.ScreenWidth / 2 - 10,
				GlobalSupport.ButtonHeight);

			yCoordinate += btnCodeValidate.Frame.Height + 5;

			lblExecutedCode.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += lblExecutedCode.Frame.Height + 5;

			lblError.Frame = GlobalSupport.FullWidthCGRect(yCoordinate);

			yCoordinate += lblError.Frame.Height + 5;

			txtCodeField.Frame = new CGRect(
				5,
				yCoordinate,
				GlobalSupport.ScreenWidth - 10,
				GlobalSupport.ScreenWidth - 10);

			if (GlobalSupport.EverythingOkay)
			{
				scrollView.ContentSize = new CGSize(GlobalSupport.ScreenWidth,
				                                    txtCodeField.Frame.Y + txtCodeField.Frame.Height);

				DrawMap();
			}
			else
			{
				this.NavigationController.PopViewController(true);
			}
		}

		public void UpdateRobot(object sender, EventArgs e)
		{
			int lineNumber = -1;

			if (e is HighlightCodeEventArgs)
			{
				lineNumber = (e as HighlightCodeEventArgs).lineNumber;

				var commandExecuted = CommandExcecuted(lineNumber, txtCodeField.Text);

				lblExecutedCode.Text = lineNumber.ToString() + " " + commandExecuted.Trim();
			}

			UpdateRobot();
		}

		public void CheckGoals(object sender, EventArgs e)
		{
			Exception ex = (e as System.ComponentModel.RunWorkerCompletedEventArgs).Error;

			if (ex != null)
			{
				SetErrorLabelMessage(ex.Message);
			}	

			CheckGoals();
		}

		public void CheckGoals()
		{
			var goalsCompleted = robot.checkGoals();
		
			if (goalsCompleted)
			{
				lblGoals.Text = "Level completed";

				int total = (int)txtCodeField.ContentSize.Height;
				int lineHeight = (int)txtCodeField.Font.LineHeight;
				int linesOfCode = total / lineHeight;

				int score = robot.CalculateHighscore(robot.Moves, robot.level.MinMoves, linesOfCode, robot.level.MinLines);
				ShowScorePopUp(score);
				achievementManager.RegisterEvent(EAchievementType.FirstLevelCompleted, 0);
				achievementManager.RegisterEvent(EAchievementType.Score1000, score);
				achievementManager.RegisterEvent(EAchievementType.Score10000, score);
			}
			else
			{
				lblGoals.Text = "Level not completed";
			}
		}

		public void ShowScorePopUp(int score)
		{

			//Create Alert
			var textInputAlertController = UIAlertController.Create("Save Your Score", String.Format("You have scored {0} points",score), UIAlertControllerStyle.Alert);

			//Add Text Input
			textInputAlertController.AddTextField(textField => {
				textField.Placeholder = "Enter Your Name";
				textField.Text = GlobalSupport.LastNameInput;
			});

			//Add Actions
			var cancelAction = UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, alertAction => Console.WriteLine ("Cancel was Pressed"));
			var okayAction = UIAlertAction.Create ("Save !", UIAlertActionStyle.Default, alertAction => {
				Console.WriteLine ("The user entered '{0}'", textInputAlertController.TextFields[0].Text);
				GlobalSupport.LastNameInput = textInputAlertController.TextFields[0].Text;
				robot.SaveHighscore(score, textInputAlertController.TextFields[0].Text);
			});

			textInputAlertController.AddAction(cancelAction);
			textInputAlertController.AddAction(okayAction);

			//Present Alert
			PresentViewController(textInputAlertController, true, null);
				

			//GlobalSupport.ShowPopupMessage("HighScore", "You have passed a level with a score of: " + score);
			//UIAlertView alterView = new UIAlertView ("HighScore", "You have passed a level with a score of: " + score, null, "OK!", null);
			//alterView.Show ();
		}

		public void SaveCode()
		{

			//Create Alert
			var textInputAlertController = UIAlertController.Create("Save Your Code", "Fill Out These Fields", UIAlertControllerStyle.Alert);

			//Add Text Input
			textInputAlertController.AddTextField(textField => {
				textField.Placeholder = "Enter File Name";
			});

			//Add Text Input
			textInputAlertController.AddTextField(textField => {
				textField.Placeholder = "Enter Your Name";
			});

			//Add Actions
			var cancelAction = UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, alertAction => Console.WriteLine ("Cancel was Pressed"));
			var okayAction = UIAlertAction.Create ("Save !", UIAlertActionStyle.Default, alertAction => {
				Console.WriteLine ("The user entered '{0}'", textInputAlertController.TextFields[0].Text);
				GlobalSupport.LastNameInput = textInputAlertController.TextFields[1].Text;
				Code code = new Code();
				code.CodeString = txtCodeField.Text;
				code.Author = textInputAlertController.TextFields[1].Text;
				code.FileName = textInputAlertController.TextFields[0].Text;
				code.Date = DateTime.Now;
				code.LevelName = GlobalSupport.GameLevel.Substring(0,GlobalSupport.GameLevel.LastIndexOf('.'));
				code.Language = GlobalSupport.GameLanguage;

				codeDatabase.Insert(code);
			});

			textInputAlertController.AddAction(cancelAction);
			textInputAlertController.AddAction(okayAction);

			//Present Alert
			PresentViewController(textInputAlertController, true, null);

		}

		public void OpenCode()
		{
			UIActionSheet actionSheet = new UIActionSheet("Selecteer Code");

			Dictionary<int, int> codeIDDictionary = new Dictionary<int, int>();

			foreach (var item in codeDatabase.SelectAll<Code> ().Where (GlobalSupport.OpenCodeQuery).ToList())
			{
				codeIDDictionary.Add((int)actionSheet.ButtonCount, item.ID);

				actionSheet.Add(String.Format("{0} {1} {2}",item.ID,item.FileName,item.Author));
			}


			actionSheet.Clicked += (sender, e) =>
			{
				if (e.ButtonIndex != actionSheet.ButtonCount - 1)
				{
					ResetCode(true);

					txtCodeField.Text = codeDatabase.SelectById<Code>(codeIDDictionary[(int)e.ButtonIndex]).CodeString.ToString();
				}
			};

			actionSheet.AddButton ("Cancel");
			actionSheet.CancelButtonIndex = actionSheet.ButtonCount;

			actionSheet.ShowInView(this.View);
		}

		public void ShareCode()
		{

			string commentNotice = "";
						switch (GlobalSupport.GameLanguage) {
						case EGameLanguage.Python:
					commentNotice = String.Format("# Look at my new ZUYD Robot code!\n # Shared By {0} on {1} \n\n", GlobalSupport.LastNameInput, DateTime.Now.ToShortDateString());
							break;
			case EGameLanguage.Pascal:
				commentNotice = "{ Look at my new ZUYD Robot code! }\n" + "{Shared by" + GlobalSupport.LastNameInput + " on " + DateTime.Now.ToShortDateString() + "} \n\n";

							break;
						default:
							break;
						}
			UIActivityViewController activityShare = new UIActivityViewController(
				                                         new NSObject[]	{ UIActivity.FromObject(commentNotice + txtCodeField.Text) },
				                                         null);
			this.NavigationController.PresentViewController(activityShare, true, null);

		}

		public void DeleteCode()
		{
			UIActionSheet actionSheet = new UIActionSheet("Selecteer Code");

			foreach (var item in codeDatabase.SelectAll<Code> ().Where (GlobalSupport.OpenCodeQuery).ToList())
			{
				actionSheet.Add(String.Format("{0} {1} {2}",item.ID,item.FileName,item.Author));
			}

			actionSheet.Add("Annuleer");

			actionSheet.Clicked += (sender, e) =>
			{
				if (e.ButtonIndex != actionSheet.ButtonCount - 1)
				{
					codeDatabase.DeleteById<Code>(Convert.ToInt32(actionSheet.ButtonTitle(e.ButtonIndex).Split(' ')[0]));
				}
			};
			actionSheet.CancelButtonIndex = actionSheet.ButtonCount;
			actionSheet.ShowInView(this.View);
		}

		public void ResetCode(bool resetRobot)
		{
			FunctionBlockList.resetList();
			lblGoals.Text = robot.getGoalsInString();
			robot.goals = robot.getGoals();
			robot.level.shopsAndInventory = new Dictionary<EShops, List<string>>();
			robot.level.GenerateShops();
			robot.inventory = new List<string>();
		
			robot.xPosition = defRobotX;
			robot.yPosition = defRobotY;
			robot.orientationEnum = EOrientation.East;

			GlobalSupport.CurrentRobotOrientation = EOrientation.East;


			lblError.Text = "";

			if (resetRobot)
			{
				lblExecutedCode.Text = "";
				UpdateRobot();	
			}
		}

		public void UpdateRobot()
		{
			if (gameField.Subviews.GetLength(0) != 0)
			{
				if (robotImage != null)
				{
					robotImage.Frame = (gameField.Subviews[level.map.GetLength(0) * robot.yPosition + robot.xPosition] as UIImageView).Frame;
				}
				else
				{
					robotImage = new UIImageView((gameField.Subviews[level.map.GetLength(0) * robot.yPosition + robot.xPosition] as UIImageView).Frame);
						
					robotImage.Image = GetImageForRobot();
					gameField.AddSubview(robotImage);
				}

				robotImage.Image = GetImageForRobot();
			}	
		}

		public void DrawMap()
		{
			float height = (float)(gameField.Frame.Height / level.map.GetLength(1));
			UIImageView imageView = null;

			foreach (UIImageView item in gameField.Subviews)
			{
				item.RemoveFromSuperview();
			}

			if (robotImage != null)
			{
				robotImage.RemoveFromSuperview();
				robotImage = null;
			}

			for (int yCoordinate = 0; yCoordinate < level.map.GetLength(0); yCoordinate++)
			{
				for (int xCoordinate = 0; xCoordinate < level.map.GetLength(1); xCoordinate++)
				{
					if (xCoordinate == 0)
					{
						if (yCoordinate == 0)
						{
							imageView = new UIImageView(
								new CGRect(
									(nfloat)0, 
									(nfloat)0, 
									(nfloat)(gameField.Frame.Width / level.map.GetLength(1)), 
									(nfloat)(gameField.Frame.Height / level.map.GetLength(0))));
						}
						else
						{
							imageView = new UIImageView(
								new CGRect(
									(nfloat)0, 
									(nfloat)(height * yCoordinate), 
									(nfloat)(gameField.Frame.Width / level.map.GetLength(1)), 
									(nfloat)(gameField.Frame.Height / level.map.GetLength(0))));
						}
					}
					else
					{
						imageView = new UIImageView(
							new CGRect(
								(nfloat)gameField.Subviews[xCoordinate - 1].Frame.X + gameField.Subviews[xCoordinate - 1].Frame.Width, 
								(nfloat)(height * yCoordinate),
								(nfloat)(gameField.Frame.Width / level.map.GetLength(1)), 
								(nfloat)(gameField.Frame.Height / level.map.GetLength(0))));
					}

					imageView.Image = GetImageForTile(level, xCoordinate, yCoordinate);

					gameField.AddSubview(imageView);
				}
			}

			DrawSpawn();
			UpdateRobot();
		}

		public void DrawSpawn()
		{
			for (int yCoordinate = 0; yCoordinate < level.map.GetLength(0); yCoordinate++)
			{
				for (int xCoordinate = 0; xCoordinate < level.map.GetLength(1); xCoordinate++)
				{
					if (xCoordinate == robot.level.spawn.x && yCoordinate == robot.level.spawn.y)
					{
						(gameField.Subviews[(yCoordinate * level.map.GetLength(0)) + xCoordinate] as UIImageView).AddSubview(new UIImageView(UIImage.FromFile("Images/MapTiles/81.png")));
					}
				}
			}
		}

		public UIImage GetImageForTile(Map map, int xCoordinate, int yCoordinate)
		{
			if (level.map[yCoordinate, xCoordinate].TileId > 80)
			{
				return UIImage.FromFile("Images/MapTiles/29.png");
			}
			else
			{
				return UIImage.FromFile("Images/MapTiles/" + level.map[yCoordinate, xCoordinate].TileId.ToString() + ".png");
			}
		}

		public UIImage GetImageForRobot()
		{
			switch (GlobalSupport.CurrentRobotOrientation)
			{
				case EOrientation.North:
					return UIImage.FromFile("Images/blueNorth.png");
				case EOrientation.East:
					return UIImage.FromFile("Images/blueEast.png");
				case EOrientation.South:
					return UIImage.FromFile("Images/blueSouth.png");
				case EOrientation.West:
					return UIImage.FromFile("Images/blueWest.png");
				default:
					return UIImage.FromFile("Images/blueEast.png");
			}
		}

		public void highLightCode(object sender, EventArgs e)
		{
			int linenumber = 0;
		
			if (sender is Command)
			{
				linenumber = (sender as Command).LineNumber;
			}
			else if (sender is Composite)
			{
				linenumber = (sender as Composite).LineNumber;
			}

			var commandExecuted = CommandExcecuted(linenumber, txtCodeField.Text);
			lblExecutedCode.Text = linenumber.ToString() + " " + commandExecuted.Trim();
		}

		public void ValidateCode()
		{
			try
			{
				ResetCode(true);

				CodeParser codeParser = null;
				robot.Moves = 0;

				switch (GlobalSupport.GameLanguage)
				{
					case EGameLanguage.Python:
						codeParser = new PythonParser();
						break;
				
					case EGameLanguage.Pascal:
						codeParser = new PascalParser();
						break;
					default:
						codeParser = new PascalParser();
						break;
				}

				string codeToParse = txtCodeField.Text;

				codeToParse = RemoveWhiteSpaces(codeToParse);
				codeToParse += "\n";

				List<ICodeBlock> result = codeParser.ParseCode(codeToParse);

				ExecuteCommand(result, txtCodeField.Text, lblExecutedCode);
			}
			catch (RobotException excRobot)
			{
				SetErrorLabelMessage(excRobot.Message);
			}
			catch (CodeParseException excCodeParse)
			{
				SetErrorLabelMessage(excCodeParse.Message);
			}
			catch (SyntaxParseException excSyntax)
			{
				SetErrorLabelMessage(excSyntax.Message);
			}
			catch (MapException excMap)
			{
				SetErrorLabelMessage(excMap.Message);
			}
			catch (ArgumentOutOfRangeException excArgOOR)
			{
				SetErrorLabelMessage(excArgOOR.Message);
			}
			catch (RunTimeException exRunTimeException){
				SetErrorLabelMessage (exRunTimeException.Message);
			}
		}

		public void ExecuteCommand(List<ICodeBlock> command, string text, UILabel lblExecutedCode)
		{
			try
			{
				MainCode m = new MainCode();

				foreach (var item in command)
				{
					m.addChild(item);
				}

				m.execute(null);
	
				lblExecutedCode.Text = "Code completed";
			}
			catch (RobotException ex)
			{
				throw ex;
			}
			catch (MapException ex)
			{
				throw ex;
			}
		}

		private static string CommandExcecuted(int lineNumber, string text)
		{
			try{
			string[] stringSeperator = new string[]{ "\n" };
			string[] lines = text.Split(stringSeperator, StringSplitOptions.None);
			if (lineNumber < 1)
			{
				lineNumber = 1;
			}
			return lines[lineNumber - 1];
			} catch(IndexOutOfRangeException ex){
				throw ex;
			}
		}

		#region utilities
		public string RemoveWhiteSpaces(string codeToParse)
		{
			return Regex.Replace(codeToParse, " ", "\t");
		}

		public void SetErrorLabelMessage(string message)
		{
			lblError.Text = message;
		}

		public void OpenHelp()
		{
			this.NavigationController.PushViewController(new VCHelpMenu((IntPtr)0x37686db0), true);
		}
		#endregion
	}
}

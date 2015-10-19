using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Graphics.Drawables;

using Shared;
using System.Threading.Tasks;
using System.Threading;
using Shared.Parsers.CodeParsers;
using Shared.Parsers.MapParsers;
using Shared.DataBase;
using Shared.Achievements;
using Shared.BusinessLayer;
using Shared.CustomEventArgs;
using Shared.Exceptions;
using Shared.Enums;
using Android.Util;

namespace Mit4Robot_Android
{
	[Activity(Label = "MIT4 Robot")]			
	public class ActivityGame : Activity
	{
		private DataBase codeDatabase;
		private Robot robot;
		private AchievementManager _achievementManager;

		private AlertDialog alert;

		private PopupMenu openCodePopUp, deleteCodePopUp;
		private Button btnOpenCode, btnSaveCode, btnDeleteCode, btnShareCode, btnValidate, btnReset, btnOptions;
		private ImageView imgGameMap;
		private EditText txtCodeField;
		private TextView lblError, txtHelp, lblGoals;
		private static TextView lblExecutedCode;

		private MapRenderer mapRenderer;

		private int defRobotX, defRobotY;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			/*SetContentView (new MapRenderer (this));*/
			alert = new AlertDialog.Builder(this).Create();

			codeDatabase = DataBase.Instance();

			DisplayMetrics metrics = Resources.DisplayMetrics;

			SetContentView(Resource.Layout.Game);
			this.Title = "Level: " + GlobalSupport.GameLevel.Remove(GlobalSupport.GameLevel.Length - 4, 4) + "; Lang: " + GlobalSupport.GameLanguage.ToString();

			SetGameSpeed();
			mapRenderer = new MapRenderer();
			robot = TiledParser.Parse(Assets.Open("Maps/" + GlobalSupport.GameDifficulty.ToString() + "/" + GlobalSupport.GameLevel),
			                          GlobalSupport.GameLevel);
			defRobotX = robot.xPosition;
			defRobotY = robot.yPosition;

			Robot.UpdateRobotEvent += new EventHandler(UpdateRobot);
			Robot.checkGoalsEvent += new EventHandler(CheckGoals);

			imgGameMap = FindViewById<ImageView>(Resource.Id.imgGameMap);

			imgGameMap.SetMinimumWidth(metrics.WidthPixels);
			imgGameMap.SetMinimumHeight(metrics.WidthPixels);

			imgGameMap.SetImageDrawable(new BitmapDrawable(Resources,
				                                              mapRenderer.Render(robot.level,
				                                                                   Resources,
				                                                                   robot.xPosition,
				                                                                   robot.yPosition)));

			txtCodeField = FindViewById<EditText>(Resource.Id.txtGameCodeInput);
			lblError = FindViewById<TextView>(Resource.Id.txtGameError);
			lblExecutedCode = FindViewById<TextView>(Resource.Id.btnGameCodeExecute);
			lblGoals = FindViewById<TextView>(Resource.Id.txtGoals);
			btnValidate = FindViewById<Button>(Resource.Id.btnGameCodeValidate);
			btnOptions = FindViewById<Button> (Resource.Id.btnGameOptions);

			_achievementManager = new AchievementManager();
			_achievementManager.AchievementUnlocked += (sender, e) =>
			{ 
				if (e is AchievementEventArgs)
				{
					var achievementString = (e as AchievementEventArgs).data.message;
					ShowPopUpMessage(achievementString);
				}
			};
			if (GlobalSupport.GameLanguage == EGameLanguage.Pascal)
			{
				_achievementManager.RegisterEvent(EAchievementType.StartPascal, 0);
			}
			else if (GlobalSupport.GameLanguage == EGameLanguage.Python)
			{
				_achievementManager.RegisterEvent(EAchievementType.StartPython, 0);
			}

			btnValidate.Click += (object sender, EventArgs e) =>
			{
				ValidateCode(); 
			};

			btnOptions.Click += (object sender, EventArgs e) =>
			{
				var optionsMenu = new Intent(this, typeof(ActivityOptions));
				StartActivity(optionsMenu);
			};

			btnReset = FindViewById<Button>(Resource.Id.btnGameCodeReset);

			btnReset.Click += (object sender, EventArgs e) =>
			{
				ResetCode(true);
			};

			btnSaveCode = FindViewById<Button>(Resource.Id.btnGameCodeSave);

			btnSaveCode.Click += (object sender, EventArgs e) =>
			{
				SaveCode();
			};

			btnOpenCode = FindViewById<Button>(Resource.Id.btnGameCodeOpen);

			btnOpenCode.Click += (object sender, EventArgs e) =>
			{
				OpenCode();
			};

			btnShareCode = FindViewById<Button>(Resource.Id.btnGameCodeShare);

			btnShareCode.Click += (object sender, EventArgs e) =>
			{
				ShareCode();
			};

			btnDeleteCode = FindViewById<Button>(Resource.Id.btnGameCodeDelete);

			btnDeleteCode.Click += (object sender, EventArgs e) =>
			{
				DeleteCode();
			};

			txtHelp = FindViewById<TextView>(Resource.Id.txtGameHelp);

			txtHelp.Click += (sender, e) =>
			{
				OpenHelp();
			};



			ResetCode(true);
			SetGoals();
		}

		/// <summary>
		/// Fetched the gamespeed from DB and updates the globalsupport
		/// </summary>
		private void SetGameSpeed()
		{
			DataBase db = DataBase.Instance();
			Settings settings = db.SelectFirst<Settings>();
			if (settings == null)
			{
				settings = new Settings();
				settings.GameSpeed = 1000;
				db.Insert(settings);
			}
			GlobalSupport.GameSpeed = settings.GameSpeed;
		}

		public void UpdateRobot(object sender, EventArgs e)
		{
			try
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
			catch (IndexOutOfRangeException ex)
			{
				SetErrorLabelMessage(ex.ToString());
			}
		}

		public void UpdateRobot()
		{
			imgGameMap.SetImageDrawable(new BitmapDrawable(Resources,
				                                              mapRenderer.Render(robot.level,
				                                                                   Resources,
				                                                                   robot.xPosition,
				                                                                   robot.yPosition)));
		}

		public void SaveCode()
		{
			int getLastID = codeDatabase.SelectAll<Code>().Count + 1;

			LinearLayout layout = new LinearLayout(this);
			layout.Orientation = Orientation.Vertical;

			EditText inputFileName = new EditText(this);
			inputFileName.Hint = "FileName";
			layout.AddView (inputFileName);

			EditText inputAuthor = new EditText(this);
			inputAuthor.Hint = "Author";
			layout.AddView (inputAuthor);

			inputAuthor.Text = GlobalSupport.LastNameInput;
			AlertDialog ad = new AlertDialog.Builder (this).Create();
			ad.SetTitle ("Enter name");
			ad.SetMessage ("Please enter a file name and your name.");
			ad.SetView (layout);
			ad.SetButton ("Save",(senderAlert, args) => {
				//Save File
				GlobalSupport.LastNameInput = inputAuthor.Text;
				Code code = new Code();
				code.CodeString = txtCodeField.Text;
				code.Author = inputAuthor.Text;
				code.FileName = inputFileName.Text;
				code.Date = DateTime.Now;
				code.LevelName = GlobalSupport.GameLevel.Substring(0,GlobalSupport.GameLevel.LastIndexOf('.'));
				code.Language = GlobalSupport.GameLanguage;

				codeDatabase.Insert(code);
			});

			ad.SetButton2 ("Cancel", (senderAlert, args) => {
				// cancels (Do nothing)
			});

			ad.Show ();


		}

		public void OpenCode()
		{
			openCodePopUp = new PopupMenu(this, btnOpenCode);

			List<Code> selected = codeDatabase.SelectAll<Code> ().Where (GlobalSupport.OpenCodeQuery).ToList();
			foreach (var item in selected)
			{
				openCodePopUp.Menu.Add(item.ID, item.ID, item.ID, item.ID + ": " + item.FileName + " (Author: " + item.Author + ")");
			}

			openCodePopUp.MenuItemClick += (sender, e) =>
			{
				ResetCode(true);

				txtCodeField.Text = codeDatabase.SelectById<Code>(e.Item.ItemId).CodeString.ToString();
			};

			openCodePopUp.Show();
		}

		public void ShareCode()
		{
			Intent sendIntent = new Intent();
			sendIntent.SetAction(Intent.ActionSend);
			string commentNotice = "";
			switch (GlobalSupport.GameLanguage) {
			case EGameLanguage.Python:
				commentNotice = String.Format("# Look at my new ZUYD Robot code!\n # Shared By {0} on {1}\n\n", GlobalSupport.LastNameInput, DateTime.Now.ToShortDateString());
				break;
			case EGameLanguage.Pascal:
				commentNotice = "{ Look at my new ZUYD Robot code! }\n" + "{Shared by" + GlobalSupport.LastNameInput + " on " + DateTime.Now.ToShortDateString() + "}\n\n";

				break;
			default:
				break;
			}
			sendIntent.PutExtra(Intent.ExtraText, commentNotice + txtCodeField.Text.ToString());
			sendIntent.SetType("text/plain");
			StartActivity(sendIntent);
		}

		public void DeleteCode()
		{
			try
			{
				deleteCodePopUp = new PopupMenu(this, btnDeleteCode);

				//savedCodes = codeDatabase.GetItems ();

				foreach (var item in codeDatabase.SelectAll<Code>().Where(GlobalSupport.OpenCodeQuery).ToList())
				{
					deleteCodePopUp.Menu.Add(item.ID, item.ID, item.ID, item.ID + ": " + item.FileName + " (Author: " + item.Author + ")");
				}

				deleteCodePopUp.MenuItemClick += (sender, e) =>
				{
					codeDatabase.DeleteById<Code>(e.Item.ItemId);
				};

				deleteCodePopUp.Show();
			}
			catch (Exception e)
			{
				ShowPopUpMessage(e.Message);
			}
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
				}

				string codeToParse = txtCodeField.Text;

				codeToParse = RemoveWhiteSpaces(codeToParse);
				codeToParse += "\n";

				List<ICodeBlock> result = codeParser.ParseCode(codeToParse);

				ExecuteCommand(result, txtCodeField.Text);
				ToggleButtons(false);

				robot.checkGoals();
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

		/// <summary>
		/// Executes the commands.
		/// </summary>
		/// <param name="command">Command.</param>
		/// <param name="text">Text.</param>
		/// <created>Stef Chappin</created>
		private  static void ExecuteCommand(List<ICodeBlock> command, string text)
		{
			try
			{
				MainCode m = new MainCode();

				foreach (var item in command)
				{
					m.addChild(item);
				}
			
				m.execute(null);
			}
			catch (RobotException excRobot)
			{
				throw excRobot;
			}
			catch (CodeParseException excCodeParse)
			{
				throw excCodeParse;
			}
			catch (SyntaxParseException excSyntax)
			{
				throw excSyntax;
			}
			catch (MapException excMap)
			{
				throw excMap;
			}
			catch (ArgumentOutOfRangeException excArgOOR)
			{
				throw excArgOOR;
			}
		}

		/// <summary>
		/// Gets the string of the command which is executed
		/// </summary>
		/// <returns>The excecuted.</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="text">Text.</param>
		/// <created>Stef Chappin</created>
		private static string CommandExcecuted(int lineNumber, string text)
		{
			try
			{
				string[] stringSeperator = new string[]{ "\n" };
				string[] lines = text.Split(stringSeperator, StringSplitOptions.None);
				if (lineNumber < 1)
				{
					lineNumber = 1;
				}
				return lines[lineNumber - 1];
			}
			catch (IndexOutOfRangeException ex)
			{
				throw ex;
			}
		}

		#region utilities
		public string RemoveWhiteSpaces(string codeToParse)
		{
			return Regex.Replace(codeToParse, " ", "\t");
		}

		public void ShowPopUpMessage(string message)
		{
			alert.SetTitle(message);
			alert.SetButton("OK", (object sender, DialogClickEventArgs e) =>
				{
				});
			alert.Show();
		}

		public void SetErrorLabelMessage(string message)
		{
			lblError.Text = message;
		}

		public void OpenHelp()
		{
			var helpScreen = new Intent(this, typeof(ActivityHelp));
			StartActivity(helpScreen);
		}

		/// <summary>
		/// Gets the goals and sets it in the lblgoals textview
		/// </summary>
		/// <created>Stef Chappin</created>
		public void SetGoals()
		{
			lblGoals.Text = robot.getGoalsInString();
		}

		/// <summary>
		/// Checks if the level is completed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		/// <created>Stef Chappin</created>
		public void CheckGoals(object sender, EventArgs e)
		{
			Exception ex = (e as System.ComponentModel.RunWorkerCompletedEventArgs).Error;
			if (ex != null)
			{
				SetErrorLabelMessage(ex.Message);
				/*switch (ex.GetType ().ToString ()) {
				case "Shared.Exceptions.RobotException":
					break;
				case "Shared.Exceptions.CodeParseException":
					break;
				case "Shared.Exceptions.MapException":
					break;
				case "System.ArgumentOutOfRangeException":
					break;
				default:
					break;
				}*/ // This code can be used in the future
			}
			else
			{
				var goalsCompleted = robot.checkGoals();
				if (goalsCompleted)
				{
					lblGoals.Text = "Level completed";
					int score = robot.CalculateHighscore(robot.Moves, robot.level.MinMoves, txtCodeField.LineCount, robot.level.MinLines);
					ShowScorePopup (score);
					_achievementManager.RegisterEvent(EAchievementType.FirstLevelCompleted, 0);
					_achievementManager.RegisterEvent (EAchievementType.Score1000, score);
					_achievementManager.RegisterEvent (EAchievementType.Score10000, score);
				}
				else
				{
					lblGoals.Text = "Level not completed";
				}
			}
			ToggleButtons(true);
		}

		private void ToggleButtons(bool toggle)
		{
			btnValidate.Enabled = toggle;
			btnReset.Enabled = toggle;
		}

		private void ShowScorePopup(int score)
		{
			EditText input = new EditText(this);
			input.Text = GlobalSupport.LastNameInput;
			AlertDialog ad = new AlertDialog.Builder (this).Create();
			ad.SetTitle ("Enter name");
			ad.SetMessage ("You have passed the level with a score of " + score + "! Please enter your name.");
			ad.SetView (input);
			ad.SetButton ("Save",(senderAlert, args) => {
				//Save Highscore
				robot.SaveHighscore(score,input.Text);
				GlobalSupport.LastNameInput = input.Text;

			});

			ad.SetButton2 ("Cancel", (senderAlert, args) => {
				// cancels (Do nothing)
			});

			ad.Show ();
		}
		#endregion
	}
}


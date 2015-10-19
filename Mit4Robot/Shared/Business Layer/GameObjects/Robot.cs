using System;
using System.ComponentModel;
using System.Collections.Generic;
using Shared.DataBase;
using Shared.CustomEventArgs;
using Shared.Exceptions;
using Shared.Enums;
using Shared.Achievements;

namespace Shared.BusinessLayer
{
	public delegate void HighLightEventDelegate(HighlightCodeEventArgs args);

	/// <summary>
	/// The robot class let the robot do commands
	/// </summary>
	public class Robot
	{
		public static EventHandler UpdateRobotEvent;
		public static EventHandler checkGoalsEvent;
		public Dictionary<string,int> goals ;

		public static BackgroundWorker bg;

		private static Robot instance;

		private Robot() {}

		/// <summary>
		/// The constructor of the Robot which sets the robotPosition and orientationEnum
		/// </summary>
		/// <param name="position">robotPosition</param>
		/// <param name="orientation">Orientation</param>
		/// <created>Stef Chappin</created>
		private Robot(EOrientation orientation, Map map){
			xPosition = map.spawn.x;
			yPosition = map.spawn.y;
			orientationEnum = orientation;
			level = map;
			inventory = new List<string> ();
			goals = getGoals ();
		}

		/// <summary>
		/// Creates an instance of the Robot
		/// </summary>
		/// <param name="position">robotPosition.</param>
		/// <param name="orientation">Orientation.</param>
		/// <created>Stef Chappin</created>
		public static Robot Create (EOrientation orientation, Map map)
		{
			instance = new Robot (orientation, map);
			return instance;
		}

		/// <summary>
		/// Gets the instance of the robot
		/// </summary>
		/// <value>The instance.</value>
		/// <created>Stef Chappin</created>
		public static Robot Instance
		{
			get 
			{
				if (instance == null)
				{
					instance = new Robot();
				}
				return instance;
			}
		}

		public int Moves {
			get;
			set;
		}

		public EOrientation orientationEnum {
			get;
			set;
		}

		public int xPosition {
			get;
			set;
		}

		public int yPosition {
			get;
			set;
		}

		public Map level {
			get;
			set;
		}

		public List<string> inventory {
			get;
			set;
		}

		/// <summary>
		/// A Switch for checking whether the robot can move to a certain direction
		/// </summary>
		/// <returns><c>true</c>, if the robot can move in the specified direction, <c>false</c> if the robot can move in the specified direction</returns>
		/// <param name="eCanInstructions">E can instructions.</param>
		public bool canMove(ECanInstructions eCanInstruction)
		{
			return level.checkMove (xPosition, yPosition, eCanInstruction, this.orientationEnum);
		}

		//public bool canMove(ECanInstructions eCanInstruction)
		//{
		//	return level.checkMove (xPosition, yPosition, eCanInstruction);
		//}

		/// <summary>
		/// The robot will rotate to the right.
		/// </summary>
		/// <returns>The orientation the robot is facing.</returns>
		/// <created>Stef Chappin</created>
		public void RotateRight (int lineNumber)
		{
			EOrientation orientation = orientationEnum;
			if ((int)orientation == 4) {
				//Will set the orientation to North.
				orientation = (EOrientation)1;
			} 
			else {
				int intOfEnum = 0;
				intOfEnum = (int)orientation + 1;
				//Will set the orientationEnum +1.
				orientation = (EOrientation)intOfEnum;
			}

			orientationEnum = orientation;

			GlobalSupport.CurrentRobotOrientation = orientationEnum;
			//highLight (lineNumber);uyuu
			UpdateGUIbg (lineNumber);
		}

		/// <summary>
		/// The robot wil move forward
		/// </summary>
		/// <created>Stef Chappin</created>
		public void Forward(int lineNumber){
			bool canMove = level.checkCoordinates (xPosition, yPosition, orientationEnum);
			switch (orientationEnum) {
			case EOrientation.North:
				if (canMove) {
					yPosition = yPosition - 1;
					UpdateGUIbg (lineNumber);
				} 
				else {
					throw new RobotException ("The robot can't move forward.");
				}
				break;
			case EOrientation.East:
				if (canMove) {
					xPosition = xPosition + 1;
					UpdateGUIbg (lineNumber); 
				}
				else {
					throw new RobotException ("The robot can't move forward.");
				}
				break;
			case EOrientation.South:
				if (canMove) {
					yPosition = yPosition + 1;
					//highLight (lineNumber);
					UpdateGUIbg (lineNumber);
				}
				else {
					throw new RobotException ("The robot can't move forward.");
				}
				break;
			case EOrientation.West:
				if (canMove) {
					xPosition = xPosition - 1;
					//highLight (lineNumber);
					UpdateGUIbg (lineNumber);
				}
				else {
					throw new RobotException ("The robot can't move forward.");
				}
				break;
			default:
				break;
			}
		}

		/// <summary>
		/// Pick the object and set it in the inventory of the Robot
		/// </summary>
		/// <param name="objectToPickUp">Object to pick up.</param>
		/// <created>Stef Chappin</created>
		public void PickUp(int lineNumber, string objectToPickUp){
			try{
				string objectPickedUp = level.checkShopInventory (xPosition, yPosition, objectToPickUp);
				if (objectPickedUp != null) {
					goals[objectToPickUp.ToLower()]-=1;
					//inventory.Add is only for test purpose to check if the pickUp works.
						inventory.Add (objectPickedUp);
				}
				else {
					throw new RobotException (string.Format("There is no {0} to pick up",objectToPickUp));
				}
			}
			catch(MapException mapEx){
				throw mapEx;
			}
			catch(KeyNotFoundException){
				throw new RobotException(string.Format("There is no{0} to pick up", objectToPickUp));
			}
		}

		/// <summary>
		/// Checks if the Robot is at a shop
		/// </summary>
		/// <param name="shopName">Shop name.</param>
		/// <created>Stef Chappin</created>
		public bool At(string shopName){
			bool isAtShop = level.checkCoordinatesForShop (xPosition, yPosition, shopName);
			return isAtShop;
		}

		public void UpdateGUI ()
		{
			if (UpdateRobotEvent != null) 
			{
				UpdateRobotEvent (this, new HighlightCodeEventArgs (1)); 
			}
		}

		public void UpdateGUIbg(int lineNr)
		{
			if (Robot.bg != null) {
				Robot.bg.ReportProgress (lineNr);
			}
		}

		public void triggerCheckGoalsEvent(){
			if (checkGoalsEvent != null) {
				checkGoalsEvent (this, new EventArgs ());
			}
		}


		public void UpdateGUI (int lineNumber)
		{
			if (UpdateRobotEvent != null) 
			{
				UpdateRobotEvent (this, new HighlightCodeEventArgs (lineNumber));
			}
		}
			

		public void SaveHighscore(int score, string name)
		{
			HighScore highScore = new HighScore ();
			highScore.Level = GlobalSupport.GameLevel;
			highScore.Score = score;
			highScore.Date = DateTime.Now;
			highScore.Name = name;

			DataBase.DataBase.Instance ().Insert (highScore);
		}

		public int CalculateHighscore(int moves, int minMoves, int lines, int minLines)
		{
			int maxScore = 1000;
			int minScore = 100;
			int modifierMoves = maxScore / minMoves;


			int scoreMoves = maxScore - ((moves - minMoves) * modifierMoves);

			if (scoreMoves < minScore) {
				scoreMoves = minScore;
			}

			int modifierLines = maxScore / minLines;

			int scoreLines = maxScore - ((lines - minLines) * modifierLines);

			if (scoreLines < minScore) {
				scoreLines = minScore;
			}

			int score = scoreLines + scoreMoves;

			return score;
		}

		/// <summary>
		/// Checks if the robot got the goals
		/// </summary>
		/// <returns><c>true</c>, if goals was checked, <c>false</c> otherwise.</returns>
		/// <created>Stef Chappin</created>
		public bool checkGoals(){
			if (xPosition == level.spawn.x && yPosition == level.spawn.y) {
				bool result = false;
				foreach (var item in goals) {
					if (item.Value == 0) {
						result = true;
					} else {
						result = false;
						break;
					}
				}
				return result;
			} 
			else {
				return false;
			}
		}

		/// <summary>
		/// Gets the goals from the map
		/// </summary>
		/// <returns>The goals.</returns>
		/// <created>Stef Chappin</created>
		public Dictionary<string, int> getGoals(){
			Dictionary<string, int> goalsDictionary = new Dictionary<string, int> ();
			goalsDictionary.Add ("cabbage", level.cabbagesToRetreive);
			goalsDictionary.Add ("sausage", level.sausagesToRetreive);
			return goalsDictionary;
		}

		/// <summary>
		/// Gets the goals in string.
		/// </summary>
		/// <returns>The goals in string.</returns>
		/// <created>Stef Chappin</created>
		public string getGoalsInString(){
			Dictionary<string, int> goalsDictionary = getGoals ();
			string result = "";
			foreach (var item in goalsDictionary) {
				if (item.Value != 0) {
					result += item.Key.ToLower () + " ";
					result += item.Value.ToString ().ToLower () + " ";
				}
			}
			return result;
		}
	}
}
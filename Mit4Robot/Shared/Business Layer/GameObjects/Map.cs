using System;
using System.Collections.Generic;
using Shared.Exceptions;
using Shared.Enums;

namespace Shared.BusinessLayer
{
	public struct Spawn
	{
		public int x;
		public int y;

		public Spawn(int x, int y){
			this.x = x;
			this.y = y;
		}
	}

	/// <summary>
	/// Creates a map
	/// </summary>
	public class Map
	{
		public Tile[,] map { get; set; }
		public String Name { get; set; }
		public int MinMoves { get; set; }
		public int MinLines { get; set; }
		public int cabbagesToRetreive { get; set; }
		public int sausagesToRetreive { get; set; }

		public Spawn spawn { get; private set; }

		public Dictionary<EShops,List<string>> shopsAndInventory = new Dictionary<EShops, List<string>>(); 

		public Map (Spawn spawn)
		{
			switch (GlobalSupport.GameDifficulty) 
			{
			case EDifficulty.Easy:
				GenerateEasyMap ();
				break;
			case EDifficulty.Medium:
				GenerateMediumMap ();
				break;
			case EDifficulty.Hard:
				GenerateHardMap ();
				break;
			}

			this.spawn = spawn;
		}

		/// <summary>
		/// Creates a map only for test
		/// </summary>
		/// <param name="difficulty">Difficulty.</param>
		public Map (EDifficulty difficulty)
		{
			switch (difficulty) 
			{
			case EDifficulty.Easy:
				GenerateEasyMap ();
				break;
			case EDifficulty.Medium:
				GenerateMediumMap ();
				break;
			case EDifficulty.Hard:
				GenerateHardMap ();
				break;
			}
			GenerateShops ();
		}

		public void GenerateShops ()
		{
			List<string> butcherInventory = new List<string> ();
			butcherInventory.Add ("Sausage");
			butcherInventory.Add ("Ham");
			shopsAndInventory.Add(EShops.Butcher, butcherInventory);
			List<string> greengrocerInventory = new List<string> ();
			greengrocerInventory.Add ("cabbage");
			shopsAndInventory.Add (EShops.Greengrocer, greengrocerInventory);
		}


		private void GenerateEasyMap()
		{
			map = new Tile[3, 3];

			// y, x - north, east, south, west
			map [0, 0] = new Tile (false, true, false, false, EShops.None, 0);
			map [0, 1] = new Tile (false, false, true, true, EShops.None,0);
			map [0, 2] = new Tile (false, false, false, false, EShops.None,0);
			map [1, 0] = new Tile (false, false, false, false, EShops.None,0);
			map [1, 1] = new Tile (true, false, true, false, EShops.None,0);
			map [1, 2] = new Tile (false, false, false, false, EShops.None,0);
			map [2, 0] = new Tile (false, true, false, false, EShops.Butcher,0);
			map [2, 1] = new Tile (true, true, false, true, EShops.None,0);
			map [2, 2] = new Tile (false, false, false, true, EShops.Greengrocer,0);

			spawn = new Spawn (0, 0);
			cabbagesToRetreive = 1;
			sausagesToRetreive = 1;
			Robot robot = Robot.Create (EOrientation.East, this);
		}

		private void GenerateMediumMap ()
		{
			map = new Tile[5, 5];

			//TO-DO: Create a medium sized maps
		}

		private void GenerateHardMap ()
		{
			map = new Tile[7, 7];
		}

		/// <summary>
		/// Check if the Robot can move towards an orientation at the Map
		/// </summary>
		/// <returns><c>true</c> If the Robot can move forward <c>false</c> If the Robot can't move forward</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="orientation">Orientation.</param>
		/// <created>Stef Chappin</created>
		public bool checkCoordinates(int x, int y, EOrientation orientation){
			var tile = map [y, x];
			switch (orientation) {
			case EOrientation.North:
				return tile.North;
			case EOrientation.East:
				return tile.East;
			case EOrientation.South:
				return tile.South;
			case EOrientation.West:
				return tile.West;
			default:
				return false;
			}
		}

		/// <summary>
		/// Checks if a move in a certain direction is possible
		/// </summary>
		/// <returns><c>true</c>, if move is possible, <c>false</c> otherwise.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="canInstruction">Can instruction.</param>
		public bool checkMove(int x, int y, ECanInstructions canInstruction, EOrientation robotOrientation)
		{
			var tile = map [y, x];
			switch (canInstruction) {
			case ECanInstructions.Forward:
				switch (robotOrientation) {
				case EOrientation.North:
					return tile.North;
				case EOrientation.East:
					return tile.East;
				case EOrientation.South:
					return tile.South;
				case EOrientation.West:
					return tile.West;
				default:
					return false;
				}
			case ECanInstructions.Backward:
				switch (robotOrientation) {
				case EOrientation.North:
					return tile.South;
				case EOrientation.East:
					return tile.West;
				case EOrientation.South:
					return tile.North;
				case EOrientation.West:
					return tile.East;
				default:
					return false;
				}
			case ECanInstructions.Left:
				switch (robotOrientation) {
				case EOrientation.North:
					return tile.West;
				case EOrientation.East:
					return tile.North;
				case EOrientation.South:
					return tile.East;
				case EOrientation.West:
					return tile.South;
				default:
					return false;
				}
			case ECanInstructions.Right:
				switch (robotOrientation) {
				case EOrientation.North:
					return tile.East;
				case EOrientation.East:
					return tile.South;
				case EOrientation.South:
					return tile.West;
				case EOrientation.West:
					return tile.North;
				default:
					return false;
				}
			case ECanInstructions.None:
				return false;
			default:
				return false;
			}
		}

		/// <summary>
		/// Checks if the Robot is at a shop
		/// </summary>
		/// <returns>The name of the shop if there is any</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <created>Stef Chappin</created>
		private EShops? checkCoordinatesForShop(int x, int y){
			var tile = map [y, x];
			if (tile.Shop != EShops.None) {
				return tile.Shop;
			}
			return null;
		}

		/// <summary>
		/// Checks the coordinates for shop.
		/// </summary>
		/// <returns><c>true</c>, if coordinates for shop was checked, <c>false</c> otherwise.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="shopName">Shop name.</param>
		/// <created>Stef Chappin</created>
		public bool checkCoordinatesForShop(int x, int y, string shopName){
			var tile = map [y, x];
			EShops shop = (EShops)Enum.Parse (typeof(EShops), UpperCaseFirst(shopName), true);
			if (shop != EShops.Home) {
				if (tile.Shop == shop) {
					return true;
				} else {
					return false;
				}
			}
			else {
				if (spawn.x == x && spawn.y == y) {
					return true;
				}
				else {
					return false;
				}
			}
		}
	
		/// <summary>
		/// Checks if the shop has the item the Robot is trying to pick up
		/// </summary>
		/// <returns>The shop inventory.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="objectToPickUp">Object to pick up.</param>
		/// <created>Stef Chappin</created>
		public string checkShopInventory(int x, int y, string objectToPickUp){
			EShops? shopName = checkCoordinatesForShop (x, y);
			if (shopName.HasValue) {
				foreach (KeyValuePair<EShops,List<string>> pair in shopsAndInventory) {
					if (pair.Key == shopName) {
						for (int i = 0; i < pair.Value.Count; i++) {
							if (pair.Value [i].Trim ().ToUpper () == objectToPickUp.Trim ().ToUpper ()) {
								var result = pair.Value [i];
								pair.Value.RemoveAt (i);
								return result;
							}
						}
					}
				}
			} 
			else {
				throw new MapException (string.Format ("There is no {0} to pickup ", objectToPickUp));
			}
			return null;
		}

		/// <summary>
		/// Sets the first character to Upper case
		/// </summary>
		/// <returns>The case first.</returns>
		/// <param name="s">S.</param>
		/// <created>Stef</created>
		private string UpperCaseFirst(string s){
			s = s.ToLower ();
			if (string.IsNullOrEmpty (s)) {
				return string.Empty;
			}
			return char.ToUpper (s [0]) + s.Substring (1);
		}
	}
}


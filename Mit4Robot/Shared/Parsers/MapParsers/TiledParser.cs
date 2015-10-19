using System;
using System.IO;
using System.Collections.Generic;
using Shared.BusinessLayer;
using Shared.Exceptions;
using Shared.Enums;

#if __IOS__
using System.Xml;
#else
#if __ANDROID__
using System.Xml;
#else
#if __WINPHONE__
// TODO: Add xml for winphone
#else
using System.Xml;
#endif
#endif
#endif

namespace Shared.Parsers.MapParsers
{
	public class MapVariables{
		public int width { get; set; }
		public int height { get; set; }

		public int firstGidRoadmap { get; set; }
		public int firstGidGraphic { get; set; }

		public string graphic { get; set; }
		public string roadmap { get; set; }

		public int minMoves { get; set; }
		public int minLines { get; set; }

		public int cabbagesToRetreive { get; set; }
		public int sausagesToRetreive { get; set; }

		public Dictionary<EShops, List<string>> mapInventory { get; set; }

		public MapVariables(){
			width = 0;
			height = 0;

			firstGidGraphic = 0;
			firstGidRoadmap = 0;

			graphic = "";
			roadmap = "";

			minMoves = 0;
			minLines = 0;

			sausagesToRetreive = 0;
			cabbagesToRetreive = 0;

			mapInventory = new Dictionary<EShops, List<string>>(); 
			mapInventory.Add (EShops.Greengrocer, new List<string> ());
			mapInventory.Add (EShops.Butcher, new List<string> ());
		}
	}

	/// Author: Guy Spronck
	/// Date: 	22-06-2015
	/// <summary>
	/// MapParser for the Tiled (.tmx) file format.
	/// </summary>
	public class TiledParser : MapParser
	{
		/// Author:	Guy Spronck
		/// Date: 	04-06-2015
		/// <summary>
		/// Parse the specified stream and return a robot object with the map included.
		/// </summary>
		/// <returns>Returns an instance of robot.</returns>
		/// <param name="stream">Stream of file to parse</param>
		/// <param name="mapName">Name of map</param>
		public static Robot Parse(Stream stream, String mapName)
		{
			var convertedMap = ReadFileXml (stream);

			var map = new Map(convertedMap.Item1);
			map.map = convertedMap.Item2;
			map.Name = mapName;
			map.MinMoves = convertedMap.Item3.minMoves;
			map.MinLines = convertedMap.Item3.minLines;
			map.cabbagesToRetreive = convertedMap.Item3.cabbagesToRetreive;
			map.sausagesToRetreive = convertedMap.Item3.sausagesToRetreive;
			map.shopsAndInventory = convertedMap.Item3.mapInventory;

			return Robot.Create (EOrientation.East, map);
		}

		/// Author:	Guy Spronck
		/// Date:	04-06-2015
		/// <summary>
		/// Generates the map with the Tile object.
		/// </summary>
		/// <returns>Tuple containing Spawn, Tilemap</returns>
		/// <param name="stringLvl">String array for roadmap</param>
		/// <param name="stringGraphicLvl">String array for graphic layer</param>
		/// <param name="mapVariables">Variables of the map.</param>
		private static Tuple<Spawn, Tile[,]> GenerateTileMap(string[,] stringLvl, string[,] stringGraphicLvl, MapVariables mapVariables)
		{
			int spawnX = 0, spawnY = 0;
			var lvl = new Tile[mapVariables.height, mapVariables.width];
			for (int iX = 0; iX < mapVariables.height; iX++) {
				for (int iY = 0; iY < mapVariables.width; iY++) {
					
					if (stringLvl [iX, iY].Contains ((mapVariables.firstGidRoadmap+5).ToString())) {
						spawnX = iY;
						spawnY = iX;
					}

					lvl [iX, iY] = new Tile (checkNorthTile(iX,iY,stringLvl),
						checkEastTile(iX,iY,stringLvl,mapVariables.width),
						checkSouthTile(iX,iY,stringLvl,mapVariables.height),
						checkWestTile(iX,iY,stringLvl),
						checkShop(iX,iY,stringLvl,mapVariables.firstGidRoadmap), 
						Convert.ToInt32(stringGraphicLvl[iX,iY]) - mapVariables.firstGidGraphic);
				}
			}
			return Tuple.Create (new Spawn(spawnX,spawnY), lvl);
		}

		/// Author:	Guy Spronck
		/// Date:	05-06-2015
		/// <summary>
		/// Checks the north tile for a road
		/// </summary>
		/// <returns><c>true</c>, if north tile was road, <c>false</c> otherwise.</returns>
		/// <param name="iX">I x.</param>
		/// <param name="iY">I y.</param>
		/// <param name="stringLvl">String lvl.</param>
		private static bool checkNorthTile(int iX, int iY,  string[,] stringLvl)
		{
			if (iX != 0) {
				return ((stringLvl [iX - 1, iY] != "0") && (stringLvl[iX,iY] != "0"));
			}
			return false;
		}

		/// Author:	Guy Spronck
		/// Date:	05-06-2015
		/// <summary>
		/// Checks the east tile for a road
		/// </summary>
		/// <returns><c>true</c>, if east tile was road, <c>false</c> otherwise.</returns>
		/// <param name="iX">I x.</param>
		/// <param name="iY">I y.</param>
		/// <param name="stringLvl">String lvl.</param>
		/// <param name="y">The y coordinate.</param>
		private static bool checkEastTile(int iX, int iY,  string[,] stringLvl,int y)
		{
			if (iY != (y - 1)) {
				return ((stringLvl [iX, iY + 1] != "0") && (stringLvl[iX,iY] != "0"));
			}
			return false;
		}

		/// Author:	Guy Spronck
		/// Date:	05-06-2015
		/// <summary>
		/// Checks the south tile for a road
		/// </summary>
		/// <returns><c>true</c>, if south tile was road, <c>false</c> otherwise.</returns>
		/// <param name="iX">I x.</param>
		/// <param name="iY">I y.</param>
		/// <param name="stringLvl">String lvl.</param>
		/// <param name="x">The x coordinate.</param>
		private static bool checkSouthTile(int iX, int iY,  string[,] stringLvl, int x)
		{
			if (iX != (x - 1)) {
				return ((stringLvl [iX + 1, iY] != "0") && (stringLvl[iX,iY] != "0"));
			}
			return false;
		}

		/// Author:	Guy Spronck
		/// Date:	05-06-2015
		/// <summary>
		/// Checks the west tile for a road
		/// </summary>
		/// <returns><c>true</c>, if west tile was road, <c>false</c> otherwise.</returns>
		/// <param name="iX">I x.</param>
		/// <param name="iY">I y.</param>
		/// <param name="stringLvl">String lvl.</param>
		private static bool checkWestTile(int iX, int iY,  string[,] stringLvl)
		{
			if (iY != 0) {
				return ((stringLvl [iX, iY - 1] != "0") && (stringLvl[iX,iY] != "0"));
			}
			return false;
		}

		/// Author:	Guy Spronck
		/// Date:	05-06-2015
		/// <summary>
		/// Checks if tile contains shop
		/// </summary>
		/// <returns>The shop.</returns>
		/// <param name="iX">X</param>
		/// <param name="iY">Y</param>
		/// <param name="stringLvl">String lvl.</param>
		/// <param name="firstGid">First gid.</param>
		private static EShops checkShop(int iX, int iY, string[,] stringLvl, int firstGid)
		{
			if (stringLvl [iX, iY].Contains ((firstGid + 2).ToString())) {
				return EShops.Butcher;
			}
			if (stringLvl [iX, iY].Contains ((firstGid + 3).ToString())) {
				return EShops.Bakery;
			}
			if (stringLvl [iX, iY].Contains ((firstGid + 4).ToString())) {
				return EShops.Greengrocer;
			}
			return EShops.None;
		}

		/// Author:	Guy Spronck
		/// Date:	05-06-2015
		/// <summary>
		/// Reads the file from using xml reader
		/// </summary>
		/// <returns>spawn, lvl, mapVariables</returns>
		/// <param name="stream">Stream.</param>
		private static Tuple<Spawn, Tile[,], MapVariables> ReadFileXml(Stream stream)
		{
			// Initialize variables
			string[,] stringLvl = null, stringGraphicLvl = null;

			MapVariables mapVariables = new MapVariables ();

			// Setup xmlreader, throw error if problem with file.
			XmlReader xmlDoc;
			try{
				xmlDoc = XmlReader.Create(stream);
			}catch(ArgumentException){
				throw new MapParseException ("No file.");
			}catch(DirectoryNotFoundException){
				throw new MapParseException ("Directory not found");
			}

			// Decode the file and get the required values
			try{
				while (xmlDoc.Read ()) {
					if (xmlDoc.NodeType == XmlNodeType.Element) {
						if (xmlDoc.Name == "map") {
							mapVariables.width = Convert.ToInt32 (xmlDoc.GetAttribute ("width"));
							mapVariables.height = Convert.ToInt32 (xmlDoc.GetAttribute ("height"));
						} else if (xmlDoc.Name == "tileset") {
							if (xmlDoc.GetAttribute ("name").ToLower() == "roadmap") {
								mapVariables.firstGidRoadmap = Convert.ToInt32 (xmlDoc.GetAttribute ("firstgid"));
							} else if (xmlDoc.GetAttribute ("name").ToLower() == "tilesroad") {
								mapVariables.firstGidGraphic = Convert.ToInt32 (xmlDoc.GetAttribute ("firstgid"));
							}
						}else if(xmlDoc.Name == "property"){
							if (xmlDoc.GetAttribute ("name") == "minMoves") {
								mapVariables.minMoves = Convert.ToInt32(xmlDoc.GetAttribute ("value"));
							}else if (xmlDoc.GetAttribute ("name") == "minLines"){
								mapVariables.minLines = Convert.ToInt32(xmlDoc.GetAttribute ("value"));
							}else if (xmlDoc.GetAttribute ("name") == "CabbagesToRetreive"){
								mapVariables.cabbagesToRetreive = Convert.ToInt32(xmlDoc.GetAttribute ("value"));
							}else if (xmlDoc.GetAttribute ("name") == "SausagesToRetreive"){
								mapVariables.sausagesToRetreive = Convert.ToInt32(xmlDoc.GetAttribute ("value"));
							}else if (xmlDoc.GetAttribute ("name") == "Sausage" || xmlDoc.GetAttribute ("name") == "Cabbage" || xmlDoc.GetAttribute ("name") == "Ham"){
								// Populate shop
								AddItemToShops(mapVariables.mapInventory, xmlDoc.GetAttribute ("name"), Convert.ToInt32(xmlDoc.GetAttribute ("value")));
							}
						} else if (xmlDoc.Name == "layer") {
							if (xmlDoc.GetAttribute ("name") == "Graphic") {
								xmlDoc.ReadToDescendant ("data");
								mapVariables.graphic = xmlDoc.ReadElementContentAsString ();
							} else if (xmlDoc.GetAttribute ("name") == "Roadmap") {
								xmlDoc.ReadToDescendant ("data");
								mapVariables.roadmap = xmlDoc.ReadElementContentAsString ();
							}
						}
					}
				}
			}catch(FormatException){
				throw new MapParseException("Invalid map format. (Contains string where int was expected.)");
			}finally{
				xmlDoc.Dispose ();
				stream.Close ();
			}
			

			stringLvl = ConvertStringMapToArray (mapVariables.roadmap, mapVariables.width, mapVariables.height);
			stringGraphicLvl = ConvertStringMapToArray (mapVariables.graphic, mapVariables.width, mapVariables.height);

			var tupleTileMap = GenerateTileMap (stringLvl, stringGraphicLvl, mapVariables);
			return Tuple.Create(tupleTileMap.Item1, tupleTileMap.Item2, mapVariables);
		}

		/// Author:	Guy Spronck
		/// Date:	05-06-2015
		/// <summary>
		/// Converts a comma seperated string to array of width and height.
		/// </summary>
		/// <returns>String array.</returns>
		/// <param name="map">Comma seperated string</param>
		/// <param name="x">x</param>
		/// <param name="y">y</param>
		private static string[,] ConvertStringMapToArray(string map, int x, int y)
		{
			string[,] mapArray = new string[y, x];

			// Remove unwanted characters from string
			map = map.Replace ("\n", "");
			map = map.Replace ("\r", "");

			string[] split = map.Split(',');
			int row = 0;
			int column = 0;
			for (int i = 0; i < split.Length; i++) {
				mapArray [row, column] = split [i];
				if (column == (x-1)) {
					row++;
					column = 0;
				}else{
					column++;
				}
			}

			return mapArray;
		}

		/// Author:	Guy Spronck
		/// Date:	18-06-2015
		/// <summary>
		/// Adds the item to shop.
		/// </summary>
		/// <param name="mapInventory">Map inventory.</param>
		/// <param name="item">Item.</param>
		/// <param name="count">Count.</param>
		private static void AddItemToShops (Dictionary<EShops, List<string>> mapInventory, string item, int count)
		{
			List<string> butcherInventory;
			List<string> greengrocerInventory;
			mapInventory.TryGetValue (EShops.Butcher, out butcherInventory);
			mapInventory.TryGetValue (EShops.Greengrocer, out greengrocerInventory);

			if (item == "Sausage" || item == "Ham") {
				for (int i = 0; i < count; i++) {
					butcherInventory.Add (item);
				}
			} else if (item == "Cabbage") {
				for (int i = 0; i < count; i++) {
					greengrocerInventory.Add (item);
				}
			}

			mapInventory[EShops.Butcher] = butcherInventory;
			mapInventory[EShops.Greengrocer] = greengrocerInventory;
		}
	}
}


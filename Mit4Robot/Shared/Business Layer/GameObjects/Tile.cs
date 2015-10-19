using System;

namespace Shared.BusinessLayer
{
	public class Tile
	{
		public Tile (bool north, bool east, bool south, bool west, EShops shop, int tileId)
		{
			North = north;
			East = east;
			South = south;
			West = west;

			Shop = shop;

			TileId = tileId;
		}

		public bool North { get; set; }
		public bool East { get; set; }
		public bool South { get; set; }
		public bool West { get; set; }

		public EShops Shop { get; set; }

		public int TileId{ get; set;}
	}
}


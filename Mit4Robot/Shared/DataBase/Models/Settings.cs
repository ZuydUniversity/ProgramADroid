using System;
using SQLite;

namespace Shared.DataBase
{
	/// Author:	Guy Spronck
	/// Date:	16-06-2015
	/// <summary>
	/// Settings model
	/// </summary>
	public class Settings : iTableDefault
	{
		public Settings ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public int GameSpeed{ get; set; }
	}
}


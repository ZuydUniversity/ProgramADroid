using System;
using SQLite;

namespace Shared.DataBase
{
	/// Author:	Guy Spronck
	/// Date:	15-06-2015
	/// <summary>
	/// High score model for database
	/// </summary>
	public class HighScore : iTableDefault
	{
		public HighScore ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string Name { get; set;}
		public string Level { get; set;}
		public int Score { get; set;}

		public DateTime Date { get; set;}
	}
}


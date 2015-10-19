using System;
using SQLite;
using Shared.Enums;

namespace Shared.DataBase
{
	public class Code : iTableDefault
	{
		public Code ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string CodeString { get; set; }
		public string LevelName{ get; set;}
		public EGameLanguage Language{ get; set;}
		public string Author { get; set;}
		public string FileName { get; set;}
		public DateTime Date { get; set;}
	}
}


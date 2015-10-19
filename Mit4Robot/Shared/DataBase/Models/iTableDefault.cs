using System;
using SQLite;

namespace Shared.DataBase
{
	/// Author:	Guy Spronck
	/// Date:	15-06-2015
	/// <summary>
	/// interface for database models, to force include Identifier
	/// </summary>
	public interface iTableDefault
	{
		[PrimaryKey, AutoIncrement]
		int ID { get; set;}
	}
}


using System;
using SQLite;
using Shared.Enums;

namespace Shared.DataBase
{
	/// <summary>
	/// Achievement data class
	/// </summary>
	public class Achievement:iTableDefault
	{
		public Achievement ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public int countToUnlock {
			get;
			set;
		}

		public int count {
			get;
			set;
		}
		public bool isUnlocked { 
			get;
			set;
		}
		public string message {
			get;
			set;
		}

		public string description {
			get;
			set;
		}

		public EAchievementType achievementType{
			get;
			set;
		}
	}
}


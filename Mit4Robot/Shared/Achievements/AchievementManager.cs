using System;
using System.Collections.Generic;
using System.Linq;
using Shared.CustomEventArgs;
using Shared.Enums;
using Shared.DataBase;

namespace Shared.Achievements
{
	/// <summary>
	/// Handles all achievements
	/// </summary>
	public class AchievementManager
	{
		private Dictionary<EAchievementType, List<Achievement>> _achievements;
		private List<Achievement> listDataBaseAchievements = new List<Achievement>();
		private DataBase.DataBase achievementDatabase = DataBase.DataBase.Instance();


		public event EventHandler AchievementUnlocked;

		/// <summary>
		/// Raises the achievement unlocked.
		/// </summary>
		/// <param name="ach">Ach.</param>
		/// <created>Stef</created>
		protected virtual void RaiseAchievementUnlocked(Achievement ach)
		{
			if (!ach.isUnlocked)
			{
				// unlock the event
				ach.isUnlocked = true;

				achievementDatabase.Update(ach);

				var del = AchievementUnlocked as EventHandler;
				if (del != null)
				{
					del(this, new AchievementEventArgs(ach));
				}
			}
		}

		public AchievementManager()
		{
			_achievements = new Dictionary<EAchievementType, List<Achievement>>();

			listDataBaseAchievements = achievementDatabase.SelectAll<Achievement>();

			if (listDataBaseAchievements.Count() == 0)
			{
				listDataBaseAchievements = AddAchievements();
				foreach (var item in listDataBaseAchievements)
				{
					achievementDatabase.Insert(item);
				}
			}

			_achievements.Add(EAchievementType.Start, listDataBaseAchievements);
		}

		/// <summary>
		/// Increments the countToUnlock
		/// </summary>
		/// <param name="type">Type.</param>
		/// <created>Stef</created>
		public void RegisterEvent(EAchievementType type, int score)
		{
			List<Achievement> achievementList = achievementDatabase.SelectAll<Achievement>();
			Achievement achDatabase = null;

			if (!CheckIfTypeExist(achievementList, type))
			{
				return;
			}

			switch (type)
			{
				case EAchievementType.Score1000:
					achDatabase = GetAchievementByType(achievementList, EAchievementType.Score1000);
					achDatabase.count += score;
					achievementDatabase.Update(achDatabase);
					break;
				case EAchievementType.Start:
					achDatabase = GetAchievementByType(achievementList, EAchievementType.Start);
					achDatabase.count++;
					achievementDatabase.Update(achDatabase);
					break;
				case EAchievementType.Score10000:
					achDatabase = GetAchievementByType(achievementList, EAchievementType.Score10000);
					achDatabase.count += score;
					achievementDatabase.Update(achDatabase);
					break;
				case EAchievementType.FirstLevelCompleted:
					achDatabase = GetAchievementByType(achievementList, EAchievementType.FirstLevelCompleted);
					achDatabase.count++;
					achievementDatabase.Update(achDatabase);
					break;
				case EAchievementType.StartPascal:
					achDatabase = GetAchievementByType(achievementList, EAchievementType.StartPascal);
					achDatabase.count++;
					achievementDatabase.Update(achDatabase);
					break;
				case EAchievementType.StartPython:
					achDatabase = GetAchievementByType(achievementList, EAchievementType.StartPython);
					achDatabase.count++;
					achievementDatabase.Update(achDatabase);
					break;
			}

			ParseAchievements(type, achDatabase);
		}

		/// <summary>
		/// Checks if the achievement has to be thrown
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="achDatabase">Ach database.</param>
		/// <created>Stef</created>
		public void ParseAchievements(EAchievementType type, Achievement achDatabase)
		{
			if (type == EAchievementType.Score1000)
			{
				if (achDatabase.count >= achDatabase.countToUnlock) RaiseAchievementUnlocked(achDatabase);
			}
			else if (type == EAchievementType.Score10000)
			{
				if (achDatabase.count >= achDatabase.countToUnlock) RaiseAchievementUnlocked(achDatabase);
			}
			else if (achDatabase.count == achDatabase.countToUnlock)
			{
				RaiseAchievementUnlocked(achDatabase);
			}
		}

		/// <summary>
		/// Add default achievements if not exist in the database
		/// </summary>
		/// <returns>The achievements.</returns>
		/// <created>Stef</created>
		public List<Achievement> AddAchievements()
		{
			List<Achievement> achievementList = new List<Achievement>();
			Achievement ach2 = new Achievement();
			ach2.countToUnlock = 1000;
			ach2.count = 0;
			ach2.isUnlocked = false;
			ach2.message = "Score over 1000.";
			ach2.description = "Achieve a score over 1000.";
			ach2.achievementType = EAchievementType.Score1000;
			achievementList.Add(ach2);
			Achievement ach3 = new Achievement();
			ach3.countToUnlock = 10000;
			ach3.count = 0;
			ach3.isUnlocked = false;
			ach3.message = "Score over 10000";
			ach3.description = "Achieve a score over 10000.";
			ach3.achievementType = EAchievementType.Score10000;
			achievementList.Add(ach3);
			Achievement ach4 = new Achievement();
			ach4.countToUnlock = 1;
			ach4.count = 0;
			ach4.isUnlocked = false;
			ach4.message = "First level completed.";
			ach4.description = "Complete your first level in any difficulty.";
			ach4.achievementType = EAchievementType.FirstLevelCompleted;
			achievementList.Add(ach4);
			Achievement ach5 = new Achievement();
			ach5.countToUnlock = 1;
			ach5.count = 0;
			ach5.isUnlocked = false;
			ach5.message = "You discovered Python.";
			ach5.description = "Start a level with the Python language.";
			ach5.achievementType = EAchievementType.StartPython;
			achievementList.Add(ach5);
			Achievement ach1 = new Achievement();
			ach1.countToUnlock = 1;
			ach1.count = 0;
			ach1.isUnlocked = false;
			ach1.message = "You discovered Pascal.";
			ach1.description = "Start a level with the Pascal language.";
			ach1.achievementType = EAchievementType.StartPascal;
			achievementList.Add(ach1);
			return achievementList;
		}

		/// <summary>
		/// Gets the type of the achievement.
		/// </summary>
		/// <returns>The achievement by type.</returns>
		/// <param name="achDatabase">Ach database.</param>
		/// <param name="achievementType">Achievement type.</param>
		/// <created>Stef</created>
		private Achievement GetAchievementByType(List<Achievement> achDatabase, EAchievementType achievementType)
		{
			foreach (var item in achDatabase)
			{
				if (item.achievementType == achievementType)
				{
					return item;
				}
			}
			return null;
		}

		/// <summary>
		/// Checks if type exist.
		/// </summary>
		/// <returns><c>true</c>, if if type exist was checked, <c>false</c> otherwise.</returns>
		/// <param name="achievementList">Achievement list.</param>
		/// <param name="type">Type.</param>
		/// <created>Stef</created>
		private bool CheckIfTypeExist(List<Achievement> achievementList, EAchievementType type)
		{
			foreach (var item in achievementList)
			{
				if (item.achievementType == type)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Get an list of strings for the GUI
		/// </summary>
		/// <returns>The achievements for GU.</returns>
		/// <created>Stef Chappin</created>
		public List<string> GetAchievementsForGUI()
		{
			List<Achievement> achievements = achievementDatabase.SelectAll<Achievement>().OrderByDescending(x => x.isUnlocked).ToList();

			List<string> stringAchievements = new List<string>();
			string unlocked;
			foreach (var item in achievements)
			{
				if (item.isUnlocked)
				{
					unlocked = "V";
				}
				else
				{
					unlocked = "X";
				}

				stringAchievements.Add(item.message + "\r\n" + item.description + "=" + unlocked);
			}
			return stringAchievements;
		}
	}
}


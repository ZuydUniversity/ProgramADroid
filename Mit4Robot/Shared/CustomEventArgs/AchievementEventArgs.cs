using System;
using Shared.Achievements;
using Shared.DataBase;

namespace Shared.CustomEventArgs
{
	public class AchievementEventArgs:EventArgs
	{
		public Achievement data;

		public AchievementEventArgs (Achievement a)
		{
			data = a;
		}
	}
}


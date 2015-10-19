
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Shared.Achievements;
using Android.Graphics;

namespace Mit4Robot_Android
{
	[Activity(Label = "Achievements")]			
	public class ActivityAchievements : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Achievement);
			AchievementManager achMan = new AchievementManager();

			List<string> achievements = achMan.GetAchievementsForGUI();

			List<string> achievementText = new List<string>();

			foreach (var item in achievements)
			{
				achievementText.Add(item.Split('=')[0]);
			}

			ListView listviewAchievements = FindViewById<ListView>(Resource.Id.listViewAchievements);
			listviewAchievements.Adapter = new ArrayAdapter<String>(this,
			                                                        Android.Resource.Layout.SimpleListItem1,
			                                                        achievementText);

			listviewAchievements.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
			{
			var bla = e.Id;
				// Handle the click event
				string lblText = (e.View as TextView).Text;
				if (achievements[Convert.ToInt32(e.Id)].Split('=')[1] == "V")
				{
					ShareAchievement(lblText);	
				}
			};
		}

		//		public void Bla()
		//		{
		//			ListView listviewAchievements = FindViewById<ListView>(Resource.Id.listViewAchievements);
		//
		//			for (int i = listviewAchievements.FirstVisiblePosition; i < listviewAchievements.LastVisiblePosition + 1; i++)
		//			{
		//				listviewAchievements.GetChildAt(i).SetBackgroundColor(Color.DarkGreen);
		//			}
		//		}

		public void CreateBackGroundColors(ListView listviewAchievements)
		{
			for (int i = listviewAchievements.FirstVisiblePosition; i < listviewAchievements.LastVisiblePosition; i++)
			{
				listviewAchievements.GetChildAt(i).SetBackgroundColor(Color.DarkGreen);
			}
		}

		public void ShareAchievement(string text)
		{
			Intent sendIntent = new Intent();
			sendIntent.SetAction(Intent.ActionSend);
			sendIntent.PutExtra(Intent.ExtraText, "Look at this achievement from the ProgramADroid app: " + text);
			sendIntent.SetType("text/plain");
			StartActivity(sendIntent);
		}
	}
}


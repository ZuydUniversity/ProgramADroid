
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

using Shared.DataBase;

namespace Mit4Robot_Android
{
	[Activity(Label = "Highscores")]			
	public class ActivityHighscores : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Highscores);

			Spinner spinnerCategory = FindViewById<Spinner>(Resource.Id.spinnerCategories);
			ListView listview = FindViewById<ListView>(Resource.Id.listHighscores);

			spinnerCategory.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
			{
				listview.Adapter = new ArrayAdapter<String>(this,
				                                            Android.Resource.Layout.SimpleListItem1,
				                                            HighscoreSelectDifficulty(spinnerCategory.SelectedItem.ToString()));
			};
				
			listview.Adapter = new ArrayAdapter<String>(this,
			                                            Android.Resource.Layout.SimpleListItem1,
			                                            HighscoreSelectDifficulty(spinnerCategory.SelectedItem.ToString()));


			listview.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
			{
				string lblText = (e.View as TextView).Text;
				string[] split = lblText.Split(':');
				if (split[1].Replace(" ", "") == "Nohighscore")
				{
					AlertDialog.Builder alert = new AlertDialog.Builder(this);

					alert.SetTitle("Sorry, you have to get a highscore to share first!");
					alert.SetCancelable(true);

					alert.SetNegativeButton("Close", (senderAlert, args) =>
						{
							//perform your own task for this conditional button click
						});

					alert.Show();
				}
				else
				{
					ShareScore(split[0], split[1].Replace(" ", ""));
				}

			};
		}

		public void ShareScore(string level, string score)
		{
			Intent sendIntent = new Intent();
			sendIntent.SetAction(Intent.ActionSend);
			sendIntent.PutExtra(Intent.ExtraText,
			                     "I got " + score + " points on " + level + " in the ProgramADroid app. Can you beat me?");
			sendIntent.SetType("text/plain");
			StartActivity(sendIntent);
		}

		private List<string> HighscoreSelectDifficulty(string difficulty)
		{
			List<string> highscoreStrings = new List<string>();
			DataBase db = DataBase.Instance();
			List<HighScore> highscores = db.SelectAll<HighScore>();
			foreach (var item in Assets.List(@"Maps/" + difficulty))
			{
				string levelname = item.Substring(0, item.Length - 4);
				try
				{
					HighScore highest = highscores.Where(x => x.Level == item).OrderByDescending(x => x.Score).First();
					highscoreStrings.Add(levelname + ": " + highest.Score + " : " + highest.Name + " : " + highest.Date.ToShortDateString());
				}
				catch (Exception)
				{
					// No highscore for this level.
					highscoreStrings.Add(levelname + ": " + "No highscore");
				}
			}

			return highscoreStrings;
		}
	}
}


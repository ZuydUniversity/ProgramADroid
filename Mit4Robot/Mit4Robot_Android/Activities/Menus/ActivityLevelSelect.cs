
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

using Shared;
using Shared.Enums;

namespace Mit4Robot_Android
{
	[Activity (Label = "Level")]			
	public class ActivityLevelSelect : Activity
	{
		string[] levelArray;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.LevelSelect);

			ListView listview = FindViewById<ListView> (Resource.Id.listLanguage);

			Spinner spinnerCatergory = FindViewById<Spinner> (Resource.Id.spinnerCategoryPlay);
			Spinner spinnerLanguage = FindViewById<Spinner> (Resource.Id.spinnerProgramLanguage);

			spinnerCatergory.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) => 
			{
				EDifficulty diff = EDifficulty.Easy;
				Enum.TryParse( spinnerCatergory.SelectedItem.ToString(), out diff);
				GlobalSupport.GameDifficulty = diff;
				levelArray = Assets.List(@"Maps/" + spinnerCatergory.SelectedItem.ToString());
				for (int i = 0; i < levelArray.GetLength(0); i++) 
				{
					string newName = levelArray [i].Substring (0, levelArray[i].Length-4);
					levelArray[i] = newName;
				}

				listview.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, levelArray);
			};



			listview.ItemClick += (sender, e) => 
			{
				var t = levelArray[e.Id];
				GlobalSupport.GameLevel = t + ".tmx";

				EGameLanguage lang = EGameLanguage.None;
				Enum.TryParse( spinnerLanguage.SelectedItem.ToString(), out lang);
				GlobalSupport.GameLanguage = lang;

				Intent startGame = new Intent (this, typeof(ActivityGame));
				StartActivity (startGame);
			};
		}

	}
}


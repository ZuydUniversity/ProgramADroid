using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Mit4Robot_Android
{
	[Activity (Label = "ProgramADroid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button btnStart = FindViewById<Button> (Resource.Id.btnMainStart);
			Button btnAchievements = FindViewById<Button> (Resource.Id.btnMainAchievements);
			Button btnOptions = FindViewById<Button> (Resource.Id.btnOptions);

			btnStart.Click += (object sender, EventArgs e) => 
			{
				var difficultyMenu = new Intent (this, typeof (ActivityLevelSelect));
				StartActivity (difficultyMenu);
			};

			btnOptions.Click += (object sender, EventArgs e) => 
			{
				var optionMenu = new Intent (this, typeof (ActivityOptions));
				StartActivity (optionMenu);
			};

			Button btnHelp = FindViewById<Button> (Resource.Id.btnMainHelp);

			btnHelp.Click += (object sender, EventArgs e) => 
			{
				var helpMenu = new Intent (this, typeof (ActivityHelp));
				StartActivity (helpMenu);
			};

			Button btnHighscores = FindViewById<Button> (Resource.Id.btnMainHighscores);

			btnHighscores.Click += (object sender, EventArgs e) => 
			{
				var highscoresMenu = new Intent (this, typeof (ActivityHighscores));
				StartActivity (highscoresMenu);
			};

			Button btnCredits = FindViewById<Button> (Resource.Id.btnMainCredits);

			btnCredits.Click += (object sender, EventArgs e) => 
			{
				var creditMenu = new Intent (this, typeof (ActivityCredits));
				StartActivity (creditMenu);
			};

			btnAchievements.Click += (object sender, EventArgs e) => 
			{
				var achievementsMenu = new Intent (this, typeof (ActivityAchievements));
				StartActivity (achievementsMenu);
			};
		}
	}
}



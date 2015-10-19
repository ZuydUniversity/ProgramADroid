
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
using Shared;

namespace Mit4Robot_Android
{
	[Activity(Label = "ActivityOptions")]			
	public class ActivityOptions : Activity
	{
		private AlertDialog alert;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Options);

			SeekBar speedBar = FindViewById<SeekBar>(Resource.Id.sbSpeed);
			Button btnSave = FindViewById<Button>(Resource.Id.btnOptionsSave);
			TextView txtValue = FindViewById <TextView>(Resource.Id.txtValue);

			int speed = 2000;
			txtValue.Text = "The gamespeed is: 2 seconds per move.";
			alert = new AlertDialog.Builder(this).Create();

			speedBar.Progress = 1;

			speedBar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) =>
			{
				if (e.FromUser)
				{
					txtValue.Text = "The gamespeed is: " + (e.Progress + 1).ToString() + " seconds per move.";
					speed = e.Progress * 1000 + 1000;
				}
			};

			btnSave.Click += (object sender, EventArgs e) =>
			{
				updateDatabase(speed);
				ShowPopUpMessage("Options saved.");
			};
		}

		/// <summary>
		/// Shows a popup message
		/// </summary>
		/// <param name="message">Message.</param>
		/// <created>Stef Chappin</created>
		private void ShowPopUpMessage(string message)
		{
			alert.SetTitle(message);
			alert.SetButton("OK", (object sender, DialogClickEventArgs e) =>
				{
				});
			alert.Show();
		}


		/// <summary>
		/// Updates the database with the options.
		/// </summary>
		/// <param name="speed">Speed.</param>
		/// <created>Stef Chappin</created>
		private void updateDatabase(int speed)
		{
			DataBase db = DataBase.Instance();
			Settings settings = db.SelectFirst<Settings>();
			if (settings != null)
			{
				settings.GameSpeed = speed;
				db.Update(settings);
			}
			else
			{
				settings = new Settings();
				settings.GameSpeed = speed;
				db.Update(settings);
			}
			GlobalSupport.GameSpeed = settings.GameSpeed;
		}
	}
}


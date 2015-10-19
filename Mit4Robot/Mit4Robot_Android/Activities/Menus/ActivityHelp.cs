
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
	[Activity(Label = "Help")]			
	public class ActivityHelp : Activity
	{
		private Button btnTiles, btnVariables, btnFunctions, btnLanguage, btnMainMenu;
		private PopupMenu popupHelp;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
		
			// Create your application here
			SetContentView(Resource.Layout.Help);

			btnTiles = FindViewById<Button>(Resource.Id.btnHelpTiles);

			btnTiles.Click += (object sender, EventArgs e) =>
			{
				CreateTilesPopUp();
			};

			btnVariables = FindViewById<Button>(Resource.Id.btnHelpVariables);

			btnVariables.Click += (object sender, EventArgs e) =>
			{
				ShowPopUpMessage(0);
			};

			btnFunctions = FindViewById<Button>(Resource.Id.btnHelpFunctions);

			btnFunctions.Click += (object sender, EventArgs e) =>
			{
				ShowPopUpMessage(803);
			};

			btnLanguage = FindViewById<Button>(Resource.Id.btnHelpLanguage);

			btnLanguage.Click += (object sender, EventArgs e) =>
			{
				if (GlobalSupport.GameLanguage == EGameLanguage.None || GlobalSupport.GameLanguage == 0)
				{
					CreateLanguagePopUp();
				}
				else
				{
					ShowPopUpMessage((int)GlobalSupport.GameLanguage);
				}
			};

			btnMainMenu = FindViewById<Button>(Resource.Id.btnMainMenu);
			btnMainMenu.Click += (object sender, EventArgs e) =>
			{
				var mainMenu = new Intent(this, typeof(MainActivity));
				StartActivity(mainMenu);
			};
		}

		public void CreateTilesPopUp()
		{
			popupHelp = new PopupMenu(this, btnTiles);

			popupHelp.Menu.Add(800, 800, 800, "Map");
			popupHelp.Menu.Add(801, 801, 801, "Road");
			popupHelp.Menu.Add(802, 802, 802, "Shop");

			popupHelp.MenuItemClick += (sender, e) =>
			{
				ShowPopUpMessage(e.Item.ItemId);
			};

			popupHelp.Show();
		}

		public void CreateLanguagePopUp()
		{
			popupHelp = new PopupMenu(this, btnLanguage);

			popupHelp.Menu.Add(500, 500, 500, "Python");
			popupHelp.Menu.Add(501, 501, 501, "Pascal");

			popupHelp.MenuItemClick += (sender, e) =>
			{
				ShowPopUpMessage(e.Item.ItemId);
			};

			popupHelp.Show();
		}

		public void ShowPopUpMessage(int text)
		{
			AlertDialog alert = new AlertDialog.Builder(this).SetNegativeButton(Resource.String.btnClose, (object s,
				                                                                                                DialogClickEventArgs eventClick) =>
				{
				}).Create();

			LinearLayout dialogLayout = new LinearLayout(this);
			dialogLayout.Orientation = Orientation.Vertical;
		
			TextView txtHelp = new TextView(this);
			;
			Button btnCancel = new Button(this);
			ImageView imgHelpView = null;

			btnCancel.SetText(Resource.String.btnCancel);

			imgHelpView = new ImageView(this);
				
			switch (text)
			{
				case 0:
					txtHelp.SetText(Resource.String.txtHelpVariables);
					imgHelpView.SetImageResource(Resource.Drawable.HelpVariable);
					break;
				case 500:
					txtHelp.SetText(Resource.String.txtHelpPython);
					imgHelpView.SetImageResource(Resource.Drawable.HelpPython);
					break;
				case 501:
					txtHelp.SetText(Resource.String.txtHelpPascal);
					imgHelpView.SetImageResource(Resource.Drawable.HelpPascal);
					break;
				case 800:
					txtHelp.SetText(Resource.String.txtHelpMap);
					imgHelpView.SetImageResource(Resource.Drawable.HelpMap);
					break;
				case 801:
					txtHelp.SetText(Resource.String.txtHelpRoad);
					imgHelpView.SetImageResource(Resource.Drawable.HelpRoad);
					break;
				case 802:
					txtHelp.SetText(Resource.String.txtHelpShop);
					imgHelpView.SetImageResource(Resource.Drawable.HelpShop);
					break;
				case 803:
					txtHelp.SetText(Resource.String.txtHelpFunctions);
					imgHelpView.SetImageResource(Resource.Drawable.blue);
					break;
			}

			if (imgHelpView != null)
			{
				dialogLayout.AddView(imgHelpView);
			}

			dialogLayout.AddView(txtHelp);

			ScrollView scrollPane = new ScrollView(this);
			scrollPane.AddView(dialogLayout);
			alert.SetView(scrollPane);

			alert.Show();
		}
	}
}


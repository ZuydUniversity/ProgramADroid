using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using Shared;
using CoreGraphics;

namespace Mit4RobotApp
{
	public class TBSAchievements : UITableViewSource
	{
		public event EventHandler<RowSelectedEventArgs> OnRowSelected;

		private string[] achievements;
		private string cellIdentifier = "cell";

		public TBSAchievements(string[] achievements)
		{
			this.achievements = achievements; 
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return achievements.Length;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			return 100;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
			}

			string[] achievementText = achievements[indexPath.Row].Split('=');

			cell.TextLabel.Text = achievementText[0];
			cell.TextLabel.Lines = 3;

			cell.TextLabel.LineBreakMode = UILineBreakMode.WordWrap;

			cell.TextLabel.TextColor = UIColor.Black;

			if (achievementText[1] == "X")
			{
				cell.BackgroundColor = new UIColor((nfloat)0.75, 0, 0, (nfloat)1.0);
			}

			else
			{
				cell.BackgroundColor = GlobalSupport.AchievementUnlocked;
			}

			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (OnRowSelected != null)
			{
				OnRowSelected(this, new RowSelectedEventArgs(tableView, indexPath));
			}
		}

		public class RowSelectedEventArgs : EventArgs
		{
			public UITableView tableView { get; set; }

			public NSIndexPath indexPath { get; set; }

			public RowSelectedEventArgs(UITableView tableView, NSIndexPath indexPath) : base()
			{ 
				this.tableView = tableView;
				this.indexPath = indexPath;
			}
		}
	}
}

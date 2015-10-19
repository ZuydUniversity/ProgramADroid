using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Mit4RobotApp
{
	public class TBSHighScores : UITableViewSource
	{
		public event EventHandler<RowSelectedEventArgs> OnRowSelected;

		private string[] highScoreItems;
		private string cellIdentifier = "cell";

		public TBSHighScores(string[] highScoreItems)
		{
			this.highScoreItems = highScoreItems; 
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return highScoreItems.Length;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
			}

			cell.TextLabel.Text = highScoreItems[indexPath.Row];
			cell.TextLabel.TextColor = UIColor.White;
			cell.BackgroundColor = UIColor.Black;

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

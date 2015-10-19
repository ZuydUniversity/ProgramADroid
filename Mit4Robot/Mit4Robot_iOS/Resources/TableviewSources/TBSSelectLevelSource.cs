using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Mit4RobotApp
{
	public class TBSSelectLevelSource : UITableViewSource
	{
		public event EventHandler<RowSelectedEventArgs> OnRowSelected;

		private string[] levelItems;
		private string cellIdentifier = "cell";

		public TBSSelectLevelSource(string[] levelItems)
		{
			this.levelItems = levelItems; 
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return levelItems.Length;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
			}

			cell.TextLabel.Text = levelItems[indexPath.Row];
			cell.BackgroundColor = UIColor.Black;
			cell.TextLabel.TextColor = UIColor.White;

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

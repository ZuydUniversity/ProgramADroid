using System;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using Shared.Exceptions;

namespace Shared.BusinessLayer
{
	public class MainCode: Composite
	{
		public MainCode ()
		{
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Execute ASync for testing purposes
		/// </summary>
		public bool execute()
		{
			foreach (ICodeBlock codeBlock in children) {
				bool result = codeBlock.execute (this);
				if (!result) {
					return false;
				}
			}
			return true;
		}

		/// Author:	Guy Spronck
		/// Date:	19-06-2015
		/// <summary>
		/// for use as base.execute()
		/// Adds references to all the varibles of the scope of the parent to the scope of the Compisite being executed.
		/// </summary>
		/// <param name="parent">Parent of the Compisite you are executing</param>
		public override bool execute (Composite parent){
			BackgroundWorker bw = new BackgroundWorker ();

			bw.WorkerReportsProgress = true;

			bw.DoWork += (object sender, DoWorkEventArgs e) => {
				Robot.bg = sender as BackgroundWorker;
				Thread.Sleep(GlobalSupport.GameSpeed);
				foreach (ICodeBlock codeBlock in children) {
					codeBlock.execute (this);
					Thread.Sleep (GlobalSupport.GameSpeed);
				}
			};


			bw.ProgressChanged += new ProgressChangedEventHandler (delegate(object o, ProgressChangedEventArgs args) {
				// Update progress, redraw robot
				Robot instance = Robot.Instance;
				instance.UpdateGUI(args.ProgressPercentage);
			});

			bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler (delegate(object o, RunWorkerCompletedEventArgs args) {
					// Thread finished -- Check if correct
					Robot.checkGoalsEvent(this, args);
			});
				
			bw.RunWorkerAsync (); // Start the worker
			return false;
		}

	}
}


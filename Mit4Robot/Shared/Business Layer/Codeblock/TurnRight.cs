using System;

namespace Shared.BusinessLayer
{
	public class TurnRight : Command
	{
		public TurnRight ()
		{
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Executes a turn right command to the robot.
		/// </summary>
		/// <param name="parent">Parent.</param>
		public override bool execute (Composite parent)
		{
			Robot robot = Robot.Instance;
			if (Robot.bg != null) {
				Robot.bg.ReportProgress (lineNumber);
			}
			robot.Moves++;
			robot.RotateRight (lineNumber);
			return true;
		}
	}
}


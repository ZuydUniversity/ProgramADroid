using System;
using System.Threading.Tasks;
using Shared.Exceptions;

namespace Shared.BusinessLayer
{
	public class Forward : Command
	{
		public Forward ()
		{
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Executes a walk 1 step forward command for the robot.
		/// </summary>
		/// <param name="parent">Parent.</param>
		public override bool execute (Composite parent)
		{
			try{
				Robot robot= Robot.Instance;
				if (Robot.bg != null) {
					Robot.bg.ReportProgress (lineNumber);
				}
				robot.Moves++;
				robot.Forward(lineNumber);
			return true;
			} catch (RobotException ex){
				throw ex;
			}
		}
	}
}


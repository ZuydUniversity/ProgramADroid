using System;
using Shared.Exceptions;

namespace Shared.BusinessLayer
{
	public class PickUp : Command
	{
		private string objectToPickUP;

		public string ObjectToPickUP {
			get {
				return objectToPickUP;
			}
		}

		public PickUp (string objectToPickUp)
		{
			this.objectToPickUP = objectToPickUp;
		}

		public override bool execute (Composite parent)
		{
			try{
				Robot robot = Robot.Instance;
				if (Robot.bg != null) {
					Robot.bg.ReportProgress (lineNumber);
				}
				robot.PickUp(lineNumber, ObjectToPickUP);
			return false;
			}
			catch(RobotException ex){
				throw ex;
			}
			catch(MapException ex){
				throw ex;
			}
		}
	}
}


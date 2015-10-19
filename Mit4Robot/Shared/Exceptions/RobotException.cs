using System;

namespace Shared.Exceptions
{
	/// <summary>
	/// Exception for the Robot
	/// </summary>
	public class RobotException:Exception
	{
		public RobotException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the RobotException/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <created>Stef Chappin</created>
		public RobotException(string message):base(message){
			
		}
	}
}


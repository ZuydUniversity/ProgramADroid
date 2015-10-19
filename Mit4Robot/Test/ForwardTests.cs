using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using Shared;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Shared.BusinessLayer;
using Shared.Exceptions;
using Shared.Enums;


namespace Test
{
	[TestFixture ()]
	public class ForwardTests
	{
		public ForwardTests ()
		{
		}

		#region ForwardTests
		/// <summary>
		/// Move forward if th orientation is East
		/// </summary>
		[Test()]
		public void ForwardGoEast(){
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.Forward (1);
			int expected = 1;
			int actual = robot.xPosition;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Move forward if the orientation is South, but in the South isn't a road
		/// </summary>
		[Test()]
		[ExpectedException(typeof(RobotException))]
		public void ForwardGoSouthException(){
			EOrientation orientation = EOrientation.South;
			Robot robot = Robot.Create (orientation, new Map (EDifficulty.Easy));
			robot.xPosition = 2;
			robot.yPosition = 2;
			robot.Forward (1);
			Assert.Fail ("Expected robotException with the message can't move forward.");
		}

		/// <summary>
		/// Move forward if th orientation is South
		/// </summary>
		[Test()]
		public void ForwardGoSouth(){
			EOrientation orientation = EOrientation.South;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.xPosition = 1;
			robot.Forward (1);
			int expected = 1;
			int actual = robot.yPosition;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Move forward if th orientation is West
		/// </summary>
		[Test()]
		public void ForwardGoWest(){
			EOrientation orientation = EOrientation.West;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.xPosition = 1;
			robot.Forward (1);
			int expected = 0;
			int actual = robot.xPosition;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Move forward if th orientation is North
		/// </summary>
		[Test()]
		public void ForwardGoNorth(){
			EOrientation orientation = EOrientation.North;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.xPosition = 1;
			robot.yPosition = 1;
			robot.Forward (1);
			int expected = 0;
			int actual = robot.yPosition;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Move forward if the orientation is West, but in the West isn't a road
		/// </summary>
		[Test()]
		[ExpectedException(typeof(RobotException))]
		public void ForwardGoWestException(){
			EOrientation orientation = EOrientation.West;
			Robot robot = Robot.Create (orientation, new Map (EDifficulty.Easy));
			robot.Forward (1);
			Assert.Fail ("Expected robotException with the message can't move forward.");
		}

		/// <summary>
		/// Move forward if the orientation is North, but in the North isn't a road
		/// </summary>
		[Test()]
		[ExpectedException(typeof(RobotException))]
		public void ForwardGoNorthException(){
			EOrientation orientation = EOrientation.North;
			Robot robot = Robot.Create (orientation, new Map (EDifficulty.Easy));
			robot.Forward (1);
			Assert.Fail ("Expected robotException with the message can't move forward.");
		}

		/// <summary>
		/// Move forward if the orientation is East, but in the East isn't a road
		/// </summary>
		[Test()]
		[ExpectedException(typeof(RobotException))]
		public void ForwardGoEastException(){
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map (EDifficulty.Easy));
			robot.xPosition = 1;
			robot.yPosition = 1;
			robot.Forward (1);
			Assert.Fail ("Expected robotException with the message can't move forward.");
		}
		#endregion
	}
}


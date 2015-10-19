using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using Shared;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Shared.BusinessLayer;
using Shared.Enums;
using Shared.Exceptions;

namespace Test
{	
	[TestFixture ()]
	public class checkGoalsTest
	{
		/// <summary>
		/// Checks if the robot has the inventory of the test map
		/// </summary>
		[Test()]
		public void checkGoalsTest1 ()
		{
			Robot robot = Robot.Create (EOrientation.East, new Map(EDifficulty.Easy));
			robot.xPosition = 0;
			robot.yPosition = 2;
			robot.PickUp (0, "Sausage");
			robot.xPosition = 2;
			robot.yPosition = 2;
			robot.PickUp (0, "Cabbage");
			robot.xPosition = 0;
			robot.yPosition = 0;
			bool actual = robot.checkGoals ();
			bool expected = true;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Checks if the robot has the inventory of the test map with only 1 item
		/// </summary>
		[Test()]
		public void checkGoalsTest2 ()
		{
			Robot robot = Robot.Create (EOrientation.East, new Map(EDifficulty.Easy));
			robot.xPosition = 0;
			robot.yPosition = 2;
			robot.PickUp (0, "Sausage");
			robot.xPosition = 0;
			robot.yPosition = 0;
			bool actual = robot.checkGoals ();
			bool expected = false;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Checks if the robot has the inventory of the test map with the wrong item
		/// </summary>
		[Test()]
		[ExpectedException(typeof(RobotException))]
		public void checkGoalsTest3 ()
		{
			Robot robot = Robot.Create (EOrientation.East, new Map(EDifficulty.Easy));
			robot.xPosition = 0;
			robot.yPosition = 2;
			robot.PickUp (0, "Sausage");
			robot.PickUp (0, "Ham");
			robot.xPosition = 0;
			robot.yPosition = 0;
			bool actual = robot.checkGoals ();
			Assert.Fail ();
		}

		/// <summary>
		/// Checks if the robot has the inventory of the test map at the wrong position
		/// </summary>
		[Test()]
		public void checkGoalsTest4 ()
		{
			Robot robot = Robot.Create (EOrientation.East, new Map(EDifficulty.Easy));
			robot.xPosition = 0;
			robot.yPosition = 2;
			robot.PickUp (0, "Sausage");
			robot.xPosition = 2;
			robot.yPosition = 2;
			robot.PickUp (0, "Cabbage");
			robot.xPosition = 1;
			robot.yPosition = 1;
			bool actual = robot.checkGoals ();
			bool expected = false;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Checks if the robot has the inventory of the test map at the right position with uppercase objects
		/// </summary>
		[Test()]
		public void checkGoalsTest5 ()
		{
			Robot robot = Robot.Create (EOrientation.East, new Map(EDifficulty.Easy));
			robot.xPosition = 0;
			robot.yPosition = 2;
			robot.PickUp (0, "sausage");
			robot.xPosition = 2;
			robot.yPosition = 2;
			robot.PickUp (0, "cabbage");
			robot.xPosition = 0;
			robot.yPosition = 0;
			bool actual = robot.checkGoals ();
			bool expected = true;
			Assert.AreEqual (expected, actual);
		}
	}
}


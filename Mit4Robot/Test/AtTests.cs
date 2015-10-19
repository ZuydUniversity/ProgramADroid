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

namespace Test
{
	[TestFixture ()]
	public class AtTests
	{
		public AtTests ()
		{
		}

		#region At

		/// <summary>
		/// Checks if the Robot is at the Butcher
		/// </summary>
		[Test()]
		public void AtButcher(){
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.yPosition = 2;
			bool actual = robot.At ("Butcher");
			bool expected = true;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Checks if the Robot is at the Greengrocer
		/// </summary>
		[Test()]
		public void AtGreengrocer(){
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.xPosition = 2;
			robot.yPosition = 2;
			bool actual = robot.At ("greengrocer");
			bool expected = true;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Checks if the Robot is at the Greengrocer if the Robot is at the Butcher
		/// </summary>
		[Test()]
		public void AtButcherCheckForGreengrocer(){
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.yPosition = 2;
			bool actual = robot.At ("Greengrocer");
			bool expected = false;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Checks if the Robot is at the Butcher if the Robot is at a road
		/// </summary>
		[Test()]
		public void AtNoShop(){
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			bool actual = robot.At ("Butcher");
			bool expected = false;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void AtHome(){
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map (EDifficulty.Easy));
			robot.xPosition = 0;
			robot.yPosition = 0;
			bool actual = robot.At ("Home");
			bool expected = true;
			Assert.AreEqual (expected, actual);
		}
		#endregion
	}
}


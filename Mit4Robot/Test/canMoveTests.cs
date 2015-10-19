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
	public class canMoveTests
	{
		#region canMove Tests
		[Test()]
		public void canMoveForward()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			bool actual = robot.canMove (ECanInstructions.Forward);
			bool expected = true;

			Assert.AreEqual (actual, expected);
		}

		[Test()]
		public void canMoveBackward()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			bool actual = robot.canMove (ECanInstructions.Backward);
			bool expected = false;

			Assert.AreEqual (actual, expected);
		}

		[Test()]
		public void canMoveLeft()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			bool actual = robot.canMove (ECanInstructions.Left);
			bool expected = false;

			Assert.AreEqual (actual, expected);
		}

		[Test()]
		public void canMoveRight()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			bool actual = robot.canMove (ECanInstructions.Right);
			bool expected = false;

			Assert.AreEqual (actual, expected);
		}
		#endregion
	}
}


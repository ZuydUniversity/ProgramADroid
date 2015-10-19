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
	public class PickUpTests
	{
		public PickUpTests ()
		{
		}

		#region PickUpTests
		/// <summary>
		/// Picks up sausage at the Butcher
		/// </summary>
		[Test()]
		public void PickUpSausageAtTheRightShop(){
			EOrientation orientation = EOrientation.West;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.yPosition = 2;
			robot.PickUp(1, "Sausage");
			List<string> expected = new List<string> ();
			expected.Add ("Sausage");
			List<string> actual = robot.inventory;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Picks up sausage at the Butcher
		/// </summary>
		[Test()]
		public void PickUpSausageAtTheRightShopLowerCase(){
			EOrientation orientation = EOrientation.West;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.yPosition = 2;
			robot.PickUp(1, "sausage");
			List<string> expected = new List<string> ();
			expected.Add ("Sausage");
			List<string> actual = robot.inventory;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Picks up sausage but there isn't a shop at the robot's location
		/// </summary>
		[Test()]
		[ExpectedException(typeof(MapException))]
		public void PickUpSausageWithNoShop(){
			EOrientation orientation = EOrientation.West;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.PickUp(1, "Sausage");
			Assert.Fail ("There is no sausage at the x=0 and y=0");
		}

		/// <summary>
		/// Picks up cabbage at the Butcher
		/// </summary>
		[Test()]
		[ExpectedException(typeof(RobotException))]
		public void PickUpCabbageAtButcher(){
			EOrientation orientation = EOrientation.West;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.yPosition = 2;
			robot.PickUp(1, "Cabbage");
			Assert.Fail ("There is no cabbage at the x=0 and y=2");
		}

		/// <summary>
		/// Picks up steak which the Butcher doesn't sell
		/// </summary>
		[Test()]
		[ExpectedException(typeof(RobotException))]
		public void PickUpItemThatDoesntExistAtButcher(){
			EOrientation orientation = EOrientation.West;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.yPosition = 2;
			robot.PickUp(1, "Steak");
			Assert.Fail ("Thereis no Steak at the x=0 and y=2");
		}
		#endregion
	}
}


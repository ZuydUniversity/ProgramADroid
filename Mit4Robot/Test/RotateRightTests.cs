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
	public class RotateRightTests
	{
		public RotateRightTests ()
	{
	}
			#region RotateRightTests
			/// <summary>
			/// Rotates the robot from North to the East
			/// </summary>
			[Test()]
			public void RotateRightEast(){
				Robot robot = Robot.Instance;
				robot.orientationEnum = EOrientation.North;
				robot.RotateRight (1);
				EOrientation actual = robot.orientationEnum;
				EOrientation expected = EOrientation.East;
				Assert.AreEqual (expected,actual);

			}

			/// <summary>
			/// Rotates the robot from East to the South
			/// </summary>
			[Test()]
			public void RotateRightSouth(){
				Robot robot = Robot.Instance;
				robot.orientationEnum = EOrientation.East;
				robot.RotateRight (1);
				EOrientation actual = robot.orientationEnum;
				EOrientation expected = EOrientation.South;
				Assert.AreEqual (expected,actual);
			}

			/// <summary>
			/// Rotates the robot from South to the West
			/// </summary>
			[Test()]
			public void RotateRightWest(){
				Robot robot = Robot.Instance;
				robot.orientationEnum = EOrientation.South;
				robot.RotateRight (1);
				EOrientation actual = robot.orientationEnum;
				EOrientation expected = EOrientation.West;
				Assert.AreEqual (expected,actual);
			}

			/// <summary>
			/// Rotates the robot from West to the North
			/// </summary>
			[Test()]
			public void RotateRightNorth(){
				Robot robot = Robot.Instance;
				robot.orientationEnum = EOrientation.West;
				robot.RotateRight (1);
				EOrientation actual = robot.orientationEnum;
				EOrientation expected = EOrientation.North;
				Assert.AreEqual (expected,actual);
			}
			#endregion

		}
	}



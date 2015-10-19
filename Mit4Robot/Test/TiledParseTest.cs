using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using Shared;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using System.IO;
using System.Xml;
using Shared.Parsers.MapParsers;
using Shared.BusinessLayer;
using Shared.Exceptions;
using Shared.Enums;

namespace Test
{
	[TestFixture ()]
	public class TiledParseTest
	{
		private string mapPath = "..\\..\\..\\..\\maps";
		[Test()]
		public void parseMap()
		{
			Stream stream = File.Open (mapPath + "\\Medium\\Medium01.tmx", FileMode.Open);
			TiledParser.Parse(stream, "testMap");
		}

		[Test()]
		[ExpectedException(typeof(MapParseException))]
		public void parseMap2()
		{
			Stream stream = File.Open (mapPath + "\\Medium\\Medium1_2.tmx", FileMode.Open);
			TiledParser.Parse(stream, "testMap");
		}

		[Test()]
		public void parseMap1()
		{
			Stream stream = File.Open (mapPath + "\\Medium\\Medium12.tmx", FileMode.Open);
			TiledParser.Parse(stream, "testMap");
		}

		[Test()]
		[ExpectedException(typeof(MapParseException))]
		public void parseNone()
		{
			TiledParser.Parse(null, "");
		}

		[Test()]
		[ExpectedException(typeof(MapParseException))]
		public void parseNone2()
		{
			TiledParser.Parse(null, "");
		}

		[Test()]
		public void checkParseMapForward()
		{
			Stream stream = File.Open (mapPath + "\\Medium\\Medium01.tmx", FileMode.Open);
			Robot rbt = TiledParser.Parse(stream, "");

			bool actual = rbt.canMove (ECanInstructions.Forward);
			bool expected = true;

			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void checkParseMapForward1()
		{
			Stream stream = File.Open (mapPath + "\\Medium\\Medium02.tmx", FileMode.Open);
			Robot rbt = TiledParser.Parse(stream, "");

			bool actual = rbt.canMove (ECanInstructions.Forward);
			bool expected = true;

			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void checkParseMapForward2()
		{
			Stream stream = File.Open (mapPath + "\\Medium\\Medium03.tmx", FileMode.Open);
			Robot rbt = TiledParser.Parse(stream, "");

			bool actual = rbt.canMove (ECanInstructions.Forward);
			bool expected = false;

			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void checkParseMapLeft()
		{
			Stream stream = File.Open (mapPath + "\\Medium\\Medium01.tmx", FileMode.Open);
			Robot rbt = TiledParser.Parse(stream, "");

			bool actual = rbt.canMove (ECanInstructions.Left);
			bool expected = false;

			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void checkParseMapLeft1()
		{
			Stream stream = File.Open (mapPath + "\\Medium\\Medium02.tmx", FileMode.Open);
			Robot rbt = TiledParser.Parse(stream, "");

			bool actual = rbt.canMove (ECanInstructions.Left);
			bool expected = true;

			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void checkParseMapRight()
		{
			Stream stream = File.Open (mapPath + "\\Medium\\Medium01.tmx", FileMode.Open);
			Robot rbt = TiledParser.Parse(stream, "");

			bool actual = rbt.canMove (ECanInstructions.Right);
			bool expected = false;

			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void checkParseMapBackward()
		{
			Stream stream = File.Open (mapPath + "\\Medium\\Medium01.tmx", FileMode.Open);
			Robot rbt = TiledParser.Parse(stream, "");

			bool actual = rbt.canMove (ECanInstructions.Backward);
			bool expected = false;

			Assert.AreEqual (expected, actual);
		}
	}
}


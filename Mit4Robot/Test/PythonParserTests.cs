using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using Shared;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Shared.Utilities;
using Shared.Parsers.CodeParsers;
using Shared.BusinessLayer;
using Shared.Enums;

namespace Test
{
	[TestFixture ()]
	public class PythonParserTests
	{

		#region PythonIFStatementParsertests

		[Test()]
		public void PythonParseSimpleIF(){
			string tokenizeString = "if left:\n" +
			                        "\tmoveForward()\n" +
			                        "\trotateRight()\n";
			PythonParser p = new PythonParser ();
			List<ICodeBlock> block = p.ParseCode (tokenizeString);
			Assert.AreEqual (1, block.Count);
			Assert.AreEqual (2, (block [0] as IfStatement).getChildren ().Count);
			Assert.IsInstanceOf<IfStatement> (block [0]);
		}

		[Test()]
		public void PythonParseSimpleELIF(){
			string tokenizeString = "if left:\n" +
			                        "\tmoveForward()\n" +
			                        "\trotateRight()\n" +
			                        "elif right:\n" +
			                        "\tmoveForward()\n";
			PythonParser p = new PythonParser ();
			List<ICodeBlock> block = p.ParseCode (tokenizeString);
			Assert.AreEqual (1, block.Count);
			Assert.AreEqual (2, (block [0] as IfStatement).getChildren ().Count);
			Assert.IsInstanceOf<IfStatement> (block [0]);
		}

		[Test()]
		public void PythonParseSimpleELSE(){
			string tokenizeString = "if left:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n" +
				"else:\n" +
				"\tmoveForward()\n" +
				"\tmoveForward()\n" +
				"\tmoveForward()\n";
			PythonParser p = new PythonParser ();
			List<ICodeBlock> block = p.ParseCode (tokenizeString);
			Assert.AreEqual (1, block.Count);
			Assert.AreEqual (2, (block [0] as IfStatement).getChildren ().Count);
			Assert.IsInstanceOf<IfStatement> (block [0]);
		}

		[Test()]
		public void PythonParseNestedIF(){
			string tokenizeString = "if left:\n" +
			                        "\tmoveForward()\n" +
			                        "\tif right:\n" +
			                        "\t\tmoveForward()\n" +
			                        "\trotateRight()\n";
			PythonParser p = new PythonParser ();
			List<ICodeBlock> block = p.ParseCode (tokenizeString);
			Assert.AreEqual (1, block.Count);
			Assert.AreEqual (3, (block [0] as IfStatement).getChildren ().Count);
			Assert.IsInstanceOf<IfStatement> (block [0]);
			Assert.IsInstanceOf<IfStatement>((block [0] as IfStatement).getChildren ().ElementAt(1));

		}

		[Test()]
		public void PythonParseNestedIF2(){
			string tokenizeString = "if left:\n" +
				"\tmoveForward()\n" +
				"\tif right:\n" +
				"\t\tmoveForward()\n" +
				"\trotateRight()\n" +
				"\trotateRight()\n";
			PythonParser p = new PythonParser ();
			List<ICodeBlock> block = p.ParseCode (tokenizeString);
			Assert.AreEqual (1, block.Count);
			Assert.AreEqual (4, (block [0] as IfStatement).getChildren ().Count);
			Assert.IsInstanceOf<IfStatement> (block [0]);
			Assert.IsInstanceOf<IfStatement>((block [0] as IfStatement).getChildren ().ElementAt(1));

		}

		[Test()]
		public void PythonParseNestedIF3(){
			string tokenizeString = "if left:\n" +
				"\tmoveForward()\n" +
				"\tif right:\n" +
				"\t\tmoveForward()\n" +
				"\trotateRight()\n" +
				"rotateRight()\n";
			PythonParser p = new PythonParser ();
			List<ICodeBlock> block = p.ParseCode (tokenizeString);
			Assert.AreEqual (2, block.Count);
			Assert.AreEqual (3, (block [0] as IfStatement).getChildren ().Count);
			Assert.IsInstanceOf<IfStatement> (block [0]);
			Assert.IsInstanceOf<IfStatement>((block [0] as IfStatement).getChildren ().ElementAt(1));

		}


		#endregion

		#region PythonLexertest




		[Test()]
		public void PythonLexerTest(){
			string tokenizeString = "while left:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n" +
				"moveForward()\n" +
				"moveForward()\n" +
				"rotateRight()\n";
			//var tokens = lexer.Tokenize(tokenizeString);
			PythonParser p = new PythonParser ();
			p.ParseCode (tokenizeString);
			Assert.AreEqual (17,17);
		}

		[Test()]
		public void PythonLexerSimpleStatementTest(){
			string tokenizeString = "moveForward()\n" +
				"moveForward()\n" +
				"rotateRight()\n" +
				"moveForward()\n" +
				"rotateRight()\n";
			PythonParser p = new PythonParser ();
			List<ICodeBlock> block = p.ParseCode (tokenizeString);
			Assert.AreEqual (5, block.Count);
		}

		[Test()]
		public void PythonLexerSingleWhileStatementTest(){
			string tokenizeString = "while left:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n" +
				"moveForward()\n" +
				"rotateRight()\n";
			PythonParser p = new PythonParser ();
			List<ICodeBlock> block = p.ParseCode (tokenizeString);
			Assert.AreEqual (3, block.Count);
		}

		[Test()]
		public void PythonLexerNestedWhileStatementTest(){
			string tokenizeString = "while left:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n" +
				"\twhile right:\n" +
				"\t\trotateRight()\n" +
				"\t\trotateRight()\n" +
				"\tmoveForward()\n" +
				"moveForward()\n" +
				"rotateRight()\n";
			PythonParser p = new PythonParser ();
			List<ICodeBlock> block = p.ParseCode (tokenizeString);
			Assert.AreEqual (3, block.Count);
		}

		[Test()]
		public void PythonLexerDoubleNestedWhileStatementTest(){
			string tokenizeString = "while left:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n" +
				"\twhile right:\n" +
				"\t\trotateRight()\n" +
				"\t\twhile right:\n" +
				"\t\t\trotateRight()\n" +
				"\t\t\trotateRight()\n" +
				"\t\trotateRight()\n" +
				"\tmoveForward()\n" +
				"moveForward()\n" +
				"rotateRight()\n";
			PythonParser p = new PythonParser ();
			List<ICodeBlock> block = p.ParseCode (tokenizeString);
			Assert.AreEqual (3, block.Count);
			Assert.IsInstanceOf<WhileLoop> (block [0]);
		}
		#endregion

		#region BuildConditionFromTokenTests

//		/// <summary>
//		/// Test the condition code on a single statement
//		/// </summary>
//		[Test()]
//		public void TestConditionCode(){
//			ILexer pythonLexer;
//			pythonLexer = new Lexer ();
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.WHILE,
//				new Regex (@"(while)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.LogicalOperator,
//				new Regex (@"(or|and|not)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.Command,
//				new Regex (@"(moveForward\(\)|rotateRight\(\)|pickUp\('([^)]+)\))")));
//
//
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.IF,
//				new Regex (@"(if|elif|else)")));
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.startBlock,
//				new Regex (@"(:)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.logicInstruction,
//				new Regex (@"(left|right|forward|backward)")));
//
//			Regex endOfLineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);
//			pythonLexer.AddDefinition(new TokenDefinition(ETokenType.EOL,endOfLineRegex));
//			pythonLexer.AddDefinition(new TokenDefinition(
//				ETokenType.WhiteSpace,
//				new Regex(@"[ \t]")));
//			List<Token> tokensToParse = pythonLexer.Tokenize ("left:").ToList();
//			PythonParser p = new PythonParser ();
//			ICondition conditons = p.BuildICondition (tokensToParse,0);
//			Assert.IsInstanceOf<ConcreteInstruction> (conditons);
//
//		}

//		/// <summary>
//		/// Test if parser understands statements with a single logic operator
//		/// </summary>
//		[Test()]
//		public void TestConditionANDCode(){
//			ILexer pythonLexer;
//			pythonLexer = new Lexer ();
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.WHILE,
//				new Regex (@"(while)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.LogicalOperator,
//				new Regex (@"(or|and|not)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.Command,
//				new Regex (@"(moveForward\(\)|rotateRight\(\)|pickUp\('([^)]+)\))")));
//
//
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.IF,
//				new Regex (@"(if|elif|else)")));
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.startBlock,
//				new Regex (@"(:)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.logicInstruction,
//				new Regex (@"(left|right|forward|backward)")));
//
//			Regex endOfLineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);
//			pythonLexer.AddDefinition(new TokenDefinition(ETokenType.EOL,endOfLineRegex));
//			pythonLexer.AddDefinition(new TokenDefinition(
//				ETokenType.WhiteSpace,
//				new Regex(@"[ \t]")));
//			List<Token> tokensToParse = pythonLexer.Tokenize ("left and right:").ToList();
//			PythonParser p = new PythonParser ();
//			ICondition conditions = p.BuildICondition (tokensToParse.Where(x => x.Type != ETokenType.WhiteSpace && x.Type != ETokenType.startBlock && x.Type != ETokenType.EOL).ToList(),0);
//			Assert.IsInstanceOf<ConditionCombo> (conditions);
//		}

//		/// <summary>
//		/// Test if parser understands statements with a multiple logic operator
//		/// </summary>
//		[Test()]
//		public void TestConditionMultipleOperatorCode(){
//			ILexer pythonLexer;
//			pythonLexer = new Lexer ();
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.WHILE,
//				new Regex (@"(while)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.LogicalOperator,
//				new Regex (@"(or|and|not)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.Command,
//				new Regex (@"(moveForward\(\)|rotateRight\(\)|pickUp\('([^)]+)\))")));
//
//
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.IF,
//				new Regex (@"(if|elif|else)")));
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.startBlock,
//				new Regex (@"(:)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.logicInstruction,
//				new Regex (@"(left|right|forward|backward)")));
//
//			Regex endOfLineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);
//			pythonLexer.AddDefinition(new TokenDefinition(ETokenType.EOL,endOfLineRegex));
//			pythonLexer.AddDefinition(new TokenDefinition(
//				ETokenType.WhiteSpace,
//				new Regex(@"[ \t]")));
//			List<Token> tokensToParse = pythonLexer.Tokenize ("forward or right and left or backward:").ToList();
//			PythonParser p = new PythonParser ();
//			ICondition condition = p.BuildICondition (tokensToParse.Where(x => x.Type != ETokenType.WhiteSpace && x.Type != ETokenType.startBlock && x.Type != ETokenType.EOF).ToList(),0);
//			Assert.IsInstanceOf<ConditionCombo> (condition);
//			Assert.AreEqual (ELogicOperators.Or, (condition as ConditionCombo).LogicOperator);
//			Solver left = (condition as ConditionCombo).LeftSolver;
//			Solver right = (condition as ConditionCombo).RightSolver;
//			Assert.IsInstanceOf<ConditionCombo> (left);
//			Assert.IsInstanceOf<ConcreteInstruction> (right);
//		}

//		/// <summary>
//		/// Test if parser understands statements with a multiple logic operator of the same type
//		/// </summary>
//		[Test()]
//		public void TestConditionMultipleOperatorOfSameTypeCode(){
//			ILexer pythonLexer;
//			pythonLexer = new Lexer ();
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.WHILE,
//				new Regex (@"(while)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.LogicalOperator,
//				new Regex (@"(or|and|not)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.Command,
//				new Regex (@"(moveForward\(\)|rotateRight\(\)|pickUp\('([^)]+)\))")));
//
//
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.IF,
//				new Regex (@"(if|elif|else)")));
//			pythonLexer.AddDefinition(new TokenDefinition (ETokenType.startBlock,
//				new Regex (@"(:)")));
//			pythonLexer.AddDefinition (new TokenDefinition (ETokenType.logicInstruction,
//				new Regex (@"(left|right|forward|backward)")));
//
//			Regex endOfLineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);
//			pythonLexer.AddDefinition(new TokenDefinition(ETokenType.EOL,endOfLineRegex));
//			pythonLexer.AddDefinition(new TokenDefinition(
//				ETokenType.WhiteSpace,
//				new Regex(@"[ \t]")));
//			List<Token> tokensToParse = pythonLexer.Tokenize ("forward or right and left or backward and forward:").ToList();
//			PythonParser p = new PythonParser ();
//			ICondition condition = p.BuildICondition (tokensToParse.Where(x => x.Type != ETokenType.WhiteSpace && x.Type != ETokenType.startBlock && x.Type != ETokenType.EOF).ToList(),0);
//			Assert.IsInstanceOf<ConditionCombo> (condition);
//			Assert.AreEqual (ELogicOperators.Or, (condition as ConditionCombo).LogicOperator);
//			Solver left = (condition as ConditionCombo).LeftSolver;
//			Solver right = (condition as ConditionCombo).RightSolver;
//			Assert.IsInstanceOf<ConditionCombo> (left);
//			Assert.IsInstanceOf<ConditionCombo> (right);
//		}

		#endregion

		#region GeneralLexerTests

		[Test()]
		public void LexerTest(){
			var lexer = new Lexer ();
			lexer.AddDefinition(new TokenDefinition(ETokenType.Invalid,
				new Regex(@"\*|\/|\+|\-")));

			lexer.AddDefinition(new TokenDefinition(
				ETokenType.Invalid,
				new Regex(@"\d+")));


			lexer.AddDefinition(new TokenDefinition(
				ETokenType.WhiteSpace,
				new Regex(@"\s+"),
				true));

			var tokens = lexer.Tokenize("1 * 2 / 3 + 4 - 5");
			Assert.AreEqual (17,17);
		}

		#endregion
	}
}


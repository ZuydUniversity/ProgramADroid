using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using Shared;
using System.Collections.Generic;
using Shared.Parsers.CodeParsers;
using Shared.BusinessLayer;
using Shared.Enums;
using Shared.Exceptions;
using NUnit;

namespace Test
{
	[TestFixture ()]
	public class PascalParserExpandedTests
	{
		const string conditionLeft = "left";
		const string conditionRight = "right";
		const string conditionForward = "forward";
		const string conditionBackward = "backward";

		const string moveForwardCommand = "moveForward();";
		const string rotateRightCommand = "rotateRight();";

		const string beginBlock = "begin";
		const string endBlock = "end;";
		const string simpleEnd = "end";
		const string endDot = "end.";

		private PascalParser pascalParser;
		private List<ICodeBlock> response;
		private String tokenize;

		[SetUp]
		public void BeforeTest(){
			pascalParser = new PascalParser ();
			response = new List<ICodeBlock> ();
			tokenize = "";
		}

		[Test]
		public void TestSimplePascalCommand(){
			tokenize = String.Format ("{0}\n", moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
		}

		[Test]
		public void TestSingleIfstatement1(){

			tokenize = String.Format ("if {0} then\n" +
				"begin\n" +
				"\t{1}\n" +
				"end;\n", conditionLeft, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (1, ifStatement.getChildren ().Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Solver solver = ifStatement.Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestSingleIfstatement2(){

			tokenize = String.Format ("if {0} then\n" +
				"begin\n" +
				"\t{1}\n" +
				"\t{1}\n" +
				"end;\n", conditionRight, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (2, ifStatement.getChildren ().Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [1]);
			Solver solver = ifStatement.Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Right, (solver as ConcreteInstruction).Instruction);

		}

		[Test]
		public void TestSingleIfstatement3(){
			tokenize = String.Format ("if {0} then\n" +
				"begin\n" +
				"\t{1}\n" +
				"\t{1}\n" +
				"\t{2}\n" +
				"end;\n", conditionForward, moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (3, ifStatement.getChildren ().Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [1]);
			Assert.IsInstanceOf<TurnRight> (ifStatement.getChildren () [2]);
			Solver solver = ifStatement.Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Forward, (solver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestComplexIfstatement1(){
			tokenize = String.Format ("if {0} and {1} then\n" +
				"begin\n" +
				"\t{2}\n" +
				"\t{2}\n" +
				"\t{3}\n" +
				"end;\n", conditionForward, conditionLeft, moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (3, ifStatement.getChildren ().Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [1]);
			Assert.IsInstanceOf<TurnRight> (ifStatement.getChildren () [2]);
			Solver solver = ifStatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			ConditionCombo combo = (solver as ConditionCombo);
			Assert.IsInstanceOf<ConcreteInstruction> (combo.LeftSolver);
			Assert.IsInstanceOf<ConcreteInstruction> (combo.RightSolver);
			Assert.AreEqual (ELogicOperators.And, combo.LogicOperator);
			Assert.AreEqual (ECanInstructions.Forward, (combo.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Left, (combo.RightSolver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestComplexIfstatement2(){
			tokenize = String.Format ("if not {0} then\n" +
				"begin\n" +
				"\t{1}\n" +
				"\t{1}\n" +
				"\t{2}\n" +
				"end;\n", conditionForward, moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (3, ifStatement.getChildren ().Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [1]);
			Assert.IsInstanceOf<TurnRight> (ifStatement.getChildren () [2]);
			Solver solver = ifStatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			ConditionCombo combo = (solver as ConditionCombo);
			Assert.IsInstanceOf<ConcreteInstruction> (combo.LeftSolver);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);
			Assert.AreEqual (ECanInstructions.Forward, (combo.LeftSolver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestComplexIfstatement3(){
			tokenize = String.Format ("if i < 10 then\n" +
				"begin\n" +
				"\t{0}\n" +
				"\t{0}\n" +
				"\t{1}\n" +
				"end;\n", moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (3, ifStatement.getChildren ().Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [1]);
			Assert.IsInstanceOf<TurnRight> (ifStatement.getChildren () [2]);
			Solver solver = ifStatement.Conditions;
			Assert.IsInstanceOf<ValueSolver> (solver);
			ValueSolver valueSolver = (solver as ValueSolver);
			Assert.AreEqual (EComparisonOperator.ValueLessThan, valueSolver.ComparisonOperator);
			Assert.IsInstanceOf<ConcreteVariable> (valueSolver.LeftSolver);
			Assert.IsInstanceOf<ConcreteVariable> (valueSolver.RightSolver);
			Assert.AreEqual ("i", (valueSolver.LeftSolver as ConcreteVariable).VariableName);
			Assert.AreEqual (10, (valueSolver.RightSolver as ConcreteVariable).MyVariable.Value);
		}

		[Test]
		public void TestComplexIfstatement4(){
			tokenize = String.Format ("if i < 10 or {0} then\n" +
				"begin\n" +
				"\t{1}\n" +
				"\t{1}\n" +
				"\t{2}\n" +
				"end;\n",conditionLeft, moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (3, ifStatement.getChildren ().Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [1]);
			Assert.IsInstanceOf<TurnRight> (ifStatement.getChildren () [2]);
			Solver solver = ifStatement.Conditions;

			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.IsInstanceOf<ValueSolver> ((solver as ConditionCombo).LeftSolver);
			Assert.IsInstanceOf<ConcreteInstruction> ((solver as ConditionCombo).RightSolver);

			ValueSolver valueSolver = ((solver as ConditionCombo).LeftSolver as ValueSolver);
			Assert.AreEqual (EComparisonOperator.ValueLessThan, valueSolver.ComparisonOperator);
			Assert.IsInstanceOf<ConcreteVariable> (valueSolver.LeftSolver);
			Assert.IsInstanceOf<ConcreteVariable> (valueSolver.RightSolver);
			Assert.AreEqual ("i", (valueSolver.LeftSolver as ConcreteVariable).VariableName);
			Assert.AreEqual (10, (valueSolver.RightSolver as ConcreteVariable).MyVariable.Value);

			ConcreteInstruction rightSolver = ((solver as ConditionCombo).RightSolver as ConcreteInstruction);
			Assert.AreEqual (ECanInstructions.Left, rightSolver.Instruction);
			Assert.AreEqual (ELogicOperators.Or, ((solver as ConditionCombo).LogicOperator));
		}


		[Test]
		public void TestIfElseStatement1(){
			tokenize = String.Format ("if {0} then\n" +
			"begin\n" +
			"\t{1}\n" +
			"\t{2}\n" +
			"end\n" +
			"else\n" +
			"begin\n" +
			"\t{2}\n" +
			"end;", conditionLeft, moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (2, ifStatement.getChildren ().Count);
			Assert.AreEqual (1, ifStatement.ElseChildren.Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> (ifStatement.getChildren () [1]);
			Assert.IsInstanceOf<TurnRight> (ifStatement.ElseChildren [0]);
		}


		[Test]
		public void TestIfElseStatement2(){
			tokenize = String.Format ("if {0} then\n" +
				"begin\n" +
				"\t{1}\n" +
				"end\n" +
				"else\n" +
				"begin\n" +
				"\t{2}\n" +
				"\t{2}\n" +
				"end;", conditionLeft, moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (1, ifStatement.getChildren ().Count);
			Assert.AreEqual (2, ifStatement.ElseChildren.Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> (ifStatement.ElseChildren [0]);
			Assert.IsInstanceOf<TurnRight> (ifStatement.ElseChildren [1]);
		}

		[Test]
		public void TestIfElseIfStatement1(){
			tokenize = String.Format ("if {0} then\n" +
				"begin\n" +
				"\t{1}\n" +
				"\t{2}\n" +
				"end\n" +
				"else if not {0} then\n" +
				"begin\n" +
				"\t{2}\n" +
				"end;", conditionLeft, moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (2, ifStatement.getChildren ().Count);
			Assert.AreEqual (1, ifStatement.ElseChildren.Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> (ifStatement.getChildren () [1]);
			Assert.IsInstanceOf<TurnRight> ((ifStatement.ElseChildren [0] as IfStatement).getChildren()[0]);
		}


		[Test]
		public void TestIfElseIfStatement2(){
			tokenize = String.Format ("if {0} then\n" +
				"begin\n" +
				"\t{1}\n" +
				"end\n" +
				"else if not {0} then\n" +
				"begin\n" +
				"\t{2}\n" +
				"\t{2}\n" +
				"end;", conditionLeft, moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (1, ifStatement.getChildren ().Count);
			Assert.AreEqual (1, ifStatement.ElseChildren.Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((ifStatement.ElseChildren [0] as IfStatement).getChildren()[0]);
			Assert.IsInstanceOf<TurnRight> ((ifStatement.ElseChildren [0] as IfStatement).getChildren()[1]);
		}

		[Test]
		public void TestIfElseIfElseStatement1(){
			tokenize = String.Format ("if {0} then\n" +
				"begin\n" +
				"\t{1}\n" +
				"\t{2}\n" +
				"end\n" +
				"else if not {0} then\n" +
				"begin\n" +
				"\t{2}\n" +
				"end\n" +
				"else\n" +
				"begin\n" +
				"\t{1}\n" +
				"end;\n", conditionLeft, moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (2, ifStatement.getChildren ().Count);
			Assert.AreEqual (1, ifStatement.ElseChildren.Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> (ifStatement.getChildren () [1]);
			Assert.IsInstanceOf<TurnRight> ((ifStatement.ElseChildren [0] as IfStatement).getChildren()[0]);
		}


		[Test]
		public void TestIfMultipleElseIFelse1(){
			tokenize = String.Format (
				"if {0} then \n" +
				"begin\n" +
				"\t{3}\n" +
				"end\n" +
				"else if {1} then \n" +
				"begin\n" +
				"\t{3}\n" +
				"end\n" +
				"else if {2} then \n" +
				"begin\n" +
				"\t{3}\n" +
				"end\n" +
				"else\n" +
				"begin\n" +
				"\t{3}\n" +
				"end;\n", conditionBackward, conditionLeft, conditionRight, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);

			IfStatement outerIfstatement = (response [0] as IfStatement);
			IfStatement innerIfstatement = outerIfstatement.ElseChildren[0] as IfStatement;
			IfStatement innerInnerIfStatemen = innerIfstatement.ElseChildren [0] as IfStatement;

			Assert.AreEqual (1, outerIfstatement.getChildren ().Count);
			Assert.AreEqual (1, innerIfstatement.getChildren ().Count);
			Assert.AreEqual (1, innerInnerIfStatemen.getChildren ().Count);
			Assert.AreEqual (1, innerInnerIfStatemen.ElseChildren.Count);

		}

		[Test]
		public void TestIfMultipleElseIFelse2(){
			tokenize = String.Format (
				"if {0} then \n" +
				"begin\n" +
				"\t{3}\n" +
				"end\n" +
				"else if {1} then \n" +
				"begin\n" +
				"\t{3}\n" +
				"\t{3}\n" +
				"end\n" +
				"else if {2} then \n" +
				"begin\n" +
				"\t{3}\n" +
				"\t{3}\n" +
				"\t{3}\n" +
				"end\n" +
				"else\n" +
				"begin\n" +
				"\t{4}\n" +
				"end;\n", conditionBackward, conditionLeft, conditionRight, rotateRightCommand, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);

			IfStatement outerIfstatement = (response [0] as IfStatement);
			IfStatement innerIfstatement = outerIfstatement.ElseChildren[0] as IfStatement;
			IfStatement innerInnerIfStatemen = innerIfstatement.ElseChildren [0] as IfStatement;

			Assert.AreEqual (1, outerIfstatement.getChildren ().Count);
			Assert.IsInstanceOf<TurnRight> (outerIfstatement.getChildren () [0]);

			Assert.AreEqual (2, innerIfstatement.getChildren ().Count);
			Assert.IsInstanceOf<TurnRight> (innerIfstatement.getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> (innerIfstatement.getChildren () [1]);

			Assert.AreEqual (3, innerInnerIfStatemen.getChildren ().Count);
			Assert.IsInstanceOf<TurnRight> (innerInnerIfStatemen.getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> (innerInnerIfStatemen.getChildren () [1]);
			Assert.IsInstanceOf<TurnRight> (innerInnerIfStatemen.getChildren () [2]);


			Assert.AreEqual (1, innerInnerIfStatemen.ElseChildren.Count);

		}

		[Test]
		public void TestIfElseIfElseStatement2(){
			tokenize = String.Format ("if {0} then\n" +
				"begin\n" +
				"\t{1}\n" +
				"end\n" +
				"else if not {0} then\n" +
				"begin\n" +
				"\t{2}\n" +
				"\t{2}\n" +
				"end\n" + 
				"else\n" + 
				"begin\n" +
				"\t{1}\n" +
				"end;\n", conditionLeft, moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifStatement = (response [0] as IfStatement);
			Assert.AreEqual (1, ifStatement.getChildren ().Count);
			Assert.AreEqual (1, ifStatement.ElseChildren.Count);
			Assert.IsInstanceOf<Forward> (ifStatement.getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((ifStatement.ElseChildren [0] as IfStatement).getChildren()[0]);
			Assert.IsInstanceOf<TurnRight> ((ifStatement.ElseChildren [0] as IfStatement).getChildren()[1]);
		}

		[Test]
		public void TestWhileStatement1(){
			tokenize = String.Format ("while {0} do\n" +
			"begin\n" +
			"\t{1}\n" +
				"end;", conditionLeft, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Solver s = (response [0] as WhileLoop).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (s);
			Assert.AreEqual (1, (response [0] as WhileLoop).getChildren ().Count);
			Assert.AreEqual (ECanInstructions.Left, (s as ConcreteInstruction).Instruction);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [0]);
		}

		[Test]
		public void TestWhileStatement2(){
			tokenize = String.Format ("while {0} and not {1} do\n" +
				"begin\n" +
				"\t{2}\n" +
				"end;", conditionLeft, conditionForward, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Solver s = (response [0] as WhileLoop).Conditions;
			Assert.IsInstanceOf<ConditionCombo> (s);

			Assert.IsInstanceOf<ConcreteInstruction> ((s as ConditionCombo).LeftSolver);
			Assert.IsInstanceOf<ConditionCombo> ((s as ConditionCombo).RightSolver);

			ConcreteInstruction instruction = (s as ConditionCombo).LeftSolver as ConcreteInstruction;
			ConditionCombo combo = (s as ConditionCombo).RightSolver as ConditionCombo;

			Assert.AreEqual (ECanInstructions.Left, instruction.Instruction);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);
			Assert.IsInstanceOf<ConcreteInstruction> (combo.LeftSolver);

			instruction = (combo.LeftSolver as ConcreteInstruction);
			Assert.AreEqual (ECanInstructions.Forward, instruction.Instruction);

			Assert.AreEqual (1, (response [0] as WhileLoop).getChildren ().Count);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [0]);
		}

		[Test]
		public void TestWhileStatement3(){
			tokenize = String.Format ("while {0} do\n" +
				"begin\n" +
				"\t{1}\n" +
				"\t{1}\n" +
				"end;", conditionLeft, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Solver s = (response [0] as WhileLoop).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (s);
			Assert.AreEqual (2, (response [0] as WhileLoop).getChildren ().Count);
			Assert.AreEqual (ECanInstructions.Left, (s as ConcreteInstruction).Instruction);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [1]);

		}

		[Test]
		public void TestWhileStatement4(){
			tokenize = String.Format ("while {0} do\n" +
				"begin\n" +
				"\t{1}\n" +
				"\t{2}\n" +
				"\t{1}\n" +
				"end;", conditionLeft, moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Solver s = (response [0] as WhileLoop).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (s);
			Assert.AreEqual (3, (response [0] as WhileLoop).getChildren ().Count);
			Assert.AreEqual (ECanInstructions.Left, (s as ConcreteInstruction).Instruction);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [0] as WhileLoop).getChildren () [1]);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [2]);

		}

		[Test]
		public void TestForLoop1(){
			tokenize = String.Format ("for count := 1 to 10 do\n" +
			"begin\n" +
				"\t{0}\n" +
				"end;\n", moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual(1, response.Count);
			Assert.IsInstanceOf<ForLoop> (response [0]);
			ForLoop forLoop = (response [0] as ForLoop);
			Assert.AreEqual (1, forLoop.getChildren ().Count);
			Assert.IsInstanceOf<Forward> (forLoop.getChildren () [0]);
		}

		[Test]
		public void TestForLoop2(){
			tokenize = String.Format ("for count := 0 to 20 do\n" +
				"begin\n" +
				"\t{0}\n" +
				"\t{1}\n" +
				"end;\n", moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual(1, response.Count);
			Assert.IsInstanceOf<ForLoop> (response [0]);
			ForLoop forLoop = (response [0] as ForLoop);
			Assert.AreEqual (2, forLoop.getChildren ().Count);
			Assert.IsInstanceOf<Forward> (forLoop.getChildren () [0]);
		}

		[Test]
		public void TestForLoop3(){
			tokenize = String.Format ("for count := 30 to 75 do\n" +
				"begin\n" +
				"\t{0}\n" +
				"\t{1}\n" +
				"\t{0}\n" +
				"end;\n", moveForwardCommand, rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual(1, response.Count);
			Assert.IsInstanceOf<ForLoop> (response [0]);
			ForLoop forLoop = (response [0] as ForLoop);
			Assert.AreEqual (3, forLoop.getChildren ().Count);
			Assert.IsInstanceOf<Forward> (forLoop.getChildren () [0]);
		}


		[Test]
		[ExpectedException( typeof( CodeParseException), ExpectedMessage = "Error At Line [1]: Syntax '{' is an invalid symbol")]
		public void TestInvalidComment(){
			tokenize = "{ some comment\n";
			response = pascalParser.ParseCode (tokenize);
		}

		[Test]
		[ExpectedException( typeof( CodeParseException), ExpectedMessage = "Error At Line [1]: An IF-ELSE IF-ELSE block cannot start with an ELSE IF")]
		public void TestStartWithElseIF(){

			tokenize = String.Format ("else if {0} then\n" + "begin\n" + "\t{1}\n" + "end;\n", conditionLeft, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		[ExpectedException( typeof( CodeParseException), ExpectedMessage = "Error At Line [1]: An IF-ELSE IF-ELSE block cannot start with an ELSE")]
		public void TestStartWithElse(){

			tokenize = String.Format ("else\n" + "begin\n" + "\t{0}\n" + "end;\n", moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		public void Test1(){
			tokenize = String.Format ("while forward do\n" + "begin;\n" + "{0}\n" +"end",moveForwardCommand);
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage = "Error At Line [1]: Invalid Syntax, parenthesis starting with '(' are not closed with corresponding ')'")]
		public void TestUnClosedParenthesis(){
			tokenize = String.Format ("if ({0} then\n" + "begin\n" + "\t{1}\n" + "end;\n", conditionLeft, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage = "Error At Line [1]: Invalid Syntax, parenthesis ending with '(' is not opened with corresponding ')'")]
		public void TestUnopenedParenthesis(){
			tokenize = String.Format ("if {0}) then\n" + "begin\n" + "\t{1}\n" + "end;\n", conditionLeft, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Expected LogicOperator to separate two conditions")]
		public void TestTwoCommandInCondition(){
			tokenize = String.Format ("if {0} {0} then\n" + "begin\n" + "\t{1}\n" + "end;\n", conditionLeft, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Expected 'not' of type LogicalOperator, but found 'Or' of type LogicalOperator")]
		public void TestOperatorCommandInCondition(){
			tokenize = String.Format ("if or {0} then\n" + "begin\n" + "\t{1}\n" + "end;\n", conditionLeft, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Unexpected token with a value of 'for' has been found")]
		public void TestForCommandInCondition(){
			tokenize = String.Format ("if for {0} then\n" + "begin\n" + "\t{1}\n" + "end;\n", conditionLeft, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Unexpected token with a value of '+' has been found")]
		public void TestPlusCommandInCondition(){
			tokenize = String.Format ("if + {0} then\n" + "begin\n" + "\t{1}\n" + "end;\n", conditionLeft, moveForwardCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		public void TestLongVariable(){
			tokenize = "var ditieenlangevariabledieminimaaltweeregelsinbeslagneemt: integer;\nditieenlangevariabledieminimaaltweeregelsinbeslagneemt:=1\n";
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Invalid Syntax, missing ';' after 'moveForward()'")]
		public void TestMethodEqualsLiteral(){
			tokenize = "moveForward():=1\n";
			response = pascalParser.ParseCode (tokenize);
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Invalid Syntax, missing ';' after 'moveFwd()'")]
		public void TestCustomMethodEqualsLiteral(){
			tokenize = "moveFwd():=1\n";
			response = pascalParser.ParseCode (tokenize);
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [2]: Invalid Syntax, Found 'Command' after := of the variable declaration of 'dfjlhgd'")]
		public void TestLiteralEqualsMethod(){
			tokenize = "var dfjlhgd: string;\ndfjlhgd := moveForward()\n";
			response = pascalParser.ParseCode (tokenize);
		}

		[Test]
		public void TestCamalCase1(){
			tokenize = "var Pascal: integer;\nPascal := 1\n";
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
		}

		[Test]
		public void TestCamalCase2(){
			tokenize = "var pasCAL: integer;\npasCAL := 1\n";
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
		}

		[Test]
		[ExpectedException(typeof(CodeParseException), ExpectedMessage="Error At Line [2]: Expected := after the declaration of drop, := is required to assign a variable")]
		public void TestCamalCase3(){
			tokenize = "var drop: integer;\ndrop table Code\n";
			response = pascalParser.ParseCode (tokenize);
		}

		[Test]
		public void TestLongNumber(){
			tokenize = "var test: integer;\ntest := 999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999\n";
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Syntax '#' is an invalid symbol")]
		public void TestHashTag(){
			tokenize = "#33CCCC:=1\n";
			response = pascalParser.ParseCode (tokenize);
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Syntax '{' is an invalid symbol")]
		public void MultiLineComment()
		{
			tokenize = "{\n" + "mytest \n" + "}\n";
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Expected LogicOperator to separate two conditions")]
		public void IncorrectAt(){
			tokenize = "if at(butcher) then\n";
			response = pascalParser.ParseCode (tokenize);
		}


		[Test]
		public void TestSemicolonAfterFunctionCall(){
			tokenize = "cusFunction();\n";
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<FunctionBlockExecute> (response [0]);
		}

		[Test]
		[ExpectedException(typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Invalid Syntax, missing ';' after 'cusFunction()'")]
		public void TestMissingSemicolonFunctionCall(){
			tokenize = "cusFunction()\n";
			response = pascalParser.ParseCode (tokenize);
		}

		[Test]
		[ExpectedException(typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Invalid Syntax, Expected eol after 'cusFunction();' but found 'moveForward()'")]
		public void TestFunctionAfterFunctionCall(){
			tokenize = "cusFunction(); moveForward();\n";
			response = pascalParser.ParseCode (tokenize);
		}

		[Test]
		[ExpectedException(typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Invalid Syntax, Expected eol after 'cusFunction();' but found 'for'")]
		public void TestForAfterFunctionCall(){
			tokenize = "cusFunction(); for x := 0 to 8\n";
			response = pascalParser.ParseCode (tokenize);
		}

		[Test]
		[ExpectedException(typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Invalid Syntax, Expected ';' after 'cusFunction()' but found 'moveForward()'")]
		public void TestFunctionBeforeSemicolonFunctionCall(){
			tokenize = "cusFunction() moveForward();\n";
			response = pascalParser.ParseCode (tokenize);
		}

		[Test]
		[ExpectedException(typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Invalid Syntax, Expected ';' after 'cusFunction()' but found 'for'")]
		public void TestForBeforeSemicolonFunctionCall(){
			tokenize = "cusFunction()  for x := 0 to 8;\n";
			response = pascalParser.ParseCode (tokenize);
		}


		[Test]
		[ExpectedException(typeof(CodeParseException), ExpectedMessage="Error At Line [4]: Invalid Syntax, expected 'end' to be used in ending of 'IF' but found 'end;'")]
		public void TestCorrectUseOfIFENDBeginBlocksInIfstatement1(){
			tokenize = String.Format ("if {0} then \nbegin \n\t{1}\nend;\nelse\nbegin\n\t{2}\nend;", conditionLeft, moveForwardCommand,rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
		}

		[Test]
		[ExpectedException(typeof(CodeParseException), ExpectedMessage="Error At Line [4]: Invalid Syntax, expected 'end;' to be used in ending of 'WHILE' but found 'end'")]
		public void TestCorrectUseOfENDINWhileBeginBlocksInIfstatement1(){
			tokenize = String.Format ("while {0} do \nbegin \n\t{1}\nend", conditionLeft, moveForwardCommand,rotateRightCommand);
			response = pascalParser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
		}
	}
}


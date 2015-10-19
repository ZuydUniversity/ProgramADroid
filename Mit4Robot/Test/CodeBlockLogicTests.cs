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
	public class CodeBlockLogicTests
	{
		[SetUp]
		public void SetupTest(){
			Robot robot = Robot.Instance;
			robot = null;

		}

		/*[Test()]
		public void TestParserPyton(){
			string codeToParse = "moveForward() \n" +
			                     "moveForward() \n" +
			                     "moveForward() \n" +
			                     "turnRight() \n" +
			                     "turnRight() \n" +
			                     "moveForward() \n" +
			                     "pickUp('bloemkool') \n" +
			                     "pickUp('biefstuk') \n" +
								 "pickUp('paprika') \n" +
								 "pickUp('perzik')" ;
			CodeParser codeParser = new PythonParser ();
			codeParser.ParseCode (codeToParse);
			Assert.AreEqual (1, 1);
		}*/

//		/// <summary>
//		/// Tests the condition code single condition.
//		/// </summary>
//		[Test()]
//		public void TestConditionCodeSingleCondition(){
//			PythonParser codeParser = new PythonParser ();
//			ConditionsClass condition = codeParser.buildConditionClass ("can_left()");
//			Assert.AreEqual (ELogicOperators.None, condition.conditionOperator);
//			Assert.AreEqual (ECanInstructions.Left,condition.leftHandInstruction);
//		}

//		/// <summary>
//		/// Tests the condition code and statement.
//		/// </summary>
//		[Test()]
//		public void TestConditionCodeAndStatement(){
//			PythonParser codeParser = new PythonParser ();
//			ConditionsClass condition = codeParser.buildConditionClass ("can_left() and can_right()");
//			Assert.AreEqual (ELogicOperators.And, condition.conditionOperator);
//			Assert.AreEqual (ECanInstructions.Left,condition.leftHandInstruction);
//			Assert.AreEqual (ECanInstructions.Right, condition.rightHandInstruction);
//		}

//		/// <summary>
//		/// Tests the condition code or statement.
//		/// </summary>
//		[Test()]
//		public void TestConditionCodeOrStatement(){
//			PythonParser codeParser = new PythonParser ();
//			ConditionsClass condition = codeParser.buildConditionClass ("can_right() or can_left()");
//			Assert.AreEqual (ELogicOperators.Or, condition.conditionOperator);
//			Assert.AreEqual (ECanInstructions.Right,condition.leftHandInstruction);
//			Assert.AreEqual (ECanInstructions.Left, condition.rightHandInstruction);
//		}

		/*[Test()]
		public void TestConditionWhile(){

			PythonParser codeParser = new PythonParser ();
			String codeToParse = "while can_forward():\n" +
			                     "\t moveForward() \n" +
			                     "\t turnRight()";
			codeParser.ParseCode (codeToParse);
			Assert.AreEqual (1, 1);
		}*/


			


		[Test()]
		public void SolverBasic()
	    {
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			Solver concreteInstruction = new ConcreteInstruction (ECanInstructions.Forward);

			bool actual = concreteInstruction.solve (null);
			bool expected = true;
			Assert.AreEqual (expected, actual);


		}

		[Test()]
		public void SolverAnd()
		{
			Solver leftSolver = new ConcreteInstruction (ECanInstructions.Backward);
			Solver rightSolver = new ConcreteInstruction (ECanInstructions.Forward);
			Solver combo = new ConditionCombo (leftSolver, rightSolver, ELogicOperators.And);

			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			bool actual = combo.solve (null);
			bool expected = false;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void SolverAnd2()
		{
			Solver leftSolver = new ConcreteInstruction (ECanInstructions.Forward);
			Solver rightSolver = new ConcreteInstruction (ECanInstructions.Forward);
			Solver combo = new ConditionCombo (leftSolver, rightSolver, ELogicOperators.And);

			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			bool actual = combo.solve (null);
			bool expected = true;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void SolverOr()
		{
			Solver leftSolver = new ConcreteInstruction (ECanInstructions.Backward);
			Solver rightSolver = new ConcreteInstruction (ECanInstructions.Forward);
			Solver combo = new ConditionCombo (leftSolver, rightSolver, ELogicOperators.Or);

			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			bool actual = combo.solve (null);
			bool expected = true;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void SolverOr2()
		{
			Solver leftSolver = new ConcreteInstruction (ECanInstructions.Backward);
			Solver rightSolver = new ConcreteInstruction (ECanInstructions.Backward);
			Solver combo = new ConditionCombo (leftSolver, rightSolver, ELogicOperators.Or);

			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			bool actual = combo.solve (null);
			bool expected = false;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void SolverNot()
		{
			Solver leftSolver = new ConcreteInstruction (ECanInstructions.Backward);
			Solver combo = new ConditionCombo (leftSolver, ELogicOperators.Not);

			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			bool actual = combo.solve (null);
			bool expected = true;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void SolverNotOr()
		{
			Solver leftSolver = new ConcreteInstruction (ECanInstructions.Backward);
			Solver rightSolver = new ConcreteInstruction (ECanInstructions.Backward);
			Solver combo = new ConditionCombo (leftSolver, rightSolver, ELogicOperators.Or);

			Solver comboNot = new ConditionCombo (combo, ELogicOperators.Not);

			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			bool actual = comboNot.solve (null);
			bool expected = true;
			Assert.AreEqual (expected, actual);
		}

		#region WhileLoop tests
		/// <summary>
		/// Tests if the while loop will break in case of an infinite loop.
		/// </summary>
		[Test()]
		public void whileLoop1()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.xPosition = 1;
			robot.yPosition = 1;

			Solver condition1 = new ConcreteInstruction (ECanInstructions.Right);
			Solver condition2 = new ConcreteInstruction (ECanInstructions.Forward);
			Solver combo = new ConditionCombo (condition1, condition2, ELogicOperators.Or);

			WhileLoop whileLoop = new WhileLoop (combo);
			bool actual = whileLoop.execute (null);
			bool expected = false;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Test if the while loop returns true when executed succesfully.
		/// </summary>
		[Test()]
		public void whileLoop2()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			Solver conditions = new ConcreteInstruction (ECanInstructions.Backward);

			WhileLoop whileLoop = new WhileLoop (conditions);
			bool actual = whileLoop.execute (null);
			bool expected = true;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Robot should do nothing, because of the condition (A and !A)
		/// </summary>
		[Test()]
		public void whileLoop3()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			Solver concreteInstruction1 = new ConcreteInstruction (ECanInstructions.Forward); //A
			Solver concreteInstruction2 = new ConcreteInstruction (ECanInstructions.Forward); //A
			Solver notInstruction = new ConditionCombo (concreteInstruction1, ELogicOperators.Not); //!A
			Solver combo = new ConditionCombo (notInstruction, concreteInstruction2, ELogicOperators.And); // !A && A

			ICodeBlock command = new TurnRight ();

			WhileLoop whileLoop = new WhileLoop (combo);
			whileLoop.addChild (command);
			whileLoop.execute (null);
			
			EOrientation actual = robot.orientationEnum;
			EOrientation expected = EOrientation.East;
			Assert.AreEqual (expected, actual);
		}

		/// <summary>
		/// Robot should take 2 steps
		/// </summary>
		[Test()]
		public void whileLoop4()
		{
			EOrientation orientation = EOrientation.South;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.xPosition = 1;
		
			Solver canForward = new ConcreteInstruction (ECanInstructions.Forward);

			ICodeBlock command = new Forward();

			WhileLoop whileLoop = new WhileLoop (canForward);
			whileLoop.addChild (command);


			MainCode mainCode = new MainCode ();
			mainCode.addChild (whileLoop);

			mainCode.execute ();

			int actual = robot.yPosition;
			int expected = 2;
			Assert.AreEqual (expected, actual);
		}

		#endregion

		#region IfStatement Tests

		[Test()]
		public void ifStatement1()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			Solver conditions = new ConcreteInstruction (ECanInstructions.Forward);

			ICodeBlock command = new TurnRight ();

			IfStatement ifStatement = new IfStatement (conditions);
			ifStatement.addChild (command);
			ifStatement.execute (null);

			EOrientation actual = robot.orientationEnum;
			EOrientation expected = EOrientation.South;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void ifStatement2()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));
			robot.xPosition = 0;
			robot.yPosition = 0;

			Solver conditions = new ConcreteInstruction (ECanInstructions.Backward);

			ICodeBlock command = new TurnRight ();

			IfStatement ifStatement = new IfStatement (conditions);
			ifStatement.ElseChildren.Add (command);
			ifStatement.execute (null);

			EOrientation actual = robot.orientationEnum;
			EOrientation expected = EOrientation.South;
			Assert.AreEqual (expected, actual);
		}
		#endregion

		[Test()]
		public void variablePlus()
		{
			MainCode mainCode = new MainCode ();

			// create i = 10
			ConcreteVariable iVar = new ConcreteVariable (new Variable (10, EVariableType.Int));
			DefineVariable iDefine = new DefineVariable (iVar);
			CmdDefineVariable iDefineCommand = new CmdDefineVariable ("i", iDefine);

			// create x = 5;
			ConcreteVariable xVar = new ConcreteVariable (new Variable (5, EVariableType.Int));
			DefineVariable xDefine = new DefineVariable (xVar);
			CmdDefineVariable xDefineCommand = new CmdDefineVariable ("x", xDefine);

			// Gather the variables
			ConcreteVariable iGather = new ConcreteVariable ("i");
			ConcreteVariable xGather = new ConcreteVariable ("x");

			// calculate and put the values in ii
			VariableSolver calculation = new VariableCombo (iGather, xGather, EMathOperator.Add);
			DefineVariable calcCommand = new DefineVariable (calculation);
			CmdDefineVariable executeCalculation = new CmdDefineVariable ("ii", calcCommand);

			mainCode.addChild (xDefineCommand);
			mainCode.addChild (iDefineCommand);
			mainCode.addChild (executeCalculation);
			mainCode.execute ();

			//ConcreteVariable iiGather = new ConcreteVariable ("ii");

			int expected = 15;
			int actual = mainCode.variables["ii"].Value;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void variableMultiply()
		{
			MainCode mainCode = new MainCode ();

			// create i = 10
			ConcreteVariable iVar = new ConcreteVariable (new Variable (10, EVariableType.Int));
			DefineVariable iDefine = new DefineVariable (iVar);
			CmdDefineVariable iDefineCommand = new CmdDefineVariable ("i", iDefine);

			// create x = 5;
			ConcreteVariable xVar = new ConcreteVariable (new Variable (5, EVariableType.Int));
			DefineVariable xDefine = new DefineVariable (xVar);
			CmdDefineVariable xDefineCommand = new CmdDefineVariable ("x", xDefine);

			// Gather the variables
			ConcreteVariable iGather = new ConcreteVariable ("i");
			ConcreteVariable xGather = new ConcreteVariable ("x");

			// calculate and put the values in ii
			VariableSolver calculation = new VariableCombo (iGather, xGather, EMathOperator.Multiply);
			DefineVariable calcCommand = new DefineVariable (calculation);
			CmdDefineVariable executeCalculation = new CmdDefineVariable ("ii", calcCommand);

			mainCode.addChild (xDefineCommand);
			mainCode.addChild (iDefineCommand);
			mainCode.addChild (executeCalculation);
			mainCode.execute ();

			//ConcreteVariable iiGather = new ConcreteVariable ("ii");

			int expected = 50;
			int actual = mainCode.variables["ii"].Value;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void variableMinus()
		{
			MainCode mainCode = new MainCode ();

			// create i = 10
			ConcreteVariable iVar = new ConcreteVariable (new Variable (10, EVariableType.Int));
			DefineVariable iDefine = new DefineVariable (iVar);
			CmdDefineVariable iDefineCommand = new CmdDefineVariable ("i", iDefine);

			// create x = 5;
			ConcreteVariable xVar = new ConcreteVariable (new Variable (5, EVariableType.Int));
			DefineVariable xDefine = new DefineVariable (xVar);
			CmdDefineVariable xDefineCommand = new CmdDefineVariable ("x", xDefine);

			// Gather the variables
			ConcreteVariable iGather = new ConcreteVariable ("i");
			ConcreteVariable xGather = new ConcreteVariable ("x");

			// calculate and put the values in ii
			VariableSolver calculation = new VariableCombo (iGather, xGather, EMathOperator.Subtract);
			DefineVariable calcCommand = new DefineVariable (calculation);
			CmdDefineVariable executeCalculation = new CmdDefineVariable ("ii", calcCommand);

			mainCode.addChild (xDefineCommand);
			mainCode.addChild (iDefineCommand);
			mainCode.addChild (executeCalculation);
			mainCode.execute ();

			//ConcreteVariable iiGather = new ConcreteVariable ("ii");

			int expected = 5;
			int actual = mainCode.variables["ii"].Value;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void variableDivide()
		{
			MainCode mainCode = new MainCode ();

			// create i = 10
			ConcreteVariable iVar = new ConcreteVariable (new Variable (10, EVariableType.Int));
			DefineVariable iDefine = new DefineVariable (iVar);
			CmdDefineVariable iDefineCommand = new CmdDefineVariable ("i", iDefine);

			// create x = 5;
			ConcreteVariable xVar = new ConcreteVariable (new Variable (5, EVariableType.Int));
			DefineVariable xDefine = new DefineVariable (xVar);
			CmdDefineVariable xDefineCommand = new CmdDefineVariable ("x", xDefine);

			// Gather the variables
			ConcreteVariable iGather = new ConcreteVariable ("i");
			ConcreteVariable xGather = new ConcreteVariable ("x");

			// calculate and put the values in ii
			VariableSolver calculation = new VariableCombo (iGather, xGather, EMathOperator.Divide);
			DefineVariable calcCommand = new DefineVariable (calculation);
			CmdDefineVariable executeCalculation = new CmdDefineVariable ("ii", calcCommand);

			mainCode.addChild (xDefineCommand);
			mainCode.addChild (iDefineCommand);
			mainCode.addChild (executeCalculation);
			mainCode.execute ();

			//ConcreteVariable iiGather = new ConcreteVariable ("ii");

			int expected = 2;
			int actual = mainCode.variables["ii"].Value;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void variableStringAdd()
		{
			MainCode mainCode = new MainCode ();

			// create i = 10
			ConcreteVariable iVar = new ConcreteVariable (new Variable ("10", EVariableType.String));
			DefineVariable iDefine = new DefineVariable (iVar);
			CmdDefineVariable iDefineCommand = new CmdDefineVariable ("i", iDefine);

			// create x = 5;
			ConcreteVariable xVar = new ConcreteVariable (new Variable (5, EVariableType.Int));
			DefineVariable xDefine = new DefineVariable (xVar);
			CmdDefineVariable xDefineCommand = new CmdDefineVariable ("x", xDefine);

			// Gather the variables
			ConcreteVariable iGather = new ConcreteVariable ("i");
			ConcreteVariable xGather = new ConcreteVariable ("x");

			// calculate and put the values in ii
			VariableSolver calculation = new VariableCombo (iGather, xGather, EMathOperator.Add);
			DefineVariable calcCommand = new DefineVariable (calculation);
			CmdDefineVariable executeCalculation = new CmdDefineVariable ("ii", calcCommand);

			mainCode.addChild (xDefineCommand);
			mainCode.addChild (iDefineCommand);
			mainCode.addChild (executeCalculation);
			mainCode.execute ();

			//ConcreteVariable iiGather = new ConcreteVariable ("ii");

			string expected = "105";
			string actual = mainCode.variables["ii"].Value;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		[ExpectedException( typeof( RunTimeException ) )]
		public void variableStringDivide()
		{
			MainCode mainCode = new MainCode ();

			// create i = 10
			ConcreteVariable iVar = new ConcreteVariable (new Variable ("10", EVariableType.String));
			DefineVariable iDefine = new DefineVariable (iVar);
			CmdDefineVariable iDefineCommand = new CmdDefineVariable ("i", iDefine);

			// create x = 5;
			ConcreteVariable xVar = new ConcreteVariable (new Variable (5, EVariableType.Int));
			DefineVariable xDefine = new DefineVariable (xVar);
			CmdDefineVariable xDefineCommand = new CmdDefineVariable ("x", xDefine);

			// Gather the variables
			ConcreteVariable iGather = new ConcreteVariable ("i");
			ConcreteVariable xGather = new ConcreteVariable ("x");

			// calculate and put the values in ii
			VariableSolver calculation = new VariableCombo (iGather, xGather, EMathOperator.Divide);
			DefineVariable calcCommand = new DefineVariable (calculation);
			CmdDefineVariable executeCalculation = new CmdDefineVariable ("ii", calcCommand);

			mainCode.addChild (xDefineCommand);
			mainCode.addChild (iDefineCommand);
			mainCode.addChild (executeCalculation);
			mainCode.execute ();

			//ConcreteVariable iiGather = new ConcreteVariable ("ii");
		}

		[Test()]
		[ExpectedException( typeof( RunTimeException ) )]
		public void variableStringMultiply()
		{
			MainCode mainCode = new MainCode ();

			// create i = 10
			ConcreteVariable iVar = new ConcreteVariable (new Variable ("10", EVariableType.String));
			DefineVariable iDefine = new DefineVariable (iVar);
			CmdDefineVariable iDefineCommand = new CmdDefineVariable ("i", iDefine);

			// create x = 5;
			ConcreteVariable xVar = new ConcreteVariable (new Variable (5, EVariableType.Int));
			DefineVariable xDefine = new DefineVariable (xVar);
			CmdDefineVariable xDefineCommand = new CmdDefineVariable ("x", xDefine);

			// Gather the variables
			ConcreteVariable iGather = new ConcreteVariable ("i");
			ConcreteVariable xGather = new ConcreteVariable ("x");

			// calculate and put the values in ii
			VariableSolver calculation = new VariableCombo (iGather, xGather, EMathOperator.Multiply);
			DefineVariable calcCommand = new DefineVariable (calculation);
			CmdDefineVariable executeCalculation = new CmdDefineVariable ("ii", calcCommand);

			mainCode.addChild (xDefineCommand);
			mainCode.addChild (iDefineCommand);
			mainCode.addChild (executeCalculation);
			mainCode.execute ();

			//ConcreteVariable iiGather = new ConcreteVariable ("ii");
		}

		[Test()]
		[ExpectedException( typeof( RunTimeException ) )]
		public void variableStringSubstract()
		{
			MainCode mainCode = new MainCode ();

			// create i = 10
			ConcreteVariable iVar = new ConcreteVariable (new Variable ("10", EVariableType.String));
			DefineVariable iDefine = new DefineVariable (iVar);
			CmdDefineVariable iDefineCommand = new CmdDefineVariable ("i", iDefine);

			// create x = 5;
			ConcreteVariable xVar = new ConcreteVariable (new Variable (5, EVariableType.Int));
			DefineVariable xDefine = new DefineVariable (xVar);
			CmdDefineVariable xDefineCommand = new CmdDefineVariable ("x", xDefine);

			// Gather the variables
			ConcreteVariable iGather = new ConcreteVariable ("i");
			ConcreteVariable xGather = new ConcreteVariable ("x");

			// calculate and put the values in ii
			VariableSolver calculation = new VariableCombo (iGather, xGather, EMathOperator.Subtract);
			DefineVariable calcCommand = new DefineVariable (calculation);
			CmdDefineVariable executeCalculation = new CmdDefineVariable ("ii", calcCommand);

			mainCode.addChild (xDefineCommand);
			mainCode.addChild (iDefineCommand);
			mainCode.addChild (executeCalculation);
			mainCode.execute ();

			//ConcreteVariable iiGather = new ConcreteVariable ("ii");
		}

		[Test()]
		public void forLoop1()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			MainCode mainCode = new MainCode ();

			// Create (int i = 0; i < 5; i++)

			//Create i
			ConcreteVariable iVar = new ConcreteVariable (new Variable (0, EVariableType.Int));
			DefineVariable iDefine = new DefineVariable (iVar);
			CmdDefineVariable iDefineCommand = new CmdDefineVariable ("i", iDefine);
		
			//create a variable thats always 5
			Variable max = new Variable (5, EVariableType.Int);
			VariableSolver maxS = new ConcreteVariable (max);

			//Create i < 5
			ValueSolver solver = new ValueSolver (iVar, maxS, EComparisonOperator.ValueLessThan);

			// Create i++
			ConcreteVariable staticOneVar = new ConcreteVariable (new Variable (1, EVariableType.Int));
			ConcreteVariable iGather = new ConcreteVariable ("i"); // Gathers i, this creates a reference to iVar
			VariableSolver iPlusOneSolver = new VariableCombo (iGather, staticOneVar, EMathOperator.Add);
			DefineVariable iPlusOneDefine = new DefineVariable (iPlusOneSolver);
			CmdDefineVariable iPlusOneSolverCommand = new CmdDefineVariable ("i", iPlusOneDefine);

			Composite forLoop = new ForLoop (solver, iDefineCommand, iPlusOneSolverCommand);

			mainCode.addChild (forLoop);
			forLoop.addChild (new TurnRight ());

			mainCode.execute ();

			EOrientation actual = robot.orientationEnum;
			EOrientation expected = EOrientation.South;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void forLoop2()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			MainCode mainCode = new MainCode ();

			// Create (int i = 1; i < 5; i*i)

			//Create i
			ConcreteVariable iVar = new ConcreteVariable (new Variable (1, EVariableType.Int));
			DefineVariable iDefine = new DefineVariable (iVar);
			CmdDefineVariable iDefineCommand = new CmdDefineVariable ("i", iDefine);

			//create a variable thats always 2
			Variable staticTwoVar = new Variable (2, EVariableType.Int);
			VariableSolver staticTwoVarS = new ConcreteVariable (staticTwoVar);

			// Create i*i
			ConcreteVariable iGather = new ConcreteVariable ("i"); // Gathers i, this creates a reference to iVar
			VariableSolver ixiSolver = new VariableCombo (iGather, staticTwoVarS, EMathOperator.Multiply);
			DefineVariable ixiDefine = new DefineVariable (ixiSolver);
			CmdDefineVariable ixiSolverCommand = new CmdDefineVariable ("i", ixiDefine);

			//create a variable thats always 5
			Variable max = new Variable (5, EVariableType.Int);
			VariableSolver maxS = new ConcreteVariable (max);


			//Create i < 4
			ValueSolver solver = new ValueSolver (iGather, maxS, EComparisonOperator.ValueLessThan);

			ForLoop forLoop = new ForLoop (solver, iDefineCommand, ixiSolverCommand);

			//forLoop.variables["i"] = new Variable(1, EVariableType.Int);

			mainCode.addChild (forLoop);
			forLoop.addChild (new TurnRight ());

			mainCode.execute ();

			EOrientation actual = robot.orientationEnum;
			EOrientation expected = EOrientation.North;
			Assert.AreEqual (expected, actual);
		}

		[Test()]
		public void forLoop3()
		{
			EOrientation orientation = EOrientation.East;
			Robot robot = Robot.Create (orientation, new Map(EDifficulty.Easy));

			MainCode mainCode = new MainCode ();

			// Create (int i = 0; i < 500; i++)

			//Create i
			ConcreteVariable iVar = new ConcreteVariable (new Variable (0, EVariableType.Int));
			DefineVariable iDefine = new DefineVariable (iVar);
			CmdDefineVariable iDefineCommand = new CmdDefineVariable ("i", iDefine);

			//create a variable thats always 500
			Variable max = new Variable (500, EVariableType.Int);
			VariableSolver maxS = new ConcreteVariable (max);

			//Create i < 5
			ValueSolver solver = new ValueSolver (iVar, maxS, EComparisonOperator.ValueLessThan);

			// Create i++
			ConcreteVariable staticOneVar = new ConcreteVariable (new Variable (1, EVariableType.Int));
			ConcreteVariable iGather = new ConcreteVariable ("i"); // Gathers i, this creates a reference to iVar
			VariableSolver iPlusOneSolver = new VariableCombo (iGather, staticOneVar, EMathOperator.Add);
			DefineVariable iPlusOneDefine = new DefineVariable (iPlusOneSolver);
			CmdDefineVariable iPlusOneSolverCommand = new CmdDefineVariable ("i", iPlusOneDefine);

			Composite forLoop = new ForLoop (solver, iDefineCommand, iPlusOneSolverCommand);

			mainCode.addChild (forLoop);
			//forLoop.addChild (new TurnRight ());

			bool actual = false;
			bool expected = mainCode.execute ();
			Assert.AreEqual (expected, actual);
		}
			
	}
}


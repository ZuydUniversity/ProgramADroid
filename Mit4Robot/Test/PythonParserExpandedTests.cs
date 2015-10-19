using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using Shared;
using System.Collections.Generic;
using Shared.Parsers.CodeParsers;
using Shared.BusinessLayer;
using Shared.Exceptions;
using Shared.Enums;


namespace Test
{
	[TestFixture ()]
	public class PythonParserExpandedTests
	{
		private PythonParser parser;
		private List<ICodeBlock> response;
		private String tokenize;

		[SetUp]
		public void BeforeTest(){
			parser = new PythonParser ();
			response = new List<ICodeBlock> ();
			tokenize = "";
		}

		#region SimpleCommands

		/// <summary>
		/// Test if a single forward command is parsed succesfully.
		/// </summary>
		[Test()]
		public void TestSingleForward(){
			tokenize = "moveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
		}

		/// <summary>
		/// Test if a single rotateRight command is parsed succesfully.
		/// </summary>
		[Test()]
		public void TestSingleRotate(){
			tokenize = "rotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<TurnRight> (response [0]);
		}

		/// <summary>
		/// Test if a single pickup command is parsed succesfully.
		/// </summary>
		[Test()]
		public void TestSinglePickup(){
			tokenize = "pickUp('paprika')\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<PickUp> (response [0]);
			Assert.AreEqual ("paprika", (response [0] as PickUp).ObjectToPickUP);
		}

		#endregion

		#region MultipleSimpleCommands

		/// <summary>
		/// Test if a multiple forward commands are parsed succesfully.
		/// </summary>
		[Test()]
		public void TestMultipleForward1(){
			tokenize = "moveForward()\n" +
			                  "moveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<Forward> (response [1]);
		}

		/// <summary>
		/// Test if a multiple forward commands are parsed succesfully.
		/// </summary>
		[Test()]
		public void TestMultipleForward2(){
			tokenize = "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (3, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<Forward> (response [1]);
			Assert.IsInstanceOf<Forward> (response [2]);
		}

		/// <summary>
		/// Test if a a lot of forward commands are parsed succesfully.
		/// </summary>
		[Test()]
		public void TestMultipleForward3(){
			tokenize = "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n" +
			                  "moveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (30, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<Forward> (response [1]);
			Assert.IsInstanceOf<Forward> (response [2]);
			Assert.IsInstanceOf<Forward> (response [3]);
			Assert.IsInstanceOf<Forward> (response [4]);
			Assert.IsInstanceOf<Forward> (response [5]);
			Assert.IsInstanceOf<Forward> (response [6]);
			Assert.IsInstanceOf<Forward> (response [7]);
			Assert.IsInstanceOf<Forward> (response [8]);
			Assert.IsInstanceOf<Forward> (response [9]);
			Assert.IsInstanceOf<Forward> (response [10]);
			Assert.IsInstanceOf<Forward> (response [11]);
			Assert.IsInstanceOf<Forward> (response [12]);
			Assert.IsInstanceOf<Forward> (response [13]);
			Assert.IsInstanceOf<Forward> (response [14]);
			Assert.IsInstanceOf<Forward> (response [15]);
			Assert.IsInstanceOf<Forward> (response [16]);
			Assert.IsInstanceOf<Forward> (response [17]);
			Assert.IsInstanceOf<Forward> (response [18]);
			Assert.IsInstanceOf<Forward> (response [19]);
			Assert.IsInstanceOf<Forward> (response [20]);
			Assert.IsInstanceOf<Forward> (response [21]);
			Assert.IsInstanceOf<Forward> (response [22]);
			Assert.IsInstanceOf<Forward> (response [23]);
			Assert.IsInstanceOf<Forward> (response [24]);
			Assert.IsInstanceOf<Forward> (response [25]);
			Assert.IsInstanceOf<Forward> (response [26]);
			Assert.IsInstanceOf<Forward> (response [27]);
			Assert.IsInstanceOf<Forward> (response [28]);
			Assert.IsInstanceOf<Forward> (response [29]);
		}

			/// <summary>
			/// Test if a multiple rotateright commands are parsed succesfully.
			/// </summary>
			[Test()]
			public void TestMultipleRotateRight1(){
			tokenize = "rotateRight()\n" +
			                  "rotateRight()\n";
				
				response = parser.ParseCode (tokenize);
				Assert.AreEqual (2, response.Count);
				Assert.IsInstanceOf<TurnRight> (response [0]);
				Assert.IsInstanceOf<TurnRight> (response [1]);
			}

			/// <summary>
			/// Test if a multiple rotateright commands are parsed succesfully.
			/// </summary>
			[Test()]
			public void TestMultipleRotateRight2(){
			tokenize =  "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n";
				
				response = parser.ParseCode (tokenize);
				Assert.AreEqual (3, response.Count);
				Assert.IsInstanceOf<TurnRight> (response [0]);
				Assert.IsInstanceOf<TurnRight> (response [1]);
				Assert.IsInstanceOf<TurnRight> (response [2]);
			}

			/// <summary>
			/// Test if a a lot of rotateright commands are parsed succesfully.
			/// </summary>
			[Test()]
			public void TestMultipleRotateRight3(){
			tokenize =  "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n" +
			                   "rotateRight()\n";
				
				response = parser.ParseCode (tokenize);
				Assert.AreEqual (30, response.Count);
				Assert.IsInstanceOf<TurnRight> (response [0]);
				Assert.IsInstanceOf<TurnRight> (response [1]);
				Assert.IsInstanceOf<TurnRight> (response [2]);
				Assert.IsInstanceOf<TurnRight> (response [3]);
				Assert.IsInstanceOf<TurnRight> (response [4]);
				Assert.IsInstanceOf<TurnRight> (response [5]);
				Assert.IsInstanceOf<TurnRight> (response [6]);
				Assert.IsInstanceOf<TurnRight> (response [7]);
				Assert.IsInstanceOf<TurnRight> (response [8]);
				Assert.IsInstanceOf<TurnRight> (response [9]);
				Assert.IsInstanceOf<TurnRight> (response [10]);
				Assert.IsInstanceOf<TurnRight> (response [11]);
				Assert.IsInstanceOf<TurnRight> (response [12]);
				Assert.IsInstanceOf<TurnRight> (response [13]);
				Assert.IsInstanceOf<TurnRight> (response [14]);
				Assert.IsInstanceOf<TurnRight> (response [15]);
				Assert.IsInstanceOf<TurnRight> (response [16]);
				Assert.IsInstanceOf<TurnRight> (response [17]);
				Assert.IsInstanceOf<TurnRight> (response [18]);
				Assert.IsInstanceOf<TurnRight> (response [19]);
				Assert.IsInstanceOf<TurnRight> (response [20]);
				Assert.IsInstanceOf<TurnRight> (response [21]);
				Assert.IsInstanceOf<TurnRight> (response [22]);
				Assert.IsInstanceOf<TurnRight> (response [23]);
				Assert.IsInstanceOf<TurnRight> (response [24]);
				Assert.IsInstanceOf<TurnRight> (response [25]);
				Assert.IsInstanceOf<TurnRight> (response [26]);
				Assert.IsInstanceOf<TurnRight> (response [27]);
				Assert.IsInstanceOf<TurnRight> (response [28]);
				Assert.IsInstanceOf<TurnRight> (response [29]);
		}


		/// <summary>
		/// Test if a multiple PickUp commands are parsed succesfully.
		/// </summary>
		[Test()]
		public void TestMultiplePickUp1(){
			tokenize = "pickUp('')\n" +
			                  "pickUp('')\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<PickUp> (response [0]);
			Assert.IsInstanceOf<PickUp> (response [1]);
		}

		/// <summary>
		/// Test if a multiple PickUp commands are parsed succesfully.
		/// </summary>
		[Test()]
		public void TestMultiplePickUp2(){
			tokenize = "pickUp('')\n" +
			                  "pickUp('')\n" +
			                  "pickUp('')\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (3, response.Count);
			Assert.IsInstanceOf<PickUp> (response [0]);
			Assert.IsInstanceOf<PickUp> (response [1]);
			Assert.IsInstanceOf<PickUp> (response [2]);
		}

		/// <summary>
		/// Test if a a lot of PickUp commands are parsed succesfully.
		/// </summary>
		[Test()]
		public void TestMultiplePickUp3(){
			tokenize = "pickUp('paprika')\n" +
			                  "pickUp('tomaat')\n" +
			                  "pickUp('aardappel')\n" +
			                  "pickUp('vis')\n" +
			                  "pickUp('gehakt')\n" +
			                  "pickUp('bloemkool')\n" +
			                  "pickUp('kaas')\n" +
			                  "pickUp('biefstuk')\n" +
			                  "pickUp('kipfilet')\n" +
			                  "pickUp('kip')\n" +
			                  "pickUp('tonijn')\n" +
			                  "pickUp('sla')\n" +
			                  "pickUp('wortel')\n" +
			                  "pickUp('rijst')\n" +
			                  "pickUp('gerst')\n" +
			                  "pickUp('broodje')\n" +
			                  "pickUp('appel')\n" +
			                  "pickUp('peer')\n" +
			                  "pickUp('druif')\n" +
			                  "pickUp('suiker')\n" +
			                  "pickUp('peper')\n" +
			                  "pickUp('banaan')\n" +
			                  "pickUp('salami')\n" +
			                  "pickUp('worst')\n" +
			                  "pickUp('advocado')\n" +
			                  "pickUp('rum')\n" +
			                  "pickUp('ertjes')\n" +
			                  "pickUp('granaatappel')\n" +
			                  "pickUp('sinasappel')\n" +
			                  "pickUp('inktvis')\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (30, response.Count);
			Assert.IsInstanceOf<PickUp> (response [0]);
			Assert.AreEqual ("paprika", (response [0] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [1]);
			Assert.AreEqual ("tomaat", (response [1] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [2]);
			Assert.AreEqual ("aardappel", (response [2] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [3]);
			Assert.AreEqual ("vis", (response [3] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [4]);
			Assert.AreEqual ("gehakt", (response [4] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [5]);
			Assert.AreEqual ("bloemkool", (response [5] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [6]);
			Assert.AreEqual ("kaas", (response [6] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [7]);
			Assert.AreEqual ("biefstuk", (response [7] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [8]);
			Assert.AreEqual ("kipfilet", (response [8] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [9]);
			Assert.AreEqual ("kip", (response [9] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [10]);
			Assert.AreEqual ("tonijn", (response [10] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [11]);
			Assert.AreEqual ("sla", (response [11] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [12]);
			Assert.AreEqual ("wortel", (response [12] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [13]);
			Assert.AreEqual ("rijst", (response [13] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [14]);
			Assert.AreEqual ("gerst", (response [14] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [15]);
			Assert.AreEqual ("broodje", (response [15] as PickUp).ObjectToPickUP);


			Assert.IsInstanceOf<PickUp> (response [16]);
			Assert.AreEqual ("appel", (response [16] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [17]);
			Assert.AreEqual ("peer", (response [17] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [18]);
			Assert.AreEqual ("druif", (response [18] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [19]);
			Assert.AreEqual ("suiker", (response [19] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [20]);
			Assert.AreEqual ("peper", (response [20] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [21]);
			Assert.AreEqual ("banaan", (response [21] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [22]);
			Assert.AreEqual ("salami", (response [22] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [23]);
			Assert.AreEqual ("worst", (response [23] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [24]);
			Assert.AreEqual ("advocado", (response [24] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [25]);
			Assert.AreEqual ("rum", (response [25] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [26]);
			Assert.AreEqual ("ertjes", (response [26] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [27]);
			Assert.AreEqual ("granaatappel", (response [27] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [28]);
			Assert.AreEqual ("sinasappel", (response [28] as PickUp).ObjectToPickUP);

			Assert.IsInstanceOf<PickUp> (response [29]);
			Assert.AreEqual ("inktvis", (response [29] as PickUp).ObjectToPickUP);

		}

		/// <summary>
		/// Tests if statements of different types are parsed correctly after eachother.
		/// </summary>
		[Test()]
		public void TestMultipleSimpleStatementsDifferentType1(){
		
			tokenize = "moveForward()\n" +
			                  "rotateRight()\n";
			PythonParser p = new PythonParser ();
			response = p.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<TurnRight> (response [1]);
		}

		/// <summary>
		/// Tests if statements of all simple types are parsed correctly after eachother.
		/// </summary>
		[Test()]
		public void TestMultipleSimpleStatementsDifferentType2(){

			tokenize = "moveForward()\n" +
			                  "rotateRight()\n" +
			                  "pickUp('paprika')\n";
			PythonParser p = new PythonParser ();
			response = p.ParseCode (tokenize);
			Assert.AreEqual (3, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<TurnRight> (response [1]);
			Assert.IsInstanceOf<PickUp> (response [2]);
			Assert.AreEqual ("paprika", (response [2] as PickUp).ObjectToPickUP);
		}

		/// <summary>
		/// Tests if statements of all simple types are parsed correctly after each other with multiple of same type.
		/// </summary>
		[Test()]
		public void TestMultipleSimpleStatementsDifferentType3(){

			tokenize = "moveForward()\n" +
			                  "moveForward()\n" +
			                  "rotateRight()\n" +
			                  "pickUp('paprika')\n" +
			                  "moveForward()\n";
			PythonParser p = new PythonParser ();
			response = p.ParseCode (tokenize);
			Assert.AreEqual (5, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<Forward> (response [1]);
			Assert.IsInstanceOf<TurnRight> (response [2]);
			Assert.IsInstanceOf<PickUp> (response [3]);
			Assert.AreEqual ("paprika", (response [3] as PickUp).ObjectToPickUP);
			Assert.IsInstanceOf<Forward> (response [4]);
		}


		#endregion

		#region WhileCommands

		[Test]
		public void TestSingleWhileSimpleCondition1(){
			tokenize = "while left:\n" +
			                  "\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [0]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestSingleWhileSimpleCondition2(){
			tokenize = "while right:\n" +
			                  "\tmoveForward()\n" +
			                  "\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [0] as WhileLoop).getChildren () [1]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Right, (solver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestSingleWhileSimpleCondition3(){
			tokenize = "moveForward()\n" +
			                  "while forward:\n" +
			                  "\tmoveForward()\n" +
			                  "\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<WhileLoop> (response [1]);
			Assert.IsInstanceOf<Forward> ((response [1] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [1] as WhileLoop).getChildren () [1]);
			Solver solver = (response [1] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Forward, (solver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestSingleWhileSimpleCondition4(){
			tokenize = "while left:\n" +
			                  "\tmoveForward()\n" +
			                  "\trotateRight()\n"
			                  + "moveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [0] as WhileLoop).getChildren () [1]);
			Assert.IsInstanceOf<Forward> (response [1]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestSingleWhileTwoConditions1(){
			tokenize = "while left and right:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [0]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Left, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Right, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestSingleWhileTwoConditions2(){
			tokenize = "while right or left:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [0] as WhileLoop).getChildren () [1]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Right, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Left, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestSingleWhileTwoConditions3(){
			tokenize = "moveForward()\n" +
				"while forward and backward:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<WhileLoop> (response [1]);
			Assert.IsInstanceOf<Forward> ((response [1] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [1] as WhileLoop).getChildren () [1]);
			Solver solver = (response [1] as Composite).Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Forward, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Backward, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestSingleWhileTwoConditions4(){
			tokenize = "while backward or forward:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n"
				+ "moveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [0] as WhileLoop).getChildren () [1]);
			Assert.IsInstanceOf<Forward> (response [1]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Backward, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Forward, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
		}


		[Test]
		public void TestNestedWhileSimpleCondition1(){
			tokenize = "while left:\n" +
							  "\twhile right:\n" +	
							  "\t\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.IsInstanceOf<WhileLoop> ((response [0] as WhileLoop).getChildren () [0]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);
			Solver nestedSolver = ((response [0] as WhileLoop).getChildren()[0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (nestedSolver);
			Assert.AreEqual (ECanInstructions.Right, (nestedSolver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestNestedWhileSimpleCondition2(){
			tokenize = "while right:\n" +
				"\twhile left:\n" +
				"\t\tmoveForward()\n" +
				"\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.IsInstanceOf<WhileLoop> ((response [0] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [0] as WhileLoop).getChildren () [1]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Right, (solver as ConcreteInstruction).Instruction);
			Solver nestedSolver = ((response [0] as WhileLoop).getChildren () [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (nestedSolver);
			Assert.AreEqual (ECanInstructions.Left, (nestedSolver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestNestedWhileSimpleCondition3(){
			tokenize = "moveForward()\n" +
			                  "while forward:\n" +
			                  "\tmoveForward()\n" +
			                  "\trotateRight()\n" +
			                  "\twhile backward:\n" +
							  "\t\trotateRight()\n" +
			                  "rotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (3, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<WhileLoop> (response [1]);
			Assert.IsInstanceOf<TurnRight> (response [2]);
			Assert.IsInstanceOf<Forward> ((response [1] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [1] as WhileLoop).getChildren () [1]);
			Assert.IsInstanceOf<WhileLoop> ((response [1] as WhileLoop).getChildren () [2]);

			Solver solver = (response [1] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Forward, (solver as ConcreteInstruction).Instruction);
			Solver nestedSolver = ((response [1] as WhileLoop).getChildren () [2] as Composite).Conditions;
			Assert.AreEqual (ECanInstructions.Backward, (nestedSolver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestNestedWhileSimpleCondition4(){
			tokenize = "while left:\n" +
				"\twhile forward:\n" +
				"\t\trotateRight()\n" +
				"\twhile backward:\n" +
				"\t\tmoveForward()\n" +
			    "moveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.IsInstanceOf<WhileLoop> ((response [0] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<WhileLoop> ((response [0] as WhileLoop).getChildren () [1]);
			Assert.IsInstanceOf<Forward> (response [1]);
			Assert.IsInstanceOf<Forward> (response [1]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);

			Solver firstNestedSolver = ((response [0] as WhileLoop).getChildren () [0] as Composite).Conditions;
			Solver secondNestedSolver = ((response [0] as WhileLoop).getChildren () [1] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (firstNestedSolver);
			Assert.IsInstanceOf<ConcreteInstruction> (secondNestedSolver);
			Assert.AreEqual (ECanInstructions.Forward, (firstNestedSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Backward, (secondNestedSolver as ConcreteInstruction).Instruction);

		}


		[Test]
		public void TestDoubleNestedWhileSimpleCondition1(){
			tokenize = "while left:\n" +
				"\twhile right:\n" + 
				"\t\twhile forward:\n" +
				"\t\t\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.IsInstanceOf<WhileLoop> ((response [0] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<WhileLoop> ((((response [0] as WhileLoop).getChildren () [0]) as WhileLoop).getChildren() [0]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestDoubleNestedWhileSimpleCondition2(){
			tokenize = "while right:\n" +
				"\twhile left:\n" +
				"\t\tmoveForward()\n" +
				"\t\twhile forward:\n" +
				"\t\t\tpickUp('paprika')\n" +
				"\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.IsInstanceOf<WhileLoop> ((response [0] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<Forward> ((((response [0] as WhileLoop).getChildren () [0]) as WhileLoop).getChildren() [0]);
			Assert.IsInstanceOf<WhileLoop> ((((response [0] as WhileLoop).getChildren () [0]) as WhileLoop).getChildren() [1]);
			Assert.IsInstanceOf<TurnRight> ((response [0] as WhileLoop).getChildren () [1]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Right, (solver as ConcreteInstruction).Instruction);
		}

		[Test]
		public void TestDoubleNestedWhileSimpleCondition3(){
			tokenize = "moveForward()\n" +
				"while forward:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n" +
				"\twhile backward:\n" +
				"\t\trotateRight()\n" +
				"\t\twhile left:\n" +
				"\t\t\tpickUp('aardappel')\n" +
				"rotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (3, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<WhileLoop> (response [1]);
			Assert.IsInstanceOf<TurnRight> (response [2]);
			Assert.IsInstanceOf<Forward> ((response [1] as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [1] as WhileLoop).getChildren () [1]);
			Assert.IsInstanceOf<WhileLoop> ((response [1] as WhileLoop).getChildren () [2]);
			Assert.IsInstanceOf<TurnRight> ((((response [1] as WhileLoop).getChildren () [2]) as WhileLoop).getChildren () [0]);
			Assert.IsInstanceOf<WhileLoop> ((((response [1] as WhileLoop).getChildren () [2]) as WhileLoop).getChildren () [1]);


			Solver solver = (response [1] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Forward, (solver as ConcreteInstruction).Instruction);
		}

		#endregion

		#region IFCommands
		

		[Test()]
		public void TestSingleIFStament1(){
		
			tokenize = "if at('greengrocer'):\n" +
			                  "\tpickUp('cabbage')\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.At, (solver as ConcreteInstruction).Instruction);

		}

		[Test()]
		public void TestSingleIFStament2(){

			tokenize = "if left:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as IfStatement).getChildren () [0]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);

		}

		[Test()]
		public void TestSingleIFStament3(){
			tokenize = "if left:\n" +
			                  "\tmoveForward()\n" +
			                  "\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as IfStatement).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [0] as IfStatement).getChildren () [1]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);
		}

		[Test()]
		public void TestSingleIFStament4(){
			tokenize = 
				"moveForward()\n" +
				"if left:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<IfStatement> (response [1]);
			Assert.IsInstanceOf<Forward> ((response [1] as IfStatement).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [1] as IfStatement).getChildren () [1]);
			Solver solver = (response [1] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);
		}

		[Test()]
		public void TestSingleIFStament5(){
			tokenize = "if left:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n" + 
				"moveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as IfStatement).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [0] as IfStatement).getChildren () [1]);
			Assert.IsInstanceOf<Forward> (response [1]);
			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);
		}

		[Test()]
		public void TestTwoIFStatements(){
			tokenize = "if left:\n" +
			                  "\tmoveForward()\n" +
			                  "if right:\n" +
			                  "\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.IsInstanceOf<IfStatement> (response [1]);
			Assert.IsInstanceOf<Forward> ((response [0] as IfStatement).getChildren () [0]);
			Assert.IsInstanceOf<TurnRight> ((response [1] as IfStatement).getChildren () [0]);

			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);

			Solver secondSolver = (response [1] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (secondSolver);
			Assert.AreEqual (ECanInstructions.Right, (secondSolver as ConcreteInstruction).Instruction);

		}

		[Test()]
		public void NestedIF(){
			tokenize = "if forward:\n" +
			                  "\tif forward:\n" +
			                  "\t\tmoveForward()\n" +
			                  "moveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.IsInstanceOf<Forward> (response [1]);
			Assert.IsInstanceOf<IfStatement> ((response [0] as IfStatement).getChildren () [0]);

			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Forward, (solver as ConcreteInstruction).Instruction);

			Solver secondSolver = ((response [0] as IfStatement).getChildren()[0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (secondSolver);
			Assert.AreEqual (ECanInstructions.Forward, (secondSolver as ConcreteInstruction).Instruction);
		}

		[Test()]
		public void NestedIF2(){
			tokenize = "if forward:\n" +
			                  "\tif forward:\n" +
			                  "\t\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.IsInstanceOf<IfStatement> ((response [0] as IfStatement).getChildren () [0]);

			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Forward, (solver as ConcreteInstruction).Instruction);

			Solver secondSolver = ((response [0] as IfStatement).getChildren()[0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (secondSolver);
			Assert.AreEqual (ECanInstructions.Forward, (secondSolver as ConcreteInstruction).Instruction);
		}

		[Test()]
		public void NestedIF3(){
			tokenize = "moveForward()\n" +
			                  "if right:\n" +
			                  "\tmoveForward()\n" +
			                  "\tif left:\n" +
			                  "\t\tmoveForward()\n" +
			                  "rotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (3, response.Count);
			Assert.IsInstanceOf<Forward> (response [0]);
			Assert.IsInstanceOf<IfStatement> (response [1]);
			Assert.IsInstanceOf<TurnRight> (response [2]);


			Solver solver = (response [1] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Right, (solver as ConcreteInstruction).Instruction);

			Solver secondSolver = ((response [1] as IfStatement).getChildren()[1] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (secondSolver);
			Assert.AreEqual (ECanInstructions.Left, (secondSolver as ConcreteInstruction).Instruction);


		}


		[Test()]
		public void IFElseTest1(){
			tokenize = "if left:\n" +
			                  "\trotateRight()\n" +
			                  "else:\n" +
			                  "\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.AreEqual (1, (response [0] as IfStatement).getChildren ().Count);
			Assert.AreEqual (1, (response [0] as IfStatement).ElseChildren.Count);
			Assert.IsInstanceOf<TurnRight> ((response [0] as IfStatement).getChildren() [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as IfStatement).ElseChildren [0]);
		}


		[Test()]
		public void IFElseTest2(){
			tokenize = "if left:\n" +
				"\trotateRight()\n" +
				"\tmoveForward()\n" +
				"else:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.AreEqual (2, (response [0] as IfStatement).getChildren ().Count);
			Assert.AreEqual (1, (response [0] as IfStatement).ElseChildren.Count);
			Assert.IsInstanceOf<TurnRight> ((response [0] as IfStatement).getChildren() [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as IfStatement).ElseChildren [0]);
		}

		[Test()]
		public void IFElseTest3(){
			tokenize = "if left:\n" +
				"\trotateRight()\n" +
				"else:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.AreEqual (1, (response [0] as IfStatement).getChildren ().Count);
			Assert.AreEqual (2, (response [0] as IfStatement).ElseChildren.Count);
			Assert.IsInstanceOf<TurnRight> ((response [0] as IfStatement).getChildren() [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as IfStatement).ElseChildren [0]);
		}

		[Test()]
		public void IFElifTest1(){
			tokenize = "if left:\n" +
				"\trotateRight()\n" +
				"elif right:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.AreEqual (1, (response [0] as IfStatement).getChildren ().Count);
			Assert.AreEqual (1, (response [0] as IfStatement).ElseChildren.Count);
			Assert.IsInstanceOf<TurnRight> ((response [0] as IfStatement).getChildren() [0]);
			Assert.IsInstanceOf<IfStatement> ((response [0] as IfStatement).ElseChildren [0]);

			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);

			Solver secondSolver = ((response [0] as IfStatement).ElseChildren[0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (secondSolver);
			Assert.AreEqual (ECanInstructions.Right, (secondSolver as ConcreteInstruction).Instruction);



		}


		[Test()]
		public void IFElifTest2(){
			tokenize = "if left:\n" +
				"\trotateRight()\n" +
				"\tmoveForward()\n" +
				"elif right:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.AreEqual (2, (response [0] as IfStatement).getChildren ().Count);
			Assert.AreEqual (1, (response [0] as IfStatement).ElseChildren.Count);
			Assert.IsInstanceOf<TurnRight> ((response [0] as IfStatement).getChildren() [0]);
			Assert.IsInstanceOf<IfStatement> ((response [0] as IfStatement).ElseChildren [0]);


			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);

			Solver secondSolver = ((response [0] as IfStatement).ElseChildren[0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (secondSolver);
			Assert.AreEqual (ECanInstructions.Right, (secondSolver as ConcreteInstruction).Instruction);
		}

		[Test()]
		public void IFElifTest3(){
			tokenize = "if left:\n" +
				"\trotateRight()\n" +
				"elif right:\n" +
				"\tmoveForward()\n" +
				"\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.AreEqual (1, (response [0] as IfStatement).getChildren ().Count);
			Assert.AreEqual (1, (response [0] as IfStatement).ElseChildren.Count);
			Assert.IsInstanceOf<TurnRight> ((response [0] as IfStatement).getChildren() [0]);
			Assert.IsInstanceOf<IfStatement> ((response [0] as IfStatement).ElseChildren [0]);

			Solver solver = (response [0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);

			Solver secondSolver = ((response [0] as IfStatement).ElseChildren[0] as Composite).Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (secondSolver);
			Assert.AreEqual (ECanInstructions.Right, (secondSolver as ConcreteInstruction).Instruction);


		}



	//butcher
   //greengrocer

		[Test()]
		public void IFTwoElifTest1(){
			tokenize = "if at('butcher'):\n" +
			                  "\trotateRight()\n" +
			                  "elif at('greengrocer'):\n" +
			                  "\tmoveForward()\n" +
			                  "elif at('bakery'):\n" +
							  "\tpickUp('bread')\n" +
							  "elif at('bakery'):\n" +
							"\tpickUp('sandwitch')\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.AreEqual (1, (response [0] as IfStatement).getChildren ().Count);
			Assert.AreEqual (1, (response [0] as IfStatement).ElseChildren.Count);
			Assert.IsInstanceOf<TurnRight> ((response [0] as IfStatement).getChildren() [0]);
			Assert.IsInstanceOf<IfStatement> ((response [0] as IfStatement).ElseChildren [0]);
			IfStatement firstElif = (response [0] as IfStatement).ElseChildren [0] as IfStatement;
			Assert.AreEqual (1, firstElif.getChildren ().Count);
			Assert.AreEqual (1, firstElif.ElseChildren.Count);
			IfStatement secondElif = firstElif.ElseChildren [0] as IfStatement;
			Assert.AreEqual (1, secondElif.getChildren ().Count);
			Assert.AreEqual (1, secondElif.ElseChildren.Count);

			IfStatement thirdElif = secondElif.ElseChildren [0] as IfStatement;
			Assert.AreEqual (1, thirdElif.getChildren ().Count);
			Assert.AreEqual (0, thirdElif.ElseChildren.Count);


			Solver solver1 = firstElif.Conditions;
			Solver solver2 = secondElif.Conditions;
			Solver solver3 = thirdElif.Conditions;

			Assert.IsInstanceOf<ConcreteInstruction> (solver1);
			Assert.IsInstanceOf<ConcreteInstruction> (solver2);
			Assert.IsInstanceOf<ConcreteInstruction> (solver3);
			Assert.AreEqual (ECanInstructions.At, (solver1 as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.At, (solver2 as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.At, (solver3 as ConcreteInstruction).Instruction);

			Assert.AreEqual ("greengrocer", solver1.ShopName);
			Assert.AreEqual ("bakery", solver2.ShopName);
			Assert.AreEqual ("bakery", solver3.ShopName);


		}

		[Test()]
		public void IFElifElseTest1()
		{
			tokenize = "if left:\n" +
			                  "\tmoveForward()\n" +
			                  "elif right:\n" +
			                  "\trotateRight()\n" +
			                  "else:\n" +
			                  "\tpickUp('bread')\n" +
			                  "\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.IsInstanceOf<Forward> ((response [0] as IfStatement).getChildren () [0]);
			Assert.IsInstanceOf<IfStatement> ((response [0] as IfStatement).ElseChildren [0]);
			IfStatement i = (response [0] as IfStatement).ElseChildren [0] as IfStatement;
			Assert.IsInstanceOf<TurnRight> (i.getChildren () [0]);
			Assert.AreEqual (2, (((response [0] as IfStatement).ElseChildren [0] as IfStatement).ElseChildren.Count));
			Assert.IsInstanceOf<PickUp> ((((response [0] as IfStatement).ElseChildren [0] as IfStatement).ElseChildren [0]));
			Assert.IsInstanceOf<Forward> ((((response [0] as IfStatement).ElseChildren [0] as IfStatement).ElseChildren [1]));

		}

		[Test()]
		public void IFElseIFTest1(){
		
			tokenize = "if left:\n" +
			                  "\tmoveForward()\n" +
			                  "else:\n" +
			                  "\tmoveForward()\n" +
			                  "if right:\n" +
			                  "\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.IsInstanceOf<IfStatement> (response [1]);
			Assert.AreEqual (1, (response [0] as IfStatement).ElseChildren.Count);
			Assert.AreEqual (0, (response [1] as IfStatement).ElseChildren.Count);
		}

		[Test()]
		public void DoubleNestIFTest1(){
		
			tokenize = "if right:\n" +
			                  "\tmoveForward()\n" +
			                  "\tif forward:\n" +
			                  "\t\trotateRight()\n" +
			                  "\t\tif backward:\n" +
			                  "\t\t\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.AreEqual (2, (response [0] as IfStatement).getChildren ().Count);
			Assert.AreEqual (2, ((response [0] as IfStatement).getChildren () [1] as IfStatement).getChildren ().Count);

		}

		[Test()]
		public void NestedIFElseTest1(){
			tokenize = "if right:\n" +
			                  "\tif left:\n" +
			                  "\t\tmoveForward()\n" +
			                  "else:\n" +
			                  "\trotateRight()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0] );
			Assert.AreEqual(1,  (response[0] as IfStatement).getChildren().Count);
			Assert.AreEqual(1,  (response[0] as IfStatement).ElseChildren.Count);
		}


		#endregion


		#region ConditionTestsPythonParser
			
		[Test()]
		public void SimpleConditionTest1(){

			tokenize = "while left:\n" +
			                  "\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);
		}

		[Test()]
		public void SimpleConditionTest2(){

			tokenize = "while forward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Forward, (solver as ConcreteInstruction).Instruction);
		}

		[Test()]
		public void SimpleConditionWithNotTest1(){

			tokenize = "while not right:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Not, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Right, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
		}

		[Test()]
		public void SimpleConditionWithNotTest2(){

			tokenize = "while not backward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Not, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Backward, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
		}


		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void SimpleConditionWithNotAfterConditionTest1(){

			tokenize = "while left not:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Not, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Right, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void SimpleConditionWithNotAfterConditionTest2(){

			tokenize = "while forward not:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Not, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Backward, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
		}

		[Test()]
		public void TwoConditionWithAndTest1(){

			tokenize = "while right and forward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Right, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Forward, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);

		}

		[Test()]
		public void TwoConditionWithAndTest2(){

			tokenize = "while backward and left:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Backward, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Left, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
		}

		[Test()]
		public void TwoConditionWithOrTest1(){

			tokenize = "while right or forward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Right, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Forward, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);

		}

		[Test()]
		public void TwoConditionWithOrTest2(){

			tokenize = "while backward or left:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Backward, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Left, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
		}


		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TwoConditionWithNotOperatorTest1(){

			tokenize = "while right not forward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Not, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Right, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Forward, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);

		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TwoConditionWithNotOperatorTest2(){

			tokenize = "while backward not left:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Not, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Backward, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Left, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
		}


		[Test()]
		public void TwoConditionWithAndOperatorAndNotConditionTest1(){

			tokenize = "while right and not forward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Right, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			ConditionCombo combo = ((solver as ConditionCombo).RightSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Forward, (combo.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);

		}

		[Test()]
		public void TwoConditionWithAndOperatorAndNotConditionTest2(){

			tokenize = "while backward and not left:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Backward, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			ConditionCombo combo = ((solver as ConditionCombo).RightSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Left, (combo.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);
		}

		[Test()]
		public void TwoConditionWithOrOperatorAndNotConditionTest1(){

			tokenize = "while right or not forward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Right, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			ConditionCombo combo = ((solver as ConditionCombo).RightSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Forward, (combo.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);

		}

		[Test()]
		public void TwoConditionWithOrOperatorAndNotConditionTest2(){

			tokenize = "while backward or not left:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Backward, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			ConditionCombo combo = ((solver as ConditionCombo).RightSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Left, (combo.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);
		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TwoConditionWithNotOperatorAndNotConditionTest1(){

			tokenize = "while right not not forward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Right, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			ConditionCombo combo = ((solver as ConditionCombo).RightSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Forward, (combo.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);

		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TwoConditionWithNotOperatorAndNotConditionTest2(){

			tokenize = "while backward not not left:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Backward, ((solver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			ConditionCombo combo = ((solver as ConditionCombo).RightSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Left, (combo.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);
		}



		[Test()]
		public void TwoConditionWithAndOperatorAndNotConditionTest3(){

			tokenize = "while not right and forward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Forward, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
			ConditionCombo combo = ((solver as ConditionCombo).LeftSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Right, (combo.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);

		}

		[Test()]
		public void TwoConditionWithAndOperatorAndNotConditionTest4(){

			tokenize = "while not backward and left:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Left, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
			ConditionCombo combo = ((solver as ConditionCombo).LeftSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Backward, (combo.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);
		}


		[Test()]
		public void TwoConditionWithOrOperatorAndNotConditionTest3(){

			tokenize = "while not right or forward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Forward, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
			ConditionCombo combo = ((solver as ConditionCombo).LeftSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Right, (combo.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);

		}

		[Test()]
		public void TwoConditionWithOrOperatorAndNotConditionTest4(){

			tokenize = "while not backward or left:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Left, ((solver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
			ConditionCombo combo = ((solver as ConditionCombo).LeftSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Backward, (combo.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, combo.LogicOperator);
		}


		[Test()]
		public void TwoConditionWithAndOperatorAndNotConditionBothOperatorsTest1(){

			tokenize = "while not right and not forward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);

			ConditionCombo comboLeft = ((solver as ConditionCombo).LeftSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Right, (comboLeft.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, comboLeft.LogicOperator);

			ConditionCombo comboRight = ((solver as ConditionCombo).RightSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Forward, (comboRight.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, comboRight.LogicOperator);

		}

		[Test()]
		public void TwoConditionWithAndOperatorAndNotConditionBothOperatorsTest2(){

			tokenize = "while not backward and not left:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);


			ConditionCombo comboLeft = ((solver as ConditionCombo).LeftSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Backward, (comboLeft.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, comboLeft.LogicOperator);

			ConditionCombo comboRight = ((solver as ConditionCombo).RightSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Left, (comboRight.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, comboRight.LogicOperator);
		}

		[Test()]
		public void TwoConditionWithOrOperatorAndNotConditionBothOperatorsTest1(){

			tokenize = "while not right or not forward:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);

			ConditionCombo comboLeft = ((solver as ConditionCombo).LeftSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Right, (comboLeft.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, comboLeft.LogicOperator);

			ConditionCombo comboRight = ((solver as ConditionCombo).RightSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Forward, (comboRight.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, comboRight.LogicOperator);

		}

		[Test()]
		public void TwoConditionWithOrOperatorAndNotConditionBothOperatorsTest2(){

			tokenize = "while not backward or not left:\n" +
				"\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			WhileLoop whileLoop = (response [0] as WhileLoop);
			Solver solver = whileLoop.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);


			ConditionCombo comboLeft = ((solver as ConditionCombo).LeftSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Backward, (comboLeft.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, comboLeft.LogicOperator);

			ConditionCombo comboRight = ((solver as ConditionCombo).RightSolver as ConditionCombo);
			Assert.AreEqual (ECanInstructions.Left, (comboRight.LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Not, comboRight.LogicOperator);
		}


		[Test()]
		public void ParenthesisTest1()
		{
			tokenize = "if (left):\n" +
			                  "\tmoveForward()\n";

			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifstatement = (response [0] as IfStatement);
			Solver solver = ifstatement.Conditions;
			Assert.IsInstanceOf<ConcreteInstruction> (solver);
			Assert.AreEqual (ECanInstructions.Left, (solver as ConcreteInstruction).Instruction);

		}

		[Test()]
		public void ParenthesisTest2()
		{
			tokenize = "if (left or right):\n" +
				"\tmoveForward()\n";

			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifstatement = (response [0] as IfStatement);
			Solver solver = ifstatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Solver leftCondition = (solver as ConditionCombo).LeftSolver;
			Solver rightCondition = (solver as ConditionCombo).RightSolver;
			Assert.IsInstanceOf<ConcreteInstruction> (leftCondition);
			Assert.IsInstanceOf<ConcreteInstruction> (rightCondition);
			Assert.AreEqual (ECanInstructions.Left, (leftCondition as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Right, (rightCondition as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);

		}

		[Test()]
		public void ParenthesisTest3()
		{
			tokenize = "if (left and right):\n" +
				"\tmoveForward()\n";

			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifstatement = (response [0] as IfStatement);
			Solver solver = ifstatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Solver leftCondition = (solver as ConditionCombo).LeftSolver;
			Solver rightCondition = (solver as ConditionCombo).RightSolver;
			Assert.IsInstanceOf<ConcreteInstruction> (leftCondition);
			Assert.IsInstanceOf<ConcreteInstruction> (rightCondition);
			Assert.AreEqual (ECanInstructions.Left, (leftCondition as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Right, (rightCondition as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);

		}

		[Test()]
		public void ParenthesisTest4()
		{
			tokenize = "if forward and (backward or left):\n" +
				"\tmoveForward()\n";

			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifstatement = (response [0] as IfStatement);
			Solver solver = ifstatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Solver leftSolver = (solver as ConditionCombo).LeftSolver;
			Solver rightSolver = (solver as ConditionCombo).RightSolver;
			Assert.IsInstanceOf<ConcreteInstruction> (leftSolver);
			Assert.IsInstanceOf<ConditionCombo> (rightSolver);
			Assert.AreEqual (ECanInstructions.Forward, (leftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);

			Assert.IsInstanceOf<ConcreteInstruction> ((rightSolver as ConditionCombo).LeftSolver);
			Assert.IsInstanceOf<ConcreteInstruction> ((rightSolver as ConditionCombo).RightSolver);
			Assert.AreEqual (ECanInstructions.Backward, ((rightSolver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Left, ((rightSolver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Or, (rightSolver as ConditionCombo).LogicOperator);

		}

		[Test()]
		public void ParenthesisTest5()
		{
			tokenize = "if right or (forward and left):\n" +
				"\tmoveForward()\n";

			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifstatement = (response [0] as IfStatement);
			Solver solver = ifstatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Solver leftSolver = (solver as ConditionCombo).LeftSolver;
			Solver rightSolver = (solver as ConditionCombo).RightSolver;
			Assert.IsInstanceOf<ConcreteInstruction> (leftSolver);
			Assert.IsInstanceOf<ConditionCombo> (rightSolver);
			Assert.AreEqual (ECanInstructions.Right, (leftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);

			Assert.IsInstanceOf<ConcreteInstruction> ((rightSolver as ConditionCombo).LeftSolver);
			Assert.IsInstanceOf<ConcreteInstruction> ((rightSolver as ConditionCombo).RightSolver);
			Assert.AreEqual (ECanInstructions.Forward, ((rightSolver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Left, ((rightSolver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.And, (rightSolver as ConditionCombo).LogicOperator);

		}

		[Test()]
		public void ParenthesisTest4b()
		{
			tokenize = "if (backward or left) and forward:\n" +
				"\tmoveForward()\n";

			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifstatement = (response [0] as IfStatement);
			Solver solver = ifstatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Solver leftSolver = (solver as ConditionCombo).LeftSolver;
			Solver rightSolver = (solver as ConditionCombo).RightSolver;
			Assert.IsInstanceOf<ConditionCombo> (leftSolver);
			Assert.IsInstanceOf<ConcreteInstruction> (rightSolver);
			Assert.AreEqual (ECanInstructions.Forward, (rightSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);

			Assert.IsInstanceOf<ConcreteInstruction> ((leftSolver as ConditionCombo).LeftSolver);
			Assert.IsInstanceOf<ConcreteInstruction> ((leftSolver as ConditionCombo).RightSolver);
			Assert.AreEqual (ECanInstructions.Backward, ((leftSolver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Left, ((leftSolver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Or, (leftSolver as ConditionCombo).LogicOperator);

		}

		[Test()]
		public void ParenthesisTest5b()
		{
			tokenize = "if (forward and left) or right:\n" +
				"\tmoveForward()\n";

			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifstatement = (response [0] as IfStatement);
			Solver solver = ifstatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Solver leftSolver = (solver as ConditionCombo).LeftSolver;
			Solver rightSolver = (solver as ConditionCombo).RightSolver;
			Assert.IsInstanceOf<ConcreteInstruction> (rightSolver);
			Assert.IsInstanceOf<ConditionCombo> (leftSolver);
			Assert.AreEqual (ECanInstructions.Right, (rightSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);

			Assert.IsInstanceOf<ConcreteInstruction> ((leftSolver as ConditionCombo).LeftSolver);
			Assert.IsInstanceOf<ConcreteInstruction> ((leftSolver as ConditionCombo).RightSolver);
			Assert.AreEqual (ECanInstructions.Forward, ((leftSolver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Left, ((leftSolver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.And, (leftSolver as ConditionCombo).LogicOperator);

		}

		[Test()]
		public void ParenthesisTest6()
		{
			tokenize = "if (left and (forward or backward)):\n" +
				"\tmoveForward()\n";

			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifstatement = (response [0] as IfStatement);
			Solver solver = ifstatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.And, (solver as ConditionCombo).LogicOperator);
			Solver leftSolver = (solver as ConditionCombo).LeftSolver;
			Solver rightSolver = (solver as ConditionCombo).RightSolver;
			Assert.IsInstanceOf<ConcreteInstruction> (leftSolver);
			Assert.IsInstanceOf<ConditionCombo> (rightSolver);
			Assert.AreEqual (ECanInstructions.Left, (leftSolver as ConcreteInstruction).Instruction);

			Assert.AreEqual (ECanInstructions.Forward, ((rightSolver as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Backward, ((rightSolver as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Or, ((rightSolver as ConditionCombo).LogicOperator));
		}

		[Test()]
		public void ParenthesisTest7()
		{
			tokenize = "if left and (right or forward) or backward:\n" +
				"\tmoveForward()\n";

			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifstatement = (response [0] as IfStatement);
			Solver solver = ifstatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Solver leftSolver = (solver as ConditionCombo).LeftSolver;
			Solver rightSolver = (solver as ConditionCombo).RightSolver;
			Assert.IsInstanceOf<ConditionCombo> (leftSolver);
			Assert.IsInstanceOf<ConcreteInstruction> (rightSolver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);

			Solver leftSolverLeft = (leftSolver as ConditionCombo).LeftSolver;
			Solver rightSolverLeft = (leftSolver as ConditionCombo).RightSolver;
			Assert.IsInstanceOf<ConcreteInstruction> (leftSolverLeft);
			Assert.IsInstanceOf<ConditionCombo> (rightSolverLeft);
			Assert.AreEqual (ELogicOperators.And, (leftSolver as ConditionCombo).LogicOperator);

			Assert.AreEqual (ECanInstructions.Left, (leftSolverLeft as ConcreteInstruction).Instruction);

			Solver leftChildSolverRightleft = (rightSolverLeft as ConditionCombo).LeftSolver;
			Solver rightChildSolverRightleft = (rightSolverLeft as ConditionCombo).RightSolver;

			Assert.AreEqual (ECanInstructions.Right, (leftChildSolverRightleft as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Forward, (rightChildSolverRightleft as ConcreteInstruction).Instruction);
			Assert.AreEqual (ELogicOperators.Or, (rightSolverLeft as ConditionCombo).LogicOperator);


		}

		[Test()]
		public void ParenthesisTest8()
		{
			tokenize = "if left and forward or backward and right:\n" +
				"\tmoveForward()\n";

			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifstatement = (response [0] as IfStatement);
			Solver solver = ifstatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);
			Solver leftSolver = (solver as ConditionCombo).LeftSolver;
			Solver rightSolver = (solver as ConditionCombo).RightSolver;

			Assert.IsInstanceOf<ConditionCombo> (leftSolver);
			Assert.IsInstanceOf<ConditionCombo> (rightSolver);

			Assert.AreEqual (ELogicOperators.And, (leftSolver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ELogicOperators.And,(rightSolver as ConditionCombo).LogicOperator);

			Solver leftSolverLeft = (leftSolver as ConditionCombo).LeftSolver;
			Solver rightSolverLeft = (leftSolver as ConditionCombo).RightSolver;

			Assert.AreEqual (ECanInstructions.Left, (leftSolverLeft as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Forward, (rightSolverLeft as ConcreteInstruction).Instruction);


			Solver leftSolverRight = (rightSolver as ConditionCombo).LeftSolver;
			Solver rightSolverRight = (rightSolver as ConditionCombo).RightSolver;

			Assert.AreEqual (ECanInstructions.Backward, (leftSolverRight as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Right, (rightSolverRight as ConcreteInstruction).Instruction);


		}

		[Test()]
		public void ParenthesisTest9()
		{
			tokenize = "if (left or (right and (forward or backward))):\n" +
				"\tmoveForward()\n";

			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			IfStatement ifstatement = (response [0] as IfStatement);
			Solver solver = ifstatement.Conditions;
			Assert.IsInstanceOf<ConditionCombo> (solver);
			Assert.AreEqual (ELogicOperators.Or, (solver as ConditionCombo).LogicOperator);
			Solver leftSolver = (solver as ConditionCombo).LeftSolver;
			Solver rightSolver = (solver as ConditionCombo).RightSolver;

			Assert.IsInstanceOf<ConcreteInstruction> (leftSolver);
			Assert.IsInstanceOf<ConditionCombo> (rightSolver);

			Assert.AreEqual (ECanInstructions.Left, (leftSolver as ConcreteInstruction).Instruction);

			Solver rightSolverLeft = (rightSolver as ConditionCombo).LeftSolver;
			Solver rightSolverRight = (rightSolver as ConditionCombo).RightSolver;
			Assert.IsInstanceOf<ConcreteInstruction> (rightSolverLeft);
			Assert.IsInstanceOf<ConditionCombo> (rightSolverRight);

			Assert.AreEqual (ELogicOperators.And, (rightSolver as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Right, (rightSolverLeft as ConcreteInstruction).Instruction);

			Assert.AreEqual (ELogicOperators.Or, (rightSolverRight as ConditionCombo).LogicOperator);
			Assert.AreEqual (ECanInstructions.Forward, ((rightSolverRight as ConditionCombo).LeftSolver as ConcreteInstruction).Instruction);
			Assert.AreEqual (ECanInstructions.Backward, ((rightSolverRight as ConditionCombo).RightSolver as ConcreteInstruction).Instruction);


		}

		#endregion


		#region CombinedICodeBlocks

		[Test()]
		public void ComplexSyntax1() {
			tokenize = "while not forward:\n" +
			                  "\tif right:\n" +
			                  "\t\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<WhileLoop> (response [0]);
			Assert.AreEqual (1, (response [0] as WhileLoop).getChildren ().Count);
			Assert.IsInstanceOf<IfStatement> ((response [0] as WhileLoop).getChildren () [0]);
		}

		[Test()]
		public void ComplexSyntax2() {
			tokenize = "if forward:\n" +
				"\twhile right:\n" +
				"\t\tmoveForward()\n";
			
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<IfStatement> (response [0]);
			Assert.AreEqual (1, (response [0] as IfStatement).getChildren ().Count);
			Assert.IsInstanceOf<WhileLoop> ((response [0] as IfStatement).getChildren () [0]);
		}

		#endregion

		#region InvalidSyntax

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TestElifException1(){
			tokenize = "elif left:\n" +
				"\tmoveForward()\n";
			
			parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TestElifException2(){
			tokenize = 
				"if right:\n" +
				"\trotateRight()\n" +
				"else:\n" +
				"\tmoveForward()\n" +
				"elif left:\n" +
				"\tmoveForward()\n";
			
			parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TestElseException1(){
			tokenize = "else right:\n" +
			                  "\tmoveForward()\n";
			
			parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TestElseException2(){
			tokenize = 
				"if right:\n" +
				"\tmoveForward()\n" +
				"else:\n" +
				"\tmoveForward()\n" +
				"else:\n" +
				"\tmoveForward()\n";
			
			parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TestEmptyIF1(){
			tokenize = 
				"moveForward()\n" +
				"if right:\n";
			
			parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TestEmptyIF2(){
			tokenize = 
				"moveForward()\n" +
				"if right:\n" +
				"moveForward()\n";
			
			parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TestEmptyIF3(){
			tokenize = 
				"moveForward()\n" +
				"if right:\n" +
				"\t\n" +
				"moveForward()\n";
			
			parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TestEmptyWhile(){
		
			tokenize = "moveForward()\n" +
			                  "while right:\n";
			
			parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TestEmptyWhile2(){
			tokenize = "moveForward()\n" +
			                  "while left:\n" +
			                  "rotateRight()\n";
			
			parser.ParseCode (tokenize);
			Assert.Fail ();
		}


		[Test()]
		[ExpectedException(typeof(CodeParseException))]
		public void TestEmpttWhile3(){
			tokenize = 
				"moveForward()\n" +
				"while right:\n" +
				"\t\n" +
				"moveForward()\n";
			
			parser.ParseCode (tokenize);
			Assert.Fail ();
		}




		#endregion

		#region VariablesTest

		[Test]
		public void TestDefineVariableInt1(){
			
			tokenize = "i = 1\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
			CmdDefineVariable variable = (response [0] as CmdDefineVariable);
			Assert.AreEqual ("i", variable.VariableName);
			DefineVariable dVar = variable.VarValue;
			Assert.IsInstanceOf<ConcreteVariable> (dVar.Assignment);
			ConcreteVariable cVar = (dVar.Assignment as ConcreteVariable);
			Assert.AreEqual (1, cVar.MyVariable.Value);
			Assert.AreEqual (EVariableType.Int, cVar.MyVariable.Type);
			Assert.AreEqual (null, cVar.VariableName);
		}

		[Test]
		public void TestDefineVariableString1(){
			
			tokenize = "j = \"this is my special text\"\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
			CmdDefineVariable variable = (response [0] as CmdDefineVariable);
			Assert.AreEqual ("j", variable.VariableName);
			DefineVariable dVar = variable.VarValue;
			Assert.IsInstanceOf<ConcreteVariable> (dVar.Assignment);
			ConcreteVariable cVar = (dVar.Assignment as ConcreteVariable);
			Assert.AreEqual ("this is my special text", cVar.MyVariable.Value);
			Assert.AreEqual (EVariableType.String, cVar.MyVariable.Type);
			Assert.AreEqual (null, cVar.VariableName);
		}

		[Test]
		public void TestDefineVariableBool1(){
			
			tokenize = "k = True\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
			CmdDefineVariable variable = (response [0] as CmdDefineVariable);
			Assert.AreEqual ("k", variable.VariableName);
			DefineVariable dVar = variable.VarValue;
			Assert.IsInstanceOf<ConcreteVariable> (dVar.Assignment);
			ConcreteVariable cVar = (dVar.Assignment as ConcreteVariable);
			Assert.AreEqual (true, cVar.MyVariable.Value);
			Assert.AreEqual (EVariableType.Bool, cVar.MyVariable.Type);
			Assert.AreEqual (null, cVar.VariableName);
		}

		[Test]
		public void TestDefineVariableInt2(){
			
			tokenize = "i = 1\n" +
			                  "j = i + 23\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
			Assert.IsInstanceOf<CmdDefineVariable> (response [1]);
			CmdDefineVariable variable = (response [0] as CmdDefineVariable);
			Assert.AreEqual ("i", variable.VariableName);
			DefineVariable dVar = variable.VarValue;
			Assert.IsInstanceOf<ConcreteVariable> (dVar.Assignment);
			ConcreteVariable cVar = (dVar.Assignment as ConcreteVariable);
			Assert.AreEqual (1, cVar.MyVariable.Value);
			Assert.AreEqual (EVariableType.Int, cVar.MyVariable.Type);
			Assert.AreEqual (null, cVar.VariableName);

			CmdDefineVariable secondVariable = (response [1] as CmdDefineVariable);
			Assert.AreEqual ("j", secondVariable.VariableName);
			DefineVariable secondDVar = secondVariable.VarValue;
			Assert.IsInstanceOf<VariableCombo> (secondDVar.Assignment);
			VariableCombo combo = (secondDVar.Assignment as VariableCombo);
			Assert.AreEqual (EMathOperator.Add, combo.MathOperator);
			Assert.IsInstanceOf<ConcreteVariable> (combo.LeftSolver);
			Assert.IsInstanceOf<ConcreteVariable> (combo.RightSolver);


			Assert.AreEqual ("i", (combo.LeftSolver as ConcreteVariable).VariableName);
			Assert.AreEqual (23, (combo.RightSolver as ConcreteVariable).MyVariable.Value);
		}

		[Test]
		public void TestDefineVariableString2(){
			
			tokenize = "j = \"this is my special text\"\n" +
			                  "j = j + \" extra text\"\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
			Assert.IsInstanceOf<CmdDefineVariable> (response [1]);
			CmdDefineVariable variable = (response [0] as CmdDefineVariable);
			Assert.AreEqual ("j", variable.VariableName);
			DefineVariable dVar = variable.VarValue;
			Assert.IsInstanceOf<ConcreteVariable> (dVar.Assignment);
			ConcreteVariable cVar = (dVar.Assignment as ConcreteVariable);
			Assert.AreEqual ("this is my special text", cVar.MyVariable.Value);
			Assert.AreEqual (EVariableType.String, cVar.MyVariable.Type);
			Assert.AreEqual (null, cVar.VariableName);

			CmdDefineVariable secondVariable = (response [1] as CmdDefineVariable);
			Assert.AreEqual ("j", secondVariable.VariableName);
			DefineVariable secondDVar = secondVariable.VarValue;
			Assert.IsInstanceOf<VariableCombo> (secondDVar.Assignment);
			VariableCombo combo = (secondDVar.Assignment as VariableCombo);
			Assert.AreEqual (EMathOperator.Add, combo.MathOperator);
			Assert.IsInstanceOf<ConcreteVariable> (combo.LeftSolver);
			Assert.IsInstanceOf<ConcreteVariable> (combo.RightSolver);

			Assert.AreEqual ("j", (combo.LeftSolver as ConcreteVariable).VariableName);
			Assert.AreEqual (" extra text", (combo.RightSolver as ConcreteVariable).MyVariable.Value);
		}

		[Test]
		public void TestLoopWithVariableSolver1(){
			
			tokenize = "i = 0\n" +
			                  "while i <= 10:\n" +
			                  "\tmoveForward()\n" +
			                  "\ti = i + 1\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
			Assert.IsInstanceOf<WhileLoop>(response[1]);
			WhileLoop whileLoop = (response [1] as WhileLoop);
			Assert.AreEqual (2, whileLoop.getChildren ().Count);
			Assert.IsInstanceOf<ValueSolver> (whileLoop.Conditions);

			ValueSolver valueSolver = whileLoop.Conditions as ValueSolver;
			Assert.AreEqual (EComparisonOperator.ValueLessThanOrEqualTo, valueSolver.ComparisonOperator);
			Assert.IsInstanceOf<ConcreteVariable> (valueSolver.LeftSolver);
			Assert.IsInstanceOf<ConcreteVariable> (valueSolver.RightSolver);


			ConcreteVariable leftVar = (valueSolver.LeftSolver as ConcreteVariable);
			ConcreteVariable rightVar = (valueSolver.RightSolver as ConcreteVariable);

			Assert.AreEqual ("i", leftVar.VariableName);
			Assert.AreEqual (10, rightVar.MyVariable.Value);
		}

		[Test]
		public void TestLoopWithVariableSolver0(){

			tokenize = "i = 0\n" +
				"while i < 10 or forward:\n" +
				"\tmoveForward()\n" +
				"\ti = i + 1\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
			Assert.IsInstanceOf<WhileLoop>(response[1]);
			WhileLoop whileLoop = (response [1] as WhileLoop);
			Assert.AreEqual (2, whileLoop.getChildren ().Count);


			Assert.IsInstanceOf<ConditionCombo> (whileLoop.Conditions);

			ValueSolver valueSolver = (whileLoop.Conditions as ConditionCombo).LeftSolver as ValueSolver;
			Assert.AreEqual (EComparisonOperator.ValueLessThan, valueSolver.ComparisonOperator);
			Assert.IsInstanceOf<ConcreteVariable> (valueSolver.LeftSolver);
			Assert.IsInstanceOf<ConcreteVariable> (valueSolver.RightSolver);


			ConcreteVariable leftVar = (valueSolver.LeftSolver as ConcreteVariable);
			ConcreteVariable rightVar = (valueSolver.RightSolver as ConcreteVariable);

			Assert.AreEqual ("i", leftVar.VariableName);
			Assert.AreEqual (10, rightVar.MyVariable.Value);
		}

		[Test]
		public void TestLoopWithVariableSolver2(){
			
			tokenize = "i = 0\n" +
				"while i <= 10 / 5:\n" +
				"\tmoveForward()\n" +
				"\ti = i + 1\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (2, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
			Assert.IsInstanceOf<WhileLoop>(response[1]);
			WhileLoop whileLoop = (response [1] as WhileLoop);
			Assert.AreEqual (2, whileLoop.getChildren ().Count);
			Assert.IsInstanceOf<ValueSolver> (whileLoop.Conditions);

			ValueSolver valueSolver = whileLoop.Conditions as ValueSolver;
			Assert.AreEqual (EComparisonOperator.ValueLessThanOrEqualTo, valueSolver.ComparisonOperator);
			Assert.IsInstanceOf<ConcreteVariable> (valueSolver.LeftSolver);
			Assert.IsInstanceOf<VariableCombo> (valueSolver.RightSolver);


			ConcreteVariable leftVar = (valueSolver.LeftSolver as ConcreteVariable);
			VariableCombo rightVar = (valueSolver.RightSolver as VariableCombo);

			Assert.AreEqual ("i", leftVar.VariableName);

			Assert.AreEqual (EMathOperator.Divide, rightVar.MathOperator);
			Assert.IsInstanceOf<ConcreteVariable> (rightVar.LeftSolver);
			Assert.IsInstanceOf<ConcreteVariable> (rightVar.RightSolver);

			ConcreteVariable leftVarRightSide = (rightVar.LeftSolver as ConcreteVariable);
			ConcreteVariable rightVarRightSide = (rightVar.RightSolver as ConcreteVariable);

			Assert.AreEqual (10, leftVarRightSide.MyVariable.Value);
			Assert.AreEqual (5, rightVarRightSide.MyVariable.Value);
		}

		[Test]
		public void TestLoopWithVariableSolver3(){
			
			tokenize = "i = 0\n" +
			                  "j = 20\n" +
			                  "while i + j * 2 <= 10 / 5:\n" +
			                  "\tmoveForward()\n" +
			                  "\ti = i + 1\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (3, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
			Assert.IsInstanceOf<CmdDefineVariable> (response [1]);
			Assert.IsInstanceOf<WhileLoop>(response[2]);
			WhileLoop whileLoop = (response [2] as WhileLoop);
			Assert.AreEqual (2, whileLoop.getChildren ().Count);
			Assert.IsInstanceOf<ValueSolver> (whileLoop.Conditions);

			ValueSolver valueSolver = whileLoop.Conditions as ValueSolver;
			Assert.AreEqual (EComparisonOperator.ValueLessThanOrEqualTo, valueSolver.ComparisonOperator);
			Assert.IsInstanceOf<VariableCombo> (valueSolver.LeftSolver);
			Assert.IsInstanceOf<VariableCombo> (valueSolver.RightSolver);


			VariableCombo leftVar = (valueSolver.LeftSolver as VariableCombo);

			Assert.IsInstanceOf<ConcreteVariable> (leftVar.LeftSolver);
			Assert.IsInstanceOf<VariableCombo> (leftVar.RightSolver);
			Assert.AreEqual (EMathOperator.Add, leftVar.MathOperator);

			ConcreteVariable leftVarb = ((leftVar.RightSolver as VariableCombo).LeftSolver as ConcreteVariable);

			ConcreteVariable rightVarb = ((leftVar.RightSolver as VariableCombo).RightSolver as ConcreteVariable);

			Assert.AreEqual ("i", (leftVar.LeftSolver as ConcreteVariable).VariableName);

			Assert.AreEqual (EMathOperator.Multiply, (leftVar.RightSolver as VariableCombo).MathOperator);
			Assert.AreEqual ("j", leftVarb.VariableName);
			Assert.AreEqual (2, rightVarb.MyVariable.Value);



			VariableCombo rightVar = (valueSolver.RightSolver as VariableCombo);


			Assert.AreEqual (EMathOperator.Divide, rightVar.MathOperator);
			Assert.IsInstanceOf<ConcreteVariable> (rightVar.LeftSolver);
			Assert.IsInstanceOf<ConcreteVariable> (rightVar.RightSolver);

			ConcreteVariable leftVarRightSide = (rightVar.LeftSolver as ConcreteVariable);
			ConcreteVariable rightVarRightSide = (rightVar.RightSolver as ConcreteVariable);

			Assert.AreEqual (10, leftVarRightSide.MyVariable.Value);
			Assert.AreEqual (5, rightVarRightSide.MyVariable.Value);
		}


		#endregion

		#region ForLoopTests

		[Test]
		public  void TestForLoop1(){

			
			tokenize = "for x in range(0,5):\n" +
			                  "\tmoveForward()\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<ForLoop> (response [0]);
			ForLoop myForLoop = response [0] as ForLoop;
			Assert.AreEqual (1, myForLoop.getChildren ().Count);
			Solver solver = myForLoop.Solver;
			Assert.IsInstanceOf<ValueSolver> (solver);
			Assert.AreEqual (EComparisonOperator.ValueLessThan, (solver as ValueSolver).ComparisonOperator);
			Assert.AreEqual ("x", ((solver as ValueSolver).LeftSolver as ConcreteVariable).VariableName);
			Assert.AreEqual (5, ((solver as ValueSolver).RightSolver as ConcreteVariable).MyVariable.Value);

			CmdDefineVariable defineVariable = myForLoop.DeclareVariable;
			Assert.AreEqual (0, (defineVariable.VarValue.Assignment as ConcreteVariable).MyVariable.Value);
			Assert.AreEqual ("x", defineVariable.VariableName);
			CmdDefineVariable incrementVariable = myForLoop.IncrementCommand;
			Assert.AreEqual ("x", incrementVariable.VariableName);
			Assert.IsInstanceOf<VariableCombo> (incrementVariable.VarValue.Assignment);
			VariableCombo myCombo = (incrementVariable.VarValue.Assignment as VariableCombo);
			Assert.AreEqual ("x", (myCombo.LeftSolver as ConcreteVariable).VariableName);
			Assert.AreEqual (1, (myCombo.RightSolver as ConcreteVariable).MyVariable.Value);
			Assert.AreEqual (EMathOperator.Add, myCombo.MathOperator);

		}

		#endregion

		#region FunctionBlocks

		[Test]
		public void TestDeclareForLoop1(){
			
			tokenize = "def rot():\n" +
			                  "\tmoveForward()\n" +
			                  "rot()\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<FunctionBlockExecute> (response [0]);
		}

		#endregion



		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage = "Error At Line [1]: Invalid Syntax, parenthesis starting with '(' are not closed with corresponding ')'")]
		public void TestUnClosedParenthesis(){
			tokenize = String.Format ("if ({0}:\n" + "\t{1}\n", "left", "moveForward()");
			response = parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage = "Error At Line [1]: Invalid Syntax, parenthesis ending with '(' is not opened with corresponding ')'")]
		public void TestUnopenedParenthesis(){
			tokenize = String.Format ("if {0}):\n" + "\t{1}\n", "left", "moveForward()");
			response = parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Expected LogicOperator to separate two conditions")]
		public void TestTwoCommandInCondition(){
			tokenize = String.Format ("if {0} {0} :\n" + "\t{1}\n", "left", "moveForward()");
			response = parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Expected 'not' of type LogicalOperator, but found 'Or' of type LogicalOperator")]
		public void TestOperatorCommandInCondition(){
			tokenize = String.Format ("if or {0} :\n" +  "\t{1}\n", "left", "moveForward()");
			response = parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Unexpected token with a value of 'for' has been found")]
		public void TestForCommandInCondition(){
			tokenize = String.Format ("if for {0} :\n" + "\t{1}\n", "left", "moveForward()");
			response = parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Unexpected token with a value of '+' has been found")]
		public void TestPlusCommandInCondition(){
			tokenize = String.Format ("if + {0} :\n" + "\t{1}\n", "left", "moveForward()");
			response = parser.ParseCode (tokenize);
			Assert.Fail ();
		}

		[Test]
		public void TestLongVariable(){
			tokenize = "ditieenlangevariabledieminimaaltweeregelsinbeslagneemt=1\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Invalid Syntax, found = after call to 'moveForward()'")]
		public void TestMethodEqualsLiteral(){
			tokenize = "moveForward()=1\n";
			response = parser.ParseCode (tokenize);
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Invalid Syntax, found = after call to 'moveFwd()'")]
		public void TestCustomMethodEqualsLiteral(){
			tokenize = "moveFwd()=1\n";
			response = parser.ParseCode (tokenize);
		}

		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Invalid Syntax, Found 'Command' after = of the variable declaration of 'dfjlhgd'")]
		public void TestLiteralEqualsMethod(){
			tokenize = "dfjlhgd = moveForward()\n";
			response = parser.ParseCode (tokenize);
		}

		[Test]
		public void TestCamalCase1(){
			tokenize = "Pascal = 1\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
		}

		[Test]
		public void TestCamalCase2(){
			tokenize = "pasCAL = 1\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
		}

		[Test]
		[ExpectedException(typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Expected = after the declaration of drop, = is required to assign a variable")]
		public void TestCamalCase3(){
			tokenize = "drop table Code\n";
			response = parser.ParseCode (tokenize);
		}

		[Test]
		public void TestLongNumber(){
			tokenize = "test = 999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (1, response.Count);
			Assert.IsInstanceOf<CmdDefineVariable> (response [0]);
		}


		[Test]
		[ExpectedException (typeof(CodeParseException), ExpectedMessage="Error At Line [1]: Expected LogicOperator to separate two conditions")]
		public void IncorrectAt(){
			tokenize = "if at(butcher) :\n";
			response = parser.ParseCode (tokenize);
		}

		[Test]
		public void IncorrectFunction(){
			tokenize = "def rond():\n" +
			"\twhile forward:\n" +
			"\t\tmoveForward()\n" +
			"\tif at('butcher'):\n" +
			"\t\tpickUp('sausage')\n" +
			"\trotateRight()\n" +
			"\n" +
			"for i in range(0,4):\n" +
			"\trond()\n" +
			"moveForward()\n" +
			"moveForward()\n";
			response = parser.ParseCode (tokenize);
			Assert.AreEqual (3, response.Count);
			Assert.IsInstanceOf<ForLoop> (response [0]);
			Assert.IsInstanceOf<Forward> (response [1]);
			Assert.IsInstanceOf<Forward> (response [2]);


		}


	}
}


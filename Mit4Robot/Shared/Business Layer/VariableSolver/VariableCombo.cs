using System;
using Shared.Exceptions;
using Shared.Enums;

#if __ANDROID__
using System.Xml;
using Microsoft.CSharp;
using System.IO;
#endif

namespace Shared.BusinessLayer
{
	public class VariableCombo: VariableSolver
	{


		VariableSolver leftSolver;
		VariableSolver rightSolver;
		EMathOperator mathOperator;

		public VariableSolver LeftSolver {
			get {
				return leftSolver;
			}
		}

		public VariableSolver RightSolver {
			get {
				return rightSolver;
			}
		}

		public EMathOperator MathOperator {
			get {
				return mathOperator;
			}
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Initializes a new instance of the <see cref="Shared.VariableCombo"/> class.
		/// </summary>
		/// <param name="leftSolver">Left solver.</param>
		/// <param name="rightSolver">Right solver.</param>
		/// <param name="mathOperator">Math operator.</param>
		public VariableCombo (VariableSolver leftSolver, VariableSolver rightSolver, EMathOperator mathOperator){
			this.leftSolver = leftSolver;
			this.rightSolver = rightSolver;
			this.mathOperator = mathOperator;
			//XmlReader reader = XmlReader.Create (new StringReader ("<books><book><title>a</title></book></books>"));
		}

		public VariableCombo (VariableSolver solverOne, VariableSolver solverTwo, EAssignmentOperator assignmentOperator)
		{
			//TODO remove this constructor
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Solves the VariableCombo.
		/// </summary>
		/// <param name="parent">Composite Parent.</param>
		public override Variable solve(Composite parent)
		{
			Variable leftValue = leftSolver.solve (parent);
			Variable rightValue = rightSolver.solve (parent);
			Variable returnValue = null;

			try {
				if (leftValue.Type == EVariableType.Int && rightValue.Type == EVariableType.Int) {
					switch (mathOperator) {
					case EMathOperator.Multiply:
						returnValue = new Variable ((leftValue.Value as int?) * (rightValue.Value as int?), EVariableType.Int);
						break;
					case EMathOperator.Divide:
						//TODO cant divide by 0
						returnValue = new Variable ((leftValue.Value as int?) / (rightValue.Value as int?), EVariableType.Int);
						break;
					case EMathOperator.Add:
						returnValue = new Variable ((leftValue.Value as int?) + (rightValue.Value as int?), EVariableType.Int);
						break;
					case EMathOperator.Subtract:
						returnValue = new Variable ((leftValue.Value as int?) - (rightValue.Value as int?), EVariableType.Int);
						break;
					default:
						//TODO throw exception
						break;
					}
				} else {
					switch (mathOperator) {
					case EMathOperator.Multiply:
						returnValue = new Variable (leftValue.Value * rightValue.Value, EVariableType.String);
						break;
					case EMathOperator.Divide:
						//TODO cant divide by 0
						returnValue = new Variable (leftValue.Value / rightValue.Value, EVariableType.String);
						break;
					case EMathOperator.Add:
						returnValue = new Variable (leftValue.Value + rightValue.Value, EVariableType.String);
						break;
					case EMathOperator.Subtract:
						returnValue = new Variable (leftValue.Value - rightValue.Value, EVariableType.String);
						break;
					default:
						//TODO throw exception
						break;
					}
				}

			} catch (DivideByZeroException){
				throw new RunTimeException ("Cannot divide by zero.");
			} 
			catch (Exception) {
				throw new RunTimeException ("Unsupported Operation.");
			} 
			return returnValue;
		}
	}
}


using System;
using Shared.Enums;

namespace Shared.BusinessLayer
{
	public class ConditionCombo : Solver
	{
		Solver leftSolver;

		public Solver LeftSolver {
			get {
				return leftSolver;
			}
		}

		Solver rightSolver;

		public Solver RightSolver {
			get {
				return rightSolver;
			}
		}

		ELogicOperators logicOperator;

		public ELogicOperators LogicOperator {
			get {
				return logicOperator;
			}
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new ConditionCombo
		/// </summary>
		/// <param name="leftSolver">Left solver.</param>
		/// <param name="rightSolver">Right solver.</param>
		/// <param name="logicOperator">Logic operator.</param>
		public ConditionCombo (Solver leftSolver, Solver rightSolver, ELogicOperators logicOperator)
		{
			this.leftSolver = leftSolver;
			this.rightSolver = rightSolver;
			this.logicOperator = logicOperator;
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new ConditionCombo without a rightsolver
		/// Use this ConditionCombo for Not statement
		/// </summary>
		/// <param name="leftSolver">Solver.</param>
		/// <param name="logicOperator">Logic operator.</param>
		public ConditionCombo (Solver leftSolver, ELogicOperators logicOperator)
		{
			this.leftSolver = leftSolver;
			this.logicOperator = logicOperator;
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Solve this instance.
		/// </summary>
		public override bool solve(Composite parent){

			bool leftValue = leftSolver.solve(parent);
			bool rightValue = (rightSolver != null) ? rightSolver.solve (parent) : false;

			switch (logicOperator) {
			case ELogicOperators.And:
				if (leftValue && rightValue) {
					return true;
				} 
				break;
			case ELogicOperators.Not:
				return !leftValue;
			case ELogicOperators.Or:
				if (leftValue || rightValue) {
					return true;
				}
				break;
			default:
				break;
			}
			return false;
		}
	}
}


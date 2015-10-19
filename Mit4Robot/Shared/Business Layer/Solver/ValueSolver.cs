using System;
using Shared.Enums;

namespace Shared.BusinessLayer
{
	public class ValueSolver: Solver
	{

		VariableSolver leftSolver;
		VariableSolver rightSolver;
		EComparisonOperator comparisonOperator;

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

		public EComparisonOperator ComparisonOperator {
			get {
				return comparisonOperator;
			}
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new ValueSolver.
		/// This solver checks for (i==5) and the like
		/// </summary>
		/// <param name="leftSolver">Left solver.</param>
		/// <param name="rightSolver">Right solver.</param>
		/// <param name="comparisonOperator">Comparison operator.</param>
		public ValueSolver (VariableSolver leftSolver, VariableSolver rightSolver, EComparisonOperator comparisonOperator)
		{
			this.leftSolver = leftSolver;
			this.rightSolver = rightSolver;
			this.comparisonOperator = comparisonOperator;
		}
			
		/// Author: Bert van Montfort
		/// <summary>
		/// Solve the specified parent.
		/// </summary>
		/// <param name="parent">Parent.</param>
		public override bool solve (Composite parent)
		{
			Variable leftVar = leftSolver.solve (parent);
			Variable rightVar = rightSolver.solve (parent);
			bool result;

			switch (comparisonOperator) {
			case EComparisonOperator.ValueEqualTo:
				if (leftVar.Value == rightVar.Value) {
					result = true;
				} else
					result = false;
				break;
			case EComparisonOperator.ValueGreaterThan:
				if (leftVar.Value > rightVar.Value) {
					result = true;
				} else
					result = false;
				break;
			case EComparisonOperator.ValueGreaterThanOrEqualTo:
				if (leftVar.Value >= rightVar.Value) {
					result = true;
				} else
					result = false;
				break;
			case EComparisonOperator.ValueLessThan:
				if (leftVar.Value < rightVar.Value) {
					result = true;
				} else
					result = false;
				break;
			case EComparisonOperator.ValueLessThanOrEqualTo:
				if (leftVar.Value <= rightVar.Value) {
					result = true;
				} else
					result = false;
				break;
			case EComparisonOperator.ValueNotEqualTo:
				if (leftVar.Value != rightVar.Value) {
					result = true;
				} else
					result = false;
				break;
			default:
				result = false;
				break;
			}
			return result;
		}
	}
}


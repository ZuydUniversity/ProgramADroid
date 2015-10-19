using System;

namespace Shared.BusinessLayer
{
	public class DefineVariable: VariableSolver
	{
		VariableSolver assignment;

		public VariableSolver Assignment {
			get {
				return assignment;
			}
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new 
		/// </summary>
		/// <param name="assignment">The VariableSolver to solve</param>
		public DefineVariable (VariableSolver assignment)
		{
			this.assignment = assignment;
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Tells the hold Variablesolver to Solve and returns it's value
		/// </summary>
		/// <param name="parent">Composite Parent.</param>
		public override Variable solve(Composite parent)
		{
			return assignment.solve (parent);
		}
	}
}


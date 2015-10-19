using System;
using Shared.Exceptions;
namespace Shared.BusinessLayer
{
	public class ConcreteVariable: VariableSolver
	{
		string variableName;
		Variable myVariable;
		public string VariableName {
			get {
				return variableName;
			}
		}

		public Variable MyVariable {
			get {
				return myVariable;
			}
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Should be used when referencing an existing value, like i
		/// </summary>
		/// <param name="variableName">Variable name.</param>
		public ConcreteVariable (string variableName)
		{
			this.variableName = variableName;
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Should be a literal value like True/False 0,1,2 or "some text"
		/// </summary>
		/// <param name="variable">Variable.</param>
		public ConcreteVariable (Variable variable){
			this.myVariable = variable;
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Solve the specified parent.
		/// </summary>
		/// <param name="parent">Composite Parent.</param>
		public override Variable solve(Composite parent)
		{
			// If the variable is defined, no lookup is required because this is a literal.
			if (myVariable != null) {
				return myVariable;
			} else {
				if (parent.variables.ContainsKey (variableName)) {
					return parent.variables [variableName];
				} else {
					throw new RunTimeException (String.Format ("Error At Line [{0}]: Use of undefined variable '{1}'", lineNumber, variableName));
				}
			}
		}
	}
}


using System;
using System.Collections.Generic;

namespace Shared.BusinessLayer
{
	public class CmdDefineVariable: Command
	{
		private string variableName;

		public string VariableName {
			get {
				return variableName;
			}
		}

		private DefineVariable varValue;

		public DefineVariable VarValue {
			get {
				return varValue;
			}
		}

		private Dictionary<string, Variable> variables;

		/// Author: Bert van Montfort
		/// <summary>
		/// Executes the CmdDefineVariable
		/// 
		/// Creates a new Variable using Definevariable varValue.
		/// If the variable already exists in the parent of this command the value will be overwritten
		/// If the varibale does not yet exist it will be added instead. 
		/// </summary>
		/// <param name="parent">The parent holing the Dictionary of variables</param>
		public override bool execute (Composite parent)
		{
			Variable collectedVar = varValue.solve (parent);
			variables = parent.variables;
			if (variables.ContainsKey (variableName)) {
				Variable oldVariable = variables [variableName];
				oldVariable.Value = collectedVar.Value;
			} else {
				variables.Add (variableName, collectedVar);
			}
			return true;
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new CmdDefineVariable
		/// </summary>
		/// <param name="variableName">Variable name.</param>
		/// <param name="varValue">Variable value.</param>
		public CmdDefineVariable (string variableName, DefineVariable varValue)
		{
			this.variableName = variableName;
			this.varValue = varValue;
		}			
	}
}


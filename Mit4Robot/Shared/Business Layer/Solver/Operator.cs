using System;
using Shared.Enums;

namespace Shared.BusinessLayer
{
	public class Operator : ICondition
	{
		private ELogicOperators logicOperator;

		public ELogicOperators LogicOperator {
			get {
				return logicOperator;
			}
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new logicOperator, it doesnt have any use on its own but it is used by CondintionCombo
		/// </summary>
		/// <param name="logicOperator">Logic operator.</param>
		public Operator (ELogicOperators logicOperator)
		{
			this.logicOperator = logicOperator;
		}
	}
}


using System;
using Shared.Enums;

namespace Shared.BusinessLayer
{
	public class AssignmentOperator: IAssignment
	{
		private int lineNumber;
		public int LineNumber {
			get {
				return this.lineNumber;
			}
			set {
				this.lineNumber = value;
			}
		}

		EAssignmentOperator assignmentOperator;

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new AssignmentOperator, for use by VariableCombo
		/// </summary>
		/// <param name="assignmentOperator">Assignment operator.</param>
		public AssignmentOperator (EAssignmentOperator assignmentOperator)
		{
			this.assignmentOperator = assignmentOperator;
		}
	}
}


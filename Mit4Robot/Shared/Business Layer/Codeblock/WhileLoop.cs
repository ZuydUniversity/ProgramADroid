using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace Shared.BusinessLayer
{
	public class WhileLoop : Composite
	{
		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new while loop.
		/// Once executed, the while loop will run until the conditions set are no longer true, or until the while loop has completed 300 loops,
		/// at which point it will assume the loop is infinite and the loop breaks.
		/// </summary>
		/// <param name="conditions">Conditions that have to remain true for the while loop to continue.</param>
		public WhileLoop (ConditionsClass conditions)
		{
			//this.conditions = conditions;
		}

		public WhileLoop (Solver conditions)
		{
			this.conditions = conditions;
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Executes the while loop until the conditions set are no longer true, or until the while loop has completed 300 loops,
		/// at which point it will assume the loop is infinite and the loop breaks.
		/// 
		/// Returns false if the while loop is infinite, true if not.
		/// </summary>
		public override bool execute (Composite parent)
		{
			base.execute (parent); //adds parents variables to own variables

			if (Robot.bg != null) {
				Robot.bg.ReportProgress (lineNumber);
			}
			int count = 0;
			while (conditions.solve(this)) {
				foreach (ICodeBlock codeBlock in children) {
					if (codeBlock.execute(this)) {
						Thread.Sleep (GlobalSupport.GameSpeed);
					}
				}
				if (count > 300) {
					return false;
				}
				count++;

				if (Robot.bg != null) {
					Robot.bg.ReportProgress (lineNumber);
					Thread.Sleep (GlobalSupport.GameSpeed/2);
				}
			}
			return true;
		}
	}
}


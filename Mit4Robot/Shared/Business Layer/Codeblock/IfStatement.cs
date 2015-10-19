using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Shared.CustomEventArgs;

namespace Shared.BusinessLayer
{
	public class IfStatement : Composite
	{
		List<ICodeBlock> elseChildren;

		public List<ICodeBlock> ElseChildren {
			get {
				return elseChildren;
			}
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new If statement.
		/// 
		/// The first child the if statement has will be executed if the set conditions are true
		/// The second child the if statement has will be executed of the set condtions are not true
		/// </summary>
		/// <param name="conditions">Conditions.</param>
		public IfStatement (Solver conditions)
		{
			this.conditions = conditions;
			elseChildren = new List<ICodeBlock> ();
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Executes the if statement
		/// 
		/// The first child the if statement has will be executed if the set conditions are true
		/// The second child the if statement has will be executed of the set condtions are not true
		/// </summary>
		public override bool execute (Composite parent)
		{
			return executeCommand (parent);
		}
		private bool executeCommand(Composite parent){
			base.execute (parent); //adds parents variables to own variables
			if (Robot.bg != null) {
				Robot.bg.ReportProgress (lineNumber);	
				Thread.Sleep (GlobalSupport.GameSpeed/2);
			}

			if (conditions.solve(parent)) {
					foreach (ICodeBlock block in children) {
						
						if (!block.execute(this)) {
							
						}
					Thread.Sleep (GlobalSupport.GameSpeed);
					}
			} else  {
				foreach (ICodeBlock block in elseChildren) {
					if (!block.execute(this)) {                   //return false when one of the children returns false
						return false;    
					}
					Thread.Sleep (GlobalSupport.GameSpeed);
				}
			} 
			return true;
		}
	}
}
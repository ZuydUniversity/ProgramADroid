using System;
using System.Threading.Tasks;
using System.Threading;

namespace Shared.BusinessLayer
{
	public class ForLoop: Composite
	{
		Solver solver;
		CmdDefineVariable declareVariable;
		CmdDefineVariable incrementCommand;

		public Solver Solver {
			get {
				return solver;
			}
		}

		public CmdDefineVariable DeclareVariable {
			get {
				return declareVariable;
			}
		}

		public CmdDefineVariable IncrementCommand {
			get {
				return incrementCommand;
			}
		}

		public ForLoop(Solver solver, int counter)
		{

		}

		public ForLoop (Solver solver, CmdDefineVariable declareVariable, CmdDefineVariable incrementCommand)
		{
			this.solver = solver;
			this.declareVariable = declareVariable;
			this.incrementCommand = incrementCommand;
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Executes the For loop
		/// The steps are:
		/// 1. the defineVariable is executed
		/// 2. the Solver is checked
		/// 3. if Solver.solve() is true all the for loop children will be executed, else the loop ends
		/// 4. incrementCommand is executed
		/// 5. back to step 2.
		/// 
		/// after 300 loops the loop will break and assume the loop is infinite. 
		/// </summary>
		/// <param name="parent">Parent of the Compisite you are executing</param>
		public override bool execute (Composite parent)
		{
			base.execute (parent);
			int count = 0;

			declareVariable.execute (this);
			while (solver.solve(this)) {
				foreach (ICodeBlock child in children) {
					child.execute (this);
					Thread.Sleep (GlobalSupport.GameSpeed);
				}
				incrementCommand.execute (this);
				count++;
				if (count >= 100) {
					return false;
				}
			}
			return true;
		}
	}
}


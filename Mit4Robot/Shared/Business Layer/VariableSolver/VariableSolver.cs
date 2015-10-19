using System;

namespace Shared.BusinessLayer
{
	public abstract class VariableSolver: IAssignment
	{
		protected int lineNumber;

		public int LineNumber {
			get {
				return this.lineNumber;
			}
			set {
				this.lineNumber = value;
			}
		}

		public VariableSolver ()
		{
		}

		public abstract Variable solve(Composite parent);
	}
}


using System;

namespace Shared.BusinessLayer
{
	public abstract class Command : ICodeBlock
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

		public Command ()
		{
		}

		abstract public bool execute(Composite parent);
	}
}


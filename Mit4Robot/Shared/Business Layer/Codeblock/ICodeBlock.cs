using System;

namespace Shared.BusinessLayer
{
	public interface ICodeBlock
	{
		bool execute(Composite parent); //return false when code failed to execute

		int LineNumber {
			get;
			set;
		}
	}
}


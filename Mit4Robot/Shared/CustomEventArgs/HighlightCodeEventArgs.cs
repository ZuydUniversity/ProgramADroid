using System;

namespace Shared.CustomEventArgs
{
	/// <summary>
	/// Highlight code event arguments.
	/// </summary>
	public class HighlightCodeEventArgs:EventArgs
	{
		public int lineNumber;
		public HighlightCodeEventArgs (int lineNumber)
		{
			this.lineNumber = lineNumber;
		}
	}
}


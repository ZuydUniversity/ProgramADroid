using System;

namespace Shared.Exceptions
{
	public class SyntaxParseException: Exception
	{
		public SyntaxParseException ()
		{
		}

		public SyntaxParseException(string message):base(message){
			
		}
	}
}


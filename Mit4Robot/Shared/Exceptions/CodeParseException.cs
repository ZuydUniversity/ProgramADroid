using System;

namespace Shared.Exceptions
{
	public class CodeParseException: Exception
	{
		public CodeParseException ()
		{
		}

		public CodeParseException(string message):base(message){
			
		}
	}
}


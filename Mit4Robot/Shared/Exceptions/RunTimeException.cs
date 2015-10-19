using System;

namespace Shared.Exceptions
{
	public class RunTimeException: Exception
	{
		public RunTimeException ()
		{
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a runtime Exception
		/// </summary>
		/// <param name="message">Message.</param>
		public RunTimeException(string message):base(message){

		}
	}
}


using System;

namespace Shared.Exceptions
{
	/// <summary>
	/// Map parse exception.
	/// </summary>
	public class MapParseException:Exception
	{
		public MapParseException ()
		{
		}

		/// Author:	Guy Spronck
		/// <summary>
		/// Initializes a new instance of the <see cref="Shared.Exceptions.MapParseException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		public MapParseException(string message):base(message){

		}
	}
}


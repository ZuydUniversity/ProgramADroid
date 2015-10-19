using System;

namespace Shared.Exceptions
{
	/// <summary>
	/// Exception for map
	/// </summary>
	public class MapException:Exception
	{
		public MapException ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the MapException/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <created>Stef Chappin</created>
		public MapException(string message):base(message){

		}
	}
}


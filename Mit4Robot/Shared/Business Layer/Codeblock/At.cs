using System;

namespace Shared
{
	public class At:Command
	{
		private string shopName;
		public At (string shopName)
		{
			this.shopName = shopName;
		}

		public override bool execute ()
		{
			//TODO Execute method At Robot class
			throw new NotImplementedException ();
		}
	}
}


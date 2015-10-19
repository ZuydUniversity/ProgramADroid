using System;

namespace Shared.Utilities
{
	public class Token
	{
		public Token (ETokenType type, string value, TokenPosition position)
		{
			Type = type;
			Value = value;
			Position = position;
		}

		public TokenPosition Position { get; set; }
		public ETokenType Type { get; set; }
		public string Value { get; set; }
	}
}


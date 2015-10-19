using System;
using System.Text.RegularExpressions;

namespace Shared.Utilities
{
	public class TokenDefinition
	{
		public TokenDefinition (ETokenType type, Regex regex):
		this(type, regex, false)
		{
		}

		public TokenDefinition(ETokenType type, Regex regex, bool isIgnored){
			Type = type;
			Regex = regex;
			IsIgnored = isIgnored;
		}

		public bool IsIgnored { get; private set; }
		public Regex Regex { get; private set; }
		public ETokenType Type { get; private set; }
	}
}


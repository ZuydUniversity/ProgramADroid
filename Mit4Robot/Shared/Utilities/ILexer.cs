using System;
using System.Collections.Generic;

namespace Shared.Utilities
{
	public interface ILexer
	{
		void AddDefinition(TokenDefinition tokenDefiniton);
		IEnumerable<Token> Tokenize(string source);
	}
}


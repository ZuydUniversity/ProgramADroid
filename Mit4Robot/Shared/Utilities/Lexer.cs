using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Shared.Exceptions;

namespace Shared.Utilities
{
	public class Lexer: ILexer
	{
		IList<TokenDefinition> tokenDefinitions = new List<TokenDefinition>();
		Regex endOfLineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);

		public Lexer ()
		{
		}

		#region ILexer implementation

		public void AddDefinition (TokenDefinition tokenDefiniton)
		{
			tokenDefinitions.Add (tokenDefiniton);
		}

		public IEnumerable<Token> Tokenize(string source)
		{
			int currentIndex = 0;
			int currentLine = 1;
			int currentColumn = 0;

			while (currentIndex < source.Length)
			{
				TokenDefinition matchedDefinition = null;
				int matchLength = 0;

				foreach (var rule in tokenDefinitions)
				{
					var match = rule.Regex.Match(source, currentIndex);

					if (match.Success && (match.Index - currentIndex) == 0)
					{
						matchedDefinition = rule;
						matchLength = match.Length;
						break;
					}
				}

				if (matchedDefinition == null)
				{

					var start = currentColumn;
					var spaceIndex = source.IndexOf (" ", start);
					if (spaceIndex == -1) {
						//there is no space following
						spaceIndex = source.Length;
					}
					string fault = source.Substring (start, spaceIndex - start).Trim ();
					throw new SyntaxParseException(string.Format("Unrecognized symbol '{0}' (line {1}, column {2}).", fault, currentLine, currentColumn));
				}
				else
				{
					var value = source.Substring(currentIndex, matchLength);

					if (!matchedDefinition.IsIgnored)
						yield return new Token(matchedDefinition.Type, value, new TokenPosition(currentIndex, currentLine, currentColumn));

					var endOfLineMatch = endOfLineRegex.Match(value);
					if (endOfLineMatch.Success)
					{
						currentLine += 1;
						currentColumn = value.Length - (endOfLineMatch.Index + endOfLineMatch.Length);
					}
					else
					{
						currentColumn += matchLength;
					}

					currentIndex += matchLength;
				}
			}

			yield return new Token(ETokenType.EOF, null, new TokenPosition(currentIndex, currentLine, currentColumn));
		}

		#endregion
	}
}


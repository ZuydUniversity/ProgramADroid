using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Shared.Utilities;
using Shared.BusinessLayer;
using Shared.Exceptions;
using Shared.Enums;
using System.Collections.ObjectModel;

namespace Shared.Parsers.CodeParsers
{
	/// Author: Max Hamulyak
	/// Date:	21-06-2015
	/// <summary>
	/// Initializes a new instance of the <see cref="Shared.Parsers.CodeParsers.PythonParser"/> class.
	/// </summary>
	public class PythonParser : CodeParser
	{
		/// Author: Max Hamulyak
		/// Date:	25-06-2015
		/// <summary>
		/// Initializes a new instance of the <see cref="Shared.Parsers.CodeParsers.PythonParser"/> class.
		/// Needed for setting protected values in the super class <see cref="Shared.Parsers.CodeParsers.CodeParser"/>
		/// </summary>
		public PythonParser(){
			ifStamentIndicatorValue = ":";
			ifStatementStartBlockIndicator = x => x.Type == ETokenType.startBlock && x.Value == ifStamentIndicatorValue;
			whileLoopStatementIndicatorValue = ":";
			whileLoopStatementStartBlockIndicator = x => x.Type == ETokenType.startBlock && x.Value == whileLoopStatementIndicatorValue;
			forLoopStatementIndicatorValue = ":";
			forLoopStatementStartBlockIndicator = x => x.Type == ETokenType.startBlock && x.Value == forLoopStatementIndicatorValue;
		}

		#region Constructor Methods

		/// Author: Max Hamulyak
		/// Date: 20-06-2015
		/// <summary>
		/// Builds a ReadOnlyCollection of Language keywords, that cannot be used in variables and such
		/// </summary>
		/// <returns>The language keywords.</returns>
		protected override ReadOnlyCollection<string> BuildLanguageKeywords ()
		{
			return new System.Collections.ObjectModel.ReadOnlyCollection<string> (new List<string> {
				"False",
				"None",
				"True",
				"and",
				"as",
				"assert",
				"break",
				"class",
				"continue",
				"def",
				"del",
				"elif",
				"else",
				"except",
				"finally",
				"for",
				"from",
				"global",
				"if",
				"import",
				"in",
				"is",
				"lambda",
				"nonlocal",
				"not",
				"or",
				"pass",
				"raise",
				"return",
				"try",
				"while",
				"width",
				"yield"
			});
		}
		private Regex forLoopRegex;

		/// Author: Max Hamulyak
		/// Date:	20-06-2015
		/// <summary>
		/// Build the lexer needed to parse a string, based on a programming language language rule.
		/// </summary>
		/// <returns>The language lexer.</returns>
		protected override ILexer BuildLanguageLexer ()
		{
			ILexer lexer = new Lexer ();


			Regex endOfLineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);
			lexer.AddDefinition(new TokenDefinition(ETokenType.EOL,endOfLineRegex));
			lexer.AddDefinition(new TokenDefinition(
				ETokenType.WhiteSpace,
				new Regex(@"[ \t]")));

			lexer.AddDefinition(new TokenDefinition(ETokenType.FUNCTIONDeclaration, new Regex(@"def[ \s](?<functionName>[a-z]+[A-Za-z0-9]*)\(\)", RegexOptions.Multiline)));
			lexer.AddDefinition (new TokenDefinition (ETokenType.Command,
				new Regex (@"(moveForward\(\)|rotateRight\(\)|pickUp\('([^)]+)\))")));

			lexer.AddDefinition(new TokenDefinition(ETokenType.FUNCTIONCall, new Regex(@"(?<functionName>[a-z]+[A-Za-z0-9]*)\(\)", RegexOptions.Multiline)));


			lexer.AddDefinition (new TokenDefinition(ETokenType.PythonRange, new Regex(@"range\(((\d+),(\d+))\)")));
			lexer.AddDefinition (new TokenDefinition (ETokenType.KeyWord, new Regex (@"(in)")));


			Regex commentRegex = new Regex (@"\#(.*)", RegexOptions.Multiline);

			lexer.AddDefinition(new TokenDefinition(ETokenType.CommentLine,commentRegex));



			lexer.AddDefinition (new TokenDefinition (ETokenType.Invalid, new Regex (@"(moveBackward\(\)|rotateLeft\(\))")));

			#region PythonBlockSyntax


			forLoopRegex = new Regex(@"for[ \t](?<loopIndex>[a-z]{1}[a-zA-Z]*)[ \t]*[ \t]*in[ \t]*range\((?<startingLow>[0-9]+),(?<endingHigh>[0-9]+)\)", RegexOptions.Multiline);
			lexer.AddDefinition(new TokenDefinition(ETokenType.FOR,forLoopRegex));


			lexer.AddDefinition(new TokenDefinition (ETokenType.WHILE,
				new Regex (@"\bwhile\b", RegexOptions.Multiline)));
			lexer.AddDefinition(new TokenDefinition (ETokenType.FOR,
				new Regex (@"\bfor\b", RegexOptions.Multiline)));
			lexer.AddDefinition(new TokenDefinition (ETokenType.IF,new Regex (@"\bif\b", RegexOptions.Multiline)));
			lexer.AddDefinition(new TokenDefinition (ETokenType.ELSEIF,
				new Regex (@"\belif\b", RegexOptions.Multiline)));
			lexer.AddDefinition(new TokenDefinition (ETokenType.ELSE,
				new Regex (@"\belse\b", RegexOptions.Multiline)));

		//	lexer.AddDefinition(new TokenDefinition(ETokenType.FUNCTIONDeclaration, new Regex(@"(?:def\s)[a-z]{3,10}\(\)", RegexOptions.Multiline)));
		//	lexer.AddDefinition(new TokenDefinition(ETokenType.FUNCTIONCall, new Regex(@"[a-z]{3,10}\(\)", RegexOptions.Compiled)));




			#endregion


			lexer.AddDefinition (new TokenDefinition (ETokenType.LogicalOperator,
				new Regex (@"(or|and|not)")));




			lexer.AddDefinition(new TokenDefinition (ETokenType.startBlock,
				new Regex (@"(:)")));
			lexer.AddDefinition (new TokenDefinition (ETokenType.logicInstruction,
				new Regex (@"(left|right|forward|backward|at\('([^)]+)\))")));






			lexer.AddDefinition (new TokenDefinition (ETokenType.leftParentheses, 
				new Regex (@"[\(]")));
			lexer.AddDefinition (new TokenDefinition (ETokenType.rightParentheses, 
			new Regex (@"[\)]")));
			lexer.AddDefinition (new TokenDefinition (ETokenType.Operator, new Regex (@"(\+|\-|\*|\/)")));
			lexer.AddDefinition (new TokenDefinition (ETokenType.ComparisonOperator, new Regex (@"(\<=|/>=|\<|\>|==|\!=)")));
			lexer.AddDefinition( new TokenDefinition (ETokenType.Literal, new Regex(@"(True|False)")));

			lexer.AddDefinition(new TokenDefinition(ETokenType.VARIABLE,new Regex(@"([A-Za-z]+)")));

			lexer.AddDefinition (new TokenDefinition (ETokenType.AssignmentOperator, new Regex ("(=)")));
			lexer.AddDefinition( new TokenDefinition (ETokenType.Literal, new Regex(@"(\d+)")));
			//"[^\"]*\"
			lexer.AddDefinition( new TokenDefinition (ETokenType.Literal, new Regex(@"\""[^\""]*\""")));

			lexer.AddDefinition (new TokenDefinition (ETokenType.KeyWord, 
				new Regex (@"\b(" + string.Join ("|", languageKeywords.Select (Regex.Escape).ToArray ()) + @"\b)", RegexOptions.Multiline)));
			

			return lexer;
		}

		#endregion
		#region Parser Abstract Methods Implementation

		/// <summary>
		/// Parses the else header.
		/// </summary>
		/// <returns><c>true</c>, if else header was parsed, <c>false</c> otherwise.</returns>
		/// <param name="headerDeclaration">Header declaration.</param>
		/// <param name="headerLineNumber">Header line number.</param>
		protected override bool ParseElseHeader (List<Token> headerDeclaration, int headerLineNumber)
		{
			Token startOfElseBlock = headerDeclaration.FirstOrDefault (x => x.Type == ETokenType.startBlock);
			if (startOfElseBlock == null) {
				if (headerDeclaration.Exists (x => x.Type == ETokenType.CommentLine)) {
					throw CommentInsideCondition (headerLineNumber, ETokenType.ELSE, ":");
				} else {
					throw IncompleteDeclarationOfCondition (headerLineNumber, ETokenType.ELSE, ":");
				}
			} else {
				int startOfBlockIndex = headerDeclaration.IndexOf (startOfElseBlock);
				List<Token> tokensTillRange = headerDeclaration.GetRange (1, startOfBlockIndex - 1);
				if (startOfBlockIndex + 2 < headerDeclaration.Count) {
					List<Token> tokensAfterRange = headerDeclaration.GetRange (startOfBlockIndex + 1, headerDeclaration.Count - startOfBlockIndex - 1);
					tokensAfterRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace || x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL);
					if (tokensAfterRange.Count > 0) {
						Token errorToken = tokensAfterRange.First ();
						if (errorToken.Type == ETokenType.VARIABLE || errorToken.Type == ETokenType.Literal || errorToken.Type == ETokenType.FUNCTIONCall) {
							throw ValueIndicatorAfterCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, errorToken.Type, errorToken.Value);
						} else {
							throw BlockIndicatorAfterCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, errorToken.Type);
						}
					}
				} 
				tokensTillRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace);
				if (tokensTillRange.Count > 0) {
					//ICondition condition = BuildICondition (tokensTillRange, headerLineNumber);
					//return condition;
					Token errorToken = tokensTillRange.First ();
					if (errorToken.Type == ETokenType.VARIABLE || errorToken.Type == ETokenType.Literal || errorToken.Type == ETokenType.FUNCTIONCall) {
						throw ValueIndicatorInsideCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, errorToken.Type, errorToken.Value);
					} else {
						throw BlockIndicatorInsideCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, errorToken.Type);
					}
				} else {
					return true;
				}
			}
		}

		/// Author: Max Hamulyak
		/// Date: 26-06-2015
		/// <summary>
		/// Parses for loop header, and finds start and ending value of the regex, in doing this it build a tuple that
		/// can be used in the creation of the forloop codeblock.
		/// </summary>
		/// <returns>The for loop header.</returns>
		/// <param name="headerDeclaration">Header declaration.</param>
		/// <param name="headerLineNumber">Header line number.</param>
		protected override Tuple<Solver, CmdDefineVariable, CmdDefineVariable> ParseForLoopHeader (List<Token> headerDeclaration, int headerLineNumber)
		{
			if (headerDeclaration.Count == 3 && headerDeclaration [0].Type == ETokenType.FOR && headerDeclaration [1].Type == ETokenType.startBlock && headerDeclaration[2].Type == ETokenType.EOL) {
				Token t = headerDeclaration.First ();
				List<String> result = new List<string> ();
				GroupCollection groups = forLoopRegex.Match (t.Value).Groups;
				if (groups.Count > 1) {
					foreach (var item in forLoopRegex.GetGroupNames()) {
						result.Add (string.Format ("Group: {0}, Value: {1}", item, groups [item].Value));
					}
					int startValue;
					int endValue;
					int.TryParse (groups ["startingLow"].Value, out startValue);
					int.TryParse (groups ["endingHigh"].Value, out endValue);

					string varName = groups ["loopIndex"].Value;
					VariableSolver a = new ConcreteVariable (varName);
					VariableSolver b = new ConcreteVariable (new Variable (1, EVariableType.Int));
					VariableSolver c = new ConcreteVariable (new Variable (endValue, EVariableType.Int));
					CmdDefineVariable defineLoopVar = new CmdDefineVariable (varName, new DefineVariable (new ConcreteVariable (new Variable (startValue, EVariableType.Int))));
					CmdDefineVariable incrementVar = new CmdDefineVariable (varName, new DefineVariable (new VariableCombo (a, b, EMathOperator.Add)));
					ValueSolver vs = new ValueSolver (a, c, EComparisonOperator.ValueLessThan);

					return new Tuple<Solver, CmdDefineVariable, CmdDefineVariable> (vs, defineLoopVar, incrementVar);
				} else {
					throw IncompleteDeclarationOfCondition (headerLineNumber, ETokenType.FOR, forLoopStatementIndicatorValue);
				}
			} else {
				Token startOfForBlock = headerDeclaration.FirstOrDefault (forLoopStatementStartBlockIndicator);
				if (startOfForBlock == null) {
					if (headerDeclaration.Exists (x => x.Type == ETokenType.CommentLine)) {
						throw CommentInsideCondition (headerLineNumber, ETokenType.FOR, ":");
					} else {
						throw IncompleteDeclarationOfCondition (headerLineNumber, ETokenType.FOR, ":");
					}
				} else {
					int startOfBlockIndex = headerDeclaration.IndexOf (startOfForBlock);
					//First token indicates this should be WHILE Loop, so it can be skipped
					List<Token> tokensTillRange = headerDeclaration.GetRange (1, startOfBlockIndex - 1);
					if (startOfBlockIndex + 2 < headerDeclaration.Count) {
						List<Token> tokensAfterRange = headerDeclaration.GetRange (startOfBlockIndex + 1, headerDeclaration.Count - startOfBlockIndex - 1);
						tokensAfterRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace || x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL);
						if (tokensAfterRange.Count > 0) {
							Token errorToken = tokensAfterRange.First ();
							if (errorToken.Type == ETokenType.VARIABLE || errorToken.Type == ETokenType.Literal || errorToken.Type == ETokenType.FUNCTIONCall) {
								throw ValueIndicatorAfterCondition (headerLineNumber, ETokenType.FOR, errorToken.Type, errorToken.Value);
							} else {
								throw BlockIndicatorAfterCondition (headerLineNumber, ETokenType.FOR, errorToken.Type);
							}
						}
					} 
					tokensTillRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace);
					if (tokensTillRange.Count > 0) {
						Token rangeToken = tokensTillRange.FirstOrDefault (x => x.Type == ETokenType.PythonRange);
						Token variableToken = tokensTillRange.FirstOrDefault (x => x.Type == ETokenType.VARIABLE);
						Token keyWordToken = tokensTillRange.FirstOrDefault (x => x.Type == ETokenType.KeyWord);
						tokensTillRange.Remove (rangeToken);
						tokensTillRange.Remove (variableToken);
						tokensTillRange.Remove (keyWordToken);

						if (tokensTillRange.Count > 0) {
							Token errorToken = tokensTillRange.First ();
							if (errorToken.Type == ETokenType.VARIABLE || errorToken.Type == ETokenType.Literal || errorToken.Type == ETokenType.FUNCTIONCall) {
								throw ValueIndicatorInsideCondition (headerLineNumber, ETokenType.FOR, errorToken.Type, errorToken.Value);
							} else {
								throw BlockIndicatorInsideCondition (headerLineNumber, ETokenType.FOR, errorToken.Type);
							}
						} else {
							string tokenValue = rangeToken.Value;
							int leftIndex = tokenValue.IndexOf ("(");
							tokenValue = tokenValue.Remove (0, leftIndex + 1).TrimEnd (')');
							string[] splitValues = tokenValue.Split (",".ToCharArray (), 2);
							int startIndex = -1;
							int maxIndex = -1;
							int.TryParse (splitValues [0], out startIndex);
							int.TryParse (splitValues [1], out maxIndex);
							string varName = variableToken.Value;
							VariableSolver a = new ConcreteVariable (varName);
							VariableSolver b = new ConcreteVariable (new Variable (1, EVariableType.Int));
							VariableSolver c = new ConcreteVariable (new Variable (maxIndex, EVariableType.Int));
							CmdDefineVariable defineLoopVar = new CmdDefineVariable (varName, new DefineVariable (new ConcreteVariable (new Variable (startIndex, EVariableType.Int))));
							CmdDefineVariable incrementVar = new CmdDefineVariable (varName, new DefineVariable (new VariableCombo (a, b, EMathOperator.Add)));
							ValueSolver vs = new ValueSolver (a, c, EComparisonOperator.ValueLessThan);

							return new Tuple<Solver, CmdDefineVariable, CmdDefineVariable> (vs, defineLoopVar, incrementVar);
						}
					} else {
						throw EmptyCondition (headerLineNumber, ETokenType.FOR, ":");
					}
				}
			}
		}
		/// Author: Max Hamulyak
		/// <summary>
		/// Parses the function header.
		/// </summary>
		/// <returns>The function header.</returns>
		/// <param name="headerDeclaration">Header declaration.</param>
		/// <param name="headerLineNumber">Header line number.</param>
		protected override string ParseFunctionHeader (List<Token> headerDeclaration, int headerLineNumber)
		{
			Token startOfFunctionBlock = headerDeclaration.FirstOrDefault (x => x.Type == ETokenType.startBlock);
			if (startOfFunctionBlock == null) {
				if (headerDeclaration.Exists (x => x.Type == ETokenType.CommentLine)) {
					throw CommentInsideCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, ":");
				} else {
					throw IncompleteDeclarationOfCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, ":");
				}
			} else {
				int startOfBlockIndex = headerDeclaration.IndexOf (startOfFunctionBlock);
				List<Token> tokensTillRange = headerDeclaration.GetRange (1, startOfBlockIndex - 1);
				if (startOfBlockIndex + 2 < headerDeclaration.Count) {
					List<Token> tokensAfterRange = headerDeclaration.GetRange (startOfBlockIndex + 1, headerDeclaration.Count - startOfBlockIndex - 1);
					tokensAfterRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace || x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL);
					if (tokensAfterRange.Count > 0) {
						Token errorToken = tokensAfterRange.First ();
						if (errorToken.Type == ETokenType.VARIABLE || errorToken.Type == ETokenType.Literal || errorToken.Type == ETokenType.FUNCTIONCall) {
							throw ValueIndicatorAfterCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, errorToken.Type, errorToken.Value);
						} else {
							throw BlockIndicatorAfterCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, errorToken.Type);
						}
					}
				} 
				tokensTillRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace);
				if (tokensTillRange.Count > 0) {
					Token errorToken = tokensTillRange.First ();
					if (errorToken.Type == ETokenType.VARIABLE || errorToken.Type == ETokenType.Literal || errorToken.Type == ETokenType.FUNCTIONCall) {
						throw ValueIndicatorInsideCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, errorToken.Type, errorToken.Value);
					} else {
						throw BlockIndicatorInsideCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, errorToken.Type);
					}
				} else {
					Token declarationToken = headerDeclaration.First ();
					string functionName = declarationToken.Value.TrimEnd ("()".ToCharArray ()).TrimStart ("def ".ToCharArray ()).Trim();
					return functionName;
				}
			}
		}
		/// Author: Max Hamulyak
		/// <summary>
		/// Parses the variable definition.
		/// </summary>
		/// <returns>The variable definition.</returns>
		/// <param name="headerDeclaration">Header declaration.</param>
		/// <param name="currentLine">Current line.</param>
		protected override VariableSolver ParseVariableDefinition(List<Token> headerDeclaration, int currentLine){
			string variableName = headerDeclaration.First ().Value;
			Token startOfDeclaration = headerDeclaration.FirstOrDefault (x => x.Type == ETokenType.AssignmentOperator);
			if (startOfDeclaration == null) {
				if (headerDeclaration.Exists (x => x.Type == ETokenType.CommentLine)) {
					throw CommentInsideVariableDeclaration (currentLine, "=", headerDeclaration.First ().Value, "#");
				} else {
					throw MissingAsignmentOperatorAfterDeclaration (currentLine, "=", headerDeclaration.First ().Value);
				}
			} else {
				int startOfDeclarationIndex = headerDeclaration.IndexOf (startOfDeclaration);
				List<Token> tokensTillRange = headerDeclaration.GetRange (1, startOfDeclarationIndex - 1);
				tokensTillRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace);
				if (tokensTillRange.Count > 0) {
					Token errorToken = tokensTillRange.First ();
					if (errorToken.Type == ETokenType.VARIABLE || errorToken.Type == ETokenType.Literal || errorToken.Type == ETokenType.FUNCTIONCall) {
						throw FoundValueBeforeAssignmentOperator (currentLine, "=", variableName, errorToken.Type, errorToken.Value);
					} else {
						throw FoundBlockBeforeAssignmentOperator (currentLine, "=", variableName, errorToken.Type);
					}
				} else {
					if (startOfDeclarationIndex + 2 < headerDeclaration.Count) {
						List<Token> tokensAfterRange = headerDeclaration.GetRange (startOfDeclarationIndex + 1, headerDeclaration.Count - startOfDeclarationIndex - 1);
						tokensAfterRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace || x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL);
						if (tokensAfterRange.Count > 0) {
							Token errorToken = tokensAfterRange.FirstOrDefault (x => x.Type != ETokenType.VARIABLE && x.Type != ETokenType.Literal && x.Type != ETokenType.Operator);
							if (errorToken != null) {
								throw InvalidTokenAfterAssignmentOperator (currentLine, "=", variableName, errorToken.Type);
							} else {
								return BuildIAssignment (tokensAfterRange, currentLine);
							}
						} else {
							throw EmptyAssignment (currentLine, "=", variableName);
						}
					} else {
						throw EmptyAssignment (currentLine, "=", variableName);
					}
				}
			}

		}
		#endregion
	
		/// Author: Max Hamulyak
		/// Date:	26-06-2015
		/// <summary>
		/// Reads an token marked as invalid and gives an exception.
		/// </summary>
		/// <param name="tokenToParse">Token to parse.</param>
		private void InvalidCommandFromToken(Token tokenToParse){
			if (tokenToParse.Value == "rotateLeft()") {
				throw InvalidCommand (tokenToParse.Position.Line, tokenToParse.Value, "rotateRight()");
			} else if (tokenToParse.Value == "moveBackward()") {
				throw InvalidCommand (tokenToParse.Position.Line, tokenToParse.Value, "moveForward()");
			}

		}

		/// Author: Max Hamulyak
		/// Date:	08-06-2015
		/// <summary>
		/// Takes a single <see cref="Token"/> and parses it to a robot instruction.
		/// Like <see cref="PickUp"/>, <see cref="Forward"/> and <see cref="TurnRight"/>
		/// that are all of the return type <see cref="Command"/>
		/// </summary>
		/// <returns>The <see cref="Command"/> from token.</returns>
		/// <param name="tokenToParse">Command token.</param>
		protected override Command SimpleCommandFromToken(List<Token> tokensToParse, int currentLine){


			Token tokenToParse = tokensToParse.First ();
			tokensToParse.Remove (tokenToParse);
			tokensToParse.RemoveAll (x => x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL || x.Type == ETokenType.WhiteSpace);
			if (tokensToParse.Count > 0) {
				throw TokenAfterFunctionCall (currentLine, tokenToParse.Value, tokensToParse.First ().Value);
			} else {
				Command robotCommand;
				if (tokenToParse.Value == "moveForward()") {
					robotCommand = new Forward ();	
				} else if (tokenToParse.Value == "rotateRight()") {
					robotCommand = new TurnRight ();
				} else {
					int quoteIndex = tokenToParse.Value.IndexOf ("'");
					String desiredObject = tokenToParse.Value.Substring (quoteIndex + 1);
					desiredObject = desiredObject.TrimEnd ("')".ToCharArray ());
					robotCommand = new PickUp (desiredObject);
				}
				robotCommand.LineNumber = tokenToParse.Position.Line;
				return robotCommand;
			}
		}

		/// Author: Max Hamulyak
		/// Date:	08-06-2015
		/// <summary>
		/// Build a List of <see cref="Token"/> from tokensToParse that are are part of a code block.
		/// These tokens are then seen as childeren of the parent parent block. 
		/// </summary>
		/// <returns>List of tokens</returns>
		/// <param name="tokensToParse">Tokens to parse.</param>
		/// <param name="currentLine">Current line.</param>
		protected override List<Token> BuildCodeBlock(List<Token> tokensToParse, int currentLine)
		{
			List<Token> blockTokens = new List<Token> ();

			bool endLoop = false;
			int peekIndex = currentLine + 1;
			do {

				List<Token> tokensAtNextLine = tokensToParse.Where(x => x.Position.Line == peekIndex).ToList();
				endLoop = EndOfCodeBlock(tokensAtNextLine,1);
				if (!endLoop){
					tokensAtNextLine.RemoveRange(0,1);
					blockTokens.AddRange(tokensAtNextLine);
					foreach (var item in blockTokens) {
						tokensToParse.Remove(item);
					}
					peekIndex++;
				}

			} while (!endLoop);

			return blockTokens;
		}

		/// Author: Max Hamulyak
		/// Date:	08-06-2015
		/// <summary>
		/// Build a List of <see cref="CodeBlock"/> from Tokens that are created by user input.
		/// This methods continuous aslong as the currentline is lower then the maximumline. At each line
		/// this method will check the tokens from the tokensToParse collection that are on the current line.
		/// If there is one ore more tokens at the line it will parse them to concrete commands. This can be a simple command
		/// like <see cref="TurnRight"/> or <see cref="Forward"/>. This means the token is a commandindicator. Otherwise it means there
		/// is a complex operator like <see cref="WhileLoop"/> or <see cref="IfStatement"/> these are handled in their seperate methods.
		/// </summary>
		/// <param name="tokensToParse">Tokens to parse.</param>
		/// <param name="currentLine">The currentline that needs to be parsed.</param>
		/// <param name="maximumLine">The maximumline number to read.</param>
		/// <returns>List of CodeBlock to be executed.</returns>
		protected override List<ICodeBlock> BuildCommandBlok(List<Token> tokensToParse, int currentLine, int maximumLine)
		{
			List<ICodeBlock> commandsToReturn = new List<ICodeBlock> ();
			while (currentLine < maximumLine) {

				List<Token> tokensAtCurrentLine = tokensToParse.Where (x => x.Position.Line == currentLine).ToList ();

				if (tokensAtCurrentLine.Count > 0) {

					Token firstTokenInLine = tokensAtCurrentLine.First ();

					if (firstTokenInLine.Type == ETokenType.Command) {

						commandsToReturn.Add (SimpleCommandFromToken (tokensAtCurrentLine, currentLine));


					} else if (firstTokenInLine.Type == ETokenType.Invalid) {
						InvalidCommandFromToken (firstTokenInLine);
					}
					else {
						if (firstTokenInLine.Type == ETokenType.VARIABLE) {
							commandsToReturn.Add (DefineVariableFromToken (tokensToParse, currentLine));
						} else if (firstTokenInLine.Type == ETokenType.WHILE) {
							commandsToReturn.Add (ParseWhile (tokensToParse, currentLine));
						} else if (firstTokenInLine.Type == ETokenType.FOR) {
							commandsToReturn.Add (ParseForloop (tokensToParse, currentLine));
						} else if (firstTokenInLine.Type == ETokenType.FUNCTIONCall) {
							commandsToReturn.Add (ParseFunctionCall (tokensAtCurrentLine));
						} else if (firstTokenInLine.Type == ETokenType.FUNCTIONDeclaration) {
							FunctionBlock functionBlock = ParseFunctionDeclaration (tokensToParse, currentLine);
							FunctionBlockList.addFunction (functionBlock.FunctionName, functionBlock);
						}
						else if (firstTokenInLine.Type == ETokenType.IF) {
							commandsToReturn.Add (ParseIfStatement (tokensToParse, currentLine));
						} else if (firstTokenInLine.Type == ETokenType.ELSEIF) {
							throw BeginIFBlockWithoutIF(currentLine,"IF-ELIF-ELSE","ELF");
						} else if (firstTokenInLine.Type == ETokenType.ELSE) {
							throw BeginIFBlockWithoutIF(currentLine,"IF-ELIF-ELSE","ELSE");
						} else if (firstTokenInLine.Type != ETokenType.WhiteSpace && firstTokenInLine.Type != ETokenType.EOL && firstTokenInLine.Type != ETokenType.CommentLine) {
							throw UnexpectedStatement (currentLine, firstTokenInLine.Value, firstTokenInLine.Type);
						}
					}
				}
				currentLine++;	
			}
			return commandsToReturn;
		}	

		/// Author: Max Hamulyak
		/// Date:	08-06-2015
		/// <summary>
		/// Ends the of code block.
		/// </summary>
		/// <returns><c>true</c>, if of code block was ended, <c>false</c> otherwise.</returns>
		/// <param name="tokens">Tokens to parse.</param>
		/// <param name="numberOfIdents">Number of idents.</param>
		private bool EndOfCodeBlock(List<Token> tokens, int numberOfIdents){
			bool endOfBlock = false;
			if (tokens.Count > 0) {
				while (numberOfIdents > 0) {
					Token t = tokens.ElementAt (0);
					if (t.Type != ETokenType.WhiteSpace) {
						endOfBlock = true;
						break;
					} else {
						numberOfIdents--;
					}
				}
				return endOfBlock;
			} else {
				return true;
			}
		}
				
	}
}


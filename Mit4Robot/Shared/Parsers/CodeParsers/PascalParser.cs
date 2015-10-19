using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Shared.Utilities;
using Shared.Enums;
using Shared.BusinessLayer;
using Shared.Exceptions;
using System.Collections.ObjectModel;

namespace Shared.Parsers.CodeParsers
{
	/// Author: Max Hamulyak
	/// Date:	21-06-2015
	/// <summary>
	/// Initializes a new instance of the <see cref="Shared.Parsers.CodeParsers.PascalParser"/> class.
	/// </summary>
	public class PascalParser : CodeParser
	{   
		private Dictionary<String, String> definedVariables = new Dictionary<string, string> ();

		/// Author: Max Hamulyak
		/// Date:	21-06-2015
		/// <summary>
		/// Initializes a new instance of the <see cref="Shared.Parsers.CodeParsers.PascalParser"/> class.
		/// </summary>
		public PascalParser(){
			incrementIndex = 1;
			ifStamentIndicatorValue = "then";
			ifStatementStartBlockIndicator = x => x.Type == ETokenType.KeyWord && x.Value == "then";

			whileLoopStatementIndicatorValue = "do";
			whileLoopStatementStartBlockIndicator = x => x.Type == ETokenType.KeyWord && x.Value == "do";

			forLoopStatementIndicatorValue = "do";
			forLoopStatementStartBlockIndicator = x => x.Type == ETokenType.KeyWord && x.Value == forLoopStatementIndicatorValue;
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
			return new System.Collections.ObjectModel.ReadOnlyCollection<string> (new List<string> { "and",
				"array",
				"begin",
				"case",
				"const",
				"div",
				"do",
				"downto",
				"else",
				"end",
				"file",
				"for",
				"function",
				"goto",
				"if",
				"in",
				"label",
				"mod",
				"nil",
				"not",
				"of",
				"or",
				"packed",
				"procedure",
				"program",
				"record",
				"repeat",
				"set",
				"then",
				"to",
				"type",
				"until",
				"var",
				"while",
				"with"
			});
		}

		private Regex forLoopRegex;
		private Regex varDeclarationRegex;

		/// Author: Max Hamulyak
		/// Date:	20-06-2015
		/// <summary>
		/// Build the lexer needed to parse a string, based on a programming language language rule.
		/// </summary>
		/// <returns>The language lexer.</returns>
		protected override ILexer BuildLanguageLexer ()
		{
			ILexer lexer = new Lexer ();


			forLoopRegex = new Regex (@"for[ \t](?<loopIndex>[a-z]{1}[a-zA-Z]+)[ \t]*\:\=[ \t]*(?<startingLow>[0-9])+[ \t](?<direction>to|downto)[ \t](?<endingHigh>[0-9])+[ \t]do", RegexOptions.Multiline);

			varDeclarationRegex = new Regex (@"^var[ \s](?<variableName>[A-Za-z]+)\:[ \t](?<variableType>integer|string|boolean);$", RegexOptions.Multiline);

			lexer.AddDefinition (new TokenDefinition (ETokenType.AssignmentOperator, new Regex ("(:=)")));


			lexer.AddDefinition(new TokenDefinition(ETokenType.startBlock,new Regex(";|:")));
			Regex commentRegex = new Regex (@"\{(.*)\}", RegexOptions.Multiline);
			lexer.AddDefinition(new TokenDefinition(ETokenType.CommentLine,commentRegex));

			lexer.AddDefinition(new TokenDefinition (ETokenType.ELSEIF,
				new Regex (@"(\belse[ \t]if\b)", RegexOptions.Multiline)));

			Regex endOfLineRegex = new Regex(@"\r\n|\r|\n", RegexOptions.Compiled);
			lexer.AddDefinition(new TokenDefinition(ETokenType.EOL,endOfLineRegex));
			lexer.AddDefinition(new TokenDefinition(
				ETokenType.WhiteSpace,
				new Regex(@"[ \t]")));

			lexer.AddDefinition (new TokenDefinition (ETokenType.Invalid, new Regex (@"([#{}])", RegexOptions.Multiline)));




			lexer.AddDefinition (new TokenDefinition (ETokenType.endBlock, new Regex (@"(\bend\b;|\bend\b.|\bend\b)", RegexOptions.Multiline)));

			lexer.AddDefinition(new TokenDefinition (ETokenType.WHILE,
				new Regex (@"\bwhile\b", RegexOptions.Multiline)));



			//lexer.AddDefinition(new TokenDefinition (ETokenType.FOR,
			//	new Regex (@"\bfor\b", RegexOptions.Multiline)));

			lexer.AddDefinition(new TokenDefinition(ETokenType.FOR,forLoopRegex));

			lexer.AddDefinition (new TokenDefinition (ETokenType.FOR, new Regex (@"\bfor\b", RegexOptions.Multiline)));

			lexer.AddDefinition(new TokenDefinition (ETokenType.IF,
				new Regex (@"\bif\b", RegexOptions.Multiline)));

			lexer.AddDefinition(new TokenDefinition (ETokenType.ELSE,
				new Regex (@"\belse\b", RegexOptions.Multiline)));

			lexer.AddDefinition (new TokenDefinition (ETokenType.VARIABLE, varDeclarationRegex));


			lexer.AddDefinition (new TokenDefinition (ETokenType.Command,
				new Regex (@"(moveForward\(\)|rotateRight\(\)|pickUp\('([^)]+)\))")));
			

			lexer.AddDefinition(new TokenDefinition(ETokenType.FUNCTIONDeclaration, new Regex(@"procedure[ \s](?<functionName>[a-z]+[A-Za-z0-9]*)\(\)", RegexOptions.Multiline)));
			lexer.AddDefinition(new TokenDefinition(ETokenType.FUNCTIONCall, new Regex(@"(?<functionName>[a-z]+[A-Za-z0-9]*)\(\)", RegexOptions.Multiline)));

			lexer.AddDefinition (new TokenDefinition (ETokenType.logicInstruction,
				new Regex (@"(left|right|forward|backward|at\('([^)]+)\))")));


			lexer.AddDefinition(new TokenDefinition (ETokenType.startBlock,
				new Regex (@"\bbegin\b", RegexOptions.Multiline)));


			lexer.AddDefinition (new TokenDefinition (ETokenType.LogicalOperator,
				new Regex (@"(or|and|not)")));

			lexer.AddDefinition (new TokenDefinition (ETokenType.KeyWord, 
				new Regex (@"\b(" + string.Join ("|", languageKeywords.Select (Regex.Escape).ToArray ()) + @"\b)", RegexOptions.Multiline)));

			lexer.AddDefinition (new TokenDefinition (ETokenType.leftParentheses, 
				new Regex (@"\(", RegexOptions.Multiline)));
			lexer.AddDefinition (new TokenDefinition (ETokenType.rightParentheses, 
				new Regex (@"[\)]")));


			lexer.AddDefinition (new TokenDefinition (ETokenType.Operator, new Regex (@"(\+|\-|\*|\/)")));
			lexer.AddDefinition (new TokenDefinition (ETokenType.ComparisonOperator, new Regex (@"(\<=|/>=|\<|\>|==|\!=)")));
			lexer.AddDefinition(new TokenDefinition(ETokenType.VARIABLE,new Regex(@"([A-Za-z]+)")));

			//lexer.AddDefinition(new TokenDefinition(ETokenType.VARIABLE,new Regex(@"(^[a-zA-Z]+$)", RegexOptions.Multiline)));


			lexer.AddDefinition( new TokenDefinition (ETokenType.Literal, new Regex(@"(\d+)")));
			lexer.AddDefinition( new TokenDefinition (ETokenType.Literal, new Regex(@"(True|False)")));
			//"[^\"]*\"
			lexer.AddDefinition( new TokenDefinition (ETokenType.Literal, new Regex(@"\""[^\""]*\""")));

			lexer.AddDefinition (new TokenDefinition (ETokenType.DataType, new Regex (@"(integer|string|boolean)")));



			return lexer;
		}

		#endregion
		#region Parser Abstract Methods Implementation

		/// Author: Max Hamulyak
		/// <summary>
		/// Parses the else header.
		/// </summary>
		/// <returns><c>true</c>, if else header was parsed, <c>false</c> otherwise.</returns>
		/// <param name="headerDeclaration">Header declaration.</param>
		/// <param name="headerLineNumber">Header line number.</param>
		protected override bool ParseElseHeader (List<Token> headerDeclaration, int headerLineNumber)
		{
			headerDeclaration.RemoveAll (x => x.Type == ETokenType.WhiteSpace || x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL);
			if (headerDeclaration.Count == 0) {
				return true;
			} else {
				return false;
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

			if (headerDeclaration.Count == 2 && headerDeclaration [0].Type == ETokenType.FOR && headerDeclaration [1].Type == ETokenType.EOL) {

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
						throw CommentInsideCondition (headerLineNumber, ETokenType.FOR, forLoopStatementIndicatorValue);
					} else {
						throw IncompleteDeclarationOfCondition (headerLineNumber, ETokenType.FOR, forLoopStatementIndicatorValue);
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
						Token variableToken = tokensTillRange.FirstOrDefault (x => x.Type == ETokenType.VARIABLE);
						Token assignmentToken = tokensTillRange.FirstOrDefault (x => x.Type == ETokenType.AssignmentOperator);
						if (assignmentToken == null) {
							throw MissingAsignmentOperatorAfterDeclaration (headerLineNumber, ":=", variableToken.Value);
						}
						Token literalToken = tokensTillRange.FirstOrDefault (x => x.Type == ETokenType.Literal);
						if (literalToken == null) {
							throw EmptyAssignment (headerLineNumber, ":=", variableToken.Value);
						}
						tokensTillRange.Remove (literalToken);
						Token keyWordTOToken = tokensTillRange.FirstOrDefault (x => x.Type == ETokenType.KeyWord && x.Value == "to");
						if (keyWordTOToken == null) {
							throw ExpectedKeyWordBetween (headerLineNumber, "to", literalToken.Value, "do");
						}
						Token literalToken2 = tokensTillRange.FirstOrDefault (x => x.Type == ETokenType.Literal);
						if (literalToken2 == null) {
							throw ExpectedAssignmentBetweenKeyword (headerLineNumber, "to", "do");
						}
						tokensTillRange.Remove (literalToken2);
						tokensTillRange.Remove (assignmentToken);
						tokensTillRange.Remove (variableToken);
						tokensTillRange.Remove (keyWordTOToken);
						tokensTillRange.Remove (startOfForBlock);
				
				
						if (tokensTillRange.Count > 0) {
							Token errorToken = tokensTillRange.First ();
							if (errorToken.Type == ETokenType.VARIABLE || errorToken.Type == ETokenType.Literal || errorToken.Type == ETokenType.FUNCTIONCall) {
								throw ValueIndicatorInsideCondition (headerLineNumber, ETokenType.FOR, errorToken.Type, errorToken.Value);
							} else {
								throw BlockIndicatorInsideCondition (headerLineNumber, ETokenType.FOR, errorToken.Type);
							}
						} else {
				
							int startValue;
							int endValue;
							int.TryParse (literalToken.Value, out startValue);
							int.TryParse (literalToken2.Value, out endValue);
				
							string varName = variableToken.Value;
							VariableSolver a = new ConcreteVariable (varName);
							VariableSolver b = new ConcreteVariable (new Variable (1, EVariableType.Int));
							VariableSolver c = new ConcreteVariable (new Variable (endValue, EVariableType.Int));
							CmdDefineVariable defineLoopVar = new CmdDefineVariable (varName, new DefineVariable (new ConcreteVariable (new Variable (startValue, EVariableType.Int))));
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
		protected override string ParseFunctionHeader (List<Token> headerDeclaration, int headerLineNumber)
		{
			Token startOfFunctionBlock = headerDeclaration.FirstOrDefault (x => x.Value == ";");
			if (startOfFunctionBlock == null) {
				if (headerDeclaration.Exists (x => x.Type == ETokenType.CommentLine)) {
					throw CommentInsideCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, ";");
				} else {
					throw IncompleteDeclarationOfCondition (headerLineNumber, ETokenType.FUNCTIONDeclaration, ";");
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
					string functionName = declarationToken.Value.TrimEnd ("()".ToCharArray ()).TrimStart ("procedure ".ToCharArray ()).Trim();
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
		protected override VariableSolver ParseVariableDefinition (List<Token> headerDeclaration, int currentLine)
		{
				string variableName = headerDeclaration.First ().Value;
			if (definedVariables.ContainsKey (variableName)) {
				Token startOfDeclaration = headerDeclaration.FirstOrDefault (x => x.Type == ETokenType.AssignmentOperator);
				if (startOfDeclaration == null) {
					if (headerDeclaration.Exists (x => x.Type == ETokenType.CommentLine)) {
						throw CommentInsideVariableDeclaration (currentLine, ":=", headerDeclaration.First ().Value, "#");
					} else {
						throw MissingAsignmentOperatorAfterDeclaration (currentLine, ":=", headerDeclaration.First ().Value);
					}
				} else {
					int startOfDeclarationIndex = headerDeclaration.IndexOf (startOfDeclaration);
					List<Token> tokensTillRange = headerDeclaration.GetRange (1, startOfDeclarationIndex - 1);
					tokensTillRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace);
					if (tokensTillRange.Count > 0) {
						Token errorToken = tokensTillRange.First ();
						if (errorToken.Type == ETokenType.VARIABLE || errorToken.Type == ETokenType.Literal || errorToken.Type == ETokenType.FUNCTIONCall) {
							throw FoundValueBeforeAssignmentOperator (currentLine, ":=", variableName, errorToken.Type, errorToken.Value);
						} else {
							throw FoundBlockBeforeAssignmentOperator (currentLine, ":=", variableName, errorToken.Type);
						}
					} else {
						if (startOfDeclarationIndex + 2 < headerDeclaration.Count) {
							List<Token> tokensAfterRange = headerDeclaration.GetRange (startOfDeclarationIndex + 1, headerDeclaration.Count - startOfDeclarationIndex - 1);
							tokensAfterRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace || x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL);
							if (tokensAfterRange.Count > 0) {
								Token errorToken = tokensAfterRange.FirstOrDefault (x => x.Type != ETokenType.VARIABLE && x.Type != ETokenType.Literal && x.Type != ETokenType.Operator);
								if (errorToken != null) {
									throw InvalidTokenAfterAssignmentOperator (currentLine, ":=", variableName, errorToken.Type);
								} else {
									return BuildIAssignment (tokensAfterRange, currentLine);
								}
							} else {
								throw EmptyAssignment (currentLine, ":=", variableName);
							}
						} else {
							throw EmptyAssignment (currentLine, ":=", variableName);
						}
					}
				} 
			} else {
				throw VariableAssignmentBeforeDeclaration (currentLine, variableName);
			}
		}

		#endregion








		protected override List<ICodeBlock> BuildCommandBlok(List<Token> tokensToParse, int currentLine, int maximumLine)
		{
			List<ICodeBlock> commandsToReturn = new List<ICodeBlock> ();
			while (currentLine < maximumLine) {

				List<Token> tokensAtCurrentLine = tokensToParse.Where (x => x.Position.Line == currentLine).ToList ();

				if (tokensAtCurrentLine.Count > 0) {

					Token firstTokenInLine = tokensAtCurrentLine.First ();

					if (firstTokenInLine.Type == ETokenType.Command) {

						commandsToReturn.Add (SimpleCommandFromToken (tokensAtCurrentLine,currentLine));


					} else {
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
							throw BeginIFBlockWithoutIF(currentLine,"IF-ELSE IF-ELSE","ELSE IF");
						} else if (firstTokenInLine.Type == ETokenType.ELSE) {
							throw BeginIFBlockWithoutIF(currentLine,"IF-ELSE IF-ELSE","ELSE");	
						} else if (firstTokenInLine.Type != ETokenType.WhiteSpace && firstTokenInLine.Type != ETokenType.EOL && firstTokenInLine.Type != ETokenType.CommentLine) {
							throw UnexpectedStatement (currentLine, firstTokenInLine.Value, firstTokenInLine.Type);
						}
					}
				}
				currentLine++;	
			}
			return commandsToReturn;
		}


		protected override IfStatement ParseIfStatement (List<Token> tokensToParse, int currentLine){
			List<Token> tokensAtCurrentLine = tokensToParse.Where (x => x.Position.Line == currentLine).ToList();
			tokensToParse.RemoveAll (x => x.Position.Line == currentLine);
			ICondition c = ParseIfStatementHeader (tokensAtCurrentLine, currentLine);//BuildICondition (tokensAtCurrentLine.Where (x => x.Type != ETokenType.WhiteSpace && x.Type != ETokenType.startBlock && x.Type != ETokenType.EOF && x.Type != ETokenType.EOL && x.Type != ETokenType.WHILE && x.Type != ETokenType.IF && x.Type != ETokenType.ELSEIF && x.Type != ETokenType.ELSE).ToList (),currentLine);
			IfStatement ifStatement = new IfStatement (c as Solver);
			List<Token> childTokens = BuildCodeBlock (tokensToParse, currentLine);
			int last;
			Token lastToken;
			if (childTokens.Count > 0) {
				last = childTokens.Last ().Position.Line + incrementIndex;
				lastToken = tokensToParse.First (x => x.Position.Line == last);
				RemoveTokensAtLine (tokensToParse, last);
				List<ICodeBlock> childeren = BuildCommandBlok (childTokens, currentLine + 1, last + 1);
				if (childeren.Count > 0) {
					foreach (var item in childeren) {
						ifStatement.addChild (item);
					}
				} else {
					throw EmptyCodeBlock (currentLine, ETokenType.IF);
				}
			} else {
				throw EmptyCodeBlock (currentLine, ETokenType.IF);
			}
			if (tokensToParse.Count > 0) {
				Token firstTokenNextLine = tokensToParse.FirstOrDefault(x => x.Position.Line == last + 1);
				if (firstTokenNextLine != null) {
					if (firstTokenNextLine.Type == ETokenType.ELSEIF) {
						EndOfBlockTokenCorrect (lastToken, "end", ETokenType.IF);
						IfStatement elif = ParseIfStatement (tokensToParse, last + 1);
						ifStatement.ElseChildren.Add (elif);
					}
					else if (firstTokenNextLine.Type == ETokenType.ELSE) {
						EndOfBlockTokenCorrect (lastToken, "end", ETokenType.IF);
						ifStatement.ElseChildren.AddRange (ParseElse (tokensToParse, last + 1));
					}
				}
			}
			ifStatement.LineNumber = currentLine;
			return ifStatement;
		}


		protected override void EndOfBlockTokenCorrect (Token endToken, string expectedEnd, ETokenType blockType)
		{
			if (endToken.Type == ETokenType.endBlock) {
				if (endToken.Value != expectedEnd) {
					throw IncorrectEndingOfBlock (endToken.Position.Line, expectedEnd, endToken.Value, blockType.ToString());
				}
			}
		}

		protected override CmdDefineVariable DefineVariableFromToken(List<Token> instructionLine, int currentLine)
		{
			
			List<Token> tokensAtCurrentLine = instructionLine.Where (x => x.Position.Line == currentLine).ToList ();
			instructionLine.RemoveAll (x => x.Position.Line == currentLine);
			if (tokensAtCurrentLine.Count == 2 && tokensAtCurrentLine [0].Type == ETokenType.VARIABLE && tokensAtCurrentLine [1].Type == ETokenType.EOL) {
				Token t = tokensAtCurrentLine.First ();
				List<String> result = new List<string> ();
				GroupCollection groups = varDeclarationRegex.Match (t.Value).Groups;
				if (groups.Count == 3) {
					foreach (var item in varDeclarationRegex.GetGroupNames()) {
						result.Add (string.Format ("Group: {0}, Value: {1}", item, groups [item].Value));
					}

					string variableName = groups ["variableName"].Value;
					string variableType = groups ["variableType"].Value;


					if (languageKeywords.Contains (variableName)) {
						throw KeyWordAsVariable (currentLine, variableName, EGameLanguage.Python);
					} 
					if (!definedVariables.ContainsKey (variableName)) {
						definedVariables.Add (variableName, variableType);
					} else {
						throw VariableNameHasAlreadyBeenDeclared (currentLine, variableName);
					}
					List<IAssignment> assignments = new List<IAssignment> ();
					assignments.Add (new ConcreteVariable (new Variable (0, EVariableType.Int)));
					VariableSolver solver = BuildDefineVariable (assignments, EMathOperator.None);
					DefineVariable myVar = new DefineVariable (solver);
					CmdDefineVariable myVariable = new CmdDefineVariable (variableName, myVar);
					return myVariable;
				} else {
					return base.DefineVariableFromToken (tokensAtCurrentLine, currentLine);
				}
			} else {
				return base.DefineVariableFromToken (tokensAtCurrentLine, currentLine);
			}
		}

		protected override List<Token> BuildCodeBlock(List<Token> tokensToParse, int currentLine)
		{
			List<Token> blockTokens = new List<Token> ();

			bool endLoop = false;
			int peekIndex = currentLine + 1;
			List<Token> tokensAtNextLine = tokensToParse.Where(x => x.Position.Line == peekIndex).ToList();
			if (tokensAtNextLine.Count > 0) {
				Token firstToken = tokensAtNextLine.First ();
				if (firstToken.Type == ETokenType.startBlock) {
					peekIndex = peekIndex + 1;
					tokensToParse.Remove (firstToken);
					do {
						tokensAtNextLine = tokensToParse.Where(x => x.Position.Line == peekIndex).ToList();
						endLoop = EndOfCodeBlock (tokensAtNextLine, 1);
						if (!endLoop) {
							tokensAtNextLine.RemoveAll(x => x.Type == ETokenType.WhiteSpace);
							blockTokens.AddRange (tokensAtNextLine);
							foreach (var item in tokensAtNextLine) {
								tokensToParse.Remove (item);
							}
							peekIndex++;
						} else {
							foreach (Token token in tokensAtNextLine){
								//tokensToParse.Remove(token);
							}
						}

					} while (!endLoop);

					return blockTokens;
				} else {
					throw MissingCodeBlockDeclaration (currentLine);
				}
			} else {
				return blockTokens;
			}
		}

		private bool EndOfCodeBlock(List<Token> tokens, int numberOfIdents){
			bool endOfBlock = false;
			if (tokens.Count > 0) {
				while (numberOfIdents > 0) {
					Token t = tokens.ElementAt (0);
					if (t.Type == ETokenType.endBlock) {
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

		protected override Command SimpleCommandFromToken (List<Token> tokensToParse, int currentLine)
		{

			Token tokenToParse = tokensToParse.First ();
			tokensToParse.Remove (tokenToParse);
			tokensToParse.RemoveAll (x => x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL || x.Type == ETokenType.WhiteSpace);
			Token semicolonToken = tokensToParse.FirstOrDefault (x => x.Value == ";");
			if (semicolonToken == null) {
				throw MissingSemicolonException (tokenToParse.Position.Line, tokenToParse.Value);
			}
			tokensToParse.Remove (semicolonToken);
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

		protected override FunctionBlockExecute ParseFunctionCall (List<Token> tokensAtCurrentLine)
		{
			Token tokenToParse = tokensAtCurrentLine.First ();
			int headerLineNumber = tokenToParse.Position.Line;
			tokensAtCurrentLine.RemoveAll (x => x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL || x.Type == ETokenType.WhiteSpace);
			Token semicolonToken = tokensAtCurrentLine.FirstOrDefault (x => x.Value == ";");
			if (semicolonToken == null) {
				throw MissingSemicolonException (tokenToParse.Position.Line, tokenToParse.Value);
			} else {
				int myIndex = tokensAtCurrentLine.IndexOf (semicolonToken);
				List<Token> tokensTillRange = tokensAtCurrentLine.GetRange (1, myIndex - 1);
				List<Token> tokensAfterRange = tokensAtCurrentLine.GetRange (myIndex + 1, tokensAtCurrentLine.Count - myIndex - 1);
				Token errorToken = null;
				if (tokensTillRange.Count != 0) {
					errorToken = tokensTillRange.First ();
					throw ValueBeforeSemicolon (headerLineNumber,tokenToParse, errorToken);
				} else if (tokensAfterRange.Count != 0) {
					errorToken = tokensAfterRange.First ();
					throw ValueAfterSemicolon (headerLineNumber,tokenToParse, errorToken);
				}
				tokensAtCurrentLine.Remove (tokenToParse);
				string functionName = tokenToParse.Value.TrimEnd ("()".ToCharArray ()).Trim ();
				FunctionBlockExecute executeFunction = new FunctionBlockExecute (functionName);
				executeFunction.LineNumber = tokenToParse.Position.Line;
				return executeFunction;
			}
		}

	}
}


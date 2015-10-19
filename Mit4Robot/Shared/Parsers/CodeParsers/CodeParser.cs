using System;
using System.Linq;
using System.Collections.Generic;
using Shared.Utilities;
using Shared.BusinessLayer;
using Shared.Enums;
using Shared.Exceptions;
using System.Collections.ObjectModel;

namespace Shared.Parsers.CodeParsers
{
	/// Author: Max Hamulyak
	/// Date:	24-06-2015
	/// <summary>
	/// The abstract code parser has functionality that can be used through 
	/// concrete ProgrammingLanguage Parsers, much of the functionality is shared,
	/// this is because every <see cref="ICodeBlock"/> is build the sameway accross different
	/// languages, the only difference is how a WHILE,FOR,IF OR FUNCTION block is declared, and how
	/// a codeblock determines his childeren. For this reason most methods are abstract, and the header
	/// declarations are Virtual, so the concrete parser has the possibility of overiding functionality.
	/// </summary>
	public abstract class CodeParser
	{
		#region Constant Fields
		const string errorAtLine = "Error At Line [{0}]: ";
		const string invalidSyntax = "Invalid Syntax, ";
		#endregion
		#region Protected Fields
		protected Func<Token,bool> ifStatementStartBlockIndicator;
		protected string ifStamentIndicatorValue;
		protected Func<Token,bool> whileLoopStatementStartBlockIndicator;
		protected string whileLoopStatementIndicatorValue;
		protected string forLoopStatementIndicatorValue;
		protected Func<Token,bool> forLoopStatementStartBlockIndicator;
		protected ILexer languageLexer;
		protected ReadOnlyCollection<string> languageKeywords;
		protected int incrementIndex = 0;
		#endregion
		#region Constructor Methods
		public CodeParser ()
		{
			languageKeywords = BuildLanguageKeywords ();
			languageLexer = BuildLanguageLexer ();
		}
		/// Author: Max Hamulyak
		/// Date:	18-06-2015
		/// <summary>
		/// Builds a ReadOnlyCollection of Language keywords, that cannot be used in variables and such
		/// </summary>
		/// <returns>The language keywords.</returns>
		protected abstract ReadOnlyCollection<string> BuildLanguageKeywords ();
		/// Author: Max Hamulyak
		/// Date:	18-06-2015	
		/// <summary>
		/// Build the lexer needed to parse a string, based on a programming language language rule.
		/// </summary>
		/// <returns>The language lexer.</returns>
		protected abstract ILexer BuildLanguageLexer ();
		#endregion
		#region Parser Abstract Methods
		protected abstract Command SimpleCommandFromToken (List<Token> tokensToParse, int currentLine);
		protected abstract List<Token> BuildCodeBlock (List<Token> tokensToParse, int currentLine);
		protected abstract List<ICodeBlock> BuildCommandBlok (List<Token> tokensToParse, int currentLine, int maximumLine);
		protected abstract bool ParseElseHeader (List<Token> headerDeclaration, int headerLineNumber);
		protected abstract Tuple<Solver, CmdDefineVariable,CmdDefineVariable> ParseForLoopHeader(List<Token> headerDeclaration, int headerLineNumber);
		protected abstract string ParseFunctionHeader (List<Token> headerDeclaration, int headerLineNumber);
		protected abstract VariableSolver ParseVariableDefinition (List<Token> headerDeclaration, int currentLine);
		#endregion
		#region Virtual Parser Methods

		/// Author: Max Hamulyak
		/// Date:	30-06-2015
		/// <summary>
		/// Builds the solver needed for an define variable statement, using a list of conditions.
		/// based on a set of rules it can be checked how the list should be parsed, recursivly. 
		/// </summary>
		/// <returns>The define variable.</returns>
		/// <param name="conditions">Conditions.</param>
		/// <param name="priorityOperator">Priority operator.</param>
		protected virtual VariableSolver BuildDefineVariable(List<IAssignment> conditions, EMathOperator priorityOperator){
			int index = 0;
			bool stopLoop = false;

			while (index < conditions.Count && !stopLoop) {

				if (conditions.Count > 3) {
					int loopIndex = 0;
					while (loopIndex < conditions.Count) {
						IAssignment assignment = conditions.ElementAt (loopIndex);
						if (assignment is MathOperator) {

							if ((assignment as MathOperator).MathOperator2 == priorityOperator) {
								int itemIndex = conditions.IndexOf (assignment); 
								IAssignment left = conditions.ElementAt (itemIndex - 1);
								IAssignment right = conditions.ElementAt (itemIndex + 1);
								conditions.Remove (left);
								conditions.Remove (right);
								conditions.Remove (assignment);
								if ((left is VariableSolver) && (right is VariableSolver)) {
									VariableCombo combo = new VariableCombo ((left as VariableSolver), (right as VariableSolver), priorityOperator);
									conditions.Insert (itemIndex - 1, combo);
								} else {
									throw InvalidAssignment (left.LineNumber);
								}
							}
						}
						loopIndex++;
					}
					BuildDefineVariable (conditions, priorityOperator + 1);
				} else {
					stopLoop = true;
				}
			}
			IAssignment returnAssignment = null;
			if (conditions.Count == 1) {
				//Should be concrete
				returnAssignment = conditions.ElementAt(0);

			} else if (conditions.Count == 2) {
				throw InvalidAssignmentTwoAssignmentValues (conditions);
			} else if (conditions.Count == 3) {
				if (conditions.ElementAt (1) is MathOperator) {
					if (conditions.ElementAt (0) is VariableSolver && conditions.ElementAt (2) is VariableSolver) {

						returnAssignment = new VariableCombo ((conditions.ElementAt (0) as VariableSolver), (conditions.ElementAt (2) as VariableSolver), (conditions.ElementAt (1) as MathOperator).MathOperator2);
					} else {
						throw InvalidAssignment (conditions.ElementAt (1).LineNumber);
					}
				} else {
					throw InvalidAssignmentMathOperator(conditions.ElementAt(1).LineNumber);
				}
			} else {
				throw InvalidAssignment (conditions.ElementAt (0).LineNumber);
			}

			return returnAssignment as VariableSolver;
		}

		/// Author: Max Hamulyak
		/// Date:	30-06-2015
		/// <summary>
		/// Removes the tokens at line.
		/// </summary>
		/// <param name="tokensToParse">Tokens to parse.</param>
		/// <param name="currentLine">Current line.</param>
		protected void RemoveTokensAtLine(List<Token> tokensToParse, int currentLine){
			tokensToParse.RemoveAll (x => x.Position.Line == currentLine);
		}

		/// Author: Max Hamulyak
		/// Date:	19-06-2015
		/// <summary>
		/// Given a list of <see cref="Token"/> builds an ifstatement code block, based on the tokens at the next lines,
		/// other then the other programming block (while,forloop etc) an ifstatement can be followed by an else if or an else.
		/// These follow-ups are handled in the parsing of the if block, en thus it can be garanteed that an else if or an else
		/// found anywhere else is invalid, because the if statement is only handled in here, and only by an preceding if is an else if or
		/// an else valid.
		/// </summary>
		/// <returns>The if statement code block</returns>
		/// <param name="tokensToParse">Tokens to parse.</param>
		/// <param name="currentLine">Current line.</param>
		protected virtual IfStatement ParseIfStatement (List<Token> tokensToParse, int currentLine){
			List<Token> tokensAtCurrentLine = tokensToParse.Where (x => x.Position.Line == currentLine).ToList();
			tokensToParse.RemoveAll (x => x.Position.Line == currentLine);
			ICondition c = ParseIfStatementHeader (tokensAtCurrentLine, currentLine);
			IfStatement ifStatement = new IfStatement (c as Solver);
			List<Token> childTokens = BuildCodeBlock (tokensToParse, currentLine);
			int last;
			if (childTokens.Count > 0) {
				last = childTokens.Last ().Position.Line + incrementIndex;
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
						IfStatement elif = ParseIfStatement (tokensToParse, last + 1);
						ifStatement.ElseChildren.Add (elif);
					}
					else if (firstTokenNextLine.Type == ETokenType.ELSE) {
						ifStatement.ElseChildren.AddRange (ParseElse (tokensToParse, last + 1));
					}
				}
			}
			ifStatement.LineNumber = currentLine;
			return ifStatement;
		}

		/// Author: Max Hamulyak
		/// Date:	19-06-2015
		/// <summary>
		/// Parses the else function block, based on its childeren en returns an list of <see cref="ICodeBlock"/>
		/// so it can be used in the else childeren of the if statement.
		/// </summary>
		/// <returns>The else.</returns>
		/// <param name="tokensToParse">Tokens to parse.</param>
		/// <param name="currentLine">Current line.</param>
		protected virtual List<ICodeBlock> ParseElse (List<Token> tokensToParse, int currentLine){
			try{
				List<Token> tokensAtCurrentLine = tokensToParse.Where (x => x.Position.Line == currentLine).ToList();
				tokensToParse.RemoveAll (x => x.Position.Line == currentLine);
				ParseElseHeader(tokensAtCurrentLine,currentLine);
				List<ICodeBlock> result = new List<ICodeBlock> ();
				List<Token> childTokens = BuildCodeBlock (tokensToParse, currentLine);
				if (childTokens.Count > 0) {
					int last = childTokens.Last ().Position.Line;
					RemoveTokensAtLine (tokensToParse, last + incrementIndex);
					List<ICodeBlock> childeren = BuildCommandBlok (childTokens, currentLine + 1, last + 1);
					if (childeren.Count > 0) {
						result.AddRange (childeren);
						return result;
					} else {
						throw EmptyCodeBlock (currentLine, ETokenType.ELSE);
					}
				} else {
					throw EmptyCodeBlock (currentLine, ETokenType.ELSE);
				}

			} catch (CodeParseException ex){
				throw ex;
			}
		}

		/// Author: Max Hamulyak
		/// Date:	08-06-2015
		/// <summary>
		/// Function is used to parse a WhileLoop, can be overriden in base class.
		/// </summary>
		/// <returns>The while.</returns>
		/// <param name="tokensToParse">Tokens to parse.</param>
		/// <param name="currentLine">Current line.</param>
		protected virtual WhileLoop ParseWhile (List<Token> tokensToParse, int currentLine){
			List<Token> tokensAtCurrentLine = tokensToParse.Where (x => x.Position.Line == currentLine).ToList();
			tokensToParse.RemoveAll (x => x.Position.Line == currentLine);
			ICondition c = ParseWhileLoopHeader (tokensAtCurrentLine, currentLine);
			WhileLoop whileLoop = new WhileLoop (c as Solver);
			List<Token> childTokens = BuildCodeBlock (tokensToParse, currentLine);
			if (childTokens.Count > 0) {
				int last = childTokens.Last ().Position.Line;
				Token lastToken = tokensToParse.First (x => x.Position.Line == last + incrementIndex);
				EndOfBlockTokenCorrect (lastToken, "end;", ETokenType.WHILE);
				RemoveTokensAtLine (tokensToParse, last + incrementIndex);
				List<ICodeBlock> childeren = BuildCommandBlok (childTokens, currentLine + 1, last + 1);
				if (childeren.Count > 0) {
					foreach (var item in childeren) {
						whileLoop.addChild (item);
					}
					whileLoop.LineNumber = currentLine;
					return whileLoop;
				} else {
					throw EmptyCodeBlock (currentLine, ETokenType.WHILE);
				}
			} else {
				throw EmptyCodeBlock (currentLine, ETokenType.WHILE);
			}
		}

		/// Author: Max Hamulyak
		/// Date:	19-06-2015
		/// <summary>
		/// Parses the forloop based on all its childeren.
		/// </summary>
		/// <returns>The forloop.</returns>
		/// <param name="tokensToParse">Tokens to parse.</param>
		/// <param name="currentLine">Current line.</param>
		protected virtual ForLoop ParseForloop (List<Token> tokensToParse, int currentLine){
			List<Token> tokensAtCurrentLine = tokensToParse.Where (x => x.Position.Line == currentLine).ToList ();
			tokensToParse.RemoveAll (x => x.Position.Line == currentLine);

			Tuple<Solver, CmdDefineVariable, CmdDefineVariable> header = ParseForLoopHeader (tokensAtCurrentLine, currentLine);
			ForLoop forLoop = new ForLoop (header.Item1, header.Item2, header.Item3);
			List<Token> childTokens = BuildCodeBlock (tokensToParse, currentLine);
			if (childTokens.Count > 0) {
				int last = childTokens.Last ().Position.Line;
				Token lastToken = tokensToParse.First (x => x.Position.Line == last + incrementIndex);
				EndOfBlockTokenCorrect (lastToken, "end;", ETokenType.FOR);
				RemoveTokensAtLine (tokensToParse, last + incrementIndex);
				List<ICodeBlock> childeren = BuildCommandBlok (childTokens, currentLine + 1, last + 1);
				if (childeren.Count > 0) {
					foreach (var item in childeren) {
						forLoop.addChild (item);
					}
					forLoop.LineNumber = currentLine;
					return forLoop;
				} else {
					throw EmptyCodeBlock (currentLine, ETokenType.FOR);
				}
			} else {
				throw EmptyCodeBlock (currentLine, ETokenType.FOR);
			}
		}

		/// Author: Max Hamulyak
		/// Date:	29-06-2015
		/// <summary>
		/// Parses the declaration of an function, so it can be reused througout the program.
		/// </summary>
		/// <returns>The function declaration.</returns>
		/// <param name="tokensToParse">Tokens to parse.</param>
		/// <param name="currentLine">Current line.</param>
		protected virtual FunctionBlock ParseFunctionDeclaration (List<Token> tokensToParse, int currentLine){
			List<Token> tokensAtCurrentLine = tokensToParse.Where (x => x.Position.Line == currentLine).ToList();
			tokensToParse.RemoveAll (x => x.Position.Line == currentLine);
			string functionName = ParseFunctionHeader(tokensAtCurrentLine,currentLine);
			FunctionBlock functionBlock = new FunctionBlock (functionName);
			List<Token> childTokens = BuildCodeBlock (tokensToParse, currentLine);
			if (childTokens.Count > 0) {
				int last = childTokens.Last ().Position.Line;
				Token lastToken = tokensToParse.First (x => x.Position.Line == last + incrementIndex);
				EndOfBlockTokenCorrect (lastToken, "end;", ETokenType.FUNCTIONDeclaration);
				RemoveTokensAtLine (tokensToParse, last + incrementIndex);
				List<ICodeBlock> childeren = BuildCommandBlok (childTokens, currentLine + 1, last + 1);
				if (childeren.Count > 0) {
					foreach (var item in childeren) {
						functionBlock.addChild (item);
					}
					functionBlock.LineNumber = currentLine;
					return functionBlock;
				} else {
					throw EmptyCodeBlock (currentLine, ETokenType.FUNCTIONDeclaration);
				}
			} else {
				throw EmptyCodeBlock (currentLine, ETokenType.FUNCTIONDeclaration);
			}
		}

		/// Author: Max Hamulyak
		/// Date:	29-06-2015
		/// <summary>
		/// Defines the variable from token, it also checks if a variable is not a keyword in the given language.
		/// </summary>
		/// <returns>The variable from token.</returns>
		/// <param name="instructionLine">Instruction line.</param>
		/// <param name="currentLine">Current line.</param>
		protected virtual CmdDefineVariable DefineVariableFromToken(List<Token> instructionLine, int currentLine)
		{
			List<Token> tokensAtCurrentLine = instructionLine.Where (x => x.Position.Line == currentLine).ToList ();
			instructionLine.RemoveAll (x => x.Position.Line == currentLine);
			string variableName = tokensAtCurrentLine.First ().Value;
			if (languageKeywords.Contains (variableName)) {
				throw KeyWordAsVariable (currentLine, variableName, EGameLanguage.Python);
			} else {
				VariableSolver solver = ParseVariableDefinition (tokensAtCurrentLine, currentLine);
				DefineVariable myVar = new DefineVariable (solver);
				CmdDefineVariable myVariable = new CmdDefineVariable (tokensAtCurrentLine.First().Value,myVar);
				return myVariable;
			}
		}

		/// Auhtor: Max Hamulyak
		/// Date:	19-06-2015
		/// <summary>
		/// Parses if statement header, so it can be used as an condition for an actual ifstatement, 
		/// </summary>
		/// <returns>The if statement header.</returns>
		/// <param name="headerDeclaration">Header declaration.</param>
		/// <param name="headerLineNumber">Header line number.</param>
		protected virtual ICondition ParseIfStatementHeader (List<Token> headerDeclaration, int headerLineNumber)
		{
			Token startOfIfBlock = headerDeclaration.FirstOrDefault (ifStatementStartBlockIndicator);
			if (startOfIfBlock == null) {
				if (headerDeclaration.Exists (x => x.Type == ETokenType.CommentLine)) {
					throw CommentInsideCondition (headerLineNumber, ETokenType.IF, ifStamentIndicatorValue);
				} else {
					throw IncompleteDeclarationOfCondition (headerLineNumber, ETokenType.IF, ifStamentIndicatorValue);
				}
			} else {
				int startOfBlockIndex = headerDeclaration.IndexOf (startOfIfBlock);
				List<Token> tokensTillRange = headerDeclaration.GetRange (1, startOfBlockIndex - 1);
				if (startOfBlockIndex + 2 < headerDeclaration.Count) {
					List<Token> tokensAfterRange = headerDeclaration.GetRange (startOfBlockIndex + 1, headerDeclaration.Count - startOfBlockIndex - 1);
					tokensAfterRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace || x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL);
					if (tokensAfterRange.Count > 0) {
						Token errorToken = tokensAfterRange.First ();
						if (errorToken.Type == ETokenType.VARIABLE || errorToken.Type == ETokenType.Literal || errorToken.Type == ETokenType.FUNCTIONCall) {
							throw ValueIndicatorAfterCondition (headerLineNumber, ETokenType.IF, errorToken.Type, errorToken.Value);
						} else {
							throw BlockIndicatorAfterCondition (headerLineNumber, ETokenType.IF, errorToken.Type);
						}
					}
				} 
				tokensTillRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace);
				if (tokensTillRange.Count > 0) {
					ICondition condition = BuildICondition (tokensTillRange, headerLineNumber);
					return condition;
				} else {
					throw EmptyCondition (headerLineNumber, ETokenType.IF, ifStamentIndicatorValue);
				}
			}

		}

		/// Author: Max Hamulyak
		/// Date:	19-06-2015
		/// <summary>
		/// Checks if the declaration of the WhileLoop is correct, as in ends with :, doesnt contain comment
		/// and has at least one element in between.
		/// It will also check if the declaration is only followed by a comment or a whitespace/eol if this is not
		/// the case, it will throw error acordingly.
		/// </summary>
		/// <returns>ICondition, that can be used in WHILE Loop as a Solver</returns>
		/// <param name="headerDeclaration">List of tokens that is used as whileloop declaration</param>
		/// <param name="headerLineNumber">Line number of the declaration</param>
		protected virtual ICondition ParseWhileLoopHeader(List<Token> headerDeclaration, int headerLineNumber){
			Token startOfWhileBlock = headerDeclaration.FirstOrDefault (whileLoopStatementStartBlockIndicator);
			if (startOfWhileBlock == null) {
				if (headerDeclaration.Exists( x => x.Type == ETokenType.CommentLine)){
					throw CommentInsideCondition (headerLineNumber, ETokenType.WHILE, whileLoopStatementIndicatorValue);
				}else {
					throw IncompleteDeclarationOfCondition (headerLineNumber, ETokenType.WHILE, whileLoopStatementIndicatorValue);
				}
			} else {
				int startOfBlockIndex = headerDeclaration.IndexOf (startOfWhileBlock);
				//First token indicates this should be WHILE Loop, so it can be skipped
				List<Token> tokensTillRange = headerDeclaration.GetRange (1, startOfBlockIndex - 1);
				if (startOfBlockIndex + 2 < headerDeclaration.Count) {
					List<Token> tokensAfterRange = headerDeclaration.GetRange (startOfBlockIndex + 1, headerDeclaration.Count - startOfBlockIndex - 1);
					tokensAfterRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace || x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL);
					if (tokensAfterRange.Count > 0) {
						Token errorToken = tokensAfterRange.First ();
						if (errorToken.Type == ETokenType.VARIABLE || errorToken.Type == ETokenType.Literal || errorToken.Type == ETokenType.FUNCTIONCall) {
							throw ValueIndicatorAfterCondition (headerLineNumber, ETokenType.WHILE, errorToken.Type, errorToken.Value);
						} else {
							throw BlockIndicatorAfterCondition (headerLineNumber, ETokenType.WHILE, errorToken.Type);
						}
					}
				} 
				tokensTillRange.RemoveAll (x => x.Type == ETokenType.WhiteSpace);
				if (tokensTillRange.Count > 0) {
					ICondition condition = BuildICondition (tokensTillRange, headerLineNumber);
					return condition;
				} else {
					throw EmptyCondition (headerLineNumber, ETokenType.WHILE, whileLoopStatementIndicatorValue);
				}
			}
		}

		/// Author: Max Hamulyak
		/// Date: 	19-06-2015
		/// <summary>
		/// Parses the call to a user function.
		/// </summary>
		/// <returns>The function call.</returns>
		/// <param name="tokensAtCurrentLine">Tokens at current line.</param>
		protected virtual FunctionBlockExecute ParseFunctionCall (List<Token> tokensAtCurrentLine)
		{
			Token tokenToParse = tokensAtCurrentLine.First ();
			tokensAtCurrentLine.Remove (tokenToParse);
			tokensAtCurrentLine.RemoveAll (x => x.Type == ETokenType.CommentLine || x.Type == ETokenType.EOL || x.Type == ETokenType.WhiteSpace);
			if (tokensAtCurrentLine.Count > 0) {
				throw TokenAfterFunctionCall (tokenToParse.Position.Line, tokenToParse.Value, tokensAtCurrentLine.First ().Value);
			}
			string functionName = tokenToParse.Value.TrimEnd ("()".ToCharArray()).Trim();
			FunctionBlockExecute executeFunction = new FunctionBlockExecute (functionName);
			executeFunction.LineNumber = tokenToParse.Position.Line;
			return executeFunction;
		}


		/// Author: Max Hamulyak
		/// Date:	08-06-2015
		/// <summary>
		/// Builds the condition class to be used, using priority rules
		/// </summary>
		/// <returns>The condition class.</returns>
		/// <param name="conditions">Conditions.</param>
		/// <param name="priorityOperator">Priority operator.</param>
		protected virtual ICondition BuildConditionClass (List<ICondition> conditions, ELogicOperators priorityOperator, int lineNumber){

			int index = 0;
			bool stopLoop = false;

			while (index < conditions.Count && !stopLoop) {

				if (conditions.Count > 3) {
					int loopIndex = 0;
					while (loopIndex < conditions.Count) {
						ICondition item = conditions.ElementAt (loopIndex);
						if (item is Operator) {

							if ((item as Operator).LogicOperator == priorityOperator) {

								int itemIndex = conditions.IndexOf (item); 
								ICondition left = conditions.ElementAt (itemIndex - 1);
								ICondition right = conditions.ElementAt (itemIndex + 1);
								conditions.Remove (left);
								conditions.Remove (right);
								conditions.Remove (item);
								if ((left is Solver) && (right is Solver)) {
									ConditionCombo newCondition = new ConditionCombo ((left as Solver), (right as Solver), priorityOperator);
									conditions.Insert (itemIndex - 1, newCondition);
								} else {
									throw InvalidConditionCombo (left, right);
								}
							}
						}
						loopIndex++;
					}
					BuildConditionClass (conditions, priorityOperator + 1, lineNumber);
				} else {
					stopLoop = true;
				}

			}
			//There is only one element, should be concrete instruction, if valid syntax
			ICondition returnCondition = null;
			if (conditions.Count == 1) {
				returnCondition = conditions.ElementAt (0);
			} else if (conditions.Count == 2) {

				if (conditions.ElementAt (0) is Operator) {
					Operator o = conditions.ElementAt (0) as Operator;
					if (o.LogicOperator == ELogicOperators.Not) {

						if (conditions.ElementAt (1) is Solver) {

							returnCondition = new ConditionCombo (conditions.ElementAt (1) as Solver, o.LogicOperator);

						} else {
							throw ExpectedConditionAfterNOT (lineNumber);
						}

					} else {
						throw ExpectedButFound(lineNumber,ETokenType.LogicalOperator,"not", ETokenType.LogicalOperator, o.LogicOperator.ToString());
					}
				} else {
					throw ExpectedButFound (lineNumber);
				}

			} else if (conditions.Count == 3) {
				if (conditions.ElementAt (1) is Operator) {

					if ((conditions.ElementAt (0) is Solver) && (conditions.ElementAt (2) is Solver)) {
						returnCondition = new ConditionCombo (conditions.ElementAt (0) as Solver, conditions.ElementAt (2) as Solver, (conditions.ElementAt (1) as Operator).LogicOperator);
					}
				}
			} else {
				throw EmptyConditionCombo (lineNumber);
			}
			return returnCondition;

		}

		#endregion
		#region Shared Enum Parsing

		/// Author: Max Hamulyak
		/// Date:	17-06-2015
		/// <summary>
		/// Reads the value of a token, and gives the correct math operator back.
		/// This method is in CodeParser because math operator ( + - / *) should be the same
		/// in all programming languages. If desired it can be overuled in a ConcreteParser.
		/// </summary>
		/// <returns>The math operator from token.</returns>
		/// <param name="token">Token.</param>
		protected virtual EMathOperator ParseMathOperatorFromToken(Token token){

			EMathOperator mathOperator = EMathOperator.None;

			switch (token.Value) {

			case "*":
				mathOperator = EMathOperator.Multiply;
				break;
			case "/":
				mathOperator = EMathOperator.Divide;
				break;
			case "+":
				mathOperator = EMathOperator.Add;
				break;
			case "-":
				mathOperator = EMathOperator.Subtract;
				break;
			default:
				break;
			}

			return mathOperator;
		}

		/// Author: Max Hamulyak
		/// Date:	17-06-2015
		/// <summary>
		/// Reads the value of a token, and gives the correct comparison operator back.
		/// This method is in CodeParser because math operator (==, !=, &lt;, &gt;, &lt;= and &gt;=) should be the same
		/// in all programming languages. If desired it can be overuled in a ConcreteParser.
		/// </summary>
		/// <returns>The comparison operator from token.</returns>
		/// <param name="token">Token.</param>
		protected virtual EComparisonOperator ParseComparisonOperatorFromToken (Token token){

			EComparisonOperator comparisionOperator = EComparisonOperator.None;

			switch (token.Value) {
			case "<=":
				comparisionOperator = EComparisonOperator.ValueLessThanOrEqualTo;
				break;
			case ">=":
				comparisionOperator = EComparisonOperator.ValueGreaterThanOrEqualTo;
				break;
			case "<":
				comparisionOperator = EComparisonOperator.ValueLessThan;
				break;
			case ">":
				comparisionOperator = EComparisonOperator.ValueGreaterThan;
				break;
			case "==":
				comparisionOperator = EComparisonOperator.ValueEqualTo;
				break;
			case "!=":
				comparisionOperator = EComparisonOperator.ValueNotEqualTo;
				break;
			default:
				break;
			}

			return comparisionOperator;

		}

		/// Author: Max Hamulyak
		/// Date:	19-06-2015 
		/// <summary>
		/// Can parse a logic operator from a token, should be able to match
		/// syntax from most programming languages, can be overriden in child class
		/// if logic is different, or more cases needed to be added.
		/// The following values ar supported:
		/// <list type="bullet">
		/// <item>
		/// <term>LOGICAL OPERATOR AND</term>
		/// <description> Support values are (and or &&)</description>
		/// </item>
		/// <item>
		/// <term>LOGICAL OPERATOR OR</term>
		/// <description> Support values are (or or ||)</description>
		/// </item>
		/// <item>
		/// <term>LOGICAL OPERATOR NOT</term>
		/// <description> Support values are (not or !) </description>
		/// </item>
		/// </list>
		/// </summary>
		/// <returns>The logic operator from token.</returns>
		/// <param name="token">Token.</param>
		protected virtual ELogicOperators ParseLogicOperatorFromToken (Token token){
			ELogicOperators logicOperator = ELogicOperators.None;
			switch (token.Value) {
			case "and":
			case "&&":
				logicOperator = ELogicOperators.And;
				break;
			case "or":
			case "||":
				logicOperator = ELogicOperators.Or;
				break;
			case "not":
			case "!":
				logicOperator = ELogicOperators.Not;
				break;
			default:
				break;
			}
			return logicOperator;
		}
			
		#endregion
		#region CodeParserExceptionMessages

		protected CodeParseException EmptyConditionCombo (int lineNumber){
			return new CodeParseException ("ERROR MESSAGE");
		}

		protected CodeParseException InvalidConditionCombo (ICondition left, ICondition right){
			return new CodeParseException ("ERROR MESSAGE");
		}

		protected CodeParseException InvalidAssignmentTwoAssignmentValues(List<IAssignment> assignments){
			return new CodeParseException ("ERROR MESSAGE");
		}

		protected CodeParseException InvalidAssignmentMathOperator(int lineNumber){
			return new CodeParseException ("ERROR MESSAGE");
		}
		protected CodeParseException InvalidAssignment(int lineNumber){
			return new CodeParseException ("ERROR MESSAGE");
		}


		/// Author: Max Hamulyak
		/// Date:	29-06-2015
		/// <summary>
		/// Should be used to indicate that a variable with the same name has already been declared.
		/// </summary>
		/// <returns>The name has already been declared.</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="variableName">Variable name.</param>
		protected CodeParseException VariableNameHasAlreadyBeenDeclared(int lineNumber, string variableName){
			return new CodeParseException (String.Format (errorAtLine + "variable with name '{1}' has already been declared", lineNumber,variableName));
		}
		/// Author: Max Hamulyak
		/// Date:	29-06-2015
		/// <summary>
		/// Should be used to indicate that a variable has been assignt a variable before it has been declared.
		/// </summary>
		/// <returns>The assignment before declaration.</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="variableName">Variable name.</param>
		protected CodeParseException VariableAssignmentBeforeDeclaration(int lineNumber, string variableName){
			return new CodeParseException(String.Format (errorAtLine + "variable '{1}' must be declared before it can be assignd",lineNumber,variableName));
		}
		/// Author: Max Hamulyak
		/// Date:	29-06-2015
		/// <summary>
		/// Should be used to indicate that a block is not closed in the proper manner.
		/// </summary>
		/// <returns>The ending of block.</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="expectedEnd">Expected end.</param>
		/// <param name="actualEnd">Actual end.</param>
		/// <param name="blockType">Block type.</param>
		protected CodeParseException IncorrectEndingOfBlock(int lineNumber, string expectedEnd, string actualEnd, string blockType){
			return new CodeParseException (String.Format (errorAtLine + invalidSyntax + "expected '{1}' to be used in ending of '{2}' but found '{3}'", lineNumber, expectedEnd, blockType, actualEnd));
		}
		/// Author: Max Hamulyak
		/// Date:	29-06-2015
		/// <summary>
		/// Should be used to indicate that there is an missing semicolon after a statement
		/// </summary>
		/// <returns>The semicolon exception.</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="afterValue">After value.</param>
		protected CodeParseException MissingSemicolonException(int lineNumber, string afterValue){
			return new CodeParseException (String.Format (errorAtLine + invalidSyntax + "missing ';' after '{1}'", lineNumber, afterValue)); 
		}
		/// Author: Max Hamulyak
		/// Date:	29-06-2015
		/// <summary>
		/// Should be used to indicate that there are incorrect values before the semcicolon
		/// </summary>
		/// <returns>The before semicolon.</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="tokenToParse">Token to parse.</param>
		/// <param name="errorToken">Error token.</param>
		protected CodeParseException ValueBeforeSemicolon(int lineNumber, Token tokenToParse, Token errorToken){
			return new CodeParseException (String.Format (errorAtLine + invalidSyntax + "Expected ';' after '{1}' but found '{2}'", lineNumber, tokenToParse.Value, errorToken.Value));
		}
		/// Author: Max Hamulyak
		/// Date:	29-06-2015
		/// <summary>
		/// Should be used to indicate that there are values after a semicolon
		/// </summary>
		/// <returns>The after semicolon.</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="tokenToParse">Token to parse.</param>
		/// <param name="errorToken">Error token.</param>
		protected CodeParseException ValueAfterSemicolon(int lineNumber, Token tokenToParse, Token errorToken){
			return new CodeParseException (String.Format (errorAtLine + invalidSyntax + "Expected eol after '{1};' but found '{2}'", lineNumber, tokenToParse.Value, errorToken.Value));
		}

		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a functioncall is followed by other value
		/// </summary>
		protected CodeParseException TokenAfterFunctionCall(int lineNumber, string functionName, string foundValue){
			return new CodeParseException (String.Format (errorAtLine + invalidSyntax + "found {1} after call to '{2}'", lineNumber, foundValue, functionName)); 
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a parenthesis is opened but not closed
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="leftParenthesis">Left parenthesis.</param>
		/// <param name="rightParenthesis">Right parenthesis.</param>
		protected CodeParseException UnclosedParenthesis(int lineNumber, string leftParenthesis, string rightParenthesis){
			return new CodeParseException(String.Format(errorAtLine + invalidSyntax + "parenthesis starting with '{1}' are not closed with corresponding '{2}'",lineNumber,leftParenthesis,rightParenthesis));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a parenthesis is closed but not opened
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="leftParenthesis">Left parenthesis.</param>
		/// <param name="rightParenthesis">Right parenthesis.</param>
		protected CodeParseException UnopenedParenthesis(int lineNumber, string leftParenthesis, string rightParenthesis){
			return new CodeParseException(String.Format(errorAtLine + invalidSyntax + "parenthesis ending with '{1}' is not opened with corresponding '{2}'",lineNumber,leftParenthesis,rightParenthesis));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a unexpected statement has been found
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="value">Value.</param>
		/// <param name="type">Type.</param>
		protected CodeParseException UnexpectedStatement(int lineNumber, string value, ETokenType type){
			if (type == ETokenType.Invalid) {
				return new CodeParseException (String.Format (errorAtLine + "Syntax '{1}' is an invalid symbol", lineNumber, value));

			} else {
				return new CodeParseException (String.Format (errorAtLine + "Syntax {1} of type {2} is unexpected", lineNumber, value, type));
			}
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a IF block is being used without opening it with an preceding IF
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="blockType">Block type.</param>
		/// <param name="error">Error.</param>
		protected CodeParseException BeginIFBlockWithoutIF(int lineNumber, string blockType, string error){
			return new CodeParseException (String.Format (errorAtLine + "An {1} block cannot start with an {2}", lineNumber, blockType, error));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a keyword is being used as a variable
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="keyWord">Key word.</param>
		/// <param name="programmingLanguage">Programming language.</param>
		protected CodeParseException KeyWordAsVariable(int lineNumber,string keyWord, EGameLanguage programmingLanguage){
			return new CodeParseException (String.Format (errorAtLine + invalidSyntax + "'{1}' is a keyword in {2} and cannot be used as a variable", lineNumber, keyWord, programmingLanguage));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a keyword is expected between two values.
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="keyWord">Key word.</param>
		/// <param name="leftSide">Left side.</param>
		/// <param name="rightSide">Right side.</param>
		protected CodeParseException ExpectedKeyWordBetween(int lineNumber, string keyWord, string leftSide, string rightSide){
			return new CodeParseException (String.Format (errorAtLine + "Expected {1}, after {2} and before {3}", lineNumber, keyWord, leftSide, rightSide));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a variable is used after assignmentOperator, but before language keyword
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="leftSide">Left side.</param>
		/// <param name="rightSide">Right side.</param>
		protected CodeParseException ExpectedAssignmentBetweenKeyword(int lineNumber, string leftSide, string rightSide){
			return new CodeParseException (String.Format (errorAtLine + "Expected at least one variable/literal between {1} and {2}", lineNumber, leftSide, rightSide));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a Comment is used inside the declaration of a variable.
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="assignmentOperator">Assignment operator.</param>
		/// <param name="variableName">Variable name.</param>
		/// <param name="commentIndicator">Comment indicator.</param>
		protected CodeParseException CommentInsideVariableDeclaration(int lineNumber, string assignmentOperator, string variableName, string commentIndicator){
			return new CodeParseException (String.Format (errorAtLine + "Expected {1} after the declaration of {2}, found a commented line as indicated by {3}, please place comments outside of variable declaration",lineNumber, assignmentOperator, variableName, commentIndicator));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a AssignmentOperator is missing after declaration of a variable
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="assignmentOperator">Assignment operator.</param>
		/// <param name="variableName">Variable name.</param>
		protected CodeParseException MissingAsignmentOperatorAfterDeclaration(int lineNumber, string assignmentOperator, string variableName){
			return new CodeParseException (String.Format (errorAtLine + "Expected {1} after the declaration of {2}, {1} is required to assign a variable",lineNumber, assignmentOperator,variableName));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a Invalid Value hase been found before an assignment operator
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="assignmentOperator">Assignment operator.</param>
		/// <param name="variableName">Variable name.</param>
		/// <param name="foundType">Found type.</param>
		/// <param name="foundValue">Found value.</param>
		protected CodeParseException FoundValueBeforeAssignmentOperator(int lineNumber, string assignmentOperator, string variableName,ETokenType foundType, string foundValue){
			return new CodeParseException (String.Format (errorAtLine + invalidSyntax + "Found {2} with a value of {3} between {4} and {1} of a variable declaration", lineNumber, assignmentOperator, foundType, foundValue, variableName)); 
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a Block is being used before assignment operator
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="assignmentOperator">Assignment operator.</param>
		/// <param name="variableName">Variable name.</param>
		/// <param name="foundType">Found type.</param>
		protected CodeParseException FoundBlockBeforeAssignmentOperator(int lineNumber, string assignmentOperator, string variableName, ETokenType foundType){
			return new CodeParseException (String.Format (errorAtLine + invalidSyntax + "Found block of type {3} between {2} and {1} of a variable declaration",lineNumber,assignmentOperator,variableName,foundType));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a InvalidToken has been found after assignement operator
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="assignmentOperator">Assignment operator.</param>
		/// <param name="variableName">Variable name.</param>
		/// <param name="foundType">Found type.</param>
		protected CodeParseException InvalidTokenAfterAssignmentOperator(int lineNumber, string assignmentOperator, string variableName, ETokenType foundType){
			String result = String.Format (errorAtLine + invalidSyntax + "Found '{3}' after {1} of the variable declaration of '{2}'", lineNumber, assignmentOperator, variableName, foundType);
			return new CodeParseException (result);
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that te assignment of the variable is empty
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="assignmentOperator">Assignment operator.</param>
		/// <param name="variableName">Variable name.</param>
		protected CodeParseException EmptyAssignment(int lineNumber, string assignmentOperator, string variableName){
			return new CodeParseException (String.Format (errorAtLine + "Expected at least one assignment after {1} of the variable declaration of {2}", lineNumber, assignmentOperator, variableName));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a a command is invalid, gives sugestion of correct type.
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="triedType">Tried type.</param>
		/// <param name="sugestedType">Sugested type.</param>
		protected CodeParseException InvalidCommand (int lineNumber, string triedType, string sugestedType){
			return new CodeParseException (String.Format (errorAtLine + "Unsupported command '{1}', did you mean to use '{2}'",lineNumber,triedType,sugestedType));
		} 
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a Condition is empty
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="compositeType">Composite type.</param>
		/// <param name="startBlockIndicator">Start block indicator.</param>
		protected CodeParseException EmptyCondition(int lineNumber, ETokenType compositeType, string startBlockIndicator){
			return new CodeParseException (String.Format (errorAtLine + "Expected at least one condition after '{1}' declaration and before '{2}'", lineNumber, compositeType,startBlockIndicator));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a comment is used inside of a condition.
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="compositeType">Composite type.</param>
		/// <param name="startBlockIndicator">Start block indicator.</param>
		protected CodeParseException CommentInsideCondition(int lineNumber, ETokenType compositeType, string startBlockIndicator){
			return new CodeParseException (String.Format (errorAtLine + "a comment cannot be used between '{1}' and '{2}' because it is a block declaration", lineNumber,compositeType, startBlockIndicator));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a condition declaration is incomplete
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="compositeType">Composite type.</param>
		/// <param name="startBlockIndicator">Start block indicator.</param>
		protected CodeParseException IncompleteDeclarationOfCondition(int lineNumber, ETokenType compositeType, string startBlockIndicator){
			return new CodeParseException (String.Format (errorAtLine + "Expected '{2}' after declaration of '{1}' because it starts the block ", lineNumber,compositeType, startBlockIndicator));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a value is used after condition declaration
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="compositeType">Composite type.</param>
		/// <param name="foundType">Found type.</param>
		/// <param name="value">Value.</param>
		protected CodeParseException ValueIndicatorAfterCondition(int lineNumber, ETokenType compositeType, ETokenType foundType, string value){
			return new CodeParseException (String.Format (errorAtLine + "Expected new line after {1} declaration, found {2} with a value of {3}", lineNumber, compositeType, foundType, value));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a BlockIndicator has been found after condition, this is invalid syntax
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="compositeType">Composite type.</param>
		/// <param name="foundType">Found type.</param>
		protected CodeParseException BlockIndicatorAfterCondition(int lineNumber, ETokenType compositeType, ETokenType foundType){
			return new CodeParseException (String.Format (errorAtLine + "Expected new line after {1} declaration, found block of type {2}", lineNumber, compositeType, foundType));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a value is used inside condition.
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="compositeType">Composite type.</param>
		/// <param name="foundType">Found type.</param>
		/// <param name="value">Value.</param>
		protected CodeParseException ValueIndicatorInsideCondition(int lineNumber, ETokenType compositeType, ETokenType foundType, string value){
			return new CodeParseException (String.Format (errorAtLine + "Found {2} with a value of {3} after {1} declaration", lineNumber, compositeType, foundType, value));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a block is being used inside a condition
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="compositeType">Composite type.</param>
		/// <param name="foundType">Found type.</param>
		protected CodeParseException BlockIndicatorInsideCondition(int lineNumber, ETokenType compositeType, ETokenType foundType){
			return new CodeParseException (String.Format (errorAtLine + "To Found block of type {2} after {1} declaration", lineNumber, compositeType, foundType));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a CodeBlock is Empty
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="compositeType">Composite type.</param>
		protected CodeParseException EmptyCodeBlock(int lineNumber, ETokenType compositeType){
			return new CodeParseException (String.Format (errorAtLine + "A {1} block must define at least one child", lineNumber,compositeType));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a CodeBlock is missing its declaration.
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		protected CodeParseException MissingCodeBlockDeclaration(int lineNumber){
			return new CodeParseException (String.Format (errorAtLine + "A block needs to be defined using BEGIN", lineNumber));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a a value was expected, but found other value
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="expectedType">Expected type.</param>
		/// <param name="expectedValue">Expected value.</param>
		/// <param name="actualType">Actual type.</param>
		/// <param name="actualValue">Actual value.</param>
		protected CodeParseException ExpectedButFound(int lineNumber, ETokenType expectedType, string expectedValue, ETokenType actualType, string actualValue){
			return new CodeParseException (String.Format (errorAtLine + "Expected '{2}' of type {1}, but found '{4}' of type {3}",lineNumber,expectedType,expectedValue,actualType,actualValue));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a value was expected but found other value
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="leftSolver">Left solver.</param>
		/// <param name="rightSolver">Right solver.</param>
		protected CodeParseException ExpectedButFound(int lineNumber){
			return new CodeParseException (String.Format (errorAtLine + "Expected LogicOperator to separate two conditions",lineNumber));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a NOT is not followed by a condition
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		protected CodeParseException ExpectedConditionAfterNOT(int lineNumber){
			return new CodeParseException (String.Format (errorAtLine + "Expected at least one condition after not",lineNumber));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that an IAssignment is Empty
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		protected CodeParseException EmptyIAssignment(int lineNumber){
			return new CodeParseException (String.Format (errorAtLine + "Assignment cannot be empty",lineNumber));
		}
		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Should be used to indicate that a unexpected value has been used inside condition.
		/// </summary>
		/// <returns>CodeParseException</returns>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="tokenType">Token type.</param>
		/// <param name="value">Value.</param>
		protected CodeParseException UnexpectedValueInSideCondition(int lineNumber, string value)
		{
			return new CodeParseException (String.Format (errorAtLine + "Unexpected token with a value of '{1}' has been found", lineNumber,value));
		}

		#endregion
		#region PublicMethods

		/// Author: Max Hamulyak
		/// Date:	08-06-2015
		/// <summary>
		/// Parses a user enterd string to valid <see cref="Token"/> using the <see cref="Lexer"/>
		/// and determines the maximum line based on the last token. These are then parsed to a List of
		/// <see cref="CodeBlock"/> that a user can execute.
		/// </summary>
		/// <returns>The code.</returns>
		/// <param name="codeToParse">Code to parse.</param>
		public List<ICodeBlock> ParseCode (string codeToParse)
		{
			try {
				List<Token> parsedTokens = languageLexer.Tokenize (codeToParse).ToList ();
				Token end = parsedTokens.Last (x => x.Type == ETokenType.EOF);
				List<ICodeBlock> myCommands = BuildCommandBlok(parsedTokens,1,end.Position.Line);
				return myCommands;
			} catch (SyntaxParseException syntaxParseException) {
				throw syntaxParseException;
			} catch (RobotException codeParseException){
				throw codeParseException;
			}
		}

		#endregion
		#region ParseICondition

		/// Author: Max Hamulyak
		/// Date:	08-09-2015
		/// <summary>
		/// Builds the <see cref="ICondition"/> that is used for a <see cref="WhileLoop"/>
		/// or an <see cref="IfStatement"/> based on the tokenLine that declares it. 
		/// </summary>
		/// <returns>The ICondition.</returns>
		/// <param name="instructionLine">Instruction line.</param>
		protected ICondition BuildICondition (List<Token> instructionLine, int lineNumber){
			if (instructionLine.Count > 0) {
				List<ICondition> conditions = new List<ICondition> ();
				int i = 0;
				while (i < instructionLine.Count) {
					bool removedTokens = false;
					Token token = instructionLine [i];


					if (token.Type == ETokenType.leftParentheses) {
						conditions.Add(BuildIConditionResolveParenthesis(instructionLine,lineNumber));
						removedTokens = true;
					} else if (token.Type == ETokenType.rightParentheses) {
						throw UnopenedParenthesis (lineNumber, "(", ")");

					} else if (token.Type == ETokenType.Literal || token.Type == ETokenType.VARIABLE) {

						conditions.Add(BuildIConditionResolveValueSolver(instructionLine,lineNumber));

					} else if (token.Type == ETokenType.logicInstruction) {
						conditions.Add (ParseLogicInstructionFromToken (token));

					} else if (token.Type == ETokenType.LogicalOperator) {
						conditions.Add (new Operator (ParseLogicOperatorFromToken (token)));
					} else {
						throw UnexpectedValueInSideCondition (lineNumber, token.Value);
					}
					if (!removedTokens) {
						i++;
					}
				}

				conditions = BuildIConditionResolveNOTOperators (conditions, lineNumber);

				return BuildConditionClass (conditions, ELogicOperators.Not, lineNumber);
			} else {
				throw EmptyCondition (lineNumber, ETokenType.Command, ":");
			}
		}


		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Given a token, this method builds a <see cref="ConcreteInstruction"/>
		/// </summary>
		/// <returns>The logic instruction from token.</returns>
		/// <param name="token">Token.</param>
		private ICondition ParseLogicInstructionFromToken(Token token){
			ICondition condition;
			if (token.Value.StartsWith ("at('")) {
				int quoteIndex = token.Value.IndexOf ("'");
				String desiredObject = token.Value.Substring (quoteIndex + 1);
				desiredObject = desiredObject.TrimEnd ("')".ToCharArray ());
				condition = new ConcreteInstruction (ECanInstructions.At, desiredObject);
			} else {

				switch (token.Value) {

				case "left":
					condition = new ConcreteInstruction (ECanInstructions.Left);
					break;
				case "right":
					condition = new ConcreteInstruction (ECanInstructions.Right);
					break;
				case "forward":
					condition = new ConcreteInstruction (ECanInstructions.Forward);
					break;
				case "backward":
					condition = new ConcreteInstruction (ECanInstructions.Backward);
					break;
				default:
					condition = null;
					break;
				}
			}
			return condition;
		}


		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Given a list of <see cref="ICondition"/>, it checks every element to be an NOT operator,
		/// and if found builds a <see cref="ConditionCombo"/> with the operator and the next value in line.
		/// This result in the fact that NOT operators will be resolved at the correct time.
		/// </summary>
		/// <returns>The new List of <see cref="ICondition"/></returns>
		/// <param name="conditions">List of <see cref="ICondition"/> to resolve NOT operators</param>
		/// <param name="lineNumber">Line number of condition to parse</param>
		private List<ICondition> BuildIConditionResolveNOTOperators(List<ICondition> conditions, int lineNumber){
			int conditionIndex = 0;
			while (conditionIndex < conditions.Count) {
				ICondition condition = conditions [conditionIndex];

				if (condition is Operator) {
					Operator operatorAtIndex = condition as Operator;
					if (operatorAtIndex.LogicOperator == ELogicOperators.Not) {
						if (conditionIndex + 1 < conditions.Count) {
							ICondition nextCondition = conditions [conditionIndex + 1];
							conditions.Remove (nextCondition);
							conditions.Remove (condition);
							List<ICondition> conditionList = new List<ICondition> ();
							conditionList.Add (condition);
							conditionList.Add (nextCondition);
							ICondition notCondition = BuildConditionClass(conditionList,ELogicOperators.None, lineNumber);
							conditions.Insert (conditionIndex, notCondition);
						} else {
							throw ExpectedConditionAfterNOT(lineNumber);
						}	
					}
				}
				conditionIndex++;
			}
			return conditions;
		}

		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Resolves the parenthesis priority from the instructionline
		/// </summary>
		/// <returns>The I condition resolve parenthesis.</returns>
		/// <param name="instructionLine">Instruction line.</param>
		/// <param name="lineNumber">Line number.</param>
		private ICondition BuildIConditionResolveParenthesis(List<Token> instructionLine, int lineNumber){
		
			Token token = instructionLine.First (x => x.Type == ETokenType.leftParentheses);
			int i = instructionLine.IndexOf(token);
			int firstIndex = i + 1;
			int expectedRightParenthesis = 1;
			int checkIndex = firstIndex;
			while (checkIndex < instructionLine.Count) {
				Token tokenAtIndex = instructionLine [checkIndex];
				if (tokenAtIndex.Type == ETokenType.leftParentheses) {
					expectedRightParenthesis++;
				} else if (tokenAtIndex.Type == ETokenType.rightParentheses) {
					expectedRightParenthesis--;
					if (expectedRightParenthesis == 0) {
						break;
					}
				}
				checkIndex++;
			}

			if (expectedRightParenthesis > 0) {
				throw UnclosedParenthesis (lineNumber, "(", ")");
			} else {

				Token lastRightParentheses = instructionLine [checkIndex];
				List<Token> tokens = instructionLine.GetRange (firstIndex, checkIndex - firstIndex);
				foreach (var item in tokens) {
					instructionLine.Remove (item);
				}
				instructionLine.Remove (token);
				instructionLine.Remove (lastRightParentheses);
				int j = tokens.Count;

				ICondition condition = BuildICondition (tokens, lineNumber);
				return condition;
			}
		}

		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Builds a value solver from the instructionline
		/// </summary>
		/// <returns>The I condition resolve value solver.</returns>
		/// <param name="instructionLine">Instruction line.</param>
		/// <param name="lineNumber">Line number.</param>
		private ICondition BuildIConditionResolveValueSolver(List<Token> instructionLine, int lineNumber){
			Token token = instructionLine.First ();
			int i = instructionLine.IndexOf(token);
			int firstIndex = i + 1;
			int expectedValues = 0;
			int numberOfCheck = 1;
			int checkIndex = firstIndex;
			int comparisonIndex = -1;

			while (checkIndex < instructionLine.Count) {
				Token tokenAtIndex = instructionLine [checkIndex];

				if (tokenAtIndex.Type == ETokenType.ComparisonOperator) {
					expectedValues++;
					numberOfCheck--;
					comparisonIndex = checkIndex;
				} else if (tokenAtIndex.Type == ETokenType.Operator) {
					expectedValues++;
				} else if (tokenAtIndex.Type == ETokenType.VARIABLE || tokenAtIndex.Type == ETokenType.Literal) {
					expectedValues--;
				}

				if (expectedValues < 0) {
					break;
					//THROW ERROR
				} else if (expectedValues == 0 && numberOfCheck == 0) {
					if (checkIndex + 1 < instructionLine.Count) {
						if (instructionLine.ElementAt (checkIndex + 1).Type != ETokenType.Operator) {
							break;
						} else {
							checkIndex++;
						}
					} else {
						break;
					}
					//SHOULD BE DONE
				} else {
					checkIndex++;
				}

			}
			if (comparisonIndex > 0) {
				List<Token> left = instructionLine.GetRange (i, comparisonIndex);
				List<Token> right = instructionLine.GetRange (comparisonIndex + 1, checkIndex - comparisonIndex);
				List<Token> removeTokens = instructionLine.GetRange (firstIndex, checkIndex - firstIndex);
				Token compareToken = instructionLine.ElementAt (comparisonIndex);

				EComparisonOperator operatorValue = ParseComparisonOperatorFromToken (compareToken);


				foreach (var myToken in removeTokens) {
					instructionLine.Remove (myToken);
				}
				instructionLine.Remove (token);
				VariableSolver leftSolver = BuildIAssignment (left, lineNumber);
				VariableSolver rightSolver = BuildIAssignment (right, lineNumber);
				ValueSolver valueSolver = new ValueSolver (leftSolver, rightSolver, operatorValue);
				return valueSolver;
			} else {
				//There is only one condition, thus should be bool
				VariableSolver leftSolver = BuildIAssignment (new List<Token> (){ token }, lineNumber);
				VariableSolver rightSolver = BuildIAssignment (new List<Token> (){ new Token (ETokenType.Literal, "True", null) }, lineNumber);
				ValueSolver valueSolver = new ValueSolver (leftSolver, rightSolver, EComparisonOperator.ValueEqualTo);
				return valueSolver;
			}
		}

		/// Author: Max Hamulyak
		/// Date:	24-06-2015
		/// <summary>
		/// Builds the I assignment.
		/// </summary>
		/// <returns>The I assignment.</returns>
		/// <param name="tokens">Tokens.</param>
		/// <param name="currentLine">Current line.</param>
		protected VariableSolver BuildIAssignment(List<Token> tokens, int currentLine){
			if (tokens.Count > 0) {
				int i = 0;
				List<IAssignment> assignments = new List<IAssignment> ();
				while (i < tokens.Count) {

					Token tokenAtIndex = tokens.ElementAt (i);

					if (tokenAtIndex.Type == ETokenType.VARIABLE) {
						ConcreteVariable variable = new ConcreteVariable(tokenAtIndex.Value);
						variable.LineNumber = currentLine;
						assignments.Add (variable);
					} else if (tokenAtIndex.Type == ETokenType.Operator) {

						EMathOperator myEMathOperator = ParseMathOperatorFromToken (tokenAtIndex);
						assignments.Add (new MathOperator (myEMathOperator));
					} else if (tokenAtIndex.Type == ETokenType.Literal) {
						bool boolValue;
						int intValue;
						Variable v = null;
						if (Boolean.TryParse (tokenAtIndex.Value, out boolValue)) {
							v = new Variable (boolValue, EVariableType.Bool);
						} else if (int.TryParse (tokenAtIndex.Value, out intValue)) {
							v = new Variable (intValue, EVariableType.Int);
						} else {
							v = new Variable (tokenAtIndex.Value.TrimStart('"').TrimEnd('"'), EVariableType.String);
						}
						ConcreteVariable variable = new ConcreteVariable (v);
						variable.LineNumber = currentLine;
						assignments.Add (variable);
					}
					i++;
				}
				return BuildDefineVariable (assignments, EMathOperator.Multiply);
			} else {
				throw EmptyIAssignment (currentLine);
			}
		}

		#endregion


		protected virtual void EndOfBlockTokenCorrect(Token endToken, string expectedEnd, ETokenType blockType){
			//DO NOTHING ONLY ON OVERRIDE CLASS, CURRENTLY ONLY PASCAL NEEDS THIS METHOD
		}
	}
}


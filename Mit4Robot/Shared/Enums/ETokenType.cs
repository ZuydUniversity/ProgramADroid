using System;

namespace Shared
{
	/// Author: Max Hamulyak
	/// Date:	14-06-2015
	/// <summary>
	/// Used to define a Token after it has been read by the lexer, this is based on a set of grammer rules, using
	/// regex. In this way a parser only needs to check for a valid tokentype in a situation, and act accordingly.
	/// </summary>
	public enum ETokenType
	{
		/// <summary>
		/// Should be avoided, only to be used if no other types will suffice.
		/// </summary>
		Invalid = 0,
		/// <summary>
		/// Should be used to identify languages keyword.
		/// </summary>
		KeyWord,
		/// <summary>
		/// Should be used to indicate the concrete RobotInstructions, like Forward, TurnRight and PickUp.
		/// </summary>
		Command,
		/// <summary>
		/// Should be used to indicate math operators like +, -, /, *, and  %, if supported by the programming language.
		/// </summary>
		Operator,
		/// <summary>
		/// Should be used to indicate assignment operators like =. += and -=, if supported by the programming language. 
		/// </summary>
		AssignmentOperator,
		/// <summary>
		/// Should be used to indicate comparison operators like ==, !=, &lt;, &gt;, &lt;= and &gt;= if supported by the programming language.
		/// </summary>
		ComparisonOperator,
		/// <summary>
		/// Should be used to indicate logical operators like and, or, not and xor if supported by the programming language.
		/// </summary>
		LogicalOperator,
		/// <summary>
		/// Should be used to indicate a fixed value in source code like 1, "NAME" and TRUE.
		/// </summary>
		Literal,
		/// <summary>
		/// Should be used to indicate a value type, like bool, int and string, if supported by the programming language.
		/// </summary>
		DataType,
		/// <summary>
		/// Should be used to Indicate start of a WHILE Block.
		/// </summary>
		WHILE,
		/// <summary>
		/// Should be used to Indicacte start of a IF Block.
		/// </summary>
		IF,
		/// <summary>
		/// Should be used to Indicate start of ELSE-IF Block. 
		/// </summary>
		ELSEIF,
		/// <summary>
		/// Should be used to Indicate start of ELSE Block.
		/// </summary>
		ELSE,
		/// <summary>
		/// Should be used to Indicate start of FOR Block.
		/// </summary>
		FOR,
		/// <summary>
		/// Should be start of User-Defined FUNCTION declaration.
		/// </summary>
		FUNCTIONDeclaration,
		/// <summary>
		/// Should be call to User-Defined FUNCTION
		/// </summary>
		FUNCTIONCall,
		/// <summary>
		/// Should be start of used defined VARIABLE
		/// </summary>
		VARIABLE,
		/// <summary>
		/// Should be used to indicate robot check values, like at, left,right,forward, backward.
		/// </summary>
		/// 
		logicInstruction,
		/// <summary>
		/// Should be used to indicate the start of any block, if supported by the programming language.
		/// </summary>
		startBlock,
		/// <summary>
		/// Should be used to indicate the END of any block, if supported by the programming language.
		/// </summary>
		endBlock,
		/// <summary>
		/// Should be used to indicate '('.
		/// </summary>
		leftParentheses,
		/// <summary>
		/// Should be used to indicate ')'.
		/// </summary>
		rightParentheses,
		/// <summary>
		/// Should be used to indicate a single comment line.
		/// </summary>
		CommentLine,
		/// <summary>
		/// The python range.
		/// </summary>
		PythonRange,
		WhiteSpace,
		/// <summary>
		/// Should be used to mark the end of a line entered by the user.
		/// </summary>
		EOL,
		/// <summary>
		/// Should be used to mark as the last token in a set of tokens, should be used only once, and can be used to track the 
		/// required number of lines. This Type is added automatically in the Lexer.
		/// </summary>
		EOF
	}
}


using System;
using System.Collections.Generic;
using Shared.Exceptions;

namespace Shared.BusinessLayer
{
	public static class FunctionBlockList
	{
		static Dictionary<string, FunctionBlock> functions = new Dictionary<string, FunctionBlock>();

		public static void resetList ()
		{
			functions = new Dictionary<string, FunctionBlock> ();
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Returns a FucntionBlock in the FunctionBlockList with the key functionName
		/// </summary>
		/// <returns>FunctionBlock</returns>
		/// <param name="functionName">The name of the function</param>
		public static FunctionBlock getFunction(string functionName, int lineNumber)
		{
			if (functions.ContainsKey(functionName)) {
				return functions [functionName];
			} else {
				throw new RunTimeException (String.Format ("[Error At Line [{0}]; use of undefined function named '{1}'", lineNumber, functionName));
			}
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Adds a FunctionBlock with key name to the FunctionBlockList
		/// </summary>
		/// <param name="name">Key used to later retrieve the added FunctionBlock</param>
		/// <param name="function">The FunctionBlock to be added.</param>
		public static void addFunction(string name, FunctionBlock function)
		{
			functions.Add (name, function);
		}
	}
}


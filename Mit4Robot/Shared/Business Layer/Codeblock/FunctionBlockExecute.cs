using System;
using System.Threading.Tasks;
using System.Threading;

namespace Shared.BusinessLayer
{
	public class FunctionBlockExecute : Command
	{
		private string functionName;

		public string FunctionName {
			get {
				return functionName;
			}
		}

		public FunctionBlockExecute (string functionName) 
		{
			this.functionName = functionName;
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Execute the specified parent.
		/// </summary>
		/// <param name="parent">Composite Parent of this Command</param>
		public override bool execute (Composite parent)
		{
			return executeCommand (parent);
		}

		private bool executeCommand(Composite parent){
			FunctionBlockList.getFunction (functionName, lineNumber).execute(parent);
			return true;
		}
			
	}
}


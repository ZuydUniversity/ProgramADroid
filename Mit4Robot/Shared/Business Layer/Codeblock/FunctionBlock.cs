using System;
using System.Threading.Tasks;
using System.Threading;

namespace Shared.BusinessLayer
{
	public class FunctionBlock : Composite
	{
		string functionName;

		public string FunctionName {
			get {
				return functionName;
			}
		}

		public FunctionBlock (string functionName)
		{
			this.functionName = functionName;
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Executes all children of this functionblock 
		/// </summary>
		/// <param name="parent">Parent of the Composite you are executing</param>
		public override bool execute (Composite parent)
		{
			base.execute (parent);
			foreach (ICodeBlock codeBlock in children) {
				codeBlock.execute (parent);
				Thread.Sleep (GlobalSupport.GameSpeed);
			}			
			return false;
		}
	}
}
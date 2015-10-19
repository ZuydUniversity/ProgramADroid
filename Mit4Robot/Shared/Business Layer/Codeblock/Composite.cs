using System;
using System.Collections.Generic;

namespace Shared.BusinessLayer
{
	public abstract class Composite : ICodeBlock
	{
		protected int lineNumber;
		public Dictionary<string, Variable> variables;
		protected Composite parent;

		public int LineNumber {
			get {
				return this.lineNumber;
			}
			set {
				this.lineNumber = value;
			}
		}

		protected List<ICodeBlock> children;
		protected Solver conditions;

		public Solver Conditions {
			get {
				return conditions;
			}
		}

		public Composite ()
		{
			children = new List<ICodeBlock> ();
			variables = new Dictionary<string, Variable> ();
		}
		/// Author: Bert van Montfort
		/// <summary>
		/// for use as base.execute()
		/// Adds references to all the varibles of the scope of the parent to the scope of the Compisite being executed.
		/// </summary>
		/// <param name="parent">Parent of the Compisite you are executing</param>
		public virtual bool execute(Composite parent){
			if (parent != null) {
			foreach (KeyValuePair<string, Variable> varPair in parent.variables) {
				this.variables[varPair.Key] = varPair.Value;
				}
			}
			return true;
		}

		public List<ICodeBlock> getChildren()
		{
			return children;
		}

		public void addChild(ICodeBlock child)
		{
			children.Add (child);
		}
			
	}
}


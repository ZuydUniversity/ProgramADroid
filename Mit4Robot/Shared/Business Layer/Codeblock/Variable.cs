using System;
using Shared.Exceptions;

namespace Shared.BusinessLayer
{
	public class Variable
	{
		private dynamic value;

		EVariableType type;

		public EVariableType Type {
			get {
				return type;
			}
		}

		public dynamic Value {
			get {
				return this.value;
			}
			set {
				this.value = value;
			}
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new Variable.
		/// </summary>
		/// <param name="value">The value of the variable</param>
		/// <param name="type">Int, String or Bool</param>
		public Variable (dynamic value, EVariableType type)
		{
			this.value = value;	
			this.type = type;

			if (value is string && type != EVariableType.String) {
				throw new RunTimeException ("Variable defined as string is of another type");
			}

			if (value is int && type != EVariableType.Int) {
				throw new RunTimeException ("Variable defined as int is of another type");
			}

			if (value is bool && type != EVariableType.Bool) {
				throw new RunTimeException ("Variable defined as bool is of another type");
			}
		}
	}
}


using System;
using Shared.Enums;

namespace Shared.BusinessLayer
{
	public class MathOperator: IAssignment
	{
		private int lineNumber;
		public int LineNumber {
			get {
				return this.lineNumber;
			}
			set {
				this.lineNumber = value;
			}
		}

		private EMathOperator mathOperator;

		public EMathOperator MathOperator2 {
			get {
				return mathOperator;
			}
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new mathOperator for use by Variable combo
		/// </summary>
		/// <param name="mathOperator">Math operator.</param>
		public MathOperator (EMathOperator mathOperator)
		{
			this.mathOperator = mathOperator;
		}
	}
}


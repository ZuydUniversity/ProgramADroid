using System;
using Shared.Enums;

namespace Shared.BusinessLayer
{
	public class ConcreteInstruction : Solver
	{
		private ECanInstructions instruction;

		public ECanInstructions Instruction {
			get {
				return instruction;
			}
		}

		public ConcreteInstruction (ECanInstructions instruction)
		{
			this.instruction = instruction;
		}

		public ConcreteInstruction(ECanInstructions instruction , string shopName){
			this.instruction = instruction;
			this.shopName = shopName;
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent">Parent.</param>
		public override bool solve(Composite parent){
			return solvePiece(instruction);
		}
	}
}


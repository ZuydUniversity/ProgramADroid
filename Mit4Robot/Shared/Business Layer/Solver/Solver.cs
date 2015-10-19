using System;
using Shared.Enums;

namespace Shared.BusinessLayer
{
	public abstract class Solver : ICondition
	{
		protected string shopName;

		public string ShopName {
			get {
				return shopName;
			}
		}

		/// Author: Bert van Montfort
		/// <summary>
		/// Creates a new Solver, this it is a constructor of an abstract class though.
		/// </summary>
		public Solver ()
		{
		}

		public Solver(string shopName){
			this.shopName = shopName;
		}

		public abstract bool solve(Composite parent);

		/// Author: Bert van Montfort
		/// <summary>
		/// Solves a concrete condition like "can the robot go forward?"
		/// or "is the robot at the grocery store?"
		/// </summary>
		/// <returns>False if the check failed, true if it succeeded</returns>
		/// <param name="instruction">Instruction.</param>
		public bool solvePiece(ECanInstructions instruction){
			Robot robot = Robot.Instance;
			switch (instruction) {
			case ECanInstructions.Backward:
				return robot.canMove (ECanInstructions.Backward);
			case ECanInstructions.Forward:
				return robot.canMove (ECanInstructions.Forward);
			case ECanInstructions.Left:
				return robot.canMove (ECanInstructions.Left);
			case ECanInstructions.Right:
				return robot.canMove (ECanInstructions.Right);
			case ECanInstructions.At:
				if (shopName != null) {
					return robot.At (shopName);
				} else {
					return false;
				}
			case ECanInstructions.None: 
				break;
			default:
				break;
			}
			return false;
		}
	}
}


﻿using System;

namespace Shared.Utilities
{
	public class TokenPosition
	{
		public TokenPosition (int index, int line, int column)
		{
			Index = index;
			Line = line;
			Column = column;
		}

		public int Column { get; private set; }
		public int Index { get; private set; }
		public int Line { get; private set; }
	}
}


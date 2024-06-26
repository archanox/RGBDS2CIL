﻿using System.Text;

namespace RGBDS2CIL
{
	public class NopLine : CodeLine, IAsmLine
	{
		public NopLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', tabCount)).Append(';').Append(" // NOP").AppendComment(Comment);
		}
	}
}
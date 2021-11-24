using System;
using System.Text;

namespace RGBDS2CIL
{
	public class IncrementLine : CodeLine, IAsmLine
	{
		public string Increment { get; set; }

		public IncrementLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Increment = base.Code[(base.Code.IndexOf("INC", StringComparison.OrdinalIgnoreCase) + "INC".Length)..]
				.Trim();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.AppendCode($"{Increment}++;", tabCount, Comment);
		}
	}
}
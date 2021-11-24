using System;
using System.Text;

namespace RGBDS2CIL
{
	public class DecrementLine : CodeLine, IAsmLine
	{
		public string Decrement { get; set; }

		public DecrementLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Decrement = base.Code[(base.Code.IndexOf("DEC", StringComparison.OrdinalIgnoreCase) + "DEC".Length)..].Trim();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.AppendCode($"{Decrement}--;", tabCount, Comment);
		}
	}
}
using System;
using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class ReturnLine : CodeLine, IAsmLine
	{
		public bool EnableInterrupts { get; set; }
		public string Return { get; set; }

		public ReturnLine(CodeLine codeLine, bool enableInterrupts) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			EnableInterrupts = enableInterrupts;
			var opcode = enableInterrupts ? "RETI" : "RET";
			Return = base.Code[(base.Code.IndexOf(opcode, StringComparison.OrdinalIgnoreCase) + opcode.Length)..].Trim();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			

			if (!string.IsNullOrWhiteSpace(Return))
			{
				sb.Append(new string('\t', tabCount)).AppendLine($"if({Return})");
				tabCount++;
			}

			sb.Append(new string('\t', tabCount))
				.Append("return;")
				.AppendComment(Comment);
		}
	}
}
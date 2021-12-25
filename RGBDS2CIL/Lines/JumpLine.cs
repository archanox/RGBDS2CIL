using System;
using System.Text;

namespace RGBDS2CIL
{
	public class JumpLine : CodeLine, IAsmLine
	{
		public string JumpDestination;

		public JumpLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			JumpDestination = base.Code[(base.Code.IndexOf("JP", StringComparison.OrdinalIgnoreCase) + "JP".Length)..]
				.Trim();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb
				.Append(new string('\t', tabCount))
				.Append($"goto {JumpDestination};")
				.AppendComment(Comment);
		}
	}
}
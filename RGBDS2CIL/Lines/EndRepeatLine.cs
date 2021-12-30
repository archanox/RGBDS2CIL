using System.Text;

namespace RGBDS2CIL
{
	public class EndRepeatLine : CodeLine, IAsmLine
	{
		public EndRepeatLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', tabCount - 1)).Append('}').AppendComment(Comment);
		}
	}
}
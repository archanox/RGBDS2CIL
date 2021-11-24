using System.Text;

namespace RGBDS2CIL
{
	public class EndConditionLine : CodeLine, IAsmLine
	{
		public EndConditionLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', tabCount)).Append('}').AppendComment(Comment);
		}
	}
}
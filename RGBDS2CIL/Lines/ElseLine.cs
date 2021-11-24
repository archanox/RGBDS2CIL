using System.Text;

namespace RGBDS2CIL
{
	public class ElseLine : CodeLine, IAsmLine
	{
		public ElseLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', --tabCount)).AppendLine("}");
			sb.Append(new string('\t', tabCount)).Append("else").AppendComment(Comment);
			sb.Append(new string('\t', ++tabCount)).AppendLine("{");
		}
	}
}
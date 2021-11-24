using System.Text;

namespace RGBDS2CIL
{
	public class EndMacroLine : CodeLine, IAsmLine
	{
		public EndMacroLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', tabCount)).Append('}').AppendComment(Comment);
		}
	}
}
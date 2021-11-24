using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class WarnLine : CodeLine, IAsmLine
	{
		public string Warning;

		public WarnLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Warning = Parser.GetStrings(codeLine.Code).Single().TrimStart('"').TrimEnd('"');
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', tabCount)).Append("Trace.TraceWarning($\"").Append(Warning).Append("\");").AppendComment(Comment);
		}
	}
}
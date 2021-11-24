using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class FailLine : CodeLine, IAsmLine
	{
		public string FailMessage;

		public FailLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			FailMessage = Parser.GetStrings(codeLine.Code).Single().TrimStart('"').TrimEnd('"');
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			for (var i = 1; i < 10; i++)
			{
				FailMessage = FailMessage.Replace($"\\{i}", $"{{args[{i}]}}");
			}

			sb.Append(new string('\t', tabCount)).Append("Trace.Fail($\"").Append(FailMessage).Append("\");").AppendComment(Comment);

		}
	}
}
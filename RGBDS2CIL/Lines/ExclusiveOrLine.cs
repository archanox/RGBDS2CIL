using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class ExclusiveOrLine : CodeLine, IAsmLine
	{
		public string From;
		public string Value;

		public ExclusiveOrLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			var values = codeLine.Code.Trim()["XOR".Length..].Trim();
			var split = values.Split(',');

			From = "A"; //default?
			if (split.Length == 2)
			{
				From = split[0].Trim();
				Value = split.Last().Trim();
			}
			else
				Value = split.Single().Trim();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.AppendCode($"{From} ^= {Value};", tabCount, Comment);
		}
	}
}
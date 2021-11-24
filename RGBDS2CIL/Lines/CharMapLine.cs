using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class CharMapLine : CodeLine, IAsmLine
	{
		public string From;
		public string Into;

		public CharMapLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			var values = codeLine.Code.Trim()["CHARMAP".Length..];
			Into = $"\"{codeLine.Strings.FirstOrDefault()}\"";
			From = values.Split(',').Last().Trim();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			var normalised = From.TrimStart('$').Insert(0, "0x");

			sb
				.Append(new string('\t', tabCount))
				.Append("CharMap[")
				.Append(normalised)
				.Append("] = ")
				.Append(Into)
				.Append(';')
				.AppendComment(Comment);
		}
	}
}
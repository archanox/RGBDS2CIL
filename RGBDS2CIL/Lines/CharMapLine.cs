using System.Linq;

namespace RGBDS2CIL
{
	public class CharMapLine : CodeLine
	{
		public string From;
		public string Into;

		public CharMapLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			var values = codeLine.Code.Trim()["CHARMAP".Length..];
			Into = $"\"{codeLine.Strings.FirstOrDefault()}\"";
			From = values.Split(',').Last().Trim();
		}
	}
}
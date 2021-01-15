using System.Linq;

namespace RGBDS2CIL
{
	public class LoadLine : CodeLine
	{
		public string From;
		public string Into;

		public LoadLine(CodeLine codeLine, string instruction) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			var values = codeLine.Code.Trim()[instruction.Length..];
			Into = values.Split(',')[0].Trim();
			From = values.Split(',').Last().Trim();
		}
	}
}
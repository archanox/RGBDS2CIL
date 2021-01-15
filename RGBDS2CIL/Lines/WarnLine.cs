using System.Linq;

namespace RGBDS2CIL
{
	public class WarnLine : CodeLine
	{
		public string Warning;

		public WarnLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			Warning = Parser.GetStrings(codeLine.Code).Single().TrimStart('"').TrimEnd('"');
		}
	}
}
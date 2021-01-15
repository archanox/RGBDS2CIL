using System.Linq;

namespace RGBDS2CIL
{
	public class FailLine : CodeLine
	{
		public string FailMessage;

		public FailLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			FailMessage = Parser.GetStrings(codeLine.Code).Single().TrimStart('"').TrimEnd('"');
		}
	}
}
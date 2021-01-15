using System.Linq;

namespace RGBDS2CIL
{
	public class PurgeLine : CodeLine
	{
		public string Purge;

		public PurgeLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			this.Purge = codeLine.Code.Split().Last();
		}
	}
}
using System.Linq;

namespace RGBDS2CIL
{
	public class CallLine : CodeLine
	{
		public string Call;

		public CallLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			this.Call = codeLine.Code.Split().Last();
		}
	}
}
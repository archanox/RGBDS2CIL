using System.Linq;

namespace RGBDS2CIL
{
	public class RestartLine : CodeLine
	{
		public string Restart;

		public RestartLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			this.Restart = codeLine.Code.Split().Last();
		}
	}
}
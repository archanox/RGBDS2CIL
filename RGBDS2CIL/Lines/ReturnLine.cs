using System.Linq;

namespace RGBDS2CIL
{
	public class ReturnLine : CodeLine
	{
		public bool EnableInterrupts { get; set; }
		public string Return { get; set; }

		public ReturnLine(CodeLine codeLine, bool enableInterrupts) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			EnableInterrupts = enableInterrupts;

			this.Return = codeLine.Code.Split().Last();
		}
	}
}
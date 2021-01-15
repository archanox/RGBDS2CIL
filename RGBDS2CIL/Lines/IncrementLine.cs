using System;

namespace RGBDS2CIL
{
	public class IncrementLine : CodeLine
	{
		public string Increment { get; set; }

		public IncrementLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			Increment = base.Code[(base.Code.IndexOf("INC", StringComparison.OrdinalIgnoreCase) + "INC".Length)..]
				.Trim();
		}
	}
}
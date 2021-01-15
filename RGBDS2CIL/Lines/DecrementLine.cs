using System;

namespace RGBDS2CIL
{
	public class DecrementLine : CodeLine
	{
		public string Decrement { get; set; }

		public DecrementLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			Decrement = base.Code[(base.Code.IndexOf("DEC", StringComparison.OrdinalIgnoreCase) + "DEC".Length)..]
				.Trim();
		}
	}
}
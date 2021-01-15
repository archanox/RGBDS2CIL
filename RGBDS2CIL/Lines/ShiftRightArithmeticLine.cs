namespace RGBDS2CIL
{
	public class ShiftRightArithmeticLine : CodeLine
	{
		public string RegisterOrByte;

		public ShiftRightArithmeticLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			RegisterOrByte = codeLine.Code["SRA".Length..].Trim();
		}
	}
}
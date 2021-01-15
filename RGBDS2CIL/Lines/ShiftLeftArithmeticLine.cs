namespace RGBDS2CIL
{
	public class ShiftLeftArithmeticLine : CodeLine
	{
		public string RegisterOrByte;

		public ShiftLeftArithmeticLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			RegisterOrByte = codeLine.Code["SLA".Length..].Trim();
		}
	}
}
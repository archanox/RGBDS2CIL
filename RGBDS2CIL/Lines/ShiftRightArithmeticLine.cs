namespace RGBDS2CIL
{
	public class ShiftRightArithmeticLine : CodeLine
	{
		public string RegisterOrByte;

		public ShiftRightArithmeticLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			RegisterOrByte = codeLine.Code["SRA".Length..].Trim();
		}
	}
}
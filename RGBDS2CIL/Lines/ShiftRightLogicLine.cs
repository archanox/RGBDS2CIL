namespace RGBDS2CIL
{
	public class ShiftRightLogicLine : CodeLine
	{
		public string RegisterOrByte;

		public ShiftRightLogicLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			RegisterOrByte = codeLine.Code["SRL".Length..].Trim();
		}
	}
}
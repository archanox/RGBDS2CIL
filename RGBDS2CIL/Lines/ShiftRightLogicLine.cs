namespace RGBDS2CIL
{
	public class ShiftRightLogicLine : CodeLine
	{
		public string RegisterOrByte;

		public ShiftRightLogicLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			RegisterOrByte = codeLine.Code["SRL".Length..].Trim();
		}
	}
}
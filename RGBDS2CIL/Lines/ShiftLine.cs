namespace RGBDS2CIL
{
	public class ShiftLine : CodeLine
	{
		public ShiftLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
namespace RGBDS2CIL
{
	public class RotateALeftLine : CodeLine
	{
		public RotateALeftLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
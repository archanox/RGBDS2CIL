namespace RGBDS2CIL
{
	public class RotateRegisterALeftLine : CodeLine
	{
		public RotateRegisterALeftLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
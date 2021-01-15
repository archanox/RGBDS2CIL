namespace RGBDS2CIL
{
	public class RotateRegisterRightLine : CodeLine
	{
		public RotateRegisterRightLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
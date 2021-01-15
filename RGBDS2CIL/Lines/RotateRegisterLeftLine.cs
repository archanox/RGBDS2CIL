namespace RGBDS2CIL
{
	public class RotateRegisterLeftLine : CodeLine
	{
		public RotateRegisterLeftLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
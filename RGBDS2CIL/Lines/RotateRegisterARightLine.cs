namespace RGBDS2CIL
{
	public class RotateRegisterARightLine : CodeLine
	{
		public RotateRegisterARightLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
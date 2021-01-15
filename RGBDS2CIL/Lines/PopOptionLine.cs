namespace RGBDS2CIL
{
	public class PopOptionLine : CodeLine
	{
		public PopOptionLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
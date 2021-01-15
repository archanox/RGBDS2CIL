namespace RGBDS2CIL
{
	public class PopLine : CodeLine
	{
		public PopLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
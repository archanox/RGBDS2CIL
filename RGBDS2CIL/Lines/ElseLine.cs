namespace RGBDS2CIL
{
	public class ElseLine : CodeLine
	{
		public ElseLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
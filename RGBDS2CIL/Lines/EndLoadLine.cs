namespace RGBDS2CIL
{
	public class EndLoadLine : CodeLine
	{
		public EndLoadLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
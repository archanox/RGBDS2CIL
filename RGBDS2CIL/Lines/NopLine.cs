namespace RGBDS2CIL
{
	public class NopLine : CodeLine
	{
		public NopLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
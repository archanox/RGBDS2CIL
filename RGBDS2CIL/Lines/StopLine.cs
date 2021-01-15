namespace RGBDS2CIL
{
	public class StopLine : CodeLine
	{
		public StopLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
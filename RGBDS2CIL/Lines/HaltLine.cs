namespace RGBDS2CIL
{
	public class HaltLine : CodeLine
	{
		public HaltLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
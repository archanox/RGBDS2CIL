namespace RGBDS2CIL
{
	public class EndConditionLine : CodeLine
	{
		public EndConditionLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
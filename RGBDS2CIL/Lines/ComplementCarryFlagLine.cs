namespace RGBDS2CIL
{
	public class ComplementCarryFlagLine : CodeLine
	{
		public ComplementCarryFlagLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
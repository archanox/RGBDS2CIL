namespace RGBDS2CIL
{
	public class SetCarryFlagLine : CodeLine
	{
		public SetCarryFlagLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
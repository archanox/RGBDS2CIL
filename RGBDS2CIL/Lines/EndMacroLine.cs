namespace RGBDS2CIL
{
	public class EndMacroLine : CodeLine
	{
		public EndMacroLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
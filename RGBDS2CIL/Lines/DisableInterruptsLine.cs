namespace RGBDS2CIL
{
	public class DisableInterruptsLine : CodeLine
	{
		public DisableInterruptsLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
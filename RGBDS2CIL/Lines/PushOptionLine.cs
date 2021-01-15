namespace RGBDS2CIL
{
	public class PushOptionLine : CodeLine
	{
		public PushOptionLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
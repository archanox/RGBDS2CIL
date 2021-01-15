namespace RGBDS2CIL
{
	public class RepeatLine : CodeLine
	{
		public string Repeat;

		public RepeatLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			Repeat = codeLine.Code.Trim()["REPT".Length..].Trim();
		}
	}
}
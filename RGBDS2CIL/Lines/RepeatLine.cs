namespace RGBDS2CIL
{
	public class RepeatLine : CodeLine
	{
		public string Repeat;

		public RepeatLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Repeat = codeLine.Code.Trim()["REPT".Length..].Trim();
		}
	}
}
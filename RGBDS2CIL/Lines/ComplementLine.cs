namespace RGBDS2CIL
{
	public class ComplementLine : CodeLine
	{
		public ComplementLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
		}
	}
}
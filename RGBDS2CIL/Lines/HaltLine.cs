namespace RGBDS2CIL
{
	public class HaltLine : CodeLine
	{
		public HaltLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
		}
	}
}
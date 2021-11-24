namespace RGBDS2CIL
{
	public class MacroArgumentLine : CodeLine
	{
		public byte Argument { get; set; }

		public MacroArgumentLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Argument = (byte)(byte.Parse(codeLine.Code.TrimStart('\\')) - 1);
		}
	}
}
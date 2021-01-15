namespace RGBDS2CIL
{
	public class MacroArgumentLine : CodeLine
	{
		public byte Argument { get; set; }

		public MacroArgumentLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			Argument = (byte)(byte.Parse(codeLine.Code.TrimStart('\\')) - 1);
		}
	}
}
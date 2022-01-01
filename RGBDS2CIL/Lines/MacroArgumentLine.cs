using System.Text;

namespace RGBDS2CIL
{
	public class MacroArgumentLine : CodeLine, IAsmLine
	{
		public byte Argument { get; set; }

		public MacroArgumentLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Argument = (byte)(byte.Parse(codeLine.Code.TrimStart('\\')) - 1);
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', tabCount)).Append("args[").Append(Argument - 1).Append("]();").AppendComment(Comment);
		}
	}
}
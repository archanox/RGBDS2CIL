using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	/// <summary>
	/// Set bit u3 in register r8 to 1. Bit 0 is the rightmost one, bit 7 the leftmost one.
	/// Set bit u3 in the byte pointed by HL to 1. Bit 0 is the rightmost one, bit 7 the leftmost one.
	/// </summary>
	public class VariableLine : CodeLine, IAsmLine
	{
		public List<string> Parameters;

		public VariableLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			var parameters = codeLine.Code[codeLine.Code.Split()[0].Length..].Trim();
			Parameters = Parser.GetParameters(parameters);
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb
				.Append(new string('\t', tabCount))
				.Append("Set(")
				.Append(Parameters[0])
				.Append(", ")
				.Append(Parameters[1])
				.Append(");")
				.AppendComment(Comment);
		}
	}
}
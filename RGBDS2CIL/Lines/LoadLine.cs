using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class LoadLine : CodeLine, IAsmLine
	{
		public string From;
		public string Into;

		public LoadLine(CodeLine codeLine, string instruction) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			var values = codeLine.Code.Trim()[instruction.Length..];
			Into = values.Split(',').First().Trim();
			From = values.Split(',').Last().Trim();

			//This is sometimes written as ‘LDIO A,[C]’, or ‘LD A,[$FF00+C]’.
			//This is sometimes written as ‘LD [HL+],A’, or ‘LDI [HL],A’.
			//This is sometimes written as ‘LD [HL-],A’, or ‘LDD [HL],A’.
			//This is sometimes written as ‘LD A,[HL-]’, or ‘LDD A,[HL]’.
			//This is sometimes written as ‘LD A,[HL+]’, or ‘LDI A,[HL]’.
		}

		public override IAsmLine Reparse()
		{
			Into = CSharp.ReplaceDataTypesInString(Into);
			From = CSharp.ReplaceDataTypesInString(From);

			for (var i = 1; i < 10; i++)
			{
				Into = Into.Replace($"\\{i}", $"args[{i - 1}]");
				From = From.Replace($"\\{i}", $"args[{i - 1}]");
			}

			return base.Reparse();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', tabCount)).Append("Load(").Append(Into).Append(", ").Append(From).Append(");").AppendComment(Comment);
		}
	}
}
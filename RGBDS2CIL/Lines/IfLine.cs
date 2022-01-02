using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class IfLine : CodeLine, IAsmLine
	{
		public string Condition;
		public bool IsElseIf { get; set; }
		public List<IAsmLine> Lines { get; set; } = new();

		public IfLine(CodeLine codeLine, bool isElseIf) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			IsElseIf = isElseIf;
			Condition = codeLine.Code[(isElseIf ? "ELIF" : "IF").Length..].Trim();
		}

		public override IAsmLine Reparse()
		{
			for (var i = 1; i < 10; i++)
			{
				Condition = Condition.Replace($"\\{i}", $"args[{i - 1}]");
			}

			Code = CSharp.ReplaceDataTypesInString(Code.Replace("_NARG", "args.Length"));
			Condition = CSharp.ReplaceDataTypesInString(Condition.Replace("_NARG", "args.Length"));

			return base.Reparse();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			var ifElse = IsElseIf ? "else if" : "if";

			if (IsElseIf)
				sb.Append(new string('\t', --tabCount)).AppendLine("}");

			sb.Append(new string('\t', tabCount)).Append(ifElse).Append(" (").Append(Condition).Append(')').AppendComment(Comment);
			sb.Append(new string('\t', tabCount)).AppendLine("{");
			
			foreach (var lineLine in Lines.Select(x => x.Reparse()))
			{
				lineLine.OutputLine(sb, tabCount + 1);
			}
		}
	}
}
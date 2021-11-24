using System.Collections.Generic;
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
				Condition = Condition.Replace($"\\{i}", $"args[{i}]");
			}

			Code = Code.Replace("_NARG", "args.Length");
			Condition = Condition.Replace("_NARG", "args.Length");

			return this;
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			tabCount = If.ProcessIf(sb, tabCount, this);
		}
	}
}
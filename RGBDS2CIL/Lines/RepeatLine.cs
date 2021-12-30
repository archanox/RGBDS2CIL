using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class RepeatLine : CodeLine, IAsmLine
	{
		public string Repeat;
		public List<IAsmLine> Lines { get; set; } = new();

		public RepeatLine(CodeLine codeLine, string v) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Repeat = codeLine.Code.Trim()[v.Length..].Trim();
		}

		public override IAsmLine Reparse()
		{
			for (var i = 1; i < 10; i++)
			{
				Repeat = Repeat.Replace($"\\{i}", $"args[{i - 1}]");
			}

			Code = Code.Replace("_NARG", "args.Length");
			Repeat = Repeat.Replace("_NARG", "args.Length");

			return this;
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			_ = this.Reparse();

			//TODO: support for loops
			//FOR n, 1, NUM_SPRITESTATEDATA_STRUCTS

			//FOR V, start, stop, step	
			//V goes from start to stop by step

			//FOR V, start, stop
			//V increments from start to stop

			//FOR V, stop
			//V increments from 0 to stop

			sb.Append(new string('\t', tabCount)).Append($"for (int i = 0; i < {Repeat}; i++)").AppendComment(Comment);
			sb.Append(new string('\t', tabCount)).AppendLine("{");

			foreach (var lineLine in Lines.Select(x => x.Reparse()))
			{
				lineLine.OutputLine(sb, tabCount + 1);
			}
		}
	}
}
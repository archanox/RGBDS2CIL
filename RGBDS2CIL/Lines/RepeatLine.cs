using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class RepeatLine : CodeLine, IAsmLine
	{
		public string Repeat;

		public string RepeatType { get; }
		public List<IAsmLine> Lines { get; set; } = new();

		public RepeatLine(CodeLine codeLine, string v) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Repeat = codeLine.Code.Trim()[v.Length..].Trim();
			RepeatType = v.ToUpper();
		}

		public override IAsmLine Reparse()
		{
			for (var i = 1; i < 10; i++)
			{
				Repeat = Repeat.Replace($"\\{i}", $"args[{i - 1}]");
			}

			Repeat = CSharp.ReplaceDataTypesInString(Repeat.Replace("_NARG", "args.Length"));

			return base.Reparse();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			//TODO: support for loops
			//FOR n, 1, NUM_SPRITESTATEDATA_STRUCTS
			//FOR n, (NUM_TM_HM + 7) / 8

			//FOR V, start, stop, step	
			//V goes from start to stop by step

			//FOR V, start, stop
			//V increments from start to stop

			//FOR V, stop
			//V increments from 0 to stop

			sb.Append(new string('\t', tabCount)).Append("for (var ");
			if (RepeatType.ToUpper() == "FOR")
			{
				var parameters = Parser.GetParameters(Repeat);
				var variableName = parameters.First();
				
				sb.Append(variableName).Append(" = ");
				if (parameters.Count == 2)
				{
					//n, (NUM_TM_HM + 7) / 8
					//equal or less than?
					sb.Append("0; ").Append(variableName).Append(" < ").Append(parameters.Last()).Append("; ").Append(variableName).Append("++)");
				}
				else if (parameters.Count == 3)
				{
					//n, 1, NUM_TMS + 1
					sb.Append(parameters[1]).Append("; ").Append(variableName).Append(" < ").Append(parameters.Last()).Append("; ").Append(variableName).Append("++)");
				}
				else //if (parameters.Count == 4)
				{
					Debugger.Break();
				}
			}
			else
			{
				sb.Append($"i = 0; i < {Repeat}; i++)");
			}
			sb.AppendComment(Comment);
			sb.Append(new string('\t', tabCount)).AppendLine("{");

			foreach (var lineLine in Lines.Select(x => x.Reparse()))
			{
				lineLine.OutputLine(sb, tabCount + 1);
			}
		}
	}
}
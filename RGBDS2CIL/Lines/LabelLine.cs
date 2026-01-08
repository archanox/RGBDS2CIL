using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class LabelLine : CodeLine, IAsmLine
	{
		public bool IsGlobal { get; set; }
		public bool HasExport { get; set; }
		public string LabelName;

		public LabelLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			IsGlobal = base.Code.Split(':').Last() == string.Empty;

			HasExport = base.Code.EndsWith("::");

			LabelName = base.Code.Trim().Split('.').Last().Trim(':');
			if (string.IsNullOrWhiteSpace(LabelName))
				LabelName = base.Code.Trim().Split(':')[0];
		}

		public override void OutputLine(StringBuilder sb, int tabCount)
		{
			// Output label as a comment since labels are not valid standalone statements in C#
			// TODO: Convert labels to actual methods when we have full context about their usage
			sb.Append(new string('\t', tabCount)).Append("// Label: ").Append(LabelName).AppendComment(Comment);
		}
	}
}
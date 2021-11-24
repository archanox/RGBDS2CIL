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

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(LabelName).Append(':').AppendComment(Comment);
		}
	}
}
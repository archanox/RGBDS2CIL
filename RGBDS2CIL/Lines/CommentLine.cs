using System;
using System.Text;

namespace RGBDS2CIL
{
	public class CommentLine : IAsmLine
	{
		public CommentLine(string raw, string comment, string fileName, int line)
		{
			Comment = comment;
			Raw = raw;
			FileName = fileName;
			Line = line;
		}
		public Guid LineId { get; set; } = Guid.NewGuid();
		public string Raw { get; set; }
		public string Comment { get; set; }
		public string FileName { get; set; }
		public int Line { get; set; }
		public IAsmLine Reparse() => this;

		public void OutputLine(StringBuilder sb, int tabCount)
		{
			if (string.IsNullOrWhiteSpace(Comment))
				sb.AppendLine();
			else
				sb.Append(new string('\t', tabCount)).Append("// ").AppendLine(Comment);
		}
	}
}
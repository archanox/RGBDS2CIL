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

		public string Raw { get; set; }
		public string Comment { get; set; }
		public string FileName { get; set; }
		public int Line { get; set; }
		public IAsmLine Reparse()
		{
			return this;
		}

		public void OutputLine(StringBuilder sb, int tabCount)
		{
			if (string.IsNullOrWhiteSpace(Comment))
				sb.AppendLine();
			else
				sb.Append(new string('\t', tabCount)).Append("// ").AppendLine(Comment);
		}
	}
}
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
	}
}
using System.Collections.Generic;
using System.Linq;

namespace RGBDS2CIL
{
	public class IncludeLine : CodeLine
	{
		public readonly string IncludeFile;
		public bool IsBinary { get; set; }
		public byte[] Binary { get; set; }
		public List<IAsmLine> Lines { get; set; } = new();

		public IncludeLine(CodeLine codeLine, bool isBinary) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			IncludeFile = Parser.GetStrings(codeLine.Code).Single().TrimStart('"').TrimEnd('"');
			IsBinary = isBinary;
		}
	}
}
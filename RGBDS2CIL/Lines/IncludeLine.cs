using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class IncludeLine : CodeLine, IAsmLine
	{
		public readonly string IncludeFile;
		public bool IsBinary { get; set; }
		public byte[] Binary { get; set; }
		public List<IAsmLine> Lines { get; set; } = new();

		public IncludeLine(CodeLine codeLine, bool isBinary) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			IncludeFile = Parser.GetStrings(codeLine.Code).Single().TrimStart('"').TrimEnd('"');
			IsBinary = isBinary;
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			if(IsBinary)
				sb.Append(new string('\t', tabCount)).Append("/* ").Append(Code).Append(" */").AppendComment(Comment);
		}
	}
}
using System;
using System.Collections.Generic;

namespace RGBDS2CIL
{
	public class CodeLine : IAsmLine
	{
		public string Code;
		public List<string> Strings;
		public string Raw { get; set; }
		public string Comment { get; set; }
		public string FileName { get; set; }
		public int Line { get; set; }

		public virtual IAsmLine Reparse()
		{
			//throw new NotImplementedException();
			Console.Error.WriteLine(this.GetType().FullName + " not re-parsed!");
			return this;
		}

		public CodeLine(string code, CodeLine codeLine, List<string> strings)
		{
			Code = code;
			//Strings = codeLine.Strings;
			Comment = codeLine.Comment;
			Raw = codeLine.Raw;
			FileName = codeLine.FileName;
			Strings = strings;
			Line = codeLine.Line;
		}

		public CodeLine(string code, string raw, string comment, string fileName, int line, List<string> strings)
		{
			Code = code;
			Strings = strings;
			Comment = comment;
			Raw = raw;
			FileName = fileName;
			Line = line;
		}
	}
}
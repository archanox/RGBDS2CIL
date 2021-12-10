using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RGBDS2CIL
{
	public class CodeLine : IAsmLine
	{
		public Guid LineId { get; set; } = Guid.NewGuid();
		public string Code;
		public readonly List<string> Strings;
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

		public void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', tabCount)).Append("/* ").Append(Code).Append(" */").AppendComment(Comment);
			//throw new NotImplementedException($"Cannot output unimplemented {this.GetType().FullName}.");
			//Debug.WriteLine(this.GetType().FullName);
			//throw new NotSupportedException($"{this.GetType().FullName} is currently unsupported.");
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

		protected CodeLine(string code, IAsmLine codeLine, List<string> strings) : this(code, codeLine.Raw, codeLine.Comment, codeLine.FileName, codeLine.Line, strings)
		{
		}
	}
}
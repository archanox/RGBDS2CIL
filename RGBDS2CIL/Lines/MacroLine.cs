using System;
using System.Collections.Generic;

namespace RGBDS2CIL
{
	public class MacroLine : CodeLine
	{
		public MacroLine(CodeLine codeLine, string name) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			IsLocal = name.StartsWith('.');

			Name = name;
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}

		public bool IsLocal { get; set; }

		public string Name { get; set; }
		public List<IAsmLine> Lines { get; set; } = new();
	}
}
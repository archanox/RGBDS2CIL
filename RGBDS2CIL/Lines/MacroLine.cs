using System;
using System.Collections.Generic;
using System.Text;

namespace RGBDS2CIL
{
	public class MacroLine : CodeLine, IAsmLine
	{
		public MacroLine(CodeLine codeLine, string name) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			IsLocal = name.StartsWith('.');

			Name = name;
		}

		public bool IsLocal { get; set; }

		public string Name { get; set; }
		public List<IAsmLine> Lines { get; set; } = new();

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			tabCount = Macro.ProcessMacro(sb, tabCount, this);
		}
	}
}
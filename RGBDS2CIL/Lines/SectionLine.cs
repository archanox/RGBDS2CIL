using System;
using System.Linq;

namespace RGBDS2CIL
{
	public class SectionLine : CodeLine
	{
		public string SectionName;
		public string Section;

		public SectionLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			var strings = Parser.GetStrings(codeLine.Code);
			SectionName = strings.Single().TrimStart('"').TrimEnd('"');
			Section = base
				.Code[(base.Code.IndexOf(strings.Single(), StringComparison.Ordinal) + strings.Single().Length)..]
				.Trim().TrimStart(',').Trim();
		}
	}
}
using System;
using System.Linq;

namespace RGBDS2CIL
{
	/// <summary>
	/// Format:
	/// <list type="table">
	/// <item> SECTION name, type[addr], options </item>
	/// </list>
	/// Examples:
	/// <example>
	/// <code>SECTION "OAM Data",WRAM0,ALIGN[8] ; align to 256 bytes</code>
	/// <code>SECTION "VRAM Data",ROMX,BANK[2],ALIGN[4] ; align to 16 bytes</code>
	/// </example>
	/// </summary>
	/// <see href="https://rgbds.gbdev.io/docs/master/rgbasm.5#SECTIONS"/>
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
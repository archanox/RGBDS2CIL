using System.Linq;

namespace RGBDS2CIL
{
	public class LoadLine : CodeLine
	{
		public string From;
		public string Into;

		public LoadLine(CodeLine codeLine, string instruction) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			var values = codeLine.Code.Trim()[instruction.Length..];
			Into = values.Split(',')[0].Trim();
			From = values.Split(',').Last().Trim();

			//This is sometimes written as ‘LDIO A,[C]’, or ‘LD A,[$FF00+C]’.
			//This is sometimes written as ‘LD [HL+],A’, or ‘LDI [HL],A’.
			//This is sometimes written as ‘LD [HL-],A’, or ‘LDD [HL],A’.
			//This is sometimes written as ‘LD A,[HL-]’, or ‘LDD A,[HL]’.
			//This is sometimes written as ‘LD A,[HL+]’, or ‘LDI A,[HL]’.
		}
	}
}
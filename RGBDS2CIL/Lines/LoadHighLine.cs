using System.Linq;

namespace RGBDS2CIL
{
	public class LoadHighLine : CodeLine
	{
		public string From;
		public string Into;

		public LoadHighLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			var values = codeLine.Code.Trim()["LDH".Length..];
			Into = values.Split(',')[0].Trim();
			From = values.Split(',').Last().Trim();
		}
	}
}
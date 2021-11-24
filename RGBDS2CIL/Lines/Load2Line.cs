using System.Linq;

namespace RGBDS2CIL
{
	public class Load2Line : CodeLine
	{
		public string Label { get; set; }
		public string Location { get; set; }

		public Load2Line(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Label = codeLine.Strings.Single().TrimStart('"').TrimEnd('"');
			Location = codeLine.Code.Split(',').Last().Trim();
		}
	}
}
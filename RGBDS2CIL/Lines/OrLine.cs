using System.Linq;

namespace RGBDS2CIL
{
	public class OrLine : CodeLine
	{
		public string From;
		public string Value;

		public OrLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			var values = codeLine.Code.Trim()["OR".Length..].Trim();
			var split = values.Split(',');

			From = "A"; //default?
			if (split.Length == 2)
			{
				From = split[0].Trim();
				Value = split.Last().Trim();
			}
			else
				Value = split.Single().Trim();
		}
	}
}
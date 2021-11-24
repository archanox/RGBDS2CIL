using System.Linq;

namespace RGBDS2CIL
{
	public class BitLine : CodeLine
	{
		public string From;
		public string Value;

		public BitLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			var values = codeLine.Code.Trim()["BIT".Length..].Trim();
			var split = values.Split(',');

			From = "u3"; //default?
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
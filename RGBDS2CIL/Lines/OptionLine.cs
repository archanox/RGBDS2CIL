using System.Collections.Generic;
using System.Linq;

namespace RGBDS2CIL
{
	public class OptionLine : CodeLine
	{
		public Dictionary<string, string> Options { get; set; }

		public OptionLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			var options = codeLine.Code["OPT".Length..].Trim().Split(',');

			Options = options.ToDictionary(x => x.Split('.')[0].Trim(), x => x.Split('.').Last().Trim());
		}
	}
}
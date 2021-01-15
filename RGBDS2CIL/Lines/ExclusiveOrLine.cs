using System.Linq;

namespace RGBDS2CIL
{
	public class ExclusiveOrLine : CodeLine
	{
		public string From;
		public string Value;

		public ExclusiveOrLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			var values = codeLine.Code.Trim()["XOR".Length..].Trim();
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
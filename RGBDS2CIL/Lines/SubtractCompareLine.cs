using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RGBDS2CIL
{
	public class SubtractCompareLine : CodeLine
	{
		public string From;
		public string Value;

		public SubtractCompareLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			var values = codeLine.Code.Trim()["CP".Length..].Trim();

			//remove function params
			const string regexString = "\\(.*?\\)|(,)";

			var rx = new Regex(regexString, RegexOptions.Compiled | RegexOptions.IgnoreCase);

			var matches = rx.Matches(values);

			var valueToTest = values;
			foreach (Match match in matches)
			{
				valueToTest = valueToTest.Replace(match.Value, "");
			}

			var split = valueToTest.Split(',');

			From = "A"; //default?
			if (split.Length == 2)
			{
				From = split[0].Trim();
				Value = split.Last().Trim();
			}
			else
				Value = values;
		}
	}
}
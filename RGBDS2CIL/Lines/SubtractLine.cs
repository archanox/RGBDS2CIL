using System;
using System.Linq;

namespace RGBDS2CIL
{
	public class SubtractLine : CodeLine
	{
		public string From;
		public string Value;
		public bool CarryFlag { get; set; }

		public SubtractLine(CodeLine codeLine, bool carryFlag) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
			CarryFlag = carryFlag;
			var split = codeLine.Code["SUB".Length..].Trim().Split(','); //SUB or SBC

			From = "A"; //default?
			if (split.Length == 2)
			{
				From = split[0].Trim();
				Value = split.Last().Trim();
			}
			else if (split.Length == 1)
				Value = split.Single().Trim();
			else
				Console.Error.WriteLine(codeLine.Code["SUB".Length..].Trim());
		}
	}
}
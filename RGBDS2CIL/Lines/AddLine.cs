using System.Linq;

namespace RGBDS2CIL
{
	public class AddLine : CodeLine
	{
		public string From;
		public string Value;
		public bool CarryFlag { get; set; }

		public AddLine(CodeLine codeLine, bool carryFlag) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
			CarryFlag = carryFlag;
			var split = codeLine.Code["ADD".Length..].Trim().Split(','); //ADD or ADC

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
using System.Linq;

namespace RGBDS2CIL
{
	public class DeclareSpaceLine : CodeLine
	{
		public string Count;
		public string Value;

		public DeclareSpaceLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			var parameters = codeLine.Code["DS".Length..].Split(',');

			if (parameters.Length == 1)
			{
				Count = parameters.Single().Trim();
			}
			else
			{
				Count = parameters[0].Trim();
				Value = parameters.Last().Trim();
			}
		}
	}
}
using System.Linq;

namespace RGBDS2CIL
{
	public class RelativeJumpLine : CodeLine
	{
		public string Condition;
		public string Offset;

		public RelativeJumpLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			var parameters = codeLine.Code["JR".Length..].Split(',');

			if (parameters.Length == 1)
			{
				Offset = parameters.Single().Trim();
			}
			else
			{
				Condition = parameters[0].Trim();
				Offset = parameters.Last().Trim();
			}
		}
	}
}
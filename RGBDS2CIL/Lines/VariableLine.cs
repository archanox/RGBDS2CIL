using System.Linq;

namespace RGBDS2CIL
{
	public class VariableLine : CodeLine
	{
		public string VariableName;
		public string VariableValue;

		public VariableLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			VariableName = codeLine.Code.Trim().Split()[0];
			VariableValue = codeLine.Code.Trim().Split().Last();
		}
	}
}
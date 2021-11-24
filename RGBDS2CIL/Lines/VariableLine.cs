using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class VariableLine : CodeLine, IAsmLine
	{
		public string VariableName;
		public string VariableValue;

		public VariableLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			VariableName = codeLine.Code.Trim().Split()[0];
			VariableValue = codeLine.Code.Trim().Split().Last();
		}

		public void OutputLine(StringBuilder sb, int tabCount)
		{
			sb
				.Append(new string('\t', tabCount))
				.Append("var ")
				.Append(VariableName)
				.Append(" = ")
				.Append(VariableValue)
				.Append(';')
				.AppendComment(Comment);
		}
	}
}
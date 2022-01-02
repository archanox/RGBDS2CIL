using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class AssertLine : CodeLine, IAsmLine
	{
		public string Condition;
		public string Message;

		public AssertLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			var parameters = base.Code.Substring(base.Code.ToUpper().IndexOf("ASSERT", StringComparison.OrdinalIgnoreCase) + "ASSERT".Length).Trim();

			var splitParameters = Parser.GetParameters(parameters);

			Condition = splitParameters?.First();
			if(splitParameters?.Count == 2)
				Message = splitParameters.LastOrDefault();

			Debug.Assert(splitParameters?.Count <= 2, "More than 2 parameters for an ASSERT are unsupported.");
		}

		public override IAsmLine Reparse()
		{
			for (var i = 1; i < 10; i++)
			{
				Condition = Condition.Replace($"\\{i}", $"args[{i - 1}]");
				Message = Message?.Replace($"\\{i}", $"args[{i - 1}]");
			}

			Condition = CSharp.ReplaceDataTypesInString(Condition);
			Message = CSharp.ReplaceDataTypesInString(Message);

			return base.Reparse();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', tabCount)).Append("Debug.Assert(").Append(Condition);

			if (!string.IsNullOrWhiteSpace(Message))
				sb.Append(", ").Append(Message);

			sb.Append(");").AppendComment(Comment);
		}
	}
}
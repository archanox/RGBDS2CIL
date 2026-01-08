using System;
using System.Text;

namespace RGBDS2CIL
{
	/// <summary>
	/// Handles DEF statements in RGBDS assembly
	/// DEF statements are used to define or modify variables
	/// Examples: DEF const_value = 0, DEF const_value += const_inc
	/// </summary>
	public class DefLine : CodeLine, IAsmLine
	{
		public string VariableName { get; set; }
		public string Operator { get; set; }  // "=", "+=", "-=", etc.
		public string Expression { get; set; }

		public DefLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			// Parse the DEF statement
			// Format: DEF variable_name = expression
			// or: DEF variable_name += expression
			var defCode = codeLine.Code.Trim();
			if (defCode.StartsWith("DEF ", StringComparison.OrdinalIgnoreCase))
			{
				defCode = defCode.Substring("DEF ".Length).Trim();
			}

			// Find the operator - check in order from longest to shortest to avoid matching substrings
			var operatorIndex = -1;
			string[] operators = { "+=", "-=", "*=", "/=", "=" };
			foreach (var op in operators)
			{
				operatorIndex = defCode.IndexOf(op);
				if (operatorIndex >= 0)
				{
					Operator = op;
					break;
				}
			}

			if (operatorIndex >= 0)
			{
				VariableName = defCode.Substring(0, operatorIndex).Trim();
				Expression = defCode.Substring(operatorIndex + Operator.Length).Trim();
			}
			else
			{
				// Fallback if no operator found
				VariableName = defCode;
				Operator = "=";
				Expression = "0";
			}
		}

		public override IAsmLine Reparse()
		{
			// Replace macro arguments in the expression
			for (var i = 1; i < 10; i++)
			{
				Expression = Expression.Replace($"\\{i}", $"args[{i - 1}]");
				VariableName = VariableName.Replace($"\\{i}", $"args[{i - 1}]");
			}

			Expression = Expression.Replace("_NARG", "args.Length");
			Expression = CSharp.ReplaceDataTypesInString(Expression);
			
			return base.Reparse();
		}

		public override void OutputLine(StringBuilder sb, int tabCount)
		{
			// Output as a C# variable assignment
			sb.Append(new string('\t', tabCount)).Append($"{VariableName} {Operator} {Expression};").AppendComment(Comment);
		}
	}
}

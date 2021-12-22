using System;
using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class ConstantLine : CodeLine, IAsmLine
	{
		public string ConstantName;
		public string ConstantValue;
		public string ConstType;
		public ConstantType ConstantValueType { get; set; }

		public ConstantLine(CodeLine codeLine, string constType) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			ConstType = constType.ToUpper();
			ConstantName = codeLine.Code.Trim().Split()[0];

			var constString = codeLine.Strings?.SingleOrDefault()?.TrimStart('"').TrimEnd('"');

			if (!string.IsNullOrWhiteSpace(constString) && constType == "EQUS")
				ConstantValue = $"\"{constString}\"";
			else
				ConstantValue = codeLine.Code[(codeLine.Code.ToUpper().IndexOf($" {constType} ") + constType.Length + 1)..].Trim();

			if (ConstantValue.StartsWith('$'))
				ConstantValueType = ConstantType.Hexadecimal;
			else if (ConstantValue.StartsWith('&'))
				ConstantValueType = ConstantType.Octal;
			else if (ConstantValue.StartsWith('%'))
				ConstantValueType = ConstantType.Binary;
			else if (ConstantValue.StartsWith('"'))
				ConstantValueType = ConstantType.String;
			else if (ConstantValue.StartsWith('`'))
				ConstantValueType = ConstantType.Graphics;
			else if (ConstantValue.Contains('.'))
				ConstantValueType = ConstantType.FixedPoint;
			else
				ConstantValueType = ConstantType.Decimal;
		}

		public override IAsmLine Reparse()
		{
			for (var i = 1; i < 10; i++)
			{
				ConstantName = ConstantName.Replace($"\\{i}", $"args[{i - 1}]");
				ConstantValue = ConstantValue.Replace($"\\{i}", $"args[{i - 1}]");
			}

			return this;
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			var value = ConstantValue;
			var valueType = "int";

			switch (ConstantValueType)
			{
				case ConstantType.Hexadecimal:
					value = value.TrimStart('$').Insert(0, "0x");
					break;
				case ConstantType.Binary:
					value = value.TrimStart('%').Insert(0, "0b");
					break;
				case ConstantType.Octal:
					value = $"Convert.ToInt32(\"{value.TrimStart('%')}\", 8)";
					break;
				case ConstantType.String:
					valueType = "string";
					break;
				case ConstantType.Decimal:
					valueType = "double";
					break;
				case ConstantType.FixedPoint: //todo: must be fixed point
					valueType = "decimal";
					break;
				case ConstantType.Graphics:
					break;
				default:
					throw new ArgumentOutOfRangeException(ConstantValueType.ToString(),
						"Unknown ConstantValueType");
			}

			sb
				.Append(new string('\t', tabCount))
				.Append("const ")
				.Append(valueType)
				.Append(' ')
				.Append(ConstantName)
				.Append(" = ")
				.Append(value)
				.Append(';')
				.AppendComment(Comment);
		}
	}
}
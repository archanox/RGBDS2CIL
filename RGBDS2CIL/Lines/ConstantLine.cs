using System;
using System.Collections.Generic;
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

			//NOTE: will fail on nested strings
			var constString = codeLine.Strings?.SingleOrDefault()?.TrimStart('"').TrimEnd('"');

			if (!string.IsNullOrWhiteSpace(constString) && constType == "EQUS")
				ConstantValue = $"\"{constString}\"";
			else
				ConstantValue = codeLine.Code[(codeLine.Code.ToUpper().IndexOf($"{constType}") + constType.Length)..].Trim();

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

			//if it's a number
			if (ConstType == "EQU")
			{
				//https://rgbds.gbdev.io/docs/v0.5.2/rgbasm.5#Operators
				//pad out the +-*/%~
				value = value
					.Replace("+", " + ")
					.Replace("*", " * ")
					.Replace("-", " - ")
					.Replace("/", " / ");

				var newValues = new List<string>();
				foreach (var splitValue in value.Split(' '))
				{
					if (splitValue.StartsWith('$'))
						newValues.Add(splitValue.TrimStart('$').Insert(0, "0x"));
					else if (splitValue.StartsWith('%'))
						newValues.Add(splitValue.TrimStart('%').Insert(0, "0b"));
					else if (splitValue.StartsWith('&'))
						newValues.Add($"Convert.ToInt32(\"{splitValue.TrimStart('%')}\", 8)");
					else
						newValues.Add(splitValue);
				}

				value = string.Join(' ', newValues);
			}

			switch (ConstantValueType)
			{
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
					//how to do?
					System.Diagnostics.Debugger.Break();
					break;
				default:
					throw new ArgumentOutOfRangeException(ConstantValueType.ToString(), "Unknown ConstantValueType");
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
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
		public bool IsRedefined = false;
		public ConstantType ConstantValueType { get; set; }

		public ConstantLine(CodeLine codeLine, string constType) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			ConstType = constType.ToUpper();
			

			//NOTE: will fail on nested strings
			var constString = codeLine.Strings?.SingleOrDefault()?.TrimStart('"').TrimEnd('"');

			//DEF s EQUS "Hello, "
			//REDEF s EQUS "{s}world!"

			//TODO: Support REDEF
			if (codeLine.Code.ToUpper().StartsWith("REDEF")) 
			{
				IsRedefined = true;
				ConstantName = codeLine.Code[(codeLine.Code.ToUpper().IndexOf("REDEF") + 5)..].Trim().Split()[0];
			}
			else
				ConstantName = codeLine.Code.Trim().Split()[0];

			if (!string.IsNullOrWhiteSpace(constString) && constType == "EQUS")
				ConstantValue = $"\"{constString}\"";
			else
				ConstantValue = codeLine.Code[(codeLine.Code.ToUpper().IndexOf(constType) + constType.Length)..].Trim();

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

			ConstantName = CSharp.ReplaceDataTypesInString(ConstantName);
			ConstantValue = CSharp.ReplaceDataTypesInString(ConstantValue);

			return base.Reparse();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			var value = ConstantValue;
			var valueType = "int";

			//if it's a number
			if (ConstType == "EQU")
			{
				value = CSharp.ReplaceDataTypesInString(value);
			}

			if (ConstantValueType == ConstantType.String)
				valueType = "string";
			else if (ConstantValueType == ConstantType.Decimal)
				valueType = "double";
			else if (ConstantValueType == ConstantType.FixedPoint) //todo: must be fixed point
				valueType = "decimal";
			else if (ConstantValueType == ConstantType.Graphics)
				System.Diagnostics.Debugger.Break();

			sb.Append(new string('\t', tabCount));

			if (!IsRedefined) 
			{
				sb.Append("const ").Append(valueType).Append(' ');
			}

			sb.Append(ConstantName).Append(" = ").Append(value).Append(';').AppendComment(Comment);
		}
	}
}
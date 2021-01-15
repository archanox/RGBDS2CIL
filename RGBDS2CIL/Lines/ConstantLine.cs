using System.Linq;

namespace RGBDS2CIL
{
	public class ConstantLine : CodeLine
	{
		public string ConstantName;
		public string ConstantValue;
		public string ConstType;
		public ConstantType ConstantValueType { get; set; }

		public ConstantLine(CodeLine codeLine, string constType) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
			this.ConstType = constType;
			ConstantName = codeLine.Code.Trim().Split()[0];

			var constString = codeLine.Strings?.SingleOrDefault()?.TrimStart('"').TrimEnd('"');

			if (!string.IsNullOrWhiteSpace(constString) && constType == "EQUS")
				ConstantValue = $"\"{constString}\"";
			else
				ConstantValue = codeLine.Code[(codeLine.Code.IndexOf(constType) + constType.Length)..].Trim();

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
				this.ConstantName = this.ConstantName.Replace($"\\{i}", $"args[{i - 1}]");
				this.ConstantValue = this.ConstantValue.Replace($"\\{i}", $"args[{i - 1}]");
			}

			return this;
		}
	}
}
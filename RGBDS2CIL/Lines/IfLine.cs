using System.Collections.Generic;

namespace RGBDS2CIL
{
	public class IfLine : CodeLine, IAsmLine
	{
		public string Condition;
		public bool IsElseIf { get; set; }
		public List<IAsmLine> Lines { get; set; } = new();

		public IfLine(CodeLine codeLine, bool isElseIf) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
			IsElseIf = isElseIf;
			if (isElseIf)
				this.Condition = codeLine.Code["ELIF".Length..].Trim();
			else
				this.Condition = codeLine.Code["IF".Length..].Trim();
		}

		public override IAsmLine Reparse()
		{
			for (var i = 1; i < 10; i++)
			{
				this.Condition = this.Condition.Replace($"\\{i}", $"args[{i}]");
			}

			this.Code = this.Code.Replace("_NARG", "args.Length");
			this.Condition = this.Condition.Replace("_NARG", "args.Length");

			return this;
		}
	}
}
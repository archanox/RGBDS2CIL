using System;
using System.Collections.Generic;

namespace RGBDS2CIL
{
	public class DefineLine : CodeLine
	{
		public List<string> Parameters { get; set; }
		public Type DefineType { get; set; }

		public DefineLine(CodeLine codeLine, Type type) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
			DefineType = type;

			var parameters = codeLine.Code[codeLine.Code.Split()[0].Length..].Trim();
			this.Parameters = Parser.GetParameters(parameters);
		}
	}
}
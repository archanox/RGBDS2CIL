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
			DefineType = type;

			var parameters = codeLine.Code[codeLine.Code.Split()[0].Length..].Trim();
			Parameters = Parser.GetParameters(parameters);
		}
	}
}
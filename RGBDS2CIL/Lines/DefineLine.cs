using System;
using System.Collections.Generic;
using System.Text;

namespace RGBDS2CIL
{
	/// <summary>
	/// https://rgbds.gbdev.io/docs/master/rgbasm.5#Defining_constant_data_in_ROM
	/// </summary>
	public class DefineLine : CodeLine, IAsmLine
	{
		public List<string> Parameters { get; set; }
		public Type DefineType { get; set; }

		public DefineLine(CodeLine codeLine, Type type) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			DefineType = type;

			var parameters = codeLine.Code[codeLine.Code.Split()[0].Length..].Trim();
			Parameters = Parser.GetParameters(parameters);
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', tabCount)).Append("Define(");

			for (var i = 0; i < Parameters.Count; i++)
			{
				sb.Append(Parameters[i]);
				if (i < Parameters.Count - 1)
					sb.Append(", ");
			}
			sb.Append(");").AppendComment(Comment);
		}
	}
}
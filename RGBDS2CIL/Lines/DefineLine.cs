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

		public override IAsmLine Reparse()
		{
			for (var j = 0; j < Parameters.Count; j++)
			{
				Parameters[j] = CSharp.ReplaceDataTypesInString(Parameters[j]);

				for (var i = 1; i < 10; i++)
				{
					Parameters[j] = Parameters[j].Replace($"\\{i}", $"args[{i - 1}]");
				}
			}

			return base.Reparse();
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			sb.Append(new string('\t', tabCount)).Append($"Define(typeof({DefineType}), ");

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
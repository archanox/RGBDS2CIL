using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace RGBDS2CIL
{
	internal class MacroCallLine : CodeLine, IAsmLine
	{
		public MacroCallLine(CodeLine codeLine, MacroLine macroLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Name = macroLine.Name.Trim(':');

			var parameters = codeLine.Code[Name.Length..].Trim();
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

		public string Name { get; set; }
		public List<string> Parameters { get; set; } = new();

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			var methodName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Name.Trim(':'));

			sb.Append(new string('\t', tabCount)).Append(methodName).Append('(');

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
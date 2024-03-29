﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public class MacroLine : CodeLine, IAsmLine
	{
		public MacroLine(CodeLine codeLine, string name) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			IsLocal = name.StartsWith('.');

			Name = name;
		}

		public bool IsLocal { get; set; }

		public string Name { get; set; }
		public List<IAsmLine> Lines { get; set; } = new();

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			//NOTE: Nested macros are prohibited
			var privatePublic = IsLocal ? "private " : "public ";
			var methodName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Name.Trim(':'));

			var argCount = 0;
			foreach (var macroLineLine in Lines.OfType<CodeLine>())
			{
				argCount = GetArgCount(macroLineLine, argCount);				
			}

			if (!string.IsNullOrWhiteSpace(Comment))
			{
				sb.Append(new string('\t', tabCount)).AppendLine("/// <summary>");
				sb.Append(new string('\t', tabCount)).Append("/// ").AppendLine(Comment);
				sb.Append(new string('\t', tabCount)).AppendLine("/// </summary>");
				for (var i = 0; i < argCount; i++)
				{
					sb.Append(new string('\t', tabCount)).Append("/// <param name=\"args[").Append(i).AppendLine("]\"></param>");
				}
				sb.Append(new string('\t', tabCount)).AppendLine("/// <returns></returns>");
			}

			sb.Append(new string('\t', tabCount)).Append(privatePublic).Append("void ").Append(methodName);

			sb.AppendLine(argCount == 0 ? "()" : "(params object[] args)");

			sb.Append(new string('\t', tabCount)).AppendLine("{");
			foreach (var lineLine in Lines.Select(x => x.Reparse()))
			{
				lineLine.OutputLine(sb, tabCount + 1);
			}
		}

		private static int GetArgCount(IAsmLine macroLineLine, int argCount)
		{
			if (macroLineLine is not CodeLine lineLine) return 0;

			for (var i = argCount; i < 10; i++)
			{
				if (!lineLine.Code.Contains($"\\{i}")) continue;
				argCount = i;
			}

			macroLineLine.Reparse();

			if (macroLineLine is IfLine ifLine)
				argCount = ifLine.Lines.Aggregate(argCount, (current, ifLineLine) => GetArgCount(ifLineLine, current));
			else if (macroLineLine is RepeatLine repeatLine)
				argCount = repeatLine.Lines.Aggregate(argCount, (current, ifLineLine) => GetArgCount(ifLineLine, current));

			return argCount;
		}
	}
}
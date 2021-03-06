﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public static class Macro
	{
		internal static int ProcessMacro(StringBuilder sb, int tabCount, MacroLine macroLine)
		{
			var ti = CultureInfo.CurrentCulture.TextInfo;
			var privatePublic = "";//macroLine.IsLocal ? "private " : "public ";
			var methodName = ti.ToTitleCase(macroLine.Name.Trim(':'));

			var argCount = 0;
			var linesToUpdate = new List<IAsmLine>();
			foreach (var macroLineLine in macroLine.Lines.OfType<CodeLine>())
			{
				argCount = ReplaceArgs(macroLineLine, argCount, linesToUpdate);
				if (macroLineLine is IfLine ifLine)
					foreach (var ifLineLine in ifLine.Lines)
					{
						argCount = ReplaceArgs(ifLineLine, argCount, linesToUpdate);
					}


			}

			if (!string.IsNullOrWhiteSpace(macroLine.Comment))
			{
				sb.Append(new string('\t', tabCount)).AppendLine("/// <summary>");
				sb.Append(new string('\t', tabCount)).Append("/// ").AppendLine(macroLine.Comment);
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
			tabCount++;
			foreach (var macroLineLine in macroLine.Lines)
			{
				var lineLine = macroLineLine.Reparse();
				CSharp.OutputCSharp(lineLine, sb, tabCount);
			}

			tabCount--;
			return tabCount;
		}

		private static int ReplaceArgs(IAsmLine macroLineLine, int argCount, List<IAsmLine> linesToUpdate)
		{
			var lineLine = macroLineLine.Reparse() as CodeLine;
			for (var i = 1; i < 10; i++)
			{
				if (!lineLine.Code.Contains($"\\{i}")) continue;
				argCount = i;
				lineLine.Code = lineLine.Code.Replace($"\\{i}", $"args[{i - 1}]");
				if (macroLineLine is ConstantLine constantLine)
				{
					constantLine.ConstantValue = constantLine.ConstantValue.Replace($"\\{i}", $"args[{i - 1}]");
					constantLine.ConstantName = constantLine.ConstantName.Replace($"\\{i}", $"args[{i - 1}]");
					lineLine = constantLine;
				}

				linesToUpdate.Add(lineLine);
			}

			return argCount;
		}
	}
}
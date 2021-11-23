using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RGBDS2CIL
{
	public static class Macro
	{
		internal static int ProcessMacro(StringBuilder sb, int tabCount, MacroLine macroLine)
		{
			//NOTE: Nested macros are prohibited
			var privatePublic = "";//macroLine.IsLocal ? "private " : "public ";
			var methodName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(macroLine.Name.Trim(':'));

			var argCount = 0;
			var linesToUpdate = new List<IAsmLine>();
			foreach (var macroLineLine in macroLine.Lines.OfType<CodeLine>())
			{
				argCount = ReplaceArgs(macroLineLine, argCount, linesToUpdate);
				if (macroLineLine is not IfLine ifLine) continue;
				argCount = ifLine.Lines.Aggregate(argCount, (current, ifLineLine) => ReplaceArgs(ifLineLine, current, linesToUpdate));
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
			foreach (var lineLine in macroLine.Lines.Select(macroLineLine => macroLineLine.Reparse()))
			{
				CSharp.OutputCSharp(lineLine, sb, tabCount);
			}

			tabCount--;
			return tabCount;
		}

		private static int ReplaceArgs(IAsmLine macroLineLine, int argCount, ICollection<IAsmLine> linesToUpdate)
		{
			if (macroLineLine.Reparse() is not CodeLine lineLine) return 0;

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
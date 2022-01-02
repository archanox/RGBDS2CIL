using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RGBDS2CIL
{
	public static class CSharp
	{
		public static string GenerateCsharp(string fileName, List<IAsmLine> parsedLines, string root)
		{
			var sb = new StringBuilder();

			var thisName = Regex.Replace(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Path.GetFileNameWithoutExtension(fileName)).Replace(" ", "").Replace('-', '_'), "[^A-Za-z0-9]", "");

			var includes = parsedLines.OfType<IncludeLine>().ToList();

			var usings = includes.Where(x => !x.IsBinary).Select(x => x.IncludeFile).Distinct().OrderByDescending(x => x.Length).ThenBy(x => x);
			foreach (var include in usings)
			{
				sb.Append("using ").Append(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Path.GetFileNameWithoutExtension(include.Replace('\\', '.').Replace('/', '.')))).AppendLine(";");
			}

			if (usings.Any())
				sb.AppendLine();

			foreach (var include in includes.Where(x => !x.IsBinary))
			{
				var includeFileName = Path.Combine(root, include.IncludeFile);
				var includedCSharp = GenerateCsharp(includeFileName, include.Lines, root);
				File.WriteAllText(includeFileName + ".cs", includedCSharp);
			}

			//TODO include binary

			var tabCount = 1;

			sb.Append("namespace ")
				.AppendLine(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(new DirectoryInfo(root).Name))
				.AppendLine("{")
				.Append(new string('\t', tabCount))
				.Append("public class ")
				.AppendLine(thisName)
				.Append(new string('\t', tabCount++))
				.AppendLine("{");

			foreach (var parsedLine in parsedLines)
			{
				parsedLine.Reparse().OutputLine(sb, tabCount);
			}

			sb.Append(new string('\t', 1)).AppendLine("}");
			sb.AppendLine("}");

			return sb.ToString();
		}

		public static string ReplaceDataTypesInString(string value)
		{
			if (string.IsNullOrWhiteSpace(value)) return null;
			if (value.StartsWith('"') && value.EndsWith('"')) return value;
			//https://rgbds.gbdev.io/docs/v0.5.2/rgbasm.5#Operators
			//pad out the +-*/%~
			value = value
				.Replace("+", " + ")
				.Replace("*", " * ")
				.Replace("-", " - ")
				.Replace("/", " / ")
				.Replace("  ", " ");

			var newValues = new List<string>();
			foreach (var splitValue in value.Split(' '))
			{
				if (splitValue.StartsWith('$'))
					newValues.Add(splitValue.TrimStart('$').Insert(0, "0x"));
				else if (splitValue.StartsWith('%'))
					newValues.Add(splitValue.TrimStart('%').Insert(0, "0b"));
				else if (splitValue.StartsWith('&') && splitValue != "&&" && splitValue.Length == 2)
					newValues.Add($"Convert.ToInt32(\"{splitValue.TrimStart('%')}\", 8)");
				else
					newValues.Add(splitValue);
			}

			value = string.Join(' ', newValues);
			return value;
		}
	}
}
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

			foreach (var include in includes.Where(x=>!x.IsBinary).Select(x=>x.IncludeFile).Distinct().OrderByDescending(x=>x.Length).ThenBy(x=>x))
			{
				sb.Append("using ").Append(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Path.GetFileNameWithoutExtension(include.Replace('\\', '.').Replace('/', '.')))).AppendLine(";");
			}

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
				.AppendLine(thisName)
				.AppendLine("{")
				.Append(new string('\t', tabCount))
				.Append("public class ")
				.AppendLine(thisName)
				.Append(new string('\t', tabCount++))
				.AppendLine("{");

			sb.Append(new string('\t', tabCount))
				.AppendLine("public static void Main()")
				.Append(new string('\t', tabCount++))
				.AppendLine("{");


			foreach (var parsedLine in parsedLines)
			{
				parsedLine.OutputLine(sb, tabCount);
			}

			sb.Append(new string('\t', 2)).AppendLine("}");
			sb.Append(new string('\t', 1)).AppendLine("}");
			sb.AppendLine("}");

			return sb.ToString();
		}
	}
}
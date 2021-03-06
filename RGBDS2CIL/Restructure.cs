using System.Collections.Generic;
using System.Linq;

namespace RGBDS2CIL
{
	public static class Restructure
	{
		internal static void RestructureMacros(List<IAsmLine> parsedLines)
		{
			var macrosToUpdate = new List<MacroLine>();
			var linesToRemove = new List<IAsmLine>();

			foreach (var macroLine in parsedLines.OfType<MacroLine>().Where(x => x.Lines.Count == 0).ToArray())
			{
				var macroContents = parsedLines
					.SkipWhile(x => x.Line < macroLine.Line)
					//.SkipWhile(x => x is not MacroLine)
					.TakeWhile(x => x is not EndMacroLine)
					.ToList();

				var endline = parsedLines[parsedLines.IndexOf(macroContents.Last()) + 1];

				var macro = macroContents.OfType<MacroLine>().First();
				macro.Lines = macroContents.Skip(1).ToList();
				macro.Lines.Add(endline);

				linesToRemove.AddRange(macro.Lines);

				macrosToUpdate.Add(macro);
			}

			parsedLines.RemoveAll(x =>
				linesToRemove.Select(x => x.Line).Contains(x.Line) &&
				linesToRemove.Select(x => x.FileName).Contains(x.FileName));

			foreach (var macroLine in macrosToUpdate)
			{
				var thisMacroLineIndex = parsedLines.FindIndex(x => x.Line == macroLine.Line && x.FileName == macroLine.FileName);
				Program.RestructureLines(macroLine.Lines);
				parsedLines[thisMacroLineIndex] = macroLine;
			}
		}

		internal static void RestructureIfs(List<IAsmLine> parsedLines)
		{
			var ifsToUpdate = new List<IfLine>();
			var linesToRemove = new List<IAsmLine>();

			foreach (var ifLine in parsedLines.OfType<IfLine>().Where(x => x.Lines.Count == 0 && !x.IsElseIf).ToArray())
			{
				var macroContents = parsedLines
					.SkipWhile(x => x.Line < ifLine.Line)
					//.SkipWhile(x => x is not MacroLine)
					.TakeWhile(x => x is not EndConditionLine)
					.ToList();

				var endline = parsedLines[parsedLines.IndexOf(macroContents.Last()) + 1];

				var macro = macroContents.OfType<IfLine>().First();
				macro.Lines = macroContents.Skip(1).ToList();
				macro.Lines.Add(endline);

				linesToRemove.AddRange(macro.Lines);

				ifsToUpdate.Add(macro);
			}

			parsedLines.RemoveAll(x =>
				linesToRemove.Select(x => x.Line).Contains(x.Line) &&
				linesToRemove.Select(x => x.FileName).Contains(x.FileName));

			foreach (var ifToUpdate in ifsToUpdate)
			{
				var thisMacroLineIndex = parsedLines.FindIndex(x => x.Line == ifToUpdate.Line && x.FileName == ifToUpdate.FileName);
				Program.RestructureLines(ifToUpdate.Lines);
				parsedLines[thisMacroLineIndex] = ifToUpdate;
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace RGBDS2CIL
{
	public static class Restructure
	{
		public static void RestructureMacros(List<IAsmLine> parsedLines)
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

			parsedLines.RemoveAll(x => linesToRemove.Select(y => y.LineId).Contains(x.LineId));

			foreach (var macroLine in macrosToUpdate)
			{
				var thisMacroLineIndex = parsedLines.FindIndex(x => x.LineId == macroLine.LineId);
				RestructureIfs(macroLine.Lines);
				RestructureRepeats(macroLine.Lines);
				parsedLines[thisMacroLineIndex] = macroLine;
			}
		}

		public static void RestructureIfs(List<IAsmLine> parsedLines)
		{
			var lastIfLine = parsedLines.OfType<IfLine>().LastOrDefault(x => !x.IsElseIf && !x.Lines.Any());
			if (lastIfLine != null)
			{
				var ifContents = parsedLines
					.SkipWhile(x => x.LineId != lastIfLine.LineId)
					.Skip(1)
					.TakeUntilIncluding(x => x is EndConditionLine)
					.ToList();

				if (ifContents.Any())
				{
					RestructureRepeats(ifContents);
					lastIfLine.Lines = ifContents;
					parsedLines.RemoveAll(x => ifContents.Any(y => y.LineId == x.LineId) && x.LineId != lastIfLine.LineId);
				}

				RestructureIfs(parsedLines);
			}
		}

		public static void RestructureRepeats(List<IAsmLine> parsedLines)
		{
			var lastRepeatLine = parsedLines.OfType<RepeatLine>().LastOrDefault(x => !x.Lines.Any());
			if (lastRepeatLine != null)
			{
				var repeatContents = parsedLines
					.SkipWhile(x => x.LineId != lastRepeatLine.LineId)
					.Skip(1)
					.TakeUntilIncluding(x => x is EndRepeatLine)
					.ToList();

				if (repeatContents.Any())
				{
					RestructureIfs(repeatContents);
					lastRepeatLine.Lines = repeatContents;
					parsedLines.RemoveAll(x => repeatContents.Any(y => y.LineId == x.LineId) && x.LineId != lastRepeatLine.LineId);
				}

				RestructureRepeats(parsedLines);
			}
		}
	}
}
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
				parsedLines[thisMacroLineIndex] = macroLine;
			}
		}

		public static void RestructureIfs(List<IAsmLine> parsedLines)
		{
			var ifsToUpdate = new List<IfLine>();
			var linesToRemove = new List<IAsmLine>();

			foreach (var ifLine in parsedLines.OfType<IfLine>().Where(x => x.Lines.Count == 0 && !x.IsElseIf).ToArray())
			{
				var ifContents = parsedLines
					.SkipWhile(x => x.Line < ifLine.Line)
					.TakeWhile(x => x is not EndConditionLine)
					.ToList();

				var endline = parsedLines[parsedLines.IndexOf(ifContents.Last()) + 1];

				var macro = ifContents.OfType<IfLine>().First();
				macro.Lines = ifContents.Skip(1).ToList();
				macro.Lines.Add(endline);

				linesToRemove.AddRange(macro.Lines);

				ifsToUpdate.Add(macro);
			}


			//this and below is broken!!
			parsedLines.RemoveAll(x => linesToRemove.Select(y => y.LineId).Contains(x.LineId));

			foreach (var ifToUpdate in ifsToUpdate)
			{
				var thisIfLineIndex = parsedLines.FindIndex(x => x.LineId == ifToUpdate.LineId);
				RestructureIfs(ifToUpdate.Lines);
				if (thisIfLineIndex == -1)
				{
					Console.WriteLine("can't find");
					//check for more ifs?

				}
				else
				{
					parsedLines[thisIfLineIndex] = ifToUpdate;
				}
			}
		}
	}
}
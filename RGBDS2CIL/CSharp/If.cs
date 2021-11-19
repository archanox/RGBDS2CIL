using System.Text;

namespace RGBDS2CIL
{
	public static class If
	{
		internal static int ProcessIf(StringBuilder sb, int tabCount, IfLine ifLine)
		{
			ifLine = ifLine.Reparse() as IfLine;

			var ifElse = ifLine.IsElseIf ? "else if" : "if";

			if (ifLine.IsElseIf)
				sb.Append(new string('\t', tabCount)).AppendLine("}");

			sb.Append(new string('\t', tabCount)).Append(ifElse).Append(" (").Append(ifLine.Condition).Append(')')
				.AppendComment(ifLine.Comment);
			sb.Append(new string('\t', tabCount)).AppendLine("{");
			tabCount++;

			foreach (var macroLineLine in ifLine.Lines)
			{
				var lineLine = macroLineLine.Reparse();
				CSharp.OutputCSharp(lineLine, sb, tabCount);
			}

			tabCount--;
			return tabCount;
		}
	}
}
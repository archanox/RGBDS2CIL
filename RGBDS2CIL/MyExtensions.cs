using System;
using System.Collections.Generic;
using System.Text;

namespace RGBDS2CIL
{
	public static class MyExtensions
	{
		public static bool CommandName(this string code, string command) =>
			code.StartsWith(command, StringComparison.OrdinalIgnoreCase) &&
			(code.Length == command.Length || char.IsWhiteSpace(code[command.Length]) || code[command.Length] == '(');

		public static void AppendComment(this StringBuilder sb, string comment)
		{
			if (!string.IsNullOrWhiteSpace(comment))
				sb.Append(" // ").AppendLine(comment);
			else
				sb.AppendLine();
		}

		public static void AppendCode(this StringBuilder sb, string code, int tabCount, string comment)
		{
			sb.Append(new string('\t', tabCount))
				.Append(code)
				.AppendComment(comment);
		}

		public static IEnumerable<T> TakeUntilIncluding<T>(this IEnumerable<T> list, Func<T, bool> predicate)
		{
			foreach (T el in list)
			{
				yield return el;
				if (predicate(el))
					yield break;
			}
		}
	}
}
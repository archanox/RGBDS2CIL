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
		internal static void GenerateCsharp(string fileName, List<IAsmLine> parsedLines, string root)
		{
			var sb = new StringBuilder();

			var thisName = Regex.Replace(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Path.GetFileNameWithoutExtension(fileName)).Replace(" ", "").Replace('-', '_'), "[^A-Za-z0-9]", "");

			var includes = parsedLines.OfType<IncludeLine>().ToList();
			//todo get using names
			//todo call GenerateCsharp on these files.

			foreach (var include in includes)
			{
				var includeFileName = Path.Combine(root, include.FileName);
				GenerateCsharp(includeFileName, include.Lines, root);
			}

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
				tabCount = OutputCSharp(parsedLine, sb, tabCount);
			}

			sb.Append(new string('\t', 2)).AppendLine("}");
			sb.Append(new string('\t', 1)).AppendLine("}");
			sb.AppendLine("}");

			File.WriteAllText(fileName + ".cs", sb.ToString());
		}

		internal static int OutputCSharp(IAsmLine parsedLine, StringBuilder sb, int tabCount)
		{
			switch (parsedLine)
			{
				case CommentLine commentLine:
					OutputCommentLine(sb, tabCount, commentLine);
					break;
				case IncludeLine includeLine:
					OutputIncludeLine(sb, tabCount, includeLine);
					break;
				case VariableLine variableLine:
					OutputVariableLine(sb, tabCount, variableLine);
					break;
				case ConstantLine constantLine:
					OutputConstantLine(sb, tabCount, constantLine);
					break;
				case LabelLine labelLine:
					sb
						.Append(labelLine.LabelName)
						.Append(':')
						.AppendComment(labelLine.Comment);
					break;
				case CharMapLine charMapLine:
					var normalised = charMapLine.From.TrimStart('$').Insert(0, "0x");

					sb
						.Append(new string('\t', tabCount))
						.Append("CharMap[")
						.Append(normalised)
						.Append("] = ")
						.Append(charMapLine.Into)
						.Append(';')
						.AppendComment(charMapLine.Comment);
					break;
				case ElseLine elseLine:
					sb.Append(new string('\t', --tabCount)).AppendLine("}");
					sb.Append(new string('\t', tabCount)).Append("else").AppendComment(elseLine.Comment);
					sb.Append(new string('\t', tabCount++)).AppendLine("{");
					break;
				case IncrementLine incrementLine:
					sb.AppendCode($"{incrementLine.Increment}++;", tabCount, incrementLine.Comment);
					break;
				case DecrementLine decrementLine:
					sb.AppendCode($"{decrementLine.Decrement}--;", tabCount, decrementLine.Comment);
					break;
				case MacroLine macroLine:
					tabCount = Macro.ProcessMacro(sb, tabCount, macroLine);
					break;
				case EndMacroLine endMacroLine:
					sb.Append(new string('\t', tabCount)).Append('}').AppendComment(endMacroLine.Comment);
					break;
				case EndConditionLine endConditionLine:
					sb.Append(new string('\t', tabCount)).Append('}').AppendComment(endConditionLine.Comment);
					break;
				case ExclusiveOrLine exclusiveOrLine:
					sb.AppendCode($"{exclusiveOrLine.From} ^= {exclusiveOrLine.Value};", tabCount, exclusiveOrLine.Comment);
					break;
				case IfLine ifLine:
					tabCount = If.ProcessIf(sb, tabCount, ifLine);
					break;
				case WarnLine warnLine:
					sb.Append(new string('\t', tabCount)).Append("Trace.TraceWarning($\"").Append(warnLine.Warning).Append("\");").AppendComment(warnLine.Comment);
					break;
				case FailLine failLine:
					for (var i = 1; i < 10; i++)
					{
						failLine.FailMessage = failLine.FailMessage.Replace($"\\{i}", $"{{args[{i}]}}");
					}

					sb.Append(new string('\t', tabCount)).Append("Trace.Fail($\"").Append(failLine.FailMessage).Append("\");").AppendComment(failLine.Comment);
					break;

				//unimplemented
				
				case HaltLine haltLine:
				case RelativeJumpLine relativeJumpLine:
				case ReturnLine returnLine:
				case DefineLine defineLine:
				case SectionLine sectionLine:
				case NopLine nopLine:
				case JumpLine jumpLine:
				case DisableInterruptsLine disableInterruptsLine:
				case SubtractCompareLine subtractCompareLine:
				case PushOptionLine pushOptionLine:
				case OptionLine optionLine:
				case PopOptionLine popOptionLine:
				case LabelCallLine labelCallLine:
				case LoadHighLine loadHighLine:
				case AndLine andLine:
				case PushLine pushLine:
				case PopLine popLine:
				case EnableInterruptsLine enableInterruptsLine:
				case RotateRegisterALeftLine rotateRegisterALeftLine:
				case ResetByteLine resetByteLine:
				case BitLine bitLine:
				case ShiftRightArithmeticLine shiftRightArithmeticLine:
				case AddLine addLine:
				case ShiftLeftArithmeticLine shiftLeftArithmeticLine:
				case RotateLeftLine rotateLeftLine:
				case RepeatLine repeatLine:
				case PurgeLine purgeLine:
				case EndRepeatLine endRepeatLine:
				case ShiftLine shiftLine:
				case ConstantAssignLine constantAssignLine:
				case RotateRegisterARightLine rotateRegisterARightLine:
				case OrLine orLine:
				case ComplementLine complementLine:
				case AssertLine assertLine:
				case SwapLine swapLine:
				case Load2Line load2Line:
				case EndLoadLine endLoadLine:
				case SubtractLine subtractLine:
				case ComplementCarryFlagLine complementCarryFlagLine:
				case SetCarryFlagLine setCarryFlagLine:
				case ShiftRightLogicLine shiftRightLogicLine:
				case DecimalAdjustAccumulatorLine decimalAdjustAccumulatorLine:
				case RotateRegisterRightLine rotateRegisterRightLine:
				case RotateALeftLine rotateALeftLine:
				case RotateRegisterLeftLine rotateRegisterLeftLine:
				case DeclareSpaceLine declareSpaceLine:
				case RestartLine restartLine:
				case MacroArgumentLine macroArgumentLine:
				case StopLine stopLine:
				case CallLine callLine:
				case LoadLine loadLine:
					OutputUnimplementedLine((CodeLine)parsedLine);
					break;

				default:
					Debug.WriteLine(parsedLine.GetType().FullName);
					throw new NotSupportedException($"{parsedLine.GetType().FullName} is currently unsupported.");
			}

			return tabCount;

			void OutputUnimplementedLine(CodeLine loadLine)
			{
				sb.Append(new string('\t', tabCount)).Append("/* ").Append(loadLine.Code).Append(" */").AppendComment(loadLine.Comment);
			}
		}

		private static void OutputConstantLine(StringBuilder sb, int tabCount, ConstantLine constantLine)
		{
			var value = constantLine.ConstantValue;
			var valueType = "int";

			switch (constantLine.ConstantValueType)
			{
				case ConstantType.Hexadecimal:
					value = value.TrimStart('$').Insert(0, "0x");
					break;
				case ConstantType.Binary:
					value = value.TrimStart('%').Insert(0, "0b");
					break;
				case ConstantType.Octal:
					value = $"Convert.ToInt32(\"{value.TrimStart('%')}\", 8)";
					break;
				case ConstantType.String:
					valueType = "string";
					break;
				case ConstantType.Decimal:
					valueType = "double";
					break;
				case ConstantType.FixedPoint: //todo: must be fixed point
					valueType = "decimal";
					break;
				case ConstantType.Graphics:
					break;
				default:
					throw new ArgumentOutOfRangeException(constantLine.ConstantValueType.ToString(),
						"Unknown ConstantValueType");
			}

			sb
				.Append(new string('\t', tabCount))
				.Append("const ")
				.Append(valueType)
				.Append(' ')
				.Append(constantLine.ConstantName)
				.Append(" = ")
				.Append(value)
				.Append(';')
				.AppendComment(constantLine.Comment);
		}

		private static void OutputVariableLine(StringBuilder sb, int tabCount, VariableLine variableLine) =>
			sb
				.Append(new string('\t', tabCount))
				.Append("var ")
				.Append(variableLine.VariableName)
				.Append(" = ")
				.Append(variableLine.VariableValue)
				.Append(';')
				.AppendComment(variableLine.Comment);

		private static void OutputIncludeLine(StringBuilder sb, int tabCount, IncludeLine includeLine) =>
			sb.Append(new string('\t', tabCount)).Append("/* ").Append(includeLine.Code).Append(" */")
				.AppendComment(includeLine.Comment);

		private static void OutputCommentLine(StringBuilder sb, int tabCount, CommentLine commentLine)
		{
			if (string.IsNullOrWhiteSpace(commentLine.Comment))
				sb.AppendLine();
			else
				sb.Append(new string('\t', tabCount)).Append("// ").AppendLine(commentLine.Comment);
		}
	}
}
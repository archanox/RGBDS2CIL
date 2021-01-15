using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace RGBDS2CIL
{
    public class CSharp
    {
        internal static void GenerateCsharp(string fileName, IEnumerable<IAsmLine> parsedLines)
        {
            var ti = CultureInfo.CurrentCulture.TextInfo;

            var sb = new StringBuilder();

            var thisName = Regex.Replace(ti.ToTitleCase(Path.GetFileNameWithoutExtension(fileName)).Replace(" ", ""), "[^A-Za-z0-9 -]", ""); ;

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

            sb.Append(new string('\t', --tabCount)).AppendLine("}");
            sb.Append(new string('\t', --tabCount)).AppendLine("}");
            sb.AppendLine("}");

            File.WriteAllText(fileName + ".cs", sb.ToString());
        }

        internal static int OutputCSharp(IAsmLine parsedLine, StringBuilder sb, int tabCount)
        {
            switch (parsedLine)
            {
                case CommentLine commentLine:
                    if (string.IsNullOrWhiteSpace(commentLine.Comment))
                        sb.AppendLine();
                    else
                        sb.Append(new string('\t', tabCount)).Append("// ").AppendLine(commentLine.Comment);
                    break;
                case IncludeLine includeLine:
                    //	Console.WriteLine($"{includeLine.}");
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(includeLine.Code).Append(" */")
                        .AppendComment(includeLine.Comment);
                    break;
                case VariableLine variableLine:
                    sb
                        .Append(new string('\t', tabCount))
                        .Append("var ")
                        .Append(variableLine.VariableName)
                        .Append(" = ")
                        .Append(variableLine.VariableValue)
                        .Append(';')
                        .AppendComment(variableLine.Comment);
                    break;
                case ConstantLine constantLine:
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
                            break;
                        case ConstantType.FixedPoint: //todo: must be fixed point
                            valueType = "decimal";
                            break;
                        case ConstantType.Graphics:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
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
                    break;
                case LabelLine labelLine:
                    sb
                        .Append(labelLine.LabelName)
                        .Append(":")
                        .AppendComment(labelLine.Comment);
                    break;
                case LoadLine loadLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(loadLine.Code).Append(" */")
                        .AppendComment(loadLine.Comment);
                    break;
                case CallLine callLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(callLine.Code).Append(" */")
                        .AppendComment(callLine.Comment);
                    break;
                case HaltLine haltLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(haltLine.Code).Append(" */")
                        .AppendComment(haltLine.Comment);
                    break;
                case RelativeJumpLine relativeJumpLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(relativeJumpLine.Code).Append(" */")
                        .AppendComment(relativeJumpLine.Comment);
                    break;
                case IncrementLine incrementLine:
                    sb.AppendCode($"{incrementLine.Increment}++;", tabCount, incrementLine.Comment);
                    break;
                case DecrementLine decrementLine:
                    sb.AppendCode($"{decrementLine.Decrement}--;", tabCount, decrementLine.Comment);
                    break;
                case ReturnLine returnLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(returnLine.Code).Append(" */")
                        .AppendComment(returnLine.Comment);
                    break;
                case DefineLine defineLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(defineLine.Code).Append(" */")
                        .AppendComment(defineLine.Comment);
                    break;
                case MacroLine macroLine:
                    tabCount = Macro.ProcessMacro(sb, tabCount, macroLine);
                    break;
                case EndMacroLine endMacroLine:
                    sb.Append(new string('\t', --tabCount)).Append("}").AppendComment(endMacroLine.Comment);
                    break;
                case EndConditionLine endConditionLine:
                    sb.Append(new string('\t', --tabCount)).Append("}").AppendComment(endConditionLine.Comment);
                    break;
                case SectionLine sectionLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(sectionLine.Code).Append(" */")
                        .AppendComment(sectionLine.Comment);
                    break;
                case NopLine nopLine:
                    sb.Append(new string('\t', tabCount)).Append("/*").Append(nopLine.Code).Append("*/").AppendComment(nopLine.Comment);
                    break;
                case JumpLine jumpLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(jumpLine.Code).Append(" */")
                        .AppendComment(jumpLine.Comment);
                    break;
                case DisableInterruptsLine disableInterruptsLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(disableInterruptsLine.Code).Append(" */")
                        .AppendComment(disableInterruptsLine.Comment);
                    break;
                case SubtractCompareLine subtractCompareLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(subtractCompareLine.Code).Append(" */")
                        .AppendComment(subtractCompareLine.Comment);
                    break;
                case ExclusiveOrLine exclusiveOrLine:
                    sb.AppendCode($"{exclusiveOrLine.From} ^= {exclusiveOrLine.Value};", tabCount, exclusiveOrLine.Comment);
                    break;
                case IfLine ifLine:
                    tabCount = If.ProcessIf(sb, tabCount, ifLine);
                    break;
                case WarnLine warnLine:
                    sb.Append(new string('\t', tabCount)).Append("Trace.TraceWarning($\"").Append(warnLine.Warning)
                        .Append("\");").AppendComment(warnLine.Comment);
                    break;
                //-- hello-world.asm
                case FailLine failLine:
                    for (var i = 1; i < 10; i++)
                    {
                        failLine.FailMessage = failLine.FailMessage.Replace($"\\{i}", $"{{args[{i}]}}");
                    }


                    sb.Append(new string('\t', tabCount)).Append("Trace.Fail($\"").Append(failLine.FailMessage).Append("\");")
                        .AppendComment(failLine.Comment);
                    break;
                case PushOptionLine pushOptionLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(pushOptionLine.Code).Append(" */")
                        .AppendComment(pushOptionLine.Comment);
                    break;
                case OptionLine optionLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(optionLine.Code).Append(" */")
                        .AppendComment(optionLine.Comment);
                    break;
                case PopOptionLine popOptionLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(popOptionLine.Code).Append(" */")
                        .AppendComment(popOptionLine.Comment);
                    break;
                case LabelCallLine labelCallLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(labelCallLine.Code).Append(" */")
                        .AppendComment(labelCallLine.Comment);
                    break;
                case LoadHighLine loadHighLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(loadHighLine.Code).Append(" */")
                        .AppendComment(loadHighLine.Comment);
                    break;
                case AndLine andLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(andLine.Code).Append(" */")
                        .AppendComment(andLine.Comment);
                    break;
                case PushLine pushLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(pushLine.Code).Append(" */")
                        .AppendComment(pushLine.Comment);
                    break;
                case PopLine popLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(popLine.Code).Append(" */")
                        .AppendComment(popLine.Comment);
                    break;
                case EnableInterruptsLine enableInterruptsLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(enableInterruptsLine.Code).Append(" */")
                        .AppendComment(enableInterruptsLine.Comment);
                    break;
                case RotateRegisterALeftLine rotateRegisterALeftLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(rotateRegisterALeftLine.Code).Append(" */")
                        .AppendComment(rotateRegisterALeftLine.Comment);
                    break;
                case ResetByteLine resetByteLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(resetByteLine.Code).Append(" */")
                        .AppendComment(resetByteLine.Comment);
                    break;
                //-- dmg_boot (2) orig.asm
                case BitLine bitLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(bitLine.Code).Append(" */")
                        .AppendComment(bitLine.Comment);
                    break;
                case ShiftRightArithmeticLine shiftRightArithmeticLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(shiftRightArithmeticLine.Code).Append("*/")
                        .AppendComment(shiftRightArithmeticLine.Comment);
                    break;
                case AddLine addLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(addLine.Code).Append(" */")
                        .AppendComment(addLine.Comment);
                    break;
                case ShiftLeftArithmeticLine shiftLeftArithmeticLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(shiftLeftArithmeticLine.Code).Append(" */")
                        .AppendComment(shiftLeftArithmeticLine.Comment);
                    break;
                case RotateLeftLine rotateLeftLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(rotateLeftLine.Code).Append(" */")
                        .AppendComment(rotateLeftLine.Comment);
                    break;
                //-- pokered: main.asm
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
                case RepeatLine repeatLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(repeatLine.Code).Append(" */")
                            .AppendComment(repeatLine.Comment);
                    break;
                case PurgeLine purgeLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(purgeLine.Code).Append(" */")
                            .AppendComment(purgeLine.Comment);
                    break;
                case EndRepeatLine endRepeatLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(endRepeatLine.Code).Append(" */")
                            .AppendComment(endRepeatLine.Comment);
                    break;
                case ShiftLine shiftLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(shiftLine.Code).Append(" */")
                            .AppendComment(shiftLine.Comment);
                    break;
                case ConstantAssignLine constantAssignLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(constantAssignLine.Code).Append(" */")
                            .AppendComment(constantAssignLine.Comment);
                    break;
                case RotateRegisterARightLine rotateRegisterARightLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(rotateRegisterARightLine.Code).Append(" */")
                            .AppendComment(rotateRegisterARightLine.Comment);
                    break;
                case OrLine orLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(orLine.Code).Append(" */")
                            .AppendComment(orLine.Comment);
                    break;
                case ComplementLine complementLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(complementLine.Code).Append(" */")
                            .AppendComment(complementLine.Comment);
                    break;
                case AssertLine assertLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(assertLine.Code).Append(" */")
                            .AppendComment(assertLine.Comment);
                    break;
                case SwapLine swapLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(swapLine.Code).Append(" */")
                            .AppendComment(swapLine.Comment);
                    break;
                case Load2Line load2Line:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(load2Line.Code).Append(" */")
                            .AppendComment(load2Line.Comment);
                    break;
                case EndLoadLine endLoadLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(endLoadLine.Code).Append(" */")
                            .AppendComment(endLoadLine.Comment);
                    break;
                case SubtractLine subtractLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(subtractLine.Code).Append(" */")
                            .AppendComment(subtractLine.Comment);
                    break;
                case ComplementCarryFlagLine complementCarryFlagLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(complementCarryFlagLine.Code).Append(" */")
                            .AppendComment(complementCarryFlagLine.Comment);
                    break;
                case SetCarryFlagLine setCarryFlagLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(setCarryFlagLine.Code).Append(" */")
                            .AppendComment(setCarryFlagLine.Comment);
                    break;
                case ShiftRightLogicLine shiftRightLogicLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(shiftRightLogicLine.Code).Append(" */")
                            .AppendComment(shiftRightLogicLine.Comment);
                    break;
                case DecimalAdjustAccumulatorLine decimalAdjustAccumulatorLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(decimalAdjustAccumulatorLine.Code).Append(" */")
                            .AppendComment(decimalAdjustAccumulatorLine.Comment);
                    break;
                case RotateRegisterRightLine rotateRegisterRightLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(rotateRegisterRightLine.Code).Append(" */")
                            .AppendComment(rotateRegisterRightLine.Comment);
                    break;
                case RotateALeftLine rotateALeftLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(rotateALeftLine.Code).Append(" */")
                            .AppendComment(rotateALeftLine.Comment);
                    break;
                case RotateRegisterLeftLine rotateRegisterLeftLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(rotateRegisterLeftLine.Code).Append(" */")
                            .AppendComment(rotateRegisterLeftLine.Comment);
                    break;
                case DeclareSpaceLine declareSpaceLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(declareSpaceLine.Code).Append(" */")
                            .AppendComment(declareSpaceLine.Comment);
                    break;
                //-- pokered: home.asm
                case RestartLine restartLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(restartLine.Code).Append(" */")
                            .AppendComment(restartLine.Comment);
                    break;
                case MacroArgumentLine macroArgumentLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(macroArgumentLine.Code).Append(" */")
                            .AppendComment(macroArgumentLine.Comment);
                    break;
                //-- Pokemon Red (UE) [S][!].asm
                case StopLine stopLine:
                    sb.Append(new string('\t', tabCount)).Append("/* ").Append(stopLine.Code).Append(" */")
                            .AppendComment(stopLine.Comment);
                    break;
                default:
                    Debug.WriteLine(parsedLine.GetType().FullName);
                    throw new NotImplementedException(parsedLine.GetType().FullName);
                    break;
            }

            return tabCount;
        }
    }
}
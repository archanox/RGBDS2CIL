using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace RGBDS2CIL
{
	public static class Parser
	{
		private static readonly Regex CommentRegex = new(@"(;.*?(\r?\n|$))|(""(?:\\[^\n]|[^""\n])*"")|(@(?:""[^""]*"")+)", RegexOptions.Compiled);

		private static readonly Regex GetStringsRegex = new("((?<![\\\\])['\"])((?:.(?!(?<![\\\\])\\1))*.?)\\1", RegexOptions.Compiled | RegexOptions.IgnoreCase);

		internal static string RootFolder { get; set; }
		private static List<LabelLine> Labels { get; } = new();
		private static List<ConstantLine> Constants { get; } = new();

		public static string ExportJson(List<IAsmLine> parsedLines)
		{
			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				Formatting = Formatting.Indented
			};
			return JsonConvert.SerializeObject(parsedLines, settings);

		}

		internal static string[] FlattenMultiLine(IList<string> fileLines)
		{
			for (var i = 0; i < fileLines.Count; i++)
			{
				if (string.IsNullOrWhiteSpace(fileLines[i])) continue;

				//var comment = GetComment(fileLines[i]);
				var code = RemoveCommentFromCode(fileLines[i]);

				if (code?.EndsWith('\\') != true) continue;
				var rowSkip = i + 1;
				var hasMore = true;

				while (hasMore)
				{
					var comment = GetComment(fileLines[i]);
					code = RemoveCommentFromCode(fileLines[i]);

					var fileLine2 = fileLines[rowSkip];
					var comment2 = GetComment(fileLine2);
					var code2 = RemoveCommentFromCode(fileLine2);
					hasMore = code2.EndsWith('\\');

					fileLines[i] = $"{code.TrimEnd('\\')} {code2} {((!string.IsNullOrWhiteSpace(comment) || !string.IsNullOrWhiteSpace(comment2)) ? "; " : " ")}{(comment + " " + comment2).Trim()}".Trim();

					fileLines[rowSkip] = null;

					rowSkip++;
				}
			}

			return fileLines.Where(x => x is not null).ToArray();
		}

		public static List<IAsmLine> GetLines(IEnumerable<string> fileLines, string fileName) => fileLines
			//.AsParallel().AsOrdered()
			.SelectMany((x, y) => ParseLine(x, fileName, y)).ToList();

		private static IEnumerable<IAsmLine> ParseLine(string fileLine, string fileName, int line)
		{
			var parsedLines = new List<IAsmLine>();

			var comment = GetComment(fileLine);

			var code = RemoveCommentFromCode(fileLine);

			if (code == null)
			{
				parsedLines.Add(new CommentLine(fileLine, comment, fileName, line));
			}
			else
			{
				var codeLine = new CodeLine(code, fileLine, comment, fileName, line, GetStrings(code));

				if (code.CommandName("INCLUDE"))
				{
					var includeFile = new IncludeLine(codeLine, false);
					var path = Path.Combine(RootFolder, includeFile.IncludeFile);
					if (File.Exists(path))
						includeFile.Lines.AddRange(GetLines(FlattenMultiLine(File.ReadAllLines(path)), includeFile.IncludeFile));
					else
					{
						Console.WriteLine($"Could not include file, {includeFile.IncludeFile}");
						//throw new FileNotFoundException("Could not include file", includeFile.IncludeFile);
					}

					parsedLines.Add(includeFile);
				}
				else if (code.CommandName("INCBIN"))
				{
					var binary = new IncludeLine(codeLine, true);
					var path = Path.Combine(RootFolder, binary.IncludeFile);
					if (File.Exists(path))
					{
						binary.ReadBinaryFile(path);
					}
					else
					{
						Console.WriteLine($"Could not include binary, {binary.IncludeFile}");
						//throw new FileNotFoundException("Could not include binary", binary.IncludeFile);
					}

					parsedLines.Add(binary);
				}
				else if (code.CommandName("SECTION"))
					parsedLines.Add(new SectionLine(codeLine));
				else if (code.CommandName("JP"))
					parsedLines.Add(new JumpLine(codeLine));

				else if (code.StartsWith('.') || code.EndsWith(':') || code.Split()[0].EndsWith("::") || code.Split()[0].EndsWith(":"))
				{
					var split = code.Split();

					if (string.Equals(split.Last(), "MACRO", StringComparison.OrdinalIgnoreCase))
					{
						parsedLines.Add(new MacroLine(codeLine, split[0]));
					}
					else
					{
						codeLine.Code = split[0];
						var label = new LabelLine(codeLine);
						Labels.Add(label);

						parsedLines.Add(label);
						if (split.Length > 1)
						{
							//Console.WriteLine(code[codeLine.Code.Length..].Trim() + " [" + code + "]");
							//parsedLines.AddRange(ParseLine(code[codeLine.Code.Length..], fileName, line));
						}
					}
				}
				else if (code.CommandName("ENDM"))
					parsedLines.Add(new EndMacroLine(codeLine));

				else if (code.Trim().ToUpper().Split().Contains("EQU"))
				{
					var constant = new ConstantLine(codeLine, "EQU");
					Constants.Add(constant);
					parsedLines.Add(constant);
				}
				else if (code.Trim().ToUpper().Split().Contains("EQUS"))
				{
					var constant = new ConstantLine(codeLine, "EQUS");
					Constants.Add(constant);
					parsedLines.Add(constant);
				}
				else if (code.Trim().Split().Length > 1 && code.Trim().Split().Skip(1).First() == "=")
				{
					var constant = new ConstantLine(codeLine, "=");
					Constants.Add(constant);
					parsedLines.Add(constant);
				}
				else if (code.CommandName("SET"))
					parsedLines.Add(new VariableLine(codeLine));
				else if (code.ToUpper().Trim() == "NOP")
					parsedLines.Add(new NopLine(codeLine));
				else if (code.CommandName("LD"))
					parsedLines.Add(new LoadLine(codeLine, "LD"));
				else if (code.CommandName("LDI"))
					parsedLines.Add(new LoadLine(codeLine, "LDI"));
				else if (code.CommandName("LDD"))
					parsedLines.Add(new LoadLine(codeLine, "LDD"));
				else if (code.CommandName("CALL"))
					parsedLines.Add(new CallLine(codeLine));
				else if (code.CommandName("RST"))
					parsedLines.Add(new RestartLine(codeLine));
				else if (code.CommandName("CP"))
					parsedLines.Add(new SubtractCompareLine(codeLine));
				else if (code.CommandName("PUSHO"))
					parsedLines.Add(new PushOptionLine(codeLine));
				else if (code.CommandName("POPO"))
					parsedLines.Add(new PopOptionLine(codeLine));
				else if (code.CommandName("DI"))
					parsedLines.Add(new DisableInterruptsLine(codeLine));
				else if (code.CommandName("HALT"))
					parsedLines.Add(new HaltLine(codeLine));
				else if (code.CommandName("JR"))
					parsedLines.Add(new RelativeJumpLine(codeLine));
				else if (code.CommandName("XOR"))
					parsedLines.Add(new ExclusiveOrLine(codeLine));
				else if (code.CommandName("ADD"))
					parsedLines.Add(new AddLine(codeLine, false));
				else if (code.CommandName("ADC"))
					parsedLines.Add(new AddLine(codeLine, true));
				else if (code.CommandName("INC"))
					parsedLines.Add(new IncrementLine(codeLine));
				else if (code.CommandName("DEC"))
					parsedLines.Add(new DecrementLine(codeLine));
				else if (code.CommandName("SUB"))
					parsedLines.Add(new SubtractLine(codeLine, false));
				else if (code.CommandName("SBC"))
					parsedLines.Add(new SubtractLine(codeLine, true));
				else if (code.CommandName("DB")) //byte 8bit
					parsedLines.Add(new DefineLine(codeLine, typeof(byte)));
				else if (code.CommandName("DW")) //word 16bit (short)
					parsedLines.Add(new DefineLine(codeLine, typeof(short)));
				else if (code.CommandName("DL")) //double-word/long 32bit (int)
					parsedLines.Add(new DefineLine(codeLine, typeof(int)));
				else if (code.CommandName("ENDR"))
					parsedLines.Add(new EndRepeatLine(codeLine));
				else if (code.CommandName("ENDC"))
					parsedLines.Add(new EndConditionLine(codeLine));
				else if (code.CommandName("WARN"))
					parsedLines.Add(new WarnLine(codeLine));
				else if (code.CommandName("FAIL"))
					parsedLines.Add(new FailLine(codeLine));
				else if (code.CommandName("CHARMAP"))
					parsedLines.Add(new CharMapLine(codeLine));
				else if (code.CommandName("IF"))
					parsedLines.Add(new IfLine(codeLine, false));
				else if (code.CommandName("ELIF"))
					parsedLines.Add(new IfLine(codeLine, true));
				else if (code.CommandName("ELSE"))
					parsedLines.Add(new ElseLine(codeLine));
				else if (code.CommandName("RET"))
					parsedLines.Add(new ReturnLine(codeLine, false));
				else if (code.CommandName("RETI"))
					parsedLines.Add(new ReturnLine(codeLine, true));
				else if (string.Equals(code, "EI", StringComparison.OrdinalIgnoreCase))
					parsedLines.Add(new EnableInterruptsLine(codeLine));
				else if (code.CommandName("PURGE"))
					parsedLines.Add(new PurgeLine(codeLine));
				else if (code.CommandName("REPT"))
					parsedLines.Add(new RepeatLine(codeLine));
				else if (code.CommandName("SHIFT"))
					parsedLines.Add(new ShiftLine(codeLine));
				else if (code.CommandName("POP"))
					parsedLines.Add(new PopLine(codeLine));
				else if (code.CommandName("PUSH"))
					parsedLines.Add(new PushLine(codeLine));
				else if (code.CommandName("RL"))
					parsedLines.Add(new RotateLeftLine(codeLine));
				else if (code.CommandName("RLA"))
					parsedLines.Add(new RotateALeftLine(codeLine));
				else if (code.CommandName("BIT"))
					parsedLines.Add(new BitLine(codeLine));
				else if (code.CommandName("LDH"))
					parsedLines.Add(new LoadHighLine(codeLine));
				else if (code.CommandName("AND"))
					parsedLines.Add(new AndLine(codeLine));
				else if (code.CommandName("CPL"))
					parsedLines.Add(new ComplementLine(codeLine));
				else if (code.CommandName("RRCA"))
					parsedLines.Add(new RotateRegisterARightLine(codeLine));
				else if (code.CommandName("RLCA"))
					parsedLines.Add(new RotateRegisterALeftLine(codeLine));
				else if (code.CommandName("RES"))
					parsedLines.Add(new ResetByteLine(codeLine));
				else if (code.CommandName("SCF"))
					parsedLines.Add(new SetCarryFlagLine(codeLine));
				else if (code.CommandName("CCF"))
					parsedLines.Add(new ComplementCarryFlagLine(codeLine));
				else if (code.CommandName("OPT"))
					parsedLines.Add(new OptionLine(codeLine));
				else if (code.CommandName("OR"))
					parsedLines.Add(new OrLine(codeLine));
				else if (code.CommandName("DS"))
					parsedLines.Add(new DeclareSpaceLine(codeLine));
				else if (code.CommandName("SWAP"))
					parsedLines.Add(new SwapLine(codeLine));
				else if (code.CommandName("RR"))
					parsedLines.Add(new RotateRegisterRightLine(codeLine));
				else if (code.CommandName("SRL"))
					parsedLines.Add(new ShiftRightLogicLine(codeLine));
				else if (code.CommandName("SLA"))
					parsedLines.Add(new ShiftLeftArithmeticLine(codeLine));
				else if (code.CommandName("DAA"))
					parsedLines.Add(new DecimalAdjustAccumulatorLine(codeLine));
				else if (code.CommandName("RRA"))
					parsedLines.Add(new RotateRegisterRightLine(codeLine));
				else if (code.CommandName("STOP"))
					parsedLines.Add(new StopLine(codeLine));
				else if (code.CommandName("ASSERT"))
					parsedLines.Add(new AssertLine(codeLine));
				else if (code.CommandName("RRC"))
					parsedLines.Add(new RotateRegisterRightLine(codeLine));
				else if (code.CommandName("RLC"))
					parsedLines.Add(new RotateRegisterLeftLine(codeLine));
				else if (code.CommandName("SRA"))
					parsedLines.Add(new ShiftRightArithmeticLine(codeLine));
				else if (code.CommandName("LOAD"))
					parsedLines.Add(new Load2Line(codeLine));
				else if (code.CommandName("ENDL"))
					parsedLines.Add(new EndLoadLine(codeLine));

				//else if (code.CommandName("UNION"))
				//	parsedLines.Add(new UnionLine(codeLine));
				//else if (code.CommandName("NEXTU"))
				//	parsedLines.Add(new NextUnionLine(codeLine));
				//else if (code.CommandName("ENDU"))
				//	parsedLines.Add(new EndUnionLine(codeLine));

				//https://rgbds.gbdev.io/docs/master/rgblink.5#ORG
				//note: moves out the address
				//else if (code.CommandName("ORG"))
				//	parsedLines.Add(new OrgLine(codeLine));

				//ifdef
				// c# = "#if (DEBUG)"
				//endif
				// c# = "#endif"

				else if (code.StartsWith('\\') && char.IsDigit(code.ToUpper()[1]))
					parsedLines.Add(new MacroArgumentLine(codeLine));
				else if (Labels.Select(x => x.LabelName.ToUpper()).Contains(codeLine.Code.Split()[0].ToUpper()))
				{
					var labelName = codeLine.Code.Split()[0].ToUpper();
					var labels = Labels
						.Where(x => string.Equals(x.LabelName, labelName, StringComparison.OrdinalIgnoreCase))
						.ToImmutableArray();
					if (labels.Length > 1)
					{
						//TODO: need to remove already declared shit, unless it's scoped differently?
						//override methods?
						//Debugger.Break();
						//TODO: get proper name spaced label
					}

					var label = labels.FirstOrDefault();
					parsedLines.Add(new LabelCallLine(codeLine, label));
				}
				else if (Constants.Select(x => x.ConstantName.ToUpper()).Contains(codeLine.Code.Split()[0].Trim().ToUpper()))
				{
					var constantName = codeLine.Code.Split()[0].Trim().ToUpper();

					var constants = Constants
						.Where(x => string.Equals(x.ConstantName, constantName, StringComparison.OrdinalIgnoreCase))
						.ToImmutableArray();
					if (constants.Length > 1)
					{
						//Debugger.Break();
					}

					var constant = constants.FirstOrDefault();
					parsedLines.Add(new ConstantAssignLine(codeLine, constant));
				}
				else
				{
					//Console.WriteLine(codeLine.Code.Trim().Split()[0]);
					//Console.WriteLine(codeLine.FileName);
					//Debugger.Break();
					//parsedLines.Add(codeLine);
					//throw new NotImplementedException($"Instruction {codeLine.Code.Split()[0]} not implemented.");
				}
			}

			return parsedLines.ToArray();
		}

		public static string RemoveCommentFromCode(string fileLine)
		{
			var code = CommentRegex.Replace(fileLine, me => me.Value.StartsWith(";") ? me.Groups[2].Value : me.Value).Trim();

			if (string.IsNullOrWhiteSpace(code))
				code = null;
			return code?.Trim();
		}

		public static string GetComment(string fileLine)
		{
			if (fileLine == null)
				throw new ArgumentNullException(nameof(fileLine));
			var clean = CommentRegex.Replace(fileLine, me => me.Value.StartsWith(";") ? me.Groups[2].Value : me.Value).Trim();

			var comment = fileLine.Remove(fileLine.IndexOf(clean, StringComparison.Ordinal), clean.Length).Trim();

			if (string.IsNullOrWhiteSpace(comment))
				return null;

			if (comment.StartsWith(';'))
				comment = comment[1..];

			return comment.Trim();
		}

		public static List<string> GetStrings(string code)
		{
			if (string.IsNullOrWhiteSpace(code)) return new List<string>();

			var returned = GetStringsRegex.Matches(code).Select(x => x.Value?.TrimStart('"').TrimEnd('"')).ToList();

			return returned.Count > 0 ? returned : null;
		}

		public static List<string> GetParameters(string code)
		{
			if (string.IsNullOrWhiteSpace(code)) return new List<string>();

			var matches = new List<string>();

			while (code.Length > 0)
			{
				var parameter = GetParameter(code);
				var newParam = parameter;
				for (var i = 1; i < 10; i++)
				{
					newParam = newParam.Replace($"\\{i}", $"args[{i - 1}]");
				}
				matches.Add(ReplaceDataTypesInString(newParam));
				code = code[parameter.Length..].TrimStart(',').Trim();
			}

			return matches.Count > 0 ? matches : null;
		}

		public static string ReplaceDataTypesInString(string value)
		{
			//https://rgbds.gbdev.io/docs/v0.5.2/rgbasm.5#Operators
			//pad out the +-*/%~
			value = value
				.Replace("+", " + ")
				.Replace("*", " * ")
				.Replace("-", " - ")
				.Replace("/", " / ");

			var newValues = new List<string>();
			foreach (var splitValue in value.Split(' '))
			{
				if (splitValue.StartsWith('$'))
					newValues.Add(splitValue.TrimStart('$').Insert(0, "0x"));
				else if (splitValue.StartsWith('%'))
					newValues.Add(splitValue.TrimStart('%').Insert(0, "0b"));
				else if (splitValue.StartsWith('&'))
					newValues.Add($"Convert.ToInt32(\"{splitValue.TrimStart('%')}\", 8)");
				else
					newValues.Add(splitValue);
			}

			value = string.Join(' ', newValues);
			return value;
		}

		private static string GetParameter(string code)
		{
			//NOTE: Does not currently support nested parameters
			//NOTE: Does not support exiting a string
			var i = 0;
			var insideString = code[0] == '\"';
			var insideFunction = false;
			for (; i < code.Length; i++)
			{
				if (!insideString)
				{
					if (code[i] == '(') insideFunction = true;
					if (code[i] == ')') insideFunction = false;
					if (code[i] == ',' && !insideFunction) break;
					continue;
				}

				if (i <= 0 || code[i] != '\"' || code[i - 1] == '\\') continue;

				i++;
				break;
			}

			return code[..i];
		}
	}
}
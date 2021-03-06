using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace RGBDS2CIL
{
	public static class Parser
	{
		private static readonly Regex CommentRegex = new Regex(@"(;.*?(\r?\n|$))|(""(?:\\[^\n]|[^""\n])*"")|(@(?:""[^""]*"")+)", RegexOptions.Compiled);

		private static readonly Regex GetStringsRegex = new("((?<![\\\\])['\"])((?:.(?!(?<![\\\\])\\1))*.?)\\1",
			RegexOptions.Compiled | RegexOptions.IgnoreCase);

		internal static string RootFolder { get; set; }
		private static List<LabelLine> Labels { get; } = new();
		private static List<ConstantLine> Constants { get; } = new();

		internal static void ExportJson(string fileName, List<IAsmLine> parsedLines)
		{
			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto,
				Formatting = Formatting.Indented
			};
			var serialized = JsonConvert.SerializeObject(parsedLines, settings);
			File.WriteAllText(fileName + ".json", serialized);
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

					fileLines[i] =
						$"{code.TrimEnd('\\')} {code2} {((!string.IsNullOrWhiteSpace(comment) || !string.IsNullOrWhiteSpace(comment2)) ? "; " : " ")}{(comment + " " + comment2).Trim()}"
							.Trim();

					fileLines[rowSkip] = null;

					rowSkip++;
				}
			}

			return fileLines.Where(x => x is not null).ToArray();
		}

		public static List<IAsmLine> GetLines(IEnumerable<string> fileLines, string fileName) => fileLines
			//.AsParallel().AsOrdered()
			.SelectMany<string, IAsmLine>((x, y) => ParseLine(x, fileName, y)).ToList();

		internal static IEnumerable<IAsmLine> ParseLine(string fileLine, string fileName, int line)
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

				if (MyExtensions.CommandName(code, "INCLUDE"))
				{
					var includeFile = new IncludeLine(codeLine, false);
					var path = Path.Combine(RootFolder, includeFile.IncludeFile);
					if (File.Exists(path))
						includeFile.Lines.AddRange(GetLines(FlattenMultiLine(File.ReadAllLines(path)), includeFile.IncludeFile));
					//else
					//	throw new FileNotFoundException("Could not include file", includeFile.IncludeFile);
					parsedLines.Add(includeFile);
				}
				else if (MyExtensions.CommandName(code, "INCBIN"))
				{
					var binary = new IncludeLine(codeLine, true);
					var path = Path.Combine(RootFolder, binary.IncludeFile);
					if (File.Exists(path))
						binary.Binary = File.ReadAllBytes(path);
					//else
					//	throw new FileNotFoundException("Could not include binary", binary.IncludeFile);
					parsedLines.Add(binary);
				}
				else if (MyExtensions.CommandName(code, "SECTION"))
					parsedLines.Add(new SectionLine(codeLine));
				else if (MyExtensions.CommandName(code, "JP"))
					parsedLines.Add(new JumpLine(codeLine));




				else if (code.StartsWith('.') || code.EndsWith(':') || code.Split()[0].EndsWith("::") ||
						 code.Split()[0].EndsWith(":"))
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
							//Console.Error.WriteLine(code[codeLine.Code.Length..].Trim() + " [" + code + "]");
							//parsedLines.AddRange(ParseLine(code[codeLine.Code.Length..], fileName, line));
						}
					}
				}
				else if (MyExtensions.CommandName(code, "ENDM"))
					parsedLines.Add(new EndMacroLine(codeLine));

				else if (Enumerable.Contains(code.Trim().ToUpper().Split(), "EQU"))
				{
					var constant = new ConstantLine(codeLine, "EQU");
					Constants.Add(constant);
					parsedLines.Add(constant);
				}
				else if (Enumerable.Contains(code.Trim().ToUpper().Split(), "EQUS"))
				{
					var constant = new ConstantLine(codeLine, "EQUS");
					Constants.Add(constant);
					parsedLines.Add(constant);
				}
				else if (code.Trim().Split().Length > 1 && Enumerable.Skip<string>(code.Trim().Split(), 1).First() == "=")
				{
					var constant = new ConstantLine(codeLine, "=");
					Constants.Add(constant);
					parsedLines.Add(constant);
				}
				else if (Enumerable.Contains(code.Trim().ToUpper().Split(), "SET"))
					parsedLines.Add(new VariableLine(codeLine));
				else if (code.ToUpper().Trim() == "NOP")
					parsedLines.Add(new NopLine(codeLine));
				else if (MyExtensions.CommandName(code, "LD"))
					parsedLines.Add(new LoadLine(codeLine, "LD"));
				else if (MyExtensions.CommandName(code, "LDI"))
					parsedLines.Add(new LoadLine(codeLine, "LDI"));
				else if (MyExtensions.CommandName(code, "LDD"))
					parsedLines.Add(new LoadLine(codeLine, "LDD"));
				else if (MyExtensions.CommandName(code, "CALL"))
					parsedLines.Add(new CallLine(codeLine));
				else if (MyExtensions.CommandName(code, "RST"))
					parsedLines.Add(new RestartLine(codeLine));
				else if (MyExtensions.CommandName(code, "CP"))
					parsedLines.Add(new SubtractCompareLine(codeLine));
				else if (MyExtensions.CommandName(code, "PUSHO"))
					parsedLines.Add(new PushOptionLine(codeLine));
				else if (MyExtensions.CommandName(code, "POPO"))
					parsedLines.Add(new PopOptionLine(codeLine));
				else if (MyExtensions.CommandName(code, "DI"))
					parsedLines.Add(new DisableInterruptsLine(codeLine));
				else if (MyExtensions.CommandName(code, "HALT"))
					parsedLines.Add(new HaltLine(codeLine));
				else if (MyExtensions.CommandName(code, "JR"))
					parsedLines.Add(new RelativeJumpLine(codeLine));
				else if (MyExtensions.CommandName(code, "XOR"))
					parsedLines.Add(new ExclusiveOrLine(codeLine));
				else if (MyExtensions.CommandName(code, "ADD"))
					parsedLines.Add(new AddLine(codeLine, false));
				else if (MyExtensions.CommandName(code, "ADC"))
					parsedLines.Add(new AddLine(codeLine, true));
				else if (MyExtensions.CommandName(code, "INC"))
					parsedLines.Add(new IncrementLine(codeLine));
				else if (MyExtensions.CommandName(code, "DEC"))
					parsedLines.Add(new DecrementLine(codeLine));
				else if (MyExtensions.CommandName(code, "SUB"))
					parsedLines.Add(new SubtractLine(codeLine, false));
				else if (MyExtensions.CommandName(code, "SBC"))
					parsedLines.Add(new SubtractLine(codeLine, true));
				else if (MyExtensions.CommandName(code, "DB")) //byte 8bit
					parsedLines.Add(new DefineLine(codeLine, typeof(byte)));
				else if (MyExtensions.CommandName(code, "DW")) //word 16bit (short)
					parsedLines.Add(new DefineLine(codeLine, typeof(short)));
				else if (MyExtensions.CommandName(code, "DL")) //double-word/long 32bit (int)
					parsedLines.Add(new DefineLine(codeLine, typeof(int)));
				else if (MyExtensions.CommandName(code, "ENDR"))
					parsedLines.Add(new EndRepeatLine(codeLine));
				else if (MyExtensions.CommandName(code, "ENDC"))
					parsedLines.Add(new EndConditionLine(codeLine));

				else if (MyExtensions.CommandName(code, "WARN"))
					parsedLines.Add(new WarnLine(codeLine));
				else if (MyExtensions.CommandName(code, "FAIL"))
					parsedLines.Add(new FailLine(codeLine));
				else if (MyExtensions.CommandName(code, "CHARMAP"))
					parsedLines.Add(new CharMapLine(codeLine));
				else if (MyExtensions.CommandName(code, "IF"))
					parsedLines.Add(new IfLine(codeLine, false));
				else if (MyExtensions.CommandName(code, "ELIF"))
					parsedLines.Add(new IfLine(codeLine, true));
				else if (MyExtensions.CommandName(code, "ELSE"))
					parsedLines.Add(new ElseLine(codeLine));
				else if (MyExtensions.CommandName(code, "RET"))
					parsedLines.Add(new ReturnLine(codeLine, false));
				else if (MyExtensions.CommandName(code, "RETI"))
					parsedLines.Add(new ReturnLine(codeLine, true));
				else if (string.Equals(code, "EI", StringComparison.OrdinalIgnoreCase))
					parsedLines.Add(new EnableInterruptsLine(codeLine));
				else if (MyExtensions.CommandName(code, "PURGE"))
					parsedLines.Add(new PurgeLine(codeLine));
				else if (MyExtensions.CommandName(code, "REPT"))
					parsedLines.Add(new RepeatLine(codeLine));
				else if (MyExtensions.CommandName(code, "SHIFT"))
					parsedLines.Add(new ShiftLine(codeLine));
				else if (MyExtensions.CommandName(code, "POP"))
					parsedLines.Add(new PopLine(codeLine));
				else if (MyExtensions.CommandName(code, "PUSH"))
					parsedLines.Add(new PushLine(codeLine));
				else if (MyExtensions.CommandName(code, "RL"))
					parsedLines.Add(new RotateLeftLine(codeLine));
				else if (MyExtensions.CommandName(code, "RLA"))
					parsedLines.Add(new RotateALeftLine(codeLine));
				else if (MyExtensions.CommandName(code, "BIT"))
					parsedLines.Add(new BitLine(codeLine));
				else if (MyExtensions.CommandName(code, "LDH"))
					parsedLines.Add(new LoadHighLine(codeLine));
				else if (MyExtensions.CommandName(code, "AND"))
					parsedLines.Add(new AndLine(codeLine));
				else if (MyExtensions.CommandName(code, "CPL"))
					parsedLines.Add(new ComplementLine(codeLine));
				else if (MyExtensions.CommandName(code, "RRCA"))
					parsedLines.Add(new RotateRegisterARightLine(codeLine));
				else if (MyExtensions.CommandName(code, "RLCA"))
					parsedLines.Add(new RotateRegisterALeftLine(codeLine));
				else if (MyExtensions.CommandName(code, "RES"))
					parsedLines.Add(new ResetByteLine(codeLine));
				else if (MyExtensions.CommandName(code, "SCF"))
					parsedLines.Add(new SetCarryFlagLine(codeLine));
				else if (MyExtensions.CommandName(code, "CCF"))
					parsedLines.Add(new ComplementCarryFlagLine(codeLine));
				else if (MyExtensions.CommandName(code, "OPT"))
					parsedLines.Add(new OptionLine(codeLine));
				else if (MyExtensions.CommandName(code, "OR"))
					parsedLines.Add(new OrLine(codeLine));
				else if (MyExtensions.CommandName(code, "DS"))
					parsedLines.Add(new DeclareSpaceLine(codeLine));
				else if (MyExtensions.CommandName(code, "SWAP"))
					parsedLines.Add(new SwapLine(codeLine));
				else if (MyExtensions.CommandName(code, "RR"))
					parsedLines.Add(new RotateRegisterRightLine(codeLine));
				else if (MyExtensions.CommandName(code, "SRL"))
					parsedLines.Add(new ShiftRightLogicLine(codeLine));
				else if (MyExtensions.CommandName(code, "SLA"))
					parsedLines.Add(new ShiftLeftArithmeticLine(codeLine));
				else if (MyExtensions.CommandName(code, "DAA"))
					parsedLines.Add(new DecimalAdjustAccumulatorLine(codeLine));
				else if (MyExtensions.CommandName(code, "RRA"))
					parsedLines.Add(new RotateRegisterRightLine(codeLine));
				else if (MyExtensions.CommandName(code, "STOP")) //do <--
					parsedLines.Add(new StopLine(codeLine));
				else if (MyExtensions.CommandName(code, "ASSERT"))
					parsedLines.Add(new AssertLine(codeLine));
				else if (MyExtensions.CommandName(code, "RRC")) //do <--
					parsedLines.Add(new RotateRegisterRightLine(codeLine));
				else if (MyExtensions.CommandName(code, "RLC")) //do <--
					parsedLines.Add(new RotateRegisterLeftLine(codeLine));
				else if (MyExtensions.CommandName(code, "SRA")) //do <--
					parsedLines.Add(new ShiftRightArithmeticLine(codeLine));
				else if (MyExtensions.CommandName(code, "LOAD")) //do <--
					parsedLines.Add(new Load2Line(codeLine));
				else if (MyExtensions.CommandName(code, "ENDL")) //do <--
					parsedLines.Add(new EndLoadLine(codeLine));

				//else if (code.CommandName("LB")) //do
				//else if (code.CommandName("LDI")) //do <--
				//else if (code.CommandName("LDD")) //do <--
				//else if (code.CommandName("DN")) //do <--

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
						//Debugger.Break();
						//TODO: get proper name spaced label
						//codeLine.Dump();
						//labels.Dump();
					}

					var label = labels.FirstOrDefault();
					parsedLines.Add(new LabelCallLine(codeLine, label));
				}

				else if (Constants.Select(x => x.ConstantName.ToUpper())
					.Contains(codeLine.Code.Split()[0].Trim().ToUpper()))
				{
					var constantName = codeLine.Code.Split()[0].Trim().ToUpper();

					var constants = Constants
						.Where(x => string.Equals(x.ConstantName, constantName, StringComparison.OrdinalIgnoreCase))
						.ToImmutableArray();
					if (constants.Length > 1)
					{
						//Debugger.Break();
						//codeLine.Dump();
						//constants.Dump();
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

			var comment = fileLine.Remove(fileLine.IndexOf(clean), clean.Length).Trim().TrimStart(';').Trim();

			if (string.IsNullOrWhiteSpace(comment))
				comment = null;
			return comment?.Trim();
		}

		public static List<string> GetStrings(string code)
		{
			if (string.IsNullOrWhiteSpace(code)) return new List<string>();

			var matches = GetStringsRegex.Matches(code);

			var returned = matches.Select(x => x.Value?.TrimStart('"').TrimEnd('"')).ToList();

			return returned.Count > 0 ? returned : null;
		}

		public static List<string> GetParameters(string code)
		{
			if (string.IsNullOrWhiteSpace(code)) return new List<string>();

			var matches = new List<string>();

			while (code.Length > 0)
			{
				var parameter = GetParameter(code);
				matches.Add(parameter);
				code = code[parameter.Length..].TrimStart(',').Trim();
			}

			return matches.Count > 0 ? matches : null;
		}

		private static string GetParameter(string code)
		{
			var i = 0;
			var isString = code[0] == '\"';
			for (; i < code.Length; i++)
			{
				if (!isString && code[i] == ',') break;
				if (!isString || i <= 0 || code[i] != '\"' || code[i - 1] == '\\') continue;
				i++;
				break;
			}

			return code.Substring(0, i);
		}
	}
}
using System;
using RGBDS2CIL;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;
using System.IO;
using System.Text;

// ReSharper disable ExceptionNotDocumented

namespace Tests
{
	public class ParsingTests
	{
		private readonly ITestOutputHelper _testOutputHelper;
		public ParsingTests(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		private readonly string FileName = "unittest.asm";

		[Theory]
		[InlineData("charmap \":\",         $9c")]
		[InlineData("charmap \";\",         $9d ; actual comment")]
		[InlineData("charmap \",\",         $f4")]
		public void CharMapLine(string fileLine)
		{
			var comment = Parser.GetComment(fileLine);

			var code = Parser.RemoveCommentFromCode(fileLine);
			var codeLine = new CodeLine(code, fileLine, comment, FileName, 0, Parser.GetStrings(code));
			var charMapLine = new CharMapLine(codeLine);

			//if (assertLine.Strings.Contains(";"))
			//	Assert.False(string.IsNullOrWhiteSpace(assertLine.Message));

			//Assert.DoesNotContain(assertLine.Condition, assertLine.Message);
			//Assert.False(assertLine.Condition.StartsWith("ASSERT", System.StringComparison.OrdinalIgnoreCase));
		}

		[Theory]
		[InlineData("charmap \";\",         $9d")]
		[InlineData("charmap \";\",         $9d ; actual comment")]
		[InlineData(";;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;")]
		public void GetComment(string fileLine)
		{
			var comment = Parser.GetComment(fileLine);

			var code = Parser.RemoveCommentFromCode(fileLine);

			_testOutputHelper.WriteLine("fileLine: " + fileLine);
			_testOutputHelper.WriteLine("comment: " + comment);
			_testOutputHelper.WriteLine("code: " + code);
		}

		[Theory]
		[InlineData("cp   STRSUB(\\1, 1 + I, 1) + $01")]
		public void SubtractCompare(string fileLine)
		{
			var comment = Parser.GetComment(fileLine);

			var code = Parser.RemoveCommentFromCode(fileLine);
			var codeLine = new CodeLine(code, fileLine, comment, FileName, 0, Parser.GetStrings(code));
			var subtractCompareLine = new SubtractCompareLine(codeLine);

			Assert.NotNull(subtractCompareLine.Value);
		}

		[Theory]
		[InlineData("ASSERT LOW(Variable) == 0")]
		[InlineData("assert Variable + 1 == OtherVariable")]
		[InlineData("assert BaseStatsEnd - BaseStats == (wMonHeaderEnd - wMonHeader) * (NUM_POKEMON - 1) ; discount Mew")]
		[InlineData("assert NUM_TMS == const_value - TM01, \"NUM_TMS({d: NUM_TMS}) does not match the number of add_tm definitions\"")]
		public void AssertLine(string fileLine)
		{
			var comment = Parser.GetComment(fileLine);

			var code = Parser.RemoveCommentFromCode(fileLine);
			var codeLine = new CodeLine(code, fileLine, comment, FileName, 0, Parser.GetStrings(code));
			var assertLine = new AssertLine(codeLine);

			Assert.DoesNotContain(assertLine.Condition, assertLine.Message);
			Assert.False(assertLine.Condition.StartsWith("ASSERT", System.StringComparison.OrdinalIgnoreCase));
		}

		[Fact]
		public void GetAssert()
		{
			var fileLines = new List<(string Line, string Condition, string Message)>();

			fileLines.AddRange(new[] {
				(
					"ASSERT !STRIN(\\1, \"@\"), STRCAT(\"String terminator \\\"@\\\" in list entry: \", \\1)",
					"!STRIN(\\1, \"@\")",
					"STRCAT(\"String terminator \\\"@\\\" in list entry: \", \\1)"
				),
				(
					"assert 0 <= (\\1) && (\\1) <= 31, \"RGB channel must be 0-31\"",
					"0 <= (\\1) && (\\1) <= 31",
					"\"RGB channel must be 0-31\""
				),
				(
					"assert NUM_TMS == const_value - TM01, \"NUM_TMS({d: NUM_TMS}) does not match the number of add_tm definitions\"",
					"NUM_TMS == const_value - TM01",
					"\"NUM_TMS({d: NUM_TMS}) does not match the number of add_tm definitions\""
				)
			});

			foreach (var fileLine in fileLines)
			{
				var assertLine = BuildAssertLine(fileLine.Line);
				Assert.NotNull(assertLine.Condition);// all asserts must have a conditon
				Assert.Equal(fileLine.Condition, assertLine.Condition);
				Assert.Equal(fileLine.Message, assertLine.Message); //optional
			}
		}

		private AssertLine BuildAssertLine(string fileLine)
		{
			var comment = Parser.GetComment(fileLine);

			var code = Parser.RemoveCommentFromCode(fileLine);
			var codeLine = new CodeLine(code, fileLine, comment, FileName, 0, Parser.GetStrings(code));
			var assertLine = new AssertLine(codeLine);

			_testOutputHelper.WriteLine("fileLine: " + fileLine);
			//_testOutputHelper.WriteLine("comment: " + comment);
			_testOutputHelper.WriteLine("code: " + code);
			return assertLine;
		}

		[Theory]
		[InlineData(@"ld_long: MACRO
	IF STRLWR(""\1"") == ""a"" 
		; ld a, [$ff40]
		db $FA
		dw \2
	ELSE 
		IF STRLWR(""\2"") == ""a"" 
			; ld [$ff40], a
			db $EA
			dw \1
		ENDC
	ENDC
ENDM")]
		[InlineData(@"IF(!DEF(VERSION))
VERSION equs ""0""
ENDC")]
		public void NestedIf(string ifBlock)
		{
			_testOutputHelper.WriteLine(ifBlock);

			var parsedLines = Parser.GetLines(ifBlock.Split(Environment.NewLine), FileName);


			Restructure.RestructureMacros(parsedLines);
			Restructure.RestructureIfs(parsedLines);

			var sb = CSharp.GenerateCsharp(FileName, parsedLines, Path.GetTempPath());

			_testOutputHelper.WriteLine(sb);

			var serializedJson = Parser.ExportJson(parsedLines);
			_testOutputHelper.WriteLine(serializedJson);
		}

		[Theory]
		[InlineData("FALSE equ 0")]
		[InlineData("BGSET		EQU	OBJSET+$1000	;$800")]
		public void Equ(string line)
		{
			var comment = Parser.GetComment(line);

			var code = Parser.RemoveCommentFromCode(line);
			var codeLine = new CodeLine(code, line, comment, FileName, 0, Parser.GetStrings(code));
			var constant = new ConstantLine(codeLine, "EQU");

			var sb = new StringBuilder();
			constant.OutputLine(sb, 0);
			_testOutputHelper.WriteLine(sb.ToString());

			if (sb.ToString().EndsWith('"'))
				Assert.DoesNotContain("equ", sb.ToString());
		}
	}
}
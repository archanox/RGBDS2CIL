using RGBDS2CIL;
using System.Diagnostics;
using Xunit;
// ReSharper disable ExceptionNotDocumented

namespace Tests
{
	public class ParsingTests
	{
		[Fact]
		public void Test1()
		{

		}

		[Theory]
		[InlineData("ASSERT LOW(Variable) == 0")]
		[InlineData("assert Variable + 1 == OtherVariable")]
		[InlineData("	assert BaseStatsEnd - BaseStats == (wMonHeaderEnd - wMonHeader) * (NUM_POKEMON - 1) ; discount Mew")]
		[InlineData("assert NUM_TMS == const_value - TM01, \"NUM_TMS({d: NUM_TMS}) does not match the number of add_tm definitions\"")]
		public void AssertLine(string fileLine)
		{
			var comment = Parser.GetComment(fileLine);

			var code = Parser.RemoveCommentFromCode(fileLine);
			var codeLine = new CodeLine(code, fileLine, comment, "", 0, Parser.GetStrings(code));
			var assertLine = new AssertLine(codeLine);

			if (assertLine.Code.Contains(','))
				Assert.False(string.IsNullOrWhiteSpace(assertLine.Message));

			Assert.DoesNotContain(assertLine.Condition, assertLine.Message);
			Assert.False(assertLine.Condition.StartsWith("ASSERT", System.StringComparison.OrdinalIgnoreCase));
		}

		[Theory]
		[InlineData("charmap \":\",         $9c")]
		[InlineData("charmap \";\",         $9d ; actual comment")]
		[InlineData("charmap \",\",         $f4")]
		public void CharMapLine(string fileLine)
		{
			var comment = Parser.GetComment(fileLine);

			var code = Parser.RemoveCommentFromCode(fileLine);
			var codeLine = new CodeLine(code, fileLine, comment, "", 0, Parser.GetStrings(code));
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

			Debug.WriteLine("fileLine: " + fileLine);
			Debug.WriteLine("comment: " + comment);
			Debug.WriteLine("code: " + code);
		}

		[Theory]
		[InlineData("cp   STRSUB(\\1, 1 + I, 1) + $01")]
		public void SubtractCompare(string fileLine)
		{
			var comment = Parser.GetComment(fileLine);

			var code = Parser.RemoveCommentFromCode(fileLine);
			var codeLine = new CodeLine(code, fileLine, comment, "", 0, Parser.GetStrings(code));
			var subtractCompareLine = new SubtractCompareLine(codeLine);




		}

		// 		[Theory]
		// 		[InlineData(@"RawBitmap:
		// 	if BuildCPCv+BuildENTv
		// 		if ScrColor16
		// 			nop
		// 		endc
		// 	endc
		// ")]
		// 		public void NestedIfParse(string sampleLines)
		// 		{
		// 			var parsedLines = Parser.GetLines(sampleLines.Split(new [] {Environment.NewLine, "\n", "\r"}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries), "unitTest.asm");
		// 			Program.RestructureLines(parsedLines);
		// 		}


		[Theory]
		[InlineData("	ASSERT !STRIN(\\1, \"@\"), STRCAT(\"String terminator \\\"@\\\" in list entry: \", \\1)")]
		public void GetAssert(string fileLine)
		{
			var comment = Parser.GetComment(fileLine);

			var code = Parser.RemoveCommentFromCode(fileLine);
			var codeLine = new CodeLine(code, fileLine, comment, "", 0, Parser.GetStrings(code));
			var assertLine = new AssertLine(codeLine);

			Debug.WriteLine("fileLine: " + fileLine);
			Debug.WriteLine("comment: " + comment);
			Debug.WriteLine("code: " + code);
		}
	}
}

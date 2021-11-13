using System;
using System.Diagnostics;
using System.Linq;

namespace RGBDS2CIL
{
	public class AssertLine : CodeLine
	{
		public string Condition;
		public string Message;

		public AssertLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			var parameters = base.Code.Substring(base.Code.IndexOf("ASSERT", StringComparison.OrdinalIgnoreCase) + "ASSERT".Length).Trim();
			
			var splitParameters = Parser.GetParameters(parameters);

			Condition = splitParameters?.First();
			if(splitParameters?.Count == 2)
				Message = splitParameters?.LastOrDefault();
			
			Debug.Assert(splitParameters?.Count <= 2, "More than 2 parameters for an ASSERT are unsupported.");
		}
	}
}
using System;
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

			Condition = base.Code.Substring(base.Code.IndexOf("ASSERT", StringComparison.OrdinalIgnoreCase) + "ASSERT".Length).Trim();

			//todo: check for ,
			Message = base.Strings?.SingleOrDefault();

			if (Message is not null)
			{
				Condition = Condition.Remove(Condition.IndexOf(Message, StringComparison.Ordinal)).TrimEnd(',');
			}
		}
	}
}
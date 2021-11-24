using System;

namespace RGBDS2CIL
{
	public class JumpLine : CodeLine
	{
		public string JumpDestination;

		public JumpLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			JumpDestination = base.Code[(base.Code.IndexOf("JP", StringComparison.OrdinalIgnoreCase) + "JP".Length)..]
				.Trim();
		}
	}
}
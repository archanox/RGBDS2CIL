using System;

namespace RGBDS2CIL
{
	public class SwapLine : CodeLine
	{
		public string Swap;

		public SwapLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Swap = base.Code[(base.Code.IndexOf("SWAP", StringComparison.OrdinalIgnoreCase) + "SWAP".Length)..].Trim();
		}
	}
}
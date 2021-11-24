using System;

namespace RGBDS2CIL
{
	public class RotateLeftLine : CodeLine
	{
		public string Rotate;

		public RotateLeftLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Rotate = base.Code[(base.Code.IndexOf("RL", StringComparison.OrdinalIgnoreCase) + "RL".Length)..].Trim();
		}
	}
}
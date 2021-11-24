using System.Linq;

namespace RGBDS2CIL
{
	/// <summary>
	/// Set bit u3 in register r8 to 0.
	/// Set bit u3 in the byte pointed by HL to 0.
	/// </summary>
	public class ResetByteLine : CodeLine
	{
		public string SetBit;
		public string Value;

		public ResetByteLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			SetBit = codeLine.Code["RES".Length..].Split(',')[0].Trim();
			Value = codeLine.Code.Split(',').Last().Trim();
		}
	}
}
using System.Linq;

namespace RGBDS2CIL
{
	public class CallLine : CodeLine
	{
		public string Call;

		public CallLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Call = codeLine.Code.Split().Last();
		}
	}
}
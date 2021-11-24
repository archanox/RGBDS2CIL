using System.Linq;

namespace RGBDS2CIL
{
	public class PurgeLine : CodeLine
	{
		public string Purge;

		public PurgeLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Purge = codeLine.Code.Split().Last();
		}
	}
}
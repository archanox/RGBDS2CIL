using System.Linq;

namespace RGBDS2CIL
{
	public class RestartLine : CodeLine
	{
		public string Restart;

		public RestartLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			Restart = codeLine.Code.Split().Last();
		}
	}
}
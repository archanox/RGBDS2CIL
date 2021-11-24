namespace RGBDS2CIL
{
	public class LabelCallLine : CodeLine
	{
		public LabelLine LabelLine { get; set; }

		public LabelCallLine(CodeLine codeLine, LabelLine labelLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			LabelLine = labelLine;
		}
	}
}
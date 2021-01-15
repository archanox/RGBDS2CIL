namespace RGBDS2CIL
{
	public class DecimalAdjustAccumulatorLine : CodeLine
	{
		public DecimalAdjustAccumulatorLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;
		}
	}
}
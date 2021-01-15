namespace RGBDS2CIL
{
	public class ConstantAssignLine : CodeLine
	{
		public ConstantLine ConstantLine { get; set; }

		public ConstantAssignLine(CodeLine codeLine, ConstantLine constantLine) : base(codeLine.Code, codeLine,
			codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			ConstantLine = constantLine;
		}
	}
}
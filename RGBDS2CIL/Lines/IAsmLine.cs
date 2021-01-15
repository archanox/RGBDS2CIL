namespace RGBDS2CIL
{
	public interface IAsmLine
	{
		public string Raw { get; set; }
		public string Comment { get; set; }
		public string FileName { get; set; }
		public int Line { get; set; }
		IAsmLine Reparse();
	}
}
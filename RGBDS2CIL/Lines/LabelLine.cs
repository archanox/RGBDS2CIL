using System.Linq;

namespace RGBDS2CIL
{
	public class LabelLine : CodeLine
	{
		public bool IsGlobal { get; set; }
		public bool HasExport { get; set; }
		public string LabelName;

		public LabelLine(CodeLine codeLine) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			base.Comment = codeLine.Comment;
			base.Raw = codeLine.Raw;

			IsGlobal = base.Code.Split(':').Last() == string.Empty;

			HasExport = base.Code.EndsWith("::");

			LabelName = base.Code.Trim().Split('.').Last().Trim(':');
			if (string.IsNullOrWhiteSpace(LabelName))
				LabelName = base.Code.Trim().Split(':')[0];
		}
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace RGBDS2CIL
{
	public class IncludeLine : CodeLine, IAsmLine
	{
		public readonly string IncludeFile;
		public bool IsBinary { get; set; }
		public byte[] Binary { get; set; }
		public uint? Start { get; set; }
		public uint? End { get; set; }
		public List<IAsmLine> Lines { get; set; } = new();

		public IncludeLine(CodeLine codeLine, bool isBinary) : base(codeLine.Code, codeLine, codeLine.Strings)
		{
			IncludeFile = Parser.GetStrings(codeLine.Code).Single().TrimStart('"').TrimEnd('"');
			IsBinary = isBinary;

			
			if (isBinary)
			{
				var parameters = Parser.GetParameters(codeLine.Code).Skip(1).ToArray();
				//INCBIN "file",<start>[,<stop>]
				//INCBIN "baserom.gb", $0, $40 - $0
				if (parameters.Any())
				{
					var isHex = parameters[0].StartsWith("0x");
					try
					{
						Start = Convert.ToUInt32(parameters[0], isHex ? 16 : 10);
					}
					catch (OverflowException)
					{
						Console.WriteLine($"Unable to convert offset '{parameters[0]}' to an unsigned integer.");
						throw;
					}
					if (parameters.Length == 2)
					{
						//TODO: move this over into the outputted c#
						try
						{
							var result2 = CSharpScript.EvaluateAsync(parameters[1]).Result;
							End = Convert.ToUInt32(result2);
						}
						catch (OverflowException)
						{
							Console.WriteLine($"Unable to convert length '{parameters[1]}' to an unsigned integer.");
							throw;
						}
					}
				}
			}
		}

		public void ReadBinaryFile(string path)
		{
			if (Start is not null)
			{
				using (var reader = new BinaryReader(File.Open(path, FileMode.Open)))
				{
					var length = reader.BaseStream.Length;
					if (End.HasValue)
						length = End.Value + Start.Value;

					if (length > Start.Value)
					{
						reader.BaseStream.Seek(Start.Value, SeekOrigin.Begin);
						Binary = reader.ReadBytes((int)(length - Start.Value));
					}
				}
			}
			else
				Binary = File.ReadAllBytes(path);
		}

		public new void OutputLine(StringBuilder sb, int tabCount)
		{
			//BUG: shouldn't need to check for null Binary here, should always be available before it gets here
			if (IsBinary && Binary is not null)
			{
				sb.Append(new string('\t', tabCount)).Append("var sevenItems = new byte[] {");
				for (var i = 0; i < Binary.Length; i++)
				{
					sb.Append(" 0x").Append(Binary[i].ToString("x4"));
					if(i < Binary.Length - 1)
						sb.Append(',');
				}
				sb.Append(" };");

				sb.AppendComment(Comment);
			}
			else if(IsBinary)
				sb.Append(new string('\t', tabCount)).Append("/* ").Append(Code).Append(" */").AppendComment(Comment);
		}
	}
}
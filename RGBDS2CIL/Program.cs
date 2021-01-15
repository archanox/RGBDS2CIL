using System.Collections.Generic;
using System.IO;

namespace RGBDS2CIL
{
	public class Program
	{
		public static void Main()
		{
			//https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.assemblybuilder?view=netcore-3.1

			var files = new[]
			{
				@"C:\source\pokered\macros/const.asm",

				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\hello.asm",
				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\hello-world.asm",
				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\score_bcd.asm",
				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\score_hex.asm",

				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\DMG_ROM.asm",
				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\dmg_boot.asm",
				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\dmg0_rom.asm",
				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\fortune_rom.asm",
				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\gamefighter_rom.asm",
				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\dmg_boot (2) orig.asm",
				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\dmg_boot (2).asm",

				@"C:\source\pokered\main.asm",
				@"C:\source\pokered\home.asm",
				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\Pokemon Red (UE) [S][!].asm",

				@"C:\Users\hce_a\OneDrive\Documents\Gameboy Hello World\sources\Multiplatform\Sources\SimpleHelloWorld\GB_HelloWorld.asm"

			};

			foreach (var fileName in files)
			{
				var fileLines1 = File.ReadAllLines(fileName);
				Parser.RootFolder = Path.GetDirectoryName(fileName);
				fileLines1 = Parser.FlattenMultiLine(fileLines1);
				var parsedLines = Parser.GetLines(fileLines1, fileName);

				//TODO compile checks: see if all the values exist


				RestructureLines(parsedLines);


				//var c = new Foo.C();
				//c.M();

				Parser.ExportJson(fileName, parsedLines);

				//var deserialized = JsonConvert.DeserializeObject<IAsmLine>(serialized, settings);

				//CIL.GenerateCIL();

				CSharp.GenerateCsharp(fileName, parsedLines);
			}
		}

		internal static void RestructureLines(List<IAsmLine> parsedLines)
		{
			Restructure.RestructureMacros(parsedLines);
			Restructure.RestructureIfs(parsedLines);
		}
	}
}
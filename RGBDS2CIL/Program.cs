using System;
using System.Collections.Generic;
using System.IO;

namespace RGBDS2CIL
{
	public static class Program
	{
		public static void Main()
		{
			//https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.assemblybuilder?view=netcore-3.1
			//https://eldred.fr/gb-asm-tutorial/what_is_gb.html
			//http://gameboy.mongenel.com/asmschool.html
			//http://gameboy.mongenel.com/dmg/mbc3.txt //Pokemon Red
			//https://binji.github.io/posts/pokegb/ //emulating just pokemon red


			var files = new[]
			{
				@"tetris.asm",
				//@"LADX-Disassembly\src\main.asm", //weird includes

				@"DKGBDisasm\home.asm",
				@"DKGBDisasm\main.asm",

				@"kirbydreamland\main.asm", //nested if issue

				@"marioland2\home.asm",
				@"marioland2\main.asm",

				@"mmania\main.asm",

				@"rgbds-template\src\hello-world.asm",

				@"supermarioland\bank0.asm",

				@"blankasm.asm",
				@"hello.asm",
				@"hello-world.asm",
				@"mrdo.asm",

				@"pokered\macros/const.asm",

				@"hello.asm",
				@"hello-world.asm",
				@"score_bcd.asm",
				@"score_hex.asm",

				@"DMG_ROM.asm",
				@"dmg_boot.asm",
				@"dmg0_rom.asm",
				@"fortune_rom.asm",
				@"gamefighter_rom.asm",
				@"dmg_boot (2) orig.asm",
				@"dmg_boot (2).asm",

			 	@"pokered\main.asm",
			 	@"pokered\home.asm",
			 	@"Pokemon Red (UE) [S][!].asm",

			};


			foreach (var file in files)
			{
				//Console.WriteLine();
				//Console.WriteLine(file);
				//Console.WriteLine(new string('=', file.Length));
				//Console.WriteLine();

				var fileName = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "..", "Assembly", file);
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
				//TODO: base the CIL off of the decompiled c#
				//CIL.GenerateCIL();

				var root = Path.GetDirectoryName(fileName);

				CSharp.GenerateCsharp(fileName, parsedLines, root);
			}
		}

		public static void RestructureLines(List<IAsmLine> parsedLines)
		{
			Restructure.RestructureMacros(parsedLines);
			Restructure.RestructureIfs(parsedLines);
		}
	}
}
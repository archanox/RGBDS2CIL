using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

			//https://sneslab.net/wiki/Graphics_Format#2bpp

			var files = new[]
			{
				//@"DKGBDisasm/macros/gfx.asm",

				//@"tetris_disassembly/main.asm",
				//@"LADX-Disassembly/src/main.asm",

				//@"DKGBDisasm/home.asm",
				//@"DKGBDisasm/main.asm",

				//@"kirbydreamland/main.asm",

				//@"marioland2/home.asm",
				//@"marioland2/main.asm",

				////@"mmania\main.asm", //INCBIN too slow...

				//@"rgbds-template/src/hello-world.asm",

				//@"supermarioland/bank0.asm",
				//@"supermarioland/bank1.asm",
				//@"supermarioland/bank2.asm",
				//@"supermarioland/bank3.asm",
				//@"supermarioland/music.asm",

				//@"blankasm.asm",
				//@"hello.asm",
				//@"hello-world.asm",
				//@"mrdo.asm",

				//@"hello.asm",
				//@"hello-world.asm",
				//@"score_bcd.asm",
				//@"score_hex.asm",

				//@"DMG_ROM.asm",
				//@"dmg_boot.asm",
				//@"dmg0_rom.asm",
				//@"fortune_rom.asm",
				//@"gamefighter_rom.asm",
				//@"dmg_boot (2) orig.asm",
				//@"dmg_boot (2).asm",

				@"pokered/macros/const.asm",
				@"pokered/main.asm",
				@"pokered/home.asm",
				//@"Pokemon Red (UE) [S][!].asm",

				//@"poketcg/src/main.asm",

				//@"shi-kong-xing-shou/main.asm",
				//@"shi-kong-xing-shou/home.asm",

				//@"telefang/telefang.inc",

				//@"robopon/home.asm",
				//@"robopon/main.asm",
			};

			// Find the Assembly directory by looking for it in the current or parent directories
			var currentDir = Environment.CurrentDirectory;
			var assemblyDir = Path.Combine(currentDir, "..", "..", "..", "..", "Assembly");
			
			// Normalize the path
			assemblyDir = Path.GetFullPath(assemblyDir);
			
			// If Assembly doesn't exist at that location, try relative to the solution directory
			if (!Directory.Exists(assemblyDir))
			{
				// Try to find the solution root by looking for the .sln file
				var searchDir = currentDir;
				for (int i = 0; i < 10; i++)
				{
					if (Directory.GetFiles(searchDir, "*.sln").Length > 0)
					{
						assemblyDir = Path.Combine(searchDir, "Assembly");
						break;
					}
					searchDir = Path.GetFullPath(Path.Combine(searchDir, ".."));
				}

				// After searching, ensure that the Assembly directory was actually found
				if (!Directory.Exists(assemblyDir))
				{
					throw new DirectoryNotFoundException(
						"Unable to locate the 'Assembly' directory. " +
						"Starting from '" + currentDir + "', searched up to 10 parent directories " +
						"for a solution (.sln) file and an 'Assembly' folder.");
				}
			}

			foreach (var file in files)
			{
				Console.WriteLine();
				Console.WriteLine(file);
				Console.WriteLine(new string('=', file.Length));
				Console.WriteLine();
				
				var fileName = Path.Combine(assemblyDir, file);
				var fileLines1 = File.ReadAllLines(fileName);
				Parser.RootFolder = Path.GetDirectoryName(fileName);
				fileLines1 = Parser.FlattenMultiLine(fileLines1);
				var parsedLines = Parser.GetLines(fileLines1, fileName);

				//TODO compile checks: see if all the values exist


				Restructure.RestructureMacros(parsedLines);
				Restructure.RestructureIfs(parsedLines);
				Restructure.RestructureRepeats(parsedLines);

				//var c = new Foo.C();
				//c.M();

				//var serializedJson = Parser.ExportJson(parsedLines);
				//File.WriteAllText(fileName + ".json", serializedJson);

				//var deserialized = JsonConvert.DeserializeObject<IAsmLine>(serialized, settings);
				//TODO: base the CIL off of the decompiled c#
				//CIL.GenerateCIL();

				var root = Path.GetDirectoryName(fileName);

				var sb = CSharp.GenerateCsharp(fileName, parsedLines, root);

				File.WriteAllText(fileName + ".cs", sb);
			}
		}
	}
}
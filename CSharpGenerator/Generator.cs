using Microsoft.CodeAnalysis;

namespace CSharpGenerator
{
	[Generator]
	public class Generator : ISourceGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{
		}

		public void Execute(GeneratorExecutionContext context)
		{
			var source = @"
using System;

namespace Foo { 
public class C
	{
		public void M()
		{
			Console.WriteLine(""Hello World"");
		}
	}
}";

			if (source != null)
			{
				context.AddSource("generated.cs", source);
			}
		}


	}
}

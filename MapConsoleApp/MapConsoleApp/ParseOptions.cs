using System;
using CommandLine;
using MapConsoleApp.Models;

namespace MapConsoleApp
{
	public static class ParseOptions
	{
		public static Options Parse(this string[] args)
		{
            ParserResult<Options> argsResult = CommandLine.Parser.Default.ParseArguments<Options>(args);

            if (argsResult.Tag == ParserResultType.Parsed)
            {
                var parsed = (Parsed<Options>)argsResult;
            }
            else
            {
                Console.WriteLine("オプションを正しく指定してください．");
                Environment.Exit(1);
            }

            Options opt = new Options();

			opt.InputPath = argsResult.Value.InputPath;
			opt.Width = argsResult.Value.Width;
			opt.Height = argsResult.Value.Height;
			opt.GradientPath = argsResult.Value.GradientPath;
			opt.OutputPath = argsResult.Value.OutputPath;
            opt.ShouldInterp = argsResult.Value.ShouldInterp;

			return opt;
        }
	}
}


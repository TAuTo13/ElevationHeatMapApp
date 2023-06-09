using System;
using CommandLine;
using MapConsoleLegend.Models;

namespace MapConsoleLegend
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

            opt.GradientPath = argsResult.Value.GradientPath;
            opt.Height = argsResult.Value.Height;
            opt.Width = argsResult.Value.Width;
            opt.OutputPath = argsResult.Value.OutputPath;
            opt.Name = argsResult.Value.Name;
            opt.ShouldAlpha = argsResult.Value.ShouldAlpha;

            return opt;
        }
    }
}


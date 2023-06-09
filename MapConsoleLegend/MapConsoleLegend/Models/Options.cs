using System;
namespace MapConsoleLegend.Models
{
    public class Options
    {

        [CommandLine.Option('h', "height", Required = false, HelpText = "Set legend's height.(optional)")]
        public int Height { get; set; } = 0;

        [CommandLine.Option('w', "width", Required = false, HelpText = "Set legend's width.(optioal)")]
        public int Width { get; set; } = 0;

        [CommandLine.Option('g',"gradient",Required = true,HelpText = "Set the path for input json file mapped Elevations and colors for Gradation.")]
        public string GradientPath { get; set; }

        [CommandLine.Option('o',"output",Required = true, HelpText = "Set the output directry path.")]
        public string OutputPath { get; set; }

        [CommandLine.Option('n', "name", Required = false, HelpText = "Set the output image's name.(optional)")]
        public string Name { get; set; } = string.Empty;

        [CommandLine.Option('a',"alpha",Required = false,HelpText = "Set the backgraund color to alpha.(optional")]
        public bool ShouldAlpha { get; set; }
    }
}


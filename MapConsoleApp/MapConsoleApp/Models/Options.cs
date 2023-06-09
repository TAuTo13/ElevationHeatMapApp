using System;
namespace MapConsoleApp.Models
{
	public class Options
	{
		[CommandLine.Option('i',"input",Required = true,HelpText = "Set the input directry path.")]
		public string InputPath
		{
			get;
			set;
		}

		[CommandLine.Option('w',"width",Required = true,HelpText = "Set the output images' width.")]
		public int Width
		{
			get;
			set;
		}

		[CommandLine.Option('h',"height",Required = true,HelpText = "Set the output images' height.")]
		public int Height
		{
			get;
			set;
		}

		[CommandLine.Option('g',"gradient",Required=true,HelpText = "Set the path for input json file mapped Elevations and colors for Gradation.")]
		public string GradientPath
		{
			get;
			set;
		}

		[CommandLine.Option('o',"output",Required = true,HelpText = "Set the output directry path.")]
		public string OutputPath
		{
			get;
			set;
		}

		[CommandLine.Option('p', "interp", Required = false, HelpText = "Interpolate image pixels.(optional)")]
		public bool ShouldInterp
		{
			get;
			set;
		}
	}
}


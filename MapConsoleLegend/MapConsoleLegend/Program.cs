
using SkiaSharp;
using System.IO;
using System.Text;

namespace MapConsoleLegend
{
    class Program
    {
        private static readonly string DEFAULTNAME = "Legend";

        static void Main(string[] args)
        {
            var gradient = new ColorLegend();

            var param = args.Parse();

            gradient.LoadJson(param.GradientPath);

            var drawer = new LegendImgDrawer(gradient);

            var info = new SKImageInfo(param.Width,param.Height);
            var surface = drawer.OnDrawCanvas(info,param.ShouldAlpha);

            string name = param.Name != string.Empty ? param.Name : DEFAULTNAME;
            string path = param.OutputPath;
            EnsureDirectory(path);

            string filename = Path.ChangeExtension(name, ".png");
            string fullpath = Path.Combine(path, filename);
            try
            {
                using (var stream = File.Create(fullpath))
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    data.SaveTo(stream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Exported ");
            sb.Append(fullpath);


            Console.WriteLine(sb.ToString());
        }

        private static void EnsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (!File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                Console.WriteLine("Output Path is not a directory.");
                Environment.Exit(1);
            }
        }
    }
}


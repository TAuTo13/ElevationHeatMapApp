using System.Text;
using NetTopologySuite.IO;
using SkiaSharp;
using System.Diagnostics;
using System.IO;
using System;
using MapConsoleApp.Models;
using MapConsoleApp.ShapeFile;
using MapConsoleApp.ColorGradient;

namespace MapConsoleApp {
    class Program
    {
        //時間計測用
        static Stopwatch sw = new Stopwatch();
        static StringBuilder sb = new StringBuilder();

        static void LogWithProcessTime(string logTxt)
        {
            var ts = sw.ElapsedMilliseconds;
            sb.Clear();

            sb.Append(logTxt);
            sb.Append(" (");
            sb.Append(ts);
            sb.Append(" ms)");
            Console.WriteLine(sb.ToString());
        }
        static void LogWithProcessTime(string[] logTxt)
        {
            var ts = sw.ElapsedMilliseconds;
            sb.Clear();

            foreach(string x in logTxt) sb.Append(x);

            sb.Append(" (");
            sb.Append(ts);
            sb.Append(" ms)");
            Console.WriteLine(sb.ToString());
        }


        static void Main(string[] args)
        {
            //コマンドオプションをパース
            var param = args.Parse();

            if (param.ShouldInterp)
            {
                Console.WriteLine("Interpolate:True");
            }
            else
            {
                Console.WriteLine("Interpolate:False");
            }

            var facade = new ShapeImagesFacade(param.InputPath);

            //インプットパスからshapeファイルを取得
            sw.Start();
            facade.LoadShapeFiles();
            sw.Stop();

            LogWithProcessTime("Loaded shapefiles");

            //jsonからグラデーションを取得
            sw.Reset();
            sw.Start();
            var blend = new ColorGradientBlend();
            blend.LoadJson(param.GradientPath);
            sw.Stop();
            LogWithProcessTime("Loaded gradient json file");

            //画像ジェネレータを取得
            var generator = new ShapeImageGenerator
            {
                Width = param.Width,
                Height = param.Height,
                Blend = blend
            };

            facade.Generator = generator;

            Console.WriteLine("Start image generating process...");
            sw.Reset();
            sw.Start();
            facade.GenerateImgs(param.ShouldInterp);
            sw.Stop();

            LogWithProcessTime("Images generated");

            Console.WriteLine("Saving images...");

            sw.Reset();
            sw.Start();
            int cnt=facade.SaveImgs(param.OutputPath);
            sw.Stop();

            LogWithProcessTime(new string[]
                { "PNG Images saved(",cnt.ToString(),
                    "/",facade.ShapesCount.ToString(),") at:" ,param.OutputPath ,
                    " size:", param.Width.ToString(),"*", param.Height.ToString() });

            sw.Reset();
            sw.Start();
            bool flag = facade.SaveBounds(param.OutputPath);
            sw.Stop();

            if (flag)
                LogWithProcessTime("Exported BoundingBox CSV file");

        }
    }
}



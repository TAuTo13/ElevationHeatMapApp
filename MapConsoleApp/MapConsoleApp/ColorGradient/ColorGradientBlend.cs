using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using SkiaSharp;
using MapConsoleApp.Models;


namespace MapConsoleApp.ColorGradient
{
	public class ColorGradientBlend
	{
		private SKColor[] _colors;
		private double[] _offsets;

        public void LoadJson(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("PATH:" + path + "  is not exist.");
                Environment.Exit(1);
            }
            if (Path.GetExtension(path) != Extensions.JSON)
            {
                Console.WriteLine("Path file is not " + Extensions.JSON + " file.");
                Environment.Exit(1);
            }

            StreamReader sr;
            string jsonStr = string.Empty;
            try
            {
                sr = new StreamReader(path);
                jsonStr = sr.ReadToEnd();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }


            BlendModel? blendModel = JsonSerializer.Deserialize<BlendModel>(jsonStr);

            if (blendModel is null)
            {
                Console.WriteLine("Failed to deserialize json.");
                Environment.Exit(1);
            }

            Initialize(blendModel.Colors, blendModel.Positions);
        }

		private void Initialize(string[] _colors, double[] _offsets)
		{
            if (_colors.Length != _offsets.Length)
            {
                Console.WriteLine("Color[] and offset[] arrays' size are not match.");
                Environment.Exit(1);
            }

            this._offsets = _offsets;
            this._colors = new SKColor[_colors.Length];

            for (int i = 0; i < _colors.Length; i++)
            {
                this._colors[i] = SKColor.Parse(_colors[i]);
            }
        }

        //標高からそのポイントが含まれるグラデーションの範囲を取得
		private int GetOffsetIndex(double offset)
		{
            int i;
            int start = 0;
            int end = _offsets.Length - 1;
			do
			{
                i = (end - start) / 2 + start;
                if (Double.Equals(offset, _offsets[i]))
                {
                    return i;
                }
				else if (offset > _offsets[i])
				{
                    start = i + 1;
                }
				else
				{
                    end = i - 1;
                }

			} while (start<=end);
			if (offset < this._offsets[i] && i != 0)
                i -= 1;

            return i;
        }

        //標高からどの色と色の間に含まれるか求め，その範囲内での位置のグラデーション色を返す．
		public SKColor GetGradientColor(double offset)
		{
			int i = GetOffsetIndex(offset);

			if (i == 0 && offset < _offsets[0])
			{
				return this._colors[0];
			}
			else if (i >= this._offsets.Length - 1)
            {
				return this._colors[this._offsets.Length - 1];
			}

			double before_offset = this._offsets[i];
			double after_offset = this._offsets[i + 1];
			SKColor before_color = this._colors[i];
			SKColor after_color = this._colors[i + 1];

			byte R = (byte)((offset - before_offset) * (after_color.Red - before_color.Red) / (after_offset - before_offset) + before_color.Red);
			byte G = (byte)((offset - before_offset) * (after_color.Green - before_color.Green) / (after_offset - before_offset) + before_color.Green);
			byte B = (byte)((offset - before_offset) * (after_color.Blue - before_color.Blue) / (after_offset - before_offset) + before_color.Blue);

			return new SKColor(R, G, B);
        }
	}
}


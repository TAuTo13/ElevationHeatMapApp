using System;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using SkiaSharp;
using MapConsoleLegend.Models;


namespace MapConsoleLegend
{
    public class ColorLegend
    {
        private const int MARGIN = 3;

        private SKColor[] _colors;
        private double[] _offsets;

        public SKColor BgColor { private get; set; } = SKColors.White;

        public int ColorsLen { get; private set; }

        public void LoadJson(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("PATH:" + path + "  is not exist.");
                Environment.Exit(1);
            }
            if (Path.GetExtension(path) != ".json")
            {
                Console.WriteLine("Path file is not " + "json" + " file.");
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

        private void Initialize(string[] colors, double[] offsets)
        {
            if (colors.Length != offsets.Length)
            {
                Console.WriteLine("Color[] and offset[] arrays' size are not match.");
                Environment.Exit(1);
            }

            this.ColorsLen = colors.Length;

            this._offsets = offsets;
            this._colors = new SKColor[colors.Length];

            for (int i = 0; i < _colors.Length; i++)
            {
                this._colors[i] = SKColor.Parse(colors[i]);
            }
        }

        public SKBitmap DrawLengends(int width,int height,out LegendLabelSet[] offsetsPositions)
        {
            int UnitLength = (height - MARGIN * (ColorsLen - 1)) / (ColorsLen - 1);
            SKBitmap legendBmp = new SKBitmap(width,height);

            offsetsPositions = new LegendLabelSet[_offsets.Length];

            int level = 0;
            for (; level < this.ColorsLen-1; level++)
            {
                int offset = level * (UnitLength + MARGIN);
                offsetsPositions[level] = new LegendLabelSet(_offsets[level].ToString(), height - offset - UnitLength/2);
                for (int position = 1; position <= UnitLength; position++)
                {
                    SKColor c_0 = this._colors[level];
                    SKColor c_1 = this._colors[level + 1];

                    byte R = (byte)(position * (c_1.Red - c_0.Red) / UnitLength + c_0.Red);
                    byte G = (byte)(position * (c_1.Green - c_0.Green) / UnitLength + c_0.Green);
                    byte B = (byte)(position * (c_1.Blue - c_0.Blue) / UnitLength + c_0.Blue);
                    SKColor color = new SKColor(R, G, B);

                    int y = height - (offset + position);
                    for (int x = 0; x < width; x++) legendBmp.SetPixel(x, y, color);
                }

                if (level < this.ColorsLen - 2){ 
                    int borderStart = height - (offset + UnitLength + 1);

                    for (int y_offs = borderStart; y_offs >= borderStart - MARGIN; y_offs--)
                        for (int x = 0; x < width; x++) legendBmp.SetPixel(x, y_offs, this.BgColor);
                }
            }
            offsetsPositions[level] = new LegendLabelSet(_offsets[level].ToString(), height - (UnitLength + MARGIN) * (this.ColorsLen-1)-UnitLength/2);

            return legendBmp;
        }
    }
}


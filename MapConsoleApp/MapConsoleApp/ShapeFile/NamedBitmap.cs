using SkiaSharp;

namespace MapConsoleApp.ShapeFile
{
    public struct NamedBitmap
    {
        public string name { get; set; }

        public SKBitmap Bmp { get; set; }

        public NamedBitmap(string name, SKBitmap bmp)
        {
            this.name = name;
            this.Bmp = bmp;
        }

        public static readonly NamedBitmap Empty = new(string.Empty, new SKBitmap());

        public bool SaveImg(string path)
        {
            string filename = Path.ChangeExtension(this.name, Extensions.PNG);
            string fullpath = Path.Combine(path, filename);
            try
            {
                using (Stream stream = File.Create(fullpath))
                {
                    SKImage image = SKImage.FromBitmap(this.Bmp);
                    SKData data = image.Encode(SKEncodedImageFormat.Png, 100);
                    data.SaveTo(stream);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }
    }
}


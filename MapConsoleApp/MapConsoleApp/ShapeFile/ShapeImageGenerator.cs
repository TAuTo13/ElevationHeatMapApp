using SkiaSharp;
using MapConsoleApp.ColorGradient;
using MapConsoleApp.ShapeFile;

namespace MapConsoleApp
{
	public class ShapeImageGenerator
	{
        private readonly SKColor _alphaPixColor = new SKColor(0, 0, 0, 0);
        
		public int Width { get; set; }

		public int Height { get; set; }

		public ColorGradientBlend Blend { private get; set; }

		public ShapeImageGenerator() : this(0, 0) {}

		public ShapeImageGenerator(int width, int height) : this(new ColorGradientBlend(), width, height) {}

		public ShapeImageGenerator(ColorGradientBlend blend,int width=0,int height=0)
		{
			this.Width = width;
			this.Height = height;
			this.Blend = blend;
		}

		public NamedBitmap GenerateImg(ShapeFileGeometry geometry)
		{
			if (Blend is null || geometry is null)
				return NamedBitmap.Empty;

			SKBitmap bmp = new((int)this.Width,(int)this.Height);

			foreach(Pix p in geometry.GetPointElevs(this.Width, this.Height))
			{
				double? e = p.Elev;
				int x = p.X;
				int y = p.Y;

				if (e is null)
					bmp.SetPixel(x, y, _alphaPixColor);
				else
					bmp.SetPixel(x, y, Blend.GetGradientColor((double)e));
			}

            return new NamedBitmap(geometry.Name,bmp);
		}

		public NamedBitmap GenerateImgInterp(ShapeFileGeometry geometry)
        {
            if (Blend is null || geometry is null)
                return NamedBitmap.Empty;

            SKBitmap bmp = new((int)this.Width, (int)this.Height);

            foreach (Pix p in geometry.GetPointElevsInterp(this.Width, this.Height))
            {
                double? e = p.Elev;
                int x = p.X;
                int y = p.Y;


				if (e is null)
				{
					e = p.InterpElev;
					if (e is null)
						bmp.SetPixel(x, y, _alphaPixColor);
					else
						bmp.SetPixel(x, y, Blend.GetGradientColor((double)e));
				}
				else
					bmp.SetPixel(x, y, Blend.GetGradientColor((double)e));
            }



            return new NamedBitmap(geometry.Name, bmp);
        }
	}
}


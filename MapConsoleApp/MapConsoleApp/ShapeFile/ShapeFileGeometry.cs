using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace MapConsoleApp.ShapeFile
{
	public class ShapeFileGeometry
	{
		public double MinX { get; private set; }
        public double MinY { get; private set; }
        public double MaxX { get; private set; }
        public double MaxY { get; private set; }
		public double Width { get; private set; }
		public double Height { get; private set; }
		public int Count { get; private set; }
        public string Name { get; private set; }
        public Envelope Bounds { get; private set; }

        private IEnumerable<Point> _points { get; set; }


		public ShapeFileGeometry()
		{
			this._points = Enumerable.Empty<Point>();
            this.Name = string.Empty;
        }

        public ShapeFileGeometry(string path):this()
        {
            this.LoadShapeFile(path);
        }

		public void LoadShapeFile(string path)
        {
            if (!File.Exists(path + Extensions.DBF))
            {
                Console.WriteLine(Extensions.DBF + " file was missing.");
                return;
            }
            if (!File.Exists(path + Extensions.PRJ))
            {
                Console.WriteLine(Extensions.PRJ + "file was missing.");
                return;
            }
            if (!File.Exists(path + Extensions.SHX))
            {
                Console.WriteLine(Extensions.SHX + " file was missing.");
                return;
            }

            Name = Path.GetFileNameWithoutExtension(path);

            var reader = new ShapefileReader(path);
			var geometries = reader.ReadAll();

			this._points = geometries.Select(a => (Point)a);

            var header = reader.Header;

            this.Bounds = header.Bounds;
            this.MaxX = this.Bounds.MaxX;
			this.MinX = this.Bounds.MinX;
			this.MaxY = this.Bounds.MaxY;
			this.MinY = this.Bounds.MinY;
			this.Count = header.FileLength;
			this.Width = this.MaxX - this.MinX;
			this.Height = this.MaxY - this.MinY;
		}

		public IEnumerable<Pix> GetPointElevs(int width,int height)
		{
            double unitX = (1.0 / (int)width) * this.Width;
            double unitY = (1.0 / (int)height) * this.Height;

            Pix[,] pixes = new Pix[height, width];

            //生成する画像サイズに合わせた2次元配列を初期化
            int capacity = (int)((this.Count / (width * height)) + 1);
            if (capacity > 1)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                        pixes[y,x] = new Pix(x, y, capacity);
                }
            }
            else
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                        pixes[y,x] = new Pix(x, y);
                }
            }

            //すべてのPointに関してそのPointが含まれる生成画像の画素にあらかじめ振り分ける
            foreach (var p in this._points)
            {
                int x = (int)((p.X - this.MinX) / unitX);
                int y = (int)((this.Height - (p.Y - this.MinY)) / unitY);

                if (x == width) x -= 1;
                if (y == height) y -= 1;

                double e = p.Z;
                pixes[y,x].Add(e);
            }


            //左上から行毎に右下にかけて全てのポイントに関してその都度平均値を計算して返す

            return pixes.Cast<Pix>();
        }

        public IEnumerable<Pix> GetPointElevsInterp(int width, int height)
        {
            double unitX = (1.0 / (int)width) * this.Width;
            double unitY = (1.0 / (int)height) * this.Height;

            Pix[,] pixes = new Pix[height, width];

            //生成する画像サイズに合わせた2次元配列を初期化
            int capacity = (int)((this.Count / (width * height)) + 1);
            if (capacity > 1)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                        pixes[y, x] = new Pix(x, y, capacity);
                }
            }
            else
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                        pixes[y, x] = new Pix(x, y);
                }
            }

            //すべてのPointに関してそのPointが含まれる生成画像の画素にあらかじめ振り分ける
            foreach (var p in this._points)
            {
                int x = (int)((p.X - this.MinX) / unitX);
                int y = (int)((this.Height - (p.Y - this.MinY)) / unitY);

                if (x == width) x -= 1;
                if (y == height) y -= 1;

                double e = p.Z;
                pixes[y, x].Add(e);

                int[] ymask;
                if (y == 0)
                {
                    ymask = new int[] { 0, 1 };
                }else if (y == (height - 1))
                {
                    ymask = new int[] { -1, 0 };
                }
                else
                {
                    ymask = new int[] { -1, 0, 1 };
                }

                int[] xmask;
                if (x == 0)
                {
                    xmask = new int[] { 0, 1 };
                }else if (x == (width - 1))
                {
                    xmask = new int[] { -1, 0 };
                }
                else
                {
                    xmask = new int[] { -1, 0, 1 };
                }

                foreach(int ydiff in ymask)
                {
                    foreach(int xdiff in xmask)
                    {
                        if (xdiff != 0 || ydiff != 0)
                        {
                            pixes[y + ydiff, x + xdiff].AddInterp(e);
                        }
                    }
                }

            }


            //左上から行毎に右下にかけて全てのポイントに関してその都度平均値を計算して返す

            return pixes.Cast<Pix>();
        }
    }
}


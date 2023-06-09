using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using MapConsoleApp.Models;


namespace MapConsoleApp.ShapeFile
{
    public class ShapeImagesFacade
    {
        private const string BB_FILE_NAME = "BoundingBox.csv";

        private string BasePath { get; set; }

        public ShapeImageGenerator? Generator { private get; set; }

        public int ShapesCount { get; private set; }

        private IEnumerable<ShapeFileGeometry> _shapeFiles;

        private IEnumerable<NamedBitmap> _bmps;

        public ShapeImagesFacade(string path)
        {
            this.BasePath = path;
            this._shapeFiles = Enumerable.Empty<ShapeFileGeometry>();
            this._bmps = Enumerable.Empty<NamedBitmap>();
        }

        public void LoadShapeFiles()
        {
            IEnumerable<string> names = Directory.GetFiles(this.BasePath,"*"+Extensions.SHP)
                .Select(f => Path.GetFileNameWithoutExtension(f))
                .Select(f => Path.Combine(this.BasePath, f));
            
            this.ShapesCount = names.Count();
            this._shapeFiles = names.Select(n => new ShapeFileGeometry(n));
        }

        public void GenerateImgs(bool shouldInterp)
        {
            if (this.Generator is null)
            {
                Console.WriteLine();
                Environment.Exit(1);
            }

            if (!shouldInterp)
                this._bmps = this._shapeFiles.Select(g => this.Generator.GenerateImg(g));
            else
                this._bmps = this._shapeFiles.Select(g => this.Generator.GenerateImgInterp(g));
        }

        public int SaveImgs(string path)
        {
            EnsureDirectory(path);

            int succeededCnt = 0;
            foreach(var bmp in this._bmps)
            {
                bool flag=bmp.SaveImg(path);
                if (flag) succeededCnt++;
            }

            return succeededCnt;
        }

        public bool SaveBounds(string path)
        {
            EnsureDirectory(path);

            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HasHeaderRecord = false,
                Delimiter = ";"
            };

            IEnumerable<BoundingBox> boundingBoxes = this._shapeFiles
                .Select(s => new BoundingBox(Path.ChangeExtension(s.Name,Extensions.PNG), s.Bounds.ToString()));

            path = Path.Combine(path,BB_FILE_NAME);

            try
            {
                using var writer = new StreamWriter(path);
                using var csvWriter = new CsvWriter(writer, config);
                csvWriter.WriteRecords(boundingBoxes);
                csvWriter.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
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


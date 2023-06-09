namespace MapConsoleApp.Models
{
    public struct BoundingBox
    {
        public string FileName { get; set; }
        public string Bounds { get; set; }

        public BoundingBox(string name,string bounds)
        {
            this.FileName = name;
            this.Bounds = bounds;
        }
    }
}


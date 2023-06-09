using System;
namespace MapConsoleApp.ShapeFile
{
    public struct Pix
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        private List<double> _elevs;
        public readonly double? Elev
        {
            get
            {
                if (_elevs.Count == 0)
                    return null;
                else
                    return _elevs.Average();
            }
        }

        private List<double> _interp;
        public readonly double? InterpElev
        {
            get
            {
                if (_interp.Count() == 0)
                    return null;
                else
                    return _interp.Average();
            }
        }

        public Pix(int x, int y)
        {
            this._elevs = new List<double>();
            this._interp = new List<double>(8);
            this.X = x;
            this.Y = y;
        }

        public Pix(int x, int y, int capacity)
        {
            this._elevs = new List<double>(capacity);
            this._interp = new List<double>(8);
            this.X = x;
            this.Y = y;
        }

        public int incount()
        {
            return _interp.Count;
        }

        public void Add(double elev)
        {
            this._elevs.Add(elev);
        }

        public void AddInterp(double elev)
        {
            this._interp.Add(elev);
        }
    }
}


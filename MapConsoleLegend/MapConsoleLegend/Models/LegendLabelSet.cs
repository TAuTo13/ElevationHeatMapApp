using System;
namespace MapConsoleLegend.Models
{
    public class LegendLabelSet
    {
        public string Label { get; set; }
        public float Position { get; set; }

        public LegendLabelSet(string label,float position)
        {
            Label = label;
            Position = position;
        }
    }
}


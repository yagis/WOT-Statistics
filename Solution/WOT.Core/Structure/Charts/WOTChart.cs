using System;
using System.Collections.Generic;
using System.Linq;

namespace WOTStatistics.Core
{
    public class WOTChart
    {
        public WOTChart()
        {
            Curves = new List<WOTCurve>();
        }

        public string Name { get; set; }
        public string Period { get; set; }
        public string PlayerId { get; set; }
        public bool Saved { get; set; }
        public List<WOTCurve> Curves { get; set; }
    }
}

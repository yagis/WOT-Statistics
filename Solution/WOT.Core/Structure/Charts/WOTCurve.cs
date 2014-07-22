using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WOTStatistics.Core
{
    public class WOTCurve
    {
        public string chartName { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public string Class { get; set; }
        public int Tier { get; set; }
        public string Tank { get; set; }
        public string Display { get; set; }
        public string Color { get; set; }
        public bool ShowValues { get; set; }
    }
}
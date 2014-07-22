using System;
using System.Collections.Generic;
using System.Linq;

namespace WOTStatistics.Core
{
    public class Player
    {
        public string PlayerID { get; set; }
        public string PlayerRealm { get; set; }
        public string WatchFile { get; set; }
        public string Monitor { get; set; }
        public string PreviousFile { get; set; }
        public string CurrentFile { get; set; }
        public string OnlineURL { get; set; }
    }
}

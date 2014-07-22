using System;
using System.Collections.Generic;
using System.Linq;

namespace WOTStatistics.Core
{
    public class TankKey
    {
        public int CountryID { get; set; }
        public int TankID { get; set; }
    }

    public class TankValue
    {
        public string Description { get; set; }
        public int Tier { get; set; }
        public int compDescr { get; set; }
        public bool Active { get; set; }
        public string TankType { get; set; }
        public int Premium { get; set; }
        //TODO
        public bool GotIt { get; set; }
    }

    class TankEqualityComparer : IEqualityComparer<TankKey>
    {
        #region IEqualityComparer<Customer> Members

        public bool Equals(TankKey x, TankKey y)
        {
            return ((x.CountryID == y.CountryID) & (x.TankID == y.TankID));
        }

        public int GetHashCode(TankKey obj)
        {
            string combined = String.Format("{0}|{1}", obj.CountryID, obj.TankID);
            return (combined.GetHashCode());
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WOTStatistics.SQLite
{
    public static class DataRowExtensions
    {
        public static int GetSafeInt(this DataRow source, string columnName)
        {
            int output;
            if (source.IsNull(columnName))
                return 0;

            if (!int.TryParse(source[columnName].ToString(), out output))
                output = 0;

            return output;
        }

        public static double GetSafeDouble(this DataRow source, string columnName)
        {
            double output;
            if (source.IsNull(columnName))
                return 0;

            if (!double.TryParse(source[columnName].ToString(), out output))
                output = 0;

            return output;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Xml;
using System.Data;
using WOTStatistics.SQLite;

namespace WOTStatistics.Core
{
    public class TankDescriptions : IEnumerable<KeyValuePair<TankKey, TankValue>>, IDisposable
    {
        private readonly Dictionary<TankKey, TankValue> _tanks = new Dictionary<TankKey, TankValue>(new TankEqualityComparer());
        private MessageQueue _message;
        private TankTypeDescription _tankTypeDescriptions;

        public TankDescriptions(MessageQueue message)
        {
            _message = message;
            _tankTypeDescriptions = new TankTypeDescription(message);
           
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(WOTHelper.GetSettingsFile());
                XmlElement root = xmlDoc.DocumentElement;
                XmlNodeList nodes = root.SelectSingleNode(@"Tanks").ChildNodes;

                foreach (XmlNode node in nodes)
                {
                    TankKey tKey = new TankKey() { CountryID = int.Parse(node.Attributes["Country"].Value), TankID = int.Parse(node.Attributes["Code"].Value) };
                    TankValue tValue = new TankValue()
                    {
                        Description = Translations.TranslationGet(node.Attributes["Country"].Value + "_" + node.Attributes["Code"].Value, "DE",  node.InnerText),
                        Tier = int.Parse(node.Attributes["Tier"].Value),
                        Premium = int.Parse(node.Attributes["Premium"].Value),
                        Active = (node.Attributes["Active"] == null ? true : bool.Parse(node.Attributes["Active"].Value)),
                        TankType = node.Attributes["TankType"].Value
                    };
                    _tanks.Add(tKey, tValue);

                }
           
        }

        public TankDescriptions(MessageQueue message, string playerDBFile) {
            _message = message;
            _tankTypeDescriptions = new TankTypeDescription(message);

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(WOTHelper.GetSettingsFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList nodes = root.SelectSingleNode(@"Tanks").ChildNodes;
            //TODO
            DataTable dt = getMyTanks(playerDBFile);
            bool gotIt = false;

            foreach (XmlNode node in nodes)
            {
                //TODO
                gotIt = false;
                string strExpr = "cmCountryID = " + int.Parse(node.Attributes["Country"].Value) +
                                " AND cmTankID = " + int.Parse(node.Attributes["Code"].Value);
                var match = dt.Select(strExpr);
                if (match.Length > 0)
                {
                    gotIt = true;
                }
                //
                TankKey tKey = new TankKey() { CountryID = int.Parse(node.Attributes["Country"].Value), TankID = int.Parse(node.Attributes["Code"].Value) };

                TankValue tValue = new TankValue()
                {
                    Description = Translations.TranslationGet(node.Attributes["Country"].Value + "_" + node.Attributes["Code"].Value, "DE", node.InnerText),
                    Tier = int.Parse(node.Attributes["Tier"].Value),
                    Premium = int.Parse(node.Attributes["Premium"].Value),
                    Active = (node.Attributes["Active"] == null ? true : bool.Parse(node.Attributes["Active"].Value)),
                    TankType = node.Attributes["TankType"].Value,
                    //TODO
                    GotIt = gotIt
                };

                _tanks.Add(tKey, tValue);

            }
        }


        private DataTable getMyTanks(string playerDBFile)
        {

            using (IDBHelpers db = new DBHelpers(playerDBFile, false))
            {
                string sql = @"SELECT DISTINCT cmCountryID, cmTankID
                            FROM File_TankDetails ORDER BY cmCountryID";
                DataSet ds = new DataSet();

                using (DataTable dt = db.GetDataTable(sql))
                {
                    dt.TableName = "myTanks";
                    ds.Tables.Add(dt);
                    return dt;
                }


            }

        }

        public string Description(int countryId, int tankID)
        {
            TankKey tk = new TankKey() { CountryID = countryId, TankID = tankID };

            TankValue tValue;
            if (!_tanks.TryGetValue(tk, out tValue))
                return "Unknown";
            else
                return Translations.TranslationGet(countryId + "_" + tankID, "DE", tValue.Description);
        }

        public int Tier(int countryId, int tankID)
        {
            TankKey tk = new TankKey() { CountryID = countryId, TankID = tankID };

            TankValue tValue;
            if (!_tanks.TryGetValue(tk, out tValue))
                return 0;
            else
                return tValue.Tier;
        }

        public bool Active(int countryId, int tankID)
        {
            TankKey tk = new TankKey() { CountryID = countryId, TankID = tankID };

            TankValue tValue;
            if (!_tanks.TryGetValue(tk, out tValue))
                return true;
            else
                return tValue.Active;
        }

        public int compDescr(int countryId, int tankID)
        {
            TankKey tk = new TankKey() { CountryID = countryId, TankID = tankID };

            TankValue tValue;
            if (!_tanks.TryGetValue(tk, out tValue))
                return 0;
            else
                return tValue.compDescr;
        }

        public bool Premium(int countryId, int tankID)
        {
            TankKey tk = new TankKey() { CountryID = countryId, TankID = tankID };

            TankValue tValue;
            if (!_tanks.TryGetValue(tk, out tValue))
                return false;
            else
                if (tValue.Premium == 1)
                    return true;
                else
                    return false;
        }

        public string TankType(int countryId, int tankID)
        {
            TankKey tk = new TankKey() { CountryID = countryId, TankID = tankID };

            TankValue tValue;
            if (!_tanks.TryGetValue(tk, out tValue))
                return "Unknown";
            else
                return tValue.TankType;
        }

        public string TankTypeDescription(int countryId, int tankID)
        {
            TankKey tk = new TankKey() { CountryID = countryId, TankID = tankID };

            TankValue tValue;
            if (!_tanks.TryGetValue(tk, out tValue))
                return "Unknown";
            else
                return _tankTypeDescriptions.Description(tValue.TankType);
        }

        public bool GotIt(int countryId, int tankID)
        {
            TankKey tk = new TankKey() { CountryID = countryId, TankID = tankID };

            TankValue tValue;
            if (!_tanks.TryGetValue(tk, out tValue))
                return false;
            else
                return true;

        }

        public Dictionary<TankKey, TankValue> Select(string filter)
        {

            using (DataTable tempDT = new DataTable())
            {
                tempDT.Columns.Add("CountryID", typeof(int));
                tempDT.Columns.Add("TankID", typeof(int));
                tempDT.Columns.Add("Description", typeof(string));
                tempDT.Columns.Add("Tier", typeof(int));
                tempDT.Columns.Add("Active", typeof(bool));
                tempDT.Columns.Add("TankType", typeof(string));
                tempDT.Columns.Add("Premium", typeof(int));
                foreach (KeyValuePair<TankKey, TankValue> kv in this)
                {
                    DataRow dr = tempDT.NewRow();
                    dr["CountryID"] = kv.Key.CountryID;
                    dr["TankID"] = kv.Key.TankID;
                    dr["Description"] = kv.Value.Description;
                    dr["Tier"] = kv.Value.Tier;
                    dr["Active"] = kv.Value.Active;
                    dr["TankType"] = kv.Value.TankType;
                    dr["Premium"] = kv.Value.Premium;
                    tempDT.Rows.Add(dr);
                }
                DataRow[] selectedRows = tempDT.Select(filter);
                Dictionary<TankKey, TankValue> returnDict = new Dictionary<TankKey, TankValue>();
                foreach (DataRow item in selectedRows)
                {
                    returnDict.Add(new TankKey() { CountryID = item.Field<int>("CountryID"), TankID = item.Field<int>("TankID") }, new TankValue() { Description = item.Field<string>("Description"), Tier = item.Field<int>("Tier"), Active = item.Field<bool>("Active"), Premium = item.Field<int>("Premium"), TankType = item.Field<string>("TankType") });
                }
                return returnDict;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TankKey, TankValue>> GetEnumerator()
        {
            return this._tanks.GetEnumerator();
        }

        public void Dispose()
        {
            _tanks.Clear();
            _tankTypeDescriptions.Dispose();
        }

    }
}
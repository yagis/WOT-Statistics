using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;


namespace WOTStatistics.Core
{
    public class WN8ExpValues : List<WN8ExpValue>
    {
      
        public Int16 WN8Version { get; set; }
        public Int32 WN8DateEpoch { get; set; }
        public DateTime WN8Date { get; set; }

        public WN8ExpValues()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList nodes = null;
            XmlNodeList headerNodes = null;
            try
            {
                xmlDoc.Load(WOTHelper.GetWN8ExpectedTankValuesFile());
                XmlElement root = xmlDoc.DocumentElement;
                nodes = root.SelectSingleNode("WN8").ChildNodes;
                headerNodes = root.SelectSingleNode("Header").ChildNodes;
            }
            catch (Exception ex)
            {
                WOTHelper.AddToLog(ex.Message);
            }
            WN8Version = 0;
            WN8DateEpoch = 0;
            WN8Date = new DateTime(1970, 1, 1, 0, 0, 0);
            if (headerNodes != null)
            {
                foreach (XmlNode headerNode in headerNodes)
                {
                    if (headerNode.Name == "Version")
                    {
                        WN8Version=Convert.ToInt16(headerNode.InnerText);

                    }

                    if (headerNode.Name == "Date")
                    {
                        WN8DateEpoch = Convert.ToInt32(headerNode.InnerText);
                        WN8Date = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(WN8DateEpoch);

                    }
                }
            }

            if (nodes != null)
            {

                MessageQueue _messages = null;
                TankDescriptions tankDescription = new TankDescriptions(_messages);
                foreach (XmlNode node in nodes)
                {

                    WN8ExpValue expVal = new WN8ExpValue();
                    
                    expVal.Tank = tankDescription.Description(Convert.ToInt32(node.Attributes["countryid"].Value), Convert.ToInt32(node.Attributes["tankid"].Value));
                    expVal.Country = Translations.TranslationGet(Convert.ToString(node.Attributes["countryid"].Value), "DE", Convert.ToString(node.Attributes["countryid"].Value));
                    expVal.Tier = tankDescription.Tier(Convert.ToInt32(node.Attributes["countryid"].Value), Convert.ToInt32(node.Attributes["tankid"].Value));
                    expVal.tankID = Convert.ToInt32(node.Attributes["tankid"].Value);
                    expVal.countryID = Convert.ToInt32(node.Attributes["countryid"].Value);
                    expVal.expFrag = double.Parse(node.Attributes["expFrag"].Value, System.Globalization.CultureInfo.InvariantCulture); 
                    expVal.expDamage = double.Parse(node.Attributes["expDamage"].Value, System.Globalization.CultureInfo.InvariantCulture); 
                    expVal.expSpot = double.Parse(node.Attributes["expSpot"].Value, System.Globalization.CultureInfo.InvariantCulture);
                    expVal.expDefense = double.Parse(node.Attributes["expDef"].Value, System.Globalization.CultureInfo.InvariantCulture); 
                    expVal.expWin = double.Parse(node.Attributes["expWinRate"].Value, System.Globalization.CultureInfo.InvariantCulture);

                    Add(expVal);
                }
            }
        }

        




        public void Add()
        {
            this.Add(new WN8ExpValue() {countryID =-1, tankID = -1 });
        }

        public void TryAddUpdate(WN8ExpValue tankValues)
        {
            this.Add(tankValues);
        }

        public WN8ExpValue GetByTankID(int countryID, int tankID)
        {
            var tankValue = (from x in this
                     where x.countryID == countryID && x.tankID == tankID
                     select x).DefaultIfEmpty(null).FirstOrDefault();

            if (tankValue != null)
            {
                return tankValue;
            }
            else
            {
                return null;
            }
        }

    }
    public class WN8ExpValue
    {
        public string Tank { get; set; }
        public string Country { get; set; }
        public int tankID { get; set; }
        public int countryID { get; set; }
        public int Tier { get; set; }
        public int IDNum { get; set; }
        public double expFrag { get; set; }
        public double expDamage { get; set; }
        public double expSpot { get; set; }
        public double expDefense { get; set; }
        public double expWin { get; set; }
    }



}
    

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace WOTStatistics.Core
{
    public static class ChartSettings
    {
        public static List<WOTChart> LoadCharts()
        {
            List<WOTChart> holder = new List<WOTChart>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(WOTHelper.GetChartFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNodeList chartNodes = root.SelectSingleNode(@"Charts").ChildNodes;
            foreach (XmlNode chartNode in chartNodes)
            {
                WOTChart myChart = new WOTChart()
                {
                    Name = chartNode.Attributes["Name"].Value.ToString(),
                    Period = chartNode.Attributes["Period"].Value.ToString(),
                    Saved = true
                };

                XmlNodeList curveNodes = chartNode.SelectSingleNode(@"Curves").ChildNodes;
                foreach (XmlNode curveNode in curveNodes)
                {
                    WOTCurve myCurve = new WOTCurve()
                    {
                        Name = curveNode.Attributes["Name"] == null ? "New Chart" : curveNode.Attributes["Name"].Value.ToString(),
                        chartName = myChart.Name.ToString(),
                        Category = curveNode.Attributes["Category"] == null ? "" : curveNode.Attributes["Category"].Value.ToString(),
                        Country = curveNode.Attributes["Country"] == null ? "" : curveNode.Attributes["Country"].Value.ToString(),
                        Class = curveNode.Attributes["Class"] == null ? "" : curveNode.Attributes["Class"].Value.ToString(),
                        Tier = curveNode.Attributes["Tier"] == null ? 0 : Convert.ToInt32(curveNode.Attributes["Tier"].Value.ToString()),
                        Tank = curveNode.Attributes["Tank"] == null ? "0" : curveNode.Attributes["Tank"].Value.ToString(),
                        Color = curveNode.Attributes["Color"] == null ? "Black" : curveNode.Attributes["Color"].Value.ToString(),
                        Display = curveNode.Attributes["Display"] == null ? "Total" : curveNode.Attributes["Display"].Value.ToString(),
                        ShowValues = curveNode.Attributes["ShowValues"] == null ? true : Convert.ToBoolean(curveNode.Attributes["ShowValues"].Value)
                    };

                    myChart.Curves.Add(myCurve);
                }
                holder.Add(myChart);
            }

            return holder;
        }

        public static void SaveChart(WOTChart myChart, int chartIndex)
        {
            //TODO - below works for adding charts/curves. need to implement editing charts/curves
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(WOTHelper.GetChartFile());
            XmlElement root = xmlDoc.DocumentElement;
            XmlNode chartsNode = root.SelectSingleNode("Charts");

            XmlElement elChart = xmlDoc.CreateElement("Chart");
            elChart.SetAttribute("Name", myChart.Name);
            elChart.SetAttribute("Period", myChart.Period);

            XmlElement elCurves = xmlDoc.CreateElement("Curves");

            elChart.AppendChild(elCurves);

            foreach (WOTCurve curve in myChart.Curves)
            {
                if (curve.chartName == myChart.Name)
                {
                    XmlElement elCurve = xmlDoc.CreateElement("Curve");
                    elCurve.SetAttribute("Name", curve.Name);
                    elCurve.SetAttribute("Category", curve.Category);
                    elCurve.SetAttribute("Country", curve.Country);
                    elCurve.SetAttribute("Class", curve.Class);
                    elCurve.SetAttribute("Tier", curve.Tier.ToString());
                    elCurve.SetAttribute("Tank", curve.Tank);
                    elCurve.SetAttribute("Display", curve.Display);
                    elCurve.SetAttribute("Color", curve.Color);
                    elCurve.SetAttribute("ShowValues", curve.ShowValues.ToString());
                    elCurves.AppendChild(elCurve);
                }
            }
            chartsNode.InsertBefore(elChart, chartsNode.ChildNodes[chartIndex + 1]);
            xmlDoc.Save(WOTHelper.GetChartFile());
            myChart.Saved = true;
        }
    }
}

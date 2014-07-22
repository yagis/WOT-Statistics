using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraCharts;
using WOTStatistics.Core;
using System.Drawing;

namespace WOT.Stats
{
    public static class ChartData
    {
        private static string _currentChart = "";

        public static void GetChartData(string playerName, string chartID, string statsBase, ChartControl chartControl)
        {
            chartControl.ClearCache();

            chartControl.AppearanceNameSerializable = UserSettings.ChartAppearance;
            chartControl.PaletteName = UserSettings.ChartPalette;
  
            GraphFields gf = new GraphsSettings(new MessageQueue()).FieldValues(chartID);
            string newChart = chartID + gf.Caption + gf.DataField + gf.InnerText + gf.Name + gf.Period + gf.StatsBase + gf.Type + playerName + UserSettings.BattleMode;

            if (_currentChart != newChart)
            {
                _currentChart = chartID + gf.Caption + gf.DataField + gf.InnerText + gf.Name + gf.Period + gf.StatsBase + gf.Type + playerName;

                if (statsBase == "Overall")
                {
                    OverallChart(playerName, chartID, gf, chartControl);
                }
                else
                {
                    TankChart(playerName, chartID, gf, chartControl);
                }
            }
        }

        private static void OverallChart(string playerName, string chartID, GraphFields gFields, ChartControl chartControl)
        {
            chartControl.Series.Clear();
            chartControl.Titles.Clear();
            Player player = new PlayerListing(new MessageQueue()).GetPlayer(playerName);
            DossierManager dossierManager = new DossierManager(player.PlayerID, player.WatchFile, player.Monitor, new MessageQueue(), null);

            DateTime endFile = dossierManager.FormatTextDate(dossierManager.GetCurrentPlayerFile().ToString());
            DateTime startFile = dossierManager.FormatTextDate(dossierManager.GetCurrentPlayerFile().ToString()).AddDays((gFields.Period+1) * -1);

            Dictionary<Int32, Int32> files = dossierManager.GetAllFilesForPlayer();
            Dictionary<Int32, Int32> selectedFiles = (from d in files
                                                       where dossierManager.FormatTextDate(d.Key.ToString()) >= startFile && dossierManager.FormatTextDate(d.Key.ToString()) <= endFile
                                                       select d).ToDictionary(x => x.Key, x => x.Value);

            Series series1 = new Series(gFields.Caption, ViewType.Line);
            series1.Label.Font = new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLCellFont), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            foreach (KeyValuePair<Int32, Int32> file in selectedFiles.OrderBy(x => x.Key))
            {
                Dossier dossierFile = new Dossier(file.Value, player.PlayerID, new MessageQueue());
                WOTStats dossierStats = dossierFile.GetStats();
                series1.Points.Add(new SeriesPoint(dossierManager.FormatTextDate(file.Key.ToString()), Math.Round(GetStatValue(gFields.DataField, dossierStats),2)));
            }

            
            
            // Add the series to the chart.
            chartControl.Series.Add(series1);

            // Set the numerical argument scale types for the series,
            // as it is qualitative, by default.
            series1.ArgumentScaleType = ScaleType.DateTime;
            series1.Label.ResolveOverlappingMode = ResolveOverlappingMode.HideOverlapped;

            // Access the view-type-specific options of the series.
            ((LineSeriesView)series1.View).LineMarkerOptions.Kind = MarkerKind.Circle;
            ((LineSeriesView)series1.View).LineStyle.DashStyle = DashStyle.Solid;

            // Access the type-specific options of the diagram.
            ((XYDiagram)chartControl.Diagram).EnableAxisXZooming = true;

            // Hide the legend (if necessary).
            chartControl.Legend.Visible = false;
            

            // Add a title to the chart (if necessary).
            chartControl.Titles.Add(new ChartTitle());
            chartControl.Titles[0].Font = new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLHeaderFont), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            chartControl.Titles[0].Text = gFields.Caption;
            chartControl.Titles.Add(new ChartTitle());
            chartControl.Titles[1].Font= new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLHeaderFont)-1.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            chartControl.Titles[1].Text = startFile.ToString(UserSettings.DateFormat) + " to " + endFile.ToString(UserSettings.DateFormat);
            chartControl.Titles.Add(new ChartTitle());
            chartControl.Titles[2].Font= new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLHeaderFont)-3, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            chartControl.Titles[2].Text = gFields.DataField;

            XYDiagram diagram = (XYDiagram)chartControl.Diagram;

            switch (gFields.Period)
            {
                case 7:
                    diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day;
                    diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                    break;
                case 14:
                    diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day;
                    diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                    break;
                case 92:
                case 184:
                case 365:
                    diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Month;
                    diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                    diagram.AxisX.Label.TextPattern = "MMM yy";
                    break;
                default:
                    diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Week;
                    diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                    break;
            }


            diagram.AxisX.Label.Font = new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLCellFont), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            diagram.AxisY.Label.Font = new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLCellFont), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));


            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            diagram.AxisY.NumericScaleOptions.AutoGrid = true;
            diagram.AxisY.DateTimeScaleOptions.AutoGrid = true;
            
        }

        private static double GetStatValue(string StatName, WOTStats wotStats)
        {
            /*
Kill Ratio
Kill/Death Ratio

             */
            switch (StatName)
            {
                case "Damage Taken : Total":
                    return wotStats.DamageReceived;
                case "Damage Dealt : Total":
                    return wotStats.DamageDealt;
                case "Damage Dealt : Average":
                    return wotStats.AverageDamageDealtPerBattle;
                case "Damage Taken : Average":
                    return wotStats.AverageDamageReceivedPerBattle;
                case "Efficiency":
                    return wotStats.RatingEff;
                case "Battle Rating":
                    return wotStats.RatingBR;
                case "WN7":
                    return wotStats.RatingWN7;
                case "WN8":
                    return wotStats.RatingWN8;
                case "Win Ratio":
                    return wotStats.Victory_Ratio * 100;
                case "Experience : Total":
                    return wotStats.Experience;
                case "Experience : Average":
                    return wotStats.AverageExperiencePerBattle;
                case "Kill Ratio":
                    return wotStats.Kills / wotStats.BattlesCount;
                case "Kill/Death Ratio":
                    return wotStats.Kills / (wotStats.BattlesCount - wotStats.Survived);
                default:
                    return 0;
            }
        }

        private static void TankChart(string playerName, string chartID, GraphFields gFields, ChartControl chartControl)
        {
            chartControl.Series.Clear();
            chartControl.Titles.Clear();
            Player player = new PlayerListing(new MessageQueue()).GetPlayer(playerName);
            DossierManager dossierManager = new DossierManager(player.PlayerID, player.WatchFile, player.Monitor, new MessageQueue(), null);

            DateTime endFile = dossierManager.FormatTextDate(dossierManager.GetCurrentPlayerFile().ToString());
            DateTime startFile = dossierManager.FormatTextDate(dossierManager.GetCurrentPlayerFile().ToString()).AddDays((gFields.Period + 1) * -1);

            Dictionary<Int32, Int32> files = dossierManager.GetAllFilesForPlayer();
            Dictionary<Int32, Int32> selectedFiles = (from d in files
                                                       where dossierManager.FormatTextDate(d.Key.ToString()) >= startFile && dossierManager.FormatTextDate(d.Key.ToString()) <= endFile
                                                       select d).ToDictionary(x => x.Key, x => x.Value);

            TankDescriptions tankDesc = new TankDescriptions(new MessageQueue());
            Series series1=null;
            foreach (string tank in gFields.InnerText.Split('|'))
            {
                series1 = new Series(tankDesc.Description(int.Parse(tank.Split('_')[0]), int.Parse(tank.Split('_')[1])), ViewType.Line);
                series1.Label.Font = new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLCellFont), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
                foreach (KeyValuePair<Int32, Int32> file in selectedFiles.OrderBy(x => x.Key))
                {
                    Dossier dossierFile = new Dossier(file.Value, player.PlayerID, new MessageQueue());
                    WOTStats dossierStats = dossierFile.GetStats();
                    series1.Points.Add(new SeriesPoint(dossierManager.FormatTextDate(file.Key.ToString()), Math.Round(GetTankStatValue(tank, gFields.DataField, dossierStats), 2)));
                }

                // Add the series to the chart.
                chartControl.Series.Add(series1);

                // Set the numerical argument scale types for the series,
                // as it is qualitative, by default.
                series1.ArgumentScaleType = ScaleType.DateTime;
                series1.Label.ResolveOverlappingMode = ResolveOverlappingMode.HideOverlapped;

                // Access the view-type-specific options of the series.
                ((LineSeriesView)series1.View).LineMarkerOptions.Kind = MarkerKind.Circle;
                ((LineSeriesView)series1.View).LineStyle.DashStyle = DashStyle.Solid;
            }



            

            // Access the type-specific options of the diagram.
            ((XYDiagram)chartControl.Diagram).EnableAxisXZooming = false;

       

            // Hide the legend (if necessary).
            chartControl.Legend.Visible = true;
            chartControl.Legend.Font = new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLCellFont), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            chartControl.Legend.AlignmentVertical = LegendAlignmentVertical.BottomOutside;
            chartControl.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Center;
            chartControl.Legend.EquallySpacedItems = true;
            chartControl.Legend.Direction = LegendDirection.LeftToRight;
  

            // Add a title to the chart (if necessary).
            chartControl.Titles.Add(new ChartTitle());
            chartControl.Titles[0].Font = new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLHeaderFont), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            chartControl.Titles[0].Text = gFields.Caption;
            chartControl.Titles.Add(new ChartTitle());
            chartControl.Titles[1].Font = new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLHeaderFont)-1.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            chartControl.Titles[1].Text = startFile.ToString(UserSettings.DateFormat) + " to "  + endFile.ToString(UserSettings.DateFormat);
            chartControl.Titles.Add(new ChartTitle());
            chartControl.Titles[2].Font = new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLHeaderFont)-3, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            chartControl.Titles[2].Text = gFields.DataField;

            XYDiagram diagram = (XYDiagram)chartControl.Diagram;

            switch (gFields.Period)
            {
                case 7:
                    diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day;
                    diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                    break;
                case 14:
                     diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day;
                    diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                    break;
                case 92:
                case 184:
                case 365:
                    diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Month;
                    diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                    diagram.AxisX.Label.TextPattern = "MMM yy";
                    break;
                default:
                    diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Week;
                    diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day;
                    break;
            }

            diagram.AxisX.Label.Font = new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLCellFont), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            diagram.AxisY.Label.Font = new System.Drawing.Font("Tahoma", float.Parse(UserSettings.HTMLCellFont), System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));

            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            diagram.AxisY.NumericScaleOptions.AutoGrid = true;
            diagram.AxisY.DateTimeScaleOptions.AutoGrid = true;
            
        }

        private static double GetTankStatValue(string tankID, string StatName, WOTStats wotStats)
        {
            int countryID = int.Parse(tankID.Split('_')[0]);
            int tempTankID = int.Parse(tankID.Split('_')[1]);

            switch (StatName)
            {
                case "Damage Taken : Total":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.Data.DamageReceived);
                case "Damage Dealt : Total":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.Data.DamageDealt);
                case "Efficiency":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.RatingEff);
                case "Battle Rating":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.RatingBR);
                case "WN7":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.RatingWN7);
                case "WN8":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.RatingWN8);
                case "Win Ratio":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.Data.VictoryPercentage);
                case "Experience : Total":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.Data.Xp);
                case "Damage Dealt : Average":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.Data.AverageDamageDealt);
                case "Damage Taken : Average":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.Data.AverageDamageReceived);
                case "Experience : Average":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.Data.AverageXP);
                case "Kill Ratio":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.Data.KillperBattle);
                case "Kill/Death Ratio":
                    return wotStats.tanks.Where(x => x.CountryID == countryID && x.TankID == tempTankID).Sum(y => y.Data.KillperDeaths);
                default:
                    return 0;
            }
        }
    }
}

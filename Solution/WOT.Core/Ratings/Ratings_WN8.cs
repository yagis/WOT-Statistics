using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using WOTStatistics.Core;

namespace WOTStatistics.Core
{

    public partial class Ratings
    {


        public static RatingStorage GetRatingWN8(RatingStructure ratingStruct)
        {
            RatingStorage WN8 = GetRatingWN8Calc(ratingStruct);

            return WN8;
        }


        public static string GetRatingWN8ToolTip(RatingStorage WN8)
        {
            return WN8_Tooltip(WN8);
        }
                
       





        private static RatingStorage GetRatingWN8Calc(RatingStructure ratingStruct)
        {
            ratingStruct.RatingType = "WN8";
            RatingStorage Storage = new RatingStorage(ratingStruct);
            if (ratingStruct.battlesCount == 0)
            {
                return Storage;
            }

            //WOTHelper.AddToLog(countryID);
            //WOTHelper.AddToLog(tankID);


            WN8ExpValue WN8ExpectedTank = null;
            try
            {
                WN8ExpectedTank = ratingStruct.WN8ExpectedTankList.GetByTankID(ratingStruct.countryID, ratingStruct.tankID);
            }
            catch (Exception ex)
            {
                WOTHelper.AddToLog(ex.Message);
            }


            
            if (WN8ExpectedTank == null)
            {
                WOTHelper.AddToLog("WNExpectedTank is NULL: " + ratingStruct.countryID + " - " + ratingStruct.tankID);
                return Storage;
            }

            //rDAMAGE=sum(TotalDamageOfSingleTank)/sum(ExpectedDamagePerTank*BattlesInThatTank)
            //rSPOT=sum(totalSpotOfSingleTank)/sum(ExpectedSpotPerTank*BattlesInThatTank)
            //rFRAG=sum(totalFragsOfSingleTank)/sum(ExpectedFragsPerTank*BattlesInThatTank)
            //rWin=sum(WinsOfSingleTanks)/Sum(ExpectedWinsPerTank*BattlesInThatTank)

            Storage.expDAMAGE = WN8ExpectedTank.expDamage;
            Storage.expFRAG = WN8ExpectedTank.expFrag;
            Storage.expSPOT = WN8ExpectedTank.expSpot;
            Storage.expDEF = WN8ExpectedTank.expDefense;
            Storage.expWIN = WN8ExpectedTank.expWin;

            //Storage.damage = ratingStruct.AvgDamageDealt;
            //Storage.frags = ratingStruct.AvgFrags;
            //Storage.spotted = ratingStruct.AvgSpotted;
            //Storage.defence = ratingStruct.AvgDefencePoints;
            //Storage.winRate = ratingStruct.winRate;

            Storage.rDAMAGEc = WN8_Damage(WN8ExpectedTank, ratingStruct);
            Storage.rFRAGc = WN8_Frags(WN8ExpectedTank, Storage.rDAMAGEc, ratingStruct);
            Storage.rSPOTc = WN8_Spotted(WN8ExpectedTank, Storage.rDAMAGEc, ratingStruct);
            Storage.rDEFc = WN8_Defence(WN8ExpectedTank, Storage.rDAMAGEc, ratingStruct);
            Storage.rWINc = WN8_WinRate(WN8ExpectedTank, ratingStruct);

            Storage.rDAMAGE = 980 * Storage.rDAMAGEc;
            Storage.rFRAG = 210 * Storage.rDAMAGEc * Storage.rFRAGc;
            Storage.rSPOT = 155 * Storage.rFRAGc * Storage.rSPOTc;
            Storage.rDEF = 75 * Storage.rDEFc * Storage.rFRAGc;
            Storage.rWIN = 145 * Math.Min(1.8, Storage.rWINc);

            //if (ratingStruct.countryID == 0 & ratingStruct.tankID == 32)

            //if (Storage.damage == 820)
            //if (ratingStruct.isOverall)
            //{

            //    WOTHelper.AddToLog("Setting exp: " + Storage.rWIN);
            //    WOTHelper.AddToLog("#############");
            //    printExpectedTank(WN8ExpectedTank);
            //    printRatingStruct(ratingStruct);
            //    WOTHelper.AddToLog("rDAMAGEc " + Storage.rDAMAGEc);
            //    WOTHelper.AddToLog("rFRAGc " + Storage.rFRAGc);
            //    WOTHelper.AddToLog("rSPOTc " + Storage.rSPOTc);
            //    WOTHelper.AddToLog("rDEFc " + Storage.rDEFc);
            //    WOTHelper.AddToLog("rWINc " + Storage.rWINc);
            //    WOTHelper.AddToLog("____");
            //    printRatingStruct(ratingStruct);
            //    WOTHelper.AddToLog("rFRAGc " + Storage.rFRAGc);
            //    WOTHelper.AddToLog("rSPOTc " + Storage.rSPOTc);
            //    WOTHelper.AddToLog("rDEFc " + Storage.rDEFc);
            //    WOTHelper.AddToLog("rWINc " + Storage.rWINc);
            //    WOTHelper.AddToLog("____");
            //    WOTHelper.AddToLog("rDAMAGE " + Storage.rDAMAGE);
            //    WOTHelper.AddToLog("rFRAG " + Storage.rFRAG);
            //    WOTHelper.AddToLog("rSPOT " + Storage.rSPOT);
            //    WOTHelper.AddToLog("rDEF " + Storage.rDEF);
            //    WOTHelper.AddToLog("rWIN " + Storage.rWIN);
            //    WOTHelper.AddToLog("rSUM " + Storage.Value);
            //    WOTHelper.AddToLog("rWeight " + Storage.Weight);
            //    WOTHelper.AddToLog("#############");
            //}

            Storage.HTMLColorDescription = WOTStatistics.Core.WOTHtml.WN8ColorScaleDescription(Storage.Value);
            Storage.HTMLColor = WOTStatistics.Core.WOTHtml.WN8ColorScale(Storage.Value);

            return Storage;

        }

        private static double WN8_Frags(WN8ExpValue WN8ExpectedTank, double rDAMAGEc, RatingStructure ratingStruct)
        {
            double rFRAG = 0;
            if (ratingStruct.isOverall)
            {
                rFRAG = ratingStruct.overallFrags;
            }
            else
            {

                if (WN8ExpectedTank.expFrag > 0)
                {
                    rFRAG = ratingStruct.AvgFrags / WN8ExpectedTank.expFrag;
                }
            }
            return Math.Min(rDAMAGEc + 0.2, Math.Max(0, (rFRAG - 0.12) / (1 - 0.12)));
        }

        private static double WN8_Damage(WN8ExpValue WN8ExpectedTank, RatingStructure ratingStruct)
        {
            double rDAMAGE = 0;
            if (ratingStruct.isOverall)
            {
                rDAMAGE = ratingStruct.overallDamageDealt;
            }
            else
            {

                if (WN8ExpectedTank.expDamage > 0)
                {
                    rDAMAGE = ratingStruct.AvgDamageDealt / WN8ExpectedTank.expDamage;
                }
            }
                 return Math.Max(0, (rDAMAGE - 0.22) / (1 - 0.22));            
        }


        private static double WN8_Spotted(WN8ExpValue WN8ExpectedTank, double rDAMAGEc, RatingStructure ratingStruct)
        {
            double rSPOT = 0;

            if (ratingStruct.isOverall)
            {
                rSPOT = ratingStruct.overallSpotted;
            }
            else
            {


                if (WN8ExpectedTank.expSpot > 0)
                {
                    rSPOT = ratingStruct.AvgSpotted / WN8ExpectedTank.expSpot;
                }
            }
            double oResult = Math.Max(0, Math.Min(rDAMAGEc + 0.1, (rSPOT - 0.38) / (1 - 0.38)));

            return oResult;

        }

        private static double WN8_Defence(WN8ExpValue WN8ExpectedTank, double rDAMAGEc, RatingStructure ratingStruct)
        {

            double rDEF = 0;
            if (ratingStruct.isOverall)
            {
                rDEF = ratingStruct.overallDefencePoints;
            }
            else
            {

                if (WN8ExpectedTank.expDefense > 0)
                {
                    rDEF = ratingStruct.AvgDefencePoints / WN8ExpectedTank.expDefense;
                }
            }
            return Math.Max(0, Math.Min(rDAMAGEc + 0.1, (rDEF - 0.10) / (1 - 0.10)));
            //Math.Min(rDAMAGEc + 0.1, Math.Max(0, (rDEF - 0.10) / (1 - 0.10)));
        }

        private static double WN8_WinRate(WN8ExpValue WN8ExpectedTank, RatingStructure ratingStruct)
        {
            double rWIN = 0;
            if (ratingStruct.isOverall)
            {
                rWIN = ratingStruct.overallWinRate;
            }
            else
            {

                if (WN8ExpectedTank.expWin > 0)
                {

                    rWIN = ratingStruct.winRate / WN8ExpectedTank.expWin;
                }
            }
            return Math.Max(0, (rWIN - 0.71) / (1 - 0.71));
        }


        private static string WN8_Tooltip(RatingStorage WN8)
            //string damageCaption, double damage, double damageFormula, string killCaption, double kill, double killFormula, string spottedCaption, double spotted, double spottedFormula, string Translations.TranslationGet("STR_DEFENCE", "de", "Defence"), double defence, double defenceFormula, string winRateCaption, double winRate, double winRateFormula)
        {
            
            
            //, WN8.damage, WN8.rDAMAGE, , WN8.frags, WN8.rFRAG, , WN8.spotted, WN8.rSPOT, , WN8.defence, WN8.rDEF, , WN8.winRate, WN8.rWIN
            string total = FormatNumberToString(WN8.Value, 2);
            double[] valueArray = new double[] { WN8.rDAMAGE, WN8.rFRAG, WN8.rSPOT, WN8.rDEF, WN8.rWIN };
            double maxValue = valueArray.Max();
            double iTotal = maxValue;

            List<string> i = new List<string>();
            string[] s = new string[8] { "", "Value", "Expected", "WN8", "0", "0", "1", "H" };
            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("STR_DAMAGE", "de", "Damage");
            s[1] = FormatNumberToString(WN8.damage, 0);
            s[2] = FormatNumberToString(WN8.rDAMAGE, 0);
            s[3] = FormatNumberToString(WN8.expDAMAGE, 0);
            s[4] = FormatNumberToString(Math.Abs(((WN8.rDAMAGE / iTotal) - 1) * 100), 2);
            s[5] = FormatNumberToString(Math.Abs(((WN8.rDAMAGE / iTotal)) * 100), 2);
            s[6] = "0";
            s[7] = "D";
            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("HTML_CONT_KILLS", "de", "Kills");
            s[1] = FormatNumberToString(WN8.frags, 1);
            s[2] = FormatNumberToString(WN8.rFRAG, 1);
            s[3] = FormatNumberToString(WN8.expFRAG, 1);
            s[4] = FormatNumberToString(Math.Abs(((WN8.rFRAG / iTotal) - 1) * 100), 2);
            s[5] = FormatNumberToString(Math.Abs(((WN8.rFRAG / iTotal)) * 100), 2);
            s[6] = "0";

            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("HTML_CONT_DETECTED", "de", "Detected");
            s[1] = FormatNumberToString(WN8.spotted, 1);
            s[2] = FormatNumberToString(WN8.rSPOT, 1);
            s[3] = FormatNumberToString(WN8.expSPOT, 1);
            s[4] = FormatNumberToString(Math.Abs(((WN8.rSPOT / iTotal) - 1) * 100), 2);
            s[5] = FormatNumberToString(Math.Abs(((WN8.rSPOT / iTotal)) * 100), 2);
            s[6] = "0";

            i.Add(string.Join("|", s));


            s[0] = Translations.TranslationGet("STR_DEFENCE", "de", "Defence");
            s[1] = FormatNumberToString(WN8.defence, 1);
            s[2] = FormatNumberToString(WN8.rDEF, 1);
            s[3] = FormatNumberToString(WN8.expDEF, 1);
            s[4] = FormatNumberToString(Math.Abs(((WN8.rDEF / iTotal) - 1) * 100), 2);
            s[5] = FormatNumberToString(Math.Abs(((WN8.rDEF / iTotal)) * 100), 2);
            s[6] = "0";
            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("STR_WINRATE", "de", "Win Percentage");
            s[1] = FormatNumberToString(WN8.winRate, 1);
            s[2] = FormatNumberToString(WN8.rWIN, 1);
            s[3] = FormatNumberToString(WN8.expWIN, 1);
            s[4] = FormatNumberToString(Math.Abs(((WN8.rWIN / iTotal) - 1) * 100), 2);
            s[5] = FormatNumberToString(Math.Abs(((WN8.rWIN / iTotal)) * 100), 2);
            s[6] = "0";
            i.Add(string.Join("|", s));

            s[0] = "Total";
            s[1] = "";
            s[2] = total;
            s[3] = "";
            s[4] = "0";
            s[5] = "0";
            s[6] = "1";
            s[7] = "T";
            i.Add(string.Join("|", s));
//WOTHelper.AddToLog (string.Join(";", i));
            return string.Join(";", i);
        }
        

        
    }
}
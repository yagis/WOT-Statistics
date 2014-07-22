using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using WOTStatistics.Core;

namespace WOTStatistics.Core
{
    public class RatingStructure
    {
        public string RatingType  { get; set; }

        public bool isOverall { get; set; }
        public bool singleTank { get; set; }

        public int countryID { get; set; }
        public int tankID { get; set; }

        public WN8ExpValues WN8ExpectedTankList { get; set; }
        
        public double tier { get; set; }
        public double globalTier { get; set; }


        public int battlesCount { get; set; }
        public int battlesCount8_8 { get; set; }


        public double overallDamageDealt { get; set; }
        public double damageDealt { get; set; }
        public double AvgDamageDealt
        {
            get
            {
                if (battlesCount > 1)
                {
                    return damageDealt / battlesCount;
                }

                return damageDealt;
              
            }
 
        }

        public double damageAssistedRadio { get; set; }
        public double AvgDamageAssistedRadio
        {
            get
            {

                if ((int)battlesCount8_8 > 0)
                {
                    return damageAssistedRadio / (battlesCount - battlesCount8_8);
                }

                return damageAssistedRadio / battlesCount;
                
            }
        }
        
        public double damageAssistedTracks { get; set; }
        public double AvgDamageAssistedTracks 
        {
            get
            {
                if ((int)battlesCount8_8 > 0)
                {
                    return damageAssistedTracks / (battlesCount - battlesCount8_8);
                }

                return damageAssistedTracks / battlesCount;
            }
        }

        public double overallFrags { get; set; }
        public double frags { get; set; }
        public double AvgFrags
        {        
            get
            {
                if (battlesCount > 1)
                {
                    return frags / battlesCount;
                }

                return frags;
            }
        }

        public double overallSpotted { get; set; }
        public double spotted { get; set; }
        public double AvgSpotted
        {
            get
            {
            if (battlesCount > 1)
            {
                return spotted / battlesCount;
            }

            return spotted;
            }
        }

        public double capturePoints { get; set; }
        public double AvgCapturePoints
        {
                        get
            {
            if (battlesCount > 1)
            {
                return capturePoints / battlesCount;
            }

            return capturePoints;
                        }
        }

        public double overallDefencePoints { get; set; }
        public double defencePoints { get; set; }
        public double AvgDefencePoints
        {
            get
            {
                if (battlesCount > 1)
                {
                    return defencePoints / battlesCount;
                }

                return defencePoints;
            }
        }

        public double wins { get; set; }
        public double expWIN
        {
           get 
            {
                try
                {
                    WN8ExpValue WN8ExpectedTank = WN8ExpectedTankList.GetByTankID(countryID, tankID);
                    if (WN8ExpectedTank != null)
                    {
                        return WN8ExpectedTank.expWin;
                    }
                    else
                    {
                        WOTHelper.AddToLog("Missing Tank in exp Values: " + countryID + " - " + tankID);
                        return 0;
                    }
                    
                }
                catch (Exception ex)
                {
                    WOTHelper.AddToLog(ex.Message);
                    return 0;
                }

            }
        }

        public double winRate
        {
            get
            {

                if (battlesCount == 1 & RatingType == "WN8")
                {
                    return expWIN;
                }

                if (battlesCount == 0 || wins == 0)
                {
                    return 0;
                }

                if (battlesCount == 1 & RatingType == "WN7")
                {
                    return 50;
                }

                //if (battlesCount > 1 & RatingType == "WN7")
                //{
                //    return globalWinRate;
                //}



                return (wins / battlesCount) * 100;
            }
        }

        public double AvgWinRate
        {
            get
            {
                return winRate;
            }
        }

        public double gWinRate { get; set; }

        public double overallWinRate { get; set; }

    }



    public partial class Ratings
    {

 
       

        public static void printRatingStruct(RatingStructure ratingStruct)
        {
                    WOTHelper.AddToLog("ratingStruct.countryID:" + ratingStruct.countryID);
                    WOTHelper.AddToLog("ratingStruct.tankID:" + ratingStruct.tankID);
                    WOTHelper.AddToLog("ratingStruct.tier:" + ratingStruct.tier);
                    WOTHelper.AddToLog("ratingStruct.battlesCount:" + ratingStruct.battlesCount);
                    WOTHelper.AddToLog("ratingStruct.battlesCount8_8:" + ratingStruct.battlesCount8_8);
                    WOTHelper.AddToLog("ratingStruct.capturePoints:" + ratingStruct.capturePoints);
                    WOTHelper.AddToLog("ratingStruct.defencePoints:" + ratingStruct.defencePoints);
                    WOTHelper.AddToLog("ratingStruct.damageAssistedRadio:" + ratingStruct.damageAssistedRadio);
                    WOTHelper.AddToLog("ratingStruct.damageAssistedTracks:" + ratingStruct.damageAssistedTracks);
                    WOTHelper.AddToLog("ratingStruct.damageDealt:" + ratingStruct.damageDealt);
                    WOTHelper.AddToLog("ratingStruct.frags:" + ratingStruct.frags);
                    WOTHelper.AddToLog("ratingStruct.spotted:" + ratingStruct.spotted);
                    WOTHelper.AddToLog("ratingStruct.wins:" + ratingStruct.wins);        
                    WOTHelper.AddToLog("ratingStruct.winRate:" + ratingStruct.winRate);
                    WOTHelper.AddToLog("--------------");
                    WOTHelper.AddToLog("ratingStruct.AvgcapturePoints:" + ratingStruct.AvgCapturePoints);
                    WOTHelper.AddToLog("ratingStruct.AvgdefencePoints:" + ratingStruct.AvgDefencePoints);
                    WOTHelper.AddToLog("ratingStruct.AvgdamageAssistedRadio:" + ratingStruct.AvgDamageAssistedRadio);
                    WOTHelper.AddToLog("ratingStruct.AvgdamageAssistedTracks:" + ratingStruct.AvgDamageAssistedTracks);
                    WOTHelper.AddToLog("ratingStruct.AvgdamageDealt:" + ratingStruct.AvgDamageDealt);
                    WOTHelper.AddToLog("ratingStruct.Avgfrags:" + ratingStruct.AvgFrags);
                    WOTHelper.AddToLog("ratingStruct.Avgspotted:" + ratingStruct.AvgSpotted);
                    WOTHelper.AddToLog("--------------");
                    WOTHelper.AddToLog("ratingStruct.overallDamageDealt:" + ratingStruct.overallDamageDealt);
                    WOTHelper.AddToLog("ratingStruct.overallFrag:" + ratingStruct.overallFrags);
                    WOTHelper.AddToLog("ratingStruct.overallSpot:" + ratingStruct.overallSpotted);
                    WOTHelper.AddToLog("ratingStruct.overallDefensePoints:" + ratingStruct.overallDefencePoints);
                    WOTHelper.AddToLog("ratingStruct.overallWinrate:" + ratingStruct.overallWinRate);

                    WOTHelper.AddToLog("--------------");
        }

        private static void printExpectedTank(WN8ExpValue WN8EX)
        {
            WOTHelper.AddToLog("WN8ExpValue.expDamage:" + WN8EX.expDamage);
            WOTHelper.AddToLog("WN8ExpValue.expDefense:" + WN8EX.expDefense);
            WOTHelper.AddToLog("WN8ExpValue.expFrag:" + WN8EX.expFrag);
            WOTHelper.AddToLog("WN8ExpValue.expSpot:" + WN8EX.expSpot);
            WOTHelper.AddToLog("WN8ExpValue.expWin:" + WN8EX.expWin);
        }

       
		
  
        
        public class RatingStorage
        {
         
            public RatingStorage(RatingStructure ratingStruct)
            {
                battlesCount = ratingStruct.battlesCount;
                battlesCount8_8 = ratingStruct.battlesCount8_8;
                damage = ratingStruct.damageDealt;
                damageAssistedRadio = ratingStruct.damageAssistedRadio;
                damageAssistedTracks = ratingStruct.damageAssistedTracks;
                frags = ratingStruct.frags;
                spotted = ratingStruct.spotted;
                defence = ratingStruct.defencePoints;
                capture = ratingStruct.capturePoints;
                winRate = ratingStruct.winRate;

                if (battlesCount>0)
                {
                    AvgDamage = damage / battlesCount;
                    AvgFrags = frags / battlesCount;
                    AvgSpotted = spotted / battlesCount;
                    AvgDefence = defence / battlesCount;
                    AvgCapture = capture / battlesCount;

                }
                if (battlesCount8_8 > 0)
                {
                    AvgDamageAssistedRadio = damageAssistedRadio / battlesCount8_8;
                    AvgDamageAssistedTracks = damageAssistedTracks / battlesCount8_8;

                }

                //printRatingStruct(ratingStruct);

            }


            public int battlesCount { get; set; }
            public int battlesCount8_8 { get; set; }
            public double damage { get; set; }
            public double damageAssistedRadio { get; set; }
            public double damageAssistedTracks { get; set; }
            public double frags { get; set; }
            public double spotted { get; set; }
            public double defence { get; set; }
            public double capture { get; set; }
            public double winRate { get; set; }

            public double AvgDamage { get; set; }
            public double AvgDamageAssistedRadio { get; set; }
            public double AvgDamageAssistedTracks { get; set; }
            public double AvgFrags { get; set; }
            public double AvgSpotted { get; set; }
            public double AvgDefence { get; set; }
            public double AvgCapture { get; set; }


            public double expDAMAGE { get; set; }
            public double expFRAG { get; set; }
            public double expSPOT { get; set; }
            public double expDEF { get; set; }
            public double expWIN { get; set; }

            public double rDAMAGEc { get; set; }
            public double rFRAGc { get; set; }
            public double rSPOTc { get; set; }
            public double rDEFc { get; set; }
            public double rWINc { get; set; }

            public double rDAMAGE { get; set; }
            public double rDAMAGE_RADIO { get; set; }
            public double rDAMAGE_TRACKS { get; set; }
            public double rFRAG { get; set; }
            public double rSPOT { get; set; }
            public double rCAP { get; set; }
            public double rDEF { get; set; }
            public double rWIN { get; set; }
            public double rTIERMALUS { get; set; }

            public string HTMLColorDescription { get; set; }
            public string HTMLColor { get; set; }

            public double Value
            {
                get
                {
                    double rSUM = rDAMAGE + rDAMAGE_RADIO + rDAMAGE_TRACKS + rFRAG + rSPOT + rCAP + rDEF + rWIN - rTIERMALUS;
                 
                    if (rSUM < 0 | double.IsNaN(rSUM))
                    {
                        //WOTHelper.AddToLog("11: " + rSUM);
                        rSUM = -1;
                    }
                    return (double)rSUM;
                }
            }
            public int Weight
            {
                get
                {
                    double rValue = Value;

                    if ((int)rValue < 1 | (int)battlesCount == 0)
                    {
                        //WOTHelper.AddToLog("33: " + rValue);
                        return 0;
                    }

                    return Convert.ToInt32((int)rValue * (int)battlesCount);
                }
            }
        
        }





        #region Helpers
        private static double GetMinValue(double value, double capValue)
        {
            if (value >= capValue)
                return capValue;
            else
                return value;
        }

        private static string FormatNumberToString(double value, int decimalPlaces)
        {

            return WOTHelper.FormatNumberToString(value, decimalPlaces);

        }
        #endregion
    }
}
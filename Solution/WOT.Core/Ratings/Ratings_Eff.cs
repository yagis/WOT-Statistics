using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using WOTStatistics.Core;

namespace WOTStatistics.Core
{
 

    partial class Ratings
    {

        public static RatingStorage GetRatingEff(RatingStructure ratingStruct)
        {
            return GetRatingEffCalc(ratingStruct);
        }

        public static string GetRatingEffToolTip(RatingStructure ratingStruct)
        {
            return Eff_Tooltip(ratingStruct);
        }


        private static RatingStorage GetRatingEffCalc(RatingStructure ratingStruct)
        {
            ratingStruct.RatingType = "Eff";
            RatingStorage oStorage = new RatingStorage(ratingStruct);
            if (ratingStruct.battlesCount==0)
            {
                return oStorage;
            }

             oStorage.rDAMAGE = EFF_Damage(ratingStruct);
             oStorage.rFRAG =  EFF_Kills(ratingStruct);
             oStorage.rSPOT = EFF_Spotted(ratingStruct);
             oStorage.rCAP = EFF_Captured(ratingStruct);
             oStorage.rDEF = EFF_Defence(ratingStruct);
            return oStorage;
        
        
        }

        private static string Eff_Tooltip(RatingStructure ratingStruct)
        {

            double damageFormula = EFF_Damage(ratingStruct);
            double killFormula = EFF_Kills(ratingStruct);
            double spottedFormula = EFF_Spotted(ratingStruct);
            double captureFormula = EFF_Captured(ratingStruct);
            double defenceFormula = EFF_Defence(ratingStruct);
            string total = FormatNumberToString(damageFormula + killFormula + spottedFormula + captureFormula + defenceFormula, 2);
            double[] valueArray = new double[] { damageFormula, killFormula, spottedFormula, captureFormula, defenceFormula };
            double maxValue = valueArray.Max();
            double iTotal = maxValue;

            List<string> i = new List<string>();
            string[] s = new string[7] { "", "Value", "Eff", "0", "0", "1", "H" };
            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("STR_DAMAGE", "de", "Damage");
            s[1] = FormatNumberToString(ratingStruct.damageDealt, 0);
            s[2] = FormatNumberToString(damageFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((damageFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((damageFormula / iTotal)) * 100), 2);
            s[5] = "0";
            s[6] = "D";
            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("HTML_CONT_KILLS", "de", "Kills");
            s[1] = FormatNumberToString(ratingStruct.frags, 0);
            s[2] = FormatNumberToString(killFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((killFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((killFormula / iTotal)) * 100), 2);
            s[5] = "0";

            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("HTML_CONT_DETECTED", "de", "Detected");
            s[1] = FormatNumberToString(ratingStruct.spotted, 0);
            s[2] = FormatNumberToString(spottedFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((spottedFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((spottedFormula / iTotal)) * 100), 2);
            s[5] = "0";

            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("STR_CAPTURE", "de", "Capture");
            s[1] = FormatNumberToString(ratingStruct.capturePoints, 0);
            s[2] = FormatNumberToString(captureFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((captureFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((captureFormula / iTotal)) * 100), 2);
            s[5] = "0";
            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("STR_DEFENCE", "de", "Defence");
            s[1] = FormatNumberToString(ratingStruct.defencePoints, 0);
            s[2] = FormatNumberToString(defenceFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((defenceFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((defenceFormula / iTotal)) * 100), 2);
            s[5] = "0";
            i.Add(string.Join("|", s));


            s[0] = "Total";
            s[1] = "";
            s[2] = total;
            s[3] = "0";
            s[4] = "0";
            s[5] = "1";
            s[6] = "T";
            i.Add(string.Join("|", s));

            return string.Join(";", i);
        }

        //DAMAGE * (10 / (TIER + 2)) * (0.23 + 2*TIER / 100)
        //FRAGS * 250 +
        //SPOT * 150 +
        //(log(CAP + 1,1.732))*150 + 
        //DEF * 150;

        private static double EFF_Damage(RatingStructure ratingStruct)
        {
            return ratingStruct.AvgDamageDealt * (10 / (ratingStruct.tier + 2)) * (0.23 + 2 * ratingStruct.tier / 100);
        }

        private static double EFF_Kills(RatingStructure ratingStruct)
        {
            return ((ratingStruct.AvgFrags) * 250);
        }

        private static double EFF_Spotted(RatingStructure ratingStruct)
        {
            return ((ratingStruct.AvgSpotted) * 150);
        }

        private static double EFF_Captured(RatingStructure ratingStruct)
        {
            return ((Math.Log((ratingStruct.AvgCapturePoints) + 1, 1.732)) * 150);
        }

        private static double EFF_Defence(RatingStructure ratingStruct)
        {
            return ((ratingStruct.AvgDefencePoints) * 150);
        }
       

        
    }
}
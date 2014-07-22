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



        public static RatingStorage GetRatingBR(RatingStructure ratingStruct)
        {
            return GetRatingBRCalc(ratingStruct);
        }



        public static string GetRatingBRToolTip(RatingStructure ratingStruct)
        {
            return BR_Tooltip(ratingStruct);//Translations.TranslationGet("STR_DAMAGE", "de", "Damage"), ratingStruct.damageDealt, BR_Damage(ratingStruct.damageDealt, ratingStruct.tier), , frags, BR_Kills(frags, avgTier), Translations.TranslationGet("HTML_CONT_DETECTED", "de", "Detected"), spotted, BR_Spotted(spotted), Translations.TranslationGet("STR_CAPTURE", "de", "Capture"), capture, BR_Captured(capture), Translations.TranslationGet("STR_DEFENCE", "de", "Defence"), defence, BR_Defence(defence), , defence, BR_DamageAssistedRadio(damageAssistedRadio, avgTier), Translations.TranslationGet("STR_DAMAGE_ASSISTED_TRACK", "de", "Damage Assisted Track"), defence, BR_DamageAssistedTrack(damageAssistedTrack, avgTier));
        }





        private static RatingStorage GetRatingBRCalc(RatingStructure ratingStruct)
        {
            ratingStruct.RatingType = "BR";
            RatingStorage oStorage = new RatingStorage(ratingStruct);

            if (ratingStruct.battlesCount == 0)
            {
                return oStorage;
            }
            
            oStorage.rDAMAGE=  BR_Damage(ratingStruct);
            oStorage.rFRAG = BR_Kills(ratingStruct);
            oStorage.rSPOT = BR_Spotted(ratingStruct);
            oStorage.rCAP = BR_Captured(ratingStruct);
            oStorage.rDEF = BR_Defence(ratingStruct);
            oStorage.rDAMAGE_RADIO = BR_DamageAssistedRadio(ratingStruct);
            oStorage.rDAMAGE_TRACKS = BR_DamageAssistedTrack(ratingStruct);

            return oStorage;

        }

        private static string BR_Tooltip(RatingStructure ratingStruct)
        {

            double damageFormula = BR_Damage(ratingStruct);
            double killFormula = BR_Kills(ratingStruct);
            double spottedFormula = BR_Spotted(ratingStruct);
            double captureFormula = BR_Captured(ratingStruct);
            double defenceFormula = BR_Defence(ratingStruct);
            double damageAssistedRadioFormula = BR_DamageAssistedRadio(ratingStruct);
            double damageAssistedTracksFormula = BR_DamageAssistedTrack(ratingStruct);


            string total = FormatNumberToString(damageFormula + killFormula + spottedFormula + captureFormula + defenceFormula + damageAssistedRadioFormula + damageAssistedTracksFormula, 2);
            double[] valueArray = new double[] { damageFormula, killFormula, spottedFormula, captureFormula, defenceFormula, damageAssistedRadioFormula, damageAssistedTracksFormula };
            double maxValue = valueArray.Max();
            double iTotal = maxValue;

            List<string> i = new List<string>();
            string[] s = new string[7] { "", "Value", "BR", "0", "0", "1", "H" };
            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("STR_DAMAGE", "de", "Damage");
            s[1] = FormatNumberToString(ratingStruct.damageDealt, 0);
            s[2] = FormatNumberToString(damageFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((damageFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((damageFormula / iTotal)) * 100), 2);
            s[5] = "0";
            s[6] = "D";
            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("STR_DAMAGE_ASSISTED_RADIO", "de", "Damage Assisted Radio");
            s[1] = FormatNumberToString(ratingStruct.damageAssistedRadio, 0);
            s[2] = FormatNumberToString(damageAssistedRadioFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((damageAssistedRadioFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((damageAssistedRadioFormula / iTotal)) * 100), 2);
            s[5] = "0";
            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("STR_DAMAGE_ASSISTED_TRACK", "de", "Damage Assisted Track");
            s[1] = FormatNumberToString(ratingStruct.damageAssistedTracks, 0);
            s[2] = FormatNumberToString(damageAssistedTracksFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((damageAssistedTracksFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((damageAssistedTracksFormula / iTotal)) * 100), 2);
            s[5] = "0";
            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("HTML_CONT_KILLS", "de", "Kills");
            s[1] = FormatNumberToString(ratingStruct.frags, 0);
            s[2] = FormatNumberToString(killFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((killFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((killFormula / iTotal)) * 100), 2);
            s[5] = "0";

            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("HTML_CONT_DETECTED", "de", "Spotted");
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

        private static double BR_Damage(RatingStructure ratingStruct)
        {
            double result = (ratingStruct.AvgDamageDealt) * (0.2 + 1.5 / ratingStruct.tier);
            //WOTHelper.AddToLog("BR Damage " + result);
            return result;
        }

        private static double BR_DamageAssistedRadio(RatingStructure ratingStruct)
        {

            double result = (((ratingStruct.damageAssistedRadio / (ratingStruct.battlesCount - ratingStruct.battlesCount8_8)) / 2)) * (0.2 + 1.5 / ratingStruct.tier);
            //if (result < 0)
            //{
            //    result = 0;
            //}
            ////WOTHelper.AddToLog("BR AssRadio " + result);
            return result; 
        }

        private static double BR_DamageAssistedTrack(RatingStructure ratingStruct)
        {
     
           double result = (((ratingStruct.damageAssistedTracks / (ratingStruct.battlesCount - ratingStruct.battlesCount8_8)) / 2)) * (0.2 + 1.5 / ratingStruct.tier);
            //if (typeof(result))
            //{
            //    result = 0;
            //}
            ////WOTHelper.AddToLog("BR AssTrack " + result);
            return result; 
        }

        private static double BR_Kills(RatingStructure ratingStruct)
        {
            double result = (ratingStruct.AvgFrags) * (350.0 - ratingStruct.tier * 20.0);
            //WOTHelper.AddToLog("BR Kills " + result);
            return result; 
        }

        private static double BR_Spotted(RatingStructure ratingStruct)
        {
            double result = 200.0 * (ratingStruct.AvgSpotted);
            //WOTHelper.AddToLog("BR Spotted " + result);
            return result; 
        }

        private static double BR_Captured(RatingStructure ratingStruct)
        {
            double result = 15.0 * (ratingStruct.AvgCapturePoints);
            //WOTHelper.AddToLog("BR Capture " + result);
            return result; 
        }

        private static double BR_Defence(RatingStructure ratingStruct)
        {
            double result = 15.0 * (ratingStruct.AvgDefencePoints);
            //WOTHelper.AddToLog("BR Defence " + result);
            return result;
        }
      
       
		
  
   
    }
}
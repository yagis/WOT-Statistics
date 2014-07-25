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

 
      

        public static RatingStorage GetRatingWN7(RatingStructure ratingStruct)
        {
            return GetRatingWN7Calc(ratingStruct);
        }




        public static string GetRatingWN7ToolTip(RatingStructure ratingStruct)
        {
            return WN7_Tooltip(ratingStruct);//Translations.TranslationGet("STR_DAMAGE", "de", "Damage"), ratingStruct.damageDealt, WN7_Damage(ratingStruct.battlesCount, ratingStruct.damageDealt, avgTier), Translations.TranslationGet("HTML_CONT_KILLS", "de", "Kills"), frags, WN7_Frags(battles, frags, avgTier), Translations.TranslationGet("HTML_CONT_DETECTED", "de", "Detected"), spotted, WN7_Spotted(ratingStruct.battlesCount, spotted, globalAvgTier), Translations.TranslationGet("STR_DEFENCE", "de", "Defence"), defence, WN7_Defence(ratingStruct.battlesCount, defence), Translations.TranslationGet("STR_WINRATE", "de", "Win Percentage"), winRate, WN7_WinRate(winRate), Translations.TranslationGet("STR_TIERMALUS", "de", "Tier Malus"), 0, WN7_TierMalus(ratingStruct.battlesCount, globalAvgTier));
        }



       
       
	
        private static RatingStorage GetRatingWN7Calc(RatingStructure ratingStruct)
        {
            ratingStruct.RatingType = "WN7";
            RatingStorage Storage = new RatingStorage(ratingStruct);
            if (ratingStruct.battlesCount == 0)
            {
                return Storage;
            }
			
            Storage.rDAMAGE = WN7_Damage(ratingStruct);
            Storage.rFRAG = WN7_Frags(ratingStruct);
            Storage.rSPOT = WN7_Spotted(ratingStruct);
            Storage.rDEF = WN7_Defence(ratingStruct) ;
            Storage.rWIN = WN7_WinRate(ratingStruct);
            Storage.rTIERMALUS = WN7_TierMalus(ratingStruct);

            //if (ratingStruct.countryID == 0 & ratingStruct.tankID == 32)
            //{
            //    WOTHelper.AddToLog("WN7 : " + Storage.Value);
            //    WOTHelper.AddToLog("TIER: " + ratingStruct.tier);
            //    WOTHelper.AddToLog("WRAT: " + ratingStruct.winRate);
            //    WOTHelper.AddToLog("TDMG: " + Storage.rDAMAGE);
            //    WOTHelper.AddToLog("TFRG: " + Storage.rFRAG);
            //    WOTHelper.AddToLog("TSPT: " + Storage.rSPOT);
            //    WOTHelper.AddToLog("TDEF: " + Storage.rDEF);
            //    WOTHelper.AddToLog("TWIN: " + Storage.rWIN);
            //    WOTHelper.AddToLog("TMAL: " + Storage.rTIERMALUS);
            //}

            return Storage;
        }

        private static double WN7_Frags(RatingStructure ratingStruct)
        {

            return (1240 - 1040 / Math.Pow(GetMinValue(ratingStruct.tier, 6), 0.164)) * (ratingStruct.AvgFrags);
        }

        private static double WN7_Damage(RatingStructure ratingStruct)
        {
            return ratingStruct.AvgDamageDealt * 530 / (184 * Math.Pow(Math.Exp(1), (0.24 * ratingStruct.tier)) + 130);
        }

        private static double WN7_Spotted(RatingStructure ratingStruct)
        {
            
            return (ratingStruct.AvgSpotted) * 125 * (GetMinValue(ratingStruct.tier, 3)) / 3;
        }

        private static double WN7_Defence(RatingStructure ratingStruct)
        {
        
            return GetMinValue(ratingStruct.AvgDefencePoints, 2.2) * 100;
        }

        private static double WN7_WinRate(RatingStructure ratingStruct)
        {
            double winRate = ratingStruct.gWinRate;

			// TODO: Added by BadButton 2014-06-25, bugfix winrate-part for wn7 formula, to be chacked by Phalynx
			if (ratingStruct.battlesCount == 1)
				winRate = 50;
			return (((185 / (0.17 + Math.Exp((winRate - 35) * -0.134))) - 500) * 0.45);

			// TODO: Removed by BadButton, if above work, just remove this
			//if (ratingStruct.gWinRate == 0 & ratingStruct.winRate > 0)
			//{
			//    ratingStruct.gWinRate = ratingStruct.winRate;
			//}
			//return (((185 / (0.17 + Math.Exp(((ratingStruct.gWinRate * 100) - 35) * -0.134))) - 500) * 0.45);
			////return ((185 / (0.17 + Math.Pow(Math.Exp(1), ((ratingStruct.winRate - 35) * -0.134)))) - 500) * 0.45;
		}

		// TODO: Added by BadButton 2014-06-25, new method to return correct winrate factor used for wn7 caculation
		private static double WN7_WinRateTooltipPrefix(RatingStructure ratingStruct)
		{
			double winRate = ratingStruct.gWinRate;
			if (ratingStruct.battlesCount == 1)
				winRate = 50;
			return winRate;
		}

        private static double WN7_TierMalus(RatingStructure ratingStruct)
        {
            return (((5 - Math.Min(ratingStruct.tier, 5)) * 125) / (1 + Math.Exp(ratingStruct.tier - Math.Pow(ratingStruct.battlesCount / 220, 3 / ratingStruct.tier)) * 1.5));
            //return (((5 - Math.Min(ratingStruct.tier, 5)) * 125) / (1 + Math.Pow(Math.Exp(1), ((ratingStruct.tier - Math.Pow((ratingStruct.battlesCount / 220), (3 / ratingStruct.tier))) * 1.5)))) * -1;
            //     (((5 - mi     n(ratingStruct.tier,5))*125)/(1+exp(ratingStruct.tier-pow($missingdossier['battles']/220,3/$missingdossier['tier']))*1.5));

        }

        private static string WN7_Tooltip(RatingStructure ratingStruct)
        {

            double damageFormula = WN7_Damage(ratingStruct);
            double killFormula = WN7_Frags(ratingStruct);
            double spottedFormula = WN7_Spotted(ratingStruct);
            double winRateFormula = WN7_WinRate(ratingStruct);
            double defenceFormula = WN7_Defence(ratingStruct);
            double tierMalusFormula = WN7_TierMalus(ratingStruct);



            string total = FormatNumberToString(damageFormula + killFormula + spottedFormula + winRateFormula + defenceFormula + tierMalusFormula, 2);
            double[] valueArray = new double[] { damageFormula, killFormula, spottedFormula, winRateFormula, defenceFormula, tierMalusFormula };
            double maxValue = valueArray.Max();
            double iTotal = maxValue;

            List<string> i = new List<string>();
            string[] s = new string[7] { "", "Value", "WN7", "0", "0", "1", "H" };
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

            s[0] = Translations.TranslationGet("HTML_CONT_DETECTED", "de", "Spotted");
            s[1] = FormatNumberToString(ratingStruct.spotted, 0);
            s[2] = FormatNumberToString(spottedFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((spottedFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((spottedFormula / iTotal)) * 100), 2);
            s[5] = "0";

            i.Add(string.Join("|", s));


            s[0] = Translations.TranslationGet("STR_DEFENCE", "de", "Defence");
            s[1] = FormatNumberToString(ratingStruct.defencePoints, 0);
            s[2] = FormatNumberToString(defenceFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((defenceFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((defenceFormula / iTotal)) * 100), 2);
            s[5] = "0";
            i.Add(string.Join("|", s));

			s[0] = Translations.TranslationGet("STR_WINRATE", "de", "Win Percentage");
			// TODO: Added by BadButton 2014-06-25 - Added method for getting correct win rate used for calculation, to be chacked by Phalynx
			s[1] = FormatNumberToString(WN7_WinRateTooltipPrefix(ratingStruct), 0);
            s[2] = FormatNumberToString(winRateFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((winRateFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((winRateFormula / iTotal)) * 100), 2);
            s[5] = "0";
            i.Add(string.Join("|", s));

            s[0] = Translations.TranslationGet("STR_TIERMALUS", "de", "Tier Malus");
            s[1] = FormatNumberToString(ratingStruct.tier, 0);
            s[2] = FormatNumberToString(tierMalusFormula, 2);
            s[3] = FormatNumberToString(Math.Abs(((tierMalusFormula / iTotal) - 1) * 100), 2);
            s[4] = FormatNumberToString(Math.Abs(((tierMalusFormula / iTotal)) * 100), 2);
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


            //RatingStorage rr = new RatingStorage(ratingStruct);

            return string.Join(";", i);

            


        }
      
        
  
  




    }
}
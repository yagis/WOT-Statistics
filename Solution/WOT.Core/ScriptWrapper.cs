using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WOT.Core
{
    public static class ScriptWrapper 
    {

        public static void Initialise(string scriptPath)
        {
            WOT_Script.Initialise(scriptPath);
        }

        public static double GetRatingEff(double battles, double damage, double avgTier, double frags, double spotted, double capture, double defence, double winRate, bool singleTank)
        {

            return WOT_Script.GetRatingEff(battles, damage, avgTier, frags, spotted, capture, defence, winRate, singleTank);
        }

        public static double GetRatingBR(double battles, double damage, double damageAssistedRadio, double damageAssistedTracks, double avgTier, double frags, double spotted, double capture, double defence, double winRate, bool singleTank)
        {

            return WOT_Script.GetRatingBR(battles, damage, damageAssistedRadio, damageAssistedTracks, avgTier, frags, spotted, capture, defence, winRate, singleTank);
        }

        public static double GetRatingWN7(double battles, double damage, double avgTier, double frags, double spotted, double capture, double defence, double winRate, bool singleTank)
        {

            return WOT_Script.GetRatingWN7(battles, damage, avgTier, frags, spotted, capture, defence, winRate, singleTank);
        }

        public static double GetRatingWN8(int countryID, int tankID, double battles, double damage, double avgTier, double frags, double spotted, double defence, double winRate, bool singleTank)
        {

            return WOT_Script.GetRatingWN8(countryID, tankID, battles, damage, avgTier, frags, spotted, defence, winRate, singleTank);
        }

        public static string GetRatingEffToolTip(double battles, int countryID, int tankID, double damage, double damageAssistedRadio, double damageAssistedTrack, double avgTier, double frags, double spotted, double capture, double defence, double winRate, double globalAvgTier, double globalAvgDefPoints, double globalBattlesCount)
        {
            return WOT_Script.EfficiencyToolTip("Eff", countryID, tankID, battles, damage, damageAssistedRadio, damageAssistedTrack, avgTier, frags, spotted, capture, defence, winRate, globalAvgTier, globalAvgDefPoints, globalBattlesCount);
        }

        public static string GetRatingBRToolTip(double battles, int countryID, int tankID, double damage, double damageAssistedRadio, double damageAssistedTrack, double avgTier, double frags, double spotted, double capture, double defence, double winRate, double globalAvgTier, double globalAvgDefPoints, double globalBattlesCount)
        {
            return WOT_Script.EfficiencyToolTip("BR", countryID, tankID, battles, damage, damageAssistedRadio, damageAssistedTrack, avgTier, frags, spotted, capture, defence, winRate, globalAvgTier, globalAvgDefPoints, globalBattlesCount);
        }

        public static string GetRatingWN7ToolTip(double battles, int countryID, int tankID, double damage, double damageAssistedRadio, double damageAssistedTrack, double avgTier, double frags, double spotted, double capture, double defence, double winRate, double globalAvgTier, double globalAvgDefPoints, double globalBattlesCount)
        {
            return WOT_Script.EfficiencyToolTip("WN7", countryID, tankID, battles, damage, damageAssistedRadio, damageAssistedTrack, avgTier, frags, spotted, capture, defence, winRate, globalAvgTier, globalAvgDefPoints, globalBattlesCount);
        }

        public static string GetRatingWN8ToolTip(double battles, int countryID, int tankID, double damage, double damageAssistedRadio, double damageAssistedTrack, double avgTier, double frags, double spotted, double capture, double defence, double winRate, double globalAvgTier, double globalAvgDefPoints, double globalBattlesCount)
        {
            return WOT_Script.EfficiencyToolTip("WN8", countryID, tankID, battles, damage, damageAssistedRadio, damageAssistedTrack, avgTier, frags, spotted, capture, defence, winRate, globalAvgTier, globalAvgDefPoints, globalBattlesCount);
        }

        public static Dictionary<string, string> GetTypeDefinitions()
        {
            return WOT_Script.GetTypeDefinitions();
        }

        public static string GetTypeDescription()
        {
            string value = "";
           //if (!WOT_Script.GetTypeDefinitions().TryGetValue(UserSettings.RatingSystem, out value))
                value = WOT_Script.GetTypeDefinitions()["EFF"];

           return value;
        }
    }
}

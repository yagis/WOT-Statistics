using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Web.UI;
using System.IO;

namespace WOTStatistics.Core
{
    

	partial class WOTHtml : WOTHtmlBase
	{
        public static WN8ExpValues WN8ExpectedTankList = new WN8ExpValues();
        public string RecentBattles(string playerName)
        {
            string html = @"<div id='toolTipLayer' style='position:absolute; visibility: hidden;left:0;right:0;'></div><p class='b-gray-text recentbattlesheading'>" + GetRecentBattlesDescription(playerName) +

                 @"</p>				<table class='b-gray-text sortable' width=100%>
								<thead>
                				<tr>
										<th>#</th>";
            switch (UserSettings.GroupLPT)
            {
                case "3":
                    html += @"<th>" + Translations.TranslationGet("HTML_HEAD_COUNTRY", "DE", "Country") + @"</th><th>" + Translations.TranslationGet("HTML_HEAD_BATTLES", "DE", "Battles") + @"</th><th>" + Translations.TranslationGet("HTML_CONT_VICTORY", "DE", "Victory") + @"</th><th>" + Translations.TranslationGet("HTML_CONT_DEFEAT", "DE", "Defeat") + @"</th><th>" + Translations.TranslationGet("HTML_CONT_DRAW", "DE", "Draw") + @"</th>";
                    break;
                case "2":
                    html += @"<th>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + @"</th><th>" + Translations.TranslationGet("HTML_HEAD_BATTLES", "DE", "Battles") + @"</th><th>" + Translations.TranslationGet("HTML_CONT_VICTORY", "DE", "Victory") + @"</th><th>" + Translations.TranslationGet("HTML_CONT_DEFEAT", "DE", "Defeat") + @"</th><th>" + Translations.TranslationGet("HTML_CONT_DRAW", "DE", "Draw") + @"</th>";
                    break;
                case "0":
                    html += @"<th>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + @"</th><th>" + Translations.TranslationGet("HTML_HEAD_TANK", "DE", "Tank") + @"</th><th>" + Translations.TranslationGet("HTML_STATUS", "DE", "Status") + @"</th>";
                    break;
                case "1":
                    html += @"<th>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + @"</th><th>" + Translations.TranslationGet("HTML_HEAD_TANK", "DE", "Tank") + @"</th><th>" + Translations.TranslationGet("HTML_HEAD_BATTLES", "DE", "Battles") + @"</th><th>" + Translations.TranslationGet("HTML_CONT_VICTORY", "DE", "Victory") + @"</th><th>" + Translations.TranslationGet("HTML_CONT_DEFEAT", "DE", "Defeat") + @"</th><th>" + Translations.TranslationGet("HTML_CONT_DRAW", "DE", "Draw") + @"</th>";
                    break;
                default:
                    html += @"<th>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + @"</th><th>" + Translations.TranslationGet("HTML_HEAD_TANK", "DE", "Tank") + @"</th><th>" + Translations.TranslationGet("HTML_STATUS", "DE", "Status") + @"</th>";
                    break;
            }


            html += @"<th>" + Translations.TranslationGet("HTML_SURVIVED", "DE", "Survived") + @"</th>
            <th>" + Translations.TranslationGet("HTML_CONT_DMGCAUSED_00", "DE", "Damage<br/>Caused") + @"</th>";
            if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                html += @"<th>" + Translations.TranslationGet("HTML_DAMDEALT_01", "DE", "Overall<br/>Damage Caused Avg.") + @"</th>";
            else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                html += @"<th>" + Translations.TranslationGet("HTML_DAMDEALT_02", "DE", "Tank<br/>Damage<br/>Caused Avg.") + @"</th>";

            html += @"<th>" + Translations.TranslationGet("HTML_CONT_DMGASSRADIO_00", "DE", "Damage<br/>Assisted Radio") + @"</th>";
            if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                html += @"<th>" + Translations.TranslationGet("HTML_DAMASSRADIO_01", "DE", "Overall<br/>Damage Assisted Radio Avg.") + @"</th>";
            else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                html += @"<th>" + Translations.TranslationGet("HTML_DAMASSRADIO_02", "DE", "Tank<br/>Damage<br/>Assisted Radio Avg.") + @"</th>";

            html += @"<th>" + Translations.TranslationGet("HTML_CONT_DMGASSTRACKS_00", "DE", "Damage<br/>Assisted Tracks") + @"</th>";
            if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                html += @"<th>" + Translations.TranslationGet("HTML_DAMASSTRACK_01", "DE", "Overall<br/>Damage Assisted Tracks Avg.") + @"</th>";
            else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                html += @"<th>" + Translations.TranslationGet("HTML_DAMASSTRACK_02", "DE", "Tank<br/>Damage<br/>Assisted Tracks Avg.") + @"</th>";

            html += @"<th>" + Translations.TranslationGet("HTML_CONT_DMGREC_00", "DE", "Damage<br/>Received") + @"</th>";
            if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                html += @"<th>" + Translations.TranslationGet("HTML_DAMRECEIV_01", "DE", "Overall<br/>Damage<br/>Received Avg.") + @"</th>";
            else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                html += @"<th>" + Translations.TranslationGet("HTML_DAMRECEIV_02", "DE", "Tank Damage Received Avg.") + @"</th>";

            html += @"<th>" + Translations.TranslationGet("HTML_CONT_KILLS", "DE", "Kills") + @"</th>";
            if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                html += @"<th>" + Translations.TranslationGet("HTML_CONT_KILLS_01", "DE", "Overall<br/>Kills Avg.") + @"</th>";
            else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                html += @"<th>" + Translations.TranslationGet("HTML_CONT_KILLS_02", "DE", "Tank<br/>Kills Avg.") + @"</th>";

            html += @"<th>" + Translations.TranslationGet("HTML_EXPER_SHORT", "DE", "XP") + @"</th>";
            if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                html += @"<th>" + Translations.TranslationGet("HTML_XP_01", "DE", "Overall<br/>XP<br/>Avg.") + @"</th>";
            else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                html += @"<th>" + Translations.TranslationGet("HTML_XP_02", "DE", "Tank<br/>XP<br/>Avg.") + @"</th>";

            html += @"<th>" + Translations.TranslationGet("HTML_CONT_DETECTED", "DE", "Detected") + @"</th>";
            if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                html += @"<th>" + Translations.TranslationGet("HTML_DETECTED_01", "DE", "Overall Detected Avg.") + @"</th>";
            else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                html += @"<th>" + Translations.TranslationGet("HTML_DETECTED_02", "DE", "Tank<br/>Detected<br/>Avg.") + @"</th>";

            html += @"<th>" + Translations.TranslationGet("HTML_CONT_CAPPOINTS_00", "DE", "Capture<br/>Points") + @"</th>";
            if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                html += @"<th>" + Translations.TranslationGet("HTML_CONT_CAPPOINTS_01", "DE", "Overall Capture Points Avg.") + @"</th>";
            else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                html += @"<th>" + Translations.TranslationGet("HTML_CONT_CAPPOINTS_02", "DE", "Tank<br/>Capture<br/>Points Avg.") + @"</th>";

            html += @"<th>" + Translations.TranslationGet("HTML_CONT_DEFPOINTS_00", "DE", "Defense<br/>Points") + @"</th>";
            if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                html += @"<th>" + Translations.TranslationGet("HTML_CONT_DEFPOINTS_01", "DE", "Overall<br/>Defence<br/>Points Avg.") + @"</th>";
            else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                html += @"<th>" + Translations.TranslationGet("HTML_CONT_DEFPOINTS_02", "DE", "Tank<br/>Defence<br/>Points Avg.") + @"</th>";

            html += @"<th>" + Translations.TranslationGet("HTML_CONT_HITRATIO_00", "DE", "Hit<br/>Ratio") + @"</th>";
            if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                html += @"<th>" + Translations.TranslationGet("HTML_CONT_HITRATIO_01", "DE", "Overall<br/>Hit<br/>Ratio Avg.") + @"</th>";
            else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                html += @"<th>" + Translations.TranslationGet("HTML_CONT_HITRATIO_02", "DE", "Tank<br/>Hit<br/>Ratio Avg.") + @"</th>";

            html += @"<th>" + Translations.TranslationGet("HTML_CONT_EFFICIENCY", "DE", "Efficiency") + @"</th>";
            html += @"<th>" + Translations.TranslationGet("STR_BR_Caption", "DE", "Battle Rating") + @"</th>";
            html += @"<th>" + Translations.TranslationGet("STR_WN7_Caption", "DE", "WN7") + @"</th>";
            html += @"<th>" + Translations.TranslationGet("STR_WN8_Caption", "DE", "WN8") + @"</th>";

            html += @"        </tr>
								</thead>
								<tbody>";

            RecentBattles lb = new RecentBattles(playerName, _message);
            TankDescriptions tankDesc = new TankDescriptions(_message);
            CountryDescriptions countryDesc = new CountryDescriptions(_message);
            DossierManager dm = null;
            WOTStats wotS = null;
            //if (AppSettings.LastPlayedCompare != 0)
            //{
            PlayerListing pll = new PlayerListing(_message);
            Player pl = pll.GetPlayer(playerName.Replace("_", "*"));
            dm = new DossierManager(playerName, pl.WatchFile, pl.Monitor, _message, null);
            Dossier ds = new Dossier(dm.GetCurrentPlayerFile(), playerName, _message);
            wotS = ds.GetStats();
            //}

            int g = 1;
            int victories = 0;
            double TotalDamageDealt = 0;
            double TotalDamageAssistedRadio = 0;
            double TotalDamageAssistedTrack = 0;
            double TotalDamageReceived = 0;
            double TotalKills = 0;
            double TotalXPReceived = 0;
            double TotalSpotted = 0;
            double TotalCapturePoints = 0;
            double TotalDefensePoints = 0;
            double TotalHits = 0;
            double TotalShots = 0;
            double BattleCount = 0;
            double BattleTierCount = 0;
            double VictoryCount = 0;
            double LossCount = 0;
            double DrawCount = 0;
            double SurvivedYesCount = 0;
            double SurvivedNoCount = 0;
            //double globalAvgTier = 0;
            //double gobalWinPercentage = 0;
            double TotalRatingEff = 0;
            double TotalRatingBR = 0;
            double TotalRatingWN7 = 0;
            double TotalRatingWN8 = 0;
            List<RecentBattle> lastBattles;



            switch (UserSettings.GroupLPT)
            {
                case "0":
                    lastBattles = (from lbl in lb
                                   orderby lbl.Value.BattleTime descending
                                   group lbl by lbl.Value.BattleTime into lastBattleList
                                   let a = lastBattleList
                                   select new RecentBattle()
                                   {
                                       CountryID = a.Select(n => n.Value.CountryID).First(),
                                       Tier = a.Select(n => n.Value.Tier).First(),
                                       Battles = 1,
                                       BattleTime = a.Select(n => n.Value.BattleTime).First(),
                                       CapturePoints = a.Sum(n => n.Value.CapturePoints / n.Value.Battles),
                                       DamageDealt = a.Sum(n => n.Value.DamageDealt / n.Value.Battles),
                                       DamageAssistedRadio = a.Sum(n => n.Value.DamageAssistedRadio / n.Value.Battles),
                                       DamageAssistedTracks = a.Sum(n => n.Value.DamageAssistedTracks / n.Value.Battles),
                                       DamageReceived = a.Sum(n => n.Value.DamageReceived / n.Value.Battles),
                                       DefencePoints = a.Sum(n => n.Value.DefencePoints / n.Value.Battles),
                                       Hits = a.Sum(n => n.Value.Hits / n.Value.Battles),
                                       Kills = a.Sum(n => n.Value.Kills / n.Value.Battles),
                                       Shot = a.Sum(n => n.Value.Shot / n.Value.Battles),
                                       Spotted = a.Sum(n => n.Value.Spotted / n.Value.Battles),
                                       XPReceived = a.Sum(n => n.Value.XPReceived / n.Value.Battles),
                                       Survived = a.Select(n => n.Value.Survived).First(),
                                       TankID = a.Select(n => n.Value.TankID).First(),
                                       Victory = a.Select(n => n.Value.Victory).First(),
                                       VictoryCount = a.Sum(n => n.Value.Victory == 0 ? 1 : 0),
                                       DefeatCount = a.Sum(n => n.Value.Victory == 1 ? 1 : 0),
                                       DrawCount = a.Sum(n => n.Value.Victory == 2 ? 1 : 0),
                                       SurviveYesCount = a.Sum(n => n.Value.Survived == 1 ? 1 : 0),
                                       SurviveNoCount = a.Sum(n => n.Value.Survived == 0 ? 1 : 0),
                                       BattlesPerTier = a.Sum(n => tankDesc.Tier(n.Value.CountryID, n.Value.TankID) * 1),
                                       OriginalBattleCount = a.Select(n => n.Value.OriginalBattleCount).First(),
                                       FragList = a.Select(n => n.Value.FragList).First(),
                                       GlobalAvgTier = a.Select(n => n.Value.GlobalAvgTier).First(),
                                       GlobalWinPercentage = a.Select(n => n.Value.GlobalWinPercentage).First(),
                                       GlobalAvgDefencePoints = a.Select(n => n.Value.GlobalAvgDefencePoints).First(),
                                       Mileage = a.Sum(n => n.Value.Mileage / n.Value.Battles),
                                       RatingBR = a.Sum(n => n.Value.RatingBR / n.Value.Battles),
                                       RatingEff = a.Sum(n => n.Value.RatingEff / n.Value.Battles),
                                       RatingWN7 = a.Sum(n => n.Value.RatingWN7 / n.Value.Battles),
                                       RatingWN8 = a.Sum(n => n.Value.RatingWN8 / n.Value.Battles)
                                   }).ToList<RecentBattle>();
                    break;
                case "1":

                    lastBattles = (from lbl in lb

                                   group lbl by new { lbl.Value.CountryID, lbl.Value.TankID } into lastBattleList
                                   let a = lastBattleList
                                   select new RecentBattle()
                                   {
                                       CountryID = a.Select(n => n.Value.CountryID).First(),
                                       Tier = a.Select(n => n.Value.Tier).First(),
                                       Battles = a.Sum(n => n.Value.Battles > 1 ? 1 : 1),
                                       BattleTime = 0,
                                       CapturePoints = a.Sum(n => n.Value.CapturePoints),
                                       DamageDealt = a.Sum(n => n.Value.DamageDealt),
                                       DamageAssistedRadio = a.Sum(n => n.Value.DamageAssistedRadio),
                                       DamageAssistedTracks = a.Sum(n => n.Value.DamageAssistedTracks),
                                       DamageReceived = a.Sum(n => n.Value.DamageReceived),
                                       DefencePoints = a.Sum(n => n.Value.DefencePoints),
                                       Hits = a.Sum(n => n.Value.Hits),
                                       Kills = a.Sum(n => n.Value.Kills),
                                       Shot = a.Sum(n => n.Value.Shot),
                                       Spotted = a.Sum(n => n.Value.Spotted),
                                       XPReceived = a.Sum(n => n.Value.XPReceived),
                                       Survived = a.Select(n => n.Value.Survived).First(),
                                       TankID = a.Select(n => n.Value.TankID).First(),
                                       VictoryCount = a.Sum(n => n.Value.Victory == 0 ? 1 : 0),
                                       DefeatCount = a.Sum(n => n.Value.Victory == 1 ? 1 : 0),
                                       DrawCount = a.Sum(n => n.Value.Victory == 2 ? 1 : 0),
                                       SurviveYesCount = a.Sum(n => n.Value.Survived == 1 ? 1 : 0),
                                       SurviveNoCount = a.Sum(n => n.Value.Survived == 0 ? 1 : 0),
                                       BattlesPerTier = a.Sum(n => tankDesc.Tier(n.Value.CountryID, n.Value.TankID) * n.Value.Battles),
                                       OriginalBattleCount = a.Select(n => n.Value.OriginalBattleCount).First(),
                                       FragList = a.Select(n => n.Value.FragList).First(),
                                       GlobalAvgTier = a.Select(n => n.Value.GlobalAvgTier).First(),
                                       GlobalWinPercentage = a.Select(n => n.Value.GlobalWinPercentage).First(),
                                       GlobalAvgDefencePoints = a.Select(n => n.Value.GlobalAvgDefencePoints).First(),
                                       RatingBR = a.Sum(n => n.Value.RatingBR),
                                       RatingEff = a.Sum(n => n.Value.RatingEff),
                                       RatingWN7 = a.Sum(n => n.Value.RatingWN7),
                                       RatingWN8 = a.Sum(n => n.Value.RatingWN8)
                                   }).ToList<RecentBattle>();


                    break;
                case "2":
                    lastBattles = (from lbl in lb
                                   orderby tankDesc.Tier(lbl.Value.CountryID, lbl.Value.TankID) descending
                                   group lbl by tankDesc.Tier(lbl.Value.CountryID, lbl.Value.TankID) into lastBattleList
                                   let a = lastBattleList
                                   select new RecentBattle()
                                   {
                                       CountryID = a.Select(n => n.Value.CountryID).First(),
                                       Tier = a.Select(n => n.Value.Tier).First(),
                                       Battles = a.Sum(n => n.Value.Battles),
                                       BattleTime = 0,
                                       CapturePoints = a.Sum(n => n.Value.CapturePoints),
                                       DamageDealt = a.Sum(n => n.Value.DamageDealt),
                                       DamageAssistedRadio = a.Sum(n => n.Value.DamageAssistedRadio),
                                       DamageAssistedTracks = a.Sum(n => n.Value.DamageAssistedTracks),
                                       DamageReceived = a.Sum(n => n.Value.DamageReceived),
                                       DefencePoints = a.Sum(n => n.Value.DefencePoints),
                                       Hits = a.Sum(n => n.Value.Hits),
                                       Kills = a.Sum(n => n.Value.Kills),
                                       Shot = a.Sum(n => n.Value.Shot),
                                       Spotted = a.Sum(n => n.Value.Spotted),
                                       XPReceived = a.Sum(n => n.Value.XPReceived),
                                       Survived = a.Select(n => n.Value.Survived).First(),
                                       TankID = a.Select(n => n.Value.TankID).First(),
                                       VictoryCount = a.Sum(n => n.Value.Victory == 0 ? 1 : 0),
                                       DefeatCount = a.Sum(n => n.Value.Victory == 1 ? 1 : 0),
                                       DrawCount = a.Sum(n => n.Value.Victory == 2 ? 1 : 0),
                                       SurviveYesCount = a.Sum(n => n.Value.Survived == 1 ? 1 : 0),
                                       SurviveNoCount = a.Sum(n => n.Value.Survived == 0 ? 1 : 0),
                                       BattlesPerTier = a.Sum(n => tankDesc.Tier(n.Value.CountryID, n.Value.TankID) * n.Value.Battles),
                                       OriginalBattleCount = a.Select(n => n.Value.OriginalBattleCount).First(),
                                       FragList = a.Select(n => n.Value.FragList).First(),
                                       GlobalAvgTier = a.Select(n => n.Value.GlobalAvgTier).First(),
                                       GlobalWinPercentage = a.Select(n => n.Value.GlobalWinPercentage).First(),
                                       GlobalAvgDefencePoints = a.Select(n => n.Value.GlobalAvgDefencePoints).First(),
                                       RatingBR = a.Sum(n => n.Value.RatingBR),
                                       RatingEff = a.Sum(n => n.Value.RatingEff),
                                       RatingWN7 = a.Sum(n => n.Value.RatingWN7),
                                       RatingWN8 = a.Sum(n => n.Value.RatingWN8)
                                   }).ToList<RecentBattle>();
                    break;
                case "3":
                    lastBattles = (from lbl in lb

                                   group lbl by lbl.Value.CountryID into lastBattleList
                                   let a = lastBattleList
                                   select new RecentBattle()
                                   {
                                       CountryID = a.Select(n => n.Value.CountryID).First(),
                                       TankID = a.Select(n => n.Value.TankID).First(),
                                       Tier = a.Select(n => n.Value.Tier).First(),
                                       Battles = a.Sum(n => n.Value.Battles),
                                       BattleTime = 0,
                                       CapturePoints = a.Sum(n => n.Value.CapturePoints),
                                       DamageDealt = a.Sum(n => n.Value.DamageDealt),
                                       DamageAssistedRadio = a.Sum(n => n.Value.DamageAssistedRadio),
                                       DamageAssistedTracks = a.Sum(n => n.Value.DamageAssistedTracks),
                                       DamageReceived = a.Sum(n => n.Value.DamageReceived),
                                       DefencePoints = a.Sum(n => n.Value.DefencePoints),
                                       Hits = a.Sum(n => n.Value.Hits),
                                       Kills = a.Sum(n => n.Value.Kills),
                                       Shot = a.Sum(n => n.Value.Shot),
                                       Spotted = a.Sum(n => n.Value.Spotted),
                                       XPReceived = a.Sum(n => n.Value.XPReceived),
                                       Survived = a.Select(n => n.Value.Survived).First(),
                                       VictoryCount = a.Sum(n => n.Value.Victory == 0 ? 1 : 0),
                                       DefeatCount = a.Sum(n => n.Value.Victory == 1 ? 1 : 0),
                                       DrawCount = a.Sum(n => n.Value.Victory == 2 ? 1 : 0),
                                       SurviveYesCount = a.Sum(n => n.Value.Survived == 1 ? 1 : 0),
                                       SurviveNoCount = a.Sum(n => n.Value.Survived == 0 ? 1 : 0),
                                       BattlesPerTier = a.Sum(n => tankDesc.Tier(n.Value.CountryID, n.Value.TankID) * n.Value.Battles),
                                       OriginalBattleCount = a.Select(n => n.Value.OriginalBattleCount).First(),
                                       FragList = a.Select(n => n.Value.FragList).First(),
                                       GlobalAvgTier = a.Select(n => n.Value.GlobalAvgTier).First(),
                                       GlobalWinPercentage = a.Select(n => n.Value.GlobalWinPercentage).First(),
                                       GlobalAvgDefencePoints = a.Select(n => n.Value.GlobalAvgDefencePoints).First(),
                                       RatingBR = a.Sum(n => n.Value.RatingBR),
                                       RatingEff = a.Sum(n => n.Value.RatingEff),
                                       RatingWN7 = a.Sum(n => n.Value.RatingWN7),
                                       RatingWN8 = a.Sum(n => n.Value.RatingWN8)
                                   }).ToList<RecentBattle>();
                    break;
                default:
                    lastBattles = (from lbl in lb
                                   orderby lbl.Value.BattleTime descending
                                   group lbl by lbl.Value.BattleTime into lastBattleList
                                   let a = lastBattleList
                                   select new RecentBattle()
                                   {
                                       CountryID = a.Select(n => n.Value.CountryID).First(),
                                       Tier = a.Select(n => n.Value.Tier).First(),
                                       Battles = 1,
                                       BattleTime = a.Select(n => n.Value.BattleTime).First(),
                                       CapturePoints = a.Sum(n => n.Value.CapturePoints / n.Value.Battles),
                                       DamageDealt = a.Sum(n => n.Value.DamageDealt / n.Value.Battles),
                                       DamageAssistedRadio = a.Sum(n => n.Value.DamageAssistedRadio / n.Value.Battles),
                                       DamageAssistedTracks = a.Sum(n => n.Value.DamageAssistedTracks / n.Value.Battles),
                                       DamageReceived = a.Sum(n => n.Value.DamageReceived / n.Value.Battles),
                                       DefencePoints = a.Sum(n => n.Value.DefencePoints / n.Value.Battles),
                                       Hits = a.Sum(n => n.Value.Hits / n.Value.Battles),
                                       Kills = a.Sum(n => n.Value.Kills / n.Value.Battles),
                                       Shot = a.Sum(n => n.Value.Shot / n.Value.Battles),
                                       Spotted = a.Sum(n => n.Value.Spotted / n.Value.Battles),
                                       Mileage = a.Sum(n => n.Value.Mileage / n.Value.Battles),
                                       XPReceived = a.Sum(n => n.Value.XPReceived / n.Value.Battles),
                                       Survived = a.Select(n => n.Value.Survived).First(),
                                       TankID = a.Select(n => n.Value.TankID).First(),
                                       Victory = a.Select(n => n.Value.Victory).First(),
                                       VictoryCount = a.Sum(n => n.Value.Victory == 0 ? 1 : 0),
                                       DefeatCount = a.Sum(n => n.Value.Victory == 1 ? 1 : 0),
                                       DrawCount = a.Sum(n => n.Value.Victory == 2 ? 1 : 0),
                                       SurviveYesCount = a.Sum(n => n.Value.Survived == 1 ? 1 : 0),
                                       SurviveNoCount = a.Sum(n => n.Value.Survived == 0 ? 1 : 0),
                                       BattlesPerTier = a.Sum(n => tankDesc.Tier(n.Value.CountryID, n.Value.TankID) * 1),
                                       OriginalBattleCount = a.Select(n => n.Value.OriginalBattleCount).First(),
                                       FragList = a.Select(n => n.Value.FragList).First(),
                                       GlobalAvgTier = a.Select(n => n.Value.GlobalAvgTier).First(),
                                       GlobalWinPercentage = a.Select(n => n.Value.GlobalWinPercentage).First(),
                                       GlobalAvgDefencePoints = a.Select(n => n.Value.GlobalAvgDefencePoints).First(),
                                       RatingBR = a.Sum(n => n.Value.RatingBR / n.Value.Battles),
                                       RatingEff = a.Sum(n => n.Value.RatingEff / n.Value.Battles),
                                       RatingWN7 = a.Sum(n => n.Value.RatingWN7 / n.Value.Battles),
                                       RatingWN8 = a.Sum(n => n.Value.RatingWN8 / n.Value.Battles)
                                   }).ToList<RecentBattle>();
                    break;
            }


            foreach (RecentBattle item in lastBattles)
            {

                if (item.Victory == 0)
                    victories++;

                //if (item.DamageDealt == 304)
                //{
                //    int gg = 200;
                //}


                RatingStructure ratingStruct = new RatingStructure();
                ratingStruct.WN8ExpectedTankList = WN8ExpectedTankList;
                ratingStruct.countryID = item.CountryID;
                ratingStruct.tankID = item.TankID;
                ratingStruct.tier = item.GlobalAvgTier;
                ratingStruct.globalTier = item.GlobalAvgTier;

                ratingStruct.singleTank = true;

                ratingStruct.battlesCount = Convert.ToInt32(item.Battles);
                ratingStruct.battlesCount8_8 = Convert.ToInt32(item.Battles88);
                ratingStruct.capturePoints = item.CapturePoints;
                ratingStruct.defencePoints = item.DefencePoints;

                ratingStruct.damageAssistedRadio = item.DamageAssistedRadio;
                ratingStruct.damageAssistedTracks = item.DamageAssistedTracks;
                ratingStruct.damageDealt = item.DamageDealt;
                ratingStruct.frags = item.Kills;
                ratingStruct.spotted = item.Spotted;

                ratingStruct.wins = item.VictoryCount;
                ratingStruct.gWinRate = item.GlobalWinPercentage;



                switch (UserSettings.GroupLPT)
                {
                    case "0":
                        html += @"<tr>
							<td align=center>" + g + @"</td>
							<td sorttable_customkey='" + tankDesc.Tier(item.CountryID, item.TankID) + @"' class='" + CountryFlag(item.CountryID) + @" td-armory-icon'>
							   <div class='wrapper'>
								  <span class='level'>
									<a class='b-gray-text'>" + GetRoman(tankDesc.Tier(item.CountryID, item.TankID)) + @"</a>
								  </span>
								  <a>" + TankImage(item.CountryID, item.TankID, tankDesc.Description(item.CountryID, item.TankID)) + @"</a>
							   </div>
						   </td>
						   <td title='Date: " + item.BattleTime_Friendly.ToString(UserSettings.DateFormat + " " + (UserSettings.TimeStamp == true ? UserSettings.TimeFormat : "")) + "&#13;Based on " + item.OriginalBattleCount + @" Battle/s' ><a href='h' onclick='window.external.Redirect(""" + item.CountryID + "_" + item.TankID + @""")'>" + tankDesc.Description(item.CountryID, item.TankID) + @"</a></td>";
                        break;
                    case "1":
                        html += @"<tr>
							<td align=center>" + g + @"</td>
							<td sorttable_customkey='" + tankDesc.Tier(item.CountryID, item.TankID) + @"' class='" + CountryFlag(item.CountryID) + @" td-armory-icon'>
							   <div class='wrapper'>
								  <span class='level'>
									<a class='b-gray-text'>" + GetRoman(tankDesc.Tier(item.CountryID, item.TankID)) + @"</a>
								  </span>
								  <a>" + TankImage(item.CountryID, item.TankID, tankDesc.Description(item.CountryID, item.TankID)) + @"</a>
							   </div>
						   </td>
						   <td><a href='h' onclick='window.external.Redirect(""" + item.CountryID + "_" + item.TankID + @""")'>" + tankDesc.Description(item.CountryID, item.TankID) + @"</a></td><td align=center>" + item.Battles + @"</td>";
                        break;
                    case "2":
                        html += @"<tr>
							<td align=center>" + g + @"</td>
							<td align=center>
									<a sorttable_customkey='" + tankDesc.Tier(item.CountryID, item.TankID) + @"' class='b-gray-text'>" + GetRoman(tankDesc.Tier(item.CountryID, item.TankID)) + @"</a>
								
						   </td><td align=center>" + item.Battles + @"</td>";
                        break;
                    case "3":
                        html += @"<tr>
							<td align=center>" + g + @"</td>
							 <td class='" + CountryFlag(item.CountryID) + @" td-armory-icon'>
												<div class='wrapper'>
													<span class='MiddleCenter'><a class='b-gray-text'>" + countryDesc.Description(item.CountryID) + @"</a></span>
												</div>
											 </td><td align=center>" + item.Battles + @"</td>";
                        break;
                    default:
                        html += @"<tr>
							<td align=center>" + g + @"</td>
							<td sorttable_customkey='" + tankDesc.Tier(item.CountryID, item.TankID) + @"' class='" + CountryFlag(item.CountryID) + @" td-armory-icon'>
							   <div class='wrapper'>
								  <span class='level'>
									<a class='b-gray-text'>" + GetRoman(tankDesc.Tier(item.CountryID, item.TankID)) + @"</a>
								  </span>
								  <a>" + TankImage(item.CountryID, item.TankID, tankDesc.Description(item.CountryID, item.TankID)) + @"</a>
							   </div>
						   </td>
						   <td><a href='h' onclick='window.external.Redirect(""" + item.CountryID + "_" + item.TankID + @""")'>" + tankDesc.Description(item.CountryID, item.TankID) + @"</a></td>";
                        break;
                }

                if (UserSettings.GroupLPT == "0")
                {
                    html += @" <td align=center>" + (item.Victory == 0 ? @"<font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + Translations.TranslationGet("HTML_CONT_VICTORY", "DE", "Victory") + @"</font>" : (item.Victory == 1 ? @"<font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNegative)) + "'>" + Translations.TranslationGet("HTML_CONT_DEFEAT", "DE", "Defeat") + @"</font>" : @"<font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNeutral)) + "'>" + Translations.TranslationGet("HTML_CONT_DRAW", "DE", "Draw") + @"</font>")) + @"</td>";
                    html += @"<td align=center>" + (item.Survived == 1 ? @"<font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + Translations.TranslationGet("NAT_TRUE", "DE", "Yes") + @"</font>" : @"<font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNegative)) + "'>" + Translations.TranslationGet("NAT_FALSE", "DE", "No") + @"</font>") + @"</td>";
                }
                else
                {
                    html += @" <td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + WOTHelper.FormatNumberToString((item.VictoryCount / item.Battles) * 100, 2) + @"%</font></td><td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNegative)) + "'>" + WOTHelper.FormatNumberToString((item.DefeatCount / item.Battles) * 100, 2) + @"%</font></td><td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNeutral)) + "'>" + WOTHelper.FormatNumberToString((item.DrawCount / item.Battles) * 100, 2) + @"%</font></td>";
                    html += @"<td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + WOTHelper.FormatNumberToString((item.SurviveYesCount / item.Battles) * 100, 2) + @"%</font></td>";
                }




                html += @"<td align=right  sorttable_customkey='" + Math.Round(item.DamageDealt, 2) * 100 + @"' >" + WOTHelper.FormatNumberToString((item.DamageDealt), 0) + @"</td>";
                TotalDamageDealt += (item.DamageDealt);


                if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                    html += String.Format(@"<td align=right  sorttable_customkey='" + Math.Round((item.DamageDealt) - (wotS.AverageDamageDealtPerBattle), 2) * 100 + @"' >{0}</td>", GetDelta((item.DamageDealt) - (wotS.AverageDamageDealtPerBattle), "", 0));
                else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.DamageDealt) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageDamageDealt)), 2) * 100 + @"'>{0}</td>", GetDelta((item.DamageDealt) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageDamageDealt)), "", 0));


                html += @"<td align=right  sorttable_customkey='" + Math.Round(item.DamageAssistedRadio, 2) * 100 + @"' >" + WOTHelper.FormatNumberToString((item.DamageAssistedRadio), 0) + @"</td>";
                TotalDamageAssistedRadio += (item.DamageAssistedRadio);
                if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                    html += String.Format(@"<td align=right  sorttable_customkey='" + Math.Round((item.DamageAssistedRadio) - (wotS.AverageDamageAssistedRadioPerBattle), 2) * 100 + @"' >{0}</td>", GetDelta((item.DamageAssistedRadio) - (wotS.AverageDamageAssistedRadioPerBattle), "", 0));
                else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.DamageAssistedRadio) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageDamageAssistedRadio)), 2) * 100 + @"'>{0}</td>", GetDelta((item.DamageAssistedRadio) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageDamageAssistedRadio)), "", 0));

                html += @"<td align=right  sorttable_customkey='" + Math.Round(item.DamageAssistedTracks, 2) * 100 + @"' >" + WOTHelper.FormatNumberToString((item.DamageAssistedTracks), 0) + @"</td>";
                TotalDamageAssistedTrack += (item.DamageAssistedTracks);
                if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                    html += String.Format(@"<td align=right  sorttable_customkey='" + Math.Round((item.DamageAssistedTracks) - (wotS.AverageDamageAssistedTracksPerBattle), 2) * 100 + @"' >{0}</td>", GetDelta((item.DamageAssistedTracks) - (wotS.AverageDamageAssistedTracksPerBattle), "", 0));
                else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.DamageAssistedTracks) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageDamageAssistedTracks)), 2) * 100 + @"'>{0}</td>", GetDelta((item.DamageAssistedTracks) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageDamageAssistedTracks)), "", 0));



                html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round(item.DamageReceived, 2) * 100 + @"' >{0}</td>", WOTHelper.FormatNumberToString(item.DamageReceived, 0));
                TotalDamageReceived += item.DamageReceived;
                if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.DamageReceived) - (wotS.AverageDamageReceivedPerBattle), 2) * 100 + @"'>{0}</td>", GetDelta((item.DamageReceived) - (wotS.AverageDamageReceivedPerBattle), "", 0, true));
                else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.DamageReceived) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageDamageReceived)), 2) * 100 + @"'>{0}</td>", GetDelta((item.DamageReceived) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageDamageReceived)), "", 0, true));

                html += String.Format(@"<td " + (UserSettings.GroupLPT == "0" ? CreateRecentBattleKillList(item.FragList, tankDesc) : "") + "align=center sorttable_customkey='" + Math.Round(item.Kills, 2) * 100 + @"' >{0}</td>", WOTHelper.FormatNumberToString(item.Kills, 0));
                TotalKills += item.Kills;
                if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.Kills) - (wotS.AverageFrags), 2) * 100 + @"'>{0}</td>", GetDelta((item.Kills) - (wotS.AverageFrags), "", 0));
                else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.Kills) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageFrags)), 2) * 100 + @"'>{0}</td>", GetDelta((item.Kills) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageFrags)), "", 0));

                html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round(item.XPReceived, 2) * 100 + @"' >{0}</td>", WOTHelper.FormatNumberToString(item.XPReceived, 0));
                TotalXPReceived += item.XPReceived;
                if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.XPReceived) - (wotS.AverageExperiencePerBattle), 2) * 100 + @"'>{0}</td>", GetDelta((item.XPReceived) - (wotS.AverageExperiencePerBattle), "", 0));
                else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.XPReceived) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageXP)), 2) * 100 + @"'>{0}</td>", GetDelta((item.XPReceived) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageXP)), "", 0));

                html += String.Format(@"<td align=center sorttable_customkey='" + Math.Round(item.Spotted, 2) * 100 + @"' >{0}</td>", WOTHelper.FormatNumberToString(item.Spotted, 0));
                TotalSpotted += item.Spotted;
                if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.Spotted) - (wotS.AverageSpotted), 2) * 100 + @"'>{0}</td>", GetDelta((item.Spotted) - (wotS.AverageSpotted), "", 0));
                else if ((UserSettings.LastPlayedCompare == 2) && (UserSettings.GroupLPT == "0"))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.Spotted) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageSpotted)), 2) * 100 + @"'>{0}</td>", GetDelta((item.Spotted) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageSpotted)), "", 0));

                html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round(item.CapturePoints, 2) * 100 + @"' >{0}</td>", WOTHelper.FormatNumberToString(item.CapturePoints, 0));
                TotalCapturePoints += item.CapturePoints;
                if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.CapturePoints) - (wotS.AverageCapturePoints), 2) * 100 + @"'>{0}</td>", GetDelta((item.CapturePoints) - (wotS.AverageCapturePoints), "", 0));
                else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.CapturePoints) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageCapturePoints)), 2) * 100 + @"'>{0}</td>", GetDelta((item.CapturePoints) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageCapturePoints)), "", 0));

                html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round(item.DefencePoints, 2) * 100 + @"' >{0}</td>", WOTHelper.FormatNumberToString(item.DefencePoints, 0));
                TotalDefensePoints += item.DefencePoints;
                if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.DefencePoints) - (wotS.AverageDefencePoints), 2) * 100 + @"'>{0}</td>", GetDelta((item.DefencePoints) - (wotS.AverageDefencePoints), "", 0));
                else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((item.DefencePoints) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageDefencePoints)), 2) * 100 + @"'>{0}</td>", GetDelta((item.DefencePoints) - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.AverageDefencePoints)), "", 0));


                html += String.Format(@"<td id='accCol'  OnMouseOver='ShowShotsHits(""accCol"", ""{3}"", {1}, ""{4}"", {2})' OnMouseOut='toolTip()' align=right sorttable_customkey='" + Math.Round(double.IsNaN((item.Hits) / (item.Shot)) ? 0 : (item.Hits) / (item.Shot), 2) * 100 + @"' >{0}%</td>", WOTHelper.FormatNumberToString((item.Hits) / (item.Shot) * 100, 2), item.Hits, item.Shot, Translations.TranslationGet("HTML_HITS", "de", "Hits"), Translations.TranslationGet("HTML_SHOTS", "de", "Shots"));
                TotalHits += (item.Hits);
                TotalShots += (item.Shot);
                if (UserSettings.LastPlayedCompare == 1 && (UserSettings.GroupLPT == "0"))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((double.IsNaN((item.Hits) / (item.Shot)) ? 0 : (item.Hits) / (item.Shot)) * 100 - (wotS.HitRatio), 2) * 100 + @"'>{0}</td>", GetDelta((double.IsNaN((item.Hits) / (item.Shot)) ? 0 : (item.Hits) / (item.Shot)) * 100 - (wotS.HitRatio), "%", 2));
                else if ((UserSettings.LastPlayedCompare == 2 && (UserSettings.GroupLPT == "0")))
                    html += String.Format(@"<td align=right sorttable_customkey='" + Math.Round((double.IsNaN((item.Hits) / (item.Shot)) ? 0 : (item.Hits) / (item.Shot)) * 100 - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.Accuracy)), 2) * 100 + @"'>{0}</td>", GetDelta((double.IsNaN((item.Hits) / (item.Shot)) ? 0 : (item.Hits) / (item.Shot)) * 100 - (wotS.tanks.Where(x => x.CountryID == item.CountryID && x.TankID == item.TankID).Sum(y => y.Data.Accuracy)), "%", 2));

                double RatingEff = WOTStatistics.Core.Ratings.GetRatingEff(ratingStruct).Value;
                TotalRatingEff += RatingEff;
                html += String.Format(String.Format(@"<td id='effCol'" +  (UserSettings.GroupLPT == "0" ? @" OnMouseOver='ShowEffValues(""{0}"")' OnMouseOut='toolTip()'" : "") + " align=right sorttable_customkey='{1}' >{{0}}</td>",
                    WOTStatistics.Core.Ratings.GetRatingEffToolTip(ratingStruct).Replace(',', '.').Replace("'", " "),
                    Math.Round(RatingEff, 2) * 100),
                    WOTHelper.FormatNumberToString(RatingEff, 0));

                double RatingBR = WOTStatistics.Core.Ratings.GetRatingBR(ratingStruct).Value;
                TotalRatingBR += RatingBR;
                html += String.Format(String.Format(@"<td id='effCol'" + (UserSettings.GroupLPT == "0" ? @" OnMouseOver='ShowEffValues(""{0}"")' OnMouseOut='toolTip()'" : "") + " align=right sorttable_customkey='{1}' >{{0}}</td>",
                    WOTStatistics.Core.Ratings.GetRatingBRToolTip(ratingStruct).Replace(',', '.').Replace("'", " "),
                    Math.Round(RatingBR, 2) * 100),
                    WOTHelper.FormatNumberToString(RatingBR, 0));

                double RatingWN7 = WOTStatistics.Core.Ratings.GetRatingWN7(ratingStruct).Value;
                TotalRatingWN7 += RatingWN7;
                html += String.Format(String.Format(@"<td id='effCol'" + (UserSettings.GroupLPT == "0" ? @" OnMouseOver='ShowEffValues(""{0}"")' OnMouseOut='toolTip()'" : "") + " align=right sorttable_customkey='{1}' >{{0}}</td>",
                    WOTStatistics.Core.Ratings.GetRatingWN7ToolTip(ratingStruct).Replace(',', '.').Replace("'", " "),
                    Math.Round(RatingWN7, 2) * 100),
                    WOTHelper.FormatNumberToString(RatingWN7, 0));

                WOTStatistics.Core.Ratings.RatingStorage WN8 = WOTStatistics.Core.Ratings.GetRatingWN8(ratingStruct);
                TotalRatingWN8 += WN8.Value;
                html += String.Format(String.Format(@"<td id='effCol'" + (UserSettings.GroupLPT == "0" ? @" OnMouseOver='ShowWN8Values(""{0}"")' OnMouseOut='toolTip()'" : "") + " align=right sorttable_customkey='{1}' >{{0}}</td>",
                    WOTStatistics.Core.Ratings.GetRatingWN8ToolTip(WN8).Replace(',', '.').Replace("'", " "),
                    Math.Round(WN8.Value, 2) * 100),
                    WOTHelper.FormatNumberToString(WN8.Value, 0));

                
                
                html += @"</tr>";
                BattleCount += item.Battles;
                VictoryCount += item.VictoryCount;
                LossCount += item.DefeatCount;
                DrawCount += item.DrawCount;
                SurvivedYesCount += item.SurviveYesCount;
                SurvivedNoCount += item.SurviveNoCount;
                BattleTierCount += item.BattlesPerTier;
                g++;

            }
            html += @"</tbody>
			<tfoot>
				<tr>";

            switch (UserSettings.GroupLPT)
            {
                case "0":
                    html += @"<td colspan=2 align=center>" + Translations.TranslationGet("HTML_TOTAL", "DE", "Total") + @"</td>
								  <td>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + ": " + WOTHelper.FormatNumberToString(BattleTierCount / BattleCount,2) + @"</td>
								  <td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + Translations.TranslationGet("HTML_CONT_VICTORY", "DE", "Victory") + @": (" + VictoryCount + ") " + WOTHelper.FormatNumberToString((VictoryCount / BattleCount) * 100, 2) + @"%</font><br><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNegative)) + "'>" + Translations.TranslationGet("HTML_CONT_DEFEAT", "DE", "Defeat") + @": (" + LossCount + ") " + WOTHelper.FormatNumberToString((LossCount / BattleCount) * 100, 2) + @"%</font><br><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNeutral)) + "'>" + Translations.TranslationGet("HTML_CONT_DRAW", "DE", "Draw") + @": (" + DrawCount + ") " + WOTHelper.FormatNumberToString((DrawCount / BattleCount) * 100, 2) + @"%</font></td>
								  <td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + Translations.TranslationGet("NAT_TRUE", "DE", "Yes") + @": " + WOTHelper.FormatNumberToString((SurvivedYesCount / BattleCount) * 100, 2) + @"%</font><br><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNegative)) + "'>" + Translations.TranslationGet("NAT_FALSE", "DE", "No") + @": " + WOTHelper.FormatNumberToString((SurvivedNoCount / BattleCount) * 100, 2) + @"%" + @"</td>";
                   // g--; //we need to remove one to compensate for loop being done earlier.
                    break;
                case "1":
                    html += @"<td colspan=2 align=center>" + Translations.TranslationGet("HTML_TOTAL", "DE", "Total") + @"</td>
								   <td>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + ": " + WOTHelper.FormatNumberToString(BattleTierCount / BattleCount, 2) + @"</td>
								  <td colspan=1 align=center>" + BattleCount + @"</td>
								  <td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + WOTHelper.FormatNumberToString((VictoryCount / BattleCount) * 100, 2) + @"%</font></td><td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNegative)) + "'>" + WOTHelper.FormatNumberToString((LossCount / BattleCount) * 100, 2) + @"%</font></td><td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNeutral)) + "'>" + WOTHelper.FormatNumberToString((DrawCount / BattleCount) * 100, 2) + @"%</font></td>
								  <td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + WOTHelper.FormatNumberToString((SurvivedYesCount / BattleCount) * 100, 2) + @"%</font></td>";
                   // g = Convert.ToInt16(BattleCount);
                    break;
                case "2":
                case "3":
                    html += @"<td colspan=2 align=center>" + Translations.TranslationGet("HTML_TOTAL", "DE", "Total") + @"</td>

								  <td colspan=1 align=center>" + BattleCount + @"</td>
								  <td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + WOTHelper.FormatNumberToString((VictoryCount / BattleCount) * 100, 2) + @"%</font></td><td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNegative)) + "'>" + WOTHelper.FormatNumberToString((LossCount / BattleCount) * 100, 2) + @"%</font></td><td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNeutral)) + "'>" + WOTHelper.FormatNumberToString((DrawCount / BattleCount) * 100, 2) + @"%</font></td>
								  <td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + WOTHelper.FormatNumberToString((SurvivedYesCount / BattleCount) * 100, 2) + @"%</font></td>";
                   // g = Convert.ToInt16(BattleCount);
                    break;
                default:
                    html += @"<td colspan=2 align=center>" + Translations.TranslationGet("HTML_TOTAL", "DE", "Total") + @"</td>
								<td>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + ": " + WOTHelper.FormatNumberToString(BattleTierCount / BattleCount, 2) + @"</td>
								  <td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + Translations.TranslationGet("HTML_CONT_VICTORY", "DE", "Victory") + @": (" + VictoryCount + ") " + WOTHelper.FormatNumberToString((VictoryCount / BattleCount) * 100, 2) + @"%</font><br><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNegative)) + "'>" + Translations.TranslationGet("HTML_CONT_DEFEAT", "DE", "Defeat") + @": (" + LossCount + ") " + WOTHelper.FormatNumberToString((LossCount / BattleCount) * 100, 2) + @"%</font><br><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNeutral)) + "'>" + Translations.TranslationGet("HTML_CONT_DRAW", "DE", "Draw") + @": (" + DrawCount + ") " + WOTHelper.FormatNumberToString((DrawCount / BattleCount) * 100, 2) + @"%</font></td>
								  <td align=center><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)) + "'>" + Translations.TranslationGet("NAT_TRUE", "DE", "Yes") + @":" + WOTHelper.FormatNumberToString((SurvivedYesCount / BattleCount) * 100, 2) + @"%</font><br><font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorNegative)) + "'>" + Translations.TranslationGet("NAT_FALSE", "DE", "No") + @": " + WOTHelper.FormatNumberToString((SurvivedNoCount / BattleCount) * 100, 2) + @"%" + @"</td>";
                   // g = Convert.ToInt16(BattleCount);
                    break;
            }

            g = Convert.ToInt16(BattleCount);
            //g--; //we need to remove one to compensate for loop being done earlier.
            html += @"<td align=right>" + WOTHelper.FormatNumberToString(TotalDamageDealt, 0) + @"<br/> Avg: " + WOTHelper.FormatNumberToString(TotalDamageDealt / g, 2) + @"</td>";
            if (UserSettings.LastPlayedCompare != 0 && (UserSettings.GroupLPT == "0"))
                html += "<td/>";
            
            html += @"<td align=right>" + WOTHelper.FormatNumberToString(TotalDamageAssistedRadio, 0) + @"<br/> Avg: " + WOTHelper.FormatNumberToString(TotalDamageAssistedRadio / g, 2) + @"</td>";
            if (UserSettings.LastPlayedCompare != 0 && (UserSettings.GroupLPT == "0"))
                html += "<td/>";
            
            html += @"<td align=right>" + WOTHelper.FormatNumberToString(TotalDamageAssistedTrack, 0) + @"<br/> Avg: " + WOTHelper.FormatNumberToString(TotalDamageAssistedTrack / g, 2) + @"</td>";
            if (UserSettings.LastPlayedCompare != 0 && (UserSettings.GroupLPT == "0"))
                html += "<td/>";

            html += "<td align=right>" + WOTHelper.FormatNumberToString(TotalDamageReceived, 0) + @"<br/> Avg: " + WOTHelper.FormatNumberToString(TotalDamageReceived / g, 2) + @"</td>";

            if (UserSettings.LastPlayedCompare != 0 && (UserSettings.GroupLPT == "0"))
                html += "<td/>";
            html += "<td align=center>" + WOTHelper.FormatNumberToString(TotalKills, 0) + @"<br/> Avg: " + WOTHelper.FormatNumberToString(TotalKills / g, 2) + @"</td>";

            if (UserSettings.LastPlayedCompare != 0 && (UserSettings.GroupLPT == "0"))
                html += "<td/>";
            html += "<td align=right>" + WOTHelper.FormatNumberToString(TotalXPReceived, 0) + @"<br/> Avg: " + WOTHelper.FormatNumberToString(TotalXPReceived / g, 2) + @"</td>";

            if (UserSettings.LastPlayedCompare != 0 && (UserSettings.GroupLPT == "0"))
                html += "<td/>";
            html += "<td align=center>" + WOTHelper.FormatNumberToString(TotalSpotted, 0) + @"<br/> Avg: " + WOTHelper.FormatNumberToString(TotalSpotted / g, 2) + @"</td>";

            if (UserSettings.LastPlayedCompare != 0 && (UserSettings.GroupLPT == "0"))
                html += "<td/>";
            html += "<td align=right>" + WOTHelper.FormatNumberToString(TotalCapturePoints, 0) + @"<br/> Avg: " + WOTHelper.FormatNumberToString(TotalCapturePoints / g, 2) + @"</td>";

            if (UserSettings.LastPlayedCompare != 0 && (UserSettings.GroupLPT == "0"))
                html += "<td/>";
            html += "<td align=right>" + WOTHelper.FormatNumberToString(TotalDefensePoints, 0) + @"<br/> Avg: " + WOTHelper.FormatNumberToString(TotalDefensePoints / g, 2) + @"</td>";

            if (UserSettings.LastPlayedCompare != 0 && (UserSettings.GroupLPT == "0"))
                html += "<td/>";
            html += "<td align=right>" + WOTHelper.FormatNumberToString(((TotalHits / TotalShots) * 100), 2) + @"%</td>";

            if (UserSettings.LastPlayedCompare != 0 && (UserSettings.GroupLPT == "0"))
                html += "<td/>";

            //double Totalefficiency = (TotalKills * (350.0 - (Math.Round(BattleTierCount / BattleCount, 2)) * 20.0) + (TotalDamageDealt) * (0.2 + 1.5 / (Math.Round(BattleTierCount / BattleCount, 2))) + 200.0 * (TotalSpotted) + 150.0 * (TotalDefensePoints) + 150.0 * (TotalCapturePoints)) / BattleCount;
            //double Totalefficiency = ((TotalDamageDealt / BattleCount) * (10 / (Math.Round(BattleTierCount / BattleCount, 2) + 2)) * (0.23 + 2 * Math.Round(BattleTierCount / BattleCount, 2) / 100)) + ((TotalKills / BattleCount) * 250) + ((TotalSpotted / BattleCount) * 150) + ((Math.Log((TotalCapturePoints / BattleCount) + 1, 1.732)) * 150) + ((TotalDefensePoints / BattleCount) * 150);
          //double Totalefficiency = ScriptWrapper.GetEFFValue(BattleCount, TotalDamageDealt, Math.Round(BattleTierCount / BattleCount, 2), TotalKills, TotalSpotted, TotalCapturePoints, TotalDefensePoints, (VictoryCount/BattleCount)*100, true, wotS.AverageTier, wotS.AverageDefencePoints, wotS.BattlesCount);
            //double Totalefficiency = -3331; //ScriptWrapper.GetEFFValue(BattleCount, TotalDamageDealt, Math.Round(BattleTierCount / BattleCount, 2), TotalKills, TotalSpotted, TotalCapturePoints, TotalDefensePoints, wotS.Victory_Ratio * 100 , true, wotS.AverageTier, wotS.AverageDefencePoints, wotS.BattlesCount, TotalDamageAssistedRadio, TotalDamageAssistedTrack);
            html += "<td align=right>" + WOTHelper.FormatNumberToString(TotalRatingEff / BattleCount, 2) + @"</td>";
            html += "<td align=right>" + WOTHelper.FormatNumberToString(TotalRatingBR / BattleCount, 2) + @"</td>";
            html += "<td align=right>" + WOTHelper.FormatNumberToString(TotalRatingWN7 / BattleCount, 2) + @"</td>";
            html += "<td align=right>" + WOTHelper.FormatNumberToString(TotalRatingWN8 / BattleCount, 2) + @"</td>";
				
            html += "</tr></tfoot></table>";
            // File.WriteAllText(@"h:\Untitled-2.htm", html);
            return html;

        }

        private string CreateRecentBattleKillList(string fragList, TankDescriptions tankDesc)
        {
            if (!string.IsNullOrEmpty(fragList))
            {
                List<string> fl = new List<string>();
                foreach (string item in fragList.Split(';'))
                {
                    int countryID = int.Parse(item.Split(':')[1].Split('_')[0]);
                    int tankID = int.Parse(item.Split(':')[1].Split('_')[1]);
                    string x = CountryFlag(countryID) + "|";
                    x += GetRoman(tankDesc.Tier(countryID, tankID)) + "|";
                    x += TankImageToolTip(countryID, tankID, tankDesc.Description(countryID, tankID)) + "|";
                    x += tankDesc.Description(countryID, tankID);
                    fl.Add(x.Replace("<Image src=", "").Replace("/>", "").Replace("'", "").Replace("\\", "\\\\"));
                }

                return String.Format(@"OnMouseOver='fragList(""{0}"")' OnMouseOut='toolTip()'", string.Join(";", fl.ToArray()));
            }
            else
                return "";

        }

        private string GetRecentBattlesDescription(string playerName)
        {
            switch (UserSettings.RecentBattlesCurrentSession)
            {
                case 0:
                    Tuple<DateTime?, DateTime?, int?> dates = RecentBattleHelpers.GetSessionDates(playerName);
                    if (dates.Item1 != null)
                    {
                        bool dateTest = Convert.ToDateTime(dates.Item2) > DateTime.Now;
                            return String.Format("{0} ({1} - {2})",
                                Translations.TranslationGet(dateTest ? "STR_CAP_RBCS" : "STR_CAP_RBS",
                                                                      "DE",
                                                                      dateTest ? "Current Session" : "Session #") + (dateTest ? "" : dates.Item3.ToString()),
                                       Convert.ToDateTime(dates.Item1).ToString(UserSettings.DateFormat + " " + (UserSettings.TimeStamp == true ? UserSettings.TimeFormat : "")),
                                       (Convert.ToDateTime(dates.Item2) > DateTime.Now ? DateTime.Now.ToString(UserSettings.DateFormat + " " + (UserSettings.TimeStamp == true ? UserSettings.TimeFormat : "")) : Convert.ToDateTime(dates.Item2).ToString(UserSettings.DateFormat + " " + (UserSettings.TimeStamp == true ? UserSettings.TimeFormat : ""))));
                       
                    }
                    return Translations.TranslationGet("STR_CAP_RBCS", "DE", "Current Session");
                case 1:
                    return Translations.TranslationGet("STR_CAP_RBTB", "DE", "Today's battles"); ;
                case 2:
                    return Translations.TranslationGet("STR_CAP_RBLTD", "DE", "Last 3 days");
                case 3:
                    return Translations.TranslationGet("STR_CAP_RBLW", "DE", "Last week");
                case 4:
                    return Translations.TranslationGet("STR_CAP_RBLX", "DE", "Last {X} battles").Replace("{X}", UserSettings.LastPlayedCompareQuota.ToString());

                default:
                    return "";
            }
        }
	}

    
}

using System;
using System.Collections.Generic;
using System.Linq;


namespace WOTStatistics.Core
{
    partial class WOTHtml : WOTHtmlBase
    {
        public string GlobalHTML(WOTStats stats, WOTStats prevStats, WOTStatsDelta delta)
        {
            string html = @"<div id='toolTipLayer' style='position:absolute; visibility: hidden;left:0;right:0;'></div> <table class='b-gray-text' Border=0 width=100%>
								<thead>
								 <tr>
										<th align=center width=33.33%><strong>" + Translations.TranslationGet("HTML_HEAD_OVRESULTS", "DE", "Overall Results") + @"<strong></th>
										<th align=center width=33.33%><strong>" + Translations.TranslationGet("HTML_HEAD_BATPERFORMANCE", "DE", "Battle Performance") + @"<strong></th>
										<th align=center width=33.33%><strong>" + Translations.TranslationGet("HTML_HEAD_BATEXPERIENCE", "DE", "Battle Experience") + @"<strong></th>
									</tr> 
								</thead> 
								<tbody>
								   
									<tr>
										<td class='td-1' class=r valign=top>" + OverallResults(stats, delta) + @"</td>
										<td class='td-1' class=r valign=top>" + BattlePerformance(stats, delta) + @"</td>
										<td class='td-1' class=r valign=top>" + BattleExperience(stats, delta) + @"</td>
									</tr>
									<tr>
										<td class='td-1' COLSPAN=3 Height=20px> </TD>
									</tr>
									<tr>
										<th align=center colspan=3><strong>" + Translations.TranslationGet("HTML_HEAD_COUNTRYSTATS", "DE", "Country Statistics") + @"<strong></th>
									</tr>
									<tr>
										<td class='td-1' class=r valign=top colspan=3>" + CountryStats(stats, prevStats, delta) + @"</td>
									</tr>
									<tr>
									   <td class='td-1' COLSPAN=3 Height=20px> </TD>
									</tr>
									<tr>
										<th align=center colspan=3><strong>" + Translations.TranslationGet("HTML_HEAD_TANKCLASSSTATS", "DE", "Tank Class Statistics") + @"<strong></th>
									</tr>
									<tr>
										<td class='td-1' class=r valign=top colspan=3>" + TankTypeStats(stats, prevStats, delta) + @"</td>
									</tr>
									<tr>
									   <td class='td-1' COLSPAN=3 Height=20px> </TD>
									</tr>
									<tr>
										<th align=center colspan=1><strong>" + Translations.TranslationGet("HTML_HEAD_TIEREFF", "DE", "Tier WN7") + @"<strong></th>
										<th align=center colspan=1><strong>" + Translations.TranslationGet("HTML_HEAD_TOPEFF", "DE", "Top 7 Efficiency") + @"<strong></th>
										<th align=center colspan=1><strong>" + Translations.TranslationGet("HTML_HEAD_TOPEXP", "DE", "Top 7 Experience") + @"<strong></th>
									</tr>
									<tr>
										<td class='td-1' class=r valign=top colspan=1>" + TierEfficiency(stats, prevStats) + @"</td>
										<td class='td-1' class=r valign=top colspan=1>" + Top05Eff(stats) + @"</td>
										<td class='td-1' class=r valign=top colspan=1>" + Top05Experience(stats) + @"</td>
									</tr>
								</tbody> 
								</table>";

            stats = null;
            delta = null;
            return html;
        }

        private string OverallResults(WOTStats stats, WOTStatsDelta delta)
        {

            string html = @"<table class=o width=100%>
									<thead>
									</thead>
									<tbody>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_BATTLESPART", "DE", "Battles Participated:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.BattlesCount, 0) + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(Math.Round((double)delta.BattlesCount, 2), 0) + @"</td>
										</tr>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_VICTORIES", "DE", "Victories:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.Victories, 0) + @" <span style='white-space: nowrap;'>(" + WOTHelper.FormatNumberToString(Math.Round(stats.Victory_Percentage, 2), 2) + "%)</span>" + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(delta.TotalVictories, 0) + @" <span style='white-space: nowrap;'>(" + GetDelta(Math.Round(delta.WinPercentage, 2), "%", 2) + @")</span></td>
										</tr>
										 <tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_DEFEATS", "DE", "Defeats:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.Losses, 0) + @" <span style='white-space: nowrap;'>(" + WOTHelper.FormatNumberToString(Math.Round(stats.Losses_Percentage, 2), 2) + "%)</span>" + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(delta.TotalLosses, 0) + @" <span style='white-space: nowrap;'>(" + GetDelta(Math.Round(delta.LossPercentage, 2), "%", 2, true) + @")</span></td>
										</tr>
										 <tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_DRAWS", "DE", "Draws:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.Draws, 0) + @" <span style='white-space: nowrap;'>(" + WOTHelper.FormatNumberToString(Math.Round(stats.Draws_Percentage, 2), 2) + "%)</span>" + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(delta.TotalDraws, 0) + @" <span style='white-space: nowrap;'>(" + GetDelta(Math.Round(delta.DrawPercentage, 2), "%", 2, true) + @")</span></td>
										</tr>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_SURVIVAL", "DE", "Survival:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.Survived, 0) + @" <span style='white-space: nowrap;'>(" + WOTHelper.FormatNumberToString(Math.Round(stats.Survived_Percentage, 2), 2) + "%)</span>" + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(delta.Survived, 0) + @" <span style='white-space: nowrap;'>(" + GetDelta(Math.Round(delta.Survived_Percentage, 2), "%", 2) + @")</span></td>
										</tr>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_TOTALBT", "DE", "Total Battle Time:") + @"</td>
											<td class='td-1'  align=right>" + FormatBattleTime(TotalBattleTime(stats)) + @"</td>
											<td class='td-1'  align=right>" + FormatBattleTime(TotalBattleTimeDelta(delta)) + @"</td>
										</tr>
									   <tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_AVGBT", "DE", "Average Battle Time:") + @"</td>
											<td class='td-1'  align=right>" + FormatBattleTime(AverageBattleTime(stats)) + @"</td>
											<td/>
										</tr> 
									   <!-- tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_TOTALMILEAGE", "DE", "Total Mileage") + @"</td>
											<td class='td-1'  align=right>" + FormatBattleTime(AverageBattleTime(stats)) + @"</td>
											<td/>
										</tr> 
									   <tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_AVGMILEAGE", "DE", "Average Mileage") + @"</td>
											<td class='td-1'  align=right>" + FormatBattleTime(AverageBattleTime(stats)) + @"</td>
											<td/>
										</tr --> 
									</tbody>
								</table>";

            return html;
        }

        private string BattlePerformance(WOTStats stats, WOTStatsDelta delta)
        {

            string html = @"<table class=o width=100%>
									<thead>
									</thead>
									<tbody>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_KILLS", "DE", "Kills:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.Destroyed, 0) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.Destroyed, 2), "", 0) + @"</td>
										</tr>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_KILLRATIO", "DE", "Kill Ratio:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(((double)stats.Destroyed / (double)stats.BattlesCount), 2) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.Destroyed / (double)stats.BattlesCount, 2), "", 2) + @"</td>
										</tr>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_DETECTED", "DE", "Detected:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.Detected, 0) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.Detected, 2), "", 0) + @"</td>
										</tr>
										 <tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_HITRATIO", "DE", "Hit Ratio:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(Math.Round(stats.HitRatio, 2), 2) + "%" + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.HitRatio, 2), "%", 2) + @"</td>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_CAPPOINTS", "DE", "Capture Points:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.CapturePoints, 0) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.CapturePoints, 2), "", 0) + @"</td>
										</tr>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_DEFPOINTS", "DE", "Defense Points:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.DefencePoints, 0) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.DefencePoints, 2), "", 0) + @"</td>
										</tr>
										<!-- tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_EFFICIENCY", "DE", "Eff") + @"</td>
											<td class='td-1'  align=right>" + EfficiencyDescription(stats.OverallRatingEff) + " - " + WOTHelper.FormatNumberToString(Math.Round(stats.OverallRatingEff, 2), 2) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.OverallRatingEff, 2), "", 2) + @"</td>
										</tr -->
										<!-- tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_BATTLERATING", "DE", "Battle Rating") + @"</td>
											<td class='td-1'  align=right>" + EfficiencyDescription(stats.OverallRatingBR) + " - " + WOTHelper.FormatNumberToString(Math.Round(stats.OverallRatingBR, 2), 2) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.OverallRatingBR, 2), "", 2) + @"</td>
										</tr -->
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("STR_WN7_Caption", "DE", "WN7") + @"</td>
											<td class='td-1'  align=right>" + WN7ColorScaleDescription(stats.OverallRatingWN7) + " - " + WOTHelper.FormatNumberToString(Math.Round(stats.OverallRatingWN7, 2), 2) + @"</td>
											<td class='td-1'  align=right>&nbsp;</td>
										</tr>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("STR_WN8_Caption", "DE", "WN8") + @"</td>
											<td class='td-1'  align=right>" + WN8ColorScaleDescription(stats.OverallRatingWN8) + " - " + WOTHelper.FormatNumberToString(Math.Round(stats.OverallRatingWN8, 2), 2) + @"</td>
											<td class='td-1'  align=right>&nbsp;</td>
										</tr>
									</tbody>
								</table>";
            //" + GetDelta(delta.OverallRatingWN7, "", 2) + @"
            return html;
        }

        private string BattleExperience(WOTStats stats, WOTStatsDelta delta)
        {

            string html = @"<table class=o width=100%>
									<thead>
									</thead>
									<tbody>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_TOTALEXP", "DE", "Total Experience:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(Math.Round(stats.Experience, 2), 0) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.Experience, 2), "", 0) + @"</td>
										</tr>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_AVGEXP", "DE", "Average Experience per Battle:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(Math.Round(stats.AverageExperiencePerBattle, 2), 2) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.AverageExperiencePerBattle, 2), "", 2) + @"</td>
										</tr>
										 <tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_MAXEXP", "DE", "Maximum Experience per Battle:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(Math.Round(stats.MaxExperience, 2), 0) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.MaxExperience, 2), "", 0) + @"</td>
										</tr>
										 <tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_DMGCAUSED", "DE", "Damage Dealt:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.DamageDealt, 0) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.DamageDealt, 2), "", 0) + @"</td>
										</tr>
										 <tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_DMGASSRADIO", "DE", "Damage Assisted Radio:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.DamageAssistedRadio, 0) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.DamageAssistedRadio, 2), "", 0) + @"</td>
										</tr>
										 <tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_DMGASSTRACK", "DE", "Damage Assisted Track:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.DamageAssistedTracks, 0) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.DamageAssistedTracks, 2), "", 0) + @"</td>
										</tr>
										 <tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_DMGREC", "DE", "Damage Received:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(stats.DamageReceived, 0) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.DamageReceived, 2), "", 0) + @"</td>
										</tr>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_COMP_AVGDAM", "DE", "Average Damage:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(Math.Round(stats.AverageDamageDealtPerBattle, 2), 2) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.AverageDamagePerBattle, 2), "", 2) + @"</td>
										</tr>
										<tr>
											<td class='td-1' >" + Translations.TranslationGet("HTML_CONT_DMGRATIO", "DE", "Damage Ratio:") + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(Math.Round(stats.DamageRatio, 2), 2) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(Math.Round(delta.DamageRatio, 2), "", 2) + @"</td>
										</tr>
									</tbody>
								</table>";

            return html;
        }

        private string CountryStats(WOTStats stats, WOTStats prevStats, WOTStatsDelta delta)
        {
            CountryDescriptions countries = new CountryDescriptions(_message);
            string html = @"<table class=o width=100%>
									<thead>
									</thead>
									<tbody>
									<tr>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_COUNTRY", "DE", "Country") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_BATTLES", "DE", "Battles") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_VICTORIES", "DE", "Victories") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_DEFEATS", "DE", "Defeats") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_DRAWS", "DE", "Draws") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_LASTPLAY", "DE", "Last Played") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_KILLS", "DE", "Kills") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_DMGCAUSED", "DE", "Damage Caused") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_DMGREC", "DE", "Damage Received") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("STR_WN7_Caption", "DE", "WN7") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
									</tr>";
            foreach (KeyValuePair<int, string> country in countries)
            {
                double tier = 0;
                for (int i = 1; i <= 10; i++)
                {
                    double var = (from x in stats.tanks
                                  where x.Tier == i && x.CountryID == country.Key
                                  select x.Data.BattlesCount).Sum() * i;
                    tier += var;

                }
                double frags = (from x in stats.tanks
                                where x.CountryID == country.Key
                                select x.Data.Frags).Sum();

                double victories = (from x in stats.tanks
                                    where x.CountryID == country.Key
                                    select x.Data.Victories).Sum();

                double defeats = (from x in stats.tanks
                                  where x.CountryID == country.Key
                                  select x.Data.Defeats).Sum();

                double draws = (from x in stats.tanks
                                where x.CountryID == country.Key
                                select x.Data.Draws).Sum();

                double damage = (from x in stats.tanks
                                 where x.CountryID == country.Key
                                 select x.Data.DamageDealt).Sum();

                double damageAssistedRadio = (from x in stats.tanks
                                 where x.CountryID == country.Key
                                 select x.Data.DamageAssistedRadio).Sum();

                double damageAssistedTrack = (from x in stats.tanks
                                              where x.CountryID == country.Key
                                              select x.Data.DamageAssistedTracks).Sum();

                double damageReceived = (from x in stats.tanks
                                         where x.CountryID == country.Key
                                         select x.Data.DamageReceived).Sum();

                double spotted = (from x in stats.tanks
                                  where x.CountryID == country.Key
                                  select x.Data.Spotted).Sum();

                double defence = (from x in stats.tanks
                                  where x.CountryID == country.Key
                                  select x.Data.DefencePoints).Sum();

                double capture = (from x in stats.tanks
                                  where x.CountryID == country.Key
                                  select x.Data.CapturePoints).Sum();

                double battles = (from x in stats.tanks
                                  where x.CountryID == country.Key
                                  select x.Data.BattlesCount).Sum();

                //double efficiency = (frags * (350.0 - (Math.Round(tier / battles, 2)) * 20.0) + damage * (0.2 + 1.5 / (Math.Round(tier / battles, 2))) + 200.0 * spotted + 150.0 * defence + 150.0 * capture) / battles;
                //double efficiency = ((damage / battles) * (10 / (Math.Round(tier / battles, 2) + 2)) * (0.23 + 2 * Math.Round(tier / battles, 2) / 100)) + ((frags / battles) * 250) + ((spotted / battles) * 150) + ((Math.Log((capture / battles) + 1, 1.732)) * 150) + ((defence / battles) * 150);
                RatingStructure ratingStruct = new RatingStructure();
                ratingStruct.WN8ExpectedTankList = WN8ExpectedTankList;
                ratingStruct.countryID = -1;
                ratingStruct.tankID = -1;
                ratingStruct.tier = tier;
                ratingStruct.globalTier = tier;

                ratingStruct.singleTank = true;

                ratingStruct.battlesCount =  Convert.ToInt32(battles);
                ratingStruct.battlesCount8_8 = 0;
                ratingStruct.capturePoints = capture;
                ratingStruct.defencePoints = defence;

                ratingStruct.damageAssistedRadio = 0;
                ratingStruct.damageAssistedTracks = 0;
                ratingStruct.damageDealt = damage;
                ratingStruct.frags = frags;
                ratingStruct.spotted = spotted;

                ratingStruct.wins = victories;
                ratingStruct.gWinRate = ratingStruct.winRate;
                double RatingWN7 = WOTStatistics.Core.Ratings.GetRatingWN7(ratingStruct).Value;


                //double delTier = 0;
                //for (int i = 1; i <= 10; i++)
                //{
                //double delVar = (from x in delta.tanks
                //                 where x.Tier == i && x.CountryID == country.Key
                //                 select x.Data.BattlesCount).Sum() * i;
                //delTier += delVar;

                //}
                //double delFrags = (from x in delta.tanks
                //                   where x.CountryID == country.Key
                //                   select x.Data.Frags).Sum();

                //double delDamage = (from x in delta.tanks
                //                    where x.CountryID == country.Key
                //                    select x.Data.DamageDealt).Sum();

                //double delSpotted = (from x in delta.tanks
                //                     where x.CountryID == country.Key
                //                     select x.Data.Spotted).Sum();

                //double delDefence = (from x in delta.tanks
                //                     where x.CountryID == country.Key
                //                     select x.Data.DefencePoints).Sum();

                //double delCapture = (from x in delta.tanks
                //                     where x.CountryID == country.Key
                //                     select x.Data.CapturePoints).Sum();

                //double delBattles = (from x in delta.tanks
                //                     where x.CountryID == country.Key
                //                     select x.Data.BattlesCount).Sum();

                double prevTier = 0;
                for (int i = 1; i <= 10; i++)
                {
                    double prevVar = (from x in prevStats.tanks
                                      where x.Tier == i && x.CountryID == country.Key
                                      select x.Data.BattlesCount).Sum() * i;
                    prevTier += prevVar;
                }
                double prevFrags = (from x in prevStats.tanks
                                    where x.CountryID == country.Key
                                    select x.Data.Frags).Sum();
                double prevVictories = (from x in prevStats.tanks
                                        where x.CountryID == country.Key
                                        select x.Data.Victories).Sum();
                double prevDefeats = (from x in prevStats.tanks
                                      where x.CountryID == country.Key
                                      select x.Data.Defeats).Sum();
                double prevDraws = (from x in prevStats.tanks
                                    where x.CountryID == country.Key
                                    select x.Data.Draws).Sum();
                double prevDamage = (from x in prevStats.tanks
                                     where x.CountryID == country.Key
                                     select x.Data.DamageDealt).Sum();
                double prevDamageAssistedRadio = (from x in prevStats.tanks
                                     where x.CountryID == country.Key
                                     select x.Data.DamageAssistedRadio).Sum();
                double prevDamageAssistedTrack = (from x in prevStats.tanks
                                     where x.CountryID == country.Key
                                     select x.Data.DamageAssistedTracks).Sum();
                double prevDamageReceived = (from x in prevStats.tanks
                                             where x.CountryID == country.Key
                                             select x.Data.DamageReceived).Sum();
                double prevSpotted = (from x in prevStats.tanks
                                      where x.CountryID == country.Key
                                      select x.Data.Spotted).Sum();
                double prevDefence = (from x in prevStats.tanks
                                      where x.CountryID == country.Key
                                      select x.Data.DefencePoints).Sum();
                double prevCapture = (from x in prevStats.tanks
                                      where x.CountryID == country.Key
                                      select x.Data.CapturePoints).Sum();
                double prevBattles = (from x in prevStats.tanks
                                      where x.CountryID == country.Key
                                      select x.Data.BattlesCount).Sum();
                // double delEfficiency = ((frags - delFrags) * (350.0 - (Math.Round((tier - delTier) / (battles - delBattles), 2)) * 20.0) + (damage - delDamage) * (0.2 + 1.5 / (Math.Round((tier - delTier) / (battles - delBattles), 2))) + 200.0 * (spotted - delSpotted) + 150.0 * (defence - delDefence) + 150.0 * (capture - delCapture)) / (battles - delBattles);
                //double delEfficiency = ((prevDamage / prevBattles) * (10 / (Math.Round(prevTier / prevBattles, 2) + 2)) * (0.23 + 2 * Math.Round(prevTier / prevBattles, 2) / 100)) + ((prevFrags / prevBattles) * 250) + ((prevSpotted / prevBattles) * 150) + ((Math.Log((prevCapture / prevBattles) + 1, 1.732)) * 150) + ((prevDefence / prevBattles) * 150);

                ratingStruct = new RatingStructure();
                ratingStruct.WN8ExpectedTankList = WN8ExpectedTankList;
                ratingStruct.countryID = -1;
                ratingStruct.tankID = -1;
                ratingStruct.tier = tier;
                ratingStruct.globalTier = tier;

                ratingStruct.singleTank = true;

                ratingStruct.battlesCount = Convert.ToInt32(prevBattles);
                ratingStruct.battlesCount8_8 = 0;
                ratingStruct.capturePoints = prevCapture;
                ratingStruct.defencePoints = prevDefence;

                ratingStruct.damageAssistedRadio = 0;
                ratingStruct.damageAssistedTracks = 0;
                ratingStruct.damageDealt = prevDamage;
                ratingStruct.frags = prevFrags;
                ratingStruct.spotted = prevSpotted;

                ratingStruct.wins = prevVictories;
                ratingStruct.gWinRate = ratingStruct.winRate;


                double prevRatingWN7 = WOTStatistics.Core.Ratings.GetRatingWN7(ratingStruct).Value;

                html += @"
										<tr>
											<td class='" + CountryFlag(country.Key) + @" td-armory-icon'>
												<div class='wrapper'>
													<span class='MiddleCenter'><a class='b-gray-text'>" + country.Value + @"</a></span>
												</div>
											 </td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(battles, 0) + @"</td>
											<td class='td-1'  align=right>" + GetDelta(battles - prevBattles, "", 0) + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(victories, 0) + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(victories - prevVictories, 0) + @" <span style='white-space: nowrap;'>(" + GetDelta(Math.Round((victories / battles * 100) - (prevVictories / prevBattles * 100), 2), "%", 2) + ")</span>" + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(defeats, 0) + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(defeats - prevDefeats, 0) + @" <span style='white-space: nowrap;'>(" + GetDelta(Math.Round((defeats / battles * 100) - (prevDefeats / prevBattles * 100), 2), "%", 2, true) + ")</span>" + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(draws, 0) + @"</td>
											<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(draws - prevDraws, 0) + @" <span style='white-space: nowrap;'>(" + GetDelta(Math.Round((draws / battles * 100) - (prevDraws / prevBattles * 100), 2), "%", 2, true) + ")</span>" + "</td>";


                try
                {
                    html += @"<td class='td-1'  align=right>" + stats.tanks.Where(x => x.CountryID == country.Key).Max(y => y.Updated_Friendly).ToString(UserSettings.DateFormat + (UserSettings.TimeStamp ? " " + UserSettings.TimeFormat : "")) + "</td>";
                }
                catch
                {
                    html += @"<td class='td-1'  align=right> </td>";
                }



                html += @" <td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(frags, 0) + @"</td>
							<td class='td-1'  align=right>" + GetDelta(Math.Round(frags - prevFrags, 2), "", 0) + @"</td>
							<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(damage, 0) + @"</td>
							<td class='td-1'  align=right>" + GetDelta(Math.Round(damage - prevDamage, 2), "", 0) + @"</td>
							<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(damageReceived, 0) + @"</td>
							<td class='td-1'  align=right>" + GetDelta(Math.Round(damageReceived - prevDamageReceived, 2), "", 0) + @"</td>
							<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(Math.Round(RatingWN7, 2), 2) + @"</td>
							<td class='td-1'  align=right>" + GetDelta(Math.Round(RatingWN7, 2) - Math.Round(prevRatingWN7, 2), "", 2) + @"</td>
						</tr>";
            }

            html += @"</tbody> 
								</table>";
            return html;
        }

        private string TankTypeStats(WOTStats stats, WOTStats prevStats, WOTStatsDelta delta)
        {
            using (TankTypeDescription tankTypes = new TankTypeDescription(_message))
            {
                string html = @"<table class=o width=100%>
												<thead>
												</thead>
												<tbody>
												<tr>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_VEHICLECLASS", "DE", "Vehicle Class") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_BATTLES", "DE", "Battles") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_VICTORIES", "DE", "Victories") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_DEFEATS", "DE", "Defeats") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_DRAWS", "DE", "Draws") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_LASTPLAY", "DE", "Last Played") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_KILLS", "DE", "Kills") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_DMGCAUSED", "DE", "Damage Dealt") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
                                                    <th align=center><strong>" + Translations.TranslationGet("HTML_CONT_DMGASSRADIO", "DE", "Damage Assisted Radio") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_CONT_DMGASSTRACK", "DE", "Damage Assisted Track") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
                                                    <th align=center><strong>" + Translations.TranslationGet("HTML_CONT_DMGREC", "DE", "Damage Received") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("STR_WN7_Caption", "DE", "WN7") + @"</strong></th>
													<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
												</tr>";
                foreach (KeyValuePair<string, string> tankType in tankTypes)
                {
                    double tier = 0;
                    for (int i = 1; i <= 10; i++)
                    {
                        double var = (from x in stats.tanks
                                      where x.Tier == i && x.TankClass == tankType.Key
                                      select x.Data.BattlesCount).Sum() * i;
                        tier += var;
                    }
                    double frags = (from x in stats.tanks
                                    where x.TankClass == tankType.Key
                                    select x.Data.Frags).Sum();
                    double victories = (from x in stats.tanks
                                        where x.TankClass == tankType.Key
                                        select x.Data.Victories).Sum();
                    double defeats = (from x in stats.tanks
                                      where x.TankClass == tankType.Key
                                      select x.Data.Defeats).Sum();
                    double draws = (from x in stats.tanks
                                    where x.TankClass == tankType.Key
                                    select x.Data.Draws).Sum();
                    double damage = (from x in stats.tanks
                                     where x.TankClass == tankType.Key
                                     select x.Data.DamageDealt).Sum();
                    double damageAssistedRadio = (from x in stats.tanks
                                     where x.TankClass == tankType.Key
                                    select x.Data.DamageAssistedRadio).Sum();
                    double damageAssistedTrack = (from x in stats.tanks
                                     where x.TankClass == tankType.Key
                                     select x.Data.DamageAssistedTracks).Sum();
                    double damageReceived = (from x in stats.tanks
                                             where x.TankClass == tankType.Key
                                             select x.Data.DamageReceived).Sum();
                    double spotted = (from x in stats.tanks
                                      where x.TankClass == tankType.Key
                                      select x.Data.Spotted).Sum();
                    double defence = (from x in stats.tanks
                                      where x.TankClass == tankType.Key
                                      select x.Data.DefencePoints).Sum();
                    double capture = (from x in stats.tanks
                                      where x.TankClass == tankType.Key
                                      select x.Data.CapturePoints).Sum();
                    double battles = (from x in stats.tanks
                                      where x.TankClass == tankType.Key
                                      select x.Data.BattlesCount).Sum();
                    //double efficiency = (frags * (350.0 - (Math.Round(tier / battles, 2)) * 20.0) + damage * (0.2 + 1.5 / (Math.Round(tier / battles, 2))) + 200.0 * spotted + 150.0 * defence + 150.0 * capture) / battles;
                    //double efficiency = ((damage / battles) * (10 / (Math.Round(tier / battles, 2) + 2)) * (0.23 + 2 * Math.Round(tier / battles, 2) / 100)) + ((frags / battles) * 250) + ((spotted / battles) * 150) + ((Math.Log((capture / battles) + 1, 1.732)) * 150) + ((defence / battles) * 150);


                    RatingStructure ratingStructs = new RatingStructure();
                    ratingStructs.countryID =0;
                    ratingStructs.tankID = 0;
                    ratingStructs.tier = tier;
                    ratingStructs.globalTier = tier;

                    ratingStructs.singleTank = false;

                    ratingStructs.battlesCount = Convert.ToInt32(battles);
                    ratingStructs.battlesCount8_8 = 0;
                    ratingStructs.capturePoints = capture;
                    ratingStructs.defencePoints = defence;

                    ratingStructs.damageAssistedRadio = damageAssistedRadio;
                    ratingStructs.damageAssistedTracks = damageAssistedTrack;
                    ratingStructs.damageDealt = damage;
                    ratingStructs.frags = frags;
                    ratingStructs.spotted = spotted;

                    
                    ratingStructs.wins=victories;
                    ratingStructs.gWinRate = ratingStructs.winRate;

                    double efficiency = WOTStatistics.Core.Ratings.GetRatingWN7(ratingStructs).Value;
                        
                        //ScriptWrapper.GetEFFValue(battles, damage, Math.Round(tier / battles, 2), frags, spotted, capture, defence, (victories / battles) * 100, true, stats.AverageTier, stats.AverageDefencePoints, stats.BattlesCount, damageAssistedRadio, damageAssistedTrack);
                    //double delTier = 0;
                    //for (int i = 1; i <= 10; i++)
                    //{
                    //double delVar = (from x in delta.tanks
                    //                 where x.Tier == i && x.TankClass == tankType.Key
                    //                 select x.Data.BattlesCount).Sum() * i;
                    //delTier += delVar;
                    //}
                    //double delFrags = (from x in delta.tanks
                    //                   where x.TankClass == tankType.Key
                    //                   select x.Data.Frags).Sum();
                    //double delDamage = (from x in delta.tanks
                    //                    where x.TankClass == tankType.Key
                    //                    select x.Data.DamageDealt).Sum();
                    //double delSpotted = (from x in delta.tanks
                    //                     where x.TankClass == tankType.Key
                    //                     select x.Data.Spotted).Sum();
                    //double delDefence = (from x in delta.tanks
                    //                     where x.TankClass == tankType.Key
                    //                     select x.Data.DefencePoints).Sum();
                    //double delCapture = (from x in delta.tanks
                    //                     where x.TankClass == tankType.Key
                    //                     select x.Data.CapturePoints).Sum();
                    //double delBattles = (from x in delta.tanks
                    //                     where x.TankClass == tankType.Key
                    //                     select x.Data.BattlesCount).Sum();

                    double prevTier = 0;
                    for (int i = 1; i <= 10; i++)
                    {
                        double prevVar = (from x in prevStats.tanks
                                          where x.Tier == i && x.TankClass == tankType.Key
                                          select x.Data.BattlesCount).Sum() * i;
                        prevTier += prevVar;
                    }
                    double prevFrags = (from x in prevStats.tanks
                                        where x.TankClass == tankType.Key
                                        select x.Data.Frags).Sum();
                    double prevVictories = (from x in prevStats.tanks
                                            where x.TankClass == tankType.Key
                                            select x.Data.Victories).Sum();
                    double prevDefeats = (from x in prevStats.tanks
                                          where x.TankClass == tankType.Key
                                          select x.Data.Defeats).Sum();
                    double prevDraws = (from x in prevStats.tanks
                                        where x.TankClass == tankType.Key
                                        select x.Data.Draws).Sum();
                    double prevDamage = (from x in prevStats.tanks
                                         where x.TankClass == tankType.Key
                                         select x.Data.DamageDealt).Sum();
                    double prevDamageAssistedRadio = (from x in prevStats.tanks
                                         where x.TankClass == tankType.Key
                                         select x.Data.DamageAssistedRadio).Sum();
                    double prevDamageAssistedTrack = (from x in prevStats.tanks
                                         where x.TankClass == tankType.Key
                                         select x.Data.DamageAssistedTracks).Sum();
                    double prevDamageReceived = (from x in prevStats.tanks
                                                 where x.TankClass == tankType.Key
                                                 select x.Data.DamageReceived).Sum();
                    double prevSpotted = (from x in prevStats.tanks
                                          where x.TankClass == tankType.Key
                                          select x.Data.Spotted).Sum();
                    double prevDefence = (from x in prevStats.tanks
                                          where x.TankClass == tankType.Key
                                          select x.Data.DefencePoints).Sum();
                    double prevCapture = (from x in prevStats.tanks
                                          where x.TankClass == tankType.Key
                                          select x.Data.CapturePoints).Sum();
                    double prevBattles = (from x in prevStats.tanks
                                          where x.TankClass == tankType.Key
                                          select x.Data.BattlesCount).Sum();
                    // double delEfficiency = ((frags - delFrags) * (350.0 - (Math.Round((tier - delTier) / (battles - delBattles), 2)) * 20.0) + (damage - delDamage) * (0.2 + 1.5 / (Math.Round((tier - delTier) / (battles - delBattles), 2))) + 200.0 * (spotted - delSpotted) + 150.0 * (defence - delDefence) + 150.0 * (capture - delCapture)) / (battles - delBattles);
                    //double delEfficiency = ((prevDamage / prevBattles) * (10 / (Math.Round(prevTier / prevBattles, 2) + 2)) * (0.23 + 2 * Math.Round(prevTier / prevBattles, 2) / 100)) + ((prevFrags / prevBattles) * 250) + ((prevSpotted / prevBattles) * 150) + ((Math.Log((prevCapture / prevBattles) + 1, 1.732)) * 150) + ((prevDefence / prevBattles) * 150);
                    //EffCalcProperties f = new EffCalcProperties(DateTime.Now) { battles = 1, winRate = 50 };

                    ratingStructs.countryID = 0;
                    ratingStructs.tankID = 0;
                    ratingStructs.tier = prevTier;
                    ratingStructs.globalTier = prevTier;

                    ratingStructs.singleTank = false;

                    ratingStructs.battlesCount = Convert.ToInt32(prevBattles);
                    ratingStructs.battlesCount8_8 = 0;
                    ratingStructs.capturePoints = prevCapture;
                    ratingStructs.defencePoints = prevDefence;

                    ratingStructs.damageAssistedRadio = prevDamageAssistedRadio;
                    ratingStructs.damageAssistedTracks = prevDamageAssistedTrack;
                    ratingStructs.damageDealt = prevDamage;
                    ratingStructs.frags = prevFrags;
                    ratingStructs.spotted = prevSpotted;

                    ratingStructs.wins= prevVictories;
                    ratingStructs.gWinRate = ratingStructs.winRate;

                    double prevEfficiency = WOTStatistics.Core.Ratings.GetRatingWN7(ratingStructs).Value;

                    //double prevEfficiency = -356; //ScriptWrapper.GetEFFValue(prevBattles, prevDamage, Math.Round(prevTier / prevBattles, 2), prevFrags, prevSpotted, prevCapture, prevDefence, (prevVictories / prevBattles) * 100, true, prevStats.AverageTier, prevStats.AverageDefencePoints, stats.BattlesCount, prevDamageAssistedRadio, prevDamageAssistedTrack);
                    html += @"
													<tr>
														<td class='td-1'  align=leff>" + tankType.Value + @"</td>
														<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(battles, 0) + @"</td>
														<td class='td-1'  align=right>" + GetDelta(battles - prevBattles, "", 0) + @"</td>
														<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(victories, 0) + @"</td>
														<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(victories - prevVictories, 0) + @" <span style='white-space: nowrap;'>(" + GetDelta(Math.Round((victories / battles * 100) - (prevVictories / prevBattles * 100), 2), "%", 2) + ")</span>" + @"</td>
														<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(defeats, 0) + @"</td>
														<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(defeats - prevDefeats, 0) + @" <span style='white-space: nowrap;'>(" + GetDelta(Math.Round((defeats / battles * 100) - (prevDefeats / prevBattles * 100), 2), "%", 2, true) + ")</span>" + @"</td>
														<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(draws, 0) + @"</td>
														<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(draws - prevDraws, 0) + @" <span style='white-space: nowrap;'>(" + GetDelta(Math.Round((draws / battles * 100) - (prevDraws / prevBattles * 100), 2), "%", 2, true) + ")</span>" + "</td>";
                    try
                    {
                        html += @"<td class='td-1'  align=right>" + stats.tanks.Where(x => x.TankClass == tankType.Key).Max(y => y.Updated_Friendly).ToString(UserSettings.DateFormat + (UserSettings.TimeStamp ? " " + UserSettings.TimeFormat : "")) + "</td>";
                    }
                    catch
                    {
                        html += @"<td class='td-1'  align=right> </td>";
                    }
                    html += @" <td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(frags, 0) + @"</td>
										<td class='td-1'  align=right>" + GetDelta(Math.Round(frags - prevFrags, 2), "", 0) + @"</td>
										<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(damage, 0) + @"</td>
										<td class='td-1'  align=right>" + GetDelta(Math.Round(damage - prevDamage, 2), "", 0) + @"</td>
                                        <td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(damageAssistedRadio, 0) + @"</td>                                        
                                        <td class='td-1'  align=right>" + GetDelta(Math.Round(damageAssistedRadio - prevDamageAssistedTrack, 2), "", 0) + @"</td>
                                        <td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(damageAssistedRadio, 0) + @"</td>                                        
                                        <td class='td-1'  align=right>" + GetDelta(Math.Round(damageAssistedTrack - prevDamageAssistedTrack, 2), "", 0) + @"</td>
                                        <td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(damageReceived, 0) + @"</td>
										<td class='td-1'  align=right>" + GetDelta(Math.Round(damageReceived - prevDamageReceived, 2), "", 0) + @"</td>
										<td class='td-1'  align=right>" + WOTHelper.FormatNumberToString(Math.Round(efficiency, 2), 2) + @"</td>
										<td class='td-1'  align=right>" + GetDelta(Math.Round(efficiency, 2) - Math.Round(prevEfficiency, 2), "", 2) + @"</td>
									</tr>";
                }
                html += @"</tbody> 
											</table>";
                return html;
            }
        }

        private string TierEfficiency(WOTStats stats, WOTStats prevStats)
        {
            TankTypeDescription tankTypes = new TankTypeDescription(_message);
            string html = @"<table class=o width=100%>
									<thead>
									</thead>
									<tbody>
									<tr>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_TANKCOUNT", "DE", "Tank Count") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_BATTLES", "DE", "Battles") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("STR_WN7_Caption", "DE", "WN7") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_DELTA", "DE", "Delta") + @"</strong></th>
									</tr>";

            for (int i = 1; i <= 10; i++)
            {

                double frags = (from x in stats.tanks
                                where x.Tier == i
                                select x.Data.Frags).Sum();
                double victories = (from x in stats.tanks
                                    where x.Tier == i
                                    select x.Data.Victories).Sum();
                double victoriesPercentage = (from x in stats.tanks
                                              where x.Tier == i
                                              select x.Data.VictoryPercentage).Sum();
                double damage = (from x in stats.tanks
                                 where x.Tier == i
                                 select x.Data.DamageDealt).Sum();
                double damageAssistedRadio = (from x in stats.tanks
                                 where x.Tier == i
                                  select x.Data.DamageAssistedRadio).Sum();
                double damageAssistedTrack = (from x in stats.tanks
                                 where x.Tier == i
                                 select x.Data.DamageAssistedTracks).Sum();
                double spotted = (from x in stats.tanks
                                  where x.Tier == i
                                  select x.Data.Spotted).Sum();
                double defence = (from x in stats.tanks
                                  where x.Tier == i
                                  select x.Data.DefencePoints).Sum();
                double capture = (from x in stats.tanks
                                  where x.Tier == i
                                  select x.Data.CapturePoints).Sum();
                double battles = (from x in stats.tanks
                                  where x.Tier == i
                                  select x.Data.BattlesCount).Sum();

                double tanksPerTier = stats.tanks.Where(x => x.Tier == i).Count();


                RatingStructure ratingStructs = new RatingStructure();
                ratingStructs.countryID = 0;
                ratingStructs.tankID = 0;
                ratingStructs.tier = stats.AverageTier;
                ratingStructs.globalTier = ratingStructs.tier;

                ratingStructs.singleTank = false;

                ratingStructs.battlesCount = Convert.ToInt32(battles);
                ratingStructs.battlesCount8_8 = 0;
                ratingStructs.capturePoints = capture;
                ratingStructs.defencePoints = defence;

                ratingStructs.damageAssistedRadio = damageAssistedRadio;
                ratingStructs.damageAssistedTracks = damageAssistedTrack;
                ratingStructs.damageDealt = damage;
                ratingStructs.frags = frags;
                ratingStructs.spotted = spotted;

                ratingStructs.wins=victories;
                ratingStructs.gWinRate = ratingStructs.winRate;

                double efficiency = WOTStatistics.Core.Ratings.GetRatingWN7(ratingStructs).Value;


                //double efficiency = -64323; //ScriptWrapper.GetEFFValue(battles, damage, i, frags, spotted, capture, defence, victoriesPercentage, true, stats.AverageTier, stats.AverageDefencePoints, stats.BattlesCount, damageAssistedRadio, damageAssistedTrack);



                double prevFrags = (from x in prevStats.tanks
                                    where x.Tier == i
                                    select x.Data.Frags).Sum();
                double prevVictories = (from x in prevStats.tanks
                                        where x.Tier == i
                                        select x.Data.Victories).Sum();
                double prevVictoriesPercentage = (from x in prevStats.tanks
                                                  where x.Tier == i
                                                  select x.Data.VictoryPercentage).Sum();
                double prevDamage = (from x in prevStats.tanks
                                     where x.Tier == i
                                     select x.Data.DamageDealt).Sum();
                double prevDamageAssistedRadio = (from x in prevStats.tanks
                                     where x.Tier == i
                                     select x.Data.DamageAssistedRadio).Sum();
                double prevDamageAssistedTrack = (from x in prevStats.tanks
                                     where x.Tier == i
                                     select x.Data.DamageAssistedTracks).Sum();
                double prevSpotted = (from x in prevStats.tanks
                                      where x.Tier == i
                                      select x.Data.Spotted).Sum();
                double prevDefence = (from x in prevStats.tanks
                                      where x.Tier == i
                                      select x.Data.DefencePoints).Sum();
                double prevCapture = (from x in prevStats.tanks
                                      where x.Tier == i
                                      select x.Data.CapturePoints).Sum();
                double prevBattles = (from x in prevStats.tanks
                                      where x.Tier == i
                                      select x.Data.BattlesCount).Sum();
                //double prevEfficiency = -6343; //ScriptWrapper.GetEFFValue(prevBattles, prevDamage, i, prevFrags, prevSpotted, prevCapture, prevDefence, prevVictoriesPercentage, true, stats.AverageTier, stats.AverageDefencePoints, stats.BattlesCount, prevDamageAssistedRadio, prevDamageAssistedTrack);
                //double prevEfficiency = ScriptWrapper.GetEFFValue(prevBattles, prevDamage, i, prevFrags, prevSpotted, prevCapture, prevDefence, (prevVictories / prevBattles) * 100, true, stats.AverageTier, stats.AverageDefencePoints, stats.BattlesCount);

                ratingStructs.countryID = 0;
                ratingStructs.tankID = 0;
                ratingStructs.tier = stats.AverageTier;
                ratingStructs.globalTier = ratingStructs.tier;

                ratingStructs.singleTank = false;

                ratingStructs.battlesCount = Convert.ToInt32(prevBattles);
                ratingStructs.battlesCount8_8 = 0;
                ratingStructs.capturePoints = prevCapture;
                ratingStructs.defencePoints = prevDefence;

                ratingStructs.damageAssistedRadio = prevDamageAssistedRadio;
                ratingStructs.damageAssistedTracks = prevDamageAssistedTrack;
                ratingStructs.damageDealt = prevDamage;
                ratingStructs.frags = prevFrags;
                ratingStructs.spotted = prevSpotted;

                ratingStructs.wins=prevVictories;
                ratingStructs.gWinRate = ratingStructs.winRate;

                double prevEfficiency = WOTStatistics.Core.Ratings.GetRatingWN7(ratingStructs).Value;


                //var answer = stats.tanks.Where(x => x.Tier == i).Select(x => new { tankid = x.CountryID + "_" + x.TankID, Tier = x.Tier, TotalEff = x.Data.BattlesCount * x.Efficiency }).Sum(y => y.TotalEff) / stats.tanks.Where(x => x.Tier == i).Sum(y => y.Data.BattlesCount);
                //var answer = stats.tanks.Where(x => x.Tier == i).Select(x => new { tankid = x.CountryID + "_" + x.TankID, Tier = x.Tier, TotalEff = x.Data.BattlesCount * x.Efficiency }).Sum(y => y.TotalEff);
                //var answer = stats.tanks.Where(x => x.Tier == i).Select(x => new { tankid = x.CountryID + "_" + x.TankID, Tier = x.Tier, TotalEff = x.Data.BattlesCount * x.Efficiency });
                //var answer = stats.tanks.Where(x => x.Tier == i).Select(x => new { tankid = x.CountryID + "_" + x.TankID, Tier = x.Tier, Battles = x.Data.BattlesCount, Eff = x.Efficiency });





                //<td class='td-1' align=right width=auto>" + WOTHelper.FormatNumberToString((stats.tanks.Where(x => x.Tier == i).Sum(y => y.Efficiency) / stats.tanks.Where(x => x.Tier == i).Count()), 2) + @"</td>


                //<td class='td-1' align=right width=auto>" + GetDelta((delta.tanks.Where(x => x.Tier == i).Sum(y => y.Efficiency) / delta.tanks.Where(x => x.Tier == i).Count()), "", 2) + @"</td>

                html += @"<tr>
							 <td class='td-1' align=center width=auto>" + GetRoman(i) + @"</td>
							 <td class='td-1' align=center width=auto>" + WOTHelper.FormatNumberToString(tanksPerTier, 0) + @"</td>
							 <td class='td-1' align=center width=auto>" + WOTHelper.FormatNumberToString(battles, 0) + @"</td>
							 <td class='td-1' align=center width=auto>" + GetDelta(battles - prevBattles, "", 0) + @"</td>
							 <td class='td-1' align=right width=auto>" + WOTHelper.FormatNumberToString(efficiency, 2) + @"</td>
				
							
							<td class='td-1' align=right width=auto>" + GetDelta(efficiency - prevEfficiency, "", 2) + @"</td>
						  </tr>";
            }
            //  
            html += @"</tbody> 
								</table>";
            return html;
        }

        private string Top05Eff(WOTStats stats)
        {
            string html = @"<table class=o width=100%>
									<thead>
									</thead>
									<tbody>
									<tr>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_RANK", "DE", "Rank") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_TANK", "DE", "Tank") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("STR_WN7_Caption", "DE", "WN7") + @"</strong></th>
									</tr>";


            int place = 1;
            foreach (Tank tank in stats.tanks.OrderByDescending(x => x.RatingWN7).Where(x => x.Data.BattlesCount >= UserSettings.TopMinPlayed).Take(UserSettings.TopXTake))
            {
                html += @"<tr>
							<td align=center>" + place + @"</td>
							<td class='" + CountryFlag(tank.CountryID) + @" td-armory-icon'>
							   <div class='wrapper'>
								  <span class='level'>
									<a class='b-gray-text'>" + GetRoman(tank.Tier) + @"</a>
								  </span>
								  <a>" + TankImage(tank.CountryID, tank.TankID, tank.Tank_Description) + @"</a>
							   </div>
						   </td>
						   <td><a href='h' onclick='window.external.Redirect(""" + tank.CountryID + "_" + tank.TankID + @""")'>" + tank.Tank_Description + @"</a></td>
						   <td align=right>" + WOTHelper.FormatNumberToString(Math.Round(tank.RatingWN7, 2), 2) + @"</td>
						 </tr>";
                place++;
            }



            html += @"</tbody> 
								</table>";
            return html;
        }

        private string Top05Experience(WOTStats stats)
        {
            string html = @"<table class=o width=100%>
									<thead>
									</thead>
									<tbody>
									<tr>
<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_RANK", "DE", "Rank") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_HEAD_TANK", "DE", "Tank") + @"</strong></th>
										<th align=center><strong>" + Translations.TranslationGet("HTML_EXPER", "DE", "Experience") + @"</strong></th>
									</tr>";


            int place = 1;
            foreach (Tank tank in stats.tanks.OrderByDescending(x => x.Data.MaxXp).Take(UserSettings.TopXTake))
            {
                html += @"<tr>
							<td align=center>" + place + @"</td>
							<td class='" + CountryFlag(tank.CountryID) + @" td-armory-icon'>
							   <div class='wrapper'>
								  <span class='level'>
									<a class='b-gray-text'>" + GetRoman(tank.Tier) + @"</a>
								  </span>
								  <a>" + TankImage(tank.CountryID, tank.TankID, tank.Tank_Description) + @"</a>
							   </div>
						   </td>
						   <td><a href='h' onclick='window.external.Redirect(""" + tank.CountryID + "_" + tank.TankID + @""")'>" + tank.Tank_Description + @"</a></td>
						   <td align=right>" + WOTHelper.FormatNumberToString(Math.Round((double)tank.Data.MaxXp, 2), 0) + @"</td>
						 </tr>";
                place++;
            }



            html += @"</tbody> 
								</table>";
            return html;
        }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
namespace WOTStatistics.Core
{
    partial class WOTHtml : WOTHtmlBase
    {
        private string TankInfo(Tank tank)
        {
            string html = @"  <Table class='b-gray-text' width=100%>
								<Thead>
									<tr>
										<td width=10%>" + TankImageLarge(tank.CountryID, tank.TankID, tank.Tank_Description) + @"</td>" +
                                        "<th width=80% align=center valign=middle><font size=" + (UserSettings.HTMLHeaderFont + 2) + @"px>(" + GetRoman(tank.Tier) + ") " + tank.Tank_Description + @"</font></th>
										<td class='" + CountryFlagFill(tank.CountryID) + @"' width=10%>" + MasterBadgeImage(tank.Special.MarkOfMastery) + @"</td>
									</tr>
								</Thead>
								<Tbody>
								<Tbody>
								</Table>

							   <Table class='b-gray-text' width=100%>
								<Thead>
									<tr>
										<th width = 50% align=Center></th>
										<th width = 50% align=center></th>
									</tr>
								</Thead>
								<Tbody>
								  <tr>
										<td  class='td-1' valign=top>
											<table width=100%>
												<thead>
												</thead>
												<tbody>
													<tr>
														<td  class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_HEAD_BATTLES", "DE", "Battles") + @"</strong>
														</td>
														<td  class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.BattlesCount, 0) + @"
														</td>
													</tr>
													<tr>
														<td  class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_CONT_VICTORIES", "DE", "Victories") + @"</strong>
														</td>
														<td  class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.Victories, 0) + " (" + WOTHelper.FormatNumberToString(Math.Round(tank.Data.VictoryPercentage, 2), 2) + @"%)
														</td>
													</tr>
													<tr>
														<td  class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_CONT_DEFEATS", "DE", "Defeats") + @"</strong>
														</td>
														<td  class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.Defeats, 0) + " (" + WOTHelper.FormatNumberToString(Math.Round(tank.Data.LossesPercentage, 2), 2) + @"%)
														</td>
													</tr>
													<tr>
														<td  class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_CONT_DRAWS", "DE", "Draws") + @"</strong>
														</td>
														<td  class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.Draws, 0) + " (" + WOTHelper.FormatNumberToString(Math.Round(tank.Data.DrawsPercentage, 2), 2) + @"%)
														</td>
													</tr>
													<tr>
														<td  class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_SURVIVED", "DE", "Survived") + @"</strong>
														</td>
														<td  class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.Survived, 0) + " (" + WOTHelper.FormatNumberToString(Math.Round((double)tank.Data.SurvivedPercentage, 2), 2) + @"%)
														</td>
													</tr>
													<tr>
														<td  class='td-1' colspan=2>
															<table class='b-gray-text' width=100%>
																<thead/>
																	<tr>
																		<td  class='td-1' align=center>" + Translations.TranslationGet("HTML_SHOTS", "DE", "Shots") + @"</td>
																		<td  class='td-1' align=center>" + Translations.TranslationGet("HTML_HITS", "DE", "Hits") + @"</td>
																		<td  class='td-1' align=center>" + Translations.TranslationGet("HTML_CONT_HITRATIO", "DE", "Accuracy") + @"</td>
																		<td  class='td-1' align=center>" + Translations.TranslationGet("HTML_MAXKILLS", "DE", "Max Kills") + @"</td>
																		<td  class='td-1' align=center>" + Translations.TranslationGet("HTML_COMP_KILLPERBATTLE", "DE", "Kills/Battles") + @"</td>
																		<td  class='td-1' align=center>" + Translations.TranslationGet("HTML_COMP_KILLPERDEATH", "DE", "Kills/Deaths") + @"</td>
																	</tr>
																<tbody>
																	<tr>
																		<td  class='td-1' align=center>" + WOTHelper.FormatNumberToString(tank.Data.Shots, 0) + @"</td>
																		<td  class='td-1' align=center>" + WOTHelper.FormatNumberToString(tank.Data.Hits, 0) + @"</td>
																		<td  class='td-1' align=center>" + WOTHelper.FormatNumberToString(Math.Round(tank.Data.Accuracy, 2), 2) + @" %</td>
																		<td class='td-1' align=Center>" + WOTHelper.FormatNumberToString(tank.Data.MaxFrags, 0) + @"</td>
																		<td align=Center> " + WOTHelper.FormatNumberToString(((double)tank.Data.Frags / (double)tank.Data.BattlesCount), 2) + @"
																		<td align=Center> " + WOTHelper.FormatNumberToString(((double)tank.Data.Frags / ((double)tank.Data.BattlesCount - (double)tank.Data.Survived)), 2) + @"
																	</tr>
																</tbody>
															</table>
														</td>
													</tr>
												</tbody>
											</table>
										</td>
										<td  class='td-1' valign=top>
											<table class='b-gray-text' width=100%>
												<thead>
												</thead>
												<tbody>
												   <tr>
														<td  class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_CONT_EFFICIENCY", "DE", "Efficiency") + @"</strong>
														</td>
														<td class='td-1' align=center>
															<strong>" + "" + EfficiencyDescription(tank.RatingEff) + " - " + WOTHelper.FormatNumberToString(Math.Round(tank.RatingEff, 2), 2) + @"</strong>
														</td>
													</tr>
												   <tr>
														<td  class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("STR_BR_Caption", "DE", "Battle Rating") + @"</strong>
														</td>
														<td class='td-1' align=center>
															<strong>" + "" + EfficiencyDescription(tank.RatingBR) + " - " + WOTHelper.FormatNumberToString(Math.Round(tank.RatingBR, 2), 2) + @"</strong>
														</td>
													</tr>
												   <tr>
														<td  class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("STR_WN7_Caption", "DE", "WN7") + @"</strong>
														</td>
														<td class='td-1' align=center>
															<strong>" + "" + WN7ColorScaleDescription(tank.RatingWN7) + " - " + WOTHelper.FormatNumberToString(Math.Round(tank.RatingWN7, 5), 2) + @"</strong>
														</td>
													</tr>
												   <tr>
														<td  class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("STR_WN8_Caption", "DE", "WN8") + @"</strong>
														</td>
														<td class='td-1' align=center>
															<strong>" + "" + WN8ColorScaleDescription(tank.RatingWN8) + " - " + WOTHelper.FormatNumberToString(Math.Round(tank.RatingWN8, 2), 2) + @"</strong>
														</td>
													</tr>
													<tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_CONT_KILLS", "DE", "Kills") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.Frags, 0) + @"
														</td>
													</tr>
													<tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_CONT_DMGCAUSED", "DE", "Damage Caused") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.DamageDealt, 0) + @"
														</td>
													</tr>
													<tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_CONT_CAPPOINTS", "DE", "Capture Points") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.CapturePoints, 0) + @"
														</td>
													</tr>
													<tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_CONT_DEFPOINTS", "DE", "Defense Points") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.DefencePoints, 0) + @"
														</td>
													</tr>
													<tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_CONT_DETECTED", "DE", "Detected") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.Spotted, 0) + @"
														</td>
													</tr>
													<tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_BATTLETIME", "DE", "Battle Time") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + FormatBattleTime(tank.Data.BattleLifeTime_Friendly) + @"
														</td>
													</tr>
												</tbody>
											</table>                                        
										</td>
								  </tr> 
								  <tr>
										<td class='td-1' valign=top>
											<table class='b-gray-text' width=100%>
												<thead>
												</thead>
												<tbody>
													<tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_EXPER", "DE", "Experience") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.Xp, 0) + @"
														</td>
													</tr>
													<tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_COMP_MAXEXP", "DE", "Maximum Experience") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.MaxXp, 0) + @"
														</td>
													</tr>
													<tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_COMP_AVGEXP", "DE", "Average Experience") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + WOTHelper.FormatNumberToString((tank.Data.Xp / (tank.Data.BattlesCount == 0 ? 1 : tank.Data.BattlesCount)), 2) + @"
														</td>
													</tr>
												</tbody>
											</table>
										</td>
										<td class='td-1' valign=top>
											<table class='b-gray-text' width=100%>
												<thead>
												</thead>
												<tbody>
												   <tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_CONT_DMGREC", "DE", "Damage Received") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.DamageReceived, 0) + @"
														</td>
													</tr>
													<tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_CONT_DMGRATIO", "DE", "Damage Ratio") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(tank.Data.DamageRatio, 2) + @"
														</td>
													</tr>
													<tr>
														<td class='td-1' align=center>    
															<strong>" + Translations.TranslationGet("HTML_COMP_AVGDAM", "DE", "Average Damage") + @"</strong>
														</td>
														<td class='td-1' align=center>
															" + WOTHelper.FormatNumberToString(Math.Round(tank.Data.AverageDamageDealt, 2), 2) + @"
														</td>
													</tr>
												</tbody>
											</table>                                        
										</td>
								  </tr>                                   
								<Tbody>
								</Table>

								 <Table class='b-gray-text' width=100%>
								<Thead>
									<tr>
										<th width = 5% align=center colspan=5>" + Translations.TranslationGet("HTML_TANKINFO_KILLBCLASS", "DE", "Kills By Class (Combined Battle Modes)") + @"</th>
									</tr>
									<tr>
									   <td class='td-1' align=center>" + Translations.TranslationGet("HTML_HEAD_VEHICLECLASS", "DE", "Vehicle Class") + @"</td>
									   <td class='td-1' align=center>" + Translations.TranslationGet("HTML_CONT_KILLS", "DE", "Kills") + @"</td>
								   </tr>
								</Thead>
								<Tbody>";
            using (TankTypeDescription tankTypes = new TankTypeDescription(_message))
            {
                foreach (KeyValuePair<string, string> tankType in tankTypes)
                {
                    html += @"<tr>
            													<td><strong> " + tankType.Value + @"<strong>
            													<td class='td-1' align=center>" + tank.FragList.Where(a => a.TankClass == tankType.Key).Sum(y => y.frags) + @"<td>
            												  </tr>";
                }
            }

            html += @"<Tbody>
								</Table>

								<Table class='b-gray-text' width=100%>
								<Thead>
									<tr>
										<th align=center colspan=10>" + Translations.TranslationGet("HTML_TANKINFO_KILLS", "DE", "Kills (Combined Battle Modes)") + @"</th>
									</tr>
								</Thead>
								<Tbody> ";
            int x = 0;
            foreach (FragCount item in tank.FragList.OrderByDescending(y => y.Tier))
            {
                if (x == 0)
                {
                    html += "<tr>";
                }

                html += String.Format(@"<td class='" + CountryFlag(item.CountryID) + @" td-armory-icon'>
																		<div class='wrapper'>
																			<span class='level'><a class='b-gray-text'>" + GetRoman(item.Tier) + @"</a></span>
																			<a>" + TankImage(item.CountryID, item.TankID, item.Tank_Description) + @"</a>
																		</div>
																	</td>
																	<td valign=left>{0} : <strong>{1}</strong></td>", item.Tank_Description, item.frags);
                x++;

                if (x == 5)
                {
                    html += "</tr>";
                    x = 0;
                }
            }

            html += @"<Tbody>
								</Table>";

            return html;
        }
    }
}

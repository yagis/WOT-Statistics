using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace WOTStatistics.Core
{

     public partial class WOTHtml : WOTHtmlBase
	{

         public WOTHtml(MessageQueue message)
             : base(message)
         {
            
         }

         public string GetGlobalHTML(string type, WOTStats stats, WOTStats prevStats, WOTStatsDelta delta)
		{
			if (type == "Global")
                return String.Format(@"{0}{1}<head>{3}</head><body class='l-div-content' onload='initToolTips()'>{2}</body></HTMl>", GetStyleSheet(), Environment.NewLine, GlobalHTML(stats, prevStats, delta), String.Format(@"<SCRIPT type='text/javascript'  SRC='{0}'></SCRIPT>", WOTHelper.GetCustomScript("tooltips.js")));
			else if (type == "Tanks")
				return String.Format(@"{0}{1}<body class='l-div-content'>{2}</body></HTMl>", GetStyleSheet(), Environment.NewLine, TanksHTML(stats, delta));
			else if (type == "KillCounts")
				return String.Format(@"{0}{1}<body class='l-div-content'>{2}</body></HTMl>", GetStyleSheet(), Environment.NewLine, KillCounts(stats, delta));
			else if (type == "KillSummary")
				return String.Format(@"{0}{1}<body class='l-div-content'>{2}</body></HTMl>", GetStyleSheet(), Environment.NewLine, KillSummary(stats));
			else if (type == "Achievements")
				return String.Format(@"{0}{1}<body class='l-div-content'>{2}</body></HTMl>", GetStyleSheet(), Environment.NewLine, AchievementsHTML(stats));
			else
				return "";
		}

		public string GetGlobalHTML(string type, Tank tank)
		{
			if (type == "TankInfo")
				return String.Format(@"{0}{1}<body class='l-div-content'>{2}</body></HTMl>", GetStyleSheet(), Environment.NewLine, TankInfo(tank));
			else
				return "";
		}

		public string GetGlobalHTML(string type, string playerName)
		{
			if (type == "LastPlayedTanks")
			{
				
			   string scripts = @"<head> 
							<meta http-equiv='Content-Type' content='text/html; charset=UTF-8'/> 
							<style type=text/css>
								tr:hover
									{
										background-color:black;
									 }
							 th:hover
									{
										text-decoration:underline;
										color:yellow;
									 }
							</style>";
				scripts += String.Format(@"<SCRIPT type='text/javascript'  SRC='{0}'></SCRIPT>", WOTHelper.GetCustomScript("sorttable.js")) + Environment.NewLine;
                scripts += String.Format(@"<SCRIPT type='text/javascript'  SRC='{0}'></SCRIPT>", WOTHelper.GetCustomScript("tooltips.js")) + Environment.NewLine;
				scripts += @"</head>";

				return String.Format(@"{0}{1}{3}<body class='l-div-content' onload='sorttable.init(), initToolTips()'>{2}</body></HTMl>", GetStyleSheet(), Environment.NewLine, RecentBattles(playerName), scripts);
			}
			else
				return "";
		}

		public string GetGlobalHTML(string type, CustomGrouping customGrouping, WOTStats stats, WOTStats PrevStats, WOTStatsDelta delta)
		{
			if (type == "CustomGroupings") 
				return String.Format(@"{0}{1}<body class='l-div-content'>{2}</body></HTMl>", GetStyleSheet(), Environment.NewLine, CustomGrouping(customGrouping, stats, PrevStats, delta));
			else
				return "";
		}

		public string GetGlobalHTML(string type, string playerID, WOTStats stats, WOTStats PrevStats, WOTStatsDelta delta)
		{

			if (type == "KillCounts")
				return String.Format(@"{0}{1}<body class='l-div-content'>{2}</body></HTMl>", GetStyleSheet(), Environment.NewLine, KillCounts(stats, delta));
			else
				return "";
		}

		public string Blank()
		{
			return String.Format(@"{0}{1}<body class='l-div-content'>{2}</body></HTMl>", GetStyleSheet(), Environment.NewLine, @"<body>
																																	<div class='b-gray-text NoData' align=center valign=middle>
																																		Please go to Setup to add players.
																																	</div>
																																</body>");
		}

	

     

        

	

    


		public string CompareHTML(string PlayerA, string tankKeyA, WOTStats statsA, string PlayerB, string tankKeyB, WOTStats statsB)
		{
			int countryIDA = int.Parse(tankKeyA.Split('_')[0]);
			int countryIDB = int.Parse(tankKeyB.Split('_')[0]);

			int tankIDA = int.Parse(tankKeyA.Split('_')[1]);
			int tankIDB = int.Parse(tankKeyB.Split('_')[1]);

			Tank tankA = (from x in statsA.tanks
						  where x.CountryID == countryIDA && x.TankID == tankIDA
						  select x).FirstOrDefault();

			Tank tankB = (from y in statsB.tanks
						  where y.CountryID == countryIDB && y.TankID == tankIDB
						  select y).FirstOrDefault();

			string html = GetStyleSheet() + @"<body class='l-div-content'><Table width=100%>
								<Thead>
									<tr>
										<th class='b-gray-text' align=center colspan=2  width=50%><strong>" + PlayerA + @"</strong></th>
										<th class='b-gray-text' align=center colspan=2  width=50%><strong>" + PlayerB + @"</strong></th>
									</tr>
									<tr>
										<td class='" + CountryFlag(tankA.CountryID) + @" td-armory-icon' width=124px>
											<div class='wrapper'>
											<span class='level'><a class='b-gray-text'>" + GetRoman(tankA.Tier) + @"</a></span>
											 <a>" + TankImage(tankA.CountryID, tankA.TankID, tankA.Tank_Description) + @"</a>
											 </div>
										</td>
										<td class='b-gray-text' align=center>" + tankA.Tank_Description + @"</td>
									<td class='b-gray-text' align=center>" + tankB.Tank_Description + @"</td>
										<td class='" + CountryFlag(tankB.CountryID) + @" td-armory-icon' width=124px>
											<div class='wrapper'>
											<span class='level'><a class='b-gray-text'>" + GetRoman(tankB.Tier) + @"</a></span>
											<a>" + TankImage(tankB.CountryID, tankB.TankID, tankB.Tank_Description) + @"</a>
											 </div>
										 </td>
										
									</tr>
								</Thead>
								<Tbody class='b-gray-text'>
								   <tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_HEAD_BATTLES", "DE", "Battles") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.Data.BattlesCount, tankB.Data.BattlesCount, "", 0) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.Data.BattlesCount, tankA.Data.BattlesCount, "", 0) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_HEAD_BATTLES", "DE", "Battles") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>                           
									<tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_CONT_VICTORYPERC", "DE", "Victory Percentage") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.Data.VictoryPercentage, tankB.Data.VictoryPercentage, "%", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.Data.VictoryPercentage, tankA.Data.VictoryPercentage, "%", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_CONT_VICTORYPERC", "DE", "Victory Percentage") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>
									<tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_CONT_DEFEATPERC", "DE", "Defeat Percentage") + @"</strong></td>
														<td width=50% align=right>" + CompareLoss(tankA.Data.LossesPercentage, tankB.Data.LossesPercentage, "%", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareLoss(tankB.Data.LossesPercentage, tankA.Data.LossesPercentage, "%", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_CONT_DEFEATPERC", "DE", "Defeat Percentage") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>
								   <tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_CONT_DRAWPERC", "DE", "Draw Percentage") + @"</strong></td>
														<td width=50% align=right>" + CompareLoss(tankA.Data.DrawsPercentage, tankB.Data.DrawsPercentage, "%", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareLoss(tankB.Data.DrawsPercentage, tankA.Data.DrawsPercentage, "%", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_CONT_DRAWPERC", "DE", "Draw Percentage") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>

									<tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_CONT_SURVIVALPERC", "DE", "Survival Percentage") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.Data.SurvivedPercentage, tankB.Data.SurvivedPercentage, "%", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td> 
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.Data.SurvivedPercentage, tankA.Data.SurvivedPercentage, "%", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_CONT_SURVIVALPERC", "DE", "Survival Percentage") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>

									<tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_AVGEXP", "DE", "Average Experience") + @"</strong></td>
														<td width=50% align=right>" + CompareWin((double)tankA.Data.Xp / (double)tankA.Data.BattlesCount, (double)tankB.Data.Xp / (double)tankB.Data.BattlesCount, "", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td> 
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin((double)tankB.Data.Xp / (double)tankB.Data.BattlesCount, (double)tankA.Data.Xp / (double)tankA.Data.BattlesCount, "", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_AVGEXP", "DE", "Average Experience") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>


									<tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_MAXEXP", "DE", "Maximum Experience") + @"</strong></td>
														<td width=50% align=right>" + CompareWin((double)tankA.Data.MaxXp, (double)tankB.Data.MaxXp, "", 0) + @"</td>
													</tr>
												</tbody>
											</table>
										</td> 
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin((double)tankB.Data.MaxXp, (double)tankA.Data.MaxXp, "", 0) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_MAXEXP", "DE", "Maximum Experience") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>

								   <tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_HITRATIO", "DE", "Accuracy Percentage") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.Data.Accuracy, tankB.Data.Accuracy, "%", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.Data.Accuracy, tankA.Data.Accuracy, "%", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_HITRATIO", "DE", "Accuracy Percentage") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>
									
									 <tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_CONT_DMGRATIO", "DE", "Damage Ratio") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.Data.DamageRatio, tankB.Data.DamageRatio, "", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.Data.DamageRatio, tankA.Data.DamageRatio, "", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_CONT_DMGRATIO", "DE", "Damage Ratio") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>            
									
								   <tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_KILLPERBATTLE", "DE", "Kills/Battle") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.Data.KillperBattle, tankB.Data.KillperBattle, "", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.Data.KillperBattle, tankA.Data.KillperBattle, "", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_KILLPERBATTLE", "DE", "Kills/Battle") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>
									<tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_KILLPERDEATH", "DE", "Kills/Deaths") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.Data.KillperDeaths, tankB.Data.KillperDeaths, "", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.Data.KillperDeaths, tankA.Data.KillperDeaths, "", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_KILLPERDEATH", "DE", "Kills/Deaths") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>



									<tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("STR_WN7_Caption", "DE", "WN7") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.RatingWN7, tankB.RatingWN7, "", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.RatingWN7, tankA.RatingWN7, "", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("STR_WN7_Caption", "DE", "WN7") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>  

								   <tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_AVGDAM", "DE", "Average Damage") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.Data.AverageDamageDealt, tankB.Data.AverageDamageDealt, "", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.Data.AverageDamageDealt, tankA.Data.AverageDamageDealt, "", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_AVGDAM", "DE", "Average Damage") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>   

									<tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_AVGCAPPOINTS", "DE", "Average Capture Points") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.Data.AverageCapturePoints, tankB.Data.AverageCapturePoints, "", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.Data.AverageCapturePoints, tankA.Data.AverageCapturePoints, "", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_AVGCAPPOINTS", "DE", "Average Capture Points") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>        

								   <tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_AVGDEFPOINTS", "DE", "Average Defense Points") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.Data.AverageDefencePoints, tankB.Data.AverageDefencePoints, "", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.Data.AverageDefencePoints, tankA.Data.AverageDefencePoints, "", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_AVGDEFPOINTS", "DE", "Average Defense Points") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>         

								  <tr>
										<td/>
										<td>
											<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_AVGSPOTS", "DE", "Average Detected") + @"</strong></td>
														<td width=50% align=right>" + CompareWin(tankA.Data.AverageSpotted, tankB.Data.AverageSpotted, "", 2) + @"</td>
													</tr>
												</tbody>
											</table>
										</td>
										
										<td>
										<table width=100%>
												<thead/>
												<tbody> 
													<tr>
														<td width=50% align=Left>" + CompareWin(tankB.Data.AverageSpotted, tankA.Data.AverageSpotted, "", 2) + @"</td>
														<td width=50% align=center><strong>" + Translations.TranslationGet("HTML_COMP_AVGSPOTS", "DE", "Average Detected") + @"</strong></td>
													</tr>
												</tbody>
											</table>
										<td/>
										<td/>
								   </tr>     

									  




";


			html += @"</Tbody>
			</Table></body>";

			return html;
		}

		public string CompareWin(double valueA, double valueB, string sign, int decimalPlaces)
		{

			if (Math.Round(valueA, decimalPlaces) > Math.Round(valueB, decimalPlaces))
				return String.Format(@"<font color='{0}''>{1} {2}</font>", ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)), WOTHelper.FormatNumberToString(valueA, decimalPlaces), sign);
			else
				return String.Format("{0} {1}", WOTHelper.FormatNumberToString(valueA, decimalPlaces), sign);

		}

		public string CompareLoss(double valueA, double valueB, string sign, int decimalPlaces)
		{

			if (Math.Round(valueA, decimalPlaces) < Math.Round(valueB, decimalPlaces))
				return String.Format(@"<font color='{0}''>{1} {2}</font>", ColorTranslator.ToHtml(Color.FromArgb(UserSettings.ColorPositive)), WOTHelper.FormatNumberToString(valueA, decimalPlaces), sign);
			else
				return String.Format("{0} {1}", WOTHelper.FormatNumberToString(valueA, decimalPlaces), sign);

		}

		public string KillSummary(WOTStats stats)
		{
			Dictionary<string, FragCount> killList = new Dictionary<string, FragCount>();
			using (TankDescriptions tankD = new TankDescriptions(_message))
			{
				foreach (Tank tank in stats.tanks)
				{
					foreach (FragCount kill in tank.FragList.Where(x=>tankD.Active(x.CountryID, x.TankID) == true))
					{
						FragCount k = new FragCount(null) { Country_Description = kill.Country_Description, CountryID = kill.CountryID, frags = kill.frags, Tank_Description = kill.Tank_Description, TankClass = kill.TankClass, TankID = kill.TankID, Tier = kill.Tier };
						string key = string.Format("{0}_{1}", kill.CountryID, kill.TankID);
						if (!killList.ContainsKey(key))
							killList.Add(key, k);
						else
						{
							killList[key].frags += kill.frags;
						}
					}
				}
			}



			string html = @"<table width=100%><thead></thead><tbody><tr><td width=80% valign=top><Table width=100% >
								<Thead>
									<tr>
										<th class='b-gray-text' align=center colspan=3><strong>" + Translations.TranslationGet("HTML_CONT_KILLS", "DE", "Kills") + @"</strong></th>
									</tr>
								</Thead>
								<Tbody>";

			int tier = 0;
			int MaxKill = (from x in killList.Values
						   select x.frags).DefaultIfEmpty(0).Max();
			foreach (FragCount item in killList.Values.OrderByDescending(y => y.Tier).ThenByDescending(mk => mk.frags))
			{
				if (tier != item.Tier)
				{
					html += String.Format(@"<tr><td class='b-gray-text' colspan=3 align=center><strong>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + @" {0}</strong></td></tr>", item.Tier);
					tier = item.Tier;
				}

				html += @"
						  <tr>
							<td class='" + CountryFlag(item.CountryID) + @" td-armory-icon' >
								<div class='wrapper'>
									<span class='level'><a class='b-gray-text'>" + GetRoman(item.Tier) + @"</a></span>
									<a>" + TankImage(item.CountryID, item.TankID, item.Tank_Description) + @"</a>
								</div>
							</td>
							<td class='b-gray-text'>" + item.Tank_Description + @"</td>
							<td width=85%>
								<table bgColor=red width=" + Math.Round((((decimal)item.frags) / ((decimal)MaxKill)) * (decimal)100,0) + @"% cellspacing=0 cellpadding=0 border=0>
									<tr>
										<td class='td-1 b-gray-text ' align=right><font color=black>" + item.frags + @"</font></td>
									</tr>
								</table>
							</td>";
				html += @"</tr>";
			}

			html += @"</Tbody>
			</Table></td><td width=20% valign=top>
			<table width=100%>
				<thead>
				<tr>
				  <th class='b-gray-text' align=center colspan=2><strong>" + Translations.TranslationGet("HTML_MASTANKER", "DE", "Master Tanker") + @"</strong></th>
				</tr>
				</thead>
				<tbody>";

			int premiumIndicator = 0;
			int ordinal = 1;
			int currentCountry = -1;
			using (CountryDescriptions countryDescription = new CountryDescriptions(_message))
			{
				foreach (KeyValuePair<TankKey, TankValue> item in new TankDescriptions(_message).OrderBy(x => x.Value.Premium).ThenBy(z => z.Key.CountryID).ThenBy(y => y.Value.Tier).Where(x => x.Value.Active == true))
				//foreach (KeyValuePair<TankKey, TankValue> item in new TankDescriptions(_message).OrderBy(x => x.Value.Premium).ThenBy(y => y.Value.Tier).Where(x => x.Value.Active == true))
				{
					if (!killList.ContainsKey(item.Key.CountryID + "_" + item.Key.TankID))
					{
						
						if (premiumIndicator != item.Value.Premium)
						{
							html += @" <tr><td class='b-gray-text' colspan=3 align=center nowrap>" + Translations.TranslationGet("HTML_PREMDESC", "DE", "Premium tanks are not required<br/>for the Master Tanker achievement") + @"</td></tr>";
							premiumIndicator = item.Value.Premium;
							ordinal = 1;
						}

						if (currentCountry != item.Key.CountryID)
						{
							html += @" <tr><th class='b-gray-text' colspan=3 align=center nowrap>" + countryDescription.Description(item.Key.CountryID) + @"</th></tr>";
							currentCountry = item.Key.CountryID;
						}

						if (item.Value.Premium == 0)
						{
							html += @"<tr><td class='b-gray-text'>" + ordinal + @"</td>
											<td class='" + CountryFlag(item.Key.CountryID) + @" td-armory-icon' width=124px>
												<div class='wrapper'>
													<span class='level'><a class='b-gray-text'>" + GetRoman(item.Value.Tier) + @"</a></span>
													<a>" + TankImage(item.Key.CountryID, item.Key.TankID, item.Value.Description) + @"</a>
												</div>
											</td>
											<td class='b-gray-text'>" + item.Value.Description + @"</td>
										  </tr>";
							ordinal++;
						}
						else
						{
							html += @"<tr><td class='b-gray-text'>" + ordinal + @"</td>
											<td class='" + CountryFlag(item.Key.CountryID) + @" td-armory-icon' width=124px>
												<div class='wrapper'>
													<span class='level'><a class='b-gray-text'>" + GetRoman(item.Value.Tier) + @"</a></span>
													<a>" + TankImage(item.Key.CountryID, item.Key.TankID, item.Value.Description) + @"</a>
												</div>
											</td>
											<td ><font color='gold'>" + item.Value.Description + ((item.Value.Premium == 1) ? " <br>(Premium)" : "") + @"<font></td>
										  </tr>";
							ordinal++;
						}
					}
				}
			}

			html += @"    </tbody>
			</table></td></tr>
			</Tbody></table>";

			return html;
		}

	

		private string CustomGrouping(CustomGrouping customGrouping, WOTStats stats, WOTStats prevStats, WOTStatsDelta delta)
		{
			string html = @"<table class='b-gray-text' width=100%>
								<thead>
									<tr>
										<th>" + Translations.TranslationGet("HTML_GROUPNAME", "DE", "Group Name") + @"</th>
										<th>" + Translations.TranslationGet("HTML_HEAD_BATTLES", "DE", "Battles") + @"</th>
										<th/>
										<th>" + Translations.TranslationGet("HTML_CONT_VICTORIES", "DE", "Victories") + @"</th>
										<th/>
										<th>" + Translations.TranslationGet("HTML_CONT_DEFEATS", "DE", "Defeats") + @"</th>
										<th/>
										<th>" + Translations.TranslationGet("HTML_CONT_DRAWS", "DE", "Draws") + @"</th>
										<th/>
										<th>" + Translations.TranslationGet("HTML_CONT_KILLS", "DE", "Kills") + @"</th>
										<th/>
										<th>" + Translations.TranslationGet("HTML_CONT_DMGCAUSED", "DE", "Damage Dealt") + @"</th>
										<th/>
										<th>" + Translations.TranslationGet("HTML_CONT_DMGASSRADIO", "DE", "Damage Assisted Radio") + @"</th>
										<th/>
										<th>" + Translations.TranslationGet("HTML_CONT_DMGASSTRACK", "DE", "Damage Assisted Track") + @"</th>
										<th/>
                                        <th>" + Translations.TranslationGet("HTML_CONT_DMGREC", "DE", "Damage Received") + @"</th>
										<th/>
										<th>" + Translations.TranslationGet("HTML_CONT_DETECTED", "DE", "Detected") + @"</th>
										<th/>
										<th>" + Translations.TranslationGet("HTML_CONT_DEFPOINTS", "DE", "Defense Points") + @"</th>
										<th/>
										<th>" + Translations.TranslationGet("HTML_CONT_CAPPOINTS", "DE", "Capture Points") + @"</th>
                                        <th/>
                                        <th>" + Translations.TranslationGet("HTML_CONT_EFFICIENCY", "DE", "Efficiency") + @"</th>
                                        <th/>
                                        <th>" + Translations.TranslationGet("STR_BR_Caption", "DE", "Battle Rating") + @"</th>
                                        <th/>
                                        <th>" + Translations.TranslationGet("STR_WN7_Caption", "DE", "WN7") + @"</th>
                                        <th/>
                                        <th>" + Translations.TranslationGet("STR_WN8_Caption", "DE", "WN8") + @"</th>
										<th/>
									<tr>
								</thead>
								<tbody>";
			
			foreach (KeyValuePair<string, Tuple<string, string>> group in customGrouping)
			{
				List<string> gList = GroupList(group.Value.Item2);
				double tankCount = 0;
				double battleCount = 0;
				double victories = 0;
				double defeats = 0;
				double draws = 0;
				double kills = 0;
				double dt = 0;
				double dd = 0;
                double damageAssistedRadio = 0;
                double damageAssistedTrack = 0;
				double spotted = 0;
				double defencePoints = 0;
				double capturePoints = 0;
                double globalAvgTier = 0;
                double globalAvgDefence = 0;
                double RatingEff = 0;
                double RatingBR = 0;
                double RatingWN7 = 0;
                double RatingWN8 = 0;

				double battleCountDelta = 0;
				double victoriesDelta = 0;
				double victoriesDeltaPerc = 0;
				double defeatsDelta = 0;
				double defeatsDeltaPerc = 0;
				double drawsDelta = 0;
				double killsDelta = 0;
				double dtDelta = 0;
				double ddDelta = 0;
                double damageAssistedRadioDelta = 0;
                double damageAssistedTrackDelta = 0;
				double spottedDelta = 0;
				double defencePointsDelta = 0;
				double capturePointsDelta = 0;
                double globalAvgTierDelta = 0;
                double globalAvgDefenceDelta = 0;

                double RatingEffDelta = 0;
                double RatingBRDelta = 0;
                double RatingWN7Delta = 0;
                double RatingWN8Delta = 0;

				tankCount = gList.Count();
				double tier = 0;
				double DelTier = 0;
				string tankNames = "";
				foreach (string item in gList)
				{
					tankNames = tankNames + new TankDescriptions(_message).Description(int.Parse(item.Split('_')[0]), int.Parse(item.Split('_')[1])) + "; ";
					tier += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.BattlesCount) * stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Tier);
					battleCount += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.BattlesCount);
					victories += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Victories);
					defeats += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Defeats);
					draws += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Draws);
					kills += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Frags);
					dt += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.DamageReceived);
					dd += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.DamageDealt);
                    damageAssistedRadio += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.DamageAssistedRadio);
                    damageAssistedTrack += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.DamageAssistedTracks);
                    RatingEff += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.RatingEff);
                    RatingBR += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.RatingBR);
                    RatingWN7 += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.RatingWN7);
                    RatingWN8 += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.RatingWN8);

					spotted += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Spotted);
					defencePoints += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.DefencePoints);
					capturePoints += stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.CapturePoints);
                    globalAvgTier = stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Parent.Parent.AverageTier);
                    globalAvgDefence = stats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Parent.Parent.AverageDefencePoints);

					DelTier += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.BattlesCount) * prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Tier);
					battleCountDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.BattlesCount);
					victoriesDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Victories);
					victoriesDeltaPerc += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.VictoryPercentage);
					defeatsDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Defeats);
					defeatsDeltaPerc += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.LossesPercentage);
					drawsDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Draws);
					killsDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Frags);
					dtDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.DamageReceived);
					ddDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.DamageDealt);
                    damageAssistedRadioDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.DamageAssistedRadio);
                    damageAssistedTrackDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.DamageAssistedTracks);
					spottedDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Spotted);
					defencePointsDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.DefencePoints);
					capturePointsDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.CapturePoints);
                    globalAvgTierDelta = prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Parent.Parent.AverageTier);
                    globalAvgDefenceDelta = prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.Parent.Parent.AverageDefencePoints);

                    RatingEffDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.RatingEff);
                    RatingBRDelta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.RatingBR);
                    RatingWN7Delta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.RatingWN7);
                    RatingWN8Delta += prevStats.tanks.Where(x => x.CountryID + "_" + x.TankID == item).Sum(y => y.Data.RatingWN8);

				}



				//double efficiency = (kills * (350.0 - (Math.Round(tier / battleCount, 2)) * 20.0) + dd * (0.2 + 1.5 / (Math.Round(tier / battleCount, 2))) + 200.0 * spotted + 150.0 * defencePoints + 150.0 * capturePoints) / battleCount;
				//double delEfficiency = ((kills - killsDelta) * (350.0 - (Math.Round((tier - DelTier) / (battleCount - battleCountDelta), 2)) * 20.0) + (dd - ddDelta) * (0.2 + 1.5 / (Math.Round((tier - DelTier) / (battleCount - battleCountDelta), 2))) + 200.0 * (spotted - spottedDelta) + 150.0 * (defencePoints - defencePointsDelta) + 150.0 * (capturePoints - capturePointsDelta)) / (battleCount - battleCountDelta);

				//double efficiency = ((dd / battleCount) * (10 / (Math.Round(tier / battleCount, 2) + 2)) * (0.23 + 2 * Math.Round(tier / battleCount, 2) / 100)) + ((kills / battleCount) * 250) + ((spotted / battleCount) * 150) + ((Math.Log((capturePoints / battleCount) + 1, 1.732)) * 150) + ((defencePoints / battleCount) * 150);
               // double efficiency = -56;  //ScriptWrapper.GetEFFValue(battleCount, dd, Math.Round(tier / battleCount, 2), kills, spotted, capturePoints, defencePoints, (victories / battleCount) * 100, true, globalAvgTier, globalAvgDefence, 0, damageAssistedRadio, damageAssistedTrack);
				//double delEfficiency = ((ddDelta / battleCountDelta) * (10 / (Math.Round(DelTier / battleCountDelta, 2) + 2)) * (0.23 + 2 * Math.Round(DelTier / battleCountDelta, 2) / 100)) + ((killsDelta / battleCountDelta) * 250) + ((spottedDelta / battleCountDelta) * 150) + ((Math.Log((capturePointsDelta / battleCountDelta) + 1, 1.732)) * 150) + ((defencePointsDelta / battleCountDelta) * 150);
                //double delEfficiency = -55;  //ScriptWrapper.GetEFFValue(battleCountDelta, ddDelta, Math.Round(DelTier / battleCountDelta, 2), killsDelta, spottedDelta, capturePointsDelta, defencePointsDelta, (victoriesDelta / battleCountDelta) * 100, true, globalAvgTierDelta, globalAvgDefenceDelta, stats.BattlesCount, damageAssistedRadioDelta, damageAssistedTrackDelta);

				html += @"<tr>
							<td title='" + tankNames + @"'>" + group.Value.Item1 + @"</td>";

				html += @"<td class='td-1' align=right>" + WOTHelper.FormatNumberToString(battleCount, 0) + @"</td>
							<td class='td-1' align=right>" + GetDelta(battleCount - battleCountDelta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + victories.ToString("###,###,##0.##") + @" <span style='white-space: nowrap;'>(" + WOTHelper.FormatNumberToString(Math.Round((victories / battleCount) * 100, 2), 2) + "%)</span>" + @"</td>
							<td  class='td-1' align=right>" + GetDelta(victories - victoriesDelta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(defeats, 0) + @" <span style='white-space: nowrap;'>(" + WOTHelper.FormatNumberToString(Math.Round((defeats / battleCount) * 100, 2), 2) + "%)</span>" + @"</td>
						<td  class='td-1' align=right>" + GetDelta(defeats - defeatsDelta, "", 0, true) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(draws, 0) + @" <span style='white-space: nowrap;'>(" + WOTHelper.FormatNumberToString(Math.Round(draws / battleCount * 100, 2), 2) + "%)</span>" + @"</td>
						<td  class='td-1' align=right>" + GetDelta(draws - drawsDelta, "", 0, true) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(kills, 0) + @"</td>
						<td  class='td-1' align=right>" + GetDelta(kills - killsDelta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(dd, 0) + @"</td>
						<td  class='td-1' align=right>" + GetDelta(dd - ddDelta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(damageAssistedRadio, 0) + @"</td>
						<td  class='td-1' align=right>" + GetDelta(damageAssistedRadio - damageAssistedRadioDelta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(damageAssistedTrack, 0) + @"</td>
						<td  class='td-1' align=right>" + GetDelta(damageAssistedTrack - damageAssistedTrackDelta, "", 0) + @"</td>
                         <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(dt, 0) + @"</td>
						<td  class='td-1' align=right>" + GetDelta(dt - dtDelta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(spotted, 0) + @"</td>
						<td  class='td-1' align=right>" + GetDelta(spotted - spottedDelta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(defencePoints, 0) + @"</td>
							<td  class='td-1' align=right>" + GetDelta(defencePoints - defencePointsDelta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(capturePoints, 0) + @"</td>
						<td  class='td-1' align=right>" + GetDelta(capturePoints- capturePointsDelta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(RatingEff, 0) + @"</td>
						<td  class='td-1' align=right>" + GetDelta(RatingEff - RatingEffDelta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(RatingBR, 0) + @"</td>
						<td  class='td-1' align=right>" + GetDelta(RatingBR - RatingBRDelta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(RatingWN7, 0) + @"</td>
						<td  class='td-1' align=right>" + GetDelta(RatingWN7 - RatingWN7Delta, "", 0) + @"</td>
						 <td  class='td-1' align=right>" + WOTHelper.FormatNumberToString(RatingWN8, 0) + @"</td>
						<td  class='td-1' align=right>" + GetDelta(RatingWN8 - RatingWN8Delta, "", 0) + @"</td>
				</tr>";
			}


			html += @"</tbody>
						</table>";

			return html;
		}

		private double GetDifference(double valueA, double valueB, double battlesA, double battlesB)
		{
			double currentPerc = (valueA / battlesA) * 100;
			double PreviousPerv = ((valueA - valueB) / (battlesA - battlesB)) * 100;
			return currentPerc - PreviousPerv;
		}

		private List<string> GroupList(string text)
		{
			return text.Split('|').ToList<string>();
		}

		

		private string FormatBattleTime(TimeSpan ts)
		{
			string retValue = "";

			if (ts.Days == 0)
				retValue = string.Format(@"{0:hh\:mm\:ss}", ts);
			else
				if (ts.Days == 1)
					retValue = string.Format(@"{0} " + Translations.TranslationGet("HTML_CONT_DAY", "DE", "Day") + @" {1:hh\:mm\:ss}", ts.Days, ts);
				else
					retValue = string.Format(@"{0} " + Translations.TranslationGet("HTML_CONT_DAYS", "DE", "Days") + @" {1:hh\:mm\:ss}", ts.Days, ts);


			return retValue;
		}



		private string GetStyleSheet()
		{

#if DEBUG
        string sBackground = "white";
#else
            string sBackground = "black";
#endif  

			return @"<!DOCTYPE HTML PUBLIC '-//W3C//DTD HTML 4.01//EN' 'http://www.w3.org/TR/html4/strict.dtd'>
<HTML>
 <style type=text/css>
  table
	  {
		border:1px solid #000;
		border-collapse: collapse;
		empty-cells: show;
	  }
	
  td 
	  { 
		 border-width: 1px; 
		 border-style: solid dotted; 
		 font-family:Arial, Helvetica, sans-serif;
		 font-size:" + UserSettings.HTMLCellFont + @"px;
		 background-repeat:no-repeat;
	  }

.toolTip {
		   font-family: Arial, Helvetica, sans-serif;
		   font-size: 8pt;
			z-index=-1;
		   /*filter:alpha(opacity=80);
		   -moz-opacity: 0.8;
		   opacity: 0.8;*/
		   /* comment the above 3 line if you don't want transparency*/
   }



  td.td-1 
	  { 
		 width:auto;
	  }

	.bg_usa 
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/2.png);
		background-position: 0 0;
		background-repeat: no-repeat;
	}

	.bg_usafill 
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/2.png);
		background-position: 0 0;
		background-repeat: no-repeat;  
		background-size: cover  ;  
	}

   .bg_uk 
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/5.png);
		background-position: 0 0;
		background-repeat: no-repeat;
	}

	.bg_ukfill 
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/5.png);
		background-position: 0 0;
		background-repeat: no-repeat;  
		background-size: cover  ;  
	}

	.bg_ussr
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/0.png);
		background-position: 0 0;
		background-repeat: no-repeat;
	}

	.bg_ussrfill 
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/0.png);
		background-position: 0 0;
		background-repeat: no-repeat;
		background-size: cover  ; 
	}

	.bg_germany
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/1.png);
		background-position: 0 0;
		background-repeat: no-repeat;
	}

	.bg_germanyfill 
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/1.png);
		background-position: 0 0;
		background-repeat: no-repeat;
		background-size: cover  ;    
	}

	.bg_french
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/4.png);
		background-position: 0 0;
		background-repeat: no-repeat;
	}

	.bg_frenchfill 
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/4.png);
		background-position: 0 0;
		background-repeat: no-repeat;
		background-size: cover  ;     
	}

	.bg_china
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/3.png);
		background-position: 0 0;
		background-repeat: no-repeat;
	}

	.bg_chinafill 
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/3.png);
		background-position: 0 0;
		background-repeat: no-repeat;
		background-size: cover  ;    
	}

    .bg_japan
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/6.png);
		background-position: 0 0;
		background-repeat: no-repeat;
	}

	.bg_japanfill 
	{
		background-image:url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/Countries/6.png);
		background-position: 0 0;
		background-repeat: no-repeat;
		background-size: cover  ;    
	}

	.b-gray-text 
		{
			color: #CED9D9;
		}

    .recentbattlesheading {
        text-align:center;
        font-family:WarHeliosCondC, Arial, Helvetica, sans-serif;
		font-size:" + (int.Parse(UserSettings.HTMLHeaderFont)+2) + @"px;
		font-weight:bold;
        margin:0;
     }

	.td-armory-icon
	{
			padding-bottom: 0;
			padding-left: 0;
			padding-right: 0;
			padding-top: 0;
			text-align: center;
			width: 124px;
	}

	.td-armory-icon .wrapper 
	  {
			height: 31px;
			overflow: hidden;
			position: relative;
			padding-bottom: 0;
			padding-left: 40px;
			padding-right: 0;
			padding-top: 0;
			width: 84px;
		}

	.l-div-content 
	{
        background-image: url(file://" + WOTHelper.GetEXEPath().Replace(@"\", @"/") + @"/Images/background/ui-bg-tile.jpg); 
        background: " + sBackground + @";
	}

	.td-armory-icon .level 
		{
			position: absolute;
			top: 0;
			left: 12px;
		}

   .MiddleCenter 
		{
			display: table-cell;
			vertical-align: middle;
			text-align: center;
		}

 tfoot 
	{
		background-color: transparent;
		border-width: 1px; 
		border-style: solid; 
		font-family:WarHeliosCondC, Arial, Helvetica, sans-serif;
		font-size:" + (int.Parse(UserSettings.HTMLCellFont) + 2) + @"px;
		font-weight:bold;

	}

  th 
	{
		background-color: transparent;
		cursor:pointer;
		border-width: 1px; 
		border-style: solid; 
		font-family:WarHeliosCondC, Arial, Helvetica, sans-serif;
		font-size:" + UserSettings.HTMLHeaderFont + @"px;

	}

	.th1 
	{
		background-color: transparent;
		cursor:auto;
		border-width: 1px; 
		border-style: solid; 
		font-family:WarHeliosCondC, Arial, Helvetica, sans-serif;
		font-size:" + UserSettings.HTMLHeaderFont + @"px;

	}

.NoData
	{
		background-color: transparent;
		font-family:WarHeliosCondC, Arial, Helvetica, sans-serif;
		font-size:" + UserSettings.HTMLHeaderFont + @"px;

	}

   a:visited 
	{
		color:#CED9D9;
		text-decoration: none;
		
	}
	 
	a:link 
	{
		color:#CED9D9;
		text-decoration: none;
		
	}
	
	a:hover 
	{
		color:#CED9D9;
		text-decoration: underline;
	}

thead td 
	{
		background-color: transparent;
		cursor:pointer;
		border-width: 1px; 
		border-style: solid; 
		font-family:WarHeliosCondC, Arial, Helvetica, sans-serif;
		font-size:" + UserSettings.HTMLHeaderFont + @"px;

	}" + Environment.NewLine + @"
		</style>";
		}

	

	

		private TimeSpan TotalBattleTime(WOTStats stats)
		{
			int tBt = (from x in stats.tanks
					   select x.Data.BattleLifeTime).Sum();

			DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0);
			DateTime endDate = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(tBt);
			TimeSpan timeSpan = endDate - startDate;
			return timeSpan.Duration();

		}

		private TimeSpan AverageBattleTime(WOTStats stats)
		{
			int tBt = (from x in stats.tanks
					   select x.Data.BattleLifeTime).Sum();
			int bc = (from x in stats.tanks
					  select x.Data.BattlesCount).Sum();

			try
			{
				DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0);
				DateTime endDate = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(tBt / bc);
				TimeSpan timeSpan = endDate - startDate;
				return timeSpan.Duration();
			}
			catch
			{
				return new TimeSpan(0);
			}


		}

		private TimeSpan TotalBattleTimeDelta(WOTStatsDelta stats)
		{
			int tBt = (from x in stats.tanks
					   select x.Data.BattleLifeTime).Sum();

			DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0);
			DateTime endDate = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(tBt);
			TimeSpan timeSpan = endDate - startDate;
			return timeSpan.Duration();

		}

		private TimeSpan AverageBattleTimeDelta(WOTStatsDelta delta)
		{

			int dtBt = (from x in delta.tanks
						select x.Data.BattleLifeTime).Sum();
			int dbc = (from x in delta.tanks
					   select x.Data.BattlesCount).Sum();

			DateTime startDate = new DateTime(1970, 1, 1, 0, 0, 0);
			DateTime endDate = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds((dtBt) / (dbc));
			TimeSpan timeSpan = endDate - startDate;
			return timeSpan.Duration();

		}

	

		private string EfficiencyDescription(double value)
		{
			if (value < 600)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass2)) + "'>" + Translations.TranslationGet("HTML_EFF_BAD", "DE", "Bad") + @"</font>";
			else if (value >= 600 && value < 900)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass3)) + "'>" + Translations.TranslationGet("HTML_EFF_BELAVG", "DE", "Below average") + @"</font>";
			else if (value >= 900 && value < 1200)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass4)) + "'>" + Translations.TranslationGet("HTML_EFF_AVG", "DE", "Average") + @"</font>";
			else if (value >= 1200 && value < 1500)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass5)) + "'>" + Translations.TranslationGet("HTML_EFF_GOOD", "DE", "Good") + @"</font>";
			else if (value >= 1500 && value < 1800)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass7)) + "'>" + Translations.TranslationGet("HTML_EFF_GREAT", "DE", "Great") + @"</font>";
			else if (value >= 1800)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass8)) + "'>" + Translations.TranslationGet("HTML_EFF_UNICUM", "DE", "Unicum") + @"</font>";
			else
				return "";
		}

        public static string WN7ColorScaleDescription(double value)
        {
            if (value < 500)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass1)) + "'>" + Translations.TranslationGet("HTML_EFF_VERYBAD", "DE", "Very Bad") + @"</font>";
            else if (value >= 500 && value < 700)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass2)) + "'>" + Translations.TranslationGet("HTML_EFF_BAD", "DE", "Bad") + @"</font>";
            else if (value >= 700 && value < 900)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass3)) + "'>" + Translations.TranslationGet("HTML_EFF_BELAVG", "DE", "Below Average") + @"</font>";
            else if (value >= 900 && value < 1100)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass4)) + "'>" + Translations.TranslationGet("HTML_EFF_AVG", "DE", "Average") + @"</font>";
            else if (value >= 1100 && value < 1350)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass5)) + "'>" + Translations.TranslationGet("HTML_EFF_GOOD", "DE", "Good") + @"</font>";
            else if (value >= 1350 && value < 1550)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass6)) + "'>" + Translations.TranslationGet("HTML_EFF_VERYGOOD", "DE", "Very Good") + @"</font>";
            else if (value >= 1550 && value < 1850)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass7)) + "'>" + Translations.TranslationGet("HTML_EFF_GREAT", "DE", "Great") + @"</font>";
            else if (value >= 1850 && value < 2050)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass8)) + "'>" + Translations.TranslationGet("HTML_EFF_UNICUM", "DE", "Unicum") + @"</font>";
            else if (value >= 2050)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass9)) + "'>" + Translations.TranslationGet("HTML_EFF_SUPERUNICUM", "DE", "Super Unicum") + @"</font>";
            else
                return "";
        }

        public static string WN8ColorScaleDescription(double value)
        {
            if (value < 300)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass1)) + "'>" + Translations.TranslationGet("HTML_EFF_VERYBAD", "DE", "Very Bad") + @"</font>";
            else if (value >= 300 && value < 600)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass2)) + "'>" + Translations.TranslationGet("HTML_EFF_BAD", "DE", "Bad") + @"</font>";
            else if (value >= 600 && value < 900)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass3)) + "'>" + Translations.TranslationGet("HTML_EFF_BELAVG", "DE", "Below Average") + @"</font>";
            else if (value >= 900 && value < 1250)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass4)) + "'>" + Translations.TranslationGet("HTML_EFF_AVG", "DE", "Average") + @"</font>";
            else if (value >= 1250 && value < 1600)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass5)) + "'>" + Translations.TranslationGet("HTML_EFF_GOOD", "DE", "Good") + @"</font>";
            else if (value >= 1600 && value < 1900)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass6)) + "'>" + Translations.TranslationGet("HTML_EFF_VERYGOOD", "DE", "Very Good") + @"</font>";
            else if (value >= 1900 && value < 2350)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass7)) + "'>" + Translations.TranslationGet("HTML_EFF_GREAT", "DE", "Great") + @"</font>";
            else if (value >= 2350 && value < 2900)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass8)) + "'>" + Translations.TranslationGet("HTML_EFF_UNICUM", "DE", "Unicum") + @"</font>";
            else if (value >= 2900)
                return "<Font color='" + ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass9)) + "'>" + Translations.TranslationGet("HTML_EFF_SUPERUNICUM", "DE", "Super Unicum") + @"</font>";
            else
                return "";
        }

        public static string WN8ColorScale(double value)
        {
            if (value < 300)
                return ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass1));
            else if (value >= 300 && value < 600)
                return ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass2));
            else if (value >= 600 && value < 900)
                return ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass3));
            else if (value >= 900 && value < 1250)
                return ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass4));
            else if (value >= 1250 && value < 1600)
                return ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass5));
            else if (value >= 1600 && value < 1900)
                return ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass6));
            else if (value >= 1900 && value < 2350)
                return ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass7));
            else if (value >= 2350 && value < 2900)
                return ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass8));
            else if (value >= 2900)
                return ColorTranslator.ToHtml(Color.FromArgb(UserSettings.colorWNClass9));
            else
                return "";
        }
	

	
	

		public string TanksHTML(WOTStats stats, WOTStatsDelta delta)
		{

			string html = @"<head> 

<style type=text/css>
	tr:hover
		{
			background-color:black;
		 }
	th:hover
		{
			text-decoration:underline;
			color:yellow;
		 }
	}
</style>

		</script>" + String.Format(@"<SCRIPT SRC='{0}'></SCRIPT>", WOTHelper.GetCustomScript("sorttable.js")) + Environment.NewLine + @"</head>
								<table class='sortable' Border=0 width=100%>
								<thead>
									<tr>
										<!--<th class='b-gray-text' align=center><strong>" + Translations.TranslationGet("HTML_HEAD_COUNTRYID", "DE", "Country ID") + @"</strong></th>
										<th class='b-gray-text' align=center><strong>" + Translations.TranslationGet("HTML_HEAD_TANKID", "DE", "Tank ID") + @"</strong></th>-->
										<th class='b-gray-text'  align=center><strong>" + Translations.TranslationGet("HTML_HEAD_TIER", "DE", "Tier") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong>" + Translations.TranslationGet("HTML_HEAD_TANK", "DE", "Tank") + @"</strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_HEAD_BATTLES", "DE", "Battles") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_CONT_VICTORIES", "DE", "Victories") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_CONT_DEFEATS", "DE", "Defeats") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_CONT_DRAWS", "DE", "Draws") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_CONT_KILLS", "DE", "Kills") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_CONT_DMGCAUSED", "DE", "Damage Caused") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_CONT_DMGREC", "DE", "Damage Received") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th> 
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_CONT_DMGRATIO", "DE", "Damage Ratio") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_CONT_CAPPOINTS", "DE", "Capture Points") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_CONT_DEFPOINTS", "DE", "Defence Points") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_CONT_DETECTED", "DE", "Detected") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("HTML_CONT_EFFICIENCY", "DE", "Efficiency") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("STR_BR_Caption", "DE", "Battle Rating") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
										<th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("STR_WN7_Caption", "DE", "WN7") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
                                        <th class='b-gray-text '  align=center><strong>" + Translations.TranslationGet("STR_WN8_Caption", "DE", "WN8") + @"</strong></th>
										<th class='b-gray-text'  align=center><strong></strong></th>
									</tr>
								</thead>
								<tbody>
								   
								   ";
			double battleCount = 0;
			double battleCountDelta = 0;
			double victories = 0;
			double victoriesDelta = 0;
			double losses = 0;
			double lossesDelta = 0;
			double draws = 0;
			double drawsDelta = 0;
			double kills = 0;
			double killsDelta = 0;
			double damageRatioDelta = 0;
			double damageDealtDelta = 0;
			double damageReceivedDelta = 0;
			double capturePointsDelta = 0;
			double defencePointsDelta = 0;
			double spottedDelta = 0;
			double RatingEffDelta = 0;
            double RatingBRDelta = 0;
            double RatingWN7Delta = 0;
            double RatingWN8Delta = 0;
			foreach (Tank tank in stats.tanks.OrderByDescending(x => x.Data.Last_Time_Played_Friendly))
			{
                //WOTHelper.AddToLog("TANK " + tank.CountryID + "_" + tank.TankID);

				battleCount = tank.Data.BattlesCount;
				battleCountDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.BattlesCount);
				victories = tank.Data.Victories;
				victoriesDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.Victories);
				losses = tank.Data.Defeats;
				lossesDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.Defeats);
				draws = tank.Data.Draws;
				drawsDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.Draws);
				kills = tank.Data.Frags;
				killsDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.Frags);
				damageDealtDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.DamageDealt);
				damageReceivedDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.DamageReceived);
				damageRatioDelta = delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.DamageRatio);
				RatingEffDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.RatingEff);
                RatingBRDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.RatingBR);
                RatingWN7Delta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.RatingWN7);
                RatingWN8Delta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.RatingWN8);
				capturePointsDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.CapturePoints);
				defencePointsDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.DefencePoints);
				spottedDelta = (double)delta.tanks.Where(x => x.CountryID == tank.CountryID && x.TankID == tank.TankID).Sum(y => y.Data.Spotted);
				html += String.Format(@"<tr>
											<!--<td class='td-1 b-gray-text'>{10}</td>
											<td class='td-1 b-gray-text'>{11}</td>-->
											<td  sorttable_customkey='" + tank.Tier + @"' class='" + CountryFlag(tank.CountryID) + @" td-armory-icon'  >
												<div class='wrapper'>
													<span class='level'><a class='b-gray-text'>" + GetRoman(tank.Tier) + @"</a></span>
													<a>" + TankImage(tank.CountryID, tank.TankID, tank.Tank_Description) + @"</a>
												</div>
											 </td>
											<td class='td-1 b-gray-text' valign='middle'><a href='h' onclick='window.external.Redirect(""" + tank.CountryID + "_" + tank.TankID + @""")'>{1}</a></td>
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(battleCount, 2) * 100 + @"' width=auto align=right >{4}</td> <!-- no of battles  -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(battleCountDelta, 2) * 100 + @"' width=auto align=right >{5}</td> <!-- Battle Delta  -->
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(victories, 2) * 100 + @"' width=auto align=right >{6}</td> <!-- Victories  -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(victoriesDelta, 2) * 100 + @"' width=auto align=right >{7}</td>  <!-- Victories delta  -->
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(losses, 2) * 100 + @"' width=auto align=right >{8}</td> <!-- Losses  -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(lossesDelta, 2) * 100 + @"' width=auto align=right >{9}</td>  <!-- Losses Delta  -->
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(draws, 2) * 100 + @"' width=auto align=right >{10}</td> <!-- draws  -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(drawsDelta, 2) * 100 + @"' width=auto align=right >{11}</td> <!-- draws Delta  -->
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(kills, 2) * 100 + @"' width=auto align=right >{12}</td> <!-- Kills  -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(killsDelta, 2) * 100 + @"' width=auto align=right >{13}</td> <!-- Kills delta  -->
											

											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round((double)tank.Data.DamageDealt, 2) * 100 + @"'  width=auto align=right >{14}</td> <!--  Damage Caused -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(damageDealtDelta, 2) * 100 + @"' width=auto align=right >{15}</td> <!-- Damage Caused delta  -->
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round((double)tank.Data.DamageReceived, 2) * 100 + @"' width=auto align=right >{16}</td>  <!-- Damage Received  -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(damageReceivedDelta, 2) * 100 + @"' width=auto align=right >{17}</td>  <!-- Damage Received delta  --> 
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round((double)tank.Data.DamageRatio, 2) * 100 + @"' width=auto align=right >{18}</td> <!-- Damage Ratio  -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(damageRatioDelta, 2) * 100 + @"' width=auto align=right >{19}</td> <!-- Damage Ratio delta  -->
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round((double)tank.Data.CapturePoints, 2) * 100 + @"' width=auto align=right >{20}</td> <!-- Capture points   -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(capturePointsDelta, 2) * 100 + @"' width=auto align=right >{21}</td> <!-- Capture points delta  -->
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round((double)tank.Data.DefencePoints, 2) * 100 + @"' width=auto align=right >{22}</td> <!-- Defence points   -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(defencePointsDelta, 2) * 100 + @"' width=auto align=right >{23}</td> <!-- Defence points delta  -->
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round((double)tank.Data.Spotted, 2) * 100 + @"' width=auto align=right >{24}</td> <!-- Enemy spotted   -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(spottedDelta, 2) * 100 + @"' width=auto align=right >{25}</td> <!-- Enemy spotted  delta  -->
											

											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round((double)tank.Data.RatingEff, 2) * 100 + @"' width=auto align=right >{26}</td> <!-- Efficiency  -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(RatingEffDelta, 2) * 100 + @"' width=auto align=right >{27}</td> <!--  Efficiency delta -->
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round((double)tank.Data.RatingBR, 2) * 100 + @"' width=auto align=right >{28}</td> <!-- BR  -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(RatingBRDelta, 2) * 100 + @"' width=auto align=right >{29}</td> <!--  BR delta -->
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round((double)tank.Data.RatingWN7, 2) * 100 + @"' width=auto align=right >{30}</td> <!-- WN7 -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(RatingWN7Delta, 2) * 100 + @"' width=auto align=right >{31}</td> <!--  WN7 delta -->
											
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round((double)tank.Data.RatingWN8, 2) * 100 + @"' width=auto align=right >{32}</td> <!-- WN8 -->
											<td class='td-1 b-gray-text' sorttable_customkey='" + Math.Round(RatingWN8Delta, 2) * 100 + @"' width=auto align=right >{33}</td> <!--  WN8 delta -->


</tr>",
							tank.Tier,
							tank.Tank_Description,
                            tank.CountryID,
                            tank.TankID,
                            WOTHelper.FormatNumberToString(battleCount, 0),
                                GetDelta(Math.Round(battleCountDelta, 2), "", 0),
							WOTHelper.FormatNumberToString(victories, 0),
                                GetDelta(Math.Round(victoriesDelta, 2), "", 0),
							WOTHelper.FormatNumberToString(losses, 0),
                                GetDelta(Math.Round(lossesDelta, 2), "", 0, true),
							WOTHelper.FormatNumberToString(draws, 0),
                             GetDelta(Math.Round(drawsDelta, 2), "", 0, true),
							WOTHelper.FormatNumberToString(tank.Data.Frags, 0),
                             GetDelta(Math.Round(killsDelta, 2), "", 0),
							WOTHelper.FormatNumberToString(tank.Data.DamageDealt, 0),
                                GetDelta(Math.Round(damageDealtDelta, 2), "", 0),
							WOTHelper.FormatNumberToString(tank.Data.DamageReceived, 0),
                                GetDelta(Math.Round(damageReceivedDelta, 2), "", 0),
							WOTHelper.FormatNumberToString(Math.Round(tank.Data.DamageRatio, 2), 2),
                                GetDelta(Math.Round(damageRatioDelta, 2), "", 2),
                            WOTHelper.FormatNumberToString(tank.Data.CapturePoints, 0),
							    GetDelta(Math.Round(capturePointsDelta, 2), "", 0),
						    WOTHelper.FormatNumberToString(tank.Data.DefencePoints, 0),
							    GetDelta(Math.Round(defencePointsDelta, 2), "", 0),
						    WOTHelper.FormatNumberToString(tank.Data.Spotted, 0),
							    GetDelta(Math.Round(spottedDelta, 2), "", 0),
						    WOTHelper.FormatNumberToString(Math.Round(tank.RatingEff, 2), 0),
                                GetDelta(Math.Round(RatingEffDelta / battleCountDelta, 2), "", 0),
                            WOTHelper.FormatNumberToString(Math.Round(tank.RatingBR, 2), 0),
                                GetDelta(Math.Round(RatingBRDelta / battleCountDelta, 2), "", 0),
                            WOTHelper.FormatNumberToString(Math.Round(tank.RatingWN7, 2), 0),
                                GetDelta(Math.Round(RatingWN7Delta / battleCountDelta, 2), "", 0),
                            WOTHelper.FormatNumberToString(Math.Round(tank.RatingWN8, 2), 0),
                                GetDelta(Math.Round(RatingWN8Delta / battleCountDelta, 2), "", 0));
			}

			html += @"
								</tbody> 

								</table>";
			return html;
		}

		public string KillCounts(WOTStats stats, WOTStatsDelta delta)
		{
			bool showTierTotals = UserSettings.KillCountsShowTierTotals;
			bool showColoumnTotals = UserSettings.KillCountsShowColumnTotals;
			bool showRowTotals = UserSettings.KillCountsShowRowTotals;
			string html = @"<head> 

<style type=text/css>
	 tr:hover
		{
			background-color:black;
		 }
</style>

		</script>
			</head><table width=100%>
						<thead>
							<tr>
								<th></th>
								<th></th>
								<th class='b-gray-text' colspan=5>I</th>
								<th class='b-gray-text'  colspan=5>II</th>
								<th class='b-gray-text' colspan=5>III</th>
								<th class='b-gray-text' colspan=5>IV</th>
								<th class='b-gray-text' colspan=5>V</th>
								<th class='b-gray-text' colspan=5>VI</th>
								<th class='b-gray-text' colspan=5>VII</th>
								<th class='b-gray-text' colspan=5>VIII</th>
								<th class='b-gray-text' colspan=5>IX</th>
								<th class='b-gray-text' colspan=5>X</th>"
								+ (showRowTotals == true ? @"<th class='b-gray-text' colspan=5>" + Translations.TranslationGet("HTML_TOTAL", "DE", "Total") + @"</th>" : "") + @"
								
							</tr>
							<tr>";
			html += @"<td></td> <td></td>"
						+ KillCountClassHeaders()
						+ KillCountClassHeaders()
						+ KillCountClassHeaders()
						+ KillCountClassHeaders()
						+ KillCountClassHeaders()
						+ KillCountClassHeaders()
						+ KillCountClassHeaders()
						+ KillCountClassHeaders()
						+ KillCountClassHeaders()
						+ KillCountClassHeaders();

			html += @"</tr>
						</thead>
						<tbody >
							";





			foreach (Tank tank in stats.tanks.OrderByDescending(x => x.Tier))
			{
				html += @"<tr class='row-wrapper'>"
							+ @"<td class='" + CountryFlag(tank.CountryID) + @" td-armory-icon' rowspan=" + (showTierTotals == true ? "2" : "1") + @">
								<div class='wrapper'>
									<span class='level'><a class='b-gray-text'>" + GetRoman(tank.Tier) + @"</a></span>
									<a>" + TankImage(tank.CountryID, tank.TankID, tank.Tank_Description) + @"</a>
								</div>
							</td>"
										 + @"<td class='b-gray-text'  rowspan=" + (showTierTotals == true ? "2" : "1") + @"><a href='h' onclick='window.external.Redirect(""" + tank.CountryID + "_" + tank.TankID + @""")'>" + tank.Tank_Description + @"</a></td> "
							+ KillCountTierDetail(1, tank, showTierTotals)
							+ KillCountTierDetail(2, tank, showTierTotals)
							+ KillCountTierDetail(3, tank, showTierTotals)
							+ KillCountTierDetail(4, tank, showTierTotals)
							+ KillCountTierDetail(5, tank, showTierTotals)
							+ KillCountTierDetail(6, tank, showTierTotals)
							+ KillCountTierDetail(7, tank, showTierTotals)
							+ KillCountTierDetail(8, tank, showTierTotals)
							+ KillCountTierDetail(9, tank, showTierTotals)
							+ KillCountTierDetail(10, tank, showTierTotals)
							+ (showTierTotals == true ? (showRowTotals == true ? @"<td class='td-1 b-gray-text' align=center rowspan=" + (showTierTotals == true ? "2" : "1") + @"><strong>" + KillCountTextColor(tank.FragList.Sum(y => y.frags)) + @"</strong></td>" : "") : "")
							+ (showTierTotals == true ? "</tr><tr>" : "")

							+ KillCountClassDetail(1, tank)
							+ KillCountClassDetail(2, tank)
							+ KillCountClassDetail(3, tank)
							+ KillCountClassDetail(4, tank)
							+ KillCountClassDetail(5, tank)
							+ KillCountClassDetail(6, tank)
							+ KillCountClassDetail(7, tank)
							+ KillCountClassDetail(8, tank)
							+ KillCountClassDetail(9, tank)
							+ KillCountClassDetail(10, tank)
							+ (showTierTotals == true ? "" : (showRowTotals == true ? @"<td class='td-1 b-gray-text' align=center rowspan=" + (showTierTotals == true ? "2" : "1") + @"><strong>" + KillCountTextColor(tank.FragList.Sum(y => y.frags)) + @"</strong></td>" : ""))
						   + @"</tr>";
			}


			if (showColoumnTotals == true)
			{
				html += @" 
							<tr><td class='td-1 b-gray-text' align=center colspan=2><strong>" + Translations.TranslationGet("HTML_TOTAL", "DE", "Total") + @"</strong></td>"
							+ @"<td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(stats.tanks.Sum(x => x.FragList.Where(y => y.Tier == 1).Sum(z => z.frags))) + @"</strong></td>"
							+ @"<td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(stats.tanks.Sum(x => x.FragList.Where(y => y.Tier == 2).Sum(z => z.frags))) + @"</strong></td>"
							+ @"<td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(stats.tanks.Sum(x => x.FragList.Where(y => y.Tier == 3).Sum(z => z.frags))) + @"</strong></td>"
							+ @"<td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(stats.tanks.Sum(x => x.FragList.Where(y => y.Tier == 4).Sum(z => z.frags))) + @"</strong></td>"
							+ @"<td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(stats.tanks.Sum(x => x.FragList.Where(y => y.Tier == 5).Sum(z => z.frags))) + @"</strong></td>"
							+ @"<td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(stats.tanks.Sum(x => x.FragList.Where(y => y.Tier == 6).Sum(z => z.frags))) + @"</strong></td>"
							+ @"<td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(stats.tanks.Sum(x => x.FragList.Where(y => y.Tier == 7).Sum(z => z.frags))) + @"</strong></td>"
							+ @"<td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(stats.tanks.Sum(x => x.FragList.Where(y => y.Tier == 8).Sum(z => z.frags))) + @"</strong></td>"
							+ @"<td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(stats.tanks.Sum(x => x.FragList.Where(y => y.Tier == 9).Sum(z => z.frags))) + @"</strong></td>"
							+ @"<td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(stats.tanks.Sum(x => x.FragList.Where(y => y.Tier == 10).Sum(z => z.frags))) + @"</strong></td>"
							+ @"<td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(stats.tanks.Sum(x => x.FragList.Sum(z => z.frags))) + @"</strong></td>"
						+ @"<tr>";
			}



			html += @"<tbody>
					</table>";

			return html;
		}

		private string KillCountClassHeaders()
		{
			string html = @" 
										
											<td class='b-gray-text' >" + Translations.TranslationGet("LT_SHORT", "DE", "LT") + @"</td>
											<td class='b-gray-text'>" + Translations.TranslationGet("MT_SHORT", "DE", "MT") + @"</td>
											<td class='b-gray-text'>" + Translations.TranslationGet("HT_SHORT", "DE", "HT") + @"</td>
											<td class='b-gray-text'>" + Translations.TranslationGet("TD_SHORT", "DE", "TD") + @"</td>
											<td class='b-gray-text'>" + Translations.TranslationGet("SPG_SHORT", "DE", "SPG") + @"</td>
										";
			return html;
		}

		private string KillCountClassDetail(int tier, Tank tank)
		{
			string html = @" 

										   <td class='td-1 b-gray-text' align=center>" + KillCountTextColor(tank.FragList.Where(x => x.Tier == tier && x.TankClass == "LT").Sum(y => y.frags)) + @"</td>
											<td class='td-1 b-gray-text' align=center>" + KillCountTextColor(tank.FragList.Where(x => x.Tier == tier && x.TankClass == "MT").Sum(y => y.frags)) + @"</td>
											<td class='td-1 b-gray-text' align=center>" + KillCountTextColor(tank.FragList.Where(x => x.Tier == tier && x.TankClass == "HT").Sum(y => y.frags)) + @"</td>
											<td class='td-1 b-gray-text' align=center>" + KillCountTextColor(tank.FragList.Where(x => x.Tier == tier && x.TankClass == "TD").Sum(y => y.frags)) + @"</td>
											<td class='td-1 b-gray-text' align=center>" + KillCountTextColor(tank.FragList.Where(x => x.Tier == tier && x.TankClass == "SPG").Sum(y => y.frags)) + @"</td>
							
										";

			return html;
		}

		private string KillCountTierDetail(int tier, Tank tank, bool showTierTotals)
		{
			if (showTierTotals == true)
				return @" <td class='td-1 b-gray-text' align=center colspan=5><strong>" + KillCountTextColor(tank.FragList.Where(x => x.Tier == tier).Sum(y => y.frags)) + @"</strong></td>";
			else
				return "";


		}

		private string KillCountTextColor(int value)
		{
			if (value > 0)
			{
				return value.ToString();
			}
			else
				return "";
		}
	}
}



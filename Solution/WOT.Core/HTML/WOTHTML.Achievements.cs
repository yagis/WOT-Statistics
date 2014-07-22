using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WOTStatistics.Core
{
    partial class WOTHtml : WOTHtmlBase
    {
        public static string AchievementsHTML(WOTStats stats)
        {
            string html = @"<table class='b-gray-text' width='100%'><tbody>";
            using (Achievements ach = new Achievements())
            {
                foreach (KeyValuePair<int, Achievement> item in ach.OrderBy(x => x.Key))
                {
                    int total = 0;
                    int completed = 0;
                    foreach (string tank in item.Value.Tanks)
                    {
                        total += (from x in stats.tanks.Select(h => h.FragList.Where(f => f.CountryID + "_" + f.TankID == tank).Select(g => g.frags).Sum())
                                  select x).Sum();
                    }

                    while (total >= item.Value.Value)
                    {
                        total -= item.Value.Value;
                        completed++;
                    }
                    html += @"<tr><th class='th1' align='center' width=100% colspan='2'>" + item.Value.Name + @"</th></tr>
										   <tr><th class='th1' align='center' width=100% colspan='2'>" + Translations.TranslationGet("STR_ACHIEVED", "DE", "Achieved: ") + completed + @"</th> </tr>";
                    html += @"<tr><td width=100%><table width=100%><tr><td align='center' width=" + Math.Round((decimal)total / (decimal)item.Value.Value * (decimal)100, 0) + "% bgColor=green><font><strong>" + (total == 0 ? "" : total.ToString()) + @"</strong></font></td>";
                    html += @"<td align='center' width=" + ((decimal)100 - Math.Round((decimal)total / (decimal)item.Value.Value * (decimal)100, 0)) + "% bgColor=red><font ><strong>" + ((item.Value.Value - total) == 0 ? "" : (item.Value.Value - total).ToString()) + @"</strong></font></td></tr></table></td></tr>";
                }
            }
            html += @"</tbody></table>";
            return html;
        }
    }
}

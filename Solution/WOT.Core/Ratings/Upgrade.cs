using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WOTStatistics.Core
{
    class Upgrade
    {


//        IDBHelpers db = new DBHelpers(dbPath, true)

//        string sqlUpdateRatings = @"SELECT * FROM RecentBattles WHERE ratingWN8=0";

//                using (DataTable dt = dbHelpers.GetDataTable(sqlUpdateRatings))
//                {
//                    foreach (DataRow item in dt.Rows)
//                    {


//                        WOTStatistics.SQLite.RatingStructure ratingStruct = new RatingStructure();
//                        ratingStruct.WN8ExpectedTankList = WN8ExpectedTankList;
//                        ratingStruct.countryID = item.CountryID;
//                        ratingStruct.tankID = item.TankID;
//                        ratingStruct.tier = item.Tier;
//                        ratingStruct.globalTier = item.GlobalAvgTier;

//                        ratingStruct.singleTank = true;

//                        ratingStruct.battlesCount = Convert.ToInt32(item.Battles);
//                        ratingStruct.battlesCount8_8 = Convert.ToInt32(item.Battles88);
//                        ratingStruct.capturePoints = item.CapturePoints;
//                        ratingStruct.defencePoints = item.DefencePoints;

//                        ratingStruct.damageAssistedRadio = item.DamageAssistedRadio;
//                        ratingStruct.damageAssistedTracks = item.DamageAssistedTracks;
//                        ratingStruct.damageDealt = item.DamageDealt;
//                        ratingStruct.frags = item.Kills;
//                        ratingStruct.spotted = item.Spotted;

//                        ratingStruct.winRate = (item.Victory / item.Battles) * 100;
//                        ratingStruct.globalWinRate = item.GlobalWinPercentage;


//                        lastbattle.Add("rbRatingEff", WOTStatistics.Core.WOTHelper.FormatNumberToString(WOTStatistics.Core.Ratings.GetRatingEff(ratingStruct), 2));
//                        lastbattle.Add("rbRatingBR", WOTStatistics.Core.WOTHelper.FormatNumberToString(WOTStatistics.Core.Ratings.GetRatingBR(ratingStruct), 2));
//                        lastbattle.Add("rbRatingWN7", WOTStatistics.Core.WOTHelper.FormatNumberToString(WOTStatistics.Core.Ratings.GetRatingWN7(ratingStruct), 2));

//                        WOTStatistics.Core.Ratings.WN8Storage WN8 = WOTStatistics.Core.Ratings.GetRatingWN8(ratingStruct);




//                        string sql = @"UPDATE RecentBattles
//                            SET
//                                    rsUEDateFrom = (SELECT ifnull(min(RecentBattles.rbBattleTime), datetime('now', 'unixepoch'))
//                                                        FROM RecentBattles
//                                                        WHERE RecentBattles.rbSessionID = RecentBattles_Session.rsKey )
//                                , rsUEDateTo = (SELECT ifnull(Max(RecentBattles.rbBattleTime), datetime('now', 'unixepoch'))
//                                                        FROM RecentBattles
//                                                        WHERE RecentBattles.rbSessionID = RecentBattles_Session.rsKey )
//                            WHERE
//                                EXISTS (
//                                    SELECT *
//                                    FROM RecentBattles
//                                    WHERE RecentBattles.rbSessionID = RecentBattles_Session.rsKey
//                                )";
//                        dbHelpers.ExecuteNonQuery(sql);


//                    }
//                }



    }
}

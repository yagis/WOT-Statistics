using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json.Linq;
using WOTStatistics.SQLite;

namespace WOTStatistics.Core
{

    public class MemoryTables : IDisposable
    {
        DataSet ds = new DataSet();
        string _dbPath;

        public MemoryTables(string dbPath)
        {
            _dbPath = dbPath;
            using (IDBHelpers db = new DBHelpers(dbPath, true))
            {
                DataTable dt = db.GetDataTable("select * from Files where 0 = 1");
                dt.TableName = "Files";
                ds.Tables.Add(dt);

                dt = db.GetDataTable("select * from File_Achievements where 0 = 1");
                dt.TableName = "File_Achievements";
                ds.Tables.Add(dt);

                dt = db.GetDataTable("select * from File_Battles where 0 = 1");
                dt.TableName = "File_Battles";
                ds.Tables.Add(dt);

                dt = db.GetDataTable("select * from File_Clan where 0 = 1");
                dt.TableName = "File_Clan";
                ds.Tables.Add(dt);

                dt = db.GetDataTable("select * from File_Historical where 0 = 1");
                dt.TableName = "File_Historical";
                ds.Tables.Add(dt);

                dt = db.GetDataTable("select * from File_Company where 0 = 1");
                dt.TableName = "File_Company";
                ds.Tables.Add(dt);

                dt = db.GetDataTable("select * from File_FragList where 0 = 1");
                dt.TableName = "File_FragList";
                ds.Tables.Add(dt);

                dt = db.GetDataTable("select * from File_TankDetails where 0 = 1");
                dt.TableName = "File_TankDetails";
                ds.Tables.Add(dt);

                dt = db.GetDataTable("select * from File_Total where 0 = 1");
                dt.TableName = "File_Total";
                ds.Tables.Add(dt);
            }


        }
        public static WN8ExpValues WN8ExpectedTankList = new WN8ExpValues();
        public void Fill(dynamic file, int fileName)
        {
            if (file == null)
            {
                WOTHelper.AddToLog("File cannot be read: " + fileName);
                return;
            }

            using (DataTable tabFile = ds.Tables["Files"])
            {
                DataRow row = tabFile.NewRow();
                row["fiID"] = fileName;
                row["fiResult"] = GetSafeValue(file.header.result);
                row["fiUserName"] = GetSafeValue(file.header.username);
                row["fiTankCount"] = GetSafeValue(file.header.tankcount);
                row["fiParser"] = GetSafeValue(file.header.parser);
                row["fiServer"] = GetSafeValue(file.header.server);
                row["fiMessage"] = GetSafeValue(file.header.message);
                row["fiDate"] = GetSafeValue(file.header.date);
                row["fiParserVersion"] = GetSafeValue(file.header.parserversion);
                tabFile.Rows.Add(row);
            }

            int tempTankID = 0;
            using (IDBHelpers db = new DBHelpers(_dbPath, false))
            {
                string id = db.ExecuteScalar("select max(cmID) + 1 nextID from File_TankDetails");
                if (!string.IsNullOrEmpty(id))
                    tempTankID = int.Parse(id);
                else
                    tempTankID = 1;
            }

            

            foreach (var item in file.tanks)
            {
                foreach (var i in item)
                {
                    if (ObjectExists(i, "common"))
                    {
                        using (DataTable tab = ds.Tables["File_TankDetails"])
                        {
                            DataRow dr = tab.NewRow();
                            dr["cmID"] = tempTankID;
                            dr["cmFileID"] = fileName;
                            dr["cmCreationTimeR"] = i.common.creationTimeR;
                            dr["cmFrags"] = i.common.frags;
                            dr["cmTankTitle"] = i.common.tanktitle;
                            dr["cmPremium"] = i.common.premium;
                            dr["cmUpdated"] = i.common.updated;
                            dr["cmTier"] = i.common.tier;
                            dr["cmUpdatedR"] = i.common.updatedR;
                            dr["cmLastBattleTime"] = i.common.lastBattleTime;
                            dr["cmFragsCompare"] = i.common.frags_compare;
                            dr["cmLastBattleTimeR"] = i.common.lastBattleTimeR;
                            dr["cmBaseVersion"] = i.common.basedonversion;
                            dr["cmCreationTime"] = i.common.creationTime;
                            dr["cmCompactDescription"] = i.common.compactDescr;
                            dr["cmCountryID"] = i.common.countryid;
                            dr["cmTankID"] = i.common.tankid;
                            dr["cmType"] = i.common.type;
                            tab.Rows.Add(dr);
                        }
                    }

                    if (ObjectExists(i, "clan"))
                    {
                        using (DataTable tab = ds.Tables["File_Clan"])
                        {
                            DataRow dr = tab.NewRow();
                            dr["clParentID"] = tempTankID;
                            dr["clBattlesCount"] = i.clan.battlesCount;
                            dr["clDefencePoints"] = i.clan.droppedCapturePoints;
                            dr["clFrags"] = i.clan.frags;
                            dr["clSpotted"] = i.clan.spotted;
                            dr["clDamageDealt"] = i.clan.damageDealt;
                            dr["clShots"] = i.clan.shots;
                            dr["clWins"] = i.clan.wins;
                            dr["clDamageReceived"] = i.clan.damageReceived;
                            dr["clLosses"] = i.clan.losses;
                            dr["clXP"] = i.clan.xp;
                            dr["clSurvivedBattles"] = i.clan.survivedBattles;
                            dr["clHits"] = i.clan.hits;
                            dr["clCapturePoints"] = i.clan.capturePoints;
                            tab.Rows.Add(dr);
                        }
                    }



                    if (ObjectExists(i, "company"))
                    {
                        using (DataTable tab = ds.Tables["File_Company"])
                        {
                            DataRow dr = tab.NewRow();
                            dr["fcParentID"] = tempTankID;
                            dr["fcBattlesCount"] = GetSafeValue(i.company.battlesCount);
                            dr["fcDefencePoints"] = GetSafeValue(i.company.droppedCapturePoints);
                            dr["fcFrags"] = GetSafeValue(i.company.frags);
                            dr["fcSpotted"] = GetSafeValue(i.company.spotted);
                            dr["fcDamageDealt"] = GetSafeValue(i.company.damageDealt);
                            dr["fcShots"] = GetSafeValue(i.company.shots);
                            dr["fcWins"] = GetSafeValue(i.company.wins);
                            dr["fcDamageReceived"] = GetSafeValue(i.company.damageReceived);
                            dr["fcLosses"] = GetSafeValue(i.company.losses);
                            dr["fcXP"] = GetSafeValue(i.company.xp);
                            dr["fcSurvivedBattles"] = GetSafeValue(i.company.survivedBattles);
                            dr["fcHits"] = GetSafeValue(i.company.hits);
                            dr["fcCapturePoints"] = GetSafeValue(i.company.capturePoints);
                            tab.Rows.Add(dr);
                        }
                    }


                    using (DataTable tab = ds.Tables["File_Achievements"])
                    {
                        DataRow dr = tab.NewRow();
                        dr["faParentID"] = tempTankID;
                        dr["faAlaric"] = GetSafeValue(GetSafeValue(i.special.alaric));
                        dr["faArmorPiercer"] = GetSafeValue(i.special.armorPiercer);
                        dr["faBattleHeroes"] = GetSafeValue(i.battle.battleHeroes);
                        dr["faBeastHunter"] = GetSafeValue(i.special.beasthunter);
                        dr["faBombardier"] = GetSafeValue(i.special.bombardier);
                        dr["faDefender"] = GetSafeValue(i.battle.defender);
                        dr["faDieHard"] = GetSafeValue(i.special.diehard);
                        dr["faDieHardSeries"] = GetSafeValue(i.series.diehardSeries);
                        dr["faEveilEye"] = GetSafeValue(i.battle.evileye);
                        dr["faFragsBeast"] = GetSafeValue(i.tankdata.fragsBeast);
                        dr["faFragsPatton"] = GetSafeValue(i.special.fragsPatton);
                        dr["faFragsSinai"] = GetSafeValue(i.battle.fragsSinai);
                        dr["faHandOfDeath"] = GetSafeValue(i.special.handOfDeath);
                        dr["faHeroesOfRasseney"] = GetSafeValue(i.special.heroesOfRassenay);
                        dr["faHuntsman"] = GetSafeValue(i.special.huntsman);
                        dr["faInvader"] = GetSafeValue(i.battle.invader);
                        dr["faInvincible"] = GetSafeValue(i.special.invincible);
                        dr["faInvincibleSeries"] = GetSafeValue(i.series.invincibleSeries);
                        dr["faIronman"] = GetSafeValue(i.special.ironMan);
                        dr["faKamikaze"] = GetSafeValue(i.special.kamikaze);
                        dr["faKillingSeries"] = GetSafeValue(i.series.killingSeries);
                        dr["faLuckyDevil"] = GetSafeValue(i.special.luckyDevil);
                        dr["faLumberJack"] = GetSafeValue(i.special.lumberjack);
                        dr["faMarkOfMastery"] = GetSafeValue(i.special.markOfMastery);
                        dr["faMaxDieHardSeries"] = GetSafeValue(i.series.maxDiehardSeries);
                        dr["faMaxInvincibleSeries"] = GetSafeValue(i.series.maxInvincibleSeries);
                        dr["faMaxKillingSeries"] = GetSafeValue(i.series.maxKillingSeries);
                        dr["faMaxPiercingSeries"] = GetSafeValue(i.series.maxPiercingSeries);
                        dr["faMaxSniperSeries"] = GetSafeValue(i.series.maxSniperSeries);
                        dr["faMedalAbrams"] = GetSafeValue(i.major.medalAbrams);
                        dr["faMedalBillotte"] = GetSafeValue(i.epic.medalBillotte);
                        dr["faMedalBrothersInArms"] = GetSafeValue(i.epic.medalBrothersInArms);
                        dr["faMedalBrunoPietro"] = GetSafeValue(i.epic.medalBrunoPietro);
                        dr["faMedalBurda"] = GetSafeValue(i.epic.medalBurda);
                        dr["faMedalCarius"] = GetSafeValue(i.major.medalCarius);
                        dr["faMedalCrucialContribution"] = GetSafeValue(i.epic.medalCrucialContribution);
                        dr["faMedalDeLanglade"] = GetSafeValue(i.epic.medalDeLanglade);
                        dr["faMedalDumitru"] = GetSafeValue(i.epic.medalDumitru);
                        dr["faMedalEkins"] = GetSafeValue(i.major.medalEkins);
                        dr["faMedalFadin"] = GetSafeValue(i.epic.medalFadin);
                        dr["faMedalHalonen"] = GetSafeValue(i.epic.medalHalonen);
                        dr["faMedalKay"] = GetSafeValue(i.major.medalKay);
                        dr["faMedalKnispel"] = GetSafeValue(i.major.medalKnispel);
                        dr["faMedalKolobanov"] = GetSafeValue(i.epic.medalKolobanov);
                        dr["faMedalLafayettePool"] = GetSafeValue(i.epic.medalLafayettePool);
                        dr["faMedalLavrinenko"] = GetSafeValue(i.major.medalLavrinenko);
                        dr["faMedalLeClerc"] = GetSafeValue(i.major.medalLeClerc);
                        dr["faMedalLehvaslaiho"] = GetSafeValue(i.epic.medalLehvaslaiho);
                        dr["faMedalNikolas"] = GetSafeValue(i.epic.medalNikolas);
                        dr["faMedalOrlik"] = GetSafeValue(i.epic.medalOrlik);
                        dr["faMedalOskin"] = GetSafeValue(i.epic.medalOskin);
                        dr["faMedalPascucci"] = GetSafeValue(i.epic.medalPascucci);
                        dr["faMedalPoppel"] = GetSafeValue(i.major.medalPoppel);
                        dr["faMedalRadleyWalters"] = GetSafeValue(i.epic.medalRadleyWalters);
                        dr["faMedalTamadaYoshio"] = GetSafeValue(i.epic.medalTamadaYoshio);
                        dr["faMedalTarczay"] = GetSafeValue(i.epic.medalTarczay);
                        dr["faMedalWittmann"] = GetSafeValue(i.epic.medalWittmann);
                        dr["faMousebane"] = GetSafeValue(i.special.mousebane);
                        dr["faPattonValley"] = GetSafeValue(i.special.pattonValley);
                        dr["faPiercingSeries"] = GetSafeValue(i.series.piercingSeries);
                        dr["faRaider"] = GetSafeValue(i.special.raider);
                        dr["faScout"] = GetSafeValue(i.battle.scout);
                        dr["faSinai"] = GetSafeValue(i.special.sinai);
                        dr["faSniper"] = GetSafeValue(i.battle.sniper);
                        dr["faSniperSeries"] = GetSafeValue(i.series.sniperSeries);
                        dr["faSteelwall"] = GetSafeValue(i.battle.steelwall);
                        dr["faSturdy"] = GetSafeValue(i.special.sturdy);
                        dr["faSupporter"] = GetSafeValue(i.battle.supporter);
                        dr["faTankExpertStrg"] = GetSafeValue(i.special.tankExpertStrg);
                        dr["faTitleSniper"] = GetSafeValue(i.special.titleSniper);
                        dr["faWarrior"] = GetSafeValue(i.battle.warrior);
                        tab.Rows.Add(dr);
                    }


                    if (ObjectExists(i, "tankdata"))
                    {
                        using (DataTable tab = ds.Tables["File_Battles"])
                        {
                            DataRow dr = tab.NewRow();
                            dr["bpParentID"] = tempTankID;
                            dr["bpBattleCount"] = GetSafeValue(i.tankdata.battlesCount);
                            dr["bpFrags8P"] = GetSafeValue(i.tankdata.frags8p);
                            dr["bpDefencePoints"] = GetSafeValue(i.tankdata.droppedCapturePoints);
                            dr["bpFrags"] = GetSafeValue(i.tankdata.frags);
                            dr["bpWinAndSurvive"] = GetSafeValue(i.tankdata.winAndSurvived);
                            dr["bpSpotted"] = GetSafeValue(i.tankdata.spotted);
                            dr["bpDamageDealt"] = GetSafeValue(i.tankdata.damageDealt);
                            dr["bpXPBefore8_8"] = 0;
                            dr["bpShots"] = GetSafeValue(i.tankdata.shots);
                            dr["bpBattlesBefore8_8"] = 0;
                            dr["bpWins"] = GetSafeValue(i.tankdata.wins);
                            dr["bpDamageReceived"] = GetSafeValue(i.tankdata.damageReceived);
                            dr["bpLosses"] = GetSafeValue(i.tankdata.losses);
                            dr["bpXP"] = GetSafeValue(i.tankdata.xp);
                            dr["bpSurvivedBattles"] = GetSafeValue(i.tankdata.survivedBattles);
                            dr["bpHits"] = GetSafeValue(i.tankdata.hits);
                            dr["bpCapturePoints"] = GetSafeValue(i.tankdata.capturePoints);
                            dr["bpDamageAssistedTracks"] = 0;
                            dr["bpHEHitsReceived"] = 0;
                            dr["bpPierced"] = 0;
                            dr["bpShotsReceived"] = 0;
                            dr["bpNoDamageShotsReceived"] = 0;
                            dr["bpOriginalXP"] = 0;
                            dr["bpHEHits"] = 0;
                            dr["bpMaxXP"] = GetSafeValue(i.tankdata.maxXP);
                            dr["bpMaxFrags"] = GetSafeValue(i.tankdata.maxFrags);
                            dr["bpMaxDamage"] = 0;
                            dr["bpMileage"] = 0;
                            dr["bpBattleMode"] = 15;
                            RatingStructure ratingStruct = new RatingStructure();
                            ratingStruct.WN8ExpectedTankList = WN8ExpectedTankList;
                            ratingStruct.countryID = GetSafeValue(i.common.countryid);
                            ratingStruct.tankID = GetSafeValue(i.common.tankid);
                            ratingStruct.tier = GetSafeValue(i.common.tier);
                            ratingStruct.globalTier = ratingStruct.tier;

                            ratingStruct.singleTank = true;

                            ratingStruct.battlesCount = GetSafeValue(i.tankdata.battlesCount);
                            ratingStruct.battlesCount8_8 = 0;
                            ratingStruct.capturePoints = GetSafeValue(i.tankdata.capturePoints);
                            ratingStruct.defencePoints = GetSafeValue(i.tankdata.droppedCapturePoints);

                            ratingStruct.damageAssistedRadio = 0;
                            ratingStruct.damageAssistedTracks = 0;
                            ratingStruct.damageDealt = GetSafeValue(i.tankdata.damageDealt);
                            ratingStruct.frags = GetSafeValue(i.tankdata.frags);
                            ratingStruct.spotted = GetSafeValue(i.tankdata.spotted);
                            ratingStruct.wins = GetSafeValue(i.tankdata.wins);


                            //ratingStruct.globalWinRate = (GetSafeValue(i.tankdata.wins) / GetSafeValue(i.tankdata.battlesCount)) * 100;

                            //WOTStatistics.Core.Ratings.printRatingStruct(ratingStruct);

                            WOTStatistics.Core.Ratings.RatingStorage Eff = Ratings.GetRatingEff(ratingStruct);
                            WOTStatistics.Core.Ratings.RatingStorage BR = Ratings.GetRatingBR(ratingStruct);
                            WOTStatistics.Core.Ratings.RatingStorage WN7 = Ratings.GetRatingWN7(ratingStruct);

                            WOTStatistics.Core.Ratings.RatingStorage WN8 = Ratings.GetRatingWN8(ratingStruct);

                            dr["bpRatingEff"] = Eff.Value;
                            dr["bpRatingEffWeight"] = Eff.Weight;
                            dr["bpRatingBR"] = BR.Value;
                            dr["bpRatingBRWeight"] = BR.Weight;
                            dr["bpRatingWN7"] = WN7.Value;
                            dr["bpRatingWN7Weight"] = WN7.Weight;
                            dr["bpRatingWN8"] = WN8.Value;
                            dr["bpRatingWN8Weight"] = WN8.Weight;
                            dr["bpRatingVersion"] = WOTStatistics.Core.UserSettings.RatingVersion;
                            tab.Rows.Add(dr);
                        }
                    }

                    using (DataTable tab = ds.Tables["File_Total"])
                    {
                        DataRow dr = tab.NewRow();
                        dr["foParentID"] = tempTankID;
                        dr["foCreationTime"] = i.common.creationTime;
                        dr["foMileage"] = Convert.ToInt32(i.common.mileage);
                        dr["foTreesCut"] = i.tankdata.treesCut;
                        dr["foLastBattleTime"] = i.tankdata.lastBattleTime;
                        dr["foBattleLifeTime"] = i.tankdata.battleLifeTime;
                        tab.Rows.Add(dr);
                    }

                    if (ObjectExists(i, "kills"))
                    {
                        foreach (var k in i.kills)
                        {
                            using (DataTable tab = ds.Tables["File_FragList"])
                            {
                                DataRow dr = tab.NewRow();
                                dr["fgParentID"] = tempTankID;
                                dr["fgCountryID"] = k[0];
                                dr["fgTankID"] = k[1];
                                dr["fgValue"] = k[2];
                                dr["fgTankDescription"] = k[3];
                                tab.Rows.Add(dr);
                            }
                        }
                    }
                }
                tempTankID++;
            }

            foreach (var item in file.tanks_v2)
            {
                foreach (var i in item)
                {
                    if (ObjectExists(i, "common"))
                    {
                        using (DataTable tab = ds.Tables["File_TankDetails"])
                        {
                            DataRow dr = tab.NewRow();
                            dr["cmID"] = tempTankID;
                            dr["cmFileID"] = fileName;
                            if (ObjectExists(i, "common"))
                            {
                                dr["cmCreationTimeR"] = i.common.creationTimeR;
                                dr["cmFrags"] = i.common.frags;
                                dr["cmTankTitle"] = i.common.tanktitle;
                                dr["cmPremium"] = i.common.premium;
                                dr["cmUpdated"] = i.common.updated;
                                dr["cmTier"] = i.common.tier;
                                dr["cmUpdatedR"] = i.common.updatedR;
                                dr["cmLastBattleTime"] = i.common.lastBattleTime;
                                dr["cmFragsCompare"] = i.common.frags_compare;
                                dr["cmLastBattleTimeR"] = i.common.lastBattleTimeR;
                                dr["cmBaseVersion"] = i.common.basedonversion;
                                dr["cmCreationTime"] = i.common.creationTime;
                                dr["cmCompactDescription"] = i.common.compactDescr;
                                dr["cmCountryID"] = i.common.countryid;
                                dr["cmTankID"] = i.common.tankid;
                                dr["cmType"] = i.common.type;
                                dr["cmHasClan"] = i.common.has_clan;
                                dr["cmHas7x7"] = i.common.has_7x7;
                                dr["cmHas15x15"] = i.common.has_15x15;
                                dr["cmHasCompany"] = i.common.has_company;
                            }
                            tab.Rows.Add(dr);
                        }
                    }

                    if (ObjectExists(i, "historical"))
                    {
                        using (DataTable tab = ds.Tables["File_Historical"])
                        {
                            DataRow dr = tab.NewRow();
                            dr["hsParentID"] = tempTankID;
                            dr["hsBattlesCount"] = i.historical.battlesCount;
                            dr["hsDefencePoints"] = i.historical.droppedCapturePoints;
                            dr["hsFrags"] = i.historical.frags;
                            dr["hsSpotted"] = i.historical.spotted;
                            dr["hsDamageDealt"] = i.historical.damageDealt;
                            dr["hsShots"] = i.historical.shots;
                            dr["hsWins"] = i.historical.wins;
                            dr["hsDamageReceived"] = i.historical.damageReceived;
                            dr["hsLosses"] = i.historical.losses;
                            dr["hsXP"] = i.historical.xp;
                            dr["hsSurvivedBattles"] = i.historical.survivedBattles;
                            dr["hsHits"] = i.historical.hits;
                            dr["hsCapturePoints"] = i.historical.capturePoints;
                            tab.Rows.Add(dr);
                        }
                    }


                    if (ObjectExists(i, "clan"))
                    {
                        using (DataTable tab = ds.Tables["File_Clan"])
                        {
                            DataRow dr = tab.NewRow();
                            dr["clParentID"] = tempTankID;
                            dr["clBattlesCount"] = i.clan.battlesCount;
                            dr["clDefencePoints"] = i.clan.droppedCapturePoints;
                            dr["clFrags"] = i.clan.frags;
                            dr["clSpotted"] = i.clan.spotted;
                            dr["clDamageDealt"] = i.clan.damageDealt;
                            dr["clXPBefore8_9"] = i.clan.xpBefore8_9;
                            dr["clShots"] = i.clan.shots;
                            dr["clBattlesCountBefore8_9"] = i.clan.battlesCountBefore8_9;
                            dr["clWins"] = i.clan.wins;
                            dr["clDamageReceived"] = i.clan.damageReceived;
                            dr["clLosses"] = i.clan.losses;
                            dr["clXP"] = i.clan.xp;
                            dr["clSurvivedBattles"] = i.clan.survivedBattles;
                            dr["clHits"] = i.clan.hits;
                            dr["clCapturePoints"] = i.clan.capturePoints;
                            tab.Rows.Add(dr);
                        }
                    }

                    if (ObjectExists(i, "company"))
                    {
                        using (DataTable tab = ds.Tables["File_Company"])
                        {
                            DataRow dr = tab.NewRow();
                            dr["fcParentID"] = tempTankID;
                            dr["fcBattlesCount"] = GetSafeValue(i.company.battlesCount);
                            dr["fcDefencePoints"] = GetSafeValue(i.company.droppedCapturePoints);
                            dr["fcFrags"] = GetSafeValue(i.company.frags);
                            dr["fcSpotted"] = GetSafeValue(i.company.spotted);
                            dr["fcDamageDealt"] = GetSafeValue(i.company.damageDealt);
                            dr["fcXPBefore8_9"] = GetSafeValue(i.company.xpBefore8_9);
                            dr["fcShots"] = GetSafeValue(i.company.shots);
                            dr["fcBattlesCountBefore8_9"] = GetSafeValue(i.company.battlesCountBefore8_9);
                            dr["fcWins"] = GetSafeValue(i.company.wins);
                            dr["fcDamageReceived"] = GetSafeValue(i.company.damageReceived);
                            dr["fcLosses"] = GetSafeValue(i.company.losses);
                            dr["fcXP"] = GetSafeValue(i.company.xp);
                            dr["fcSurvivedBattles"] = GetSafeValue(i.company.survivedBattles);
                            dr["fcHits"] = GetSafeValue(i.company.hits);
                            dr["fcCapturePoints"] = GetSafeValue(i.company.capturePoints);
                            tab.Rows.Add(dr);
                        }
                    }

                    if (ObjectExists(i, "achievements"))
                    {
                        using (DataTable tab = ds.Tables["File_Achievements"])
                        {
                            DataRow dr = tab.NewRow();
                            dr["faParentID"] = tempTankID;
                            dr["faAlaric"] = GetSafeValue(i.achievements.alaric);
                            dr["faArmorPiercer"] = GetSafeValue(i.achievements.armorPiercer);
                            dr["faBattleHeroes"] = GetSafeValue(i.achievements.battleHeroes);
                            dr["faBeastHunter"] = GetSafeValue(i.achievements.beasthunter);
                            dr["faBombardier"] = GetSafeValue(i.achievements.bombardier);
                            dr["faDefender"] = GetSafeValue(i.achievements.defender);
                            dr["faDieHard"] = GetSafeValue(i.achievements.diehard);
                            dr["faDieHardSeries"] = GetSafeValue(i.achievements.diehardSeries);
                            dr["faEveilEye"] = GetSafeValue(i.achievements.evileye);
                            dr["faFragsBeast"] = GetSafeValue(i.achievements.fragsBeast);
                            dr["faFragsPatton"] = GetSafeValue(i.achievements.fragsPatton);
                            dr["faFragsSinai"] = GetSafeValue(i.achievements.fragsSinai);
                            dr["faHandOfDeath"] = GetSafeValue(i.achievements.handOfDeath);
                            dr["faHeroesOfRasseney"] = GetSafeValue(i.achievements.heroesOfRassenay);
                            dr["faHuntsman"] = GetSafeValue(i.achievements.huntsman);
                            dr["faInvader"] = GetSafeValue(i.achievements.invader);
                            dr["faInvincible"] = GetSafeValue(i.achievements.invincible);
                            dr["faInvincibleSeries"] = GetSafeValue(i.achievements.invincibleSeries);
                            dr["faIronman"] = GetSafeValue(i.achievements.ironMan);
                            dr["faKamikaze"] = GetSafeValue(i.achievements.kamikaze);
                            dr["faKillingSeries"] = GetSafeValue(i.achievements.killingSeries);
                            dr["faLuckyDevil"] = GetSafeValue(i.achievements.luckyDevil);
                            dr["faLumberJack"] = GetSafeValue(i.achievements.lumberjack);
                            dr["faMarkOfMastery"] = GetSafeValue(i.achievements.markOfMastery);
                            dr["faMaxDieHardSeries"] = GetSafeValue(i.achievements.maxDiehardSeries);
                            dr["faMaxInvincibleSeries"] = GetSafeValue(i.achievements.maxInvincibleSeries);
                            dr["faMaxKillingSeries"] = GetSafeValue(i.achievements.maxKillingSeries);
                            dr["faMaxPiercingSeries"] = GetSafeValue(i.achievements.maxPiercingSeries);
                            dr["faMaxSniperSeries"] = GetSafeValue(i.achievements.maxSniperSeries);
                            dr["faMedalAbrams"] = GetSafeValue(i.achievements.medalAbrams);
                            dr["faMedalBillotte"] = GetSafeValue(i.achievements.medalBillotte);
                            dr["faMedalBrothersInArms"] = GetSafeValue(i.achievements.medalBrothersInArms);
                            dr["faMedalBrunoPietro"] = GetSafeValue(i.achievements.medalBrunoPietro);
                            dr["faMedalBurda"] = GetSafeValue(i.achievements.medalBurda);
                            dr["faMedalCarius"] = GetSafeValue(i.achievements.medalCarius);
                            dr["faMedalCrucialContribution"] = GetSafeValue(i.achievements.medalCrucialContribution);
                            dr["faMedalDeLanglade"] = GetSafeValue(i.achievements.medalDeLanglade);
                            dr["faMedalDumitru"] = GetSafeValue(i.achievements.medalDumitru);
                            dr["faMedalEkins"] = GetSafeValue(i.achievements.medalEkins);
                            dr["faMedalFadin"] = GetSafeValue(i.achievements.medalFadin);
                            dr["faMedalHalonen"] = GetSafeValue(i.achievements.medalHalonen);
                            dr["faMedalKay"] = GetSafeValue(i.achievements.medalKay);
                            dr["faMedalKnispel"] = GetSafeValue(i.achievements.medalKnispel);
                            dr["faMedalKolobanov"] = GetSafeValue(i.achievements.medalKolobanov);
                            dr["faMedalLafayettePool"] = GetSafeValue(i.achievements.medalLafayettePool);
                            dr["faMedalLavrinenko"] = GetSafeValue(i.achievements.medalLavrinenko);
                            dr["faMedalLeClerc"] = GetSafeValue(i.achievements.medalLeClerc);
                            dr["faMedalLehvaslaiho"] = GetSafeValue(i.achievements.medalLehvaslaiho);
                            dr["faMedalNikolas"] = GetSafeValue(i.achievements.medalNikolas);
                            dr["faMedalOrlik"] = GetSafeValue(i.achievements.medalOrlik);
                            dr["faMedalOskin"] = GetSafeValue(i.achievements.medalOskin);
                            dr["faMedalPascucci"] = GetSafeValue(i.achievements.medalPascucci);
                            dr["faMedalPoppel"] = GetSafeValue(i.achievements.medalPoppel);
                            dr["faMedalRadleyWalters"] = GetSafeValue(i.achievements.medalRadleyWalters);
                            dr["faMedalTamadaYoshio"] = GetSafeValue(i.achievements.medalTamadaYoshio);
                            dr["faMedalTarczay"] = GetSafeValue(i.achievements.medalTarczay);
                            dr["faMedalWittmann"] = GetSafeValue(i.achievements.medalWittmann);
                            dr["faMousebane"] = GetSafeValue(i.achievements.mousebane);
                            dr["faPattonValley"] = GetSafeValue(i.achievements.pattonValley);
                            dr["faPiercingSeries"] = GetSafeValue(i.achievements.piercingSeries);
                            dr["faRaider"] = GetSafeValue(i.achievements.raider);
                            dr["faScout"] = GetSafeValue(i.achievements.scout);
                            dr["faSinai"] = GetSafeValue(i.achievements.sinai);
                            dr["faSniper"] = GetSafeValue(i.achievements.sniper);
                            dr["faSniperSeries"] = GetSafeValue(i.achievements.sniperSeries);
                            dr["faSteelwall"] = GetSafeValue(i.achievements.steelwall);
                            dr["faSturdy"] = GetSafeValue(i.achievements.sturdy);
                            dr["faSupporter"] = GetSafeValue(i.achievements.supporter);
                            dr["faTankExpertStrg"] = GetSafeValue(i.achievements.tankExpertStrg);
                            dr["faTitleSniper"] = GetSafeValue(i.achievements.titleSniper);
                            dr["faWarrior"] = GetSafeValue(i.achievements.warrior);
                            tab.Rows.Add(dr);
                        }
                    }

                     if (ObjectExists(i, "a15x15"))
                    {
                        using (DataTable tab = ds.Tables["File_Battles"])
                        {
                            DataRow dr = tab.NewRow();
                            dr["bpParentID"] = tempTankID;
                            dr["bpBattleCount"] = GetSafeValue(i.a15x15.battlesCount);
                            dr["bpFrags8P"] = GetSafeValue(i.a15x15.frags8p);
                            dr["bpDefencePoints"] = GetSafeValue(i.a15x15.droppedCapturePoints);
                            dr["bpFrags"] = GetSafeValue(i.a15x15.frags);
                            dr["bpWinAndSurvive"] = GetSafeValue(i.a15x15.winAndSurvived);
                            dr["bpSpotted"] = GetSafeValue(i.a15x15.spotted);
                            dr["bpDamageDealt"] = GetSafeValue(i.a15x15.damageDealt);
                            dr["bpXPBefore8_8"] = GetSafeValue(i.a15x15.xpBefore8_8);
                            dr["bpShots"] = GetSafeValue(i.a15x15.shots);
                            dr["bpBattlesBefore8_8"] = GetSafeValue(i.a15x15.battlesCountBefore8_8);
                            dr["bpWins"] = GetSafeValue(i.a15x15.wins);
                            dr["bpDamageReceived"] = GetSafeValue(i.a15x15.damageReceived);
                            dr["bpLosses"] = GetSafeValue(i.a15x15.losses);
                            dr["bpXP"] = GetSafeValue(i.a15x15.xp);
                            dr["bpSurvivedBattles"] = GetSafeValue(i.a15x15.survivedBattles);
                            dr["bpHits"] = GetSafeValue(i.a15x15.hits);
                            dr["bpCapturePoints"] = GetSafeValue(i.a15x15.capturePoints);

                            if (ObjectExists(i, "a15x15_2"))
                            {

                                dr["bpDamageAssistedRadio"] = GetSafeValue(i.a15x15_2.damageAssistedRadio);
                                dr["bpDamageAssistedTracks"] = GetSafeValue(i.a15x15_2.damageAssistedTrack);
                                dr["bpHEHitsReceived"] = GetSafeValue(i.a15x15_2.heHitsReceived);
                                dr["bpPierced"] = GetSafeValue(i.a15x15_2.pierced);
                                dr["bpPiercedReceived"] = GetSafeValue(i.a15x15_2.piercedReceived);
                                dr["bpShotsReceived"] = GetSafeValue(i.a15x15_2.shotsReceived);
                                dr["bpNoDamageShotsReceived"] = GetSafeValue(i.a15x15_2.noDamageShotsReceived);
                                dr["bpOriginalXP"] = GetSafeValue(i.a15x15_2.originalXP);
                                dr["bpHEHits"] = GetSafeValue(i.a15x15_2.he_hits);
                            }

                            if (ObjectExists(i, "max15x15"))
                            {
                                dr["bpMaxXP"] = GetSafeValue(i.max15x15.maxXP);
                                dr["bpMaxFrags"] = GetSafeValue(i.max15x15.maxFrags);
                                dr["bpMaxDamage"] = GetSafeValue(i.max15x15.maxDamage); ;
                            }
                            dr["bpBattleMode"] = 15;

                            RatingStructure ratingStruct = new RatingStructure();
                            ratingStruct.WN8ExpectedTankList = WN8ExpectedTankList;
                            ratingStruct.countryID = GetSafeValue(i.common.countryid);
                            ratingStruct.tankID = GetSafeValue(i.common.tankid);
                            ratingStruct.tier = GetSafeValue(i.common.tier);
                            ratingStruct.globalTier = ratingStruct.tier;
                            
                            ratingStruct.singleTank = true;

                            ratingStruct.battlesCount = GetSafeValue(i.a15x15.battlesCount);
                            ratingStruct.battlesCount8_8 = GetSafeValue(i.a15x15.battlesCountBefore8_8);
                            ratingStruct.capturePoints = GetSafeValue(i.a15x15.capturePoints);
                            ratingStruct.defencePoints = GetSafeValue(i.a15x15.droppedCapturePoints);

                            ratingStruct.damageAssistedRadio = GetSafeValue(i.a15x15_2.damageAssistedRadio);
                            ratingStruct.damageAssistedTracks = GetSafeValue(i.a15x15_2.damageAssistedTrack);
                            ratingStruct.damageDealt = GetSafeValue(i.a15x15.damageDealt);
                            ratingStruct.frags = GetSafeValue(i.a15x15.frags);
                            ratingStruct.spotted = GetSafeValue(i.a15x15.spotted);


                            ratingStruct.wins=GetSafeValue(i.a15x15.wins);

                            ratingStruct.gWinRate = ratingStruct.winRate;

                            WOTStatistics.Core.Ratings.RatingStorage Eff = Ratings.GetRatingEff(ratingStruct);
                            WOTStatistics.Core.Ratings.RatingStorage BR = Ratings.GetRatingBR(ratingStruct);
                            WOTStatistics.Core.Ratings.RatingStorage WN7 = Ratings.GetRatingWN7(ratingStruct);

                            WOTStatistics.Core.Ratings.RatingStorage WN8 = Ratings.GetRatingWN8(ratingStruct);

                            dr["bpRatingEff"] = Eff.Value;
                            dr["bpRatingEffWeight"] = Eff.Weight;
                            dr["bpRatingBR"] = BR.Value;
                            dr["bpRatingBRWeight"] = BR.Weight;
                            dr["bpRatingWN7"] = WN7.Value;
                            dr["bpRatingWN7Weight"] = WN7.Weight;
                            dr["bpRatingWN8"] = WN8.Value;
                            dr["bpRatingWN8Weight"] = WN8.Weight;
                            dr["bpRatingVersion"] = WOTStatistics.Core.UserSettings.RatingVersion;
                            //foreach (DataRow drt in dr)
                            //{

                            //}
                            
                            //foreach (DataRow  itemarr in dr.ItemArray)
                            //{
                            //    WOTHelper.AddToLog("IA " + itemarr["bpBattleMode"]);
                            //}


                            tab.Rows.Add(dr);
                        }
                    }

                     if (ObjectExists(i, "a7x7"))
                    {
                        using (DataTable tab = ds.Tables["File_Battles"])
                        {
                            DataRow dr = tab.NewRow();
                            dr["bpParentID"] = tempTankID;
                            dr["bpBattleCount"] = GetSafeValue(i.a7x7.battlesCount);
                            dr["bpFrags8P"] = GetSafeValue(i.a7x7.frags8p);
                            dr["bpDefencePoints"] = GetSafeValue(i.a7x7.droppedCapturePoints);
                            dr["bpFrags"] = GetSafeValue(i.a7x7.frags);
                            dr["bpWinAndSurvive"] = GetSafeValue(i.a7x7.winAndSurvived);
                            dr["bpSpotted"] = GetSafeValue(i.a7x7.spotted);
                            dr["bpDamageDealt"] = GetSafeValue(i.a7x7.damageDealt);
                            dr["bpXPBefore8_8"] = GetSafeValue(i.a7x7.xpBefore8_8);
                            dr["bpShots"] = GetSafeValue(i.a7x7.shots);
                            dr["bpBattlesBefore8_8"] = GetSafeValue(i.a7x7.battlesCountBefore8_8);
                            dr["bpWins"] = GetSafeValue(i.a7x7.wins);
                            dr["bpDamageReceived"] = GetSafeValue(i.a7x7.damageReceived);
                            dr["bpLosses"] = GetSafeValue(i.a7x7.losses);
                            dr["bpXP"] = GetSafeValue(i.a7x7.xp);
                            dr["bpSurvivedBattles"] = GetSafeValue(i.a7x7.survivedBattles);
                            dr["bpHits"] = GetSafeValue(i.a7x7.hits);
                            dr["bpCapturePoints"] = GetSafeValue(i.a7x7.capturePoints);
                            dr["bpDamageAssistedRadio"] = GetSafeValue(i.a7x7.damageAssistedRadio);
                            dr["bpDamageAssistedTracks"] = GetSafeValue(i.a7x7.damageAssistedTrack);
                            dr["bpHEHitsReceived"] = GetSafeValue(i.a7x7.heHitsReceived);
                            dr["bpPierced"] = GetSafeValue(i.a7x7.pierced);
                            dr["bpPiercedReceived"] = GetSafeValue(i.a7x7.piercedReceived);
                            dr["bpShotsReceived"] = GetSafeValue(i.a7x7.shotsReceived);
                            dr["bpNoDamageShotsReceived"] = GetSafeValue(i.a7x7.noDamageShotsReceived);
                            dr["bpOriginalXP"] = GetSafeValue(i.a7x7.originalXP);
                            dr["bpHEHits"] = GetSafeValue(i.a7x7.he_hits);


                            if (ObjectExists(i, "max7x7"))
                            {
                                dr["bpMaxXP"] = GetSafeValue(i.max7x7.maxXP);
                                dr["bpMaxFrags"] = GetSafeValue(i.max7x7.maxFrags);
                                dr["bpMaxDamage"] = GetSafeValue(i.max7x7.maxDamage);
                            }

                            dr["bpBattleMode"] = 7;


                            RatingStructure ratingStruct = new RatingStructure();
                            ratingStruct.WN8ExpectedTankList = WN8ExpectedTankList;
                            ratingStruct.countryID = GetSafeValue(i.common.countryid);
                            ratingStruct.tankID = GetSafeValue(i.common.tankid);
                            ratingStruct.tier = GetSafeValue(i.common.tier);
                            ratingStruct.globalTier = ratingStruct.tier;

                            ratingStruct.singleTank = true;

                            ratingStruct.battlesCount = GetSafeValue(i.a7x7.battlesCount);
                            ratingStruct.battlesCount8_8 = 0;
                            ratingStruct.capturePoints = GetSafeValue(i.a7x7.capturePoints);
                            ratingStruct.defencePoints = GetSafeValue(i.a7x7.droppedCapturePoints);

                            ratingStruct.damageAssistedRadio = GetSafeValue(i.a7x7.damageAssistedRadio);
                            ratingStruct.damageAssistedTracks = GetSafeValue(i.a7x7.damageAssistedTrack);
                            ratingStruct.damageDealt = GetSafeValue(i.a7x7.damageDealt);
                            ratingStruct.frags = GetSafeValue(i.a7x7.frags);
                            ratingStruct.spotted = GetSafeValue(i.a7x7.spotted);


                            ratingStruct.wins = GetSafeValue(i.a7x7.wins);
                            ratingStruct.gWinRate = ratingStruct.winRate;

                            WOTStatistics.Core.Ratings.RatingStorage Eff = Ratings.GetRatingEff(ratingStruct);
                            WOTStatistics.Core.Ratings.RatingStorage BR = Ratings.GetRatingBR(ratingStruct);
                            WOTStatistics.Core.Ratings.RatingStorage WN7 = Ratings.GetRatingWN7(ratingStruct);
                            WOTStatistics.Core.Ratings.RatingStorage WN8 = Ratings.GetRatingWN8(ratingStruct);

                            dr["bpRatingEff"] = Eff.Value;
                            dr["bpRatingEffWeight"] = Eff.Weight;
                            dr["bpRatingBR"] = BR.Value;
                            dr["bpRatingBRWeight"] = BR.Weight;
                            dr["bpRatingWN7"] = WN7.Value;
                            dr["bpRatingWN7Weight"] = WN7.Weight;
                            dr["bpRatingWN8"] = WN8.Value;
                            dr["bpRatingWN8Weight"] = WN8.Weight;
                            dr["bpRatingVersion"] = WOTStatistics.Core.UserSettings.RatingVersion;

                            tab.Rows.Add(dr);
                        }
                    }

                     if (ObjectExists(i, "total"))
                     {
                         using (DataTable tab = ds.Tables["File_Total"])
                         {
                             DataRow dr = tab.NewRow();
                             dr["foParentID"] = tempTankID;
                             dr["foCreationTime"] = i.total.creationTime;
                             dr["foMileage"] = Convert.ToInt32(i.total.mileage);
                             dr["foTreesCut"] = i.total.treesCut;
                             dr["foLastBattleTime"] = i.total.lastBattleTime;
                             dr["foBattleLifeTime"] = i.total.battleLifeTime;
                             tab.Rows.Add(dr);
                         }
                     }

                     if (ObjectExists(i, "fragslist"))
                     {
                         foreach (var k in i.fragslist)
                         {
                             using (DataTable tab = ds.Tables["File_FragList"])
                             {
                                 DataRow dr = tab.NewRow();
                                 dr["fgParentID"] = tempTankID;
                                 dr["fgCountryID"] = k[0];
                                 dr["fgTankID"] = k[1];
                                 dr["fgValue"] = k[2];
                                 dr["fgTankDescription"] = k[3];
                                 tab.Rows.Add(dr);
                             }
                         }
                     }
                }

                

                tempTankID++;
            }
            
        }

        public DataTable GetTable(string tableName)
        {
            return ds.Tables[tableName];
        }

    
        private static bool ObjectExists(object collection, string testValue)
        {
            try
            {
                JToken value;
                if (collection.GetType() == typeof(JObject))
                {
                    if (((JObject)collection).TryGetValue(testValue, out value))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch 
            {
                
                return false;
            }
        }

        private static object GetSafeValue(object testField)
        {
            if (testField != null)
            {
                return testField;
            }

            return DBNull.Value;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
        }
        ~MemoryTables()
        {
            Dispose(false);
        }
    }
}

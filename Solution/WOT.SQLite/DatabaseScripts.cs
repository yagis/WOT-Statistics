using System;

namespace WOTStatistics.SQLite
{
    internal static class DatabaseScripts
    {
        public static string CreateRecentBattlesTable
        {
            get
            {
                return @"CREATE TABLE [RecentBattles] (
                        [rbID] INTEGER PRIMARY KEY NOT NULL,
                        [rbCountryID] INTEGER default (0)  NOT NULL,
                        [rbTankID] INTEGER default (0)  NOT NULL,
                        [rbOriginalBattleCount] REAL default (0)  NULL,
                        [rbBattles] REAL default (0)  NULL,
                        [rbKills] REAL default (0)  NULL,
                        [rbDamageReceived] REAL default (0)  NULL,
                        [rbDamageDealt] REAL default (0)  NULL,
                        [rbDamageAssistedRadio] real default(0),
                        [rbDamageAssistedTracks] real default(0),
                        [rbXPReceived] REAL default (0)  NULL,
                        [rbSpotted] REAL default (0)  NULL,
                        [rbCapturePoints] REAL default (0)  NULL,
                        [rbDefencePoints] REAL default (0)  NULL,
                        [rbSurvived] INTEGER default (0)  NOT NULL,
                        [rbVictory] INTEGER default (0)  NULL,
                        [rbBattleTime] INTEGER default (0)  NULL,
                        [rbShot] REAL default (0)  NULL,
                        [rbHits] REAL default (0)  NULL,
                        [rbTier] INTEGER default (0)  NULL,
                        [rbBattlesPerTier] REAL default (0)  NULL,
                        [rbVictoryCount] REAL default (0)  NULL,
                        [rbDefeatCount] REAL default (0)  NULL,
                        [rbDrawCount] REAL  default (0) NULL,
                        [rbSurviveYesCount] REAL default (0)  NULL,
                        [rbSurviveNoCount] REAL default (0)  NULL,
                        [rbFragList] TEXT  NULL,
                        [rbBattleTimeFriendly] text  NULL,
                        [rbGlobalAvgTier] REAL default (0)  NULL,
                        [rbGlobalWinPercentage] REAL default (0)  NULL,
                        [rbSessionID] varchar(50)  NULL,
                        [rbGlobalAvgDefPoints] REAL default (0)  NULL,
                        [rbBattleMode] INTEGER default (15)  NULL,
                        [rbRatingEff] real default(0),
                        [rbRatingBR] real default(0),
                        [rbRatingWN7] real default(0),
                        [rbRatingWN8] real default(0),
                        [rbMileage] real default(0)
                        );" + Environment.NewLine +

                       @"CREATE UNIQUE INDEX [IDX_RECENTBATTLES_K1_K2_K3_K28] ON [RecentBattles](
                        [rbID]  ASC,
                        [rbCountryID]  ASC,
                        [rbTankID]  ASC,
                        [rbSessionID]  ASC
                        );";
            }
        }

        public static string CreateRecentBattlesSessionTable
        {
            get
            {
                return @"CREATE TABLE [RecentBattles_Session] (
                        [rsID] INTEGER  NOT NULL PRIMARY KEY,
                        [rsKey] varchar(50)  NULL,
                        [rsDateFrom] text  NULL,
                        [rsDateTo] Text  NULL,
                        [rsUEDateFrom] real NULL,
                        [rsUEDateTo] real NULL
                        );" + Environment.NewLine +

                        @"CREATE INDEX [IDX_RECENTBATTLES_SESSION_K2_K3_K4] ON [RecentBattles_Session](
                        [rsKey]  ASC,
                        [rsUEDateFrom]  ASC,
                        [rsUEDateTo]  ASC
                        );";
            }
        }

        public static string CreateFilesTable
        {
            get
            {
                return @"CREATE TABLE [Files] (
                                            [fiID] INTEGER  PRIMARY KEY NOT NULL,
                                            [fiResult] TEXT  NULL,
                                            [fiUserName] TEXT  NULL,
                                            [fiTankCount] INTEGER  NULL,
                                            [fiParser] TEXT  NULL,
                                            [fiServer] TEXT  NULL,
                                            [fiMessage] TEXT  NULL,
                                            [fiDate] REAL  NULL,
                                            [fiParserVersion] TEXT  NULL
                                            );" + Environment.NewLine +

                        @"CREATE INDEX [IDX_FILES_K1] ON [Files](
                                                            [fiID]  ASC
                                                            );";
            }
        }

        public static string CreateFile_TankDetailsTable
        {
            get
            {
                return @"CREATE TABLE [File_TankDetails] (
                                            [cmID] INTEGER  PRIMARY KEY NOT NULL,
                                            [cmFileID] INTEGER  NULL,
                                            [cmCreationTimeR] TEXT  NULL,
                                            [cmFrags] INTEGER  NULL,
                                            [cmTankTitle] TEXT  NULL,
                                            [cmPremium] INTEGER  NULL,
                                            [cmUpdated] REAL  NULL,
                                            [cmTier] INTEGER  NULL,
                                            [cmUpdatedR] TEXT  NULL,
                                            [cmLastBattleTime] REAL  NULL,
                                            [cmFragsCompare] INTEGER  NULL,
                                            [cmLastBattleTimeR] TEXT  NULL,
                                            [cmBaseVersion] INTEGER  NULL,
                                            [cmCreationTime] REAL  NULL,
                                            [cmCompactDescription] INTEGER  NULL,
                                            [cmCountryID] INTEGER  NULL,
                                            [cmTankID] INTEGER  NULL,
                                            [cmType] INTEGER  NULL,
                                            [cmHasClan] INTEGER  NULL,
                                            [cmHas7x7] INTEGER  NULL,
                                            [cmHas15x15] INTEGER  NULL,
                                            [cmHasCompany] INTEGER  NULL
                                            );" + Environment.NewLine +

                       @"CREATE INDEX [IDX_FILE_TANKDETAILS_K1_K2] ON [File_TankDetails](
                                            [cmID]  ASC,
                                            [cmFileID]  ASC
                                            );";
            }
        }

        public static string CreateFile_TotalTable
        {
            get
            {
                return @"CREATE TABLE [File_Total] (
                                        [foParentID] INTEGER  NULL,
                                        [foCreationTime] REAL  NULL,
                                        [foMileage] INTEGER  NULL,
                                        [foTreesCut] INTEGER  NULL,
                                        [foLastBattleTime] REAL  NULL,
                                        [foBattleLifeTime] REAL  NULL
                                        );" + Environment.NewLine +

                       @"CREATE INDEX [IDX_FILE_TOTAL_K1_K2] ON [File_Total](
                                        [foParentID]  ASC
                                        );";
            }
        }

        public static string CreateFile_FragListTable
        {
            get
            {
                return @"CREATE TABLE [File_FragList] (
                                            [fgParentID] INTEGER  NULL,
                                            [fgCountryID] INTEGER  NULL,
                                            [fgTankID] INTEGER  NULL,
                                            [fgValue] INTEGER  NULL,
                                            [fgTankDescription] TEXT  NULL
                                            );" + Environment.NewLine +

                       @"CREATE INDEX [IDX_FILE_FRAGLIST_K1_K2] ON [File_FragList](
                                            [fgParentID]  ASC
                                            );";
            }
        }

        public static string CreateFile_CompanyTable
        {
            get
            {
                return @"CREATE TABLE [File_Company] (
                                        [fcParentID] INTEGER  NULL,
                                        [fcBattlesCount] INTEGER  NULL,
                                        [fcDefencePoints] INTEGER  NULL,
                                        [fcFrags] INTEGER  NULL,
                                        [fcSpotted] INTEGER  NULL,
                                        [fcDamageDealt] INTEGER  NULL,
                                        [fcXPBefore8_9] INTEGER  NULL,
                                        [fcShots] INTEGER  NULL,
                                        [fcBattlesCountBefore8_9] INTEGER  NULL,
                                        [fcWins] INTEGER  NULL,
                                        [fcDamageReceived] INTEGER  NULL,
                                        [fcLosses] INTEGER  NULL,
                                        [fcXP] INTEGER  NULL,
                                        [fcSurvivedBattles] INTEGER  NULL,
                                        [fcHits] INTEGER  NULL,
                                        [fcCapturePoints] INTEGER  NULL
                                        );" + Environment.NewLine +

                       @"CREATE INDEX [IDX_FILE_COMPANY_K1_K2] ON [File_Company](
                                        [fcParentID]  ASC
                                        );";
            }
        }

        public static string CreateFile_HistoricalTable
        {
            get
            {
                return @"CREATE TABLE [File_Historical] (
                                    [hsParentID] INTEGER  NULL,
                                    [hsBattlesCount] INTEGER  NULL,
                                    [hsDefencePoints] INTEGER  NULL,
                                    [hsFrags] INTEGER  NULL,
                                    [hsSpotted] INTEGER  NULL,
                                    [hsDamageDealt] INTEGER  NULL,
                                    [hsShots] INTEGER  NULL,
                                    [hsWins] INTEGER  NULL,
                                    [hsDamageReceived] INTEGER  NULL,
                                    [hsLosses] INTEGER  NULL,
                                    [hsXP] INTEGER  NULL,
                                    [hsSurvivedBattles] INTEGER  NULL,
                                    [hsHits] INTEGER  NULL,
                                    [hsCapturePoints] INTEGER  NULL
                                    );" + Environment.NewLine +

                      @"CREATE INDEX [IDX_FILE_HISTORICAL_K1_K2] ON [File_Historical](
                                    [hsParentID]  ASC
                                    );";
            }
        }

        public static string CreateFile_ClanTable
        {
            get
            {
                return @"CREATE TABLE [File_Clan] (
                                    [clParentID] INTEGER  NULL,
                                    [clBattlesCount] INTEGER  NULL,
                                    [clDefencePoints] INTEGER  NULL,
                                    [clFrags] INTEGER  NULL,
                                    [clSpotted] INTEGER  NULL,
                                    [clDamageDealt] INTEGER  NULL,
                                    [clXPBefore8_9] INTEGER  NULL,
                                    [clShots] INTEGER  NULL,
                                    [clBattlesCountBefore8_9] INTEGER  NULL,
                                    [clWins] INTEGER  NULL,
                                    [clDamageReceived] INTEGER  NULL,
                                    [clLosses] INTEGER  NULL,
                                    [clXP] INTEGER  NULL,
                                    [clSurvivedBattles] INTEGER  NULL,
                                    [clHits] INTEGER  NULL,
                                    [clCapturePoints] INTEGER  NULL
                                    );" + Environment.NewLine +

                      @"CREATE INDEX [IDX_FILE_CLAN_K1_K2] ON [File_Clan](
                                    [clParentID]  ASC
                                    );";
            }
        }

        public static string CreateFile_BattlesTable
        {
            get
            {
                return @"CREATE TABLE [File_Battles] (
                                            [bpParentID] INTEGER  NULL,
                                            [bpBattleCount] INTEGER  NULL,
                                            [bpFrags8P] INTEGER  NULL,
                                            [bpDefencePoints] INTEGER  NULL,
                                            [bpFrags] INTEGER  NULL,
                                            [bpWinAndSurvive] INTEGER  NULL,
                                            [bpSpotted] INTEGER  NULL,
                                            [bpDamageDealt] INTEGER  NULL,
                                            [bpDamageAssistedRadio] INTEGER  NULL,
                                            [bpDamageAssistedTracks] INTEGER  NULL,
                                            [bpXPBefore8_8] INTEGER  NULL,
                                            [bpShots] INTEGER  NULL,
                                            [bpBattlesBefore8_8] INTEGER  NULL,
                                            [bpWins] INTEGER  NULL,
                                            [bpDamageReceived] INTEGER  NULL,
                                            [bpLosses] INTEGER  NULL,
                                            [bpXP] INTEGER  NULL,
                                            [bpSurvivedBattles] INTEGER  NULL,
                                            [bpHits] INTEGER  NULL,
                                            [bpCapturePoints] INTEGER  NULL,
                                            [bpHEHitsReceived] INTEGER  NULL,
                                            [bpPiercedReceived] INTEGER  NULL,
                                            [bpPierced] INTEGER  NULL,
                                            [bpShotsReceived] INTEGER  NULL,
                                            [bpNoDamageShotsReceived] INTEGER  NULL,
                                            [bpOriginalXP] INTEGER  NULL,
                                            [bpHEHits] INTEGER  NULL,
                                            [bpMaxXP] INTEGER  NULL,
                                            [bpMaxFrags] INTEGER  NULL,
                                            [bpMaxDamage] INTEGER  NULL,
                                            [bpBattleMode] INTEGER  NULL,
                                            [bpRatingEff] INTEGER  NULL,
                                            [bpRatingEffWeight] INTEGER  NULL,
                                            [bpRatingBR] INTEGER  NULL,
                                            [bpRatingBRWeight] INTEGER  NULL,
                                            [bpRatingWN7] INTEGER  NULL,
                                            [bpRatingWN7Weight] INTEGER  NULL,
                                            [bpRatingWN8] INTEGER  NULL,
                                            [bpRatingWN8Weight] INTEGER  NULL,
                                            [bpRatingVersion] INTEGER NULL,
                                            [bpMileage] INTEGER  NULL
                                            );" + Environment.NewLine +

                       @"CREATE INDEX [IDX_FILE_BATTLES_K1_K2] ON [File_Battles](
                                            [bpParentID]  ASC
                                            );";
            }
        }

        public static string CreateFile_OverallTable
        {
            get
            {
                return @"CREATE TABLE [Overall] (
                                                [ovRatingEff] real  NULL,
                                                [ovRatingEffPrev] real  NULL,
                                                [ovRatingBR] real  NULL,
                                                [ovRatingBRPrev] real  NULL,
                                                [ovRatingWN7] real  NULL,
                                                [ovRatingWN7Prev] real  NULL,
                                                [ovRatingWN8] real  NULL,
                                                [ovRatingWN8Prev] real  NULL,
                                                [ovWinRate] real  NULL,
                                                [ovWinRatePrev] real  NULL
                                                );";
            }
        }

        public static string CreateFile_AchievementsTable
        {
            get
            {
                return @"CREATE TABLE [File_Achievements] (
                                                [faParentID] INTEGER  NULL,
                                                [faAlaric] INTEGER  NULL,
                                                [faArmorPiercer] INTEGER  NULL,
                                                [faBattleHeroes] INTEGER  NULL,
                                                [faBeastHunter] INTEGER  NULL,
                                                [faBombardier] INTEGER  NULL,
                                                [faDefender] INTEGER  NULL,
                                                [faDieHard] INTEGER  NULL,
                                                [faDieHardSeries] INTEGER  NULL,
                                                [faEveilEye] INTEGER  NULL,
                                                [faFragsBeast] INTEGER  NULL,
                                                [faFragsPatton] INTEGER  NULL,
                                                [faFragsSinai] INTEGER  NULL,
                                                [faHandOfDeath] INTEGER  NULL,
                                                [faHeroesOfRasseney] INTEGER  NULL,
                                                [faHuntsman] INTEGER  NULL,
                                                [faInvader] INTEGER  NULL,
                                                [faInvincible] INTEGER  NULL,
                                                [faInvincibleSeries] INTEGER  NULL,
                                                [faIronman] INTEGER  NULL,
                                                [faKamikaze] INTEGER  NULL,
                                                [faKillingSeries] INTEGER  NULL,
                                                [faLuckyDevil] INTEGER  NULL,
                                                [faLumberJack] INTEGER  NULL,
                                                [faMarkOfMastery] INTEGER  NULL,
                                                [faMaxDieHardSeries] INTEGER  NULL,
                                                [faMaxInvincibleSeries] INTEGER  NULL,
                                                [faMaxKillingSeries] INTEGER  NULL,
                                                [faMaxPiercingSeries] INTEGER  NULL,
                                                [faMaxSniperSeries] INTEGER  NULL,
                                                [faMedalAbrams] INTEGER  NULL,
                                                [faMedalBillotte] INTEGER  NULL,
                                                [faMedalBrothersInArms] INTEGER  NULL,
                                                [faMedalBrunoPietro] INTEGER  NULL,
                                                [faMedalBurda] INTEGER  NULL,
                                                [faMedalCarius] INTEGER  NULL,
                                                [faMedalCrucialContribution] INTEGER  NULL,
                                                [faMedalDeLanglade] INTEGER  NULL,
                                                [faMedalDumitru] INTEGER  NULL,
                                                [faMedalEkins] INTEGER  NULL,
                                                [faMedalFadin] INTEGER  NULL,
                                                [faMedalHalonen] INTEGER  NULL,
                                                [faMedalKay] INTEGER  NULL,
                                                [faMedalKnispel] INTEGER  NULL,
                                                [faMedalKolobanov] INTEGER  NULL,
                                                [faMedalLafayettePool] INTEGER  NULL,
                                                [faMedalLavrinenko] INTEGER  NULL,
                                                [faMedalLeClerc] INTEGER  NULL,
                                                [faMedalLehvaslaiho] INTEGER  NULL,
                                                [faMedalNikolas] INTEGER  NULL,
                                                [faMedalOrlik] INTEGER  NULL,
                                                [faMedalOskin] INTEGER  NULL,
                                                [faMedalPascucci] INTEGER  NULL,
                                                [faMedalPoppel] INTEGER  NULL,
                                                [faMedalRadleyWalters] INTEGER  NULL,
                                                [faMedalTamadaYoshio] INTEGER  NULL,
                                                [faMedalTarczay] INTEGER  NULL,
                                                [faMedalWittmann] INTEGER  NULL,
                                                [faMousebane] INTEGER  NULL,
                                                [faPattonValley] INTEGER  NULL,
                                                [faPiercingSeries] INTEGER  NULL,
                                                [faRaider] INTEGER  NULL,
                                                [faScout] INTEGER  NULL,
                                                [faSinai] INTEGER  NULL,
                                                [faSniper] INTEGER  NULL,
                                                [faSniperSeries] INTEGER  NULL,
                                                [faSteelwall] INTEGER  NULL,
                                                [faSturdy] INTEGER  NULL,
                                                [faSupporter] INTEGER  NULL,
                                                [faTankExpertStrg] INTEGER  NULL,
                                                [faTitleSniper] INTEGER  NULL,
                                                [faWarrior] INTEGER  NULL
                                                );" + Environment.NewLine +

                   @"CREATE INDEX [IDX_FILE_ACHIEVEMENTS_K1_K2] ON [File_Achievements](
                                        [faParentID]  ASC
                                        );";
            }
        }

        public static string CreateCache_LastGame
        {
            get
            {
                return @"CREATE TABLE [Cache_LastGame] (
                                    [clID] INTEGER  NOT NULL PRIMARY KEY,
                                    [clFileID] INTEGER  NOT NULL,
                                    [clCountryID] INTEGER  NULL,
                                    [clTankID] INTEGER  NULL,
                                    [clBattleCount] INTEGER  NULL);";
            }
        }



    }
}

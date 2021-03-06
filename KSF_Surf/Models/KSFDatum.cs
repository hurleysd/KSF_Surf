﻿
using System.Collections.Generic;

// Classes for KSF API response JSON deserialization

namespace KSF_Surf.Models
{
    // LivePage ----------------------------------------------------------------------
    #region LivePage

    // SERVER LIST

    public class ServerPlayerDatum
    {
        public string playerid { get; set; }
        public string playername { get; set; }
        public string rank { get; set; }
        public string points { get; set; }
        public string timeconnected { get; set; }
        public string pc { get; set; }
        public string zone { get; set; }
        public string timeinzone { get; set; }
    }


    public class KSFServerDatum 
    {
        public string serverID { get; set; }
        public string hostname { get; set; }
        public string surftimer_servername { get; set; }
        public string ipport { get; set; }
        public string timesinceupdate { get; set; }
        public string timestamp { get; set; }
        public string currentmap { get; set; }
        public string timeleft { get; set; }
        public string mp_timelimit { get; set; }
        public string playersonline { get; set; }
        public string playersalive { get; set; }
        public string playersspectate { get; set; }
        public List<ServerPlayerDatum> players { get; set; }
    }

    public class KSFServerRootObject
    {
        public string status { get; set; }
        public List<KSFServerDatum> data { get; set; }
    }

    #endregion  
    // MapsPage ----------------------------------------------------------------------
    #region MapsPage

    // MAPS LIST

    public class DetailedMapDatum
    {
        public string name { get; set; }
        public string created { get; set; }
        public string lastplayed { get; set; }
        public string playtime { get; set; }
        public string totalplaytimes { get; set; }
        public string maptype { get; set; }
        public string tier { get; set; }
        public string cp_count { get; set; }
        public string b_count { get; set; }
        public string maxvelocity { get; set; }
        public string cheats { get; set; }
        public string popularity { get; set; }
    }

    public class DetailedMapsRootObject
    {
        public string status { get; set; }
        public List<DetailedMapDatum> data { get; set; }
    }

    // MAP INFO

    public class MapSettings
    {
        public string created { get; set; }
        public string lastplayed { get; set; }
        public string playtime { get; set; }
        public string totalplaytimes { get; set; }
        public string maptype { get; set; }
        public string cp_count { get; set; }
        public string b_count { get; set; }
        public string tier { get; set; }
        public string wr_points_0 { get; set; }
        public string wr_points_1 { get; set; }
        public string wr_points_2 { get; set; }
        public string wr_points_3 { get; set; }
        public string map_finish { get; set; }
        public string stage_finish { get; set; }
        public string bonus_finish { get; set; }
        public string maxvelocity { get; set; }
        public string cheats { get; set; }
        public string enabled { get; set; }
    }

    public class Mapper
    {
        public string mapperName { get; set; }
        public string mapperID { get; set; }
        public string steamID { get; set; }
        public string typeID { get; set; }
        public string typeName { get; set; }
    }

    public class MapInfoData
    {
        public string MapID { get; set; }
        public MapSettings MapSettings { get; set; }
        public List<Mapper> Mappers { get; set; }
    }

    public class MapInfoRootObject
    {
        public string status { get; set; }
        public MapInfoData data { get; set; }
    }

    // MAP TOP 

    public class TopDatum
    {
        public string name { get; set; }
        public string playerid { get; set; }
        public string steamID { get; set; }
        public string country { get; set; }
        public string time { get; set; }
        public string count { get; set; }
        public string date { get; set; }
        public string dateNow { get; set; }
        public string wrDiff { get; set; }
    }

    public class MapTopRootObject
    {
        public string status { get; set; }
        public List<TopDatum> data { get; set; }
    }

    // MAP POINTS 

    public class PointsData
    {
        public string MapID { get; set; }
        public string Tier { get; set; }
        public string TotalPlayers { get; set; }
        public double WRPoints { get; set; }
        public List<double> TopPoints { get; set; }
        public List<int> GroupRanks { get; set; }
        public List<double> GroupPoints { get; set; }
    }

    public class MapPointsRootObject
    {
        public string status { get; set; }
        public PointsData data { get; set; }
    }

    // PERSONAL RECORD INFO

    public class MapPRInfoDatum
    {
        public string mapID { get; set; }
        public string tier { get; set; }
        public string stageID { get; set; }
        public int zoneID { get; set; }
        public string playerID { get; set; }
        public BasicInfo basicInfo { get; set; }
        public string recordID { get; set; }
        public int? rank { get; set; }               // 0 or null
        public string totalRanks { get; set; }
        public int? group { get; set; }             // null if none, 0 if top10
        public string time { get; set; }            
        public string wrDiff { get; set; }          
        public string r2Diff { get; set; }          // null if no r2
        public string count { get; set; }          
        public string date { get; set; }          
        public string avgvel { get; set; }          
        public string startvel { get; set; }          
        public string endvel { get; set; }          
        public string total_time { get; set; }          
        public string attempts { get; set; }          
        public string first_date { get; set; }          // none for other styles besides FW
        public string first_timetaken { get; set; }     // none for other styles besides FW    
        public string first_attempts { get; set; }      // none for other styles besides FW   
        public string date_lastplayed { get; set; }     // none for other styles besides FW     
    }

    public class MapPRInfoRootObject
    {
        public string status { get; set; }
        public MapPRInfoDatum data { get; set; }
    }

    // PERSONAL RECORD

    public class MapPRDetails
    {
        public string zoneID { get; set; }
        public string stageID { get; set; }
        public string surfTime { get; set; }
        public string rank { get; set; }               // 0 or null
        public string totalRanks { get; set; }
        public string avgVel { get; set; }
        public string startVel { get; set; }
        public string endVel { get; set; }
        public string dateSet { get; set; }
        public string count { get; set; }
        public string attempts { get; set; }
        public string totalSurfTime { get; set; }
        public string dateLastPlayed { get; set; }
        public string firstDate { get; set; }       // none for other styles besides FW
        public string firstTimeTaken { get; set; }  // none for other styles besides FW
        public string firstAttempts { get; set; }   // none for other styles besides FW
        public int? group { get; set; }              // null if none, 0 if top10
    }

    public class MapPRDatum
    {
        public string mapID { get; set; }
        public string tier { get; set; }
        public BasicInfo basicInfo { get; set; }
        public List<MapPRDetails> PRInfo { get; set; }
        
    }

    public class MapPRRootObject
    {
        public string status { get; set; }
        public MapPRDatum data { get; set; }
    }

    // CPR

    public class MapCPRDetails
    {
        public string zoneID { get; set; }
        public string WRTime { get; set; }
        public string WRTouchVel { get; set; }
        public string playerTime { get; set; }
        public string playerTouchVel { get; set; }
        public string timeDiff { get; set; }
        public string velDiff { get; set; }

    }

    public class MapCPRDatum
    {
        public string mapID { get; set; }
        public string stageID { get; set; }
        public string mapType { get; set; }
        public BasicInfo basicInfo { get; set; }
        public BasicInfo basicInfoWR { get; set; }
        public List<MapCPRDetails> CPR { get; set; }

    }

    public class MapCPRRootObject
    {
        public string status { get; set; }
        public MapCPRDatum data { get; set; }
    }

    // CCP

    public class MapCCPDetails
    {
        public string zoneID { get; set; }
        public string cpTimeWR { get; set; }
        public string avgVelWR { get; set; }
        public string attemptsWR { get; set; }
        public string cpTimePlayer { get; set; }
        public string avgVelPlayer { get; set; }
        public string attemptsPlayer { get; set; }
        public string rankPlayer { get; set; }
        public string totalRanks { get; set; }

    }

    public class MapCCPDatum
    {
        public string mapID { get; set; }
        public string stageID { get; set; }
        public string mapType { get; set; }
        public BasicInfo basicInfo { get; set; }
        public BasicInfo basicInfoWR { get; set; }
        public List<MapCCPDetails> CCP { get; set; }

    }

    public class MapCCPRootObject
    {
        public string status { get; set; }
        public MapCCPDatum data { get; set; }
    }

    #endregion
    // RecordsPage -------------------------------------------------------------------
    #region RecordsPage

    // SURF TOP

    public class SurfTopDatum
    {
        public string name { get; set; }
        public string points { get; set; }
        public string playerID { get; set; }
        public string steamID { get; set; }
        public string country { get; set; }
        public string lastOnline { get; set; }
    }

    public class SurfTopRootObject
    {
        public string status { get; set; }
        public List<SurfTopDatum> data { get; set; }
    }

    // RECENT RECORDS

    public class RRDatum
    {
        public string date { get; set; }
        public string mapName { get; set; }
        public string zoneID { get; set; }
        public string finishtype { get; set; }
        public string surfTime { get; set; }
        public string wrDiff { get; set; }
        public string r2Diff { get; set; }
        public string dateNow { get; set; }
        public string server { get; set; }
        public string playerID { get; set; }
        public string playerName { get; set; }
        public string country { get; set; }
        public string steamid { get; set; }
    }

    public class RRRootObject
    {
        public string status { get; set; }
        public List<RRDatum> data { get; set; }
    }

    // RECENT TOP 10
    public class RR10Datum
    {
        public string date { get; set; }
        public string mapName { get; set; }
        public string zoneID { get; set; }
        public string newRank { get; set; }
        public string prevRank { get; set; }
        public string finishType { get; set; }
        public string dateNow { get; set; }
        public string server { get; set; }
        public string surfTime { get; set; }
        public string wrDiff { get; set; }
        public string playerID { get; set; }
        public string playerName { get; set; }
        public string country { get; set; }
        public string steamid { get; set; }
    }

    public class RR10RootObject
    {
        public string status { get; set; }
        public List<RR10Datum> data { get; set; }
    }

    // MOST BY TYPE (COUNT)

    public class MostCountDatum
    {
        public string name { get; set; }
        public string playerID { get; set; }
        public string steamID { get; set; }
        public string country { get; set; }
        public string lastonline { get; set; }
        public string total { get; set; }
    }

    public class MostCountRootObject
    {
        public string status { get; set; }
        public List<MostCountDatum> data { get; set; }
    }

    // MOST BY TYPE (CONTESTED ZONE)

    public class MostContZoneDatum
    {
        public string mapID { get; set; }
        public string mapName { get; set; }
        public string zoneID { get; set; }
        public string stageID { get; set; }
        public string total { get; set; }
        public string date { get; set; }
    }

    public class MostContZoneRootObject
    {
        public string status { get; set; }
        public List<MostContZoneDatum> data { get; set; }
    }

    // MOST BY TYPE (GROUP)

    public class MostGroupDatum
    {
        public string name { get; set; }
        public string playerID { get; set; }
        public string steamID { get; set; }
        public string country { get; set; }
        public string lastonline { get; set; }
        public string groupPoints { get; set; }
    }

    public class MostGroupRootObject
    {
        public string status { get; set; }
        public List<MostGroupDatum> data { get; set; }
    }

    // MOST BY TYPE (PC)

    public class MostPCDatum
    {
        public string name { get; set; }
        public string playerID { get; set; }
        public string steamID { get; set; }
        public string country { get; set; }
        public string lastonline { get; set; }
        public string percentCompletion { get; set; }
    }

    public class MostPCRootObject
    {
        public string status { get; set; }
        public List<MostPCDatum> data { get; set; }
    }

    // MOST BY TYPE (TOP)

    public class MostTopDatum
    {
        public string name { get; set; }
        public string playerID { get; set; }
        public string steamID { get; set; }
        public string country { get; set; }
        public string lastonline { get; set; }
        public string top10Points { get; set; }
    }

    public class MostTopRootObject
    {
        public string status { get; set; }
        public List<MostTopDatum> data { get; set; }
    }

    // MOST BY TYPE (CONTESTED WR)

    public class MostContWrDatum
    {
        public string mapID { get; set; }
        public string mapName { get; set; }
        public string total { get; set; }
        public string date { get; set; }
    }

    public class MostContWrRootObject
    {
        public string status { get; set; }
        public List<MostContWrDatum> data { get; set; }
    }

    // MOST BY TYPE (TIME)

    public class MostTimeDatum
    {
        public string mapID { get; set; }
        public string mapName { get; set; }
        public int totalplaytime { get; set; }
    }

    public class MostTimeRootObject
    {
        public string status { get; set; }
        public List<MostTimeDatum> data { get; set; }
    }

    // OLDEST RECORDS

    public class OldRecord
    {
        public string mapName { get; set; }
        public string mapID { get; set; }
        public string zoneID { get; set; }
        public string stageID { get; set; }
        public string finishType { get; set; }
        public string points { get; set; }
        public string date { get; set; }
        public string dateNow { get; set; }
        public string surfTime { get; set; }
        public string r2Diff { get; set; }
        public string playerID { get; set; }
        public string playerName { get; set; }
        public string country { get; set; }
        public string steamID { get; set; }
    }

    public class OldestRecordsRootObject
    {
        public string status { get; set; }
        public List<OldRecord> data { get; set; }
    }

    // TOP COUNTRIES

    public class CountryPoints
    {
        public string country { get; set; }
        public string points { get; set; }
    }

    public class TopCountriesRootObject
    {
        public string status { get; set; }
        public List<CountryPoints> data { get; set; }
    }

    // COUNTRY TOP

    public class CountryPlayer
    {
        public string playerName { get; set; }
        public string points { get; set; }
        public string playerID { get; set; }
        public string steamID { get; set; }
        public string lastonline { get; set; }
    }

    public class CountryTopRootObject
    {
        public string status { get; set; }
        public List<CountryPlayer> data { get; set; }
    }

    #endregion
    // PlayerPage --------------------------------------------------------------------
    #region PlayerPage

    // PLAYER INFO

    public class TotalZones
    {
        public string TotalMaps { get; set; }
        public string TotalStages { get; set; }
        public string TotalBonuses { get; set; }
    }

    public class WRZones
    {
        public string wr { get; set; }
        public string wrcp { get; set; }
        public string wrb { get; set; }
    }

    public class Top10Groups
    {
        public string top10 { get; set; }
        public string groups { get; set; }
        public string rank1 { get; set; }
        public string rank2 { get; set; }
        public string rank3 { get; set; }
        public string rank4 { get; set; }
        public string rank5 { get; set; }
        public string rank6 { get; set; }
        public string rank7 { get; set; }
        public string rank8 { get; set; }
        public string rank9 { get; set; }
        public string rank10 { get; set; }
        public string g1 { get; set; }
        public string g2 { get; set; }
        public string g3 { get; set; }
        public string g4 { get; set; }
        public string g5 { get; set; }
        public string g6 { get; set; }
    }

    public class CompletedZones
    {
        public string map { get; set; }
        public string stage { get; set; }
        public string bonus { get; set; }
    }

    public class PlayerPoints
    {
        public string points { get; set; }
        public string top10 { get; set; }
        public string groups { get; set; }
        public string wrcp { get; set; }
        public string wrb { get; set; }
        public string map { get; set; }
        public string stage { get; set; }
        public string bonus { get; set; }
    }

    public class BasicInfo
    {
        public string steamID { get; set; }
        public string name { get; set; }
        public string playerID { get; set; }
        public string country { get; set; }
        public string lastOnline { get; set; }
        public string firstOnline { get; set; }
        public string onlineTime { get; set; }
        public string aliveTime { get; set; }
        public string deadTime { get; set; }
        public string totalConnections { get; set; }
    }

    public class PlayerInfoDatum
    {
        public object banStatus { get; set; }
        public BasicInfo basicInfo { get; set; }
        
        public string KSFStatus { get; set; } // "member" or null
        public string vipStatus { get; set; } // "active" or null
        public string adminStatus { get; set; } // "active" or null
        public string mapperID { get; set; } // "<number>" or null
        public PlayerPoints playerPoints { get; set; }
        public CompletedZones CompletedZones { get; set; }
        public Top10Groups Top10Groups { get; set; }
        public WRZones WRZones { get; set; }
        public string SurfRank { get; set; }
        public string SurfTotalRank { get; set; }
        public string percentCompletion { get; set; }
        public TotalZones TotalZones { get; set; }
    }

    public class PlayerInfoRootObject
    {
        public string status { get; set; }
        public PlayerInfoDatum data { get; set; }
    }

    // PLAYER RECENT RECORDS

    public class RecentPlayerRecords
    {
        public string date { get; set; }
        public string mapName { get; set; }
        public string zoneID { get; set; }
        public string newRank { get; set; }
        public string prevRank { get; set; }
        public string finishType { get; set; }
        public string groupChange { get; set; }
        public string dateNow { get; set; }
        public string server { get; set; }
        public string surfTime { get; set; }
        public string wrDiff { get; set; }
        public string recordType { get; set; }
    }

    public class PlayerRecordsDatum
    {
        public BasicInfo basicInfo { get; set; }
        public List<RecentPlayerRecords> recentRecords { get; set; }
    }

    public class PlayerRecordsRootObject
    {
        public string status { get; set; }
        public PlayerRecordsDatum data { get; set; }
    }

    // PLAYER WORLD RECORDS

    public class PlayerWorldRecords
    {
        public string mapName { get; set; }
        public string mapID { get; set; }
        public string stageID { get; set; }
        public string zoneID { get; set; }
        public string finishType { get; set; }
        public string date { get; set; }
        public string dateNow { get; set; }
        public string surfTime { get; set; }
        public string r2Diff { get; set; }
    }

    public class PlayerWRsDatum
    {
        public BasicInfo basicInfo { get; set; }
        public List<PlayerWorldRecords> records { get; set; }
    }

    public class PlayerWRsRootObject
    {
        public string status { get; set; }
        public PlayerWRsDatum data { get; set; }
    }

    // PLAYER COMPLETION BY TIER

    public class PlayerTierCompletion
    {
        public string tier { get; set; }
        public string map { get; set; }
        public string mapTotal { get; set; }
        public string stage { get; set; }
        public string stageTotal { get; set; }
        public string bonus { get; set; }
        public string bonusTotal { get; set; }
    }

    public class PlayerTierCompletionDatum
    {
        public BasicInfo basicInfo { get; set; }
        public List<PlayerTierCompletion> records { get; set; }
    }

    public class PlayerTierCompletionRootObject
    {
        public string status { get; set; }
        public PlayerTierCompletionDatum data { get; set; }
    }

    // PLAYER COMPLETION BY TIER

    public class PlayerOldRecord
    {
        public string mapName { get; set; }
        public string mapID { get; set; }
        public string zoneID { get; set; }                  // wrcp/wrb stage/bonus
        public string stageID { get; set; }
        public string finishType { get; set; }
        public string rank { get; set; }                    // top10
        public string top10Points { get; set; }             // top10
        public string groupPoints { get; set; }             // top10
        public string points { get; set; }                  // map
        public string top10Group { get; set; }              // map
        public string date { get; set; }
        public string dateNow { get; set; }
        public string surfTime { get; set; }
        public string count { get; set; }                   // map/stage/bonus
        public string wrdiff { get; set; }                  // top10
        public string r2Diff { get; set; }                  // wr/wrcp/wrb/top10
    }

    public class PlayerOldestRecordsDatum
    {
        public BasicInfo basicInfo { get; set; }
        public List<PlayerOldRecord> records { get; set; }
    }

    public class PlayerOldestRecordsRootObject
    {
        public string status { get; set; }
        public PlayerOldestRecordsDatum data { get; set; }
    }

    // PLAYER COMPLETIONS

    public class PlayerCompletionRecord
    {
        public string mapName { get; set; }
        public string mapID { get; set; }
        public string mapType { get; set; }
        public string cp_count { get; set; }
        public string b_count { get; set; }
        public string tier { get; set; }
        public string completedZones { get; set; }
        public string totalZones { get; set; }
    }

    public class PlayerMapCompletionDatum
    {
        public BasicInfo basicInfo { get; set; }
        public List<PlayerCompletionRecord> records { get; set; }
    }

    public class PlayerMapsCompletionRootObject
    {
        public string status { get; set; }
        public PlayerMapCompletionDatum data { get; set; }
    }

    #endregion
}

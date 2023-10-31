using System.Collections.Generic;

namespace KSF_Surf.Models
{
    // Classes for KSF API response JSON deserialization

    // LivePage -------------------------------------------------------------------------------
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

    public class ServerDatum 
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

    public class ServersRoot
    {
        public string status { get; set; }
        public List<ServerDatum> data { get; set; }
    }

    #endregion  
    // MapsPage -------------------------------------------------------------------------------
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

    public class DetailedMapsRoot
    {
        public string status { get; set; }
        public List<DetailedMapDatum> data { get; set; }
    }

    // MAP INFO

    public class MapSettingsDatum
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

    public class MapperDatum
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
        public MapSettingsDatum MapSettings { get; set; }
        public List<MapperDatum> Mappers { get; set; }
    }

    public class MapInfoRoot
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

    public class MapTopsRoot
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

    public class MapPersonalRecordInfoDatum
    {
        public string mapID { get; set; }
        public string tier { get; set; }
        public string stageID { get; set; }
        public int zoneID { get; set; }
        public string playerID { get; set; }
        public BasicInfoDatum basicInfo { get; set; }
        public string recordID { get; set; }
        public int? rank { get; set; }                  // 0 or null
        public string totalRanks { get; set; }
        public int? group { get; set; }                 // null if none, 0 if top10
        public string time { get; set; }            
        public string wrDiff { get; set; }          
        public string r2Diff { get; set; }              // null if no r2
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

    public class MapPersonalRecordInfoRoot
    {
        public string status { get; set; }
        public MapPersonalRecordInfoDatum data { get; set; }
    }

    // PERSONAL RECORD

    public class MapPersonalRecordDetails
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

    public class MapPersonalRecordDatum
    {
        public string mapID { get; set; }
        public string tier { get; set; }
        public BasicInfoDatum basicInfo { get; set; }
        public List<MapPersonalRecordDetails> PRInfo { get; set; }
    }

    public class MapPersonalRecordRoot
    {
        public string status { get; set; }
        public MapPersonalRecordDatum data { get; set; }
    }

    // CPR

    public class MapComparePersonalRecordDetails
    {
        public string zoneID { get; set; }
        public string WRTime { get; set; }
        public string WRTouchVel { get; set; }
        public string playerTime { get; set; }
        public string playerTouchVel { get; set; }
        public string timeDiff { get; set; }
        public string velDiff { get; set; }

    }

    public class MapComparePersonalRecordDatum
    {
        public string mapID { get; set; }
        public string stageID { get; set; }
        public string mapType { get; set; }
        public BasicInfoDatum basicInfo { get; set; }
        public BasicInfoDatum basicInfoWR { get; set; }
        public List<MapComparePersonalRecordDetails> CPR { get; set; }

    }

    public class MapComparePersonalRecordRoot
    {
        public string status { get; set; }
        public MapComparePersonalRecordDatum data { get; set; }
    }

    // CCP

    public class MapComapreCheckPointsDetails
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

    public class MapCompareCheckPointsDatum
    {
        public string mapID { get; set; }
        public string stageID { get; set; }
        public string mapType { get; set; }
        public BasicInfoDatum basicInfo { get; set; }
        public BasicInfoDatum basicInfoWR { get; set; }
        public List<MapComapreCheckPointsDetails> CCP { get; set; }

    }

    public class MapCompareCheckPointsRoot
    {
        public string status { get; set; }
        public MapCompareCheckPointsDatum data { get; set; }
    }

    #endregion
    // RecordsPage ----------------------------------------------------------------------------
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

    public class SurfTopRoot
    {
        public string status { get; set; }
        public List<SurfTopDatum> data { get; set; }
    }

    // RECENT RECORDS

    public class RecentRecordDatum
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

    public class RecentRecordsRoot
    {
        public string status { get; set; }
        public List<RecentRecordDatum> data { get; set; }
    }

    // RECENT TOP 10
    public class RecentRecord10Datum
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

    public class RecentRecords10Root
    {
        public string status { get; set; }
        public List<RecentRecord10Datum> data { get; set; }
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

    public class MostCountRoot
    {
        public string status { get; set; }
        public List<MostCountDatum> data { get; set; }
    }

    // MOST BY TYPE (CONTESTED ZONE)

    public class MostContestedZoneDatum
    {
        public string mapID { get; set; }
        public string mapName { get; set; }
        public string zoneID { get; set; }
        public string stageID { get; set; }
        public string total { get; set; }
        public string date { get; set; }
    }

    public class MostContestedZoneRoot
    {
        public string status { get; set; }
        public List<MostContestedZoneDatum> data { get; set; }
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

    public class MostGroupRoot
    {
        public string status { get; set; }
        public List<MostGroupDatum> data { get; set; }
    }

    // MOST BY TYPE (PC)

    public class MostPercentCompletionDatum
    {
        public string name { get; set; }
        public string playerID { get; set; }
        public string steamID { get; set; }
        public string country { get; set; }
        public string lastonline { get; set; }
        public string percentCompletion { get; set; }
    }

    public class MostPercentCompletionRoot
    {
        public string status { get; set; }
        public List<MostPercentCompletionDatum> data { get; set; }
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

    public class MostTopsRoot
    {
        public string status { get; set; }
        public List<MostTopDatum> data { get; set; }
    }

    // MOST BY TYPE (CONTESTED WR)

    public class MostContestedWorldRecordDatum
    {
        public string mapID { get; set; }
        public string mapName { get; set; }
        public string total { get; set; }
        public string date { get; set; }
    }

    public class MostContestedWorldRecordsRoot
    {
        public string status { get; set; }
        public List<MostContestedWorldRecordDatum> data { get; set; }
    }

    // MOST BY TYPE (TIME)

    public class MostTimeDatum
    {
        public string mapID { get; set; }
        public string mapName { get; set; }
        public int totalplaytime { get; set; }
    }

    public class MostTimeRoot
    {
        public string status { get; set; }
        public List<MostTimeDatum> data { get; set; }
    }

    // OLDEST RECORDS

    public class OldestRecordDatum
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

    public class OldestRecordsRoot
    {
        public string status { get; set; }
        public List<OldestRecordDatum> data { get; set; }
    }

    // TOP COUNTRIES

    public class TopCountryDatum
    {
        public string country { get; set; }
        public string points { get; set; }
    }

    public class TopCountriesRoot
    {
        public string status { get; set; }
        public List<TopCountryDatum> data { get; set; }
    }

    // COUNTRY TOP

    public class CountryTopDatum
    {
        public string playerName { get; set; }
        public string points { get; set; }
        public string playerID { get; set; }
        public string steamID { get; set; }
        public string lastonline { get; set; }
    }

    public class CountryTopsObject
    {
        public string status { get; set; }
        public List<CountryTopDatum> data { get; set; }
    }

    #endregion
    // PlayerPage -----------------------------------------------------------------------------
    #region PlayerPage

    // PLAYER INFO

    public class TotalZonesDatum
    {
        public string TotalMaps { get; set; }
        public string TotalStages { get; set; }
        public string TotalBonuses { get; set; }
    }

    public class WorldRecordZonesDatum
    {
        public string wr { get; set; }
        public string wrcp { get; set; }
        public string wrb { get; set; }
    }

    public class Top10GroupsDatum
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

    public class CompletedZonesDatum
    {
        public string map { get; set; }
        public string stage { get; set; }
        public string bonus { get; set; }
    }

    public class PlayerPointsDatum
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

    public class BasicInfoDatum
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
        public bool banStatus { get; set; }
        public bool muteStatus { get; set; }
        public BasicInfoDatum basicInfo { get; set; }
        
        public string KSFStatus { get; set; }           // "member" or null
        public string vipStatus { get; set; }           // "active" or null
        public string adminStatus { get; set; }         // "active" or null
        public string mapperID { get; set; }            // "<number>" or null
        public PlayerPointsDatum playerPoints { get; set; }
        public CompletedZonesDatum CompletedZones { get; set; }
        public Top10GroupsDatum Top10Groups { get; set; }
        public WorldRecordZonesDatum WRZones { get; set; }
        public string SurfRank { get; set; }
        public string SurfTotalRank { get; set; }
        public string percentCompletion { get; set; }
        public TotalZonesDatum TotalZones { get; set; }
    }

    public class PlayerInfoRoot
    {
        public string status { get; set; }
        public PlayerInfoDatum data { get; set; }
    }

    // PLAYER RECENT RECORDS

    public class PlayerRecentRecordDatum
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

    public class PlayerRecentRecordsDatum
    {
        public BasicInfoDatum basicInfo { get; set; }
        public List<PlayerRecentRecordDatum> recentRecords { get; set; }
    }

    public class PlayerRecentRecordsRoot
    {
        public string status { get; set; }
        public PlayerRecentRecordsDatum data { get; set; }
    }

    // PLAYER WORLD RECORDS

    public class PlayerWorldRecordDatum
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

    public class PlayerWorldRecordsDatum
    {
        public BasicInfoDatum basicInfo { get; set; }
        public List<PlayerWorldRecordDatum> records { get; set; }
    }

    public class PlayerWorldRecordsRoot
    {
        public string status { get; set; }
        public PlayerWorldRecordsDatum data { get; set; }
    }

    // PLAYER COMPLETION BY TIER

    public class PlayerTierCompletionDatum
    {
        public string tier { get; set; }
        public string map { get; set; }
        public string mapTotal { get; set; }
        public string stage { get; set; }
        public string stageTotal { get; set; }
        public string bonus { get; set; }
        public string bonusTotal { get; set; }
    }

    public class PlayerTierCompletionsDatum
    {
        public BasicInfoDatum basicInfo { get; set; }
        public List<PlayerTierCompletionDatum> records { get; set; }
    }

    public class PlayerTierCompletionRoot
    {
        public string status { get; set; }
        public PlayerTierCompletionsDatum data { get; set; }
    }

    // PLAYER OLDEST RECORDS

    public class PlayerOldestRecordDatum
    {
        public string mapName { get; set; }
        public string mapID { get; set; }
        public string zoneID { get; set; }              // wrcp/wrb stage/bonus
        public string stageID { get; set; }
        public string finishType { get; set; }
        public string rank { get; set; }                // top10
        public string top10Points { get; set; }         // top10
        public string groupPoints { get; set; }         // top10
        public string points { get; set; }              // map
        public string top10Group { get; set; }          // map
        public string date { get; set; }
        public string dateNow { get; set; }
        public string surfTime { get; set; }
        public string count { get; set; }               // map/stage/bonus
        public string wrdiff { get; set; }              // top10
        public string r2Diff { get; set; }              // wr/wrcp/wrb/top10
    }

    public class PlayerOldestRecordsDatum
    {
        public BasicInfoDatum basicInfo { get; set; }
        public List<PlayerOldestRecordDatum> records { get; set; }
    }

    public class PlayerOldestRecordsRoot
    {
        public string status { get; set; }
        public PlayerOldestRecordsDatum data { get; set; }
    }

    // PLAYER COMPLETIONS

    public class PlayerMapCompletionDatum
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

    public class PlayerMapCompletionsDatum
    {
        public BasicInfoDatum basicInfo { get; set; }
        public List<PlayerMapCompletionDatum> records { get; set; }
    }

    public class PlayerMapCompletionsRoot
    {
        public string status { get; set; }
        public PlayerMapCompletionsDatum data { get; set; }
    }

    #endregion
}

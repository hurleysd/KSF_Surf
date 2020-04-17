using System.Collections.Generic;

namespace KSF_Surf.Models
{
    // Classes for KSF API response JSON deserialization-----------------------------------

    // SERVER LIST

    public class KSFServerDatum
    {
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
        public List<object> players { get; set; }
    }

    public class KSFServerRootObject
    {
        public string status { get; set; }
        public List<KSFServerDatum> data { get; set; }
    }

    // DETAILED MAP

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
}

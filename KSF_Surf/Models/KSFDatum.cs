using System;
using System.Collections.Generic;

namespace KSF_Surf.Models
{
    public enum EFilter_Game
    {
        none, css, css100t, csgo
    }

    public enum EFilter_Sort
    {
        none, name, created, lastplayed, playtime, popularity
    }

    public enum EFilter_MapType
    {
        none = -1, any = 0, linear = 1, staged = 2
    }

    public enum EFilter_Mode
    {
        fw = 0, sw = 1, hsw = 2, bw = 3
    }

    public static class EFilter_ToString
    {
        public static string toString(EFilter_Game game)
        {
            string gameString = "";
            switch (game)
            {
                case EFilter_Game.css: gameString = "css"; break;
                case EFilter_Game.css100t: gameString = "css100t"; break;
                case EFilter_Game.csgo: gameString = "csgo"; break;
                default: break;
            }
            return gameString;
        }

        public static string toString(EFilter_Sort sort)
        {
            string sortString = "";
            switch (sort)
            {
                case EFilter_Sort.name: sortString = "name"; break;
                case EFilter_Sort.created: sortString = "created"; break;
                case EFilter_Sort.lastplayed: sortString = "lastplayed"; break;
                case EFilter_Sort.playtime: sortString = "playtime"; break;
                case EFilter_Sort.popularity: sortString = "popularity"; break;
                default: break;
            }
            return sortString;
        }

        public static string toString(EFilter_Mode mode)
        {
            string modeString = "";
            switch (mode)
            {
                case EFilter_Mode.fw: modeString = "FW"; break;
                case EFilter_Mode.hsw: modeString = "HSW"; break;
                case EFilter_Mode.sw: modeString = "SW"; break;
                case EFilter_Mode.bw: modeString = "BW"; break;
                default: break;
            }
            return modeString;
        }
    }

    public static class Seconds_Formatter
    {
        public static string toString_PlayTime(string seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(Int64.Parse(seconds));
            return time.ToString(@"d\.h\:mm\:ss");
        }

        public static string toString_RankTime(string seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(double.Parse(seconds));
            return time.ToString(@"m\:ss\.FFFFFF");
        }
    }

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
}
using System;

namespace KSF_Surf.Models
{
    // FILTER ENUMS -------------------------------------------------------------------------
    #region filters
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

    public enum EFilter_RRType
    {
        all, map, top, stage, bonus
    }

    public enum EFilter_MostType
    {
        pc,                                                 // uses MostPC
        wr, wrcp, wrb, mostwr, mostwrcp, mostwrb ,          // uses MostCount 
        top10,                                              // uses MostTop
        group,                                              // uses MostGroup
        mostcontestedwr,                                    // uses MostContWr
        mostcontestedwrcp, mostcontestedwrb,                // uses MostContZone
        playtimeday, playtimeweek, playtimemonth            // uses MostTime
    }

    #endregion
    // TOSTRING -----------------------------------------------------------------------------
    #region ToString

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

        public static string toString2(EFilter_Game game)
        {
            string gameString = "";
            switch (game)
            {
                case EFilter_Game.css: gameString = "CS:S"; break;
                case EFilter_Game.css100t: gameString = "CS:S 100T"; break;
                case EFilter_Game.csgo: gameString = "CS:GO"; break;
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

        public static readonly string[] modes_arr = new string[] { "FW", "HSW", "SW", "BW" };

        public static string toString(EFilter_Mode mode)
        {
            string modeString = "";
            switch (mode)
            {
                case EFilter_Mode.fw: modeString = modes_arr[0]; break;
                case EFilter_Mode.hsw: modeString = modes_arr[1]; break;
                case EFilter_Mode.sw: modeString = modes_arr[2]; break;
                case EFilter_Mode.bw: modeString = modes_arr[3]; break;
                default: break;
            }
            return modeString;
        }

        public static string toString(EFilter_RRType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_RRType.all: typeString = "all"; break;
                case EFilter_RRType.map: typeString = "map"; break;
                case EFilter_RRType.top: typeString = "top10"; break;
                case EFilter_RRType.stage: typeString = "stage"; break;
                case EFilter_RRType.bonus: typeString = "bonus"; break;
                default: break;
            }
            return typeString;
        }

        public static readonly string[] rrtype_arr = new string[] { "Map", "Top10", "Stage", "Bonus", "All" };

        public static string toString2(EFilter_RRType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_RRType.all: typeString = rrtype_arr[4]; break;
                case EFilter_RRType.map: typeString = rrtype_arr[0]; break;
                case EFilter_RRType.top: typeString = rrtype_arr[1]; break;
                case EFilter_RRType.stage: typeString = rrtype_arr[2]; break;
                case EFilter_RRType.bonus: typeString = rrtype_arr[3]; break;
                default: break;
            }
            return typeString;
        }

        public static string toString(EFilter_MostType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_MostType.pc: typeString = "pc"; break;
                case EFilter_MostType.wr: typeString = "wr"; break;
                case EFilter_MostType.wrcp: typeString = "wrcp"; break;
                case EFilter_MostType.wrb: typeString = "wrb"; break;
                case EFilter_MostType.top10: typeString = "top10"; break;
                case EFilter_MostType.group: typeString = "group"; break;
                case EFilter_MostType.mostwr: typeString = "mostwr"; break;
                case EFilter_MostType.mostwrcp: typeString = "mostwrcp"; break;
                case EFilter_MostType.mostwrb: typeString = "mostwrb"; break;
                case EFilter_MostType.mostcontestedwr: typeString = "mostcontestedwr"; break;
                case EFilter_MostType.mostcontestedwrcp: typeString = "mostcontestedwrcp"; break;
                case EFilter_MostType.mostcontestedwrb: typeString = "mostcontestedwrb"; break;
                case EFilter_MostType.playtimeday: typeString = "playtimeday"; break;
                case EFilter_MostType.playtimeweek: typeString = "playtimeweek"; break;
                case EFilter_MostType.playtimemonth: typeString = "playtimemonth"; break;
                default: break;
            }
            return typeString;
        }

        public static readonly string[] mosttype_arr = new string[] { "Completion", "Current WRs", "Current WRCPs", "Current WRBs",
            "Top10 Points",  "Group Points", "Broken WRs", "Broken WRCPs", "Broken WRBs", "Contested WR", "Contested WRCP", "Contested WRB",
            "Play Time Day", "Play Time Week", "Play Time Month" };

        public static string toString2(EFilter_MostType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_MostType.pc: typeString = mosttype_arr[0]; break;
                case EFilter_MostType.wr: typeString = mosttype_arr[1]; break;
                case EFilter_MostType.wrcp: typeString = mosttype_arr[2]; break;
                case EFilter_MostType.wrb: typeString = mosttype_arr[3]; break;
                case EFilter_MostType.top10: typeString = mosttype_arr[4]; break;
                case EFilter_MostType.group: typeString = mosttype_arr[5]; break;
                case EFilter_MostType.mostwr: typeString = mosttype_arr[6]; break;
                case EFilter_MostType.mostwrcp: typeString = mosttype_arr[7]; break;
                case EFilter_MostType.mostwrb: typeString = mosttype_arr[8]; break;
                case EFilter_MostType.mostcontestedwr: typeString = mosttype_arr[9]; break;
                case EFilter_MostType.mostcontestedwrcp: typeString = mosttype_arr[10]; break;
                case EFilter_MostType.mostcontestedwrb: typeString = mosttype_arr[11]; break;
                case EFilter_MostType.playtimeday: typeString = mosttype_arr[12]; break;
                case EFilter_MostType.playtimeweek: typeString = mosttype_arr[13]; break;
                case EFilter_MostType.playtimemonth: typeString = mosttype_arr[14]; break;
                default: break;
            }
            return typeString;
        }

        public static string zoneFormatter(string z)
        {
            string zoneString = "Main";

            int zone = int.Parse(z);
            if (zone != 0) // if not main
            {
                if (zone < 31) // stage
                {
                    zoneString = "S" + zone;
                }
                else // bonus
                {
                    zoneString = "B" + (zone - 30);
                }
            }
            return zoneString;
        }
    }

    public static class Seconds_Formatter
    {
        public static string toString_PlayTime(string seconds, bool abbreviate)
        {
            TimeSpan time = TimeSpan.FromSeconds(Int64.Parse(seconds));
            if (abbreviate)
            {
                if (time.Days > 0)
                {
                    return String.Format("{0:##}d {1:#0}:{2:00}:{3:00}", time.Days, time.Hours, time.Minutes, time.Seconds);
                }
                return String.Format("{0:#0}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);
            }
            if (time.Days > 0)
            {
                return String.Format("{0:##}d {0:#0}h {1:#0}m {2:#0}s", time.Days, time.Hours, time.Minutes, time.Seconds);
            }
            else if (time.Hours > 0)
            {
                return String.Format("{0:##}h {1:#0}m {2:#0}s", time.Hours, time.Minutes, time.Seconds);
            }
            return String.Format("{0:#0}m {1:#0}s", time.Minutes, time.Seconds);
        }

        public static string toString_RankTime(string seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(double.Parse(seconds));
            return String.Format("{0:#0}:{1:00}.{2:00}", time.Minutes, time.Seconds, (int)(time.Milliseconds/10));
        }
    }

    #endregion
}
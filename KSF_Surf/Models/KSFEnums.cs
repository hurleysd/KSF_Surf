using System;

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

        public static string toString2(EFilter_RRType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_RRType.all: typeString = "All"; break;
                case EFilter_RRType.map: typeString = "Map"; break;
                case EFilter_RRType.top: typeString = "Top10"; break;
                case EFilter_RRType.stage: typeString = "Stage"; break;
                case EFilter_RRType.bonus: typeString = "Bonus"; break;
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

        public static string toString2(EFilter_MostType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_MostType.pc: typeString = "Percent Completion"; break;
                case EFilter_MostType.wr: typeString = "Current WRs"; break;
                case EFilter_MostType.wrcp: typeString = "Current WRCPs"; break;
                case EFilter_MostType.wrb: typeString = "Current WRBs"; break;
                case EFilter_MostType.top10: typeString = "Top10 Points"; break;
                case EFilter_MostType.group: typeString = "Group Points"; break;
                case EFilter_MostType.mostwr: typeString = "Broken WRs"; break;
                case EFilter_MostType.mostwrcp: typeString = "Broken WRCPs"; break;
                case EFilter_MostType.mostwrb: typeString = "Broken WRBs"; break;
                case EFilter_MostType.mostcontestedwr: typeString = "Contested WR"; break;
                case EFilter_MostType.mostcontestedwrcp: typeString = "Contested WRCP"; break;
                case EFilter_MostType.mostcontestedwrb: typeString = "Contested WRB"; break;
                case EFilter_MostType.playtimeday: typeString = "Play Time Today"; break;
                case EFilter_MostType.playtimeweek: typeString = "Play Time This Week"; break;
                case EFilter_MostType.playtimemonth: typeString = "Play Time This Month"; break;
                default: break;
            }
            return typeString;
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
            return time.ToString(@"m\:ss\.FF");
        }
    }
}
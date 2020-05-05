using System;

using Xamarin.Forms;

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
        none = -1, fw = 0, sw = 1, hsw = 2, bw = 3
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

    public enum EFilter_ORType
    {
        map, stage, bonus
    }

    public enum EFilter_PlayerType
    { 
        none, steamid, rank, me
    }

    public enum EFilter_PlayerRecordsType
    {
        set, broken
    }

    public enum EFilter_PlayerWRsType
    {
        none, wr, wrcp, wrb
    }

    public enum EFilter_PlayerCompletionType
    {
        complete, incomplete, completionbytier
    }

    public enum EFilter_PlayerOldestType
    {
        wr, wrcp, wrb, top10, map, stage, bonus
    }

    #endregion
}
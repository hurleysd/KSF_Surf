namespace KSF_Surf.Models
{
    public enum GameEnum
    {
        NONE,
        CSS,
        CSS100T,
        CSGO
    }

    public enum SortEnum
    {
        NONE,
        NAME,
        CREATED,
        LAST_PLAYED,
        PLAYTIME,
        POPULARITY
    }

    public enum MapTypeEnum
    {
        NODE = -1,
        ANY = 0,
        LINEAR = 1,
        STAGED = 2
    }

    public enum ModeEnum
    {
        NONE = -1,
        FW = 0,
        SW = 1,
        HSW = 2,
        BW = 3
    }

    public enum RecentRecordsTypeEnum
    {
        ALL,
        MAP,
        TOP,
        STAGE,
        BONUS
    }

    public enum MostTypeEnum
    {
        PC,                                                 // uses MostPC
        WR, WRCP, WRB, MOST_WR, MOST_WRCP, MOST_WRB,        // uses MostCount 
        TOP10,                                              // uses MostTop
        GROUP,                                              // uses MostGroup
        MOST_CONTESTED_WR,                                  // uses MostContWr
        MOST_CONTESTED_WRCP, MOST_CONTESTED_WRB,            // uses MostContZone
        PLAYTIME_DAY, PLAYTIME_WEEK, PLAYTIME_MONTH         // uses MostTime
    }

    public enum OldestRecordsTypeEnum
    {
        MAP,
        STAGE,
        BONUS
    }

    public enum PlayerTypeEnum
    { 
        NONE,
        STEAM_ID,
        RANK,
        ME
    }

    public enum PlayerRecordsTypeEnum
    {
        SET,
        BROKEN
    }

    public enum PlayerWorldRecordsTypeEnum
    {
        NONE,
        WR,
        WRCP,
        WRB
    }

    public enum PlayerCompletionTypeEnum
    {
        COMPLETE,
        INCOMPLETE,
        COMPLETION_BY_TIER
    }

    public enum PlayerOldestRecordsTypeEnum
    {
        WR,
        WRCP,
        WRB,
        TOP10,
        MAP,
        STAGE,
        BONUS
    }

    public enum RecordComparisonTypeEnum
    {
        CPR,
        CCP
    }
}
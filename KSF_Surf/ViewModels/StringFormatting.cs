using System;
using Xamarin.Forms;
using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    // Enum to string -------------------------------------------------------------------------
    #region enumtostring

    public static class EnumToString
    {

        // GAME -----------------------------------------------------------------------------

        public static string APIString(GameEnum game)
        {
            string gameString = "";
            switch (game)
            {
                case GameEnum.CSS: gameString = "css"; break;
                case GameEnum.CSS100T: gameString = "css100t"; break;
                case GameEnum.CSGO: gameString = "csgo"; break;
                default: break;
            }
            return gameString;
        }

        public static string NameString(GameEnum game)
        {
            string gameString = "";
            switch (game)
            {
                case GameEnum.CSS: gameString = "CS:S"; break;
                case GameEnum.CSS100T: gameString = "CS:S 100T"; break;
                case GameEnum.CSGO: gameString = "CS:GO"; break;
                default: break;
            }
            return gameString;
        }

        // SORT -----------------------------------------------------------------------------

        public static string APIString(SortEnum sort)
        {
            string sortString = "";
            switch (sort)
            {
                case SortEnum.NAME: sortString = "name"; break;
                case SortEnum.CREATED: sortString = "created"; break;
                case SortEnum.LAST_PLAYED: sortString = "lastplayed"; break;
                case SortEnum.PLAYTIME: sortString = "playtime"; break;
                case SortEnum.POPULARITY: sortString = "popularity"; break;
                default: break;
            }
            return sortString;
        }

        // MAP TYPE ---------------------------------------------------------------------------

        public static string NameString(MapTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case MapTypeEnum.LINEAR: typeString = "Linear"; break;
                case MapTypeEnum.STAGED: typeString = "Staged"; break;
                default: break;
            }
            return typeString;
        }

        // MODE -------------------------------------------------------------------------------

        public static readonly string[] ModeNames = { 
            "FW",
            "HSW",
            "SW",
            "BW"
        };

        public static readonly string[] ModeFullNames = {
            "Forwards",
            "Half-Sideways",
            "Sideways",
            "Backwards"
        };

        public static string APIString(ModeEnum mode)
        {
            string modeString = "";
            switch (mode)
            {
                case ModeEnum.FW: modeString = "fw"; break;
                case ModeEnum.HSW: modeString = "hsw"; break;
                case ModeEnum.SW: modeString = "sw"; break;
                case ModeEnum.BW: modeString = "bw"; break;
                default: break;
            }
            return modeString;
        }

        public static string NameString(ModeEnum mode)
        {
            string modeString = "";
            switch (mode)
            {
                case ModeEnum.FW: modeString = ModeNames[0]; break;
                case ModeEnum.HSW: modeString = ModeNames[1]; break;
                case ModeEnum.SW: modeString = ModeNames[2]; break;
                case ModeEnum.BW: modeString = ModeNames[3]; break;
                default: break;
            }
            return modeString;
        }

        public static string FullNameString(ModeEnum mode)
        {
            string modeString = "";
            switch (mode)
            {
                case ModeEnum.FW: modeString = ModeFullNames[0]; break;
                case ModeEnum.HSW: modeString = ModeFullNames[1]; break;
                case ModeEnum.SW: modeString = ModeFullNames[2]; break;
                case ModeEnum.BW: modeString = ModeFullNames[3]; break;
                default: break;
            }
            return modeString;
        }

        // RECENT RECORDS TYPE ----------------------------------------------------------------

        public static readonly string[] RecentRecordsTypeNames = { 
            "Map",
            "Top10",
            "Stage",
            "Bonus",
            "All WRs"
        };

        public static string APIString(RecentRecordsTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case RecentRecordsTypeEnum.ALL: typeString = "all"; break;
                case RecentRecordsTypeEnum.MAP: typeString = "map"; break;
                case RecentRecordsTypeEnum.TOP: typeString = "top10"; break;
                case RecentRecordsTypeEnum.STAGE: typeString = "stage"; break;
                case RecentRecordsTypeEnum.BONUS: typeString = "bonus"; break;
                default: break;
            }
            return typeString;
        }

        public static string NameString(RecentRecordsTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case RecentRecordsTypeEnum.ALL: typeString = RecentRecordsTypeNames[4]; break;
                case RecentRecordsTypeEnum.MAP: typeString = RecentRecordsTypeNames[0]; break;
                case RecentRecordsTypeEnum.TOP: typeString = RecentRecordsTypeNames[1]; break;
                case RecentRecordsTypeEnum.STAGE: typeString = RecentRecordsTypeNames[2]; break;
                case RecentRecordsTypeEnum.BONUS: typeString = RecentRecordsTypeNames[3]; break;
                default: break;
            }
            return typeString;
        }

        // MOST TYPE --------------------------------------------------------------------------

        public static readonly string[] MostTypeNames = new string[] {
            "Completion",
            "Current WRs",
            "Current WRCPs",
            "Current WRBs",
            "Top10 Points",
            "Group Points",
            "Broken WRs",
            "Broken WRCPs",
            "Broken WRBs",
            "Contested WR",
            "Contested WRCP",
            "Contested WRB",
            "Play Time Day",
            "Play Time Week",
            "Play Time Month"
        };

        public static string APIString(MostTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case MostTypeEnum.PC: typeString = "pc"; break;
                case MostTypeEnum.WR: typeString = "wr"; break;
                case MostTypeEnum.WRCP: typeString = "wrcp"; break;
                case MostTypeEnum.WRB: typeString = "wrb"; break;
                case MostTypeEnum.TOP10: typeString = "top10"; break;
                case MostTypeEnum.GROUP: typeString = "group"; break;
                case MostTypeEnum.MOST_WR: typeString = "mostwr"; break;
                case MostTypeEnum.MOST_WRCP: typeString = "mostwrcp"; break;
                case MostTypeEnum.MOST_WRB: typeString = "mostwrb"; break;
                case MostTypeEnum.MOST_CONTESTED_WR: typeString = "mostcontestedwr"; break;
                case MostTypeEnum.MOST_CONTESTED_WRCP: typeString = "mostcontestedwrcp"; break;
                case MostTypeEnum.MOST_CONTESTED_WRB: typeString = "mostcontestedwrb"; break;
                case MostTypeEnum.PLAYTIME_DAY: typeString = "playtimeday"; break;
                case MostTypeEnum.PLAYTIME_WEEK: typeString = "playtimeweek"; break;
                case MostTypeEnum.PLAYTIME_MONTH: typeString = "playtimemonth"; break;
                default: break;
            }
            return typeString;
        }

        public static string NameString(MostTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case MostTypeEnum.PC: typeString = MostTypeNames[0]; break;
                case MostTypeEnum.WR: typeString = MostTypeNames[1]; break;
                case MostTypeEnum.WRCP: typeString = MostTypeNames[2]; break;
                case MostTypeEnum.WRB: typeString = MostTypeNames[3]; break;
                case MostTypeEnum.TOP10: typeString = MostTypeNames[4]; break;
                case MostTypeEnum.GROUP: typeString = MostTypeNames[5]; break;
                case MostTypeEnum.MOST_WR: typeString = MostTypeNames[6]; break;
                case MostTypeEnum.MOST_WRCP: typeString = MostTypeNames[7]; break;
                case MostTypeEnum.MOST_WRB: typeString = MostTypeNames[8]; break;
                case MostTypeEnum.MOST_CONTESTED_WR: typeString = MostTypeNames[9]; break;
                case MostTypeEnum.MOST_CONTESTED_WRCP: typeString = MostTypeNames[10]; break;
                case MostTypeEnum.MOST_CONTESTED_WRB: typeString = MostTypeNames[11]; break;
                case MostTypeEnum.PLAYTIME_DAY: typeString = MostTypeNames[12]; break;
                case MostTypeEnum.PLAYTIME_WEEK: typeString = MostTypeNames[13]; break;
                case MostTypeEnum.PLAYTIME_MONTH: typeString = MostTypeNames[14]; break;
                default: break;
            }
            return typeString;
        }

        // OLDEST RECORDS TYPE ----------------------------------------------------------------

        public static readonly string[] OldestRecordsTypeNames = { 
            "Map",
            "Stage",
            "Bonus"
        };

        public static string APIString(OldestRecordsTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case OldestRecordsTypeEnum.MAP: typeString = "map"; break;
                case OldestRecordsTypeEnum.STAGE: typeString = "stage"; break;
                case OldestRecordsTypeEnum.BONUS: typeString = "bonus"; break;
                default: break;
            }
            return typeString;
        }

        public static string NameString(OldestRecordsTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case OldestRecordsTypeEnum.MAP: typeString = OldestRecordsTypeNames[0]; break;
                case OldestRecordsTypeEnum.STAGE: typeString = OldestRecordsTypeNames[1]; break;
                case OldestRecordsTypeEnum.BONUS: typeString = OldestRecordsTypeNames[2]; break;
                default: break;
            }
            return typeString;
        }

        // PLAYER TYPE ------------------------------------------------------------------------

        public static string APIString(PlayerTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case PlayerTypeEnum.STEAM_ID: typeString = "steamid"; break;
                case PlayerTypeEnum.RANK: typeString = "rank"; break;
                case PlayerTypeEnum.ME: goto case PlayerTypeEnum.STEAM_ID;
                default: break;
            }
            return typeString;
        }

        // PLAYER RECORDS TYPE ----------------------------------------------------------------

        public static string APIString(PlayerRecordsTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case PlayerRecordsTypeEnum.SET: typeString = "recordset"; break;
                case PlayerRecordsTypeEnum.BROKEN: typeString = "recordbroken"; break;
                default: break;
            }
            return typeString;
        }

        // PLAYER WORLD RECORDS TYPE ----------------------------------------------------------

        public static readonly string[] WorldRecordsTypeAPIs = { "wr", "wrcp", "wrb" };
        public static readonly string[] WorldRecordsTypeNames = { "WR", "WRCP", "WRB" };

        public static string APIString(PlayerWorldRecordsTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case PlayerWorldRecordsTypeEnum.WR: typeString = WorldRecordsTypeAPIs[0]; break;
                case PlayerWorldRecordsTypeEnum.WRCP: typeString = WorldRecordsTypeAPIs[1]; break;
                case PlayerWorldRecordsTypeEnum.WRB: typeString = WorldRecordsTypeAPIs[2]; break;
                default: break;
            }
            return typeString;
        }

        public static string NameString(PlayerWorldRecordsTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case PlayerWorldRecordsTypeEnum.WR: typeString = WorldRecordsTypeNames[0]; break;
                case PlayerWorldRecordsTypeEnum.WRCP: typeString = WorldRecordsTypeNames[1]; break;
                case PlayerWorldRecordsTypeEnum.WRB: typeString = WorldRecordsTypeNames[2]; break;
                default: break;
            }
            return typeString;
        }

        // PLAYER COMPLETION TYPE -------------------------------------------------------------

        public static string APIString(PlayerCompletionTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case PlayerCompletionTypeEnum.COMPLETE: typeString = "complete"; break;
                case PlayerCompletionTypeEnum.INCOMPLETE: typeString = "incomplete"; break;
                case PlayerCompletionTypeEnum.COMPLETION_BY_TIER: typeString = "completionbytier"; break;
                default: break;
            }
            return typeString;
        }

        public static string NameString(PlayerCompletionTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case PlayerCompletionTypeEnum.COMPLETE: typeString = "Complete Maps"; break;
                case PlayerCompletionTypeEnum.INCOMPLETE: typeString = "Incomplete Maps"; break;
                default: break;
            }
            return typeString;
        }

        // PLAYER OLDEST RECORDS TYPE ---------------------------------------------------------

        public static readonly string[] PlayerOldestRecordsTypeNames = {
            "WR",
            "WRCP",
            "WRB",
            "Top10",
            "Map",
            "Stage",
            "Bonus"
        };

        public static string APIString(PlayerOldestRecordsTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case PlayerOldestRecordsTypeEnum.WR: typeString = "oldestwr"; break;
                case PlayerOldestRecordsTypeEnum.WRCP: typeString = "oldestwrcp"; break;
                case PlayerOldestRecordsTypeEnum.WRB: typeString = "oldestwrb"; break;
                case PlayerOldestRecordsTypeEnum.TOP10: typeString = "oldesttop10"; break;
                case PlayerOldestRecordsTypeEnum.MAP: typeString = "oldestmap"; break;
                case PlayerOldestRecordsTypeEnum.STAGE: typeString = "oldeststage"; break;
                case PlayerOldestRecordsTypeEnum.BONUS: typeString = "oldestbonus"; break;
                default: break;
            }
            return typeString;
        }

        public static string NameString(PlayerOldestRecordsTypeEnum type)
        {
            string typeString = "";
            switch (type)
            {
                case PlayerOldestRecordsTypeEnum.WR: typeString = PlayerOldestRecordsTypeNames[0]; break;
                case PlayerOldestRecordsTypeEnum.WRCP: typeString = PlayerOldestRecordsTypeNames[1]; break;
                case PlayerOldestRecordsTypeEnum.WRB: typeString = PlayerOldestRecordsTypeNames[2]; break;
                case PlayerOldestRecordsTypeEnum.TOP10: typeString = PlayerOldestRecordsTypeNames[3]; break;
                case PlayerOldestRecordsTypeEnum.MAP: typeString = PlayerOldestRecordsTypeNames[4]; break;
                case PlayerOldestRecordsTypeEnum.STAGE: typeString = PlayerOldestRecordsTypeNames[5]; break;
                case PlayerOldestRecordsTypeEnum.BONUS: typeString = PlayerOldestRecordsTypeNames[6]; break;
                default: break;
            }
            return typeString;
        }
    }

    #endregion
    // Formatting -----------------------------------------------------------------------------
    #region formatting

    public static class StringFormatter
    {

        // STEAM ID ---------------------------------------------------------------------------

        public static string Steam32to64(string steam32)
        {
            string[] steam32Split = steam32.Split(':');
            if (steam32Split.Length != 3) return "";

            long convertedTo64Bit;
            try
            {
                convertedTo64Bit = long.Parse(steam32Split[2]) * 2;
                convertedTo64Bit += 76561197960265728; // Valve's magic constant
                convertedTo64Bit += long.Parse(steam32Split[1]);
            }
            catch (Exception)
            {
                return "";
            }

            return convertedTo64Bit.ToString();
        }

        // ZONES ------------------------------------------------------------------------------

        public static string ZoneString(string z, bool includeMain, bool fullName)
        {
            int zone = int.Parse(z);
            string zoneString = "";

            if (zone != 0) // if not main
            {
                if (zone < 31) // stage
                {
                    if (fullName) zoneString = "Stage " + zone;
                    else zoneString = "S" + zone;
                }
                else // bonus
                {
                    if (fullName) zoneString = "Bonus " + (zone - 30);
                    else zoneString = "B" + (zone - 30);
                }
            }
            else if (includeMain) zoneString = "Main";

            return zoneString;
        }

        public static string CPRZoneString(string z, MapTypeEnum mapType)
        {
            int zone = int.Parse(z);
            string zoneString = "";

            if (zone == 1) return "Start";
            else if (zone == 0) return "End";

            if (mapType == MapTypeEnum.STAGED) zoneString = "Stage " + zone;
            else if (mapType == MapTypeEnum.LINEAR) zoneString = "Checkpoint " + (zone - 1);

            return zoneString;
        }

        // RANKS ------------------------------------------------------------------------------

        public static readonly string[] RankTitles = { 
            "MASTER",
            "ELITE",
            "VETERAN",
            "PRO",
            "EXPERT",
            "HOTSHOT",
            "EXCEPTIONAL",
            "SEASONED",
            "EXPERIENCED",
            "ACCOMPLISHED",
            "ADEPT",
            "PROFICIENT",
            "SKILLED",
            "CASUAL",
            "BEGINNER",
            "ROOKIE"
        };

        public static readonly Color[] RankColors = {
            Color.Magenta,
            Color.HotPink,
            Color.Red,
            Color.Orange,
            Color.Gold,
            Color.GreenYellow,
            Color.LimeGreen,
            Color.MediumSpringGreen,
            Color.LightSeaGreen,
            Color.SkyBlue,
            Color.RoyalBlue,
            Color.DarkSlateBlue,
            Color.DarkOliveGreen,
            Color.DarkGoldenrod,
            Color.SaddleBrown,
            Color.Gray
        };

        public static string RankTitleString(string rankString, string pointsString)
        {
            int rank = int.Parse(rankString);
            double points = double.Parse(pointsString);
            string title;

            if (1 <= rank && rank <= 10) title = RankTitles[0];
            else if (rank <= 25) title = RankTitles[1];
            else if (rank <= 50) title = RankTitles[2];
            else if (rank <= 100) title = RankTitles[3];
            else if (rank <= 200) title = RankTitles[4];
            else if (rank <= 300) title = RankTitles[5];
            else if (rank <= 500) title = RankTitles[6];
            else if (rank <= 750) title = RankTitles[7];
            else if (rank <= 1500) title = RankTitles[8];
            else if (points >= 13000) title = RankTitles[9];
            else if (points >= 9000) title = RankTitles[10];
            else if (points >= 6000) title = RankTitles[11];
            else if (points >= 4000) title = RankTitles[12];
            else if (points >= 2500) title = RankTitles[13];
            else if (points >= 1000) title = RankTitles[14];
            else title = RankTitles[15];

            return title;
        }

        // COMPLETION PERCENT -----------------------------------------------------------------
    
        public static string CompletionPercentString(string completeString, string totalString)
        {
            int complete = int.Parse(completeString);
            int total = int.Parse(totalString);
            int percent = ((total == 0) ? 0 : (int)(((double)complete / total) * 100));

            return percent + "%";
        }

        // TIME -------------------------------------------------------------------------------

        public static string PlayTimeString(string secondsString, bool abbreviate)
        {
            TimeSpan time = TimeSpan.FromSeconds(double.Parse(secondsString));

            if (abbreviate)
            {
                if (time.Days > 365)
                {
                    int years = time.Days / 365;
                    int days = time.Days % 365;
                    return String.Format("{0:##}y {1:#0}d {2:#0}h", years, days, time.Hours);
                }
                else if (time.Days > 0) return String.Format("{0:##}d {1:#0}h {2:#0}m", time.Days, time.Hours, time.Minutes);
                else if (time.Hours > 0) return String.Format("{0:#0}h {1:#0}m", time.Hours, time.Minutes);
                return String.Format("{0:#0}m", time.Minutes);
            }

            if (time.Days > 0) return String.Format("{0:##}d {1:#0}h {2:#0}m {3:#0}s", time.Days, time.Hours, time.Minutes, time.Seconds);
            else if (time.Hours > 0) return String.Format("{0:##}h {1:#0}m {2:#0}s", time.Hours, time.Minutes, time.Seconds);
            else return String.Format("{0:#0}m {1:#0}s", time.Minutes, time.Seconds);
        }

        public static string RankTimeString(string secondsString)
        {
            if (secondsString == "") return "0.000";

            TimeSpan time = TimeSpan.FromSeconds(double.Parse(secondsString));
            if (time.Minutes > 0) return String.Format("{0:#0}:{1:00}.{2:000}", time.Minutes, time.Seconds, time.Milliseconds);
            else return String.Format("{0:#0}.{1:000}", time.Seconds, time.Milliseconds);
        }

        public static readonly DateTime KSFStartDate = new DateTime(1970, 1, 1);

        public static string KSFDateString(string secondsString)
        {
            if (secondsString == "0") return "Before July 2012";

            DateTime date = KSFStartDate.AddSeconds(double.Parse(secondsString));
            return String.Format("{0} {1}, {2}", date.ToString("MMM"), date.Day, date.Year);
        }

        public static string LastOnlineString(string secondsString)
        {
            if (secondsString == "0") return "Before July 2012";

            TimeSpan diff = DateTime.Now.ToUniversalTime() - KSFStartDate.AddSeconds(double.Parse(secondsString));
            if (diff.Days < 30) return PlayTimeString(diff.TotalSeconds.ToString(), true) + " ago";
            else return KSFDateString(secondsString);
        }

        // POINTS -----------------------------------------------------------------------------

        public static string PointsString(string pointsString)
        {
            return ((int)double.Parse(pointsString)).ToString("N0");
        }

        public static string PointsString(double points)
        {
            return ((int)points).ToString("N0");
        }

        public static string IntString(string numString)
        {
            return int.Parse(numString).ToString("N0");
        }

        // COUNTRIES --------------------------------------------------------------------------

        public static readonly string[] CountryTopCountries = {
            "Argentina", "Australia", "Austria",
            "Belarus", "Belgium", "Brazil", "Bulgaria",
            "Canada", "Chile", "China", "Colombia", "Croatia", "Czech Republic", "Czechia",
            "Denmark",
            "Egypt", "Estonia",
            "Finland", "France",
            "Germany", "Greece", "Greenland",
            "Hong Kong", "Hungary",
            "Iceland", "Ireland", "Israel", "Italy",
            "Japan",
            "Kazakhstan", "Korea, Republic of", "Kuwait",
            "Latvia", "Lithuania", "Luxembourg",
            "Macedonia", "Malaysia", "Mexico",
            "Netherlands", "New Zealand", "Norway",
            "Peru", "Poland", "Portugal",
            "Romania", "Russia", "Russian Federation",
            "Serbia", "Singapore", "Slovakia", "Slovenia", "South Africa", "Spain", "Sweden", "Switzerland",
            "Turkey",
            "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "Uruguay"
        };

        public static string CountryEmoji(string country)
        {
            string emoji;
            switch (country)
            {
                case "Ascension Island": emoji = "\U0000001F1E6\U0001F1E8"; break;
                case "Andorra": emoji = "\U0001F1E6\U0001F1E9"; break;
                case "United Arab Emirates": emoji = "\U0001F1E6\U0001F1EA"; break;
                case "Afghanistan": emoji = "\U0001F1E6\U0001F1EB"; break;
                case "Antigua and Barbuda": emoji = "\U0001F1E6\U0001F1EC"; break;
                case "Anguilla": emoji = "\U0001F1E6\U0001F1EE"; break;
                case "Albania": emoji = "\U0001F1E6\U0001F1F1"; break;
                case "Armenia": emoji = "\U0001F1E6\U0001F1F2"; break;
                case "Angola": emoji = "\U0001F1E6\U0001F1F4"; break;
                case "Antarctica": emoji = "\U0001F1E6\U0001F1F6"; break;
                case "Argentina": emoji = "\U0001F1E6\U0001F1F7"; break;
                case "American Samoa": emoji = "\U0001F1E6\U0001F1F8"; break;
                case "Austria": emoji = "\U0001F1E6\U0001F1F9"; break;
                case "Australia": emoji = "\U0001F1E6\U0001F1FA"; break;
                case "Aruba": emoji = "\U0001F1E6\U0001F1FC"; break;
                case "Aland Islands": emoji = "\U0001F1E6\U0001F1FD"; break;
                case "Åland": emoji = "\U0001F1E6\U0001F1FD"; break;
                case "Azerbaijan": emoji = "\U0001F1E6\U0001F1FF"; break;
                case "Bosnia & Herzegovina": emoji = "\U0001F1E7\U0001F1E6"; break;
                case "Bosnia and Herzegovina": emoji = "\U0001F1E7\U0001F1E6"; break;
                case "Barbados": emoji = "\U0001F1E7\U0001F1E7"; break;
                case "Bangladesh": emoji = "\U0001F1E7\U0001F1E9"; break;
                case "Belgium": emoji = "\U0001F1E7\U0001F1EA"; break;
                case "Burkina Faso": emoji = "\U0001F1E7\U0001F1EB"; break;
                case "Bulgaria": emoji = "\U0001F1E7\U0001F1EC"; break;
                case "Bahrain": emoji = "\U0001F1E7\U0001F1ED"; break;
                case "Burundi": emoji = "\U0001F1E7\U0001F1EE"; break;
                case "Benin": emoji = "\U0001F1E7\U0001F1EF"; break;
                case "St. Barthelemy": emoji = "\U0001F1E7\U0001F1F1"; break;
                case "Bermuda": emoji = "\U0001F1E7\U0001F1F2"; break;
                case "Brunei": emoji = "\U0001F1E7\U0001F1F3"; break;
                case "Brunei Darussalam": emoji = "\U0001F1E7\U0001F1F3"; break;
                case "Bolivia": emoji = "\U0001F1E7\U0001F1F4"; break;
                case "Caribbean Netherlands": emoji = "\U0001F1E7\U0001F1F6"; break;
                case "Netherlands Antilles": emoji = "\U0001F1E7\U0001F1F6"; break;
                case "Brazil": emoji = "\U0001F1E7\U0001F1F7"; break;
                case "Bahamas": emoji = "\U0001F1E7\U0001F1F8"; break;
                case "Bhutan": emoji = "\U0001F1E7\U0001F1F9"; break;
                case "Bouvet Island": emoji = "\U0001F1E7\U0001F1FB"; break;
                case "Botswana": emoji = "\U0001F1E7\U0001F1FC"; break;
                case "Belarus": emoji = "\U0001F1E7\U0001F1FE"; break;
                case "Belize": emoji = "\U0001F1E7\U0001F1FF"; break;
                case "Canada": emoji = "\U0001F1E8\U0001F1E6"; break;
                case "Cocos (Keeling) Islands": emoji = "\U0001F1E8\U0001F1E8"; break;
                case "Congo - Kinshasa": emoji = "\U0001F1E8\U0001F1E9"; break;
                case "Central African Republic": emoji = "\U0001F1E8\U0001F1EB"; break;
                case "Congo - Brazzaville": emoji = "\U0001F1E8\U0001F1EC"; break;
                case "Switzerland": emoji = "\U0001F1E8\U0001F1ED"; break;
                case "Cote DIvoire": emoji = "\U0001F1E8\U0001F1EE"; break;
                case "Cook Islands": emoji = "\U0001F1E8\U0001F1F0"; break;
                case "Chile": emoji = "\U0001F1E8\U0001F1F1"; break;
                case "Cameroon": emoji = "\U0001F1E8\U0001F1F2"; break;
                case "China": emoji = "\U0001F1E8\U0001F1F3"; break;
                case "Colombia": emoji = "\U0001F1E8\U0001F1F4"; break;
                case "Clipperton Island": emoji = "\U0001F1E8\U0001F1F5"; break;
                case "Costa Rica": emoji = "\U0001F1E8\U0001F1F7"; break;
                case "Cuba": emoji = "\U0001F1E8\U0001F1FA"; break;
                case "Cape Verde": emoji = "\U0001F1E8\U0001F1FB"; break;
                case "Curacao": emoji = "\U0001F1E8\U0001F1FC"; break;
                case "Christmas Island": emoji = "\U0001F1E8\U0001F1FD"; break;
                case "Cyprus": emoji = "\U0001F1E8\U0001F1FE"; break;
                case "Czechia": emoji = "\U0001F1E8\U0001F1FF"; break;
                case "Czech Republic": emoji = "\U0001F1E8\U0001F1FF"; break;
                case "Germany": emoji = "\U0001F1E9\U0001F1EA"; break;
                case "Diego Garcia": emoji = "\U0001F1E9\U0001F1EC"; break;
                case "Djibouti": emoji = "\U0001F1E9\U0001F1EF"; break;
                case "Denmark": emoji = "\U0001F1E9\U0001F1F0"; break;
                case "Dominica": emoji = "\U0001F1E9\U0001F1F2"; break;
                case "Dominican Republic": emoji = "\U0001F1E9\U0001F1F4"; break;
                case "Algeria": emoji = "\U0001F1E9\U0001F1FF"; break;
                case "Ceuta and Melilla": emoji = "\U0001F1EA\U0001F1E6"; break;
                case "Ecuador": emoji = "\U0001F1EA\U0001F1E8"; break;
                case "Estonia": emoji = "\U0001F1EA\U0001F1EA"; break;
                case "Egypt": emoji = "\U0001F1EA\U0001F1EC"; break;
                case "Western Sahara": emoji = "\U0001F1EA\U0001F1ED"; break;
                case "Eritrea": emoji = "\U0001F1EA\U0001F1F7"; break;
                case "Spain": emoji = "\U0001F1EA\U0001F1F8"; break;
                case "Ethiopia": emoji = "\U0001F1EA\U0001F1F9"; break;
                case "European Union": emoji = "\U0001F1EA\U0001F1FA"; break;
                case "Finland": emoji = "\U0001F1EB\U0001F1EE"; break;
                case "Fiji": emoji = "\U0001F1EB\U0001F1EF"; break;
                case "Falkland Islands": emoji = "\U0001F1EB\U0001F1F0"; break;
                case "Micronesia": emoji = "\U0001F1EB\U0001F1F2"; break;
                case "Faroe Islands": emoji = "\U0001F1EB\U0001F1F4"; break;
                case "France": emoji = "\U0001F1EB\U0001F1F7"; break;
                case "Gabon": emoji = "\U0001F1EC\U0001F1E6"; break;
                case "United Kingdom": emoji = "\U0001F1EC\U0001F1E7"; break;
                case "Grenada": emoji = "\U0001F1EC\U0001F1E9"; break;
                case "Georgia": emoji = "\U0001F1EC\U0001F1EA"; break;
                case "French Guiana": emoji = "\U0001F1EC\U0001F1EB"; break;
                case "Guernsey": emoji = "\U0001F1EC\U0001F1EC"; break;
                case "Ghana": emoji = "\U0001F1EC\U0001F1ED"; break;
                case "Gibraltar": emoji = "\U0001F1EC\U0001F1EE"; break;
                case "Greenland": emoji = "\U0001F1EC\U0001F1F1"; break;
                case "Gambia": emoji = "\U0001F1EC\U0001F1F2"; break;
                case "Guinea": emoji = "\U0001F1EC\U0001F1F3"; break;
                case "Guadeloupe": emoji = "\U0001F1EC\U0001F1F5"; break;
                case "Equatorial Guinea": emoji = "\U0001F1EC\U0001F1F6"; break;
                case "Greece": emoji = "\U0001F1EC\U0001F1F7"; break;
                case "South Georgia and South Sandwich Islands": emoji = "\U0001F1EC\U0001F1F8"; break;
                case "Guatemala": emoji = "\U0001F1EC\U0001F1F9"; break;
                case "Guam": emoji = "\U0001F1EC\U0001F1FA"; break;
                case "Guinea-Bissau": emoji = "\U0001F1EC\U0001F1FC"; break;
                case "Guyana": emoji = "\U0001F1EC\U0001F1FE"; break;
                case "Hong Kong": emoji = "\U0001F1ED\U0001F1F0"; break;
                case "Heard and McDonald Islands": emoji = "\U0001F1ED\U0001F1F2"; break;
                case "Honduras": emoji = "\U0001F1ED\U0001F1F3"; break;
                case "Croatia": emoji = "\U0001F1ED\U0001F1F7"; break;
                case "Haiti": emoji = "\U0001F1ED\U0001F1F9"; break;
                case "Hungary": emoji = "\U0001F1ED\U0001F1FA"; break;
                case "Canary Islands": emoji = "\U0001F1EE\U0001F1E8"; break;
                case "Indonesia": emoji = "\U0001F1EE\U0001F1E9"; break;
                case "Ireland": emoji = "\U0001F1EE\U0001F1EA"; break;
                case "Israel": emoji = "\U0001F1EE\U0001F1F1"; break;
                case "Isle of Man": emoji = "\U0001F1EE\U0001F1F2"; break;
                case "India": emoji = "\U0001F1EE\U0001F1F3"; break;
                case "British Indian Ocean Territory": emoji = "\U0001F1EE\U0001F1F4"; break;
                case "Iraq": emoji = "\U0001F1EE\U0001F1F6"; break;
                case "Iran": emoji = "\U0001F1EE\U0001F1F7"; break;
                case "Iran, Islamic Republic of": emoji = "\U0001F1EE\U0001F1F7"; break;
                case "Iceland": emoji = "\U0001F1EE\U0001F1F8"; break;
                case "Italy": emoji = "\U0001F1EE\U0001F1F9"; break;
                case "Jersey": emoji = "\U0001F1EF\U0001F1EA"; break;
                case "Jamaica": emoji = "\U0001F1EF\U0001F1F2"; break;
                case "Jordan": emoji = "\U0001F1EF\U0001F1F4"; break;
                case "Hashemite Kingdom of Jordan": emoji = "\U0001F1EF\U0001F1F4"; break;
                case "Japan": emoji = "\U0001F1EF\U0001F1F5"; break;
                case "Kenya": emoji = "\U0001F1F0\U0001F1EA"; break;
                case "Kyrgyzstan": emoji = "\U0001F1F0\U0001F1EC"; break;
                case "Cambodia": emoji = "\U0001F1F0\U0001F1ED"; break;
                case "Kiribati": emoji = "\U0001F1F0\U0001F1EE"; break;
                case "Comoros": emoji = "\U0001F1F0\U0001F1F2"; break;
                case "St. Kitts and Nevis": emoji = "\U0001F1F0\U0001F1F3"; break;
                case "North Korea": emoji = "\U0001F1F0\U0001F1F5"; break;
                case "South Korea": emoji = "\U0001F1F0\U0001F1F7"; break;
                case "Korea, Republic of": emoji = "\U0001F1F0\U0001F1F7"; break;
                case "Kuwait": emoji = "\U0001F1F0\U0001F1FC"; break;
                case "Cayman Islands": emoji = "\U0001F1F0\U0001F1FE"; break;
                case "Kazakhstan": emoji = "\U0001F1F0\U0001F1FF"; break;
                case "Laos": emoji = "\U0001F1F1\U0001F1E6"; break;
                case "Lao Peoples Democratic Republic": emoji = "\U0001F1F1\U0001F1E6"; break;
                case "Lebanon": emoji = "\U0001F1F1\U0001F1E7"; break;
                case "St. Lucia": emoji = "\U0001F1F1\U0001F1E8"; break;
                case "Liechtenstein": emoji = "\U0001F1F1\U0001F1EE"; break;
                case "Sri Lanka": emoji = "\U0001F1F1\U0001F1F0"; break;
                case "Liberia": emoji = "\U0001F1F1\U0001F1F7"; break;
                case "Lesotho": emoji = "\U0001F1F1\U0001F1F8"; break;
                case "Lithuania": emoji = "\U0001F1F1\U0001F1F9"; break;
                case "Republic of Lithuania": emoji = "\U0001F1F1\U0001F1F9"; break;
                case "Luxembourg": emoji = "\U0001F1F1\U0001F1FA"; break;
                case "Latvia": emoji = "\U0001F1F1\U0001F1FB"; break;
                case "Libya": emoji = "\U0001F1F1\U0001F1FE"; break;
                case "Morocco": emoji = "\U0001F1F2\U0001F1E6"; break;
                case "Monaco": emoji = "\U0001F1F2\U0001F1E8"; break;
                case "Moldova": emoji = "\U0001F1F2\U0001F1E9"; break;
                case "Republic of Moldova": emoji = "\U0001F1F2\U0001F1E9"; break;
                case "Moldova, Republic of": emoji = "\U0001F1F2\U0001F1E9"; break;
                case "Montenegro": emoji = "\U0001F1F2\U0001F1EA"; break;
                case "St. Martin": emoji = "\U0001F1F2\U0001F1EB"; break;
                case "Madagascar": emoji = "\U0001F1F2\U0001F1EC"; break;
                case "Marshall Islands": emoji = "\U0001F1F2\U0001F1ED"; break;
                case "Macedonia": emoji = "\U0001F1F2\U0001F1F0"; break;
                case "North Macedonia": emoji = "\U0001F1F2\U0001F1F0"; break;
                case "Mali": emoji = "\U0001F1F2\U0001F1F1"; break;
                case "Myanmar": emoji = "\U0001F1F2\U0001F1F2"; break;
                case "Mongolia": emoji = "\U0001F1F2\U0001F1F3"; break;
                case "Macau": emoji = "\U0001F1F2\U0001F1F4"; break;
                case "Northern Mariana Islands": emoji = "\U0001F1F2\U0001F1F5"; break;
                case "Martinique": emoji = "\U0001F1F2\U0001F1F6"; break;
                case "Mauritania": emoji = "\U0001F1F2\U0001F1F7"; break;
                case "Montserrat": emoji = "\U0001F1F2\U0001F1F8"; break;
                case "Malta": emoji = "\U0001F1F2\U0001F1F9"; break;
                case "Mauritius": emoji = "\U0001F1F2\U0001F1FA"; break;
                case "Maldives": emoji = "\U0001F1F2\U0001F1FB"; break;
                case "Malawi": emoji = "\U0001F1F2\U0001F1FC"; break;
                case "Mexico": emoji = "\U0001F1F2\U0001F1FD"; break;
                case "Malaysia": emoji = "\U0001F1F2\U0001F1FE"; break;
                case "Mozambique": emoji = "\U0001F1F2\U0001F1FF"; break;
                case "Namibia": emoji = "\U0001F1F3\U0001F1E6"; break;
                case "New Caledonia": emoji = "\U0001F1F3\U0001F1E8"; break;
                case "Niger": emoji = "\U0001F1F3\U0001F1EA"; break;
                case "Norfolk Island": emoji = "\U0001F1F3\U0001F1EB"; break;
                case "Nigeria": emoji = "\U0001F1F3\U0001F1EC"; break;
                case "Nicaragua": emoji = "\U0001F1F3\U0001F1EE"; break;
                case "Netherlands": emoji = "\U0001F1F3\U0001F1F1"; break;
                case "Norway": emoji = "\U0001F1F3\U0001F1F4"; break;
                case "Nepal": emoji = "\U0001F1F3\U0001F1F5"; break;
                case "Nauru": emoji = "\U0001F1F3\U0001F1F7"; break;
                case "Niue": emoji = "\U0001F1F3\U0001F1FA"; break;
                case "New Zealand": emoji = "\U0001F1F3\U0001F1FF"; break;
                case "Oman": emoji = "\U0001F1F4\U0001F1F2"; break;
                case "Panama": emoji = "\U0001F1F5\U0001F1E6"; break;
                case "Peru": emoji = "\U0001F1F5\U0001F1EA"; break;
                case "French Polynesia": emoji = "\U0001F1F5\U0001F1EB"; break;
                case "Papua New Guinea": emoji = "\U0001F1F5\U0001F1EC"; break;
                case "Philippines": emoji = "\U0001F1F5\U0001F1ED"; break;
                case "Pakistan": emoji = "\U0001F1F5\U0001F1F0"; break;
                case "Poland": emoji = "\U0001F1F5\U0001F1F1"; break;
                case "St. Pierre and Miquelon": emoji = "\U0001F1F5\U0001F1F2"; break;
                case "Pitcairn Islands": emoji = "\U0001F1F5\U0001F1F3"; break;
                case "Puerto Rico": emoji = "\U0001F1F5\U0001F1F7"; break;
                case "Palestinian Territories": emoji = "\U0001F1F5\U0001F1F8"; break;
                case "Palestinian Territory": emoji = "\U0001F1F5\U0001F1F8"; break;
                case "Portugal": emoji = "\U0001F1F5\U0001F1F9"; break;
                case "Palau": emoji = "\U0001F1F5\U0001F1FC"; break;
                case "Paraguay": emoji = "\U0001F1F5\U0001F1FE"; break;
                case "Qatar": emoji = "\U0001F1F6\U0001F1E6"; break;
                case "Reunion": emoji = "\U0001F1F7\U0001F1EA"; break;
                case "Romania": emoji = "\U0001F1F7\U0001F1F4"; break;
                case "Serbia": emoji = "\U0001F1F7\U0001F1F8"; break;
                case "Russia": emoji = "\U0001F1F7\U0001F1FA"; break;
                case "Russian Federation": emoji = "\U0001F1F7\U0001F1FA"; break;
                case "Rwanda": emoji = "\U0001F1F7\U0001F1FC"; break;
                case "Saudi Arabia": emoji = "\U0001F1F8\U0001F1E6"; break;
                case "Solomon Islands": emoji = "\U0001F1F8\U0001F1E7"; break;
                case "Seychelles": emoji = "\U0001F1F8\U0001F1E8"; break;
                case "Sudan": emoji = "\U0001F1F8\U0001F1E9"; break;
                case "Sweden": emoji = "\U0001F1F8\U0001F1EA"; break;
                case "Singapore": emoji = "\U0001F1F8\U0001F1EC"; break;
                case "St. Helena": emoji = "\U0001F1F8\U0001F1ED"; break;
                case "Slovenia": emoji = "\U0001F1F8\U0001F1EE"; break;
                case "Svalbard & Jan Mayen": emoji = "\U0001F1F8\U0001F1EF"; break;
                case "Slovakia": emoji = "\U0001F1F8\U0001F1F0"; break;
                case "Sierra Leone": emoji = "\U0001F1F8\U0001F1F1"; break;
                case "San Marino": emoji = "\U0001F1F8\U0001F1F2"; break;
                case "Senegal": emoji = "\U0001F1F8\U0001F1F3"; break;
                case "Somalia": emoji = "\U0001F1F8\U0001F1F4"; break;
                case "Suriname": emoji = "\U0001F1F8\U0001F1F7"; break;
                case "South Sudan": emoji = "\U0001F1F8\U0001F1F8"; break;
                case "Sao Tome and Pri­ncipe": emoji = "\U0001F1F8\U0001F1F9"; break;
                case "El Salvador": emoji = "\U0001F1F8\U0001F1FB"; break;
                case "Sint Maarten": emoji = "\U0001F1F8\U0001F1FD"; break;
                case "Syria": emoji = "\U0001F1F8\U0001F1FE"; break;
                case "Syrian Arab Republic": emoji = "\U0001F1F8\U0001F1FE"; break;
                case "Swaziland": emoji = "\U0001F1F8\U0001F1FF"; break;
                case "Tristan da Cunha": emoji = "\U0001F1F9\U0001F1E6"; break;
                case "Turks and Caicos Islands": emoji = "\U0001F1F9\U0001F1E8"; break;
                case "Chad": emoji = "\U0001F1F9\U0001F1E9"; break;
                case "French Southern Territories": emoji = "\U0001F1F9\U0001F1EB"; break;
                case "Togo": emoji = "\U0001F1F9\U0001F1EC"; break;
                case "Thailand": emoji = "\U0001F1F9\U0001F1ED"; break;
                case "Tajikistan": emoji = "\U0001F1F9\U0001F1EF"; break;
                case "Tokelau": emoji = "\U0001F1F9\U0001F1F0"; break;
                case "Timor-Leste": emoji = "\U0001F1F9\U0001F1F1"; break;
                case "Turkmenistan": emoji = "\U0001F1F9\U0001F1F2"; break;
                case "Tunisia": emoji = "\U0001F1F9\U0001F1F3"; break;
                case "Tonga": emoji = "\U0001F1F9\U0001F1F4"; break;
                case "Turkey": emoji = "\U0001F1F9\U0001F1F7"; break;
                case "Trinidad and Tobago": emoji = "\U0001F1F9\U0001F1F9"; break;
                case "Tuvalu": emoji = "\U0001F1F9\U0001F1FB"; break;
                case "Taiwan": emoji = "\U0001F1F9\U0001F1FC"; break;
                case "Tanzania": emoji = "\U0001F1F9\U0001F1FF"; break;
                case "Ukraine": emoji = "\U0001F1FA\U0001F1E6"; break;
                case "Uganda": emoji = "\U0001F1FA\U0001F1EC"; break;
                case "U.S. Outlying Islands": emoji = "\U0001F1FA\U0001F1F2"; break;
                case "United Nations": emoji = "\U0001F1FA\U0001F1F3"; break;
                case "United States": emoji = "\U0001F1FA\U0001F1F8"; break;
                case "Uruguay": emoji = "\U0001F1FA\U0001F1FE"; break;
                case "Uzbekistan": emoji = "\U0001F1FA\U0001F1FF"; break;
                case "Vatican City": emoji = "\U0001F1FB\U0001F1E6"; break;
                case "St. Vincent and the Grenadines": emoji = "\U0001F1FB\U0001F1E8"; break;
                case "Saint Vincent and the Grenadines": emoji = "\U0001F1FB\U0001F1E8"; break;
                case "Venezuela": emoji = "\U0001F1FB\U0001F1EA"; break;
                case "British Virgin Islands": emoji = "\U0001F1FB\U0001F1EC"; break;
                case "U.S. Virgin Islands": emoji = "\U0001F1FB\U0001F1EE"; break;
                case "Virgin Islands, U.S.": emoji = "\U0001F1FB\U0001F1EE"; break;
                case "Vietnam": emoji = "\U0001F1FB\U0001F1F3"; break;
                case "Vanuatu": emoji = "\U0001F1FB\U0001F1FA"; break;
                case "Wallis & Futuna": emoji = "\U0001F1FC\U0001F1EB"; break;
                case "Samoa": emoji = "\U0001F1FC\U0001F1F8"; break;
                case "Kosovo": emoji = "\U0001F1FD\U0001F1F0"; break;
                case "Yemen": emoji = "\U0001F1FE\U0001F1EA"; break;
                case "Mayotte": emoji = "\U0001F1FE\U0001F1F9"; break;
                case "South Africa": emoji = "\U0001F1FF\U0001F1E6"; break;
                case "Zambia": emoji = "\U0001F1FF\U0001F1F2"; break;
                case "Zimbabwe": emoji = "\U0001F1FF\U0001F1FC"; break;
                default: emoji = "\U0001F3F3"; break;
            }
            return emoji;
        }
    }

    #endregion
}
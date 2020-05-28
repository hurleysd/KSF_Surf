using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Essentials;

using UIKit;

using RestSharp;

using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        internal readonly static string deviceString = Device.RuntimePlatform;
        internal readonly static string appVersionString = "1.1.32";

        internal readonly static string KSF = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Precondition.KSF));
        internal readonly static string STEAM = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Precondition.STEAM));
        internal readonly static string TWITCH = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Precondition.TWITCH));
        internal readonly static string TWITCH_O = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Precondition.TWITCH_O));

        internal readonly static string AGENT = propertiesDict_getUserAgent();

        internal readonly RestClient client = new RestClient();
        internal readonly RestRequest request = new RestRequest
        {
            Method = Method.GET,
            RequestFormat = DataFormat.Json
        };
        internal IRestResponse response = null;

        public BaseViewModel()
        {
            App.Current.Properties.Remove("agent"); // TODO: REMOVE once all agents have been cleared
            client.UserAgent = AGENT;
            request.AddHeader("x-auth-token", KSF);
        }

        #region autogen
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion


        // System wide static methods ------------------------------------------------------
        #region system
        internal static void vibrate(bool allowVibrate)
        {
            if (!allowVibrate) return;

            if (deviceString == Device.iOS)
            {
                if (Device.Idiom != TargetIdiom.Phone || !UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    return;
                }
                var impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Light);
                impact.Prepare();
                impact.ImpactOccurred();
            }
        }

        internal static bool hasConnection()
        {
            var current = Connectivity.NetworkAccess;
            return (current == NetworkAccess.Internet);
        }

        // Properties Dicitonary --------------------------------------------------------------------------

        internal static string propertiesDict_getUserAgent()
        {
            // EXAMPLE AGENT:   Phone/Apple/iOS/13.3 (Sean's iPhone) 0584707f-0f7a-4a9e-8106-f7b01c6354cd/1.1.0

            string guid = "";
            if (App.Current.Properties.ContainsKey("guid"))
            {
                guid = App.Current.Properties["guid"] as string;
            }
            else
            {
                guid = System.Guid.NewGuid().ToString();
                App.Current.Properties.Add("guid", guid);
                App.Current.SavePropertiesAsync();
            }

            string agent = DeviceInfo.Idiom + "/" + DeviceInfo.Manufacturer + "/" + DeviceInfo.Platform + "/" + DeviceInfo.VersionString + " (" + DeviceInfo.Name + ")";
            agent += " " + guid + "/" + appVersionString;
            return agent;
        }

        internal static string propertiesDict_getSteamID()
        {
            string id = "STEAM_0:0:47620794";  // Sean's steam ID
            if (App.Current.Properties.ContainsKey("steamid"))
            {
                id = App.Current.Properties["steamid"] as string;
            }
            else
            {
                App.Current.Properties.Add("steamid", id);
                App.Current.SavePropertiesAsync();
            }
            return id;
        }

        internal static EFilter_Game propertiesDict_getGame()
        {
            EFilter_Game game = EFilter_Game.css;

            if (App.Current.Properties.ContainsKey("game"))
            {
                string gameString = App.Current.Properties["game"] as string;

                switch (gameString)
                {
                    case "css": game = EFilter_Game.css; break;
                    case "css100t": game = EFilter_Game.css100t; break;
                    case "csgo": game = EFilter_Game.csgo; break;
                    default: goto case "css";
                }
            }
            else
            {
                App.Current.Properties.Add("game", EFilter_ToString.toString(game));
                App.Current.SavePropertiesAsync();
            }

            return game;
        }

        internal static EFilter_Mode propertiesDict_getMode()
        {
            EFilter_Mode mode = EFilter_Mode.fw;

            if (App.Current.Properties.ContainsKey("mode"))
            {
                string modeString = App.Current.Properties["mode"] as string;

                switch (modeString)
                {
                    case "FW": mode = EFilter_Mode.fw; break;
                    case "HSW": mode = EFilter_Mode.hsw; break;
                    case "SW": mode = EFilter_Mode.sw; break;
                    case "BW": mode = EFilter_Mode.bw; break;
                    default: goto case "FW";
                }
            }
            else
            {
                App.Current.Properties.Add("mode", EFilter_ToString.toString(mode));
                App.Current.SavePropertiesAsync();
            }

            return mode;
        }
        #endregion
    }

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

        public static string toString(EFilter_MapType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_MapType.linear: typeString = "Linear"; break;
                case EFilter_MapType.staged: typeString = "Staged"; break;
                default: break;
            }
            return typeString;
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

        public static readonly string[] rrtype_arr = new string[] { "Map", "Top10", "Stage", "Bonus", "All WRs" };

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

        public static readonly string[] otype_arr = new string[] { "Map", "Stage", "Bonus" };
        public static string toString(EFilter_ORType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_ORType.map: typeString = "map"; break;
                case EFilter_ORType.stage: typeString = "stage"; break;
                case EFilter_ORType.bonus: typeString = "bonus"; break;
                default: break;
            }
            return typeString;
        }

        public static string toString2(EFilter_ORType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_ORType.map: typeString = otype_arr[0]; break;
                case EFilter_ORType.stage: typeString = otype_arr[1]; break;
                case EFilter_ORType.bonus: typeString = otype_arr[2]; break;
                default: break;
            }
            return typeString;
        }

        public static string toString(EFilter_PlayerType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_PlayerType.steamid: typeString = "steamid"; break;
                case EFilter_PlayerType.rank: typeString = "rank"; break;
                case EFilter_PlayerType.me: goto case EFilter_PlayerType.steamid;
                default: break;
            }
            return typeString;
        }

        public static string toString(EFilter_PlayerRecordsType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_PlayerRecordsType.set: typeString = "recordset"; break;
                case EFilter_PlayerRecordsType.broken: typeString = "recordbroken"; break;
                default: break;
            }
            return typeString;
        }

        public static readonly string[] wrtype_arr = new string[] { "wr", "wrcp", "wrb" };
        public static readonly string[] wrtype_arr2 = new string[] { "WR", "WRCP", "WRB" };

        public static string toString(EFilter_PlayerWRsType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_PlayerWRsType.wr: typeString = wrtype_arr[0]; break;
                case EFilter_PlayerWRsType.wrcp: typeString = wrtype_arr[1]; break;
                case EFilter_PlayerWRsType.wrb: typeString = wrtype_arr[2]; break;
                default: break;
            }
            return typeString;
        }

        public static string toString2(EFilter_PlayerWRsType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_PlayerWRsType.wr: typeString = wrtype_arr2[0]; break;
                case EFilter_PlayerWRsType.wrcp: typeString = wrtype_arr2[1]; break;
                case EFilter_PlayerWRsType.wrb: typeString = wrtype_arr2[2]; break;
                default: break;
            }
            return typeString;
        }

        public static string toString(EFilter_PlayerCompletionType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_PlayerCompletionType.complete: typeString = "complete"; break;
                case EFilter_PlayerCompletionType.incomplete: typeString = "incomplete"; break;
                case EFilter_PlayerCompletionType.completionbytier: typeString = "completionbytier"; break;
                default: break;
            }
            return typeString;
        }

        public static string toString2(EFilter_PlayerCompletionType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_PlayerCompletionType.complete: typeString = "Complete Maps"; break;
                case EFilter_PlayerCompletionType.incomplete: typeString = "Incomplete Maps"; break;
                default: break;
            }
            return typeString;
        }

        public static readonly string[] ortype_arr = new string[] { "WR", "WRCP", "WRB", "Top10", "Map", "Stage", "Bonus" };
        public static string toString(EFilter_PlayerOldestType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_PlayerOldestType.wr: typeString = "oldestwr"; break;
                case EFilter_PlayerOldestType.wrcp: typeString = "oldestwrcp"; break;
                case EFilter_PlayerOldestType.wrb: typeString = "oldestwrb"; break;
                case EFilter_PlayerOldestType.top10: typeString = "oldesttop10"; break;
                case EFilter_PlayerOldestType.map: typeString = "oldestmap"; break;
                case EFilter_PlayerOldestType.stage: typeString = "oldeststage"; break;
                case EFilter_PlayerOldestType.bonus: typeString = "oldestbonus"; break;
                default: break;
            }
            return typeString;
        }

        public static string toString2(EFilter_PlayerOldestType type)
        {
            string typeString = "";
            switch (type)
            {
                case EFilter_PlayerOldestType.wr: typeString = ortype_arr[0]; break;
                case EFilter_PlayerOldestType.wrcp: typeString = ortype_arr[1]; break;
                case EFilter_PlayerOldestType.wrb: typeString = ortype_arr[2]; break;
                case EFilter_PlayerOldestType.top10: typeString = ortype_arr[3]; break;
                case EFilter_PlayerOldestType.map: typeString = ortype_arr[4]; break;
                case EFilter_PlayerOldestType.stage: typeString = ortype_arr[5]; break;
                case EFilter_PlayerOldestType.bonus: typeString = ortype_arr[6]; break;
                default: break;
            }
            return typeString;
        }


        public static string zoneFormatter(string z, bool includeMain, bool fullName)
        {
            string zoneString = "";

            int zone = int.Parse(z);
            if (zone != 0) // if not main
            {
                if (zone < 31) // stage
                {
                    if (fullName)
                    {
                        zoneString = "Stage " + zone;
                    }
                    else
                    {
                        zoneString = "S" + zone;
                    }
                }
                else // bonus
                {
                    if (fullName)
                    {
                        zoneString = "Bonus " + (zone - 30);
                    }
                    else
                    {
                        zoneString = "B" + (zone - 30);
                    }
                }
            }
            else
            {
                if (includeMain)
                {
                    zoneString = "Main";
                }
            }
            return zoneString;
        }

        public static string CPRZoneFormatter(string z, EFilter_MapType mapType)
        {
            string zoneString = "";
            int zone = int.Parse(z);

            if (zone == 1)
            {
                return "Start";
            }
            else if (zone == 0)
            {
                return "End";
            }

            if (mapType == EFilter_MapType.staged)
            {
                zoneString = "Stage " + zone;
            }
            else if (mapType == EFilter_MapType.linear)
            {
                zoneString = "Checkpoint " + (zone - 1);
            }

            return zoneString;
        }

        public static readonly string[] rankTitles = { "MASTER", "ELITE", "VETERAN", "PRO", "EXPERT", "HOTSHOT",
            "EXCEPTIONAL", "EXPERIENCED", "SKILLED", "CASUAL", "BEGINNER", "ROOKIE"};

        public static readonly Color[] rankColors = { Color.Magenta, Color.HotPink, Color.Red, Color.Orange, Color.Gold,
            Color.LimeGreen, Color.SeaGreen,  Color.SkyBlue, Color.DarkSlateBlue, Color.DarkOliveGreen, Color.SaddleBrown, Color.Gray};

        public static string getRankTitle(string rankString, string pointsString)
        {
            string title;

            int rank = int.Parse(rankString);
            double points = double.Parse(pointsString);

            if (1 <= rank && rank <= 10)
            {
                title = rankTitles[0];
            }
            else if (rank <= 25)
            {
                title = rankTitles[1];
            }
            else if (rank <= 50)
            {
                title = rankTitles[2];
            }
            else if (rank <= 100)
            {
                title = rankTitles[3];
            }
            else if (rank <= 200)
            {
                title = rankTitles[4];
            }
            else if (rank <= 350)
            {
                title = rankTitles[5];
            }
            else if (rank <= 500)
            {
                title = rankTitles[6];
            }
            else if (points >= 6000)
            {
                title = rankTitles[7];
            }
            else if (points >= 4000)
            {
                title = rankTitles[8];
            }
            else if (points >= 2500)
            {
                title = rankTitles[9];
            }
            else if (points >= 1000)
            {
                title = rankTitles[10];
            }
            else
            {
                title = rankTitles[11];
            }

            return title;
        }

        public static readonly string[] ctopCountries = {
            "Argentina", "Australia", "Austria",
            "Belarus", "Belgium", "Brazil", "Bulgaria",
            "Canada", "Chile", "China", "Colombia", "Croatia", "Czech Republic", "Czechia",
            "Denmark",
            "Egypt", "Estonia",
            "Finland", "France",
            "Germany", "Greece", "Greenland",
            "Hungary",
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
            "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "Uruguay" };
    }

    #endregion
    // STRING FORMAT ------------------------------------------------------------------------
    #region stringformat

    public static class String_Formatter
    {
        public static string toString_CompletionPercent(string completeString, string totalString)
        {
            int complete = int.Parse(completeString);
            int total = int.Parse(totalString);
            int percent = (total == 0)? 0 : (int)(((double)complete / total) * 100);

            return percent + "%";
        }

        public static string toString_PlayTime(string seconds, bool abbreviate)
        {
            TimeSpan time = TimeSpan.FromSeconds(double.Parse(seconds));
            if (abbreviate)
            {
                if (time.Days > 365)
                {
                    int years = time.Days / 365;
                    int days = time.Days % 365;
                    return String.Format("{0:##}y {1:#0}d {2:#0}h", years, days, time.Hours);
                }
                else if (time.Days > 0)
                {
                    return String.Format("{0:##}d {1:#0}h {2:#0}m", time.Days, time.Hours, time.Minutes);
                }
                else if (time.Hours > 0)
                {
                    return String.Format("{0:#0}h {1:#0}m", time.Hours, time.Minutes);
                }
                return String.Format("{0:#0}m", time.Minutes);
            }

            if (time.Days > 0)
            {
                return String.Format("{0:##}d {1:#0}h {2:#0}m {3:#0}s", time.Days, time.Hours, time.Minutes, time.Seconds);
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
            if (time.Minutes > 0)
            {
                return String.Format("{0:#0}:{1:00}.{2:000}", time.Minutes, time.Seconds, time.Milliseconds);
            }
            return String.Format("{0:#0}.{1:000}", time.Seconds, time.Milliseconds);
        }

        public static readonly DateTime ksf_start = new DateTime(1970, 1, 1);

        public static string toString_KSFDate(string seconds)
        {
            if (seconds == "0")
            {
                return "Before July 2012";
            }

            DateTime date = ksf_start.AddSeconds(double.Parse(seconds));
            return String.Format("{0} {1}, {2}", date.ToString("MMM"), date.Day, date.Year);
        }

        public static string toString_LastOnline(string seconds)
        {
            if (seconds == "0")
            {
                return "Before July 2012";
            }

            TimeSpan diff = DateTime.Now.ToUniversalTime() - ksf_start.AddSeconds(double.Parse(seconds));
            if (diff.Days < 30)
            {
                return toString_PlayTime(diff.TotalSeconds.ToString(), true) + " ago";
            }
            return toString_KSFDate(seconds);
        }

        public static string toString_Points(string points)
        {
            return ((int)double.Parse(points)).ToString("N0");
        }

        public static string toString_Points(double points)
        {
            return ((int)points).ToString("N0");
        }

        public static string toString_Int(string points)
        {
            return int.Parse(points).ToString("N0");
        }

        public static string toEmoji_Country(string country)
        {
            string emoji;
            switch (country)
            {
                case "Ascension Island": emoji = "\U0000001F1E6\U0001F1E8"; break;
                case "Andorra": emoji = "\U0001F1E6\U0001F1E9"; break;
                case "United Arab Emirates": emoji = "\U0001F1E6\U0001F1EA"; break;
                case "Afghanistan": emoji = "\U0001F1E6\U0001F1EB"; break;
                case "Antigua & Barbuda": emoji = "\U0001F1E6\U0001F1EC"; break;
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
                case "Ã…land Islands": emoji = "\U0001F1E6\U0001F1FD"; break;
                case "Azerbaijan": emoji = "\U0001F1E6\U0001F1FF"; break;
                case "Bosnia & Herzegovina": emoji = "\U0001F1E7\U0001F1E6"; break;
                case "Barbados": emoji = "\U0001F1E7\U0001F1E7"; break;
                case "Bangladesh": emoji = "\U0001F1E7\U0001F1E9"; break;
                case "Belgium": emoji = "\U0001F1E7\U0001F1EA"; break;
                case "Burkina Faso": emoji = "\U0001F1E7\U0001F1EB"; break;
                case "Bulgaria": emoji = "\U0001F1E7\U0001F1EC"; break;
                case "Bahrain": emoji = "\U0001F1E7\U0001F1ED"; break;
                case "Burundi": emoji = "\U0001F1E7\U0001F1EE"; break;
                case "Benin": emoji = "\U0001F1E7\U0001F1EF"; break;
                case "St. BarthÃ©lemy": emoji = "\U0001F1E7\U0001F1F1"; break;
                case "Bermuda": emoji = "\U0001F1E7\U0001F1F2"; break;
                case "Brunei": emoji = "\U0001F1E7\U0001F1F3"; break;
                case "Bolivia": emoji = "\U0001F1E7\U0001F1F4"; break;
                case "Caribbean Netherlands": emoji = "\U0001F1E7\U0001F1F6"; break;
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
                case "CÃ´te dâ€™Ivoire": emoji = "\U0001F1E8\U0001F1EE"; break;
                case "Cook Islands": emoji = "\U0001F1E8\U0001F1F0"; break;
                case "Chile": emoji = "\U0001F1E8\U0001F1F1"; break;
                case "Cameroon": emoji = "\U0001F1E8\U0001F1F2"; break;
                case "China": emoji = "\U0001F1E8\U0001F1F3"; break;
                case "Colombia": emoji = "\U0001F1E8\U0001F1F4"; break;
                case "Clipperton Island": emoji = "\U0001F1E8\U0001F1F5"; break;
                case "Costa Rica": emoji = "\U0001F1E8\U0001F1F7"; break;
                case "Cuba": emoji = "\U0001F1E8\U0001F1FA"; break;
                case "Cape Verde": emoji = "\U0001F1E8\U0001F1FB"; break;
                case "CuraÃ§ao": emoji = "\U0001F1E8\U0001F1FC"; break;
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
                case "Ceuta & Melilla": emoji = "\U0001F1EA\U0001F1E6"; break;
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
                case "South Georgia & South Sandwich Islands": emoji = "\U0001F1EC\U0001F1F8"; break;
                case "Guatemala": emoji = "\U0001F1EC\U0001F1F9"; break;
                case "Guam": emoji = "\U0001F1EC\U0001F1FA"; break;
                case "Guinea-Bissau": emoji = "\U0001F1EC\U0001F1FC"; break;
                case "Guyana": emoji = "\U0001F1EC\U0001F1FE"; break;
                case "Hong Kong SAR China": emoji = "\U0001F1ED\U0001F1F0"; break;
                case "Heard & McDonald Islands": emoji = "\U0001F1ED\U0001F1F2"; break;
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
                case "Iceland": emoji = "\U0001F1EE\U0001F1F8"; break;
                case "Italy": emoji = "\U0001F1EE\U0001F1F9"; break;
                case "Jersey": emoji = "\U0001F1EF\U0001F1EA"; break;
                case "Jamaica": emoji = "\U0001F1EF\U0001F1F2"; break;
                case "Jordan": emoji = "\U0001F1EF\U0001F1F4"; break;
                case "Japan": emoji = "\U0001F1EF\U0001F1F5"; break;
                case "Kenya": emoji = "\U0001F1F0\U0001F1EA"; break;
                case "Kyrgyzstan": emoji = "\U0001F1F0\U0001F1EC"; break;
                case "Cambodia": emoji = "\U0001F1F0\U0001F1ED"; break;
                case "Kiribati": emoji = "\U0001F1F0\U0001F1EE"; break;
                case "Comoros": emoji = "\U0001F1F0\U0001F1F2"; break;
                case "St. Kitts & Nevis": emoji = "\U0001F1F0\U0001F1F3"; break;
                case "North Korea": emoji = "\U0001F1F0\U0001F1F5"; break;
                case "South Korea": emoji = "\U0001F1F0\U0001F1F7"; break;
                case "Korea, Republic of": emoji = "\U0001F1F0\U0001F1F7"; break;
                case "Kuwait": emoji = "\U0001F1F0\U0001F1FC"; break;
                case "Cayman Islands": emoji = "\U0001F1F0\U0001F1FE"; break;
                case "Kazakhstan": emoji = "\U0001F1F0\U0001F1FF"; break;
                case "Laos": emoji = "\U0001F1F1\U0001F1E6"; break;
                case "Lebanon": emoji = "\U0001F1F1\U0001F1E7"; break;
                case "St. Lucia": emoji = "\U0001F1F1\U0001F1E8"; break;
                case "Liechtenstein": emoji = "\U0001F1F1\U0001F1EE"; break;
                case "Sri Lanka": emoji = "\U0001F1F1\U0001F1F0"; break;
                case "Liberia": emoji = "\U0001F1F1\U0001F1F7"; break;
                case "Lesotho": emoji = "\U0001F1F1\U0001F1F8"; break;
                case "Lithuania": emoji = "\U0001F1F1\U0001F1F9"; break;
                case "Luxembourg": emoji = "\U0001F1F1\U0001F1FA"; break;
                case "Latvia": emoji = "\U0001F1F1\U0001F1FB"; break;
                case "Libya": emoji = "\U0001F1F1\U0001F1FE"; break;
                case "Morocco": emoji = "\U0001F1F2\U0001F1E6"; break;
                case "Monaco": emoji = "\U0001F1F2\U0001F1E8"; break;
                case "Moldova": emoji = "\U0001F1F2\U0001F1E9"; break;
                case "Montenegro": emoji = "\U0001F1F2\U0001F1EA"; break;
                case "St. Martin": emoji = "\U0001F1F2\U0001F1EB"; break;
                case "Madagascar": emoji = "\U0001F1F2\U0001F1EC"; break;
                case "Marshall Islands": emoji = "\U0001F1F2\U0001F1ED"; break;
                case "Macedonia": emoji = "\U0001F1F2\U0001F1F0"; break;
                case "Mali": emoji = "\U0001F1F2\U0001F1F1"; break;
                case "Myanmar (Burma)": emoji = "\U0001F1F2\U0001F1F2"; break;
                case "Mongolia": emoji = "\U0001F1F2\U0001F1F3"; break;
                case "Macau SAR China": emoji = "\U0001F1F2\U0001F1F4"; break;
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
                case "St. Pierre & Miquelon": emoji = "\U0001F1F5\U0001F1F2"; break;
                case "Pitcairn Islands": emoji = "\U0001F1F5\U0001F1F3"; break;
                case "Puerto Rico": emoji = "\U0001F1F5\U0001F1F7"; break;
                case "Palestinian Territories": emoji = "\U0001F1F5\U0001F1F8"; break;
                case "Portugal": emoji = "\U0001F1F5\U0001F1F9"; break;
                case "Palau": emoji = "\U0001F1F5\U0001F1FC"; break;
                case "Paraguay": emoji = "\U0001F1F5\U0001F1FE"; break;
                case "Qatar": emoji = "\U0001F1F6\U0001F1E6"; break;
                case "RÃ©union": emoji = "\U0001F1F7\U0001F1EA"; break;
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
                case "SÃ£o TomÃ© & PrÃ­ncipe": emoji = "\U0001F1F8\U0001F1F9"; break;
                case "El Salvador": emoji = "\U0001F1F8\U0001F1FB"; break;
                case "Sint Maarten": emoji = "\U0001F1F8\U0001F1FD"; break;
                case "Syria": emoji = "\U0001F1F8\U0001F1FE"; break;
                case "Swaziland": emoji = "\U0001F1F8\U0001F1FF"; break;
                case "Tristan da Cunha": emoji = "\U0001F1F9\U0001F1E6"; break;
                case "Turks & Caicos Islands": emoji = "\U0001F1F9\U0001F1E8"; break;
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
                case "Trinidad & Tobago": emoji = "\U0001F1F9\U0001F1F9"; break;
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
                case "St. Vincent & Grenadines": emoji = "\U0001F1FB\U0001F1E8"; break;
                case "Venezuela": emoji = "\U0001F1FB\U0001F1EA"; break;
                case "British Virgin Islands": emoji = "\U0001F1FB\U0001F1EC"; break;
                case "U.S. Virgin Islands": emoji = "\U0001F1FB\U0001F1EE"; break;
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
        #endregion
    }
}

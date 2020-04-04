using System;
using System.Collections.Generic;

namespace KSF_Surf.Models
{
    // Classes for KSF API response JSON deserialization-----------------------------------
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

    public class KSFDetailedMapDatum
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

    public class KSFDetailedMapsRootObject
    {
        public string status { get; set; }
        public List<KSFDetailedMapDatum> data { get; set; }
    }

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
}
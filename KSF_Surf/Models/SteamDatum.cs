using System;
using System.Collections.Generic;

namespace KSF_Surf.Models
{
    // Classes for Steam API response JSON deserialization ------------------------------------

    public class SteamProfileDatum
    {
        public string steamid { get; set; }
        public int communityvisibilitystate { get; set; }
        public int profilestate { get; set; }
        public string personaname { get; set; }
        public string profileurl { get; set; }
        public string avatar { get; set; }
        public string avatarmedium { get; set; }
        public string avatarfull { get; set; }
        public long lastlogoff { get; set; }
        public int personastate { get; set; }
        public string realname { get; set; }
        public string primaryclanid { get; set; }
        public long timecreated { get; set; }
        public int personastateflags { get; set; }
        public string loccountrycode { get; set; }
    }

    public class SteamProfilesDatum
    {
        public List<SteamProfileDatum> players { get; set; }
    }

    public class SteamProfilesRoot
    {
        public SteamProfilesDatum response { get; set; }
    }
}
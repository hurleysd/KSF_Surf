using System.Collections.Generic;

namespace KSF_Surf.Models
{
    // Classes for Steam API response JSON deserialization --------------------------------

    public class SteamProfile
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

    public class SteamProfileResponse
    {
        public List<SteamProfile> players { get; set; }
    }


    public class SteamProfileRootObject
    {
        public SteamProfileResponse response { get; set; }
    }

    public static class SteamIDConverter
    { 
        public static string Steam32to64(string steam32)
        {
            string[] steam32_arr = steam32.Split(':');

            long convertedTo64Bit = long.Parse(steam32_arr[2]) * 2;
            convertedTo64Bit += 76561197960265728; // Valve's magic constant
            convertedTo64Bit += long.Parse(steam32_arr[1]);

            return convertedTo64Bit.ToString();
        }
    }

}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace KSF_Surf.Models
{
    // Classes for Twitch API response JSON deserialization --------------------------------
    public class TwitchDatum
    {
        public string id { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string game_id { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public int viewer_count { get; set; }
        public DateTime started_at { get; set; }
        public string language { get; set; }
        public string thumbnail_url { get; set; }
        public List<string> tag_ids { get; set; }
    }

    public class TwitchPagination
    {
        public string cursor { get; set; }
    }

    public class TwitchRootObject
    {
        public ObservableCollection<TwitchDatum> data { get; set; }
        public TwitchPagination pagination { get; set; }
    }
}
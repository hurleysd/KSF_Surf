using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KSF_Surf.ViewModels
{
    public class LiveViewModel : BaseViewModel
    {

        internal RootObject streams;

        public LiveViewModel()
        {
            Title = "Live";
            twitchConnect();
        }

        private void twitchConnect()
        {
            string clientID = ""; // HIDE!!!!

            var client = new RestClient();
            client.BaseUrl = new Uri("https://api.twitch.tv/helix/streams?first=6"); // TODO: change to actual surf streams (probably though a following list?)

            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Client-ID", clientID);
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                streams = JsonConvert.DeserializeObject<RootObject>(response.Content);
            } 
        }

        // Classes for Twitch API response JSON deserialization
        public class Datum
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

        public class Pagination
        {
            public string cursor { get; set; }
        }

        public class RootObject
        {
            public List<Datum> data { get; set; }
            public Pagination pagination { get; set; }
        }
    }
}
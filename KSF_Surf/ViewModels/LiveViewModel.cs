using System;

using RestSharp;
using Newtonsoft.Json;

using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class LiveViewModel : BaseViewModel
    {
        // object for "Surfer Streams"
        internal TwitchRootObject streams;

        // objects for HTTP requests
        private readonly RestClient client;
        private readonly RestRequest request;

        public LiveViewModel()
        {
            Title = "Live";
            
            client = new RestClient();
            request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            twitchConnect();
        }

        // KSF API call -------------------------------------------------------------------------
        #region ksf

        internal KSFServerRootObject GetServers(EFilter_Game game)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            if (gameString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/servers/list");
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<KSFServerRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        #endregion
        //Twitch API call ------------------------------------------------------------------------
        #region twitch

        private void twitchConnect()
        {
            if (!BaseViewModel.hasConnection()) return;

            string clientID = "";
            string query = "https://api.twitch.tv/helix/streams?";

            foreach (string username in streamers)
            {
                query += "user_login=" + username + "&";
            }
            query = query.Substring(0, query.Length - 1);

            client.BaseUrl = new Uri(query);

            var trequest = new RestRequest();
            trequest.Method = Method.GET;
            trequest.AddHeader("Client-ID", clientID);
            trequest.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(trequest);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                streams = JsonConvert.DeserializeObject<TwitchRootObject>(response.Content);
            }
            else
            {
                streams = null;
            }
        }

        internal void twitchRefresh()
        {
            twitchConnect();
        }

        private readonly string[] streamers =
        {
            // 57 out of 100 (max per twitch API call)
            "truktruk",
            "gocnak",
            "troflecopter",
            "mariowned",
            "beetle179",
            "silverthingtg",
            "caffrey",
            "sneak_it",
            "rickzter",
            "draaph",
            "olivernb",
            "ignis_au",
            "wayne3288",
            "systm_",
            "exuwew_",
            "rredccolour",
            "redvenomsurf",
            "crashfort",
            "robinsurf",
            "hardex",
            "rulldar",
            "redsurfs",
            "xtra_festive",
            "im_president_sloth",
            "jooshua",
            "pretzl",
            "chrissybear",
            "haywire404",
            "fluxxi",
            "surfleague",
            "mako__o",
            "itsasteral",
            "theshoxter",
            "recoilsurf",
            "sensemcsweggense",
            "illxjoey",
            "lordsyfo",
            "ret_rded",
            "pignucss",
            "thegaranimal",
            "surfking",
            "moostercow",
            "wildddd",
            "spy_complex",
            "blueraven",
            "ghostfacee",
            "muta44",
            "gorange_ninja",
			"txcks",
            "kiiru",
            "marblesurfs",
            "virgn4life",
            "zacki9",
            "simexi",
            "louieismyname",
            "konga",
            "mbnlol",
            "granis_"
        };

        #endregion
    }
}

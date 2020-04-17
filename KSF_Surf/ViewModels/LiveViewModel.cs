using System;

using RestSharp;
using Newtonsoft.Json;

using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class LiveViewModel : BaseViewModel
    {

        internal TwitchRootObject streams;
        internal KSFServerRootObject css_servers;
        internal KSFServerRootObject css100t_servers;
        internal KSFServerRootObject csgo_servers;

        private static RestClient client;
        private static RestRequest request;

        public LiveViewModel()
        {
            Title = "Live";
            
            client = new RestClient();
            request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            ksfConnect();
            twitchConnect();
        }

        private void ksfConnect()
        {
            if (!BaseViewModel.hasConnection()) return;

            // CSS server list ---------------------------------------------------------------------
            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/css/servers/list");
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                css_servers = JsonConvert.DeserializeObject<KSFServerRootObject>(response.Content);
            }
            else
            {
                css_servers = null;
            }

            // CSS100T server list ------------------------------------------------------------------
            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/css100t/servers/list");
            response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                css100t_servers = JsonConvert.DeserializeObject<KSFServerRootObject>(response.Content);
            }
            else
            {
                css100t_servers = null;
            }

            // CSGO server list ------------------------------------------------------------------
            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/csgo/servers/list");
            response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                csgo_servers = JsonConvert.DeserializeObject<KSFServerRootObject>(response.Content);
            }
            else
            {
                csgo_servers = null;
            }
        }

        internal void ksfRefresh()
        {
            ksfConnect();
        }

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
            // 55 out of 100 (max per twitch API call)
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
            "konga"
        };
    }
}

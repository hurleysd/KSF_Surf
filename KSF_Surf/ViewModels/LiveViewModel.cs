using System;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

using RestSharp;
using Newtonsoft.Json;

using System.Collections.Generic;
using KSF_Surf.Models;


namespace KSF_Surf.ViewModels
{
    public class LiveViewModel : BaseViewModel
    {

        internal TwitchRootObject streams;
        internal KSFServerRootObject css_servers;
        internal KSFServerRootObject css100t_servers;
        internal KSFServerRootObject csgo_servers;

        public LiveViewModel()
        {
            Title = "Live";
            ksfConnect();
            twitchConnect();
        }

        private void ksfConnect()
        {
            var client = new RestClient();

            // CSS server list ---------------------------------------------------------------------
            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/css/servers/list");

            var request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                css_servers = JsonConvert.DeserializeObject<KSFServerRootObject>(response.Content);
            }

            // CSS100T server list ------------------------------------------------------------------
            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/css100t/servers/list");

            request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                css100t_servers = JsonConvert.DeserializeObject<KSFServerRootObject>(response.Content);
            }

            // CSGO server list ------------------------------------------------------------------
            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/csgo/servers/list");

            request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                csgo_servers = JsonConvert.DeserializeObject<KSFServerRootObject>(response.Content);
            }
        }

        private void twitchConnect()
        {
            string clientID = "iz4y3mwgjedffv1t2oifwuvn5n6iyr"; // HIDE!!!!
            string query = "https://api.twitch.tv/helix/streams?";

            foreach (string username in streamers)
            {
                query += "user_login=" + username + "&";
            }
            query = query.Substring(0, query.Length - 1);
            Console.WriteLine(query);

            var client = new RestClient();
            client.BaseUrl = new Uri(query);

            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Client-ID", clientID);
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                streams = JsonConvert.DeserializeObject<TwitchRootObject>(response.Content);
            } 
        }

        internal void twitchRefresh()
        {
            twitchConnect();
        }

        public readonly string[] streamers =
        {
            "truktruk",
            "gocnak",
            "troflecopter",
            "mariowned",
            "beetle179",
            "silverthingtg",
            "caffrey",
            "sneak_it",
            "rickzter",
            "aimer_b",
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
            "gorange_ninja"
        };
    }
}

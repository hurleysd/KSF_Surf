using System;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft.Json;

using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class LiveViewModel : BaseViewModel
    {
        // object for "Surfer Streams"
        internal TwitchRootObject streams;

        public LiveViewModel() : base()
        {
            Title = "Live";
        }

        // KSF API call -------------------------------------------------------------------------
        #region ksf

        internal async Task<KSFServerRootObject> GetServers(EFilter_Game game)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            if (gameString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/servers/list");
            await Task.Run(() => response = client.Execute(request));

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

        private async Task twitchConnect()
        {
            if (!BaseViewModel.hasConnection()) return;

            string query = "https://api.twitch.tv/helix/streams?";
            foreach (string username in streamers)
            {
                query += "user_login=" + username + "&";
            }
            query = query.Substring(0, query.Length - 1);

            client.BaseUrl = new Uri(query);

            var trequest = new RestRequest
            {
                Method = Method.GET
            };
            trequest.AddHeader("Authorization", BaseViewModel.TWITCH_O);
            trequest.AddHeader("Client-ID", BaseViewModel.TWITCH);
            trequest.RequestFormat = DataFormat.Json;

            await Task.Run(() => response = client.Execute(trequest));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                streams = JsonConvert.DeserializeObject<TwitchRootObject>(response.Content);
            }
            else
            {
                streams = null;
            }
        }

        internal async Task twitchRefresh()
        {
            await twitchConnect();
        }

        private readonly string[] streamers =
        {
            // 65 out of 100 (max per twitch API call)
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
            "granis_",
            "taiikii",
            "benowy",
            "melinder",
            "idkdreams",
            "weazr",
            "usernamesurf",
            "dangersurfs",
            "Bas_Kahvo",
            "snosnowyyyy_y"
        };

        #endregion
    }
}

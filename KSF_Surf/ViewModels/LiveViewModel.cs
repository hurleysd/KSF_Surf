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
                streams = JsonConvert.DeserializeObject<TwitchRootObject>(response.Content);
            } 
        }

        internal void twitchRefresh()
        {
            twitchConnect();
        }
    }
}

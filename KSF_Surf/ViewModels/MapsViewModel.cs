using Newtonsoft.Json;
using RestSharp;

using System;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class MapsViewModel : BaseViewModel
    {
        internal KSFMapsRootObject css_maps;
        internal KSFMapsRootObject csgo_maps;

        public MapsViewModel()
        {
            Title = "Maps";
            ksfConnect();
        }

        private void ksfConnect()
        {
            var client = new RestClient();

            // CSS Maps List ------------------------------------------------------------------
            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/css/maplist/list");

            var request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                css_maps = JsonConvert.DeserializeObject<KSFMapsRootObject>(response.Content);
            }

            // CSGO Maps List ------------------------------------------------------------------
            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/csgo/maplist/list");

            request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                csgo_maps = JsonConvert.DeserializeObject<KSFMapsRootObject>(response.Content);
            }
        }

    }
}
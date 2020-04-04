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

        public MapsViewModel()
        {
            Title = "Maps";
        }

        internal static KSFDetailedMapsRootObject getDetailedMapsList(EFilter_Game game, EFilter_Sort sort)
        {
            string gameString = "";
            string sortString = "";

            switch (game)
            {
                case EFilter_Game.css: gameString = "css"; break;
                case EFilter_Game.css100t: gameString = "css100t"; break;
                case EFilter_Game.csgo: gameString = "csgo"; break;
                default: break;
            }
            switch (sort)
            {
                case EFilter_Sort.name: sortString = "name"; break;
                case EFilter_Sort.created: sortString = "created"; break;
                case EFilter_Sort.lastplayed: sortString = "lastplayed"; break;
                case EFilter_Sort.playtime: sortString = "playtime"; break;
                case EFilter_Sort.popularity: sortString = "popularity"; break;
                default: break;
            }


            var client = new RestClient();

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/maplist/detailedlist/" + sortString + "/1,999");

            var request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<KSFDetailedMapsRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

    }
}
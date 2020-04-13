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

        internal static DetailedMapsRootObject GetDetailedMapsList(EFilter_Game game, EFilter_Sort sort)
        {
            string gameString = EFilter_ToString.toString(game);
            string sortString = EFilter_ToString.toString(sort);

            if (gameString == "" || sortString == "") return null;

            var client = new RestClient();

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/maplist/detailedlist/" + sortString + "/1,999");

            var request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<DetailedMapsRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static MapInfoRootObject GetMapInfo(EFilter_Game game, string map)
        {
            string gameString = EFilter_ToString.toString(game);

            if (gameString == "" || map == "") return null;

            var client = new RestClient();

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/map/" + map + "/mapinfo");

            var request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<MapInfoRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static MapTopRootObject GetMapTop(EFilter_Game game, string map, EFilter_Mode mode, int zone)
        {
            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || map == "" || zone < 0) return null;

            var client = new RestClient();

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/map/" + map + "/zone/" + zone + "/1,10/" + modeString);

            var request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<MapTopRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static MapPointsRootObject GetMapPoints(EFilter_Game game, string map)
        {
            string gameString = EFilter_ToString.toString(game);

            if (gameString == "" || map == "") return null;

            var client = new RestClient();

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/map/" + map + "/points");

            var request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<MapPointsRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

    }
}
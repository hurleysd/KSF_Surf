using System;
using System.Threading.Tasks;

using Newtonsoft.Json;
using RestSharp;

using KSF_Surf.Models;


namespace KSF_Surf.ViewModels
{
    public class MapsViewModel : BaseViewModel
    {
        // objects for HTTP requests
        private readonly RestClient client;
        private readonly RestRequest request;
        private IRestResponse response = null;

        public MapsViewModel()
        {
            Title = "Maps";

            client = new RestClient();
            request = new RestRequest
            {
                Method = Method.GET,
                RequestFormat = DataFormat.Json
            };
        }

        // KSF API calls -----------------------------------------------------------------------------------------------------------

        internal async Task<DetailedMapsRootObject> GetDetailedMapsList(EFilter_Game game, EFilter_Sort sort)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string sortString = EFilter_ToString.toString(sort);

            if (gameString == "" || sortString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/maplist/detailedlist/" + sortString + "/1,999");
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<DetailedMapsRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MapInfoRootObject> GetMapInfo(EFilter_Game game, string map)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);

            if (gameString == "" || map == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/map/" + map + "/mapinfo");
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<MapInfoRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MapTopRootObject> GetMapTop(EFilter_Game game, string map, EFilter_Mode mode, int zone)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || map == "" || zone < 0) return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/map/" + map + "/zone/" + zone + "/1,10/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<MapTopRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MapPointsRootObject> GetMapPoints(EFilter_Game game, string map)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);

            if (gameString == "" || map == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/map/" + map + "/points");
            await Task.Run(() => response = client.Execute(request));

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
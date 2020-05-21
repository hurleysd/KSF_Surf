using System;
using System.Threading.Tasks;

using Newtonsoft.Json;
using RestSharp;

using KSF_Surf.Models;


namespace KSF_Surf.ViewModels
{
    public class MapsViewModel : BaseViewModel
    {
        public MapsViewModel()
        {
            Title = "Maps";
        }

        // KSF API calls -----------------------------------------------------------------------------------------------------------
        #region ksf

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

        internal async Task<MapTopRootObject> GetMapTop(EFilter_Game game, string map, EFilter_Mode mode, int zone, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || map == "" || zone < 0) return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/map/" + map + "/zone/" + zone 
                + "/" + start_index + ",25/" + modeString);
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

        internal async Task<MapPRInfoRootObject> GetMapPRInfo(EFilter_Game game, EFilter_Mode mode, string map, EFilter_PlayerType playerType, string playerValue)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EFilter_ToString.toString(playerType);

            if (gameString == "" || map == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/map/" + map + "/zone/0/" 
                + playerTypeString + "/" + playerValue + "/recordinfo/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<MapPRInfoRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MapPRRootObject> GetMapPR(EFilter_Game game, EFilter_Mode mode, string map, EFilter_PlayerType playerType, string playerValue)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EFilter_ToString.toString(playerType);

            if (gameString == "" || map == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/" + playerTypeString 
                + "/" + playerValue + "/prinfo/map/" + map + "/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<MapPRRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MapCPRRootObject> GetMapCPR(EFilter_Game game, EFilter_Mode mode, string map, EFilter_PlayerType playerType, string playerValue)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EFilter_ToString.toString(playerType);

            if (gameString == "" || map == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/" + playerTypeString
                + "/" + playerValue + "/cpr/map/" + map + "/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<MapCPRRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MapCCPRootObject> GetMapCCP(EFilter_Game game, EFilter_Mode mode, string map, EFilter_PlayerType playerType, string playerValue)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EFilter_ToString.toString(playerType);

            if (gameString == "" || map == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/" + playerTypeString
                + "/" + playerValue + "/ccp/map/" + map + "/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<MapCCPRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
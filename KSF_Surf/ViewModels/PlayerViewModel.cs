using Newtonsoft.Json;
using RestSharp;

using System;

using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        // objects for HTTP requests
        private readonly RestClient client;
        private readonly RestRequest request;

        public PlayerViewModel()
        {
            Title = "Player";

            client = new RestClient();
            request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;
        }

        // KSF API calls ------------------------------------------------------------------------------------------------------
        #region ksf
        
        internal PlayerInfoRootObject GetPlayerInfo(EFilter_Game game, EFilter_Mode mode, EFilter_PlayerType playerType, string playerValue)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EFilter_ToString.toString(playerType);
            if (gameString == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/" + playerTypeString + "/" + playerValue + "/playerinfo/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<PlayerInfoRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal PlayerRecordsRootObject GetPlayerRecords(EFilter_Game game, EFilter_Mode mode, EFilter_PlayerRecordsType recordsType, EFilter_PlayerType playerType, string playerValue)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string recordsString = EFilter_ToString.toString(recordsType);
            string playerTypeString = EFilter_ToString.toString(playerType);
            if (gameString == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/" + playerTypeString + "/" + playerValue + "/" + recordsString + "/1,10/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<PlayerRecordsRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }
#endregion
        // Steam API call -----------------------------------------------------------------------------------------------------
        #region steam
        internal SteamProfileRootObject GetPlayerSteamProfile(string steamid)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string steamid64 = SteamIDConverter.Steam32to64(steamid);
            string key = "";

            client.BaseUrl = new Uri("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + key + "&steamids=" + steamid64);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<SteamProfileRootObject>(response.Content);
            }
            else
            {
                return null;
            }
            #endregion
        }

    }
}
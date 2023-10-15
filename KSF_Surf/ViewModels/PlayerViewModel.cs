using System;
using System.Threading.Tasks;

using Newtonsoft.Json;
using RestSharp;

using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {

        public PlayerViewModel()
        {
            Title = "Player";
        }

        // KSF API calls ------------------------------------------------------------------------------------------------------
        #region ksf
        
        internal async Task<PlayerInfoRootObject> GetPlayerInfo(EFilter_Game game, EFilter_Mode mode, EFilter_PlayerType playerType, string playerValue)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EFilter_ToString.toString(playerType);
            if (gameString == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/" + playerTypeString + "/" + playerValue + "/playerinfo/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<PlayerInfoRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<PlayerRecordsRootObject> GetPlayerRecords(EFilter_Game game, EFilter_Mode mode, EFilter_PlayerRecordsType recordsType, 
            EFilter_PlayerType playerType, string playerValue, int startIndex)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string recordsString = EFilter_ToString.toString(recordsType);
            string playerTypeString = EFilter_ToString.toString(playerType);
            if (gameString == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/" + playerTypeString + "/" + playerValue + "/" 
                + recordsString + "/" + startIndex + ",10/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<PlayerRecordsRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<PlayerWRsRootObject> GetPlayerWRs(EFilter_Game game, EFilter_Mode mode, EFilter_PlayerWRsType wrType, 
            EFilter_PlayerType playerType, string playerValue, int startIndex)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string wrString = EFilter_ToString.toString(wrType);
            string playerTypeString = EFilter_ToString.toString(playerType);
            if (gameString == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/" + playerTypeString + "/" + playerValue + "/" 
                + wrString + "/" + startIndex + ",10/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<PlayerWRsRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<PlayerTierCompletionRootObject> GetPlayerTierCompletion(EFilter_Game game, EFilter_Mode mode, 
            EFilter_PlayerType playerType, string playerValue)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EFilter_ToString.toString(playerType);
            if (gameString == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/" + playerTypeString + "/" + playerValue 
                + "/completionbytier/1,10/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<PlayerTierCompletionRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<PlayerMapsCompletionRootObject> GetPlayerMapsCompletion(EFilter_Game game, EFilter_Mode mode, EFilter_PlayerCompletionType type,
            EFilter_PlayerType playerType, string playerValue, int startIndex)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EFilter_ToString.toString(playerType);
            string completionString = EFilter_ToString.toString(type);
            if (gameString == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/" + playerTypeString + "/" + playerValue
                + "/" + completionString + "/" + startIndex + ",15/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<PlayerMapsCompletionRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<PlayerOldestRecordsRootObject> GetPlayerOldestRecords(EFilter_Game game, EFilter_Mode mode, EFilter_PlayerOldestType oldType,
            EFilter_PlayerType playerType, string playerValue, int startIndex)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EFilter_ToString.toString(playerType);
            string oldestString = EFilter_ToString.toString(oldType);

            if (gameString == "" || playerValue == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/" + playerTypeString + "/" + playerValue + "/" 
                + oldestString + "/" + startIndex + ",10/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<PlayerOldestRecordsRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        #endregion
        // Steam API call -----------------------------------------------------------------------------------------------------
        #region steam

        internal async Task<SteamProfileRootObject> GetPlayerSteamProfile(string steamid)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string steamid64 = SteamIDConverter.Steam32to64(steamid);

            var srequest = new RestRequest
            {
                Method = Method.GET
            };
            srequest.RequestFormat = DataFormat.Json;

            client.BaseUrl = new Uri("http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=" + Precondition.STEAM + "&steamids=" + steamid64);
            await Task.Run(() => response = client.Execute(srequest));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<SteamProfileRootObject>(response.Content);
            }
            else
            {
                return null;
            }  
        }
        #endregion

    }
}
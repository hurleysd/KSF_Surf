using System.Threading.Tasks;
using System.Text.Json;
using RestSharp;
using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class PlayerViewModel : BaseViewModel
    {
        // query size limits
        internal readonly static int SETBROKEN_RECORDS_QLIMIT = 10;
        internal readonly static int WORLD_RECORDS_QLIMIT = 15;
        internal readonly static int MAPS_COMPLETION_QLIMIT = 15;
        internal readonly static int OLDEST_RECORDS_QLIMIT = 15;

        // call size limits
        internal readonly static int SETBROKEN_RECORDS_CLIMIT = 50;
        internal readonly static int WORLD_RECORDS_CLIMIT = 200;
        internal readonly static int MAPS_COMPLETION_CLIMIT = 999;
        internal readonly static int OLDEST_RECORDS_CLIMIT = 200;

        public PlayerViewModel()
        {
        }

        // KSF API calls ------------------------------------------------------------------------------------------------------
        #region ksf
        
        internal async Task<PlayerInfoRoot> GetPlayerInfo(GameEnum game, ModeEnum mode, PlayerTypeEnum playerType, string playerValue)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EnumToString.APIString(playerType);

            RestRequest request = new RestRequest(gameString + "/" + playerTypeString + "/" + playerValue + "/playerinfo/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<PlayerInfoRoot>(response.Content);
            else return null;
        }

        internal async Task<PlayerRecentRecordsRoot> GetPlayerRecords(GameEnum game, ModeEnum mode, PlayerRecordsTypeEnum recordsType, 
            PlayerTypeEnum playerType, string playerValue, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();
            string recordsString = EnumToString.APIString(recordsType);
            string playerTypeString = EnumToString.APIString(playerType);

            RestRequest request = new RestRequest(gameString + "/" + playerTypeString + "/" + playerValue + "/" + recordsString + "/" + startIndex + "," + SETBROKEN_RECORDS_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<PlayerRecentRecordsRoot>(response.Content);
            else return null;
        }

        internal async Task<PlayerWorldRecordsRoot> GetPlayerWRs(GameEnum game, ModeEnum mode, PlayerWorldRecordsTypeEnum wrType, 
            PlayerTypeEnum playerType, string playerValue, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();
            string wrString = EnumToString.APIString(wrType);
            string playerTypeString = EnumToString.APIString(playerType);

            RestRequest request = new RestRequest(gameString + "/" + playerTypeString + "/" + playerValue + "/" + wrString + "/" + startIndex + "," + WORLD_RECORDS_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<PlayerWorldRecordsRoot>(response.Content);
            else return null;
        }

        internal async Task<PlayerTierCompletionRoot> GetPlayerTierCompletion(GameEnum game, ModeEnum mode, 
            PlayerTypeEnum playerType, string playerValue)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EnumToString.APIString(playerType);

            RestRequest request = new RestRequest(gameString + "/" + playerTypeString + "/" + playerValue + "/completionbytier/1,10/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<PlayerTierCompletionRoot>(response.Content);
            else return null;
        }

        internal async Task<PlayerMapCompletionsRoot> GetPlayerMapsCompletion(GameEnum game, ModeEnum mode, PlayerCompletionTypeEnum type,
            PlayerTypeEnum playerType, string playerValue, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EnumToString.APIString(playerType);
            string completionString = EnumToString.APIString(type);

            RestRequest request = new RestRequest(gameString + "/" + playerTypeString + "/" + playerValue + "/" + completionString + "/" + startIndex + "," + MAPS_COMPLETION_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<PlayerMapCompletionsRoot>(response.Content);
            else return null;
        }

        internal async Task<PlayerOldestRecordsRoot> GetPlayerOldestRecords(GameEnum game, ModeEnum mode, PlayerOldestRecordsTypeEnum oldType,
            PlayerTypeEnum playerType, string playerValue, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EnumToString.APIString(playerType);
            string oldestString = EnumToString.APIString(oldType);

            RestRequest request = new RestRequest(gameString + "/" + playerTypeString + "/" + playerValue + "/" + oldestString + "/" + startIndex + "," + OLDEST_RECORDS_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<PlayerOldestRecordsRoot>(response.Content);
            else return null;
        }

        #endregion
        // Steam API call -----------------------------------------------------------------------------------------------------
        #region steam

        internal async Task<SteamProfilesRoot> GetPlayerSteamProfile(string steamID)
        {
            if (!BaseViewModel.HasConnection()) return null;

            RestRequest request = new RestRequest("ISteamUser/GetPlayerSummaries/v0002/?key=" + Precondition.STEAM + "&steamids=" + StringFormatter.Steam32to64(steamID))
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await SteamClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<SteamProfilesRoot>(response.Content);
            else return null;
        }
        #endregion
    }
}
using System.Threading.Tasks;
using System.Text.Json;
using RestSharp;
using KSF_Surf.Models;


namespace KSF_Surf.ViewModels
{
    public class MapsViewModel : BaseViewModel
    {
        // query size limits
        internal readonly static int TOP_QLIMIT = 25;

        // call size limits
        internal readonly static int TOP_CLIMIT = 100;

        public MapsViewModel()
        {
        }

        // KSF API calls -----------------------------------------------------------------------------------------------------------
        #region ksf

        internal async Task<DetailedMapsRoot> GetDetailedMapsList(GameEnum game, SortEnum sort)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string sortString = EnumToString.APIString(sort);

            RestRequest request = new RestRequest(gameString + "/maplist/detailedlist/" + sortString + "/1,999")
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<DetailedMapsRoot>(response.Content);
            else return null;
        }

        internal async Task<MapInfoRoot> GetMapInfo(GameEnum game, string map)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);

            RestRequest request = new RestRequest(gameString + "/map/" + map + "/mapinfo")
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            return JsonSerializer.Deserialize<MapInfoRoot>(response.Content);
            else return null;
        }

        internal async Task<MapTopsRoot> GetMapTop(GameEnum game, string map, ModeEnum mode, int zone, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/map/" + map + "/zone/" + zone + "/" + startIndex + "," + TOP_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MapTopsRoot>(response.Content);
            else return null;
        }

        internal async Task<MapPointsRootObject> GetMapPoints(GameEnum game, string map)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);

            RestRequest request = new RestRequest(gameString + "/map/" + map + "/points")
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MapPointsRootObject>(response.Content);
            else return null;
        }

        internal async Task<MapPersonalRecordInfoRoot> GetMapPRInfo(GameEnum game, ModeEnum mode, string map, PlayerTypeEnum playerType, string playerValue)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EnumToString.APIString(playerType);

            RestRequest request = new RestRequest(gameString + "/map/" + map + "/zone/0/" + playerTypeString + "/" + playerValue + "/recordinfo/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MapPersonalRecordInfoRoot>(response.Content);
            else return null;
        }

        internal async Task<MapPersonalRecordRoot> GetMapPR(GameEnum game, ModeEnum mode, string map, PlayerTypeEnum playerType, string playerValue)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EnumToString.APIString(playerType);

            RestRequest request = new RestRequest(gameString + "/" + playerTypeString + "/" + playerValue + "/prinfo/map/" + map + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MapPersonalRecordRoot>(response.Content);
            else return null;
        }

        internal async Task<MapComparePersonalRecordRoot> GetMapCPR(GameEnum game, ModeEnum mode, string map, PlayerTypeEnum playerType, string playerValue)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EnumToString.APIString(playerType);

            RestRequest request = new RestRequest(gameString + "/" + playerTypeString + "/" + playerValue + "/cpr/map/" + map + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MapComparePersonalRecordRoot>(response.Content);
            else return null;
        }

        internal async Task<MapCompareCheckPointsRoot> GetMapCCP(GameEnum game, ModeEnum mode, string map, PlayerTypeEnum playerType, string playerValue)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();
            string playerTypeString = EnumToString.APIString(playerType);

            RestRequest request = new RestRequest(gameString + "/" + playerTypeString + "/" + playerValue + "/ccp/map/" + map + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MapCompareCheckPointsRoot>(response.Content);
            else return null;
        }

        #endregion
    }
}
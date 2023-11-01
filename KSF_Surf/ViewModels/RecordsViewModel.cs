using System.Threading.Tasks;
using System.Text.Json;
using RestSharp;
using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class RecordsViewModel : BaseViewModel
    {
        // query size limits
        internal readonly static int SURF_TOP_QLIMIT = 25;
        internal readonly static int RECENT_RECORDS_QLIMIT = 10;
        internal readonly static int OLDEST_RECORDS_QLIMIT = 10;
        internal readonly static int MOST_QLIMIT = 25;
        internal readonly static int TOP_COUNTRIES_QLIMIT = 25;
        internal readonly static int COUNTRY_TOP_QLIMIT = 25;

        // call size limits
        internal readonly static int SURF_TOP_CLIMIT = 500;
        internal readonly static int RECENT_RECORDS_CLIMIT = 100;
        internal readonly static int OLDEST_RECORDS_CLIMIT = 250;
        internal readonly static int MOST_CLIMIT = 500;
        internal readonly static int TOP_COUNTRIES_CLIMIT = 200;
        internal readonly static int COUNTRY_TOP_CLIMIT = 500;

        public RecordsViewModel()
        {
        }

        // KSF API calls -------------------------------------------------------------------------------------------
        #region ksf

        internal async Task<SurfTopRoot> GetSurfTop(GameEnum game, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/server/" + startIndex + "," + SURF_TOP_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<SurfTopRoot>(response.Content);
            else return null;
        }

        internal async Task<RecentRecordsRoot> GetRecentRecords(GameEnum game, RecentRecordsTypeEnum type, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string typeString = EnumToString.APIString(type);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/recentrecords/server/" + typeString + "/" + startIndex + "," + RECENT_RECORDS_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<RecentRecordsRoot>(response.Content);
            else return null;
        }

        internal async Task<RecentRecords10Root> GetRecentRecords10(GameEnum game, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/recentrecords/server/top10/" + startIndex + "," + RECENT_RECORDS_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<RecentRecords10Root>(response.Content);
            else return null;
        }

        internal async Task<OldestRecordsRoot> GetOldestRecords(GameEnum game, OldestRecordsTypeEnum type, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string typeString = EnumToString.APIString(type);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/oldestrecords/server/" + typeString + "/" + startIndex + "," + OLDEST_RECORDS_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<OldestRecordsRoot>(response.Content);
            else return null;
        }

        internal async Task<MostPercentCompletionRoot> GetMostPC(GameEnum game, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/pc/" + startIndex + "," + MOST_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MostPercentCompletionRoot>(response.Content);
            else return null;
        }

        internal async Task<MostCountRoot> GetMostCount(GameEnum game, MostTypeEnum type, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string typeString = EnumToString.APIString(type);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/" + typeString + "/" + startIndex + "," + MOST_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MostCountRoot>(response.Content);
            else return null;
        }

        internal async Task<MostTopsRoot> GetMostTop(GameEnum game, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/top10/" + startIndex + "," + MOST_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MostTopsRoot>(response.Content);
            else return null;
        }

        internal async Task<MostGroupRoot> GetMostGroup(GameEnum game, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/group/" + startIndex + "," + MOST_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MostGroupRoot>(response.Content);
            else return null;
        }

        internal async Task<MostContestedWorldRecordsRoot> GetMostContWr(GameEnum game, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/mostcontestedwr/" + startIndex + "," + MOST_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MostContestedWorldRecordsRoot>(response.Content);
            else return null;
        }

        internal async Task<MostContestedZoneRoot> GetMostContZone(GameEnum game, MostTypeEnum type, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string typeString = EnumToString.APIString(type);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/" + typeString + "/" + startIndex + "," + MOST_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MostContestedZoneRoot>(response.Content);
            else return null;
        }

        internal async Task<MostTimeRoot> GetMostTime(GameEnum game, MostTypeEnum type, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string typeString = EnumToString.APIString(type);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/" + typeString + "/" + startIndex + "," + MOST_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<MostTimeRoot>(response.Content);
            else return null;
        }

        internal async Task<TopCountriesRoot> GetTopCountries(GameEnum game, ModeEnum mode, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/countries/" + startIndex + "," + MOST_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<TopCountriesRoot>(response.Content);
            else return null;
        }

        internal async Task<string> GetTopCountry(GameEnum game, ModeEnum mode)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/countries/1,1/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TopCountriesRoot tcro = JsonSerializer.Deserialize<TopCountriesRoot>(response.Content);
                return tcro.data[0]?.country;
            }
            else return null;
        }

        internal async Task<CountryTopsObject> GetCountryTop(GameEnum game, ModeEnum mode, string country, int startIndex)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);
            string modeString = ((int)mode).ToString();

            RestRequest request = new RestRequest(gameString + "/top/country/" + country + "/" + startIndex + "," + COUNTRY_TOP_QLIMIT + "/" + modeString)
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<CountryTopsObject>(response.Content);
            else return null;
        }
        #endregion
    }
}
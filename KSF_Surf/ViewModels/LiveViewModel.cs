using System.Threading.Tasks;
using System.Text.Json;
using RestSharp;
using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class LiveViewModel : BaseViewModel
    {
        public LiveViewModel()
        {
        }

        // KSF API call -----------------------------------------------------------------------
        #region ksf

        internal async Task<ServersRoot> GetServers(GameEnum game)
        {
            if (!BaseViewModel.HasConnection()) return null;

            string gameString = EnumToString.APIString(game);

            RestRequest request = new RestRequest(gameString + "/servers/list")
            {
                Method = Method.Get,
                RequestFormat = DataFormat.Json,
            }.AddHeader("x-auth-token", Precondition.KSF);

            RestResponse response = await KSFClient.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK) return JsonSerializer.Deserialize<ServersRoot>(response.Content);
            else return null;
        }

        #endregion
    }
}

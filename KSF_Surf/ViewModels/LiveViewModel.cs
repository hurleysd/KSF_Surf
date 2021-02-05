using System;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft.Json;

using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class LiveViewModel : BaseViewModel
    {
        public LiveViewModel()
        {
            Title = "Live";
        }

        // KSF API call -------------------------------------------------------------------------
        #region ksf

        internal async Task<KSFServerRootObject> GetServers(EFilter_Game game)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            if (gameString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/servers/list");
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<KSFServerRootObject>(response.Content);
            }
            else
            {
                if (BaseViewModel.client.UserAgent != "" || BaseViewModel.AGENT != "")
                {
                    BaseViewModel.AGENT = "";
                    BaseViewModel.client.UserAgent = "";
                    return await GetServers(game);
                }
                return null;
            }
        }

        #endregion
    }
}

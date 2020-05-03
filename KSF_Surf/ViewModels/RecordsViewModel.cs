using System;
using System.Threading.Tasks;

using RestSharp;
using Newtonsoft.Json;

using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class RecordsViewModel : BaseViewModel
    {
        // objects for HTTP requests
        private readonly RestClient client;
        private readonly RestRequest request;
        private IRestResponse response = null;

        public RecordsViewModel()
        {
            Title = "Records";

            client = new RestClient();
            request = new RestRequest
            {
                Method = Method.GET,
                RequestFormat = DataFormat.Json
            };
        }

        // KSF API calls -------------------------------------------------------------------------------------------
        #region ksf

        internal async Task<SurfTopRootObject> GetSurfTop(EFilter_Game game, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/server/" + start_index + ",25/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<SurfTopRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<RRRootObject> GetRecentRecords(EFilter_Game game, EFilter_RRType type, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string typeString = EFilter_ToString.toString(type);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || typeString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/recentrecords/server/" + type + "/1,10/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                
                return JsonConvert.DeserializeObject<RRRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<RR10RootObject> GetRecentRecords10(EFilter_Game game, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/recentrecords/server/top10/1,10/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<RR10RootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<OldestRecordsObject> GetOldestRecords(EFilter_Game game, EFilter_ORType type, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string typeString = EFilter_ToString.toString(type);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || typeString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/oldestrecords/server/" + typeString + "/" + start_index + ",10/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<OldestRecordsObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MostPCRootObject> GetMostPC(EFilter_Game game, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "" ) return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/pc/" + start_index + ",25/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostPCRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MostCountRootObject> GetMostCount(EFilter_Game game, EFilter_MostType type, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string typeString = EFilter_ToString.toString(type);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || typeString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/" + typeString + "/" + start_index + ",25/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostCountRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MostTopRootObject> GetMostTop(EFilter_Game game, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/top10/" + start_index + ",25/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostTopRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MostGroupRootObject> GetMostGroup(EFilter_Game game, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/group/" + start_index + ",25/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostGroupRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MostContWrRootObject> GetMostContWr(EFilter_Game game, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/mostcontestedwr/" + start_index + ",25/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostContWrRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MostContZoneRootObject> GetMostContZone(EFilter_Game game, EFilter_MostType type, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string typeString = EFilter_ToString.toString(type);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || typeString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/" + typeString + "/" + start_index + ",25/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostContZoneRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<MostTimeRootObject> GetMostTime(EFilter_Game game, EFilter_MostType type, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string typeString = EFilter_ToString.toString(type);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || typeString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/" + typeString + "/" + start_index + ",25/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostTimeRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
using System;

using RestSharp;
using Newtonsoft.Json;
using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class RecordsViewModel : BaseViewModel
    {
        private static RestClient client;
        private static RestRequest request;

        public RecordsViewModel()
        {
            Title = "Records";

            client = new RestClient();
            request = new RestRequest();
            request.Method = Method.GET;
            request.RequestFormat = DataFormat.Json;
        }

        internal static SurfTopRootObject GetSurfTop(EFilter_Game game, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/server/1,10/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<SurfTopRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static RRRootObject GetRecentRecords(EFilter_Game game, EFilter_RRType type, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string typeString = EFilter_ToString.toString(type);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || typeString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/recentrecords/server/" + type + "/1,10/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                
                return JsonConvert.DeserializeObject<RRRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static RR10RootObject GetRecentRecords10(EFilter_Game game, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/recentrecords/server/top10/1,10/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<RR10RootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static MostPCRootObject GetMostPC(EFilter_Game game, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "" ) return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/pc/1,10/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostPCRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static MostCountRootObject GetMostCount(EFilter_Game game, EFilter_MostType type, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string typeString = EFilter_ToString.toString(type);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || typeString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/" + typeString + "/1,10/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostCountRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static MostTopRootObject GetMostTop(EFilter_Game game, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/top10/1,10/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostTopRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static MostGroupRootObject GetMostGroup(EFilter_Game game, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/group/1,10/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostGroupRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static MostContWrRootObject GetMostContWr(EFilter_Game game, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/mostcontestedwr/1,10/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostContWrRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static MostContZoneRootObject GetMostContZone(EFilter_Game game, EFilter_MostType type, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string typeString = EFilter_ToString.toString(type);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || typeString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/" + typeString + "/1,10/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostContZoneRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal static MostTimeRootObject GetMostTime(EFilter_Game game, EFilter_MostType type, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string typeString = EFilter_ToString.toString(type);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || typeString == "") return null;


            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/" + typeString + "/1,10/" + modeString);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                return JsonConvert.DeserializeObject<MostTimeRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }
    }
}
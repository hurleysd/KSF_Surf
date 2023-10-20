using System;
using System.Threading.Tasks;

using Newtonsoft.Json;

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
            Title = "Records";
        }

        // KSF API calls -------------------------------------------------------------------------------------------
        #region ksf

        internal async Task<SurfTopRootObject> GetSurfTop(EFilter_Game game, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/server/" + start_index + "," + SURF_TOP_QLIMIT + "/" + modeString);
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

        internal async Task<RRRootObject> GetRecentRecords(EFilter_Game game, EFilter_RRType type, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string typeString = EFilter_ToString.toString(type);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || typeString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/recentrecords/server/" + type + "/" + start_index + "," + RECENT_RECORDS_QLIMIT + "/" + modeString);
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

        internal async Task<RR10RootObject> GetRecentRecords10(EFilter_Game game, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/recentrecords/server/top10/" + start_index + "," + RECENT_RECORDS_QLIMIT + "/" + modeString);
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

        internal async Task<OldestRecordsRootObject> GetOldestRecords(EFilter_Game game, EFilter_ORType type, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string typeString = EFilter_ToString.toString(type);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || typeString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/oldestrecords/server/" + typeString + "/" + start_index + "," + OLDEST_RECORDS_QLIMIT + "/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<OldestRecordsRootObject>(response.Content);
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

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/pc/" + start_index + "," + MOST_QLIMIT + "/" + modeString);
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

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/" + typeString + "/" + start_index + "," + MOST_QLIMIT + "/" + modeString);
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

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/top10/" + start_index + "," + MOST_QLIMIT + "/" + modeString);
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

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/group/" + start_index + "," + MOST_QLIMIT + "/" + modeString);
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

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/mostcontestedwr/" + start_index + "," + MOST_QLIMIT + "/" + modeString);
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

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/" + typeString + "/" + start_index + "," + MOST_QLIMIT + "/" + modeString);
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

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/" + typeString + "/" + start_index + "," + MOST_QLIMIT + "/" + modeString);
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

        internal async Task<TopCountriesRootObject> GetTopCountries(EFilter_Game game, EFilter_Mode mode, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/countries/" + start_index + "," + MOST_QLIMIT + "/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<TopCountriesRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }

        internal async Task<string> GetTopCountry(EFilter_Game game, EFilter_Mode mode)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/countries/1,1/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TopCountriesRootObject tcro = JsonConvert.DeserializeObject<TopCountriesRootObject>(response.Content);
                return tcro.data[0]?.country;
            }
            else
            {
                return null;
            }
        }

        internal async Task<CountryTopRootObject> GetCountryTop(EFilter_Game game, EFilter_Mode mode, string country, int start_index)
        {
            if (!BaseViewModel.hasConnection()) return null;

            string gameString = EFilter_ToString.toString(game);
            string modeString = ((int)mode).ToString();

            if (gameString == "" || country == "") return null;

            client.BaseUrl = new Uri("http://surf.ksfclan.com/api2/" + gameString + "/top/country/" + country + "/" + start_index + "," + COUNTRY_TOP_QLIMIT + "/" + modeString);
            await Task.Run(() => response = client.Execute(request));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<CountryTopRootObject>(response.Content);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
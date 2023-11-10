using Xamarin.Essentials;
using RestSharp;

namespace KSF_Surf.ViewModels
{
    public class BaseViewModel
    {
        // Strings
        internal readonly static string APP_VERSION = "2.2.0 (35)";
        internal readonly static string DEFAULT_ME_STEAM_ID = "STEAM_0:0:47620794"; // Sean's steam ID (hesuka)

        // Rest
        internal static RestClient KSFClient = new RestClient(new RestClientOptions("http://surf.ksfclan.com/api2/")
        {
            UserAgent = PropertiesDict.GetUserAgent()
        });

        internal static RestClient SteamClient = new RestClient(new RestClientOptions("http://api.steampowered.com/"));

        public BaseViewModel()
        {
        }

        internal static bool HasConnection()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    }
}

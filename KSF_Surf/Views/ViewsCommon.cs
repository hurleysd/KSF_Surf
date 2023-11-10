using System.Threading.Tasks;
using Xamarin.Forms;

namespace KSF_Surf.Views
{
    internal static class ViewsCommon
    {
        // Alerts -----------------------------------------------------------------------------

        public static async Task DisplayNoConnectionAlert(ContentPage p)
        {
            await p.DisplayAlert("Could not connect to KSF!", "Please connect to the Internet", "OK");
        }

        public static async Task DisplayNoSteamConnectionAlert(ContentPage p)
        {
            await p.DisplayAlert("Could not connect to Steam!", "Please connect to the Internet", "OK");
        }

        public static async Task DisplayNoWebConnectionAlert(ContentPage p)
        {
            await p.DisplayAlert("Could not open web page!", "Please connect to the Internet", "OK");
        }

        public static async Task DisplayProfileFailureAlert(ContentPage p, bool includeRank)
        {
            await p.DisplayAlert("Could not find player profile!", "Invalid Steam ID" + (includeRank? " or rank" : ""), "OK");
        }
    }
}

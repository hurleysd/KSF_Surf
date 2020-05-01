using Xamarin.Essentials;
using Xamarin.Forms;

using KSF_Surf.Views;
using KSF_Surf.Models;

namespace KSF_Surf
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static void ApplyTheme()
        {
            if (AppInfo.RequestedTheme == AppTheme.Dark)
            {
                App.Current.Resources["ViewBackgroundColor"] = Color.FromHex("#1b1b1b");
                App.Current.Resources["GrayTextColor"] = Color.FromHex("#9e9e9e");
                App.Current.Resources["HeaderTextColor"] = Color.WhiteSmoke;
                App.Current.Resources["SeparatorColor"] = Color.Black;
                App.Current.Resources["BorderColor"] = Color.FromHex("#4f4f4f");
                App.Current.Resources["AppBackgroundColor"] = Color.Black;
                App.Current.Resources["BarBackgroundColor"] = Color.FromHex("#171717");
                App.Current.Resources["TabBackgroundColor"] = Color.Black;
            }
            else
            {
                App.Current.Resources["ViewBackgroundColor"] = Color.FromHex("#e4e4e4");
                App.Current.Resources["GrayTextColor"] = Color.FromHex("#696969");
                App.Current.Resources["HeaderTextColor"] = Color.Black;
                App.Current.Resources["SeparatorColor"] = Color.WhiteSmoke;
                App.Current.Resources["BorderColor"] = Color.FromHex("#b5b5b5");
                App.Current.Resources["AppBackgroundColor"] = Color.WhiteSmoke;
                App.Current.Resources["BarBackgroundColor"] = Color.FromHex("#e8e8e8");
                App.Current.Resources["TabBackgroundColor"] = Color.FromHex("#e8e8e8");
            }
        }

    }
}

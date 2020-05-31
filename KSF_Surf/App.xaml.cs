using System.Globalization;

using Xamarin.Essentials;
using Xamarin.Forms;

using KSF_Surf.Views;

namespace KSF_Surf
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SetCultureToUSEnglish();

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

        #region theme

        public static void ApplyTheme()
        {
            if (AppInfo.RequestedTheme == AppTheme.Dark)
            {
                ApplyDarkTheme();
            }
            else
            {
                ApplyLightTheme();
            }
        }

        public static void ApplyDarkTheme()
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

        public static void ApplyLightTheme()
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

        private void SetCultureToUSEnglish()
        {
            CultureInfo englishUSCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = englishUSCulture;
        }

        #endregion
    }
}

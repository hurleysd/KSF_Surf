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
                App.Current.Resources["ViewBackgroundColor"] = Xamarin.Forms.Color.FromHex("#363636");
                App.Current.Resources["GrayTextColor"] = Xamarin.Forms.Color.FromHex("#9e9e9e");
                App.Current.Resources["HeaderTextColor"] = Xamarin.Forms.Color.WhiteSmoke;
                App.Current.Resources["SeparatorColor"] = Xamarin.Forms.Color.Black;
                App.Current.Resources["AppBackgroundColor"] = Xamarin.Forms.Color.Black;
                App.Current.Resources["TabBackgroundColor"] = Xamarin.Forms.Color.FromHex("#171717");
            }
            else
            {
                App.Current.Resources["ViewBackgroundColor"] = Xamarin.Forms.Color.LightGray;
                App.Current.Resources["GrayTextColor"] = Xamarin.Forms.Color.FromHex("#696969");
                App.Current.Resources["HeaderTextColor"] = Xamarin.Forms.Color.Black;
                App.Current.Resources["SeparatorColor"] = Xamarin.Forms.Color.WhiteSmoke;
                App.Current.Resources["AppBackgroundColor"] = Xamarin.Forms.Color.WhiteSmoke;
                App.Current.Resources["TabBackgroundColor"] = Xamarin.Forms.Color.FromHex("#e8e8e8");
            }
        }
    }
}

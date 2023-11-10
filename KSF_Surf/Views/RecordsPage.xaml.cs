using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using KSF_Surf.Models;
using KSF_Surf.ViewModels;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsPage : ContentPage
    {
        // variables for current filters
        private readonly GameEnum defaultGame;
        private readonly ModeEnum defaultMode;
        private GameEnum game;
        private ModeEnum mode;

        public RecordsPage()
        {
            defaultGame = PropertiesDict.GetGame();
            defaultMode = PropertiesDict.GetMode();
            game = defaultGame;
            mode = defaultMode;

            InitializeComponent();
            Title = "[" + EnumToString.NameString(game) + "," + EnumToString.NameString(mode) + "] Records";
        }

        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecordsFilterPage(ApplyFilters, game, mode, defaultGame, defaultMode));
        }

        internal void ApplyFilters(GameEnum newGame, ModeEnum newMode)
        {
            game = newGame;
            mode = newMode;
            Title = "[" + EnumToString.NameString(game) + "," + EnumToString.NameString(mode) + "] Records";
        }

        private async void RecentRecords_Tapped(object sender, EventArgs e)
        {
            RecentRecordsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsRecentPage(game, mode, defaultGame, defaultMode));
            }
            else await DisplayNoConnectionAlert();

            RecentRecordsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void SurfTop_Tapped(object sender, EventArgs e)
        {
            SurfTopButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsTopPage(game, mode, defaultGame, defaultMode));
            }
            else await DisplayNoConnectionAlert();

            SurfTopButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void CountryTop_Tapped(object sender, EventArgs e)
        {
            CountryTopButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsCountryTopPage(game, mode, defaultGame, defaultMode));
            }
            else await DisplayNoConnectionAlert();

            CountryTopButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void TopCountries_Tapped(object sender, EventArgs e)
        {
            TopCountriesButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsTopCountriesPage(game, mode, defaultGame, defaultMode));
            }
            else await DisplayNoConnectionAlert();

            TopCountriesButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void MostByType_Tapped(object sender, EventArgs e)
        {
            MostButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsMostPage(game, mode, defaultGame, defaultMode));
            }
            else await DisplayNoConnectionAlert();

            MostButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void OldestRecords_Tapped(object sender, EventArgs e)
        {
            OldestRecordsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsOldestPage(game, mode, defaultGame, defaultMode));
            }
            else await DisplayNoConnectionAlert();

            OldestRecordsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async Task DisplayNoConnectionAlert()
        {
            await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }
    }

    #endregion
}
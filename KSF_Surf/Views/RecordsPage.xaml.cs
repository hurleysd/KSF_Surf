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
            ChangeTitle(game, mode);
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private void ChangeTitle(GameEnum game, ModeEnum mode)
        {
            Title = "[" + EnumToString.NameString(game) + "]";
            if (mode != ModeEnum.FW) Title += "[" + EnumToString.NameString(mode) + "]";
            Title += " Records";
        }

        #endregion
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

            ChangeTitle(game, mode);
        }

        private async void RecentRecords_Tapped(object sender, EventArgs e)
        {
            RecentRecordsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsRecentPage(game, mode, defaultGame, defaultMode));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            RecentRecordsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void SurfTop_Tapped(object sender, EventArgs e)
        {
            SurfTopButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsTopPage(game, mode, defaultGame, defaultMode));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            SurfTopButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void CountryTop_Tapped(object sender, EventArgs e)
        {
            CountryTopButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsCountryTopPage(game, mode, defaultGame, defaultMode));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            CountryTopButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void TopCountries_Tapped(object sender, EventArgs e)
        {
            TopCountriesButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsTopCountriesPage(game, mode, defaultGame, defaultMode));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            TopCountriesButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void MostByType_Tapped(object sender, EventArgs e)
        {
            MostButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsMostPage(game, mode, defaultGame, defaultMode));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            MostButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void OldestRecords_Tapped(object sender, EventArgs e)
        {
            OldestRecordsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsOldestPage(game, mode, defaultGame, defaultMode));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            OldestRecordsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }
    }

    #endregion
}
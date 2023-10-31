using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsCountryTopPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "SurfTop" call
        private List<CountryTopDatum> countryTopData;
        private int list_index = 1;

        // variables for filters
        private readonly GameEnum defaultGame;
        private readonly ModeEnum defaultMode;
        private GameEnum game;
        private ModeEnum mode;
        private string country;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> recordsCountryTopCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();

        public RecordsCountryTopPage(GameEnum game, ModeEnum mode, GameEnum defaultGame, ModeEnum defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title = "Records [" + EnumToString.NameString(game) + ", " + EnumToString.NameString(mode) + "]";
            CountryPicker.ItemsSource = StringFormatter.CountryTopCountries;
            RecordsCountryTopCollectionView.ItemsSource = recordsCountryTopCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeCountryTop(bool clearPrev)
        {
            var countryTopDatum = await recordsViewModel.GetCountryTop(game, mode, country, list_index);
            countryTopData = countryTopDatum?.data;
            if (countryTopData is null) return;

            if (clearPrev) recordsCountryTopCollectionViewItemsSource.Clear();
            LayoutCountryTop();
            CountryOptionLabel.Text = "Country: " + StringFormatter.CountryEmoji(country);
            Title = "Records [" + EnumToString.NameString(game) + ", " + EnumToString.NameString(mode) + "]";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutCountryTop()
        {
            foreach (CountryTopDatum datum in countryTopData)
            {
                string nameString = list_index + ". " + datum.playerName;
                string pointsString = StringFormatter.PointsString(datum.points);

                recordsCountryTopCollectionViewItemsSource.Add(new Tuple<string, string, string>(
                    nameString, pointsString, datum.steamID));

                list_index++;
            }
        }

        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;
                country = await recordsViewModel.GetTopCountry(game, mode);
                if (country is null || country == "")
                {
                    country = StringFormatter.CountryTopCountries[58]; // USA
                }

                await ChangeCountryTop(false);

                LoadingAnimation.IsRunning = false;
                RecordsCountryTopStack.IsVisible = true;
            }
        }

        private void CountryOptionLabel_Tapped(object sender, EventArgs e)
        {
            CountryPicker.SelectedItem = country;
            CountryPicker.Focus();
        }

        private async void CountryPicker_Unfocused(object sender, FocusEventArgs e)
        {
            string selected = (string)CountryPicker.SelectedItem;
            if (selected == country) return;

            country = selected;
            list_index = 1;

            LoadingAnimation.IsRunning = true;
            isLoading = true;

            await ChangeCountryTop(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void CountryRecordsTop_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((list_index - 1) % RecordsViewModel.COUNTRY_TOP_QLIMIT) != 0) return; // didn't get full results
            if (list_index >= RecordsViewModel.COUNTRY_TOP_CLIMIT) return; // at call limit

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeCountryTop(false);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void CountryRecordsTop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string> selectedPlayer = (Tuple<string, string, string>)RecordsCountryTopCollectionView.SelectedItem;
            RecordsCountryTopCollectionView.SelectedItem = null;

            string playerSteamId = selectedPlayer.Item3;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsPlayerPage(game, mode, playerSteamId));
            }
            else await DisplayNoConnectionAlert();
        }

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (hasLoaded && BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsFilterPage(ApplyFilters, game, mode, defaultGame, defaultMode));
            }
            else await DisplayNoConnectionAlert();
        }

        internal async void ApplyFilters(GameEnum newGame, ModeEnum newMode)
        {
            if (newGame == game && newMode == mode) return;

            if (BaseViewModel.HasConnection())
            {
                game = newGame;
                mode = newMode;
                list_index = 1;

                LoadingAnimation.IsRunning = true;
                isLoading = true;

                await ChangeCountryTop(true);

                LoadingAnimation.IsRunning = false;
                isLoading = false;
            }
            else await DisplayNoConnectionAlert();
        }

        private async Task DisplayNoConnectionAlert()
        {
            await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.ViewModels;
using KSF_Surf.Models;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsCountryTopPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;
        private readonly int CALL_LIMIT = 500;

        // objects used by "SurfTop" call
        private List<CountryPlayer> countryTopData;
        private int list_index = 1;

        // variables for filters
        private readonly EFilter_Game defaultGame;
        private readonly EFilter_Mode defaultMode;
        private EFilter_Game game;
        private EFilter_Mode mode;
        private string country;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> recordsCountryTopCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();

        public RecordsCountryTopPage(EFilter_Game game, EFilter_Mode mode, EFilter_Game defaultGame, EFilter_Mode defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
            CountryPicker.ItemsSource = EFilter_ToString.ctopCountries;
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
            CountryOptionLabel.Text = "Country: " + String_Formatter.toEmoji_Country(country);
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutCountryTop()
        {
            foreach (CountryPlayer datum in countryTopData)
            {
                string nameString = list_index + ". " + datum.playerName;
                string pointsString = String_Formatter.toString_Points(datum.points);

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
                    country = EFilter_ToString.ctopCountries[58]; // USA
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
            if (isLoading || !BaseViewModel.hasConnection() || list_index == CALL_LIMIT) return;
           
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

            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new RecordsPlayerPage(game, mode, playerSteamId));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
        }

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (hasLoaded && BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new RecordsFilterPage(ApplyFilters, game, mode, defaultGame, defaultMode));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
        }

        internal async void ApplyFilters(EFilter_Game newGame, EFilter_Mode newMode)
        {
            if (newGame == game && newMode == mode) return;
            if (BaseViewModel.hasConnection())
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
            else
            {
                await DisplayNoConnectionAlert();
            }
        }

        private async Task DisplayNoConnectionAlert()
        {
            await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }

        #endregion
    }
}
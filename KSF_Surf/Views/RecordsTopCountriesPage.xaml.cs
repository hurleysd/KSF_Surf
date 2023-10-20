using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.ViewModels;
using KSF_Surf.Models;
using System.Collections.ObjectModel;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsTopCountriesPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "SurfTop" call
        private List<CountryPoints> topCountriesData;
        private int list_index = 1;

        // variables for filters
        private readonly EFilter_Game defaultGame;
        private readonly EFilter_Mode defaultMode;
        private EFilter_Game game;
        private EFilter_Mode mode;

        // collection view
        public ObservableCollection<Tuple<string, string>> recordsTopCountriesCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string>>();

        public RecordsTopCountriesPage(EFilter_Game game, EFilter_Mode mode, EFilter_Game defaultGame, EFilter_Mode defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
            RecordsTopCountriesCollectionView.ItemsSource = recordsTopCountriesCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeTopCountries(bool clearPrev)
        {
            var topCountriesDatum = await recordsViewModel.GetTopCountries(game, mode, list_index);
            topCountriesData = topCountriesDatum?.data;
            if (topCountriesData is null) return;

            if (clearPrev) recordsTopCountriesCollectionViewItemsSource.Clear();
            LayoutTopCountries();
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutTopCountries()
        {
            foreach (CountryPoints datum in topCountriesData)
            {
                string countryString = list_index + ". " + String_Formatter.toEmoji_Country(datum.country) + " " + datum.country;
                string pointsString = String_Formatter.toString_Points(datum.points);

                recordsTopCountriesCollectionViewItemsSource.Add(new Tuple<string, string>(countryString, pointsString));

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
                await ChangeTopCountries(false);

                LoadingAnimation.IsRunning = false;
                RecordsTopCountriesStack.IsVisible = true;
            }
        }

        private async void RecordsTopCoutries_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection()) return;
            if (((list_index - 1) % RecordsViewModel.TOP_COUNTRIES_QLIMIT) != 0) return; // didn't get full results
            if (list_index >= RecordsViewModel.TOP_COUNTRIES_CLIMIT) return; // at call limit

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeTopCountries(false);
            
            LoadingAnimation.IsRunning = false;
            isLoading = false;
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

                await ChangeTopCountries(true);

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
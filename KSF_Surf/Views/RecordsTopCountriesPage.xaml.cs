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
    public partial class RecordsTopCountriesPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "SurfTop" call
        private List<TopCountryDatum> topCountriesData;
        private int listIndex = 1;

        // variables for filters
        private readonly GameEnum defaultGame;
        private readonly ModeEnum defaultMode;
        private GameEnum game;
        private ModeEnum mode;

        // collection view
        public ObservableCollection<Tuple<string, string>> recordsTopCountriesCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string>>();

        public RecordsTopCountriesPage(GameEnum game, ModeEnum mode, GameEnum defaultGame, ModeEnum defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title = "[" + EnumToString.NameString(game) + "," + EnumToString.NameString(mode) + "] Records";
            RecordsTopCountriesCollectionView.ItemsSource = recordsTopCountriesCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeTopCountries(bool clearPrev)
        {
            var topCountriesDatum = await recordsViewModel.GetTopCountries(game, mode, listIndex);
            topCountriesData = topCountriesDatum?.data;
            if (topCountriesData is null) return;

            if (clearPrev) recordsTopCountriesCollectionViewItemsSource.Clear();
            LayoutTopCountries();
            Title = "[" + EnumToString.NameString(game) + "," + EnumToString.NameString(mode) + "] Records";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutTopCountries()
        {
            foreach (TopCountryDatum datum in topCountriesData)
            {
                string countryString = listIndex + ". " + StringFormatter.CountryEmoji(datum.country) + " " + datum.country;
                string pointsString = StringFormatter.PointsString(datum.points);

                recordsTopCountriesCollectionViewItemsSource.Add(new Tuple<string, string>(countryString, pointsString));

                listIndex++;
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
            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((listIndex - 1) % RecordsViewModel.TOP_COUNTRIES_QLIMIT) != 0) return; // didn't get full results
            if (listIndex >= RecordsViewModel.TOP_COUNTRIES_CLIMIT) return; // at call limit

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeTopCountries(false);
            
            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (hasLoaded && BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsFilterPage(ApplyFilters, game, mode, defaultGame, defaultMode));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);
        }

        internal async void ApplyFilters(GameEnum newGame, ModeEnum newMode)
        {
            if (newGame == game && newMode == mode) return;

            if (BaseViewModel.HasConnection())
            {
                game = newGame;
                mode = newMode;
                listIndex = 1;

                LoadingAnimation.IsRunning = true;
                isLoading = true;

                await ChangeTopCountries(true);

                LoadingAnimation.IsRunning = false;
                isLoading = false;
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);
        }

        #endregion
    }
}
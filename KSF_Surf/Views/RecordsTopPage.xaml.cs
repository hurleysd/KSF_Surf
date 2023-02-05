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
    public partial class RecordsTopPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;
        private readonly int CALL_LIMIT = 500;

        // objects used by "SurfTop" call
        private List<SurfTopDatum> surfTopData;
        private int list_index = 1;

        // variables for filters
        private readonly EFilter_Game defaultGame;
        private readonly EFilter_Mode defaultMode;
        private EFilter_Game game;
        private EFilter_Mode mode;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> recordsTopCollectionViewItemsSource { get; } 
                = new ObservableCollection<Tuple<string, string, string>>();

        public RecordsTopPage(EFilter_Game game, EFilter_Mode mode, EFilter_Game defaultGame, EFilter_Mode defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
            RecordsTopCollectionView.ItemsSource = recordsTopCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecordsTop(bool clearPrev)
        {
            var surfTopDatum = await recordsViewModel.GetSurfTop(game, mode, list_index);
            surfTopData = surfTopDatum?.data;
            if (surfTopData is null) return;

            if (clearPrev) recordsTopCollectionViewItemsSource.Clear();
            LayoutSurfTop();
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutSurfTop()
        {
            foreach (SurfTopDatum datum in surfTopData)
            {
                string nameString = list_index + ". " + String_Formatter.toEmoji_Country(datum.country) + " " + datum.name;
                string pointsString = String_Formatter.toString_Points(datum.points);

                recordsTopCollectionViewItemsSource.Add(new Tuple<string, string, string>(nameString, pointsString, datum.steamID));

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
                await ChangeRecordsTop(false);

                LoadingAnimation.IsRunning = false;
                RecordsTopStack.IsVisible = true;
            }
        }

        private async void RecordsTop_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection() || list_index == CALL_LIMIT) return;

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecordsTop(false);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecordsTop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string> selectedPlayer = (Tuple<string, string, string>)RecordsTopCollectionView.SelectedItem;
            RecordsTopCollectionView.SelectedItem = null;

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

                await ChangeRecordsTop(true);

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
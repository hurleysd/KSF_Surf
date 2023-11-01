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
    public partial class RecordsTopPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "SurfTop" call
        private List<SurfTopDatum> surfTopData;
        private int listIndex = 1;

        // variables for filters
        private readonly GameEnum defaultGame;
        private readonly ModeEnum defaultMode;
        private GameEnum game;
        private ModeEnum mode;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> recordsTopCollectionViewItemsSource { get; } 
                = new ObservableCollection<Tuple<string, string, string>>();

        public RecordsTopPage(GameEnum game, ModeEnum mode, GameEnum defaultGame, ModeEnum defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title = "Records [" + EnumToString.NameString(game) + ", " + EnumToString.NameString(mode) + "]";
            RecordsTopCollectionView.ItemsSource = recordsTopCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecordsTop(bool clearPrev)
        {
            var surfTopDatum = await recordsViewModel.GetSurfTop(game, mode, listIndex);
            surfTopData = surfTopDatum?.data;
            if (surfTopData is null) return;

            if (clearPrev) recordsTopCollectionViewItemsSource.Clear();
            LayoutSurfTop();
            Title = "Records [" + EnumToString.NameString(game) + ", " + EnumToString.NameString(mode) + "]";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutSurfTop()
        {
            foreach (SurfTopDatum datum in surfTopData)
            {
                string nameString = listIndex + ". " + StringFormatter.CountryEmoji(datum.country) + " " + datum.name;
                string pointsString = StringFormatter.PointsString(datum.points);

                recordsTopCollectionViewItemsSource.Add(new Tuple<string, string, string>(nameString, pointsString, datum.steamID));

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
                await ChangeRecordsTop(false);

                LoadingAnimation.IsRunning = false;
                RecordsTopStack.IsVisible = true;
            }
        }

        private async void RecordsTop_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((listIndex - 1) % RecordsViewModel.SURF_TOP_QLIMIT) != 0) return; // didn't get full results
            if (listIndex >= RecordsViewModel.SURF_TOP_CLIMIT) return; // at call limit

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

            if (BaseViewModel.HasConnection())
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
            if (hasLoaded && BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsFilterPage(ApplyFilters, game, mode, defaultGame, defaultMode));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
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
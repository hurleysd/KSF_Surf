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
    public partial class PlayerMapsCompletionPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;
        private readonly int CALL_LIMIT = 999;

        // objects used by "(In)Complete Maps" call
        private List<PlayerCompletionRecord> recordsData;
        private int list_index = 1;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly EFilter_PlayerType playerType;
        private readonly string playerValue;
        private readonly EFilter_PlayerCompletionType completionType;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> mapsCompletionCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();

        public PlayerMapsCompletionPage(string title, EFilter_Game game, EFilter_Mode mode,
            EFilter_PlayerCompletionType completionType, EFilter_PlayerType playerType, string playerValue)
        {
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;
            this.completionType = completionType;

            playerViewModel = new PlayerViewModel();

            InitializeComponent();
            Title = title;
            HeaderLabel.Text = EFilter_ToString.toString2(completionType);
            MapsCompletionCollectionView.ItemsSource = mapsCompletionCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeCompletion()
        {
            var completionDatum = await playerViewModel.GetPlayerMapsCompletion(game, mode, completionType, playerType, playerValue, list_index);
            recordsData = completionDatum?.data.records;
            if (recordsData is null) return;

            LayoutRecords();
        }

        // Dispaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            foreach (PlayerCompletionRecord datum in recordsData)
            {
                if (datum.completedZones is null) datum.completedZones = "0";
                string mapCompletionString = datum.mapName + " (" + datum.completedZones + "/" + datum.totalZones + ")";


                EFilter_MapType mapType = (EFilter_MapType)int.Parse(datum.mapType);
                string cptypeString = (mapType == EFilter_MapType.linear) ? "CPs" : "Stages";
                string rrinfoString = datum.cp_count + " " + cptypeString + ", " + datum.b_count + " Bonus";
                if (datum.b_count != "1") rrinfoString += "es";
                string mapSummaryString = "Tier " + datum.tier + " " + EFilter_ToString.toString(mapType) + " - " + rrinfoString;

                mapsCompletionCollectionViewItemsSource.Add(new Tuple<string, string, string>(
                    mapCompletionString, mapSummaryString, datum.mapName));

                list_index++;
            }

            if (list_index == 0) // no (in)complete maps
            {
                MapsCompletionCollectionViewEmptyLabel.Text = "None ! "
                    + ((completionType == EFilter_PlayerCompletionType.complete) ? ":(" :  ":)");
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
                await ChangeCompletion();

                LoadingAnimation.IsRunning = false;
                MapsCompletionStack.IsVisible = true;
            }
        }

        private async void MapsCompletion_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection() || list_index == CALL_LIMIT) return;
            if ((list_index - 1) % 10 != 0) return; // avoid loading more when there weren't enough before

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeCompletion();

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void MapsCompletion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string> selectedMap =
                (Tuple<string, string, string>)MapsCompletionCollectionView.SelectedItem;
            MapsCompletionCollectionView.SelectedItem = null;

            string mapName = selectedMap.Item3;

            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new MapsMapPage(mapName, game));
            }
            else
            {
                await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
            }
        }

        #endregion
    }
}
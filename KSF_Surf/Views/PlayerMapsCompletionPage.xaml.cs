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
    public partial class PlayerMapsCompletionPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "(In)Complete Maps" call
        private List<PlayerMapCompletionDatum> recordsData;
        private int listIndex = 1;

        // variables for filters
        private readonly GameEnum game;
        private readonly ModeEnum mode;
        private readonly PlayerTypeEnum playerType;
        private readonly string playerValue;
        private readonly PlayerCompletionTypeEnum completionType;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> mapsCompletionCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();

        public PlayerMapsCompletionPage(string title, GameEnum game, ModeEnum mode,
            PlayerCompletionTypeEnum completionType, PlayerTypeEnum playerType, string playerValue)
        {
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;
            this.completionType = completionType;

            playerViewModel = new PlayerViewModel();

            InitializeComponent();
            Title = title;
            HeaderLabel.Text = EnumToString.NameString(completionType);
            MapsCompletionCollectionView.ItemsSource = mapsCompletionCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeCompletion()
        {
            var completionDatum = await playerViewModel.GetPlayerMapsCompletion(game, mode, completionType, playerType, playerValue, listIndex);
            recordsData = completionDatum?.data.records;
            if (recordsData is null) return;

            LayoutRecords();
        }

        // Dispaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            foreach (PlayerMapCompletionDatum datum in recordsData)
            {
                if (datum.completedZones is null) datum.completedZones = "0";
                string mapCompletionString = datum.mapName + " (" + datum.completedZones + "/" + datum.totalZones + ")";

                MapTypeEnum mapType = (MapTypeEnum)int.Parse(datum.mapType);
                string cptypeString = "";
                string rrinfoString = "";

                // Sam / Untouch adjusted CP count display to not include the end zone
                if (mapType == MapTypeEnum.LINEAR)
                {
                    cptypeString = "CPs";

                    try
                    {
                        int adjustedCPCount = Int32.Parse(datum.cp_count) - 1;
                        rrinfoString = adjustedCPCount.ToString();
                    }
                    catch (FormatException)
                    {
                        rrinfoString = datum.cp_count;
                    }

                    rrinfoString += " " + cptypeString + ", " + datum.b_count + " Bonus";
                }
                else if (mapType == MapTypeEnum.STAGED)
                {
                    cptypeString = "Stages";
                    rrinfoString = datum.cp_count + " " + cptypeString + ", " + datum.b_count + " Bonus";
                }

                if (datum.b_count != "1") rrinfoString += "es";
                string mapSummaryString = "Tier " + datum.tier + " " + EnumToString.NameString(mapType) + " - " + rrinfoString;

                mapsCompletionCollectionViewItemsSource.Add(new Tuple<string, string, string>(
                    mapCompletionString, mapSummaryString, datum.mapName));

                listIndex++;
            }

            // no (in)complete maps
            if (listIndex == 1) MapsCompletionCollectionViewEmptyLabel.Text = "No records found";
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
            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((listIndex - 1) % PlayerViewModel.MAPS_COMPLETION_QLIMIT) != 0) return; // didn't get full results
            if (listIndex >= PlayerViewModel.MAPS_COMPLETION_CLIMIT) return; // at call limit

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

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new MapsMapPage(mapName, game));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);
        }

        #endregion
    }
}
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
    public partial class PlayerWorldRecordsPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "World Records" call
        private List<PlayerWorldRecordDatum> worldRecordsData;
        private int listIndex = 1;

        // variables for filters
        private readonly GameEnum game;
        private readonly ModeEnum mode;
        private readonly PlayerTypeEnum playerType;
        private readonly string playerValue;
        private PlayerWorldRecordsTypeEnum wrsType;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> worldRecordsCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();

        public PlayerWorldRecordsPage(string title, GameEnum game, ModeEnum mode, 
            PlayerTypeEnum playerType, string playerValue, PlayerWorldRecordsTypeEnum wrsType)
        {
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;
            this.wrsType = wrsType;

            playerViewModel = new PlayerViewModel();

            InitializeComponent();
            Title = title;
            WorldRecordsCollectionView.ItemsSource = worldRecordsCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords(bool clearPrev)
        {
            var worldRecordsDatum = await playerViewModel.GetPlayerWRs(game, mode, wrsType, playerType, playerValue, listIndex);
            worldRecordsData = worldRecordsDatum?.data.records;
            if (worldRecordsData is null) return;

            if (clearPrev) worldRecordsCollectionViewItemsSource.Clear();
            LayoutRecords();
            WRTypeOptionButton.Text = EnumToString.NameString(wrsType);
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            foreach (PlayerWorldRecordDatum datum in worldRecordsData)
            {
                string mapZoneString = datum.mapName;
                if (wrsType != PlayerWorldRecordsTypeEnum.WR) mapZoneString += " " + StringFormatter.ZoneString(datum.zoneID, false, false);

                string rrtimeString = "in " + StringFormatter.RankTimeString(datum.surfTime) + " (";
                if (datum.r2Diff is null) rrtimeString += "WR N/A";
                else rrtimeString += "WR-" + StringFormatter.RankTimeString(datum.r2Diff.Substring(1));
                rrtimeString += ") (" + StringFormatter.LastOnlineString(datum.date) + ")";

                worldRecordsCollectionViewItemsSource.Add(new Tuple<string, string, string>(
                    mapZoneString, rrtimeString, datum.mapName));

                listIndex++;
            }

            // no world records
            if (listIndex == 1) WorldRecordsCollectionViewEmptyLabel.Text = "No records found";
        }

        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;
                await ChangeRecords(false);

                LoadingAnimation.IsRunning = false;
                WorldRecordsStack.IsVisible = true;
            }
        }

        private async void WRTypeOptionButton_Clicked(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EnumToString.NameString(wrsType);
            foreach (string type in EnumToString.WorldRecordsTypeNames)
            {
                if (type != currentTypeString) types.Add(type);
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());
            switch (newTypeString)
            {
                case "WRCP": wrsType = PlayerWorldRecordsTypeEnum.WRCP; break;
                case "WRB": wrsType = PlayerWorldRecordsTypeEnum.WRB; break;
                case "WR": wrsType = PlayerWorldRecordsTypeEnum.WR; break;
                default: return;
            }

            listIndex = 1;

            LoadingAnimation.IsRunning = true;
            isLoading = true;

            await ChangeRecords(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void WorldRecords_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((listIndex - 1) % PlayerViewModel.WORLD_RECORDS_QLIMIT) != 0) return; // didn't get full results
            if (listIndex >= PlayerViewModel.WORLD_RECORDS_CLIMIT) return; // at call limit

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecords(false);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void WorldRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string> selectedMap =
                (Tuple<string, string, string>)WorldRecordsCollectionView.SelectedItem;
            WorldRecordsCollectionView.SelectedItem = null;

            string mapName = selectedMap.Item3;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new MapsMapPage(mapName, game));
            }
            else await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }

        #endregion
    }
}
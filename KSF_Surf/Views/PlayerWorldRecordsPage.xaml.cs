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
    public partial class PlayerWorldRecordsPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;
        private readonly int CALL_LIMIT = 200;

        // objects used by "World Records" call
        private List<PlayerWorldRecords> worldRecordsData;
        private int list_index = 1;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly EFilter_PlayerType playerType;
        private readonly string playerValue;
        private EFilter_PlayerWRsType wrsType;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> worldRecordsCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();

        public PlayerWorldRecordsPage(string title, EFilter_Game game, EFilter_Mode mode, 
            EFilter_PlayerType playerType, string playerValue, EFilter_PlayerWRsType wrsType)
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
            var worldRecordsDatum = await playerViewModel.GetPlayerWRs(game, mode, wrsType, playerType, playerValue, list_index);
            worldRecordsData = worldRecordsDatum?.data.records;
            if (worldRecordsData is null) return;

            if (clearPrev) worldRecordsCollectionViewItemsSource.Clear();
            LayoutRecords();
            WRTypeOptionLabel.Text = "Type: " + EFilter_ToString.toString2(wrsType);
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            foreach (PlayerWorldRecords datum in worldRecordsData)
            {
                string mapZoneString = datum.mapName;
                if (wrsType != EFilter_PlayerWRsType.wr)
                {
                    mapZoneString += " " + EFilter_ToString.zoneFormatter(datum.zoneID, false, false);
                }

                string rrtimeString = "in " + String_Formatter.toString_RankTime(datum.surfTime) + " (";
                if (datum.r2Diff is null)
                {
                    rrtimeString += "WR N/A";
                }
                else
                {
                    rrtimeString += "WR-" + String_Formatter.toString_RankTime(datum.r2Diff.Substring(1));
                }
                rrtimeString += ") (" + String_Formatter.toString_LastOnline(datum.date) + ")";

                worldRecordsCollectionViewItemsSource.Add(new Tuple<string, string, string>(
                    mapZoneString, rrtimeString, datum.mapName));

                list_index++;
            }

            if (list_index == 1) // no world records
            {
                WorldRecordsCollectionViewEmptyLabel.Text = "None! :(";
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
                await ChangeRecords(false);

                LoadingAnimation.IsRunning = false;
                WorldRecordsStack.IsVisible = true;
            }
        }

        private async void WRTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EFilter_ToString.toString2(wrsType);
            foreach (string type in EFilter_ToString.wrtype_arr2)
            {
                if (type != currentTypeString)
                {
                    types.Add(type);
                }
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());
            switch (newTypeString)
            {
                case "WRCP": wrsType = EFilter_PlayerWRsType.wrcp; break;
                case "WRB": wrsType = EFilter_PlayerWRsType.wrb; break;
                case "WR": wrsType = EFilter_PlayerWRsType.wr; break;
                default: return;
            }

            list_index = 1;

            LoadingAnimation.IsRunning = true;
            isLoading = true;

            await ChangeRecords(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void WorldRecords_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection() || list_index == CALL_LIMIT) return;
            if ((list_index - 1) % 10 != 0) return; // avoid loading more when there weren't enough before

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
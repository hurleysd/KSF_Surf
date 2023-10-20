using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.ViewModels;
using KSF_Surf.Models;
using System.Collections.ObjectModel;
using System;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class PlayerRecentSetRecordsPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // object used by "Records Set" calls
        private List<RecentPlayerRecords> recordsSetData;
        private int list_index = 1;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly EFilter_PlayerType playerType;
        private readonly string playerValue;

        // collection view
        public ObservableCollection<Tuple<string, string, string, string>> recentRecordsSetCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string, string>>();

        public PlayerRecentSetRecordsPage(string title, EFilter_Game game, EFilter_Mode mode, EFilter_PlayerType playerType, string playerValue)
        {
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;

            playerViewModel = new PlayerViewModel();

            InitializeComponent();
            Title = title;
            RecentSetRecordsCollectionView.ItemsSource = recentRecordsSetCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords()
        {
            var recordsSetDatum = await playerViewModel.GetPlayerRecords(game, mode, EFilter_PlayerRecordsType.set, playerType, playerValue, list_index);
            recordsSetData = recordsSetDatum?.data.recentRecords;
            if (recordsSetData is null) return;

            LayoutRecords();
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            foreach (RecentPlayerRecords datum in recordsSetData)
            {
                string mapZoneString = datum.mapName + " ";
                if (datum.zoneID != "0")
                {
                    mapZoneString += EFilter_ToString.zoneFormatter(datum.zoneID, false, false) + " ";
                }

                string recordString = datum.recordType + " set on " + datum.server + " server";

                string rrtimeString = "[R" + datum.newRank + "] in " + String_Formatter.toString_RankTime(datum.surfTime) + " (";
                if (datum.wrDiff != "0")
                {
                    if (datum.newRank == "1")
                    {
                        rrtimeString += "now ";
                    }
                    rrtimeString += "WR+" + String_Formatter.toString_RankTime(datum.wrDiff) + ") (";
                }
                rrtimeString += String_Formatter.toString_LastOnline(datum.date) + ")";

                recentRecordsSetCollectionViewItemsSource.Add(new Tuple<string, string, string, string>(
                    mapZoneString, recordString, rrtimeString, datum.mapName));

                list_index++;
            }


            if (list_index == 1) // no recently set records
            {
                RecentSetRecordsCollectionViewEmptyLabel.Text = "None! :(";
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
                await ChangeRecords();

                LoadingAnimation.IsRunning = false;
                RecentSetRecordsStack.IsVisible = true;
            }
        }

        private async void RecentSetRecords_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection()) return;
            if (((list_index - 1) % PlayerViewModel.SETBROKEN_RECORDS_QLIMIT) != 0) return; // didn't get full results
            if (list_index >= PlayerViewModel.SETBROKEN_RECORDS_CLIMIT) return; // at call limit

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecords();

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecentSetRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string, string> selectedMap =
                (Tuple<string, string, string, string>)RecentSetRecordsCollectionView.SelectedItem;
            RecentSetRecordsCollectionView.SelectedItem = null;

            string mapName = selectedMap.Item4;

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
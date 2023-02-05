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
    public partial class PlayerRecentBrokenRecordsPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;
        private readonly int CALL_LIMIT = 50;

        // object used by "Records Broken" calls
        private List<RecentPlayerRecords> recordsBrokenData;
        private int list_index = 1;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly EFilter_PlayerType playerType;
        private readonly string playerValue;

        // collection view
        public ObservableCollection<Tuple<string, string, string, string>> recentRecordsBrokenCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string, string>>();

        public PlayerRecentBrokenRecordsPage(string title, EFilter_Game game, EFilter_Mode mode, EFilter_PlayerType playerType, string playerValue)
        {
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;

            playerViewModel = new PlayerViewModel();

            InitializeComponent();
            Title = title;
            RecentBrokenRecordsCollectionView.ItemsSource = recentRecordsBrokenCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords()
        {
            var recordsBrokenDatum = await playerViewModel.GetPlayerRecords(game, mode, EFilter_PlayerRecordsType.broken, playerType, playerValue, list_index);
            recordsBrokenData = recordsBrokenDatum?.data.recentRecords;
            if (recordsBrokenData is null) return;

            LayoutRecords();
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            foreach (RecentPlayerRecords datum in recordsBrokenData)
            {
                string mapZoneString = datum.mapName + " ";
                if (datum.zoneID != "0")
                {
                    mapZoneString += EFilter_ToString.zoneFormatter(datum.zoneID, false, false) + " ";
                }


                if (datum.recordType.Contains("Top10"))
                {
                    if (datum.prevRank != "10")
                    {
                        datum.recordType = "[R" + datum.prevRank + "]";
                    }
                    else
                    {
                        datum.recordType = "Top10";
                    }
                }
                string recordString = datum.recordType + " lost on " + datum.server + " server";

                string rrtimeString = "now [R" + datum.newRank + "] (";
                if (datum.wrDiff == "0")
                {
                    rrtimeString += "RETAKEN";
                }
                else
                {
                    rrtimeString += "WR+" + String_Formatter.toString_RankTime(datum.wrDiff);
                }
                rrtimeString += ") (" + String_Formatter.toString_LastOnline(datum.date) + ")";

                recentRecordsBrokenCollectionViewItemsSource.Add(new Tuple<string, string, string, string>(
                    mapZoneString, recordString, rrtimeString, datum.mapName));

                list_index++;
            }
                

            if (list_index == 1) // no recently broken records
            {
                RecentBrokenRecordsCollectionViewEmptyLabel.Text = "None! :)";
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
                RecentBrokenRecordsStack.IsVisible = true;
            }
        }

        private async void RecentBrokenRecords_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection() || list_index == CALL_LIMIT) return;
            if ((list_index - 1) % 10 != 0) return; // avoid loading more when there weren't enough before

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecords();

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecentBrokenRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string, string> selectedMap =
                (Tuple<string, string, string, string>)RecentBrokenRecordsCollectionView.SelectedItem;
            RecentBrokenRecordsCollectionView.SelectedItem = null;

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
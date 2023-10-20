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
    public partial class RecordsRecentPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "Recent" call
        private List<RRDatum> recentRecordsData;
        private List<RR10Datum> recentRecords10Data;
        private int list_index = 1;

        // variables for filters
        private readonly EFilter_Game defaultGame;
        private readonly EFilter_Mode defaultMode;
        private EFilter_Game game;
        private EFilter_Mode mode;
        private EFilter_RRType recentRecordsType;

        // collection view
        public ObservableCollection<Tuple<string, string, string, string>> RecordsRecentCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string, string>>();

        public RecordsRecentPage(EFilter_Game game, EFilter_Mode mode, EFilter_Game defaultGame, EFilter_Mode defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;
            recentRecordsType = EFilter_RRType.map;

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title =  "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
            RecordsRecentCollectionView.ItemsSource = RecordsRecentCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecentRecords(bool clearPrev)
        {
            if (recentRecordsType == EFilter_RRType.top)
            {
                var recentRecords10Datum = await recordsViewModel.GetRecentRecords10(game, mode, list_index);
                recentRecords10Data = recentRecords10Datum?.data;
                if (recentRecords10Data is null) return;

                if (clearPrev) RecordsRecentCollectionViewItemsSource.Clear();
                RRTypeOptionLabel.Text = "Type: Top10";
                LayoutRecentRecords10();
            }
            else
            {
                var recentRecordsDatum = await recordsViewModel.GetRecentRecords(game, recentRecordsType, mode, list_index);
                recentRecordsData = recentRecordsDatum?.data;
                if (recentRecordsData is null) return;

                if (clearPrev) RecordsRecentCollectionViewItemsSource.Clear();
                RRTypeOptionLabel.Text = "Type: " + EFilter_ToString.toString2(recentRecordsType);
                LayoutRecentRecords(EFilter_ToString.toString2(recentRecordsType));
            }

            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutRecentRecords(string typeString)
        {
            foreach (RRDatum datum in recentRecordsData)
            {
                string mapZoneString = datum.mapName + " " + EFilter_ToString.zoneFormatter(datum.zoneID, false, false);
                string playerServerString = String_Formatter.toEmoji_Country(datum.country) + " " + datum.playerName + " on " + datum.server + " server";

                string rrtimeString = "in " + String_Formatter.toString_RankTime(datum.surfTime) + " (";
                if (datum.wrDiff == "0")
                {
                    if (datum.r2Diff is null)
                    {
                        rrtimeString += "WR N/A";
                    }
                    else
                    {
                        rrtimeString += "WR-" + String_Formatter.toString_RankTime(datum.r2Diff.Substring(1));
                    }
                }
                else
                {
                    rrtimeString += "now WR+" + String_Formatter.toString_RankTime(datum.wrDiff);
                }
                rrtimeString += ") (" + String_Formatter.toString_LastOnline(datum.date) + ")";

                RecordsRecentCollectionViewItemsSource.Add(new Tuple<string, string, string, string>(
                    mapZoneString, playerServerString, rrtimeString, datum.mapName));

                list_index++;
            }
        }

        private void LayoutRecentRecords10()
        {
            foreach (RR10Datum datum in recentRecords10Data)
            {
                int rank = int.Parse(datum.newRank);
                string mapZoneString = datum.mapName + " [R" + rank + "]";
                string playerServerString = String_Formatter.toEmoji_Country(datum.country) + " " + datum.playerName + " on " + datum.server + " server";

                string rrtimeString = "in " + String_Formatter.toString_RankTime(datum.surfTime) + " (";
                if (datum.wrDiff == "0")
                {
                    rrtimeString += "WR";
                }
                else
                {
                    rrtimeString += "WR+" + String_Formatter.toString_RankTime(datum.wrDiff);
                }
                rrtimeString += ") (" + String_Formatter.toString_LastOnline(datum.date) + ")";

                RecordsRecentCollectionViewItemsSource.Add(new Tuple<string, string, string, string>(
                   mapZoneString, playerServerString, rrtimeString, datum.mapName));

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
                await ChangeRecentRecords(false);

                LoadingAnimation.IsRunning = false;
                RecordsRecentStack.IsVisible = true;
            }
        }

        private async void RRTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EFilter_ToString.toString2(recentRecordsType);
            foreach (string type in EFilter_ToString.rrtype_arr)
            {
                if (type != currentTypeString)
                {
                    types.Add(type);
                }
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());
            switch (newTypeString)
            {
                case "Top10": recentRecordsType = EFilter_RRType.top; break;
                case "Stage": recentRecordsType = EFilter_RRType.stage; break;
                case "Bonus": recentRecordsType = EFilter_RRType.bonus; break;
                case "All WRs": recentRecordsType = EFilter_RRType.all; break;
                case "Map": recentRecordsType = EFilter_RRType.map; break;
                default: return;
            }

            list_index = 1;

            LoadingAnimation.IsRunning = true;
            isLoading = true;

            await ChangeRecentRecords(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecordsRecent_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection()) return;
            if (((list_index - 1) % RecordsViewModel.RECENT_RECORDS_QLIMIT) != 0) return; // didn't get full results
            if (list_index >= RecordsViewModel.RECENT_RECORDS_CLIMIT) return; // at call limit

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecentRecords(false);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecordsRecent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string, string> selectedMap =
                (Tuple<string, string, string, string>)RecordsRecentCollectionView.SelectedItem;
            RecordsRecentCollectionView.SelectedItem = null;

            string mapName = selectedMap.Item4;

            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new MapsMapPage(mapName, game));
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

                isLoading = true;
                LoadingAnimation.IsRunning = true;

                await ChangeRecentRecords(true);

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
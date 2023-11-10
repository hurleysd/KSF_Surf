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
    public partial class RecordsRecentPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "Recent" call
        private List<RecentRecordDatum> recentRecordsData;
        private List<RecentRecord10Datum> recentRecords10Data;
        private int listIndex = 1;

        // variables for filters
        private readonly GameEnum defaultGame;
        private readonly ModeEnum defaultMode;
        private GameEnum game;
        private ModeEnum mode;
        private RecentRecordsTypeEnum recentRecordsType;

        // collection view
        public ObservableCollection<Tuple<string, string, string, string>> recordsRecentCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string, string>>();

        public RecordsRecentPage(GameEnum game, ModeEnum mode, GameEnum defaultGame, ModeEnum defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;
            recentRecordsType = RecentRecordsTypeEnum.MAP;

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title = "[" + EnumToString.NameString(game) + "," + EnumToString.NameString(mode) + "] Records";
            RecordsRecentCollectionView.ItemsSource = recordsRecentCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecentRecords(bool clearPrev)
        {
            if (recentRecordsType == RecentRecordsTypeEnum.TOP)
            {
                var recentRecords10Datum = await recordsViewModel.GetRecentRecords10(game, mode, listIndex);
                recentRecords10Data = recentRecords10Datum?.data;
                if (recentRecords10Data is null) return;

                if (clearPrev) recordsRecentCollectionViewItemsSource.Clear();
                RRTypeOptionButton.Text = EnumToString.NameString(recentRecordsType);
                LayoutRecentRecords10();
            }
            else
            {
                var recentRecordsDatum = await recordsViewModel.GetRecentRecords(game, recentRecordsType, mode, listIndex);
                recentRecordsData = recentRecordsDatum?.data;
                if (recentRecordsData is null) return;

                if (clearPrev) recordsRecentCollectionViewItemsSource.Clear();
                RRTypeOptionButton.Text = EnumToString.NameString(recentRecordsType);
                LayoutRecentRecords();
            }

            Title = "[" + EnumToString.NameString(game) + "," + EnumToString.NameString(mode) + "] Records";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutRecentRecords()
        {
            foreach (RecentRecordDatum datum in recentRecordsData)
            {
                string mapZoneString = datum.mapName + " " + StringFormatter.ZoneString(datum.zoneID, false, false);
                string playerServerString = StringFormatter.CountryEmoji(datum.country) + " " + datum.playerName + " on " + datum.server + " server";

                string rrtimeString = "in " + StringFormatter.RankTimeString(datum.surfTime) + " (";
                if (datum.wrDiff == "0")
                {
                    if (datum.r2Diff is null) rrtimeString += "WR N/A";
                    else rrtimeString += "WR-" + StringFormatter.RankTimeString(datum.r2Diff.Substring(1));
                }
                else rrtimeString += "now WR+" + StringFormatter.RankTimeString(datum.wrDiff);
                rrtimeString += ") (" + StringFormatter.LastOnlineString(datum.date) + ")";

                recordsRecentCollectionViewItemsSource.Add(new Tuple<string, string, string, string>(
                    mapZoneString, playerServerString, rrtimeString, datum.mapName));

                listIndex++;
            }
        }

        private void LayoutRecentRecords10()
        {
            foreach (RecentRecord10Datum datum in recentRecords10Data)
            {
                int rank = int.Parse(datum.newRank);
                string mapZoneString = datum.mapName + " [R" + rank + "]";
                string playerServerString = StringFormatter.CountryEmoji(datum.country) + " " + datum.playerName + " on " + datum.server + " server";

                string rrtimeString = "in " + StringFormatter.RankTimeString(datum.surfTime) + " (";
                if (datum.wrDiff == "0") rrtimeString += "WR";
                else rrtimeString += "WR+" + StringFormatter.RankTimeString(datum.wrDiff);
                rrtimeString += ") (" + StringFormatter.LastOnlineString(datum.date) + ")";

                recordsRecentCollectionViewItemsSource.Add(new Tuple<string, string, string, string>(
                   mapZoneString, playerServerString, rrtimeString, datum.mapName));

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
                await ChangeRecentRecords(false);

                LoadingAnimation.IsRunning = false;
                RecordsRecentStack.IsVisible = true;
            }
        }

        private async void RRTypeOptionButton_Clicked(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EnumToString.NameString(recentRecordsType);
            foreach (string type in EnumToString.RecentRecordsTypeNames)
            {
                if (type != currentTypeString) types.Add(type);
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());
            switch (newTypeString)
            {
                case "Top10": recentRecordsType = RecentRecordsTypeEnum.TOP; break;
                case "Stage": recentRecordsType = RecentRecordsTypeEnum.STAGE; break;
                case "Bonus": recentRecordsType = RecentRecordsTypeEnum.BONUS; break;
                case "All WRs": recentRecordsType = RecentRecordsTypeEnum.ALL; break;
                case "Map": recentRecordsType = RecentRecordsTypeEnum.MAP; break;
                default: return;
            }

            listIndex = 1;

            LoadingAnimation.IsRunning = true;
            isLoading = true;

            await ChangeRecentRecords(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecordsRecent_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((listIndex - 1) % RecordsViewModel.RECENT_RECORDS_QLIMIT) != 0) return; // didn't get full results
            if (listIndex >= RecordsViewModel.RECENT_RECORDS_CLIMIT) return; // at call limit

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

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new MapsMapPage(mapName, game));
            }
            else await DisplayNoConnectionAlert();
        }

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (hasLoaded && BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsFilterPage(ApplyFilters, game, mode, defaultGame, defaultMode));
            }
            else await DisplayNoConnectionAlert();
        }

        internal async void ApplyFilters(GameEnum newGame, ModeEnum newMode)
        {
            if (newGame == game && newMode == mode) return;
            if (BaseViewModel.HasConnection())
            {
                game = newGame;
                mode = newMode;
                listIndex = 1;

                isLoading = true;
                LoadingAnimation.IsRunning = true;

                await ChangeRecentRecords(true);

                LoadingAnimation.IsRunning = false;
                isLoading = false;
            }
            else await DisplayNoConnectionAlert();
        }

        private async Task DisplayNoConnectionAlert()
        {
            await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }

        #endregion
    }
}
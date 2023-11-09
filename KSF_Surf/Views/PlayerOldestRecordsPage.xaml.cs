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
    public partial class PlayerOldestRecordsPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "Oldest Records" call
        private List<PlayerOldestRecordDatum> oldRecordData;
        private List<string> oldRecordsOptionStrings;
        private int listIndex = 1;

        // variables for filters
        private readonly GameEnum game;
        private readonly ModeEnum mode;
        private readonly PlayerTypeEnum playerType;
        private readonly string playerValue;
        private PlayerWorldRecordsTypeEnum wrsType;
        private bool hasTop;
        private PlayerOldestRecordsTypeEnum oldestType;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> oldestRecordsCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();

        public PlayerOldestRecordsPage(string title, GameEnum game, ModeEnum mode, 
            PlayerTypeEnum playerType, string playerValue, PlayerWorldRecordsTypeEnum wrsType, bool hasTop)
        {
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;
            this.wrsType = wrsType;
            this.hasTop = hasTop;

            playerViewModel = new PlayerViewModel();
            oldRecordsOptionStrings = new List<string>(EnumToString.PlayerOldestRecordsTypeNames);

            InitializeComponent();
            Title = title;
            OldestRecordsCollectionView.ItemsSource = oldestRecordsCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords(bool clearPrev)
        {
            var oldRecordDatum = await playerViewModel.GetPlayerOldestRecords(game, mode, oldestType, playerType, playerValue, listIndex);
            oldRecordData = oldRecordDatum?.data.records;
            if (oldRecordData is null) return;

            if (clearPrev) oldestRecordsCollectionViewItemsSource.Clear();
            LayoutRecords();
            ORTypeOptionButton.Text = EnumToString.NameString(oldestType);
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            foreach (PlayerOldestRecordDatum datum in oldRecordData)
            {
                string mapZoneString = datum.mapName;
                if (datum.zoneID != null) mapZoneString += " " + StringFormatter.ZoneString(datum.zoneID, false, false);

                string rrtimeString = "";
                string rrdiffString = "";
                if (oldestType == PlayerOldestRecordsTypeEnum.TOP10)
                {
                    rrtimeString += "[R" + datum.rank + "] ";

                    if (datum.wrDiff == "0") rrdiffString += " (WR)";
                    else rrdiffString += " (WR+" + StringFormatter.RankTimeString(datum.wrDiff) + ")";
                }
                else if (oldestType == PlayerOldestRecordsTypeEnum.MAP)
                {
                    if (datum.top10Group != "0") rrtimeString += "[G" + datum.top10Group.Substring(1) + "] ";
                }
                else if (oldestType == PlayerOldestRecordsTypeEnum.WR
                    || oldestType == PlayerOldestRecordsTypeEnum.WRCP
                    || oldestType == PlayerOldestRecordsTypeEnum.WRB)
                {
                    if (!(datum.r2Diff is null))
                    {
                        if (datum.r2Diff != "0") rrdiffString += " (WR-" + StringFormatter.RankTimeString(datum.r2Diff.Substring(1)) + ")";
                        else rrdiffString += " (RETAKEN)";
                    }
                    else rrdiffString += " (WR N/A)";
                }

                rrtimeString += "in " + StringFormatter.RankTimeString(datum.surfTime) + rrdiffString;
                rrtimeString += " (" + StringFormatter.LastOnlineString(datum.date) + ")";

                oldestRecordsCollectionViewItemsSource.Add(new Tuple<string, string, string>(
                    mapZoneString, rrtimeString, datum.mapName));

                listIndex++;
            }

            // no records
            if (listIndex == 1) OldestRecordsCollectionViewEmptyLabel.Text = "No records found";
        }


        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;

                oldestType = PlayerOldestRecordsTypeEnum.MAP;
                switch (wrsType)
                {
                    case PlayerWorldRecordsTypeEnum.WR: oldestType = PlayerOldestRecordsTypeEnum.WR; break;
                    case PlayerWorldRecordsTypeEnum.WRCP: oldestType = PlayerOldestRecordsTypeEnum.WRCP;break;
                    case PlayerWorldRecordsTypeEnum.WRB: oldestType = PlayerOldestRecordsTypeEnum.WRB; break;
                    default: 
                        {
                            oldRecordsOptionStrings.RemoveRange(0, 3);
                            if (hasTop) oldestType = PlayerOldestRecordsTypeEnum.TOP10;
                            else oldRecordsOptionStrings.Remove("Top10");
                            break; 
                        }
                }
                await ChangeRecords(false);

                LoadingAnimation.IsRunning = false;
                OldestRecordsStack.IsVisible = true;
            }
        }

        private async void ORTypeOptionButton_Clicked(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EnumToString.NameString(oldestType);
            foreach (string type in oldRecordsOptionStrings)
            {
                if (type != currentTypeString) types.Add(type);
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());
            switch (newTypeString)
            {
                case "Stage": oldestType = PlayerOldestRecordsTypeEnum.STAGE; break;
                case "Bonus": oldestType = PlayerOldestRecordsTypeEnum.BONUS; break;
                case "WR": oldestType = PlayerOldestRecordsTypeEnum.WR; break;
                case "WRCP": oldestType = PlayerOldestRecordsTypeEnum.WRCP; break;
                case "WRB": oldestType = PlayerOldestRecordsTypeEnum.WRB; break;
                case "Top10": oldestType = PlayerOldestRecordsTypeEnum.TOP10; break;
                case "Map": oldestType = PlayerOldestRecordsTypeEnum.MAP; break;
                default: return;
            }


            listIndex = 1;

            LoadingAnimation.IsRunning = true;
            isLoading = true;

            await ChangeRecords(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void OldestRecords_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((listIndex - 1) % PlayerViewModel.OLDEST_RECORDS_QLIMIT) != 0) return; // didn't get full results
            if (listIndex >= PlayerViewModel.OLDEST_RECORDS_CLIMIT) return; // at call limit

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecords(false);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void OldestRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string> selectedMap =
                (Tuple<string, string, string>)OldestRecordsCollectionView.SelectedItem;
            OldestRecordsCollectionView.SelectedItem = null;

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
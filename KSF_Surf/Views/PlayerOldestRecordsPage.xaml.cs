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
    public partial class PlayerOldestRecordsPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "Oldest Records" call
        private List<PlayerOldRecord> oldRecordData;
        private List<string> oldRecordsOptionStrings;
        private int list_index = 1;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly EFilter_PlayerType playerType;
        private readonly string playerValue;
        private EFilter_PlayerWRsType wrsType;
        private bool hasTop;
        private EFilter_PlayerOldestType oldestType;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> oldestRecordsCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();

        public PlayerOldestRecordsPage(string title, EFilter_Game game, EFilter_Mode mode, 
            EFilter_PlayerType playerType, string playerValue, EFilter_PlayerWRsType wrsType, bool hasTop)
        {
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;
            this.wrsType = wrsType;
            this.hasTop = hasTop;

            playerViewModel = new PlayerViewModel();
            oldRecordsOptionStrings = new List<string>(EFilter_ToString.ortype_arr);

            InitializeComponent();
            Title = title;
            OldestRecordsCollectionView.ItemsSource = oldestRecordsCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords(bool clearPrev)
        {
            var oldRecordDatum = await playerViewModel.GetPlayerOldestRecords(game, mode, oldestType, playerType, playerValue, list_index);
            oldRecordData = oldRecordDatum?.data.records;
            if (oldRecordData is null) return;

            
            if (clearPrev) oldestRecordsCollectionViewItemsSource.Clear();
            LayoutRecords();
            ORTypeOptionLabel.Text = "Type: " + EFilter_ToString.toString2(oldestType);
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            foreach (PlayerOldRecord datum in oldRecordData)
            {
                string mapZoneString = datum.mapName;
                if (datum.zoneID != null)
                {
                    mapZoneString += " " + EFilter_ToString.zoneFormatter(datum.zoneID, false, false);
                }

                
                string rrtimeString = "";
                string rrdiffString = "";
                if (oldestType == EFilter_PlayerOldestType.top10)
                {
                    rrtimeString += "[R" + datum.rank + "] ";
                    if (datum.wrdiff == "0")
                    {
                        rrdiffString += " (WR)";
                    }
                    else
                    {
                        rrdiffString += " (WR+" + String_Formatter.toString_RankTime(datum.wrdiff) + ")";
                    }
                }
                else if (oldestType == EFilter_PlayerOldestType.map)
                {
                    if (datum.top10Group != "0") rrtimeString += "[G" + datum.top10Group.Substring(1) + "] ";
                }
                else if (oldestType == EFilter_PlayerOldestType.wr ||
                    oldestType == EFilter_PlayerOldestType.wrcp ||
                    oldestType == EFilter_PlayerOldestType.wrb)
                {
                    if (!(datum.r2Diff is null))
                    {
                        if (datum.r2Diff != "0")
                        {
                            rrdiffString += " (WR-" + String_Formatter.toString_RankTime(datum.r2Diff.Substring(1)) + ")";
                        }
                        else
                        {
                            rrdiffString += " (RETAKEN)";
                        }
                    }
                    else
                    {
                        rrdiffString += " (WR N/A)";
                    }
                }

                rrtimeString += "in " + String_Formatter.toString_RankTime(datum.surfTime) + rrdiffString;
                rrtimeString += " (" + String_Formatter.toString_LastOnline(datum.date) + ")";

                oldestRecordsCollectionViewItemsSource.Add(new Tuple<string, string, string>(
                    mapZoneString, rrtimeString, datum.mapName));

                list_index++;
            }

            if (list_index == 1) // no records
            {
                OldestRecordsCollectionViewEmptyLabel.Text = "None! :(";
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

                oldestType = EFilter_PlayerOldestType.map;
                switch (wrsType)
                {
                    case EFilter_PlayerWRsType.wr: oldestType = EFilter_PlayerOldestType.wr; break;
                    case EFilter_PlayerWRsType.wrcp: oldestType = EFilter_PlayerOldestType.wrcp;break;
                    case EFilter_PlayerWRsType.wrb: oldestType = EFilter_PlayerOldestType.wrb; break;
                    default: 
                        {
                            oldRecordsOptionStrings.RemoveRange(0, 3);
                            if (hasTop)
                            {
                                oldestType = EFilter_PlayerOldestType.top10;
                            }
                            else
                            {
                                oldRecordsOptionStrings.Remove("Top10");
                            }
                            break; 
                        }
                }
                await ChangeRecords(false);

                LoadingAnimation.IsRunning = false;
                OldestRecordsStack.IsVisible = true;
            }
        }

        private async void ORTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EFilter_ToString.toString2(oldestType);
            foreach (string type in oldRecordsOptionStrings)
            {
                if (type != currentTypeString)
                {
                    types.Add(type);
                }
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());
            switch (newTypeString)
            {
                case "Stage": oldestType = EFilter_PlayerOldestType.stage; break;
                case "Bonus": oldestType = EFilter_PlayerOldestType.bonus; break;
                case "WR": oldestType = EFilter_PlayerOldestType.wr; break;
                case "WRCP": oldestType = EFilter_PlayerOldestType.wrcp; break;
                case "WRB": oldestType = EFilter_PlayerOldestType.wrb; break;
                case "Top10": oldestType = EFilter_PlayerOldestType.top10; break;
                case "Map": oldestType = EFilter_PlayerOldestType.map; break;
                default: return;
            }


            list_index = 1;

            LoadingAnimation.IsRunning = true;
            isLoading = true;

            await ChangeRecords(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void OldestRecords_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection()) return;
            if (((list_index - 1) % PlayerViewModel.OLDEST_RECORDS_QLIMIT) != 0) return; // didn't get full results
            if (list_index >= PlayerViewModel.OLDEST_RECORDS_CLIMIT) return; // at call limit

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
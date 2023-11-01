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
    public partial class RecordsOldestPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "Oldest Records" call
        private List<OldestRecordDatum> oldRecordData;
        private int listIndex = 1;

        // variables for filters
        private readonly GameEnum defaultGame;
        private readonly ModeEnum defaultMode;
        private GameEnum game;
        private ModeEnum mode;
        private OldestRecordsTypeEnum oldestType;

        // collection view
        public ObservableCollection<Tuple<string, string, string, string>> recordsOldestCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string, string>>();

        public RecordsOldestPage(GameEnum game, ModeEnum mode, GameEnum defaultGame, ModeEnum defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;
            oldestType = OldestRecordsTypeEnum.MAP;

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title = "Records [" + EnumToString.NameString(game) + ", " + EnumToString.NameString(mode) + "]";
            RecordsOldestCollectionView.ItemsSource = recordsOldestCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords(bool clearPrev)
        {
            var oldRecordDatum = await recordsViewModel.GetOldestRecords(game, oldestType, mode, listIndex);
            oldRecordData = oldRecordDatum?.data;
            if (oldRecordData is null) return;

            if (clearPrev) recordsOldestCollectionViewItemsSource.Clear();
            LayoutRecords();
            ORTypeOptionLabel.Text = "Type: " + EnumToString.NameString(oldestType);
            Title = "Records [" + EnumToString.NameString(game) + ", " + EnumToString.NameString(mode) + "]";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            foreach (OldestRecordDatum datum in oldRecordData)
            {
                string rrString = listIndex + ". " + datum.mapName;
                if (datum.zoneID != null)
                {
                    rrString += " " + StringFormatter.ZoneString(datum.zoneID, false, false);
                }

                string playerString = StringFormatter.CountryEmoji(datum.country) + " " + datum.playerName;

                string rrtimeString = "in " + StringFormatter.RankTimeString(datum.surfTime);
                if (!(datum.r2Diff is null))
                {
                    if (datum.r2Diff != "0") rrtimeString += " (WR-" + StringFormatter.RankTimeString(datum.r2Diff.Substring(1)) + ")";
                    else rrtimeString += " (RETAKEN)";
                }
                else rrtimeString += " (WR N/A)";
                rrtimeString += " (" + StringFormatter.LastOnlineString(datum.date) + ")";

                recordsOldestCollectionViewItemsSource.Add(new Tuple<string, string, string, string>(
                    rrString, playerString, rrtimeString, datum.mapName));
                
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
                await ChangeRecords(false);

                LoadingAnimation.IsRunning = false;
                RecordsOldestStack.IsVisible = true;
            }
        }

        private async void ORTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EnumToString.NameString(oldestType);
            foreach (string type in EnumToString.OldestRecordsTypeNames)
            {
                if (type != currentTypeString)
                {
                    types.Add(type);
                }
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());
            switch (newTypeString)
            {
                case "Stage": oldestType = OldestRecordsTypeEnum.STAGE; break;
                case "Bonus": oldestType = OldestRecordsTypeEnum.BONUS; break;
                case "Map": oldestType = OldestRecordsTypeEnum.MAP; break;
                default: return;
            }

            listIndex = 1;

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecords(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecordsOldest_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((listIndex - 1) % RecordsViewModel.OLDEST_RECORDS_QLIMIT) != 0) return; // didn't get full results
            if (listIndex >= RecordsViewModel.OLDEST_RECORDS_CLIMIT) return; // at call limit

            isLoading = true; 
            LoadingAnimation.IsRunning = true;

            await ChangeRecords(false);
            
            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecordsOldest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string, string> selectedMap = 
                (Tuple<string, string, string, string>)RecordsOldestCollectionView.SelectedItem;
            RecordsOldestCollectionView.SelectedItem = null;

            string mapName = selectedMap.Item4;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new MapsMapPage(mapName, game));
            }
            else await DisplayNoConnectionAlert();
        }

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (hasLoaded)
            {
                await Navigation.PushAsync(new RecordsFilterPage(ApplyFilters, game, mode, defaultGame, defaultMode));
            }
        }

        internal async void ApplyFilters(GameEnum newGame, ModeEnum newMode)
        {
            if (newGame == game && newMode == mode) return;
            if (BaseViewModel.HasConnection())
            {
                game = newGame;
                mode = newMode;
                listIndex = 1;

                LoadingAnimation.IsRunning = true;
                isLoading = true;

                await ChangeRecords(false);

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
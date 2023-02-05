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
    public partial class RecordsOldestPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;
        private readonly int CALL_LIMIT = 250;

        // objects used by "Oldest Records" call
        private List<OldRecord> oldRecordData;
        private int list_index = 1;

        // variables for filters
        private readonly EFilter_Game defaultGame;
        private readonly EFilter_Mode defaultMode;
        private EFilter_Game game;
        private EFilter_Mode mode;
        private EFilter_ORType oldestType;

        // collection view
        public ObservableCollection<Tuple<string, string, string, string>> recordsOldestCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string, string>>();

        public RecordsOldestPage(EFilter_Game game, EFilter_Mode mode, EFilter_Game defaultGame, EFilter_Mode defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;
            oldestType = EFilter_ORType.map;

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
            RecordsOldestCollectionView.ItemsSource = recordsOldestCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords(bool clearPrev)
        {
            var oldRecordDatum = await recordsViewModel.GetOldestRecords(game, oldestType, mode, list_index);
            oldRecordData = oldRecordDatum?.data;
            if (oldRecordData is null) return;

            if (clearPrev) recordsOldestCollectionViewItemsSource.Clear();
            LayoutRecords();
            ORTypeOptionLabel.Text = "Type: " + EFilter_ToString.toString2(oldestType);
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            foreach (OldRecord datum in oldRecordData)
            {
                string rrString = list_index + ". " + datum.mapName;
                if (datum.zoneID != null)
                {
                    rrString += EFilter_ToString.zoneFormatter(datum.zoneID, false, false);
                }

                string playerString = String_Formatter.toEmoji_Country(datum.country) + " " + datum.playerName;

                string rrtimeString = "in " + String_Formatter.toString_RankTime(datum.surfTime);
                if (!(datum.r2Diff is null))
                {
                    if (datum.r2Diff != "0")
                    {
                        rrtimeString += " (WR-" + String_Formatter.toString_RankTime(datum.r2Diff.Substring(1)) + ")";
                    }
                    else
                    {
                        rrtimeString += " (RETAKEN)";
                    }
                }
                else
                {
                    rrtimeString += " (WR N/A)";
                }
                rrtimeString += " (" + String_Formatter.toString_LastOnline(datum.date) + ")";


                recordsOldestCollectionViewItemsSource.Add(new Tuple<string, string, string, string>(
                    rrString, playerString, rrtimeString, datum.mapName));
                
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
                await ChangeRecords(false);

                LoadingAnimation.IsRunning = false;
                RecordsOldestStack.IsVisible = true;
            }
        }

        private async void ORTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EFilter_ToString.toString2(oldestType);
            foreach (string type in EFilter_ToString.otype_arr)
            {
                if (type != currentTypeString)
                {
                    types.Add(type);
                }
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());
            switch (newTypeString)
            {
                case "Stage": oldestType = EFilter_ORType.stage; break;
                case "Bonus": oldestType = EFilter_ORType.bonus; break;
                case "Map": oldestType = EFilter_ORType.map; break;
                default: return;
            }

            list_index = 1;

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecords(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecordsOldest_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection() || list_index == CALL_LIMIT) return;
            
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
            if (hasLoaded)
            {
                await Navigation.PushAsync(new RecordsFilterPage(ApplyFilters, game, mode, defaultGame, defaultMode));
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

                LoadingAnimation.IsRunning = true;
                isLoading = true;

                await ChangeRecords(false);

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
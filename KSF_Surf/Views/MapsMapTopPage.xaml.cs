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
    public partial class MapsMapTopPage : ContentPage
    {
        private readonly MapsViewModel mapsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "SurfTop" call
        private List<TopDatum> topData;
        private int list_index = 1;

        // variables for filters
        private readonly GameEnum game;
        private readonly string map;
        private ModeEnum currentMode;

        private int currentZone;
        private string currentZoneString;

        private List<string> zonePickerList;
        private int stageCount;
        private int bonusCount;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> mapsMapTopCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();

        public MapsMapTopPage(string title, GameEnum game, string map, int stageCount, int bonusCount, List<string> zonePickerList)
        {
            this.game = game;
            this.map = map;
            currentMode = PropertiesDict.GetMode();

            this.stageCount = stageCount;
            this.bonusCount = bonusCount;
            this.zonePickerList = zonePickerList;
            currentZone = 0;
            currentZoneString = "Main";

            mapsViewModel = new MapsViewModel();
            
            InitializeComponent();
            Title = title;
            ZonePicker.ItemsSource = zonePickerList;
            MapsMapTopCollectionView.ItemsSource = mapsMapTopCollectionViewItemsSource;
            StyleOptionLabel.Text = "Style: " + EnumToString.NameString(currentMode);
            ZoneOptionLabel.Text = "Zone: " + currentZoneString;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords(ModeEnum mode, int zone)
        {
            int temp_list_index = list_index;
            if (mode != currentMode || zone != currentZone) temp_list_index = 1;

            var topDatum = await mapsViewModel.GetMapTop(game, map, mode, zone, temp_list_index);
            topData = topDatum?.data;
            if (topData is null)
            {
                await DisplayAlert("No " + EnumToString.NameString(mode) + " zone completions.", "Be the first!", "OK");
                return;
            };

            currentMode = mode;
            currentZone = zone;
            list_index = temp_list_index;

            if (temp_list_index == 1) mapsMapTopCollectionViewItemsSource.Clear();
            LayoutTop();
            StyleOptionLabel.Text = "Style: " + EnumToString.NameString(currentMode);
            ZoneOptionLabel.Text = "Zone: " + currentZoneString;
        }

        // Displaying Changes ---------------------------------------------------------------------------------

        private void LayoutTop()
        {
            foreach (TopDatum datum in topData)
            {
                string playerString = list_index + ". " + StringFormatter.CountryEmoji(datum.country) + " " + datum.name;
                string timeString = "in " + StringFormatter.RankTimeString(datum.time);
                string dateString = " (" + StringFormatter.KSFDateString(datum.date) + ")";

                if (list_index != 1) timeString += " (+" + StringFormatter.RankTimeString(datum.wrDiff) + ")";
                else timeString += " (WR)";

                mapsMapTopCollectionViewItemsSource.Add(new Tuple<string, string, string>(
                    playerString, timeString + dateString, datum.steamID));

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
                await ChangeRecords(currentMode, currentZone);

                LoadingAnimation.IsRunning = false;
                MapsMapTopStack.IsVisible = true;
            }
        }

        private async void StyleOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> modes = new List<string>();
            string currentModeString = EnumToString.NameString(currentMode);
            foreach (string mode in EnumToString.ModeNames)
            {
                if (mode != currentModeString)
                {
                    modes.Add(mode);
                }
            }

            string newStyle = await DisplayActionSheet("Choose a different style", "Cancel", null, modes.ToArray());
            ModeEnum newMode = ModeEnum.NONE;
            switch (newStyle)
            {
                case "HSW": newMode = ModeEnum.HSW; break;
                case "SW": newMode = ModeEnum.SW; break;
                case "BW": newMode = ModeEnum.BW; break;
                case "FW": newMode = ModeEnum.FW; break;
                default: return;
            }

            // don't reset list index yet

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecords(newMode, currentZone);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private void ZoneOptionLabel_Tapped(object sender, EventArgs e)
        {
            ZonePicker.SelectedItem = currentZoneString;
            ZonePicker.Focus();
        }

        private async void ZonePicker_Unfocused(object sender, FocusEventArgs e)
        {
            string selected = (string)ZonePicker.SelectedItem;
            if (selected == currentZoneString)return;

            int newZoneNum = -1;
            switch (selected[0])
            {
                case 'M': newZoneNum = 0; break;
                case 'S': newZoneNum = int.Parse(selected.Substring(1)); break;
                case 'B': newZoneNum = 30 + int.Parse(selected.Substring(1)); break;
            }

            if (newZoneNum != -1)
            {
                // don't reset list index yet

                isLoading = true;
                LoadingAnimation.IsRunning = true;

                await ChangeRecords(currentMode, newZoneNum);

                LoadingAnimation.IsRunning = false;
                isLoading = false;
            }
        }

        private async void MapsMapTop_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((list_index - 1) % MapsViewModel.TOP_QLIMIT) != 0) return; // didn't get full results
            if (list_index >= MapsViewModel.TOP_CLIMIT) return; // at call limit

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecords(currentMode, currentZone);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void MapsMapTop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string> selectedPlayer =
                (Tuple<string, string, string>)MapsMapTopCollectionView.SelectedItem;
            MapsMapTopCollectionView.SelectedItem = null;

            string playerSteamId = selectedPlayer.Item3;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsPlayerPage(game, currentMode, playerSteamId));
            }
            else await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }

        #endregion
    }
}
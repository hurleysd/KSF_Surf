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
        private int listIndex = 1;

        // variables for filters
        private readonly GameEnum game;
        private readonly string map;
        private ModeEnum currentMode;

        private int currentZone;
        private string currentZoneString;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> mapsMapTopCollectionViewItemsSource { get; }
            = new ObservableCollection<Tuple<string, string, string>>();

        public MapsMapTopPage(string title, GameEnum game, string map, int stageCount, int bonusCount, List<string> zonePickerList)
        {
            this.game = game;
            this.map = map;
            currentMode = PropertiesDict.GetMode();

            currentZone = 0;
            currentZoneString = StringFormatter.ZoneString(currentZone.ToString(), true, false); // starting on Main

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

        private async Task ChangeRecords(bool clearPrev)
        {
            if (clearPrev) mapsMapTopCollectionViewItemsSource.Clear();
            StyleOptionLabel.Text = "Style: " + EnumToString.NameString(currentMode);
            ZoneOptionLabel.Text = "Zone: " + currentZoneString;

            var topDatum = await mapsViewModel.GetMapTop(game, map, currentMode, currentZone, listIndex);
            topData = topDatum?.data;
            if (topData is null) return;

            LayoutTop();
        }

        // Displaying Changes ---------------------------------------------------------------------------------

        private void LayoutTop()
        {
            foreach (TopDatum datum in topData)
            {
                string playerString = listIndex + ". " + StringFormatter.CountryEmoji(datum.country) + " " + datum.name;
                string timeString = "in " + StringFormatter.RankTimeString(datum.time);
                string dateString = " (" + StringFormatter.KSFDateString(datum.date) + ")";

                if (listIndex != 1) timeString += " (+" + StringFormatter.RankTimeString(datum.wrDiff) + ")";
                else timeString += " (WR)";

                mapsMapTopCollectionViewItemsSource.Add(new Tuple<string, string, string>(
                    playerString, timeString + dateString, datum.steamID));

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
                MapsMapTopStack.IsVisible = true;
            }
        }

        private async void StyleOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> modes = new List<string>();
            string currentModeString = EnumToString.NameString(currentMode);
            foreach (string mode in EnumToString.ModeNames)
            {
                if (mode != currentModeString) modes.Add(mode);
            }

            string newStyle = await DisplayActionSheet("Choose a different style", "Cancel", null, modes.ToArray());
            switch (newStyle)
            {
                case "HSW": currentMode = ModeEnum.HSW; break;
                case "SW": currentMode = ModeEnum.SW; break;
                case "BW": currentMode = ModeEnum.BW; break;
                case "FW": currentMode = ModeEnum.FW; break;
                default: return;
            }

            listIndex = 1;

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecords(true);

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
            if (selected == currentZoneString) return;

            int newZoneNum = -1;
            switch (selected[0])
            {
                case 'M': newZoneNum = 0; break;
                case 'S': newZoneNum = int.Parse(selected.Substring(1)); break;
                case 'B': newZoneNum = 30 + int.Parse(selected.Substring(1)); break;
            }

            if (newZoneNum != -1)
            {
                currentZone = newZoneNum;
                currentZoneString = selected;
                listIndex = 1;

                isLoading = true;
                LoadingAnimation.IsRunning = true;

                await ChangeRecords(true);

                LoadingAnimation.IsRunning = false;
                isLoading = false;
            }
        }

        private async void MapsMapTop_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((listIndex - 1) % MapsViewModel.TOP_QLIMIT) != 0) return; // didn't get full results
            if (listIndex >= MapsViewModel.TOP_CLIMIT) return; // at call limit

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeRecords(false);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void MapsMapTop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string> selectedPlayer =
                (Tuple<string, string, string>)MapsMapTopCollectionView.SelectedItem;
            MapsMapTopCollectionView.SelectedItem = null;

            string playerSteamID = selectedPlayer.Item3;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsPlayerPage(game, currentMode, playerSteamID));
            }
            else await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
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
        private readonly int LIST_LIMIT = 25;
        private readonly int CALL_LIMIT = 250;

        // objects used by "SurfTop" call
        private List<TopDatum> topData;
        private int list_index = 1;
        private bool moreRecords = false;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly string map;
        private EFilter_Mode currentMode;

        private int currentZone;
        private string currentZoneString;

        private List<string> zonePickerList;
        private int stageCount;
        private int bonusCount;

        public MapsMapTopPage(string title, MapsViewModel mapsViewModel, EFilter_Game game, string map, int stageCount, int bonusCount, List<string> zonePickerList)
        {
            this.mapsViewModel = mapsViewModel;
            this.game = game;
            this.map = map;
            currentMode = BaseViewModel.propertiesDict_getMode();

            this.stageCount = stageCount;
            this.bonusCount = bonusCount;
            this.zonePickerList = zonePickerList;
            currentZone = 0;
            currentZoneString = "Main";
            

            InitializeComponent();
            Title = title;
            ZonePicker.ItemsSource = zonePickerList;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords()
        {
            var topDatum = await mapsViewModel.GetMapTop(game, map, currentMode, currentZone, list_index);
            topData = topDatum?.data;
            if (topData is null)
            {
                StyleOptionLabel.Text = "Style: " + EFilter_ToString.toString(currentMode);
                ZoneOptionLabel.Text = "Zone: " + currentZoneString;
                await DisplayAlert("No " + EFilter_ToString.toString(currentMode) + " Main completions.", "Be the first!", "OK");
                return;
            };

            LayoutTop(EFilter_ToString.toString(currentMode), "Main");
        }

        // Displaying Changes ---------------------------------------------------------------------------------

        private void LayoutTop(string modeString, string zone)
        {
            StyleOptionLabel.Text = "Style: " + modeString;
            ZoneOptionLabel.Text = "Zone: " + zone;

            foreach (TopDatum datum in topData)
            {
                TopRankStack.Children.Add(new Label
                {
                    Text = list_index + ". " + String_Formatter.toEmoji_Country(datum.country) + " " + datum.name + " (" + datum.count + ")",
                    Style = Resources["TopStyle"] as Style
                });

                Label TimeLabel = new Label
                {
                    Text = String_Formatter.toString_RankTime(datum.time),
                    Style = Resources["TopStyle"] as Style
                };
                if (list_index != 1)
                {
                    TimeLabel.Text += " (+" + String_Formatter.toString_RankTime(datum.wrDiff) + ")";
                }
                else
                {
                    TimeLabel.Text += " (WR)";
                }
                TopTimeStack.Children.Add(TimeLabel);

                list_index++;
            }
            
            moreRecords = (((list_index - 1) % LIST_LIMIT == 0) && ((list_index - 1) < CALL_LIMIT));
            MoreFrame.IsVisible = moreRecords;
        }

        private void ClearTopGrid()
        {
            TopRankStack.Children.Clear();
            TopRankStack.Children.Add(TopColLabel1);
            TopTimeStack.Children.Clear();
            TopTimeStack.Children.Add(TopColLabel2);
        }

        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                await ChangeRecords();

                LoadingAnimation.IsRunning = false;
                MapsMapTopScrollView.IsVisible = true;
                hasLoaded = true;
            }
        }

        private async void StyleOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> modes = new List<string>();
            string currentModeString = EFilter_ToString.toString(currentMode);
            foreach (string mode in EFilter_ToString.modes_arr)
            {
                if (mode != currentModeString)
                {
                    modes.Add(mode);
                }
            }

            string newStyle = await DisplayActionSheet("Choose a different style", "Cancel", null, modes.ToArray());

            EFilter_Mode newCurrentMode = EFilter_Mode.fw;
            switch (newStyle)
            {
                case "Cancel": return;
                case "HSW": newCurrentMode = EFilter_Mode.hsw; break;
                case "SW": newCurrentMode = EFilter_Mode.sw; break;
                case "BW": newCurrentMode = EFilter_Mode.bw; break;
            }
            
            LoadingAnimation.IsRunning = true;

            var newTopDatum = await mapsViewModel.GetMapTop(game, map, newCurrentMode, currentZone, 1);
            List<TopDatum> newTopData = newTopDatum?.data;
            if (newTopData is null)
            {
                LoadingAnimation.IsRunning = false;
                await DisplayAlert("No " + newStyle + " " + currentZoneString + " completions.", "Be the first!", "OK");
                return;
            }
            topData = newTopData;
            currentMode = newCurrentMode;

            list_index = 1;
            ClearTopGrid();

            
            LayoutTop(newStyle, currentZoneString);
            LoadingAnimation.IsRunning = false;
        }

        private void ZoneOptionLabel_Tapped(object sender, EventArgs e)
        {
            ZonePicker.SelectedItem = currentZoneString;
            ZonePicker.Focus();
        }

        private async void ZonePicker_Unfocused(object sender, FocusEventArgs e)
        {
            string selected = (string)ZonePicker.SelectedItem;
            if (selected == currentZoneString)
            {
                return;
            }
            LoadingAnimation.IsRunning = true;

            int newZoneNum = -1;
            switch (selected[0])
            {
                case 'M': newZoneNum = 0; break;
                case 'S': newZoneNum = int.Parse(selected.Substring(1)); break;
                case 'B': newZoneNum = 30 + int.Parse(selected.Substring(1)); break;
            }

            if (newZoneNum != -1)
            {
                var newTopDatum = await mapsViewModel.GetMapTop(game, map, currentMode, newZoneNum, 1);
                List<TopDatum> newTopData = newTopDatum?.data;
                if (newTopData is null)
                {
                    LoadingAnimation.IsRunning = false;
                    await DisplayAlert("No " + EFilter_ToString.toString(currentMode) + " " + selected + " completions.", "Be the first!", "OK");
                    return;
                }

                topData = newTopData;
                currentZone = newZoneNum;
                currentZoneString = selected;

                list_index = 1;
                ClearTopGrid();

                LayoutTop(EFilter_ToString.toString(currentMode), currentZoneString);
                LoadingAnimation.IsRunning = false;
            }
        }

        private async void MoreButton_Tapped(object sender, EventArgs e)
        {
            if (!BaseViewModel.hasConnection()) return;

            MoreButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            MoreLabel.IsVisible = false;
            MoreLoadingAnimation.IsRunning = true;

            var topDatum = await mapsViewModel.GetMapTop(game, map, currentMode, currentZone, list_index);
            topData = topDatum?.data;

            MoreButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;

            if (topData is null || topData.Count < 1)
            {
                MoreFrame.IsVisible = false;
                return;
            }

            LayoutTop(EFilter_ToString.toString(currentMode), currentZoneString);
            MoreLoadingAnimation.IsRunning = false;
            MoreLabel.IsVisible = true;
        }

        #endregion
    }
}
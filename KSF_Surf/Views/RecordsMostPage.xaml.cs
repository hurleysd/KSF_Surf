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
    public partial class RecordsMostPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects possibly used by "Most By Type" calls
        private List<MostPCDatum> mostPCData;
        private List<MostCountDatum> mostCountData;
        private List<MostTopDatum> mostTopData;
        private List<MostGroupDatum> mostGroupData;
        private List<MostContWrDatum> mostContWrData;
        private List<MostContZoneDatum> mostContZoneData;
        private List<MostTimeDatum> mostTimeData;
        private int list_index = 1;

        // variables for filters
        private readonly EFilter_Game defaultGame;
        private readonly EFilter_Mode defaultMode;
        private EFilter_Game game;
        private EFilter_Mode mode;
        private EFilter_MostType mostType;
        private string mostTypeString;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> recordsMostCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();


        public RecordsMostPage(EFilter_Game game, EFilter_Mode mode, EFilter_Game defaultGame, EFilter_Mode defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;
            mostType = EFilter_MostType.wr;
            mostTypeString = "Current WRs";

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
            MostTypePicker.ItemsSource = EFilter_ToString.mosttype_arr;
            RecordsMostCollectionView.ItemsSource = recordsMostCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeMostByType(bool clearPrev)
        {
            List<string> players = new List<string>();
            List<string> values = new List<string>();
            List<string> links = new List<string>();

            switch (mostType)
            {
                case EFilter_MostType.pc:
                    {
                        var mostPCDatum = await recordsViewModel.GetMostPC(game, mode, list_index);
                        mostPCData = mostPCDatum?.data;
                        if (mostPCData is null || mostPCData.Count < 1) return;

                        foreach (MostPCDatum datum in mostPCData)
                        {
                            players.Add(String_Formatter.toEmoji_Country(datum.country) + " " + datum.name);
                            values.Add((double.Parse(datum.percentCompletion) * 100).ToString("0.00") + "%");
                            links.Add(datum.steamID);
                        }
                        break;
                    }
                case EFilter_MostType.wr:
                case EFilter_MostType.wrcp:
                case EFilter_MostType.wrb:
                case EFilter_MostType.mostwr:
                case EFilter_MostType.mostwrcp:
                case EFilter_MostType.mostwrb:
                    {
                        var mostCountDatum = await recordsViewModel.GetMostCount(game, mostType, mode, list_index);
                        mostCountData = mostCountDatum?.data;
                        if (mostCountData is null || mostCountData.Count < 1) return;

                        foreach (MostCountDatum datum in mostCountData)
                        {
                            players.Add(String_Formatter.toEmoji_Country(datum.country) + " " + datum.name);
                            values.Add(String_Formatter.toString_Int(datum.total));
                            links.Add(datum.steamID);
                        }
                        break;
                    }
                case EFilter_MostType.top10:
                    {
                        var mostTopDatum = await recordsViewModel.GetMostTop(game, mode, list_index);
                        mostTopData = mostTopDatum?.data;
                        if (mostTopData is null || mostTopData.Count < 1) return;

                        foreach (MostTopDatum datum in mostTopData)
                        {
                            players.Add(String_Formatter.toEmoji_Country(datum.country) + " " + datum.name);
                            values.Add(String_Formatter.toString_Points(datum.top10Points));
                            links.Add(datum.steamID);
                        }
                        break;
                    }
                case EFilter_MostType.group:
                    {
                        var mostGroupDatum = await recordsViewModel.GetMostGroup(game, mode, list_index);
                        mostGroupData = mostGroupDatum?.data;
                        if (mostGroupData is null || mostGroupData.Count < 1) return;

                        foreach (MostGroupDatum datum in mostGroupData)
                        {
                            players.Add(String_Formatter.toEmoji_Country(datum.country) + " " + datum.name);
                            values.Add(String_Formatter.toString_Points(datum.groupPoints));
                            links.Add(datum.steamID);
                        }
                        break;
                    }
                case EFilter_MostType.mostcontestedwr:
                    {
                        var mostContWrDatum = await recordsViewModel.GetMostContWr(game, mode, list_index);
                        mostContWrData = mostContWrDatum?.data;
                        if (mostContWrData is null || mostContWrData.Count < 1) return;

                        foreach (MostContWrDatum datum in mostContWrData)
                        {
                            players.Add(datum.mapName);
                            values.Add(String_Formatter.toString_Int(datum.total));
                            links.Add(datum.mapName);
                        }
                        break;
                    }
                case EFilter_MostType.mostcontestedwrcp:
                case EFilter_MostType.mostcontestedwrb:
                    {
                        var mostContZoneDatum = await recordsViewModel.GetMostContZone(game, mostType, mode, list_index);
                        mostContZoneData = mostContZoneDatum?.data;
                        if (mostContZoneData is null || mostContZoneData.Count < 1) return;

                        foreach (MostContZoneDatum datum in mostContZoneData)
                        {
                            string zoneString = EFilter_ToString.zoneFormatter(datum.zoneID, false, false);
                            players.Add(datum.mapName + " " + zoneString);
                            values.Add(String_Formatter.toString_Int(datum.total));
                            links.Add(datum.mapName);
                        }
                        break;
                    }
                case EFilter_MostType.playtimeday:
                case EFilter_MostType.playtimeweek:
                case EFilter_MostType.playtimemonth:
                    {
                        var mostTimeDatum = await recordsViewModel.GetMostTime(game, mostType, mode, list_index);
                        mostTimeData = mostTimeDatum?.data;
                        if (mostTimeData is null || mostTimeData.Count < 1) return;

                        foreach (MostTimeDatum datum in mostTimeData)
                        {
                            players.Add(datum.mapName);
                            values.Add(String_Formatter.toString_PlayTime(datum.totalplaytime.ToString(), true));
                            links.Add(datum.mapName);
                        }
                        break;
                    }
                default: return;
            }

            if (clearPrev) recordsMostCollectionViewItemsSource.Clear();
            LayoutMostByType(players, values, links);
            MostTypeOptionLabel.Text = "Type: " + EFilter_ToString.toString2(mostType);
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutMostByType(List<string> players, List<string> values, List<string> links)
        {
            for (int i = 0; i < players.Count && i < values.Count && i < links.Count; i++, list_index++)
            {
                recordsMostCollectionViewItemsSource.Add(new Tuple<string, string, string>(list_index + ". " + players[i], values[i], links[i]));
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
                await ChangeMostByType(false);

                LoadingAnimation.IsRunning = false;
                RecordsMostStack.IsVisible = true;
            }
        }

        private void MostTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            MostTypePicker.SelectedItem = mostTypeString;
            MostTypePicker.Focus();
        }

        private async void MostTypePicker_Unfocused(object sender, FocusEventArgs e)
        {
            string selected = (string)MostTypePicker.SelectedItem;
            if (selected == mostTypeString) return;

            switch (selected)
            {
                case "Completion": mostType = EFilter_MostType.pc; break;
                case "Current WRs": mostType = EFilter_MostType.wr; break;
                case "Current WRCPs": mostType = EFilter_MostType.wrcp; break;
                case "Current WRBs": mostType = EFilter_MostType.wrb; break;
                case "Top10 Points": mostType = EFilter_MostType.top10; break;
                case "Group Points": mostType = EFilter_MostType.group; break;
                case "Broken WRs": mostType = EFilter_MostType.mostwr; break;
                case "Broken WRCPs": mostType = EFilter_MostType.mostwrcp; break;
                case "Broken WRBs": mostType = EFilter_MostType.mostwrb; break;
                case "Contested WR": mostType = EFilter_MostType.mostcontestedwr; break;
                case "Contested WRCP": mostType = EFilter_MostType.mostcontestedwrcp; break;
                case "Contested WRB": mostType = EFilter_MostType.mostcontestedwrb; break;
                case "Play Time Day": mostType = EFilter_MostType.playtimeday; break;
                case "Play Time Week": mostType = EFilter_MostType.playtimeweek; break;
                case "Play Time Month": mostType = EFilter_MostType.playtimemonth; break;
                default: return;
            }

            mostTypeString = selected;
            list_index = 1;

            LoadingAnimation.IsRunning = true;
            isLoading = true;

            await ChangeMostByType(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecordsMost_ThresholdReached(object sender, EventArgs e)
        {
            if (mostType == EFilter_MostType.playtimeday
                || mostType == EFilter_MostType.playtimeweek
                || mostType == EFilter_MostType.playtimemonth) return;
            // calling with these ^ types again leads to JSON deserialization errors 

            if (isLoading || !BaseViewModel.hasConnection()) return;
            if (((list_index - 1) % RecordsViewModel.MOST_QLIMIT) != 0) return; // didn't get full results
            if (list_index >= RecordsViewModel.MOST_CLIMIT) return; // at call limit

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeMostByType(false);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecordsMost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string> selection = (Tuple<string, string, string>)RecordsMostCollectionView.SelectedItem;
            RecordsMostCollectionView.SelectedItem = null;

            string linkValue = selection.Item3;
            bool linkIsSteamId = linkValue.StartsWith("STEAM");

            if (BaseViewModel.hasConnection())
            {
                if (linkIsSteamId) await Navigation.PushAsync(new RecordsPlayerPage(game, mode, linkValue));
                else  await Navigation.PushAsync(new MapsMapPage(linkValue, game));
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

                LoadingAnimation.IsRunning = true;
                isLoading = true;

                await ChangeMostByType(true);

                LoadingAnimation.IsRunning = false;
                isLoading = true;
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
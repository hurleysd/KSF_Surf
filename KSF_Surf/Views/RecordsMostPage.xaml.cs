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
    public partial class RecordsMostPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects possibly used by "Most By Type" calls
        private List<MostPercentCompletionDatum> mostPCData;
        private List<MostCountDatum> mostCountData;
        private List<MostTopDatum> mostTopData;
        private List<MostGroupDatum> mostGroupData;
        private List<MostContestedWorldRecordDatum> mostContWrData;
        private List<MostContestedZoneDatum> mostContZoneData;
        private List<MostTimeDatum> mostTimeData;
        private int listIndex = 1;

        // variables for filters
        private readonly GameEnum defaultGame;
        private readonly ModeEnum defaultMode;
        private GameEnum game;
        private ModeEnum mode;
        private MostTypeEnum mostType;
        private string mostTypeString;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> recordsMostCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();

        public RecordsMostPage(GameEnum game, ModeEnum mode, GameEnum defaultGame, ModeEnum defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;
            mostType = MostTypeEnum.WR;
            mostTypeString = "Current WRs";

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            Title = "Records [" + EnumToString.NameString(game) + ", " + EnumToString.NameString(mode) + "]";
            MostTypePicker.ItemsSource = EnumToString.MostTypeNames;
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
                case MostTypeEnum.PC:
                    {
                        var mostPCDatum = await recordsViewModel.GetMostPC(game, mode, listIndex);
                        mostPCData = mostPCDatum?.data;
                        if (mostPCData is null || mostPCData.Count < 1) return;

                        foreach (MostPercentCompletionDatum datum in mostPCData)
                        {
                            players.Add(StringFormatter.CountryEmoji(datum.country) + " " + datum.name);
                            values.Add((double.Parse(datum.percentCompletion) * 100).ToString("0.00") + "%");
                            links.Add(datum.steamID);
                        }
                        break;
                    }
                case MostTypeEnum.WR:
                case MostTypeEnum.WRCP:
                case MostTypeEnum.WRB:
                case MostTypeEnum.MOST_WR:
                case MostTypeEnum.MOST_WRCP:
                case MostTypeEnum.MOST_WRB:
                    {
                        var mostCountDatum = await recordsViewModel.GetMostCount(game, mostType, mode, listIndex);
                        mostCountData = mostCountDatum?.data;
                        if (mostCountData is null || mostCountData.Count < 1) return;

                        foreach (MostCountDatum datum in mostCountData)
                        {
                            players.Add(StringFormatter.CountryEmoji(datum.country) + " " + datum.name);
                            values.Add(StringFormatter.IntString(datum.total));
                            links.Add(datum.steamID);
                        }
                        break;
                    }
                case MostTypeEnum.TOP10:
                    {
                        var mostTopDatum = await recordsViewModel.GetMostTop(game, mode, listIndex);
                        mostTopData = mostTopDatum?.data;
                        if (mostTopData is null || mostTopData.Count < 1) return;

                        foreach (MostTopDatum datum in mostTopData)
                        {
                            players.Add(StringFormatter.CountryEmoji(datum.country) + " " + datum.name);
                            values.Add(StringFormatter.PointsString(datum.top10Points));
                            links.Add(datum.steamID);
                        }
                        break;
                    }
                case MostTypeEnum.GROUP:
                    {
                        var mostGroupDatum = await recordsViewModel.GetMostGroup(game, mode, listIndex);
                        mostGroupData = mostGroupDatum?.data;
                        if (mostGroupData is null || mostGroupData.Count < 1) return;

                        foreach (MostGroupDatum datum in mostGroupData)
                        {
                            players.Add(StringFormatter.CountryEmoji(datum.country) + " " + datum.name);
                            values.Add(StringFormatter.PointsString(datum.groupPoints));
                            links.Add(datum.steamID);
                        }
                        break;
                    }
                case MostTypeEnum.MOST_CONTESTED_WR:
                    {
                        var mostContWrDatum = await recordsViewModel.GetMostContWr(game, mode, listIndex);
                        mostContWrData = mostContWrDatum?.data;
                        if (mostContWrData is null || mostContWrData.Count < 1) return;

                        foreach (MostContestedWorldRecordDatum datum in mostContWrData)
                        {
                            players.Add(datum.mapName);
                            values.Add(StringFormatter.IntString(datum.total));
                            links.Add(datum.mapName);
                        }
                        break;
                    }
                case MostTypeEnum.MOST_CONTESTED_WRCP:
                case MostTypeEnum.MOST_CONTESTED_WRB:
                    {
                        var mostContZoneDatum = await recordsViewModel.GetMostContZone(game, mostType, mode, listIndex);
                        mostContZoneData = mostContZoneDatum?.data;
                        if (mostContZoneData is null || mostContZoneData.Count < 1) return;

                        foreach (MostContestedZoneDatum datum in mostContZoneData)
                        {
                            string zoneString = StringFormatter.ZoneString(datum.zoneID, false, false);
                            players.Add(datum.mapName + " " + zoneString);
                            values.Add(StringFormatter.IntString(datum.total));
                            links.Add(datum.mapName);
                        }
                        break;
                    }
                case MostTypeEnum.PLAYTIME_DAY:
                case MostTypeEnum.PLAYTIME_WEEK:
                case MostTypeEnum.PLAYTIME_MONTH:
                    {
                        var mostTimeDatum = await recordsViewModel.GetMostTime(game, mostType, mode, listIndex);
                        mostTimeData = mostTimeDatum?.data;
                        if (mostTimeData is null || mostTimeData.Count < 1) return;

                        foreach (MostTimeDatum datum in mostTimeData)
                        {
                            players.Add(datum.mapName);
                            values.Add(StringFormatter.PlayTimeString(datum.totalplaytime.ToString(), true));
                            links.Add(datum.mapName);
                        }
                        break;
                    }
                default: return;
            }

            if (clearPrev) recordsMostCollectionViewItemsSource.Clear();
            LayoutMostByType(players, values, links);
            MostTypeOptionLabel.Text = "Type: " + EnumToString.NameString(mostType);
            Title = "Records [" + EnumToString.NameString(game) + ", " + EnumToString.NameString(mode) + "]";
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutMostByType(List<string> players, List<string> values, List<string> links)
        {
            for (int i = 0; i < players.Count && i < values.Count && i < links.Count; i++, listIndex++)
            {
                recordsMostCollectionViewItemsSource.Add(new Tuple<string, string, string>(listIndex + ". " + players[i], values[i], links[i]));
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
                case "Completion": mostType = MostTypeEnum.PC; break;
                case "Current WRs": mostType = MostTypeEnum.WR; break;
                case "Current WRCPs": mostType = MostTypeEnum.WRCP; break;
                case "Current WRBs": mostType = MostTypeEnum.WRB; break;
                case "Top10 Points": mostType = MostTypeEnum.TOP10; break;
                case "Group Points": mostType = MostTypeEnum.GROUP; break;
                case "Broken WRs": mostType = MostTypeEnum.MOST_WR; break;
                case "Broken WRCPs": mostType = MostTypeEnum.MOST_WRCP; break;
                case "Broken WRBs": mostType = MostTypeEnum.MOST_WRB; break;
                case "Contested WR": mostType = MostTypeEnum.MOST_CONTESTED_WR; break;
                case "Contested WRCP": mostType = MostTypeEnum.MOST_CONTESTED_WRCP; break;
                case "Contested WRB": mostType = MostTypeEnum.MOST_CONTESTED_WRB; break;
                case "Play Time Day": mostType = MostTypeEnum.PLAYTIME_DAY; break;
                case "Play Time Week": mostType = MostTypeEnum.PLAYTIME_WEEK; break;
                case "Play Time Month": mostType = MostTypeEnum.PLAYTIME_MONTH; break;
                default: return;
            }

            mostTypeString = selected;
            listIndex = 1;

            LoadingAnimation.IsRunning = true;
            isLoading = true;

            await ChangeMostByType(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void RecordsMost_ThresholdReached(object sender, EventArgs e)
        {
            if (mostType == MostTypeEnum.PLAYTIME_DAY
                || mostType == MostTypeEnum.PLAYTIME_WEEK
                || mostType == MostTypeEnum.PLAYTIME_MONTH) return;
            // calling with these ^ types again leads to JSON deserialization errors 

            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((listIndex - 1) % RecordsViewModel.MOST_QLIMIT) != 0) return; // didn't get full results
            if (listIndex >= RecordsViewModel.MOST_CLIMIT) return; // at call limit

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

            if (BaseViewModel.HasConnection())
            {
                if (linkIsSteamId) await Navigation.PushAsync(new RecordsPlayerPage(game, mode, linkValue));
                else await Navigation.PushAsync(new MapsMapPage(linkValue, game));
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

                LoadingAnimation.IsRunning = true;
                isLoading = true;

                await ChangeMostByType(true);

                LoadingAnimation.IsRunning = false;
                isLoading = true;
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
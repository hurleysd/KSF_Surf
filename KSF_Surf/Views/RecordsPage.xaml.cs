using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.Models;
using KSF_Surf.ViewModels;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;

        // objects used by "Recent" and "Surf Top" calls
        private List<SurfTopDatum> surfTopData;
        private List<RRDatum> recentRecordsData;
        private List<RR10Datum> recentRecords10Data;

        // objects possibly used by "Most By Type" calls
        private List<MostPCDatum> mostPCData;
        private List<MostCountDatum> mostCountData;
        private List<MostTopDatum> mostTopData;
        private List<MostGroupDatum> mostGroupData;
        private List<MostContWrDatum> mostContWrData;
        private List<MostContZoneDatum> mostContZoneData;
        private List<MostTimeDatum> mostTimeData;

        // variables for current filters
        private EFilter_Game game;
        private readonly EFilter_Game defaultGame;
        private EFilter_Mode mode;
        private readonly EFilter_Mode defaultMode;
        private EFilter_RRType recentRecordsType;
        private EFilter_MostType mostType;
        private string mostTypeString;

        public RecordsPage()
        {
            recordsViewModel = new RecordsViewModel();
            InitializeComponent();

            game = BaseViewModel.propertiesDict_getGame();
            defaultGame = game;
            mode = BaseViewModel.propertiesDict_getMode();
            defaultMode = mode;
            recentRecordsType = EFilter_RRType.map;
            mostType = EFilter_MostType.wr;
            mostTypeString = "Current WRs";

            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
            MostTypePicker.ItemsSource = EFilter_ToString.mosttype_arr;
        }

        // UI ---------------------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecentRecords(EFilter_Game game, EFilter_RRType type, EFilter_Mode mode)
        {
            if (type == EFilter_RRType.top)
            {
                var recentRecords10Datum = await recordsViewModel.GetRecentRecords10(game, mode);
                recentRecords10Data = recentRecords10Datum?.data;
                if (recentRecords10Data is null) return;
                LayoutRecentRecords10();
            }
            else
            {
                var recentRecordsDatum = await recordsViewModel.GetRecentRecords(game, type, mode);
                recentRecordsData = recentRecordsDatum?.data;
                if (recentRecordsData is null) return;
                LayoutRecentRecords(EFilter_ToString.toString2(type));
            }
        }

        private async Task ChangeMostByType(EFilter_Game game, EFilter_MostType type, EFilter_Mode mode, bool clearGrid)
        {
            string rightColString = "Player";
            string leftColString = "";
            List<string> players = new List<string>();
            List<string> values = new List<string>();

            switch (type)
            {
                case EFilter_MostType.pc:
                    {
                        var mostPCDatum = await recordsViewModel.GetMostPC(game, mode);
                        mostPCData = mostPCDatum?.data;
                        if (mostPCData is null) return;
                        
                        leftColString = "Completion";
                        foreach (MostPCDatum datum in mostPCData)
                        {
                            players.Add(String_Formatter.toEmoji_Country(datum.country) + " " + datum.name);
                            values.Add((double.Parse(datum.percentCompletion) * 100).ToString("0.00") + "%");
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
                        var mostCountDatum = await recordsViewModel.GetMostCount(game, type, mode);
                        mostCountData = mostCountDatum?.data;
                        if (mostCountData is null) return;
                        
                        leftColString = "Total";
                        foreach (MostCountDatum datum in mostCountData)
                        {
                            players.Add(String_Formatter.toEmoji_Country(datum.country) + " " + datum.name);
                            values.Add(String_Formatter.toString_Int(datum.total));
                        }
                        break;
                    }
                case EFilter_MostType.top10:
                    {
                        var mostTopDatum = await recordsViewModel.GetMostTop(game, mode);
                        mostTopData = mostTopDatum?.data;
                        if (mostTopData is null) return;
                        
                        leftColString = "Points";
                        foreach (MostTopDatum datum in mostTopData)
                        {
                            players.Add(String_Formatter.toEmoji_Country(datum.country) + " " + datum.name);
                            values.Add(String_Formatter.toString_Points(datum.top10Points));
                        }
                        break;
                    }
                case EFilter_MostType.group:
                    {
                        var mostGroupDatum = await recordsViewModel.GetMostGroup(game, mode);
                        mostGroupData = mostGroupDatum?.data;
                        if (mostGroupData is null) return;
                        
                        leftColString = "Points";
                        foreach (MostGroupDatum datum in mostGroupData)
                        {
                            players.Add(String_Formatter.toEmoji_Country(datum.country) + " " + datum.name);
                            values.Add(String_Formatter.toString_Points(datum.groupPoints));
                        }
                        break;
                    }
                case EFilter_MostType.mostcontestedwr:
                    {
                        var mostContWrDatum = await recordsViewModel.GetMostContWr(game, mode);
                        mostContWrData = mostContWrDatum?.data;
                        if (mostContWrData is null) return;
                        
                        rightColString = "Map";
                        leftColString = "Times Broken";
                        foreach (MostContWrDatum datum in mostContWrData)
                        {
                            players.Add(datum.mapName);
                            values.Add(String_Formatter.toString_Int(datum.total));
                        }
                        break;
                    }
                case EFilter_MostType.mostcontestedwrcp: 
                case EFilter_MostType.mostcontestedwrb:
                    {
                        var mostContZoneDatum = await recordsViewModel.GetMostContZone(game, type, mode);
                        mostContZoneData = mostContZoneDatum?.data;
                        if (mostContZoneData is null) return;
                        
                        rightColString = "Zone";
                        leftColString = "Times Broken";
                        foreach (MostContZoneDatum datum in mostContZoneData)
                        {
                            string zoneString = EFilter_ToString.zoneFormatter(datum.zoneID, false);
                            players.Add(datum.mapName + " " + zoneString);
                            values.Add(String_Formatter.toString_Int(datum.total));
                        }
                        break;
                    }
                case EFilter_MostType.playtimeday:
                case EFilter_MostType.playtimeweek:
                case EFilter_MostType.playtimemonth:
                    {
                        var mostTimeDatum = await recordsViewModel.GetMostTime(game, type, mode);
                        mostTimeData = mostTimeDatum?.data;
                        if (mostTimeData is null) return;

                        rightColString = "Map";
                        leftColString = "Time";
                        foreach (MostTimeDatum datum in mostTimeData)
                        {
                            players.Add(datum.mapName);
                            values.Add(String_Formatter.toString_PlayTime(datum.totalplaytime.ToString(), true));
                        }
                        break;
                    }
                default: return;
            }

            if (clearGrid) ClearMostByTypeGrid(rightColString, leftColString);
            LayoutMostByType(type, players, values);
        }

        private async Task ChangeSurfTop(EFilter_Game game, EFilter_Mode mode, bool clearGrid)
        {
            if (clearGrid) ClearSurfTopGrid();

            var surfTopDatum = await recordsViewModel.GetSurfTop(game, mode);
            surfTopData = surfTopDatum?.data;
            if (surfTopData is null) return;
            LayoutSurfTop();
        }

        // Displaying Changes --------------------------------------------------------------------------

        private void LayoutSurfTop()
        {
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";

            int i = 1;
            foreach (SurfTopDatum datum in surfTopData)
            {
                TopRankStack.Children.Add(new Label {
                    Text = i + ". " + String_Formatter.toEmoji_Country(datum.country) + " " + datum.name,
                    Style = App.Current.Resources["GridLabelStyle"] as Style
                });
                TopPointsStack.Children.Add( new Label {
                    Text = String_Formatter.toString_Points(datum.points),
                    Style = App.Current.Resources["GridLabelStyle"] as Style
                });

                i++;
            }
        }

        private void LayoutMostByType(EFilter_MostType type, List<string> players, List<string> values)
        {
            MostTypeOptionLabel.Text = "[Type: " + EFilter_ToString.toString2(type) + "]";

            for (int i = 1; i <= players.Count; i++)
            {
                MostPlayerStack.Children.Add(new Label {
                    Text = i + ". " + players[i - 1],
                    Style = App.Current.Resources["GridLabelStyle"] as Style
                });
                MostValueStack.Children.Add( new Label {
                    Text = values[i - 1],
                    Style = App.Current.Resources["GridLabelStyle"] as Style
                });
            }
        }


        private void LayoutRecentRecords(string typeString)
        {
            RRTypeOptionLabel.Text = "[Type: " + typeString + "]";

            RecordsStack.Children.Clear();

            int i = 0;
            int length = recentRecordsData.Count;
            foreach (RRDatum datum in recentRecordsData)
            {
                RecordsStack.Children.Add(new Label
                {
                    Text = datum.mapName + " " + EFilter_ToString.zoneFormatter(datum.zoneID, false),
                    Style = App.Current.Resources["RRLabelStyle"] as Style

                });
                RecordsStack.Children.Add(new Label
                {
                    Text = String_Formatter.toEmoji_Country(datum.country) + " " + datum.playerName + " on " + datum.server + " server",
                    Style = App.Current.Resources["RR2LabelStyle"] as Style
                });

                string rrtime = "in " + String_Formatter.toString_RankTime(datum.surfTime) + " (";
                if (datum.wrDiff == "0")
                {
                    if (datum.r2Diff is null)
                    {
                        rrtime += "WR N/A";
                    }
                    else
                    {
                        rrtime += "WR-" + String_Formatter.toString_RankTime(datum.r2Diff.Substring(1));
                    }
                }
                else
                {
                    rrtime += "now WR+" + String_Formatter.toString_RankTime(datum.wrDiff);
                }
                rrtime += ") (" + String_Formatter.toString_LastOnline(datum.date) + ")";
                RecordsStack.Children.Add(new Label
                {
                    Text = rrtime,
                    Style = App.Current.Resources["TimeLabelStyle"] as Style
                });

                if (++i != length)
                {
                    RecordsStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["SeparatorStyle"] as Style
                    });
                }
            }
        }

        private void LayoutRecentRecords10()
        {
            RRTypeOptionLabel.Text = "[Type: Top10]";

            RecordsStack.Children.Clear();

            int i = 0;
            int length = recentRecords10Data.Count;
            foreach (RR10Datum datum in recentRecords10Data)
            {
                int rank = int.Parse(datum.newRank);
                RecordsStack.Children.Add(new Label
                {
                    Text = datum.mapName + " [R" + rank + "]",
                    Style = App.Current.Resources["RRLabelStyle"] as Style

                });
                RecordsStack.Children.Add(new Label
                {
                    Text = String_Formatter.toEmoji_Country(datum.country) + " " + datum.playerName + " on " + datum.server + " server",
                    Style = App.Current.Resources["RR2LabelStyle"] as Style,
                });

                string rrtime = "in " + String_Formatter.toString_RankTime(datum.surfTime) + " (";
                if (datum.wrDiff == "0")
                {
                    rrtime += "WR";
                }
                else
                {
                    rrtime += "WR+" + String_Formatter.toString_RankTime(datum.wrDiff);
                }
                rrtime += ") (" + String_Formatter.toString_LastOnline(datum.date) + ")";
                RecordsStack.Children.Add(new Label
                {
                    Text = rrtime,
                    Style = App.Current.Resources["TimeLabelStyle"] as Style
                });

                if (++i != length)
                {
                    RecordsStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["SeparatorStyle"] as Style
                    });
                }
            }
        }

        private void ClearSurfTopGrid()
        {
            TopRankStack.Children.Clear();
            TopRankStack.Children.Add(TopRankLabel);
            TopPointsStack.Children.Clear();
            TopPointsStack.Children.Add(TopPointsLabel);
        }

        private void ClearMostByTypeGrid(string rightColText, string leftColText)
        {
            MostPlayerStack.Children.Clear();
            MostPlayerLabel.Text = rightColText;
            MostPlayerStack.Children.Add(MostPlayerLabel);
            MostValueStack.Children.Clear();
            MostValueLabel.Text = leftColText;
            MostValueStack.Children.Add(MostValueLabel);
        }

        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                await ChangeRecentRecords(game, recentRecordsType, mode);
                await ChangeMostByType(game, mostType, mode, false);
                await ChangeSurfTop(game, mode, false);
                hasLoaded = true;
            }
        }

        private async void RRTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EFilter_ToString.toString2(recentRecordsType);
            foreach (string type in EFilter_ToString.rrtype_arr)
            {
                if (type != currentTypeString)
                {
                    types.Add(type);
                }
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());

            EFilter_RRType newType = EFilter_RRType.map;
            switch (newTypeString)
            {
                case "Cancel": return;
                case "Top10": newType = EFilter_RRType.top; break;
                case "Stage": newType = EFilter_RRType.stage; break;
                case "Bonus": newType = EFilter_RRType.bonus; break;
                case "All WRs": newType = EFilter_RRType.all; break;
            }

            recentRecordsType = newType;
            await ChangeRecentRecords(game, recentRecordsType, mode);
        }

        private void MostTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            MostTypePicker.SelectedItem = mostTypeString;
            MostTypePicker.Focus();
        }

        private async void MostTypePicker_Unfocused(object sender, FocusEventArgs e)
        {
            string selected = (string)MostTypePicker.SelectedItem;
            if (selected == mostTypeString)
            {
                return;
            }

            EFilter_MostType newType = EFilter_MostType.wr;
            switch (selected)
            {
                case "Cancel": return;
                case "Completion": newType = EFilter_MostType.pc; break;
                case "Current WRs": newType = EFilter_MostType.wr; break;
                case "Current WRCPs": newType = EFilter_MostType.wrcp; break;
                case "Current WRBs": newType = EFilter_MostType.wrb; break;
                case "Top10 Points": newType = EFilter_MostType.top10; break;
                case "Group Points": newType = EFilter_MostType.group; break;
                case "Broken WRs": newType = EFilter_MostType.mostwr; break;
                case "Broken WRCPs": newType = EFilter_MostType.mostwrcp; break;
                case "Broken WRBs": newType = EFilter_MostType.mostwrb; break;
                case "Contested WR": newType = EFilter_MostType.mostcontestedwr; break;
                case "Contested WRCP": newType = EFilter_MostType.mostcontestedwrcp; break;
                case "Contested WRB": newType = EFilter_MostType.mostcontestedwrb; break;
                case "Play Time Day": newType = EFilter_MostType.playtimeday; break;
                case "Play Time Week": newType = EFilter_MostType.playtimeweek; break;
                case "Play Time Month": newType = EFilter_MostType.playtimemonth; break;
            }

            mostType = newType;
            mostTypeString = selected;
            await ChangeMostByType(game, mostType, mode, true);
        }

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new RecordsFilterPage(ApplyFilters, game, mode, defaultGame, defaultMode));
            }
            else
            {
                await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
            }
        }

        internal async void ApplyFilters(EFilter_Game newGame, EFilter_Mode newMode)
        {
            if (BaseViewModel.hasConnection())
            {
                if (newGame == game && newMode == mode) return;

                game = newGame;
                mode = newMode;
                
                await ChangeRecentRecords(game, recentRecordsType, mode);
                await ChangeMostByType(game, mostType, mode, true);
                await ChangeSurfTop(game, mode, true);
            }
            else
            {
                await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
            }
            await RecordsPageScrollView.ScrollToAsync(0, 0, true);
        }
    }

    #endregion
}
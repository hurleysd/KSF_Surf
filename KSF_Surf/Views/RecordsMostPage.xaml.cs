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
    public partial class RecordsMostPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;
        private readonly int LIST_LIMIT = 25;
        private readonly int CALL_LIMIT = 250;

        // objects possibly used by "Most By Type" calls
        private List<MostPCDatum> mostPCData;
        private List<MostCountDatum> mostCountData;
        private List<MostTopDatum> mostTopData;
        private List<MostGroupDatum> mostGroupData;
        private List<MostContWrDatum> mostContWrData;
        private List<MostContZoneDatum> mostContZoneData;
        private List<MostTimeDatum> mostTimeData;

        private int list_index = 1;
        private bool moreRecords = false;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private EFilter_MostType mostType;
        private string mostTypeString;


        public RecordsMostPage(string title, RecordsViewModel recordsViewModel, EFilter_Game game, EFilter_Mode mode)
        {
            this.recordsViewModel = recordsViewModel;
            this.game = game;
            this.mode = mode;
            mostType = EFilter_MostType.wr;
            mostTypeString = "Current WRs";

            InitializeComponent();
            Title = title;
            MostTypePicker.ItemsSource = EFilter_ToString.mosttype_arr;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

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
                        var mostPCDatum = await recordsViewModel.GetMostPC(game, mode, list_index);
                        mostPCData = mostPCDatum?.data;
                        if (mostPCData is null || mostPCData.Count < 1)
                        {
                            MoreFrame.IsVisible = false;
                            return;
                        }

                        leftColString = "Total";
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
                        var mostCountDatum = await recordsViewModel.GetMostCount(game, type, mode, list_index);
                        mostCountData = mostCountDatum?.data;
                        if (mostCountData is null || mostCountData.Count < 1)
                        {
                            MoreFrame.IsVisible = false;
                            return;
                        }

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
                        var mostTopDatum = await recordsViewModel.GetMostTop(game, mode, list_index);
                        mostTopData = mostTopDatum?.data;
                        if (mostTopData is null || mostTopData.Count < 1)
                        {
                            MoreFrame.IsVisible = false;
                            return;
                        }

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
                        var mostGroupDatum = await recordsViewModel.GetMostGroup(game, mode, list_index);
                        mostGroupData = mostGroupDatum?.data;
                        if (mostGroupData is null || mostGroupData.Count < 1)
                        {
                            MoreFrame.IsVisible = false;
                            return;
                        }

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
                        var mostContWrDatum = await recordsViewModel.GetMostContWr(game, mode, list_index);
                        mostContWrData = mostContWrDatum?.data;
                        if (mostContWrData is null || mostContWrData.Count < 1)
                        {
                            MoreFrame.IsVisible = false;
                            return;
                        }

                        rightColString = "Map";
                        leftColString = "Beaten";
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
                        var mostContZoneDatum = await recordsViewModel.GetMostContZone(game, type, mode, list_index);
                        mostContZoneData = mostContZoneDatum?.data;
                        if (mostContZoneData is null || mostContZoneData.Count < 1)
                        {
                            MoreFrame.IsVisible = false;
                            return;
                        }

                        rightColString = "Zone";
                        leftColString = "Beaten";
                        foreach (MostContZoneDatum datum in mostContZoneData)
                        {
                            string zoneString = EFilter_ToString.zoneFormatter(datum.zoneID, false, false);
                            players.Add(datum.mapName + " " + zoneString);
                            values.Add(String_Formatter.toString_Int(datum.total));
                        }
                        break;
                    }
                case EFilter_MostType.playtimeday:
                case EFilter_MostType.playtimeweek:
                case EFilter_MostType.playtimemonth:
                    {
                        var mostTimeDatum = await recordsViewModel.GetMostTime(game, type, mode, list_index);
                        mostTimeData = mostTimeDatum?.data;
                        if (mostTimeData is null || mostTimeData.Count < 1) return;

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

            MostTypeOptionLabel.Text = "Type: " + EFilter_ToString.toString2(type);

            if (clearGrid) ClearMostByTypeGrid(rightColString, leftColString);
            LayoutMostByType(type, players, values);
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutMostByType(EFilter_MostType type, List<string> players, List<string> values)
        {
            for (int i = 0; i < players.Count; i++, list_index++)
            {
                MostPlayerStack.Children.Add(new Label
                {
                    Text = list_index + ". " + players[i],
                    Style = App.Current.Resources["GridLabelStyle"] as Style
                });
                MostValueStack.Children.Add(new Label
                {
                    Text = values[i],
                    Style = App.Current.Resources["GridLabelStyle"] as Style
                });
            }

            moreRecords = (((list_index - 1) % LIST_LIMIT == 0) && ((list_index - 1) < CALL_LIMIT));
            if (type == EFilter_MostType.playtimeday || type == EFilter_MostType.playtimeweek || type == EFilter_MostType.playtimemonth)
            {
                moreRecords = false; // calling with these types again leads to JSON deserialization errors 
            }
            MoreFrame.IsVisible = moreRecords;
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
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;
                await ChangeMostByType(game, mostType, mode, false);

                LoadingAnimation.IsRunning = false;
                RecordsMostPageScrollView.IsVisible = true;
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
            list_index = 1;

            LoadingAnimation.IsRunning = true;
            await ChangeMostByType(game, mostType, mode, true);
            LoadingAnimation.IsRunning = false;
        }

        private async void MoreButton_Tapped(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection()) return;
            isLoading = true;

            MoreButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            MoreLabel.IsVisible = false;
            MoreLoadingAnimation.IsRunning = true;

            await ChangeMostByType(game, mostType, mode, false);
            
            MoreButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
            MoreLoadingAnimation.IsRunning = false;
            MoreLabel.IsVisible = true;
            isLoading = false;
        }

        #endregion
    }
}
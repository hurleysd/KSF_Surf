using System;
using System.ComponentModel;
using System.Collections.Generic;

using Xamarin.Forms;

using KSF_Surf.Models;
using KSF_Surf.ViewModels;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsPage : ContentPage
    {
        private RecordsViewModel recordsViewModel;

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
        private EFilter_Mode surfTopMode;
        private EFilter_Mode recentRecordsMode;
        private EFilter_RRType recentRecordsType;
        private EFilter_Mode mostMode;
        private EFilter_MostType mostType;
        private string mostTypeString;

        // vibration
        private bool allowVibrate = true;

        public RecordsPage()
        {
            recordsViewModel = new RecordsViewModel();
            InitializeComponent();

            game = EFilter_Game.css;
            surfTopMode = EFilter_Mode.fw;
            recentRecordsMode = EFilter_Mode.fw;
            recentRecordsType = EFilter_RRType.map;
            mostMode = EFilter_Mode.fw;
            mostType = EFilter_MostType.wr;
            mostTypeString = "Current WRs";

            ChangeRecentRecords(game, recentRecordsType, recentRecordsMode);
            ChangeMostByType(game, mostType, mostMode, false);
            ChangeSurfTop(game, surfTopMode, false);

            MostTypePicker.ItemsSource = EFilter_ToString.mosttype_arr;
        }

        // UI ---------------------------------------------------------------------------------------------------------
        #region UI

        private void ChangeAndLayoutGame(EFilter_Game newGame)
        {
            if (game == newGame) return;
            BaseViewModel.vibrate(allowVibrate);

            Color GrayTextColor = (Color)App.Current.Resources["GrayTextColor"];
            Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];

            switch (game)
            {
                case EFilter_Game.css: GameCSSLabel.TextColor = GrayTextColor; break;
                case EFilter_Game.css100t: GameCSS100TLabel.TextColor = GrayTextColor; ; break;
                case EFilter_Game.csgo: GameCSGOLabel.TextColor = GrayTextColor; break;
                default: return;
            }

            switch (newGame)
            {
                case EFilter_Game.css: GameCSSLabel.TextColor = tappedTextColor; break;
                case EFilter_Game.css100t: GameCSS100TLabel.TextColor = tappedTextColor; break;
                case EFilter_Game.csgo: GameCSGOLabel.TextColor = tappedTextColor; break;
                default: return;
            }

            game = newGame;
            ChangeRecentRecords(game, recentRecordsType, recentRecordsMode);
            ChangeMostByType(game, mostType, mostMode, true);
            ChangeSurfTop(game, surfTopMode, true);
        }


        private void ChangeRecentRecords(EFilter_Game game, EFilter_RRType type, EFilter_Mode mode)
        {

            if (type == EFilter_RRType.top)
            {
                recentRecords10Data = RecordsViewModel.GetRecentRecords10(game, mode)?.data;
                if (recentRecords10Data is null) return;
                LayoutRecentRecords10(EFilter_ToString.toString(recentRecordsMode));
            }
            else
            {
                recentRecordsData = RecordsViewModel.GetRecentRecords(game, type, mode)?.data;
                if (recentRecordsData is null) return;
                LayoutRecentRecords(EFilter_ToString.toString2(type), EFilter_ToString.toString(mode));
            }
        }

        private void ChangeMostByType(EFilter_Game game, EFilter_MostType type, EFilter_Mode mode, bool clearGrid)
        {
            string rightColString = "Player";
            string leftColString = "";
            List<string> players = new List<string>();
            List<string> values = new List<string>();

            switch (type)
            {
                case EFilter_MostType.pc:
                    {
                        mostPCData = RecordsViewModel.GetMostPC(game, mode)?.data;
                        if (mostPCData is null) return;
                        
                        leftColString = "Completion";
                        foreach (MostPCDatum datum in mostPCData)
                        {
                            players.Add(datum.name);
                            values.Add((double.Parse(datum.percentCompletion) * 100).ToString("0.##") + "%");
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
                        mostCountData = RecordsViewModel.GetMostCount(game, type, mode)?.data;
                        if (mostCountData is null) return;
                        
                        leftColString = "Total";
                        foreach (MostCountDatum datum in mostCountData)
                        {
                            players.Add(datum.name);
                            values.Add(datum.total);
                        }
                        break;
                    }
                case EFilter_MostType.top10:
                    {
                        mostTopData = RecordsViewModel.GetMostTop(game, mode)?.data;
                        if (mostTopData is null) return;
                        
                        leftColString = "Points";
                        foreach (MostTopDatum datum in mostTopData)
                        {
                            players.Add(datum.name);
                            values.Add(datum.top10Points);
                        }
                        break;
                    }
                case EFilter_MostType.group:
                    {
                        mostGroupData = RecordsViewModel.GetMostGroup(game, mode)?.data;
                        if (mostGroupData is null) return;
                        
                        leftColString = "Points";
                        foreach (MostGroupDatum datum in mostGroupData)
                        {
                            players.Add(datum.name);
                            values.Add(datum.groupPoints);
                        }
                        break;
                    }
                case EFilter_MostType.mostcontestedwr:
                    {
                        mostContWrData = RecordsViewModel.GetMostContWr(game, mode)?.data;
                        if (mostContWrData is null) return;
                        
                        rightColString = "Map";
                        leftColString = "Times Broken";
                        foreach (MostContWrDatum datum in mostContWrData)
                        {
                            players.Add(datum.mapName);
                            values.Add(datum.total);
                        }
                        break;
                    }
                case EFilter_MostType.mostcontestedwrcp: 
                case EFilter_MostType.mostcontestedwrb:
                    {
                        mostContZoneData = RecordsViewModel.GetMostContZone(game, type, mode)?.data;
                        if (mostContZoneData is null) return;
                        
                        rightColString = "Zone";
                        leftColString = "Times Broken";
                        foreach (MostContZoneDatum datum in mostContZoneData)
                        {
                            string zoneString = EFilter_ToString.zoneFormatter(datum.zoneID);
                            players.Add(datum.mapName + " " + zoneString);
                            values.Add(datum.total);
                        }
                        break;
                    }
                case EFilter_MostType.playtimeday:
                case EFilter_MostType.playtimeweek:
                case EFilter_MostType.playtimemonth:
                    {
                        mostTimeData = RecordsViewModel.GetMostTime(game, type, mode)?.data;
                        if (mostTimeData is null) return;

                        rightColString = "Map";
                        leftColString = "Time";
                        foreach (MostTimeDatum datum in mostTimeData)
                        {
                            players.Add(datum.mapName);
                            values.Add(Seconds_Formatter.toString_PlayTime(datum.totalplaytime.ToString(), true));
                        }
                        break;
                    }
                default: return;
            }

            if (clearGrid) ClearMostByTypeGrid(rightColString, leftColString);
            LayoutMostByType(type, EFilter_ToString.toString(mode), players, values);
        }

        private void ChangeSurfTop(EFilter_Game game, EFilter_Mode mode, bool clearGrid)
        {
            if (clearGrid) ClearSurfTopGrid();

            surfTopData = RecordsViewModel.GetSurfTop(game, surfTopMode)?.data;
            if (surfTopData is null) return;
            LayoutSurfTop(EFilter_ToString.toString(surfTopMode));
        }

        // Displaying Changes --------------------------------------------------------------------------

        private void LayoutSurfTop(string modeString)
        {
            TopStyleOptionLabel.Text = "[Style: " + modeString + "]";

            int i = 1;
            foreach (SurfTopDatum datum in surfTopData)
            {
                Label RankLabel = new Label
                {
                    Text = i + ". " + datum.name,
                    Style = Resources["GridLabelStyle"] as Style
                };
                TopRankStack.Children.Add(RankLabel);

                Label PointsLabel = new Label
                {
                    Text = datum.points,
                    Style = Resources["GridLabelStyle"] as Style
                };
                TopPointsStack.Children.Add(PointsLabel);

                i++;
            }
        }

        private void LayoutMostByType(EFilter_MostType type, string modeString, List<string> players, List<string> values)
        {
            MostTypeOptionLabel.Text = "[Type: " + EFilter_ToString.toString2(type) + "]";
            MostStyleOptionLabel.Text = "[Style: " + modeString + "]";

            for (int i = 1; i <= players.Count; i++)
            {
                Label PlayerLabel = new Label
                {
                    Text = i + ". " + players[i - 1],
                    Style = Resources["GridLabelStyle"] as Style
                };
                MostPlayerStack.Children.Add(PlayerLabel);

                Label ValueLabel = new Label
                {
                    Text = values[i - 1],
                    Style = Resources["GridLabelStyle"] as Style
                };
                MostValueStack.Children.Add(ValueLabel);
            }
        }


        private void LayoutRecentRecords(string typeString, string modeString)
        {
            RRTypeOptionLabel.Text = "[Type: " + typeString + "]";
            RRStyleOptionLabel.Text = "[Style: " + modeString + "]";

            string[] rrstring_arr = new string[10];
            string[] rrtime_arr = new string[10];

            int i = 0;
            foreach (RRDatum datum in recentRecordsData)
            {
                string rrstring = "";
                rrstring += datum.playerName;
                rrstring += " on " + datum.mapName;
                rrstring_arr[i] = rrstring;

                string rrtime = "";
                string zoneString = EFilter_ToString.zoneFormatter(datum.zoneID);
                rrtime += zoneString + " ";
                
                rrtime += "in " + Seconds_Formatter.toString_RankTime(datum.surfTime);
                Int64 when = Int64.Parse(datum.dateNow) - Int64.Parse(datum.date);
                rrtime += " (" + Seconds_Formatter.toString_PlayTime(when.ToString(), false) + " ago)";
                rrtime_arr[i] = rrtime;

                i++;
            }

            reassignRRLabels(rrstring_arr, rrtime_arr);
        }

        private void LayoutRecentRecords10(string modeString)
        {
            RRTypeOptionLabel.Text = "[Type: Top10]";
            RRStyleOptionLabel.Text = "[Style: " + modeString + "]";

            string[] rrstring_arr = new string[10];
            string[] rrtime_arr = new string[10];

            int i = 0;
            foreach (RR10Datum datum in recentRecords10Data)
            {
                string rrstring = "";
                rrstring += datum.playerName;
                int rank = int.Parse(datum.newRank);

                rrstring += " R" + rank + " on " + datum.mapName;
                rrstring_arr[i] = rrstring;

                string rrtime = "";
                string zoneString = EFilter_ToString.zoneFormatter(datum.zoneID);
                rrtime += zoneString + " ";


                rrtime += "in " + Seconds_Formatter.toString_RankTime(datum.surfTime);
                Int64 when = Int64.Parse(datum.dateNow) - Int64.Parse(datum.date);
                rrtime += " (" + Seconds_Formatter.toString_PlayTime(when.ToString(), false) + " ago)";
                rrtime_arr[i] = rrtime;

                i++;
            }

            reassignRRLabels(rrstring_arr, rrtime_arr);
        }

        private void reassignRRLabels(string[] rr, string[] time)
        {
            Label_RR0.Text = rr[0];
            Label_RR1.Text = rr[1];
            Label_RR2.Text = rr[2];
            Label_RR3.Text = rr[3];
            Label_RR4.Text = rr[4];
            Label_RR5.Text = rr[5];
            Label_RR6.Text = rr[6];
            Label_RR7.Text = rr[7];
            Label_RR8.Text = rr[8];
            Label_RR9.Text = rr[9];

            Label_Time0.Text = time[0];
            Label_Time1.Text = time[1];
            Label_Time2.Text = time[2];
            Label_Time3.Text = time[3];
            Label_Time4.Text = time[4];
            Label_Time5.Text = time[5];
            Label_Time6.Text = time[6];
            Label_Time7.Text = time[7];
            Label_Time8.Text = time[8];
            Label_Time9.Text = time[9];
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

        private void GameCSSLabel_Tapped(object sender, EventArgs e) => ChangeAndLayoutGame(EFilter_Game.css);

        private void GameCSS100TLabel_Tapped(object sender, EventArgs e) => ChangeAndLayoutGame(EFilter_Game.css100t);

        private void GameCSGOLabel_Tapped(object sender, EventArgs e) => ChangeAndLayoutGame(EFilter_Game.csgo);

        private async void TopStyleOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> modes = new List<string>();
            string currentModeString = EFilter_ToString.toString(surfTopMode);
            foreach (string mode in EFilter_ToString.modes_arr)
            {
                if (mode != currentModeString)
                {
                    modes.Add(mode);
                }
            }

            string newStyle = await DisplayActionSheet("Choose a different style", "Cancel", null, modes.ToArray());

            EFilter_Mode newMode = EFilter_Mode.fw;
            switch (newStyle)
            {
                case "Cancel": return;
                case "HSW": newMode = EFilter_Mode.hsw; break;
                case "SW": newMode = EFilter_Mode.sw; break;
                case "BW": newMode = EFilter_Mode.bw; break;
            }

            surfTopMode = newMode;
            ChangeSurfTop(game, surfTopMode, true);
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
                case "All": newType = EFilter_RRType.all; break;
            }

            recentRecordsType = newType;
            ChangeRecentRecords(game, recentRecordsType, recentRecordsMode);
        }

        private async void RRStyleOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> modes = new List<string>();
            string currentModeString = EFilter_ToString.toString(recentRecordsMode);
            foreach (string mode in EFilter_ToString.modes_arr)
            {
                if (mode != currentModeString)
                {
                    modes.Add(mode);
                }
            }

            string newStyle = await DisplayActionSheet("Choose a different style", "Cancel", null, modes.ToArray());

            EFilter_Mode newMode = EFilter_Mode.fw;
            switch (newStyle)
            {
                case "Cancel": return;
                case "HSW": newMode = EFilter_Mode.hsw; break;
                case "SW": newMode = EFilter_Mode.sw; break;
                case "BW": newMode = EFilter_Mode.bw; break;
            }

            recentRecordsMode = newMode;
            ChangeRecentRecords(game, recentRecordsType, recentRecordsMode);
        }

        private void MostTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            MostTypePicker.SelectedItem = mostTypeString;
            MostTypePicker.Focus();
        }

        private void MostTypePicker_Unfocused(object sender, FocusEventArgs e)
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
            ChangeMostByType(game, mostType, mostMode, true);
        }

        private async void MostStyleOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> modes = new List<string>();
            string currentModeString = EFilter_ToString.toString(mostMode);
            foreach (string mode in EFilter_ToString.modes_arr)
            {
                if (mode != currentModeString)
                {
                    modes.Add(mode);
                }
            }

            string newStyle = await DisplayActionSheet("Choose a different style", "Cancel", null, modes.ToArray());

            EFilter_Mode newMode = EFilter_Mode.fw;
            switch (newStyle)
            {
                case "Cancel": return;
                case "HSW": newMode = EFilter_Mode.hsw; break;
                case "SW": newMode = EFilter_Mode.sw; break;
                case "BW": newMode = EFilter_Mode.bw; break;
            }

            mostMode = newMode;
            ChangeMostByType(game, mostType, mostMode, true);
        }
    }

    #endregion
}
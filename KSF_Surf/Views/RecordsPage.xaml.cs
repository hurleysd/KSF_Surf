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

        private List<SurfTopDatum> surfTopData;
        private List<RRDatum> recentRecordsData;
        private List<RR10Datum> recentRecords10Data;

        private EFilter_Game game;
        private EFilter_Mode surfTopMode;
        private EFilter_Mode recentRecordsMode;
        private EFilter_RRType recentRecordsType;

        private bool allowVibrate = true;


        public RecordsPage()
        {
            recordsViewModel = new RecordsViewModel();
            InitializeComponent();

            game = EFilter_Game.css;
            surfTopMode = EFilter_Mode.fw;
            recentRecordsMode = EFilter_Mode.fw;
            recentRecordsType = EFilter_RRType.map;

            ChangeRecentRecords(game, recentRecordsType, recentRecordsMode);
            ChangeSurfTop(game, surfTopMode);
        }

        private void ChangeRecentRecords(EFilter_Game game, EFilter_RRType type, EFilter_Mode mode)
        {
            ClearRecentRecordsGrid();

            if (type == EFilter_RRType.top)
            {
                recentRecords10Data = RecordsViewModel.GetRecentRecords10(game, recentRecordsMode)?.data;
                if (recentRecords10Data is null) return;
                LayoutRecentRecords10(EFilter_ToString.toString(recentRecordsMode));
            }
            else
            {
                recentRecordsData = RecordsViewModel.GetRecentRecords(game, type, recentRecordsMode)?.data;
                if (recentRecordsData is null) return;
                LayoutRecentRecords(EFilter_ToString.toString2(type), EFilter_ToString.toString(recentRecordsMode));
            }
        }

        private void ChangeSurfTop(EFilter_Game game, EFilter_Mode mode)
        {
            ClearSurfTopGrid();

            surfTopData = RecordsViewModel.GetSurfTop(game, surfTopMode)?.data;
            if (surfTopData is null) return;
            LayoutSurfTop(EFilter_ToString.toString(surfTopMode));
        }

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

        private void ClearSurfTopGrid()
        {
            TopRankStack.Children.Clear();
            TopRankStack.Children.Add(TopRankLabel);
            TopPointsStack.Children.Clear();
            TopPointsStack.Children.Add(TopPointsLabel);
        }

        private void LayoutRecentRecords(string typeString, string modeString)
        {
            RRTypeOptionLabel.Text = "[Type: " + typeString + "]";
            RRStyleOptionLabel.Text = "[Style: " + modeString + "]";

            foreach (RRDatum datum in recentRecordsData)
            {
                Label PlayerLabel = new Label
                {
                    Text = datum.playerName,
                    Style = Resources["GridLabelStyle"] as Style
                };
                Label BlankLabel = new Label()
                {
                    Text = " ",
                    FontSize = 14
                };
                RRPlayerStack.Children.Add(PlayerLabel);
                RRPlayerStack.Children.Add(BlankLabel);

                Label MapLabel = new Label
                {
                    Text = datum.mapName,
                    Style = Resources["GridLabelStyle"] as Style
                };

                string zoneString = "";
                int zone = int.Parse(datum.zoneID);
                if (zone != 0) // if not main
                {
                    if (zone < 31) // stage
                    {
                        zoneString = " S" + zone;
                    }
                    else // bonus
                    {
                        zoneString = " B" + (zone - 30);
                    }
                }

                Label TimeLabel = new Label
                {
                    Text = "\t" + zoneString + " in " + Seconds_Formatter.toString_RankTime(datum.surfTime),
                    LineBreakMode = LineBreakMode.NoWrap,
                    FontSize = 14,
                    TextColor = Xamarin.Forms.Color.Gray
                };
                RRMapStack.Children.Add(MapLabel);
                RRMapStack.Children.Add(TimeLabel);

            }
        }

        private void LayoutRecentRecords10(string modeString)
        {
            RRTypeOptionLabel.Text = "[Type: Top10]";
            RRStyleOptionLabel.Text = "[Style: " + modeString + "]";

            foreach (RR10Datum datum in recentRecords10Data)
            {
                Label PlayerLabel = new Label
                {
                    Text = datum.playerName,
                    Style = Resources["GridLabelStyle"] as Style
                };
                Label BlankLabel = new Label()
                {
                    Text = " ",
                    FontSize = 14
                };
                RRPlayerStack.Children.Add(PlayerLabel);
                RRPlayerStack.Children.Add(BlankLabel);

                Label MapLabel = new Label
                {
                    Text = datum.mapName,
                    Style = Resources["GridLabelStyle"] as Style
                };
                
                int rank = int.Parse(datum.newRank);
                Label TimeLabel = new Label
                {
                    Text = "\tR" + rank + " in " + Seconds_Formatter.toString_RankTime(datum.surfTime),
                    LineBreakMode = LineBreakMode.NoWrap,
                    FontSize = 14,
                    TextColor = Xamarin.Forms.Color.Gray
                };
                RRMapStack.Children.Add(MapLabel);
                RRMapStack.Children.Add(TimeLabel);
            }
        }

        private void ClearRecentRecordsGrid()
        {
            RRPlayerStack.Children.Clear();
            RRPlayerStack.Children.Add(RRPlayerLabel);
            RRMapStack.Children.Clear();
            RRMapStack.Children.Add(RRMapLabel);
        }

        private void ChangeGameFilter(EFilter_Game newGame)
        {
            if (game == newGame) return;
            BaseViewModel.vibrate(allowVibrate);

            switch (game)
            {
                case EFilter_Game.css: GameCSSLabel.TextColor = Xamarin.Forms.Color.Gray; break;
                case EFilter_Game.css100t: GameCSS100TLabel.TextColor = Xamarin.Forms.Color.Gray; break;
                case EFilter_Game.csgo: GameCSGOLabel.TextColor = Xamarin.Forms.Color.Gray; break;
                default: break;
            }

            switch (newGame)
            {
                case EFilter_Game.css: GameCSSLabel.TextColor = Xamarin.Forms.Color.FromHex("147efb"); break;
                case EFilter_Game.css100t: GameCSS100TLabel.TextColor = Xamarin.Forms.Color.FromHex("147efb"); break;
                case EFilter_Game.csgo: GameCSGOLabel.TextColor = Xamarin.Forms.Color.FromHex("147efb"); break;
            }

            game = newGame;
            ChangeRecentRecords(game, recentRecordsType, recentRecordsMode);
            ChangeSurfTop(game, surfTopMode);
        }

        // Event Handlers --------------------------------------------------------------------------------------------------------------------------

        private void GameCSSLabel_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.css);

        private void GameCSS100TLabel_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.css100t);

        private void GameCSGOLabel_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.csgo);


        private readonly List<string> type_list = new List<string> { "Map", "Top10", "Stage", "Bonus", "All" };
        private readonly List<string> modes_list = new List<string> { "FW", "HSW", "SW", "BW" };

        private async void TopStyleOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> modes = new List<string>();
            string currentModeString = EFilter_ToString.toString(surfTopMode);
            foreach (string mode in modes_list)
            {
                if (mode != currentModeString)
                {
                    modes.Add(mode);
                }
            }

            string newStyle = await DisplayActionSheet("Choose a different style", "Cancel", null, modes[0], modes[1], modes[2]);

            EFilter_Mode newMode = EFilter_Mode.fw;
            switch (newStyle)
            {
                case "Cancel": return;
                case "HSW": newMode = EFilter_Mode.hsw; break;
                case "SW": newMode = EFilter_Mode.sw; break;
                case "BW": newMode = EFilter_Mode.bw; break;
            }

            surfTopData = RecordsViewModel.GetSurfTop(game, newMode)?.data;
            if (surfTopData is null) return;
            surfTopMode = newMode;

            ClearSurfTopGrid();
            LayoutSurfTop(newStyle);
        }

        private async void RRTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EFilter_ToString.toString2(recentRecordsType);
            foreach (string type in type_list)
            {
                if (type != currentTypeString)
                {
                    types.Add(type);
                }
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types[0], types[1], types[2], types[3]);

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
            foreach (string mode in modes_list)
            {
                if (mode != currentModeString)
                {
                    modes.Add(mode);
                }
            }

            string newStyle = await DisplayActionSheet("Choose a different style", "Cancel", null, modes[0], modes[1], modes[2]);

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
    }
}
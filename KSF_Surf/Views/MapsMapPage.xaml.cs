using KSF_Surf.Models;
using KSF_Surf.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;


namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsMapPage : ContentPage
    {
        // NOTE: Points values are truncated

        private readonly string map;
        private readonly EFilter_Game game;
        private EFilter_MapType mapType;
        private EFilter_Mode currentMode;
        private int currentZone;
        private string currentZoneString;

        private MapInfoData mapInfoData;
        private MapSettings mapSettings;
        private List<Mapper> mappers;

        private List<TopDatum> topData;

        private PointsData pointsData;

        private List<string> zonePickerList;

        public MapsMapPage(string mapName, EFilter_Game gameFilter)
        {
            map = mapName;
            game = gameFilter;
            currentMode = EFilter_Mode.fw;
            currentZone = 0;
            currentZoneString = "Main";
            zonePickerList = new List<string>() { "Main" };
            InitializeComponent();

            LoadMapInfo();
        }

        private void LoadMapInfo()
        {
            string gameString = " [";
            switch (game)
            {
                case EFilter_Game.css: gameString += "CS:S"; break;
                case EFilter_Game.css100t: gameString += "CS:S 100T"; break;
                case EFilter_Game.csgo: gameString += "CS:GO"; break;
            }
            MapsMap.Title = map + gameString + "]";

            // running query and setting member variables
            mapInfoData = MapsViewModel.GetMapInfo(game, map).data;
            if (mapInfoData is null) return;

            
            pointsData = MapsViewModel.GetMapPoints(game, map).data;
            if (pointsData is null) return;

            mapSettings = mapInfoData.MapSettings;
            mappers = mapInfoData.Mappers;
            try
            {
                mapType = (EFilter_MapType)int.Parse(mapSettings.maptype);
            }
            catch (FormatException)
            {
                Console.WriteLine("Problem parsing mapSettings.maptype field (MapsMapPage mapType)");
            }

            // filling in XAML
            LayoutGeneralMapInfo();
            LayoutMappers();
            LayoutStats();
            LayoutPoints();

            // setting zone options
            int stageCount = 0;
            int bonusCount = 0;
            try
            {
                stageCount = int.Parse(mapSettings.cp_count);
                bonusCount = int.Parse(mapSettings.b_count);
            }
            catch (FormatException)
            {
                Console.WriteLine("Problem parsing mapSettings.maptype field (MapsMapPage counts)");
            }

            if ((int)mapType == 1)
            {
                stageCount = 0;
            }
            else
            {
                for (int i = 1; i <= stageCount; i++)
                {
                    zonePickerList.Add("S" + i);
                }
            }
            for (int i = 1; i <= bonusCount; i++)
            {
                zonePickerList.Add("B" + i);
            }

            ZonePicker.ItemsSource = zonePickerList;

            topData = MapsViewModel.GetMapTop(game, map, currentMode, currentZone)?.data;
            if (topData is null) return;
            LayoutTop("FW", "Main");

        }

        private void LayoutGeneralMapInfo()
        {    
            switch (mapType)
            {
                case EFilter_MapType.linear: MapTypeLabel.Text = "CPs"; break;
                case EFilter_MapType.staged: MapTypeLabel.Text = "Stages"; break;
            }
            
            TierLabel.Text = mapSettings.tier;
            CheckpointsLabel.Text = mapSettings.cp_count;
            BonusesLabel.Text = mapSettings.b_count;
        }

        private void LayoutMappers()
        {
            if (mappers.Count == 0)
            {
                MappersLabel.IsVisible = false;
                MappersGrid.IsVisible = false;
                MappersNTopSeparator.IsVisible = false;
                return;
            }
            else
            {
                foreach (Mapper mapper in mappers)
                {
                    Label MapperTypeLabel = new Label
                    {
                        Text = mapper.typeName,
                        FontSize = 20,
                        TextColor = Xamarin.Forms.Color.Gray
                    };
                    MapperTypeStack.Children.Add(MapperTypeLabel);

                    Label MapperNameLabel = new Label
                    {
                        Text = mapper.mapperName,
                        FontSize = 20,
                        TextColor = Xamarin.Forms.Color.Black,
                        HorizontalTextAlignment = TextAlignment.End
                    };
                    MapperNameStack.Children.Add(MapperNameLabel);
                }
            }
        }

        private void LayoutStats()
        {
            CompletionsLabel.Text = pointsData.TotalPlayers;
            TimesPlayedLabel.Text = mapSettings.totalplaytimes;
            PlayTimeLabel.Text = Seconds_Formatter.toString_PlayTime(mapSettings.playtime);
        }

        private void LayoutTop(string mode, string zone)
        {
            StyleOptionLabel.Text = "[Style: " + mode + "]";
            ZoneOptionLabel.Text = "[Zone: " + zone + "]";

            int i = 1;
            foreach (TopDatum datum in topData)
            {
                Label RankLabel = new Label
                {
                    Text = i + ". " + datum.name,
                    LineBreakMode = LineBreakMode.NoWrap,
                    FontSize = 16,
                    TextColor = Xamarin.Forms.Color.Gray
                };
                TopRankStack.Children.Add(RankLabel);

                Label TimeLabel = new Label
                {
                    Text = Seconds_Formatter.toString_RankTime(datum.time),
                    LineBreakMode = LineBreakMode.NoWrap,
                    FontSize = 16,
                    TextColor = Xamarin.Forms.Color.Gray,
                };
                TopTimeStack.Children.Add(TimeLabel);

                Label DiffLabel = new Label();
                if (i == 1)
                {
                    DiffLabel.Text = "WR";
                }
                else
                {
                    DiffLabel.Text = "(+" + Seconds_Formatter.toString_RankTime(datum.wrDiff) + ")";
                }
                DiffLabel.LineBreakMode = LineBreakMode.NoWrap;
                DiffLabel.FontSize = 16;
                DiffLabel.TextColor = Xamarin.Forms.Color.Gray;

                TopDiffStack.Children.Add(DiffLabel);

                i++;
            }

        }

        private void LayoutPoints()
        {
            int i = 1;
            foreach (double points in pointsData.TopPoints)
            {
                if (points == 0) continue;

                Label RankLabel = new Label
                {
                    Text = (i != 1) ? "R" + i + ": " + ((int)points) : "R1: " + ((int)double.Parse(mapInfoData.MapSettings.wr_points_0)),
                    FontSize = 16,
                    TextColor = Xamarin.Forms.Color.Gray,
                    LineBreakMode = LineBreakMode.NoWrap
                };
                TopGroupStack.Children.Add(RankLabel);

                i++;
            }

            i = 0;
            int rank = 10;
            foreach (double points in pointsData.GroupPoints)
            {
                if (points == 0) continue;
                i++;

                int groupEnd = 10 + pointsData.GroupRanks[i];

                Label RankLabel = new Label
                {
                    Text = "G" + i + " (" + (rank + 1) + "-" + groupEnd + "): " + ((int)points),
                    FontSize = 16,
                    TextColor = Xamarin.Forms.Color.Gray,
                    LineBreakMode = LineBreakMode.NoWrap
                };
                GroupStack.Children.Add(RankLabel);

                rank = groupEnd;
            }
        }

        private void clearTopGrid()
        {
            TopRankStack.Children.Clear();
            TopRankStack.Children.Add(TopColLabel1);
            TopTimeStack.Children.Clear();
            TopTimeStack.Children.Add(TopColLabel2);
            TopDiffStack.Children.Clear();
            TopDiffStack.Children.Add(TopColLabel3);
        }

        // Event Handlers --------------------------------------------------------------------------------------------------------------------------

        private readonly List<string> modes_list = new List<string> { "FW", "HSW", "SW", "BW" };

        private async void StyleOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> modes = new List<string>();
            string currentModeString = EFilter_ToString.toString(currentMode);
            foreach (string mode in modes_list)
            {
                if (mode != currentModeString)
                {
                    modes.Add(mode);
                }
            }

            string newStyle = await DisplayActionSheet("Choose a different style", "Cancel", null, modes[0], modes[1], modes[2]);

            EFilter_Mode newCurrentMode = EFilter_Mode.fw;
            switch (newStyle)
            {
                case "Cancel": return;
                case "HSW": newCurrentMode = EFilter_Mode.hsw; break;
                case "SW": newCurrentMode = EFilter_Mode.sw; break;
                case "BW": newCurrentMode = EFilter_Mode.bw; break;
            }

            List<TopDatum> newTopData = MapsViewModel.GetMapTop(game, map, newCurrentMode, currentZone)?.data;
            if (newTopData is null) return;
            topData = newTopData;
            currentMode = newCurrentMode;

            clearTopGrid();
            LayoutTop(newStyle, currentZoneString);
        }

        
        private void ZoneOptionLabel_Tapped(object sender, EventArgs e)
        {
            ZonePicker.Focus();
            ZonePicker.SelectedItem = currentZoneString;
        }

        private void ZonePicker_Unfocused(object sender, FocusEventArgs e)
        {
            string selected = (string)ZonePicker.SelectedItem;
            if (selected == currentZoneString)
            {
                return;
            }

            int newZoneNum = -1; 
            switch (selected[0])
            {
                case 'M': newZoneNum = 0;  break;
                case 'S': newZoneNum = selected[1] - '0'; break;
                case 'B': newZoneNum = 30 + selected[1] - '0'; break;
            }

            if (newZoneNum != -1)
            {
                List<TopDatum> newTopData = MapsViewModel.GetMapTop(game, map, currentMode, newZoneNum)?.data;
                if (newTopData is null) return;
                topData = newTopData;
                currentZone = newZoneNum;
                currentZoneString = selected;

                clearTopGrid();
                LayoutTop(EFilter_ToString.toString(currentMode), currentZoneString);
            }
        }
    }

}
using System;
using System.Collections.Generic;
using System.ComponentModel;

using Xamarin.Forms;

using KSF_Surf.Models;
using KSF_Surf.ViewModels;


namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsMapPage : ContentPage
    {
        private readonly MapsViewModel mapsViewModel;

        // objects for map information
        private MapInfoData mapInfoData;
        private MapSettings mapSettings;
        private List<Mapper> mappers;

        private List<TopDatum> topData;

        private PointsData pointsData;

        // variables for this map
        private readonly string map;
        private readonly EFilter_Game game;
        private EFilter_MapType mapType;
        private EFilter_Mode currentMode;
        
        private int currentZone;
        private string currentZoneString;

        private List<string> zonePickerList;
        private int stageCount;
        private int bonusCount;

        public MapsMapPage(string mapName, EFilter_Game gameFilter)
        {
            mapsViewModel = new MapsViewModel();
            
            map = mapName;
            game = gameFilter;
            currentMode = EFilter_Mode.fw;
            
            currentZone = 0;
            currentZoneString = "Main";
            zonePickerList = new List<string>() { "Main" };
            stageCount = 0;
            bonusCount = 0;

            InitializeComponent();

            LoadMapInfo();
        }

        // UI --------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private void LoadMapInfo()
        {
            MapsMap.Title = map + " [" + EFilter_ToString.toString2(game) + "]";

            // running query and assigning to map information objects
            mapInfoData = mapsViewModel.GetMapInfo(game, map)?.data;
            if (mapInfoData is null) return;

            
            pointsData = mapsViewModel.GetMapPoints(game, map)?.data;
            if (pointsData is null) return;

            mapSettings = mapInfoData.MapSettings;
            mappers = mapInfoData.Mappers;
            mapType = (EFilter_MapType)int.Parse(mapSettings.maptype);

            // filling in UI and setting zone options
            LayoutGeneralMapInfo();
            LayoutMappers();
            LayoutStats();

            stageCount = int.Parse(mapSettings.cp_count);
            bonusCount = int.Parse(mapSettings.b_count);
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

            LayoutPoints();
            ZonePicker.ItemsSource = zonePickerList;

            topData = mapsViewModel.GetMapTop(game, map, currentMode, currentZone)?.data;
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
                    MapperTypeStack.Children.Add(new Label {
                        Text = mapper.typeName,
                        Style = App.Current.Resources["LeftColStyle"] as Style
                    });
                    MapperNameStack.Children.Add(new Label
                    {
                        Text = mapper.mapperName,
                        Style = App.Current.Resources["RightColStyle"] as Style
                    });
                }
            }
        }

        private void LayoutStats()
        {
            CompletionsLabel.Text = String_Formatter.toString_Int(pointsData.TotalPlayers);
            TimesPlayedLabel.Text = String_Formatter.toString_Int(mapSettings.totalplaytimes);
            PlayTimeLabel.Text = String_Formatter.toString_PlayTime(mapSettings.playtime, true);
        }

        private void LayoutTop(string modeString, string zone)
        {
            StyleOptionLabel.Text = "[Style: " + modeString + "]";
            ZoneOptionLabel.Text = "[Zone: " + zone + "]";

            int i = 1;
            foreach (TopDatum datum in topData)
            {
                TopRankStack.Children.Add(new Label {
                    Text = i + ". " + String_Formatter.toEmoji_Country(datum.country) + " " + datum.name + " (" + datum.count + ")",
                    Style = Resources["TopNPointsStyle"] as Style
                });

                Label TimeLabel = new Label
                {
                    Text = String_Formatter.toString_RankTime(datum.time),
                    Style = Resources["TopNPointsStyle"] as Style
                };
                if (i != 1)
                {
                    TimeLabel.Text += " (+" + String_Formatter.toString_RankTime(datum.wrDiff) + ")";
                }
                TopTimeStack.Children.Add(TimeLabel);

                i++;
            }

        }

        private void LayoutPoints()
        {

            CompletionStack.Children.Add( new Label {
                Text = "Map Completion: " + String_Formatter.toString_Points(mapSettings.map_finish),
                Style = Resources["TopNPointsStyle"] as Style
            });

            if (stageCount > 0)
            {
                CompletionStack.Children.Add(new Label {
                    Text = "Stage Completion: " + String_Formatter.toString_Points(mapSettings.stage_finish),
                    Style = Resources["TopNPointsStyle"] as Style
                });
            }

            if (bonusCount > 0)
            {
                CompletionStack.Children.Add(new Label {
                    Text = "Bonus Completion: " + String_Formatter.toString_Points(mapSettings.bonus_finish),
                    Style = Resources["TopNPointsStyle"] as Style
                });
            }


            int i = 1;
            foreach (double points in pointsData.TopPoints)
            {
                if (points == 0) continue;

                TopGroupStack.Children.Add(new Label {
                    Text = (i != 1) ? "R" + i + ": " + String_Formatter.toString_Points(points) : "R1: " + String_Formatter.toString_Points(mapInfoData.MapSettings.wr_points_0),
                    Style = Resources["TopNPointsStyle"] as Style
                });

                i++;
            }

            i = 0;
            int rank = 10;
            foreach (double points in pointsData.GroupPoints)
            {
                if (points == 0) continue;
                i++;

                int groupEnd = 10 + pointsData.GroupRanks[i];

                GroupStack.Children.Add(new Label {
                    Text = "G" + i + " (" + (rank + 1) + "-" + groupEnd + "): " + String_Formatter.toString_Points(points),
                    Style = Resources["TopNPointsStyle"] as Style
                });

                rank = groupEnd;
            }
        }

        private void ClearTopGrid()
        {
            TopRankStack.Children.Clear();
            TopRankStack.Children.Add(TopColLabel1);
            TopTimeStack.Children.Clear();
            TopTimeStack.Children.Add(TopColLabel2);
        }

        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

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

            List<TopDatum> newTopData = mapsViewModel.GetMapTop(game, map, newCurrentMode, currentZone)?.data;
            if (newTopData is null) return;
            topData = newTopData;
            currentMode = newCurrentMode;

            ClearTopGrid();
            LayoutTop(newStyle, currentZoneString);
        }

        
        private void ZoneOptionLabel_Tapped(object sender, EventArgs e)
        {
            ZonePicker.SelectedItem = currentZoneString;
            ZonePicker.Focus();
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
                List<TopDatum> newTopData = mapsViewModel.GetMapTop(game, map, currentMode, newZoneNum)?.data;
                if (newTopData is null) return;
                
                topData = newTopData;
                currentZone = newZoneNum;
                currentZoneString = selected;

                ClearTopGrid();
                LayoutTop(EFilter_ToString.toString(currentMode), currentZoneString);
            }
        }
    }

    #endregion
}
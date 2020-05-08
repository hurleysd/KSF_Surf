using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.Models;
using KSF_Surf.ViewModels;


namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsMapPage : ContentPage
    {
        private readonly MapsViewModel mapsViewModel;
        private bool hasLoaded = false;

        // objects for map information
        private MapInfoData mapInfoData;
        private MapSettings mapSettings;
        private List<Mapper> mappers;

        private PointsData pointsData;

        // variables for this map
        private readonly string map;
        private readonly EFilter_Game game;
        private EFilter_MapType mapType;

        private List<string> zonePickerList;
        private int stageCount;
        private int bonusCount;

        public MapsMapPage(string mapName, EFilter_Game gameFilter)
        {
            mapsViewModel = new MapsViewModel();
            
            map = mapName;
            game = gameFilter;

            zonePickerList = new List<string>() { "Main" };
            stageCount = 0;
            bonusCount = 0;

            InitializeComponent();
        }

        // UI --------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private async Task LoadMapInfo()
        {
            MapsMap.Title = map + " [" + EFilter_ToString.toString2(game) + "]";

            // running query and assigning to map information objects
            var mapInfoDatum = await mapsViewModel.GetMapInfo(game, map);
            mapInfoData = mapInfoDatum?.data;
            if (mapInfoData is null) return;

            var mapPointsDatum = await mapsViewModel.GetMapPoints(game, map);
            pointsData = mapPointsDatum?.data;
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

        private void LayoutPoints()
        {
            int fontsize = 16;

            CompletionStack.Children.Add( new Label {
                Text = "Map",
                Style = Resources["PointsStyle"] as Style,
                FontSize = fontsize
            });

            CompletionValueStack.Children.Add(new Label
            {
                Text = String_Formatter.toString_Points(mapSettings.map_finish),
                Style = App.Current.Resources["RightColStyle"] as Style,
                FontSize = fontsize
            });

            if (stageCount > 0)
            {
                CompletionStack.Children.Add( new Label {
                Text = "Stage",
                Style = Resources["PointsStyle"] as Style
                });

                CompletionValueStack.Children.Add(new Label
                {
                    Text = String_Formatter.toString_Points(mapSettings.stage_finish),
                    Style = App.Current.Resources["RightColStyle"] as Style,
                    FontSize = fontsize
                });
            }

            if (bonusCount > 0)
            {
                CompletionStack.Children.Add(new Label {
                    Text = "Bonus",
                    Style = Resources["PointsStyle"] as Style,
                    FontSize = fontsize
                });

                CompletionValueStack.Children.Add(new Label
                {
                    Text = String_Formatter.toString_Points(mapSettings.bonus_finish),
                    Style = App.Current.Resources["RightColStyle"] as Style,
                    FontSize = fontsize
                });
            }


            int i = 1;
            foreach (double points in pointsData.TopPoints)
            {
                if (points == 0) continue;

                TopGroupStack.Children.Add(new Label {
                    Text = (i != 1) ? "R" + i : "WR",
                    Style = Resources["PointsStyle"] as Style,
                    FontSize = fontsize
                });

                TopGroupValueStack.Children.Add(new Label
                {
                    Text = (i != 1) ? String_Formatter.toString_Points(points) : String_Formatter.toString_Points(mapInfoData.MapSettings.wr_points_0),
                    Style = App.Current.Resources["RightColStyle"] as Style,
                    FontSize = fontsize
                }); ;

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
                    Text = "G" + i + " (" + String_Formatter.toString_Points(rank + 1) + "-" + String_Formatter.toString_Points(groupEnd) + ")",
                    Style = Resources["PointsStyle"] as Style,
                    FontSize = fontsize
                });

                GroupValueStack.Children.Add(new Label
                {
                    Text = String_Formatter.toString_Points(points),
                    Style = App.Current.Resources["RightColStyle"] as Style,
                    FontSize = fontsize
                }); ;

                rank = groupEnd;
            }
        }

        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                await LoadMapInfo();
                hasLoaded = true;
            }
        }

        private async void Top_Tapped(object sender, EventArgs e)
        {
            TopButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new MapsMapTopPage(Title, mapsViewModel, game, map, stageCount, bonusCount, zonePickerList));
            }
            else
            {
                await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
            }
            TopButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void PR_Tapped(object sender, EventArgs e)
        {
            PRButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new MapsMapPRPage(Title.Replace(']', ','), mapsViewModel, game, map, 
                    (stageCount + bonusCount > 0), (mapType == EFilter_MapType.staged)));
            }
            else
            {
                await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
            }
            PRButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }
    }

    #endregion
}
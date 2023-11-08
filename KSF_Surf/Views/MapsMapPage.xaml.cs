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
        private MapSettingsDatum mapSettings;
        private List<MapperDatum> mappers;

        private PointsData pointsData;

        // variables for this map
        private readonly string map;
        private readonly GameEnum game;
        private MapTypeEnum mapType;

        private List<string> zonePickerList;
        private int stageCount;
        private int bonusCount;

        public MapsMapPage(string mapName, GameEnum game)
        {
            map = mapName;
            this.game = game;

            mapsViewModel = new MapsViewModel();
            zonePickerList = new List<string>() { "Main" };
            stageCount = 0;
            bonusCount = 0;

            InitializeComponent();
        }

        // UI --------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private async Task LoadMapInfo()
        {
            string mapName = map;
            if (mapName.Length > 20) mapName = mapName.Substring(0, 17) + "...";
            MapsMap.Title = mapName + " [" + EnumToString.NameString(game) + "]";

            // running query and assigning to map information objects
            var mapInfoDatum = await mapsViewModel.GetMapInfo(game, map);
            mapInfoData = mapInfoDatum?.data;
            if (mapInfoData is null) return;

            var mapPointsDatum = await mapsViewModel.GetMapPoints(game, map);
            pointsData = mapPointsDatum?.data;
            if (pointsData is null) return;

            mapSettings = mapInfoData.MapSettings;
            mappers = mapInfoData.Mappers;
            mapType = (MapTypeEnum)int.Parse(mapSettings.maptype);

            // filling in UI and setting zone options
            LayoutGeneralMapInfo();
            LayoutMappers();
            LayoutStats();

            stageCount = int.Parse(mapSettings.cp_count);
            bonusCount = int.Parse(mapSettings.b_count);

            if ((int)mapType == 1) stageCount = 0;
            else
            {
                for (int i = 1; i <= stageCount; i++) zonePickerList.Add("S" + i);
            }

            for (int i = 1; i <= bonusCount; i++) zonePickerList.Add("B" + i);

            LayoutPoints();
        }

        private void LayoutGeneralMapInfo()
        {    
            switch (mapType)
            {
                case MapTypeEnum.LINEAR: MapTypeLabel.Text = "CPs"; break;
                case MapTypeEnum.STAGED: MapTypeLabel.Text = "Stages"; break;
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
                foreach (MapperDatum mapper in mappers)
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
            CreatedLabel.Text = StringFormatter.KSFDateString(mapSettings.created);
            CompletionsLabel.Text = StringFormatter.IntString(pointsData.TotalPlayers);
            TimesPlayedLabel.Text = StringFormatter.IntString(mapSettings.totalplaytimes);
            PlayTimeLabel.Text = StringFormatter.PlayTimeString(mapSettings.playtime, true);
        }
        private void LayoutPoints()
        {
            int fontsize = 16;

            CompletionStack.Children.Add(new Label
            {
                Text = "Map",
                Style = App.Current.Resources["PointsStyle"] as Style,
                FontSize = fontsize
            });

            CompletionValueStack.Children.Add(new Label
            {
                Text = StringFormatter.PointsString(mapSettings.map_finish),
                Style = App.Current.Resources["RightColStyle"] as Style,
                FontSize = fontsize
            });

            if (stageCount > 0)
            {
                CompletionStack.Children.Add(new Label
                {
                    Text = "Stage",
                    Style = App.Current.Resources["PointsStyle"] as Style
                });

                CompletionValueStack.Children.Add(new Label
                {
                    Text = StringFormatter.PointsString(mapSettings.stage_finish),
                    Style = App.Current.Resources["RightColStyle"] as Style,
                    FontSize = fontsize
                });
            }

            if (bonusCount > 0)
            {
                CompletionStack.Children.Add(new Label
                {
                    Text = "Bonus",
                    Style = App.Current.Resources["PointsStyle"] as Style,
                    FontSize = fontsize
                });

                CompletionValueStack.Children.Add(new Label
                {
                    Text = StringFormatter.PointsString(mapSettings.bonus_finish),
                    Style = App.Current.Resources["RightColStyle"] as Style,
                    FontSize = fontsize
                });
            }


            int i = 1;
            foreach (double points in pointsData.TopPoints)
            {
                if (points == 0) continue;

                TopGroupStack.Children.Add(new Label
                {
                    Text = (i != 1) ? "R" + i : "WR",
                    Style = App.Current.Resources["PointsStyle"] as Style,
                    FontSize = fontsize
                });

                TopGroupValueStack.Children.Add(new Label
                {
                    Text = StringFormatter.PointsString(points),
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

                GroupStack.Children.Add(new Label
                {
                    Text = "G" + i + " (" + StringFormatter.PointsString(rank + 1) + "-" + StringFormatter.PointsString(groupEnd) + ")",
                    Style = App.Current.Resources["PointsStyle"] as Style,
                    FontSize = fontsize
                });

                GroupValueStack.Children.Add(new Label
                {
                    Text = StringFormatter.PointsString(points),
                    Style = App.Current.Resources["RightColStyle"] as Style,
                    FontSize = fontsize
                });

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
                hasLoaded = true;
                await LoadMapInfo();

                LoadingAnimation.IsRunning = false;
                MapsMapScrollView.IsVisible = true;
            }
        }

        private async void Top_Tapped(object sender, EventArgs e)
        {
            TopButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new MapsMapTopPage(Title, game, map, stageCount, bonusCount, zonePickerList));
            }
            else await DisplayNoConnectionAlert();

            TopButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void PR_Tapped(object sender, EventArgs e)
        {
            PRButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                string mapName = map;
                if (mapName.Length > 16) mapName = mapName.Substring(0, 13) + "...";
                string prPageTitle = mapName + " [" + EnumToString.NameString(game) + ",";

                await Navigation.PushAsync(new MapsMapPRPage(prPageTitle, game, map, 
                    (stageCount + bonusCount > 0), (mapType == MapTypeEnum.STAGED)));
            }
            else await DisplayNoConnectionAlert();

            PRButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async Task DisplayNoConnectionAlert()
        {
            await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }
    }

    #endregion
}
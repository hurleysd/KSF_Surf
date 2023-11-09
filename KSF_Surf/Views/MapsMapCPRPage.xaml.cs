using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using SkiaSharp;
using Microcharts;
using KSF_Surf.ViewModels;
using KSF_Surf.Models;
using System;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsMapCPRPage : ContentPage
    {
        private readonly MapsViewModel mapsViewModel;
        private bool hasLoaded = false;

        // objects used by "Compare to World Record" call
        private List<MapComparePersonalRecordDetails> CPRDetails;

        // variables for filters
        private readonly GameEnum game;
        private readonly ModeEnum mode;
        private readonly string map;
        private MapTypeEnum mapType = MapTypeEnum.NODE;
        private readonly string playerSteamID;

        public MapsMapCPRPage(string title, GameEnum game, ModeEnum mode, string map, string playerSteamID)
        {
            this.game = game;
            this.mode = mode;
            this.map = map;
            this.playerSteamID = playerSteamID;

            mapsViewModel = new MapsViewModel();

            InitializeComponent();
            Title = title;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeCPRDetails()
        {
            var cprDatum = await mapsViewModel.GetMapCPR(game, mode, map, PlayerTypeEnum.STEAM_ID, playerSteamID);
            CPRDetails = cprDatum?.data.CPR;
            if (CPRDetails is null || CPRDetails.Count < 1) return;

            mapType = (MapTypeEnum)int.Parse(cprDatum.data.mapType);

            WRPlayer.Text = "CPR vs " + StringFormatter.CountryEmoji(cprDatum.data.basicInfoWR.country) + " " + cprDatum.data.basicInfoWR.name;
            LayoutCPRDetails();
            LayoutCPRChart();
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutCPRDetails()
        {
            int fontsize = 18;
            string units = " u/s";
            int i = 0;

            foreach (MapComparePersonalRecordDetails zoneCPR in CPRDetails)
            {
                CPRStack.Children.Add(new Label
                {
                    Text = StringFormatter.CPRZoneString(zoneCPR.zoneID, mapType),
                    Style = App.Current.Resources["HeaderLabel"] as Style,
                    Margin = new Thickness(10, 0, 0, 0)
                });

                Grid recordGrid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = 102 },
                        new ColumnDefinition { Width = 92 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                    Style = App.Current.Resources["ColumnGridStyle"] as Style
                };

                int playerVel = (int)double.Parse(zoneCPR.playerTouchVel);
                int wrVel = (int)double.Parse(zoneCPR.WRTouchVel);
                int diffVel = (int)double.Parse(zoneCPR.velDiff);
                
                recordGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = playerVel + units,
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = fontsize
                        },
                        new Label {
                            Text = StringFormatter.RankTimeString(zoneCPR.playerTime),
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = fontsize,
                            IsVisible = i != 0
                        }
                    }
                }, 0, 0);

                recordGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = wrVel + units,
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = fontsize
                        },
                        new Label {
                            Text = StringFormatter.RankTimeString(zoneCPR.WRTime),
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = fontsize,
                            IsVisible = i != 0
                        }
                    }
                }, 1, 0);

                string velPrefix = (diffVel > 0)? "+" : "";
                string timePrefix = "+";
                if (zoneCPR.timeDiff.Contains("-"))
                {
                    zoneCPR.timeDiff = zoneCPR.timeDiff.Replace("-", ""); 
                    timePrefix = "-";
                }
                
                recordGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = "[" + velPrefix + diffVel + units + "]",
                            Style = App.Current.Resources["RightColStyle"] as Style,
                            FontSize = fontsize
                        },
                        new Label {
                            Text = "(" + timePrefix + StringFormatter.RankTimeString(zoneCPR.timeDiff) + ")",
                            Style = App.Current.Resources["RightColStyle"] as Style,
                            FontSize = fontsize,
                            IsVisible = i != 0
                        }
                    }
                }, 2, 0);

                CPRStack.Children.Add(recordGrid);

                if (++i != CPRDetails.Count)
                {
                    CPRStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["Separator2Style"] as Style
                    });
                }
            }               
        }

        private void LayoutCPRChart()
        {
            // parse out time diffs for colors
            List<float> timeDiffs = new List<float>();
            float maxTimeDiff = float.MinValue;
            float minTimeDiff = float.MaxValue;

            foreach (MapComparePersonalRecordDetails zoneCPR in CPRDetails)
            {
                float timeDiff = float.Parse(zoneCPR.timeDiff);
                timeDiffs.Add(timeDiff);

                if (timeDiff >= maxTimeDiff) maxTimeDiff = timeDiff;
                if (timeDiff <= minTimeDiff) minTimeDiff = timeDiff;
            }

            List<ChartEntry> chartEntries = new List<ChartEntry>();

            int i = 0;
            foreach (MapComparePersonalRecordDetails zoneCPR in CPRDetails)
            {
                byte rColorDiff = 0;
                byte gColorDiff = 0;
                byte bColorDiff = 0;

                if (timeDiffs[i] > 0) // gradient to red
                {
                    byte gbColorDiff = (byte)((int)(timeDiffs[i] / maxTimeDiff * 255));
                    gColorDiff = gbColorDiff;
                    bColorDiff = gbColorDiff;
                }
                else if (timeDiffs[i] < 0) // gradient to green
                {
                    byte rbColorDiff = (byte)((int)(timeDiffs[i] / minTimeDiff * 255));
                    rColorDiff = rbColorDiff;
                    bColorDiff = rbColorDiff;
                }

                ChartEntry entry = new ChartEntry(timeDiffs[i])
                {
                    Color = new SKColor((byte)(255 - rColorDiff), (byte)(255 - gColorDiff), (byte)(255 - bColorDiff)),
                    Label = StringFormatter.ZoneChartString(zoneCPR.zoneID, mapType, RecordComparisonTypeEnum.CPR),
                    ValueLabel = " " // if value label is empty, label won't show for some reason 
                };

                chartEntries.Add(entry);
                i++;
            }

            // chart background won't react to live theme changes
            bool hasLightBackground = (DeviceInfo.Platform == DevicePlatform.iOS && AppInfo.RequestedTheme == AppTheme.Light);
            CPRChart.Chart = new LineChart { 
                Entries = chartEntries,
                MaxValue = maxTimeDiff,
                MinValue = minTimeDiff,
                LineMode = LineMode.Straight,
                BackgroundColor = hasLightBackground ? SKColor.Parse("#e8e8e8") : SKColor.Parse("#171717"),
                Margin = 20,
                LabelTextSize = 32,
                LabelColor = hasLightBackground ? SKColors.Black : SKColors.White,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                PointSize = 15
            };
        }

        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;
                await ChangeCPRDetails();

                LoadingAnimation.IsRunning = false;
                MapsMapCPRScrollView.IsVisible = true;
            }
        }

        #endregion
    }
}
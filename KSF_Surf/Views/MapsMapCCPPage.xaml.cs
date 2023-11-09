using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using SkiaSharp;
using Microcharts;
using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsMapCCPPage : ContentPage
    {
        private readonly MapsViewModel mapsViewModel;
        private bool hasLoaded = false;

        // objects used by "Compare to World Record by Checkpoints" call
        private List<MapComapreCheckPointsDetails> CCPDetails;

        // variables for filters
        private readonly GameEnum game;
        private readonly ModeEnum mode;
        private readonly string map;
        private readonly string playerSteamID;

        public MapsMapCCPPage(string title, GameEnum game, ModeEnum mode, string map, string playerSteamID)
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

        private async Task ChangeCCPDetails()
        {
            var ccpDatum = await mapsViewModel.GetMapCCP(game, mode, map, PlayerTypeEnum.STEAM_ID, playerSteamID);
            CCPDetails = ccpDatum?.data.CCP;
            if (CCPDetails is null || CCPDetails.Count < 1) return;

            WRPlayer.Text = "CCP vs " + StringFormatter.CountryEmoji(ccpDatum.data.basicInfoWR.country) + " " + ccpDatum.data.basicInfoWR.name;
            LayoutCCPDetails();
            LayoutCCPChart();
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutCCPDetails()
        {
            int fontsize = 18;
            string units = " u/s";
            int i = 0;

            foreach (MapComapreCheckPointsDetails zoneCCP in CCPDetails)
            {
                CCPStack.Children.Add(new Label
                {
                    Text = StringFormatter.ZoneString(zoneCCP.zoneID, true, true),
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

                int playerVel = (int)double.Parse(zoneCCP.avgVelPlayer);
                int wrVel = (int)double.Parse(zoneCCP.avgVelWR);
                int diffVel = playerVel - wrVel;
                
                recordGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = StringFormatter.RankTimeString(zoneCCP.cpTimePlayer),
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = fontsize,
                        },
                        new Label {
                            Text = playerVel + units,
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = fontsize
                        }
                    }
                }, 0, 0);

                recordGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = StringFormatter.RankTimeString(zoneCCP.cpTimeWR),
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = fontsize,
                        },
                        new Label {
                            Text = wrVel + units,
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = fontsize
                        }
                    }
                }, 1, 0);

                double playerTime = double.Parse(zoneCCP.cpTimePlayer);
                double wrTime = double.Parse(zoneCCP.cpTimeWR);
                double timeDiff = playerTime - wrTime;

                string timePrefix = (timeDiff > 0) ? "+" : "";
                string velPrefix = (diffVel > 0) ? "+" : "";

                string timeString = StringFormatter.RankTimeString(timeDiff.ToString());
                if (timeString.Contains("-"))
                {
                    timeString = timeString.Replace("-", "");
                    timePrefix = "-";
                }

                recordGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = "(" + timePrefix + timeString + ")",
                            Style = App.Current.Resources["RightColStyle"] as Style,
                            FontSize = fontsize
                        },
                        new Label {
                            Text = "[" + velPrefix + diffVel + units + "]",
                            Style = App.Current.Resources["RightColStyle"] as Style,
                            FontSize = fontsize
                        }
                    }
                }, 2, 0);

                CCPStack.Children.Add(recordGrid);

                if (++i != CCPDetails.Count)
                {
                    CCPStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["Separator2Style"] as Style
                    });
                }
            }     
        }

        private void LayoutCCPChart()
        {
            // parse out time diffs for colors
            List<float> timeDiffs = new List<float>();
            float maxTimeDiff = float.MinValue;
            float minTimeDiff = float.MaxValue;

            foreach (MapComapreCheckPointsDetails zoneCCP in CCPDetails)
            {
                float playerTime = float.Parse(zoneCCP.cpTimePlayer);
                float wrTime = float.Parse(zoneCCP.cpTimeWR);
                float timeDiff = playerTime - wrTime;
                timeDiffs.Add(timeDiff);

                if (timeDiff >= maxTimeDiff) maxTimeDiff = timeDiff;
                if (timeDiff <= minTimeDiff) minTimeDiff = timeDiff;
            }

            List<ChartEntry> chartEntries = new List<ChartEntry>();

            int i = 0;
            foreach (MapComapreCheckPointsDetails zoneCCP in CCPDetails)
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
                    Label = StringFormatter.ZoneChartString(zoneCCP.zoneID, MapTypeEnum.STAGED, RecordComparisonTypeEnum.CCP),
                    ValueLabel = " " // if value label is empty, label won't show for some reason 
                };

                chartEntries.Add(entry);
                i++;
            }

            // chart background won't react to live theme changes
            bool hasLightBackground = (DeviceInfo.Platform == DevicePlatform.iOS && AppInfo.RequestedTheme == AppTheme.Light);
            CCPChart.Chart = new LineChart {
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
                await ChangeCCPDetails();

                LoadingAnimation.IsRunning = false;
                MapsMapCCPScrollView.IsVisible = true;
            }
        }

        #endregion
    }
}
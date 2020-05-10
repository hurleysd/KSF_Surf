using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsMapCPRPage : ContentPage
    {
        private readonly MapsViewModel mapsViewModel;
        private bool hasLoaded = false;
        private readonly int GRIDFONT = 18;

        // objects used by "Compare to World Record" call
        private List<MapCPRDetails> CPRDetails;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly string map;
        private EFilter_MapType mapType = EFilter_MapType.none;
        private readonly string playerSteamID;

        public MapsMapCPRPage(string title, MapsViewModel mapsViewModel, EFilter_Game game, EFilter_Mode mode, 
            string map, string playerSteamID)
        {
            this.mapsViewModel = mapsViewModel;
            this.game = game;
            this.mode = mode;
            this.map = map;
            this.playerSteamID = playerSteamID;

            InitializeComponent();
            Title = title;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangePRDetails()
        {
            var cprDatum = await mapsViewModel.GetMapCPR(game, mode, map, EFilter_PlayerType.steamid, playerSteamID);
            CPRDetails = cprDatum?.data.CPR;
            if (CPRDetails is null || CPRDetails.Count < 1) return;

            mapType = (EFilter_MapType)int.Parse(cprDatum.data.mapType);

            WRPlayer.Text = "PR vs " + String_Formatter.toEmoji_Country(cprDatum.data.basicInfoWR.country) + " " + cprDatum.data.basicInfoWR.name;
            LayoutPRDetails();
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutPRDetails()
        {
            string units = " u/s";
            int i = 0;

            foreach (MapCPRDetails zoneCPR in CPRDetails)
            {
                CPRStack.Children.Add(new Label
                {
                    Text = EFilter_ToString.CPRZoneFormatter(zoneCPR.zoneID, mapType),
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
                            Text = String_Formatter.toString_RankTime(zoneCPR.playerTime),
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = GRIDFONT,
                            IsVisible = i != 0
                        },
                        new Label {
                            Text = playerVel + units,
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = GRIDFONT
                        },
                    }
                }, 0, 0);

                recordGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = String_Formatter.toString_RankTime(zoneCPR.WRTime),
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = GRIDFONT,
                            IsVisible = i != 0
                        },
                        new Label {
                            Text = wrVel + units,
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = GRIDFONT
                        },
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
                            Text = "(" + timePrefix + String_Formatter.toString_RankTime(zoneCPR.timeDiff) + ")",
                            Style = App.Current.Resources["RightColStyle"] as Style,
                            FontSize = GRIDFONT,
                            IsVisible = i != 0
                        },
                        new Label {
                            Text = "[" + velPrefix + diffVel + units + "]",
                            Style = App.Current.Resources["RightColStyle"] as Style,
                            FontSize = GRIDFONT
                        },
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

        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                await ChangePRDetails();

                LoadingAnimation.IsRunning = false;
                MapsMapCPRScrollView.IsVisible = true;
                hasLoaded = true;
            }
        }

        #endregion
    }
}
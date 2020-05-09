using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsMapCCPPage : ContentPage
    {
        private readonly MapsViewModel mapsViewModel;
        private bool hasLoaded = false;
        private readonly int GRIDFONT = 18;

        // objects used by "Compare to World Record by Checkpoints" call
        private List<MapCCPDetails> CCPDetails;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly string map;
        private readonly string playerSteamID;

        public MapsMapCCPPage(string title, MapsViewModel mapsViewModel, EFilter_Game game, EFilter_Mode mode, 
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
            var ccpDatum = await mapsViewModel.GetMapCCP(game, mode, map, EFilter_PlayerType.steamid, playerSteamID);
            CCPDetails = ccpDatum?.data.CCP;
            if (CCPDetails is null || CCPDetails.Count < 1) return;

            WRPlayer.Text = "PRCP vs " + String_Formatter.toEmoji_Country(ccpDatum.data.basicInfoWR.country) + " " + ccpDatum.data.basicInfoWR.name;
            LayoutPRDetails();
        }

        // Dispaying Changes -------------------------------------------------------------------------------

        private void LayoutPRDetails()
        {
            string units = " u/s";
            int i = 0;

            foreach (MapCCPDetails zoneCCP in CCPDetails)
            {
                CCPStack.Children.Add(new Label
                {
                    Text = EFilter_ToString.zoneFormatter(zoneCCP.zoneID, true, true),
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
                            Text = String_Formatter.toString_RankTime(zoneCCP.cpTimePlayer),
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = GRIDFONT,
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
                            Text = String_Formatter.toString_RankTime(zoneCCP.cpTimeWR),
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = GRIDFONT,
                        },
                        new Label {
                            Text = wrVel + units,
                            Style = App.Current.Resources["LeftColStyle"] as Style,
                            FontSize = GRIDFONT
                        },
                    }
                }, 1, 0);

                double playerTime = double.Parse(zoneCCP.cpTimePlayer);
                double wrTime = double.Parse(zoneCCP.cpTimeWR);
                double timeDiff = playerTime - wrTime;

                string timePrefix = (timeDiff > 0) ? "+" : "";
                string velPrefix = (diffVel > 0) ? "+" : "";

                string timeString = String_Formatter.toString_RankTime(timeDiff.ToString());
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
                            FontSize = GRIDFONT
                        },
                        new Label {
                            Text = "[" + velPrefix + diffVel + units + "]",
                            Style = App.Current.Resources["RightColStyle"] as Style,
                            FontSize = GRIDFONT
                        },
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

        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                await ChangePRDetails();

                LoadingAnimation.IsRunning = false;
                MapsMapCCPScrollView.IsVisible = true;
                hasLoaded = true;
            }
        }

        #endregion
    }
}
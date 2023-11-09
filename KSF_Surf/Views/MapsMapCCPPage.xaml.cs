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

        private async Task ChangePRDetails()
        {
            var ccpDatum = await mapsViewModel.GetMapCCP(game, mode, map, PlayerTypeEnum.STEAM_ID, playerSteamID);
            CCPDetails = ccpDatum?.data.CCP;
            if (CCPDetails is null || CCPDetails.Count < 1) return;

            WRPlayer.Text = "CCP vs " + StringFormatter.CountryEmoji(ccpDatum.data.basicInfoWR.country) + " " + ccpDatum.data.basicInfoWR.name;
            LayoutPRDetails();
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutPRDetails()
        {
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
                            Text = StringFormatter.RankTimeString(zoneCCP.cpTimeWR),
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
                hasLoaded = true;
                await ChangePRDetails();

                LoadingAnimation.IsRunning = false;
                MapsMapCCPScrollView.IsVisible = true;
            }
        }

        #endregion
    }
}
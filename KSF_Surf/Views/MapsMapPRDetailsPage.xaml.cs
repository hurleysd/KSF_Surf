﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsMapPRDetailsPage : ContentPage
    {
        private readonly MapsViewModel mapsViewModel;
        private bool hasLoaded = false;

        // objects used by "Personal Zone Records" call
        private List<MapPersonalRecordDetails> mapPRDetails;

        // variables for filters
        private readonly GameEnum game;
        private readonly ModeEnum mode;
        private readonly string map;
        
        private string playerSteamID;

        public MapsMapPRDetailsPage(string title, GameEnum game, ModeEnum mode, ModeEnum defaultMode, string map, string playerSteamID)
        {
            this.game = game;
            this.mode = (mode == ModeEnum.NONE)? defaultMode : mode;
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
            var prDatum = await mapsViewModel.GetMapPR(game, mode, map, PlayerTypeEnum.STEAM_ID, playerSteamID);
            mapPRDetails = prDatum?.data.PRInfo;
            if (mapPRDetails is null) return;

            PRTitleLabel.Text = StringFormatter.CountryEmoji(prDatum.data.basicInfo.country) + " " + prDatum.data.basicInfo.name;
            LayoutPRDetails();
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutPRDetails()
        {
            string units = " u/s";
            int i = 0;

            foreach (MapPersonalRecordDetails zonePR in mapPRDetails)
            {
                if (zonePR.zoneID == "0") continue;
                bool noTime = (zonePR.surfTime is null || zonePR.surfTime == "0");

                if (i != 0)
                {
                    ZoneRecordStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["Separator2Style"] as Style
                    });
                }

                ZoneRecordStack.Children.Add(new Label
                {
                    Text = StringFormatter.ZoneString(zonePR.zoneID, false, true),
                    Style = App.Current.Resources["HeaderLabel"] as Style,
                    Margin = new Thickness(10, 0, 0, 0)
                });

                // Info -----------------------------------------------------------------
                Grid recordGrid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = 45 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                    Style = App.Current.Resources["ColumnGridStyle"] as Style
                };

                recordGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = "Time",
                            Style = App.Current.Resources["LeftColStyle"] as Style
                        },
                        new Label {
                            Text = "Rank",
                            Style = App.Current.Resources["LeftColStyle"] as Style
                        }
                    }
                }, 0, 0);

                if (noTime)
                {
                    recordGrid.Children.Add(new StackLayout
                    {
                        Children = {
                            new Label {
                                Text = "None",
                                Style = App.Current.Resources["RightColStyle"] as Style
                            },
                            new Label {
                                Text = "N/A",
                                Style = App.Current.Resources["RightColStyle"] as Style
                            }
                        }
                    }, 1, 0);
                }
                else
                {
                    string rank = zonePR.rank + "/" + zonePR.totalRanks;
                    if (zonePR.rank == "1") rank = "[WR] " + rank;
                    else if (int.Parse(zonePR.rank) <= 10) rank = "[Top10] " + rank;

                    recordGrid.Children.Add(new StackLayout
                    {
                        Children = {
                            new Label {
                                Text = StringFormatter.RankTimeString(zonePR.surfTime),
                                Style = App.Current.Resources["RightColStyle"] as Style
                            },
                            new Label {
                                Text = rank,
                                Style = App.Current.Resources["RightColStyle"] as Style
                            }
                        }
                    }, 1, 0);
                }

                ZoneRecordStack.Children.Add(recordGrid);
                if (noTime)
                {
                    i++;
                    continue;
                }

                ZoneRecordStack.Children.Add(new BoxView
                {
                    Style = App.Current.Resources["MiniSeparatorStyle"] as Style
                });

                // Velocity -------------------------------------------------------------
                Grid velGrid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = 95 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                    Style = App.Current.Resources["ColumnGridStyle"] as Style
                };

                velGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = "Avg Vel",
                            Style = App.Current.Resources["LeftColStyle"] as Style
                        },
                        new Label {
                            Text = "Start Vel",
                            Style = App.Current.Resources["LeftColStyle"] as Style
                        },
                        new Label {
                            Text = "End Vel",
                            Style = App.Current.Resources["LeftColStyle"] as Style
                        }
                    }
                }, 0, 0);

                velGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = ((int)double.Parse(zonePR.avgVel)) + units,
                            Style = App.Current.Resources["RightColStyle"] as Style
                        },
                        new Label {
                            Text = ((int)double.Parse(zonePR.startVel)) + units,
                            Style = App.Current.Resources["RightColStyle"] as Style
                        },
                        new Label {
                            Text = ((int)double.Parse(zonePR.endVel)) + units,
                            Style = App.Current.Resources["RightColStyle"] as Style
                        }
                    }
                }, 1, 0);

                ZoneRecordStack.Children.Add(velGrid);
                ZoneRecordStack.Children.Add(new BoxView
                {
                    Style = App.Current.Resources["MiniSeparatorStyle"] as Style
                });

                // Completion ------------------------------------------------------------------
                string completion = zonePR.count;
                if (!(zonePR.attempts is null))
                {
                    string percent = StringFormatter.CompletionPercentString(zonePR.count, zonePR.attempts);
                    completion += "/" + zonePR.attempts + " (" + percent + ")";
                }

                string time = "";
                if (!(zonePR.totalSurfTime is null))
                {
                    time = StringFormatter.PlayTimeString(zonePR.totalSurfTime, true);
                }

                Grid compGrid = new Grid
                {
                    ColumnDefinitions = {
                        new ColumnDefinition { Width = 120 },
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    },
                    Style = App.Current.Resources["ColumnGridStyle"] as Style
                };

                compGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = "Completions",
                            Style = App.Current.Resources["LeftColStyle"] as Style
                        },
                        new Label {
                            Text = "Time in Zone",
                            Style = App.Current.Resources["LeftColStyle"] as Style
                        }
                    }
                }, 0, 0);

                compGrid.Children.Add(new StackLayout
                {
                    Children = {
                        new Label {
                            Text = completion,
                            Style = App.Current.Resources["RightColStyle"] as Style
                        },
                        new Label {
                            Text = time,
                            Style = App.Current.Resources["RightColStyle"] as Style
                        }
                    }
                }, 1, 0);

                ZoneRecordStack.Children.Add(compGrid);
                i++;
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
                MapsMapPRDetailsScrollView.IsVisible = true;
            }
        }

        #endregion
    }
}
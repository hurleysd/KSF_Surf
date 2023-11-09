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
    public partial class LivePage : ContentPage
    {
        private readonly LiveViewModel liveViewModel;
        private bool hasLoaded = false;
        private readonly GameEnum defaultGame;

        // objects for "KSFClan Servers"
        private List<ServerDatum> cssServerData;
        private List<ServerDatum> css100tServerData;
        private List<ServerDatum> csgoServerData;

        // Date of last refresh
        private DateTime lastRefresh = DateTime.Now;

        public LivePage()
        {
            liveViewModel = new LiveViewModel();
            defaultGame = PropertiesDict.GetGame();

            InitializeComponent();

            // Refresh command lambda
            LiveRefreshView.Command = new Command(async () =>
            {
                if (hasLoaded)
                {
                    if (BaseViewModel.HasConnection())
                    {
                        TimeSpan sinceRefresh = DateTime.Now - lastRefresh;
                        bool tooSoon = sinceRefresh.TotalSeconds < 10;

                        if (tooSoon) await Task.Delay(500); // 0.5 seconds
                        else
                        {
                            await LoadServers(false);
                            lastRefresh = DateTime.Now;
                        }
                    }
                    else await DisplayAlert("Unable to refresh", "Please connect to the Internet.", "OK");
                }

                LiveRefreshView.IsRefreshing = false;
            });
        }

        // UI -------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private async Task LayoutDesign()
        {
            if (BaseViewModel.HasConnection()) await LoadServers(true);
            else
            {
                bool tryAgain = await DisplayAlert("Could not reach KSF servers", "Would you like to retry?", "Retry", "Cancel");
                if (tryAgain) await LayoutDesign();
                else System.Environment.Exit(0); // exit the app (no internet)
            }
        }

        private async Task LoadServers(Boolean order)
        {
            var css_ServerDatum = await liveViewModel.GetServers(GameEnum.CSS);
            var css100t_ServerDatum = await liveViewModel.GetServers(GameEnum.CSS100T);
            var csgos_ServerDatum = await liveViewModel.GetServers(GameEnum.CSGO);
            
            cssServerData = css_ServerDatum?.data;
            css100tServerData = css100t_ServerDatum?.data;
            csgoServerData = csgos_ServerDatum?.data;

            if (cssServerData is null || css100tServerData is null || csgoServerData is null) return;

            if (order) OrderServers();
            ChangeServers();
        }

        private void OrderServers()
        {
            if (defaultGame != GameEnum.CSS)
            {
                LiveServersStack.Children.Clear();
                LiveServersStack.Children.Add(TopGameLabel);

                if (defaultGame == GameEnum.CSGO)
                {
                    TopGameLabel.Text = "Counter-Strike: GO";
                    BottomGameLabel.Text = "Counter-Strike: Source";

                    LiveServersStack.Children.Add(CSGOGrid);
                    LiveServersStack.Children.Add(GameServerSeperator);
                    LiveServersStack.Children.Add(BottomGameLabel);
                    LiveServersStack.Children.Add(CSSGrid);
                    LiveServersStack.Children.Add(CSSServerSeperator);
                    LiveServersStack.Children.Add(CSS100TGrid);
                }
                else // CSS100T default game
                {
                    LiveServersStack.Children.Add(CSS100TGrid);
                    LiveServersStack.Children.Add(CSSServerSeperator);
                    LiveServersStack.Children.Add(CSSGrid);
                    LiveServersStack.Children.Add(GameServerSeperator);
                    LiveServersStack.Children.Add(BottomGameLabel);
                    LiveServersStack.Children.Add(CSGOGrid);
                }
            }
        }

        private void ChangeServers()
        {
            CSSServerStack.Children.Clear();
            CSSMapsStack.Children.Clear();
            CSS100TServerStack.Children.Clear();
            CSS100TMapsStack.Children.Clear();
            CSGOServerStack.Children.Clear();
            CSGOMapsStack.Children.Clear();

            int i = 1;
            int len = cssServerData.Count;
            foreach (ServerDatum datum in cssServerData)
            {
                if (datum.surftimer_servername.Equals("test", StringComparison.OrdinalIgnoreCase)
                   || datum.surftimer_servername.Equals("Map Contest", StringComparison.OrdinalIgnoreCase))
                {
                    if (i == len - 1)
                    {
                        CSSMapsStack.Children.RemoveAt(CSSMapsStack.Children.Count - 1); // remove separator added at the end of the previous map label
                        break;
                    }
                    else
                    {
                        --len;
                        continue;
                    }  
                }

                CSSServerStack.Children.Add(new Label
                {
                    Text = datum.surftimer_servername,
                    Style = App.Current.Resources["LeftColStyle"] as Style
                });

                Label mapLabel = new Label
                {
                    Text = datum.currentmap,
                    Style = App.Current.Resources["Right2ColStyle"] as Style
                };
                var tapMapGestureRecognizer = new TapGestureRecognizer();
                tapMapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new MapsMapPage(datum.currentmap, GameEnum.CSS));
                };
                mapLabel.GestureRecognizers.Add(tapMapGestureRecognizer);
                CSSMapsStack.Children.Add(mapLabel);

                CSSMapsStack.Children.Add(new Label
                {
                    Text = datum.playersonline + " players, " + StringFormatter.PlayTimeString(datum.timeleft, true) + " left",
                    Style = App.Current.Resources["Right3ColStyle"] as Style
                });

                if (++i < len)
                {
                    CSSMapsStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["MapSeparatorStyle"] as Style
                    });
                }
            }

            i = 1;
            len = css100tServerData.Count;
            foreach (ServerDatum datum in css100tServerData)
            {
                if (datum.surftimer_servername.IndexOf("test", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    if (i == len - 1)
                    {
                        CSS100TMapsStack.Children.RemoveAt(CSS100TMapsStack.Children.Count - 1); // remove separator added at the end of the previous map label
                        break;
                    }
                    else
                    {
                        --len;
                        continue;
                    }
                }

                CSS100TServerStack.Children.Add(new Label
                {
                    Text = datum.surftimer_servername,
                    Style = App.Current.Resources["LeftColStyle"] as Style
                });

                Label mapLabel = new Label
                {
                    Text = datum.currentmap,
                    Style = App.Current.Resources["Right2ColStyle"] as Style
                };
                var tapMapGestureRecognizer = new TapGestureRecognizer();
                tapMapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new MapsMapPage(datum.currentmap, GameEnum.CSS100T));
                };
                mapLabel.GestureRecognizers.Add(tapMapGestureRecognizer);
                CSS100TMapsStack.Children.Add(mapLabel);

                CSS100TMapsStack.Children.Add(new Label
                {
                    Text = datum.playersonline + " players, " + StringFormatter.PlayTimeString(datum.timeleft, true) + " left",
                    Style = App.Current.Resources["Right3ColStyle"] as Style
                });

                if (++i < len)
                {
                    CSS100TMapsStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["MapSeparatorStyle"] as Style
                    });
                }
            }

            i = 1;
            len = csgoServerData.Count;
            foreach (ServerDatum datum in csgoServerData)
            {
                if (datum.surftimer_servername.Equals("test", StringComparison.OrdinalIgnoreCase))
                {
                    if (i == len - 1)
                    {
                        CSGOMapsStack.Children.RemoveAt(CSGOMapsStack.Children.Count - 1); // remove separator added at the end of the previous map label
                        break;
                    }
                    else
                    {
                        --len;
                        continue;
                    }
                }

                if (datum.surftimer_servername == "EasySurf Europe")
                {
                    datum.surftimer_servername = "EasySurf EU";
                }

                CSGOServerStack.Children.Add(new Label
                {
                    Text = datum.surftimer_servername,
                    Style = App.Current.Resources["LeftColStyle"] as Style
                });

                Label mapLabel = new Label
                {
                    Text = datum.currentmap,
                    Style = App.Current.Resources["Right2ColStyle"] as Style
                };
                var tapMapGestureRecognizer = new TapGestureRecognizer();
                tapMapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new MapsMapPage(datum.currentmap, GameEnum.CSGO));
                };
                mapLabel.GestureRecognizers.Add(tapMapGestureRecognizer);
                CSGOMapsStack.Children.Add(mapLabel);

                CSGOMapsStack.Children.Add(new Label
                {
                    Text = datum.playersonline + " players, " + StringFormatter.PlayTimeString(datum.timeleft, true) + " left",
                    Style = App.Current.Resources["Right3ColStyle"] as Style
                });

                if (++i < len)
                {
                    CSGOMapsStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["MapSeparatorStyle"] as Style
                    });
                }
            }
        }

        #endregion
        // Event Handlers -------------------------------------------------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;

                // Check player steam ID has been set
                if (string.IsNullOrEmpty(PropertiesDict.GetSteamID()))
                {
                    // Set it to default (protect from just switching tabs to PlayerPage without saving settings edits)
                    if (!App.Current.Properties.ContainsKey("steamid")) App.Current.Properties.Add("steamid", BaseViewModel.DEFAULT_ME_STEAM_ID);
                    else App.Current.Properties["steamid"] = BaseViewModel.DEFAULT_ME_STEAM_ID;
                    _ = App.Current.SavePropertiesAsync(); // NO AWAIT

                    bool setMeSteamID = await DisplayAlert("Your Steam ID has not been set.", "Would you like to set it?", "Yes", "No");
                    if (setMeSteamID) await Navigation.PushAsync(new SettingsPage());
                }

                await LayoutDesign();
                lastRefresh = DateTime.Now;

                LoadingAnimation.IsRunning = false;
                LiveScrollView.IsVisible = true;
            }
        }

        #endregion
    }
}
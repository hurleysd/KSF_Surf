using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Essentials;

using KSF_Surf.Models;
using KSF_Surf.ViewModels;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class LivePage : ContentPage
    {
        private readonly LiveViewModel liveViewModel;
        private bool hasLoaded = false;
        private readonly EFilter_Game defaultGame;

        // objects for "KSFClan Servers"
        private List<KSFServerDatum> css_serverData;
        private List<KSFServerDatum> css100t_serverData;
        private List<KSFServerDatum> csgo_serverData;

        // Date of last refresh
        private DateTime lastRefresh = DateTime.Now;

        public LivePage()
        {
            liveViewModel = new LiveViewModel();
            defaultGame = BaseViewModel.propertiesDict_getGame();

            InitializeComponent();

            // Refresh command lambda
            LiveRefreshView.Command = new Command(async () =>
            {
                if (hasLoaded)
                {
                    if (BaseViewModel.hasConnection())
                    {
                        TimeSpan sinceRefresh = DateTime.Now - lastRefresh;
                        bool tooSoon = sinceRefresh.TotalSeconds < 10;

                        if (tooSoon)
                        {
                            await Task.Delay(500); // 0.5 seconds
                        }
                        else
                        {
                            await LoadServers(false);
                            lastRefresh = DateTime.Now;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Unable to refresh", "Please connect to the Internet.", "OK");
                    }
                }

                LiveRefreshView.IsRefreshing = false;
            });
        }

        // UI -------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private async Task LayoutDesign()
        {
            if (BaseViewModel.hasConnection())
            {
                await LoadServers(true);
            }
            else
            {
                bool tryAgain = await DisplayAlert("Could not connect to KSF servers :(", "Would you like to retry?", "Retry", "Cancel");
                if (tryAgain)
                {
                    await LayoutDesign();
                }
                else
                {
                    System.Environment.Exit(0); // exit the app (no WIFI)
                }
            }
        }

        private async Task LoadServers(Boolean order)
        {
            var css_ServerDatum = await liveViewModel.GetServers(EFilter_Game.css);
            var css100t_ServerDatum = await liveViewModel.GetServers(EFilter_Game.css100t);
            var csgos_ServerDatum = await liveViewModel.GetServers(EFilter_Game.csgo);
            
            css_serverData = css_ServerDatum?.data;
            css100t_serverData = css100t_ServerDatum?.data;
            csgo_serverData = csgos_ServerDatum?.data;

            if (css_serverData is null || css100t_serverData is null || csgo_serverData is null) return;

            if (order) OrderServers();
            ChangeServers();
        }

        private void OrderServers()
        {
            if (defaultGame != EFilter_Game.css)
            {
                LiveServersStack.Children.Clear();
                LiveServersStack.Children.Add(LiveServersLabel);
                LiveServersStack.Children.Add(TopGameLabel);

                if (defaultGame == EFilter_Game.csgo)
                {
                    TopGameLabel.Text = "CS:GO";
                    BottomGameLabel.Text = "CS:S";

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
            int len = css_serverData.Count;
            foreach (KSFServerDatum datum in css_serverData)
            {
                if (datum.surftimer_servername.Equals("test", StringComparison.OrdinalIgnoreCase))
                {
                    --len;
                    continue;
                }

                CSSServerStack.Children.Add(new Label
                {
                    Text = datum.surftimer_servername,
                    Style = App.Current.Resources["LeftColStyle"] as Style,
                });

                Label mapLabel = new Label
                {
                    Text = datum.currentmap,
                    Style = Resources["Right2ColStyle"] as Style
                };
                var tapMapGestureRecognizer = new TapGestureRecognizer();
                tapMapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new MapsMapPage(datum.currentmap, EFilter_Game.css));
                };
                mapLabel.GestureRecognizers.Add(tapMapGestureRecognizer);
                CSSMapsStack.Children.Add(mapLabel);

                CSSMapsStack.Children.Add(new Label
                {
                    Text = datum.playersonline + " players, " + String_Formatter.toString_PlayTime(datum.timeleft, true) + " left",

                    Style = Resources["Right3ColStyle"] as Style
                });

                if (++i < len)
                {
                    CSSMapsStack.Children.Add(new BoxView
                    {
                        Style = Resources["MapSeparatorStyle"] as Style
                    });
                }
            }

            i = 1;
            len = css100t_serverData.Count;
            foreach (KSFServerDatum datum in css100t_serverData)
            {
                if (datum.surftimer_servername.IndexOf("test", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    --len;
                    continue;
                }

                CSS100TServerStack.Children.Add(new Label
                {
                    Text = datum.surftimer_servername,
                    Style = App.Current.Resources["LeftColStyle"] as Style,
                });

                Label mapLabel = new Label
                {
                    Text = datum.currentmap,
                    Style = Resources["Right2ColStyle"] as Style
                };
                var tapMapGestureRecognizer = new TapGestureRecognizer();
                tapMapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new MapsMapPage(datum.currentmap, EFilter_Game.css100t));
                };
                mapLabel.GestureRecognizers.Add(tapMapGestureRecognizer);
                CSS100TMapsStack.Children.Add(mapLabel);

                CSS100TMapsStack.Children.Add(new Label
                {
                    Text = datum.playersonline + " players, " + String_Formatter.toString_PlayTime(datum.timeleft, true) + " left",

                    Style = Resources["Right3ColStyle"] as Style
                });

                if (++i < len)
                {
                    CSS100TMapsStack.Children.Add(new BoxView
                    {
                        Style = Resources["MapSeparatorStyle"] as Style
                    });
                }
            }

            i = 1;
            len = csgo_serverData.Count;
            foreach (KSFServerDatum datum in csgo_serverData)
            {
                if (datum.surftimer_servername.Equals("test", StringComparison.OrdinalIgnoreCase))
                {
                    --len;
                    continue;
                }

                if (datum.surftimer_servername == "EasySurf Europe")
                {
                    datum.surftimer_servername = "EasySurf EU";
                }

                CSGOServerStack.Children.Add(new Label
                {
                    Text = datum.surftimer_servername,
                    Style = App.Current.Resources["LeftColStyle"] as Style,
                });

                Label mapLabel = new Label
                {
                    Text = datum.currentmap,
                    Style = Resources["Right2ColStyle"] as Style
                };
                var tapMapGestureRecognizer = new TapGestureRecognizer();
                tapMapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new MapsMapPage(datum.currentmap, EFilter_Game.csgo));
                };
                mapLabel.GestureRecognizers.Add(tapMapGestureRecognizer);
                CSGOMapsStack.Children.Add(mapLabel);

                CSGOMapsStack.Children.Add(new Label
                {
                    Text = datum.playersonline + " players, " + String_Formatter.toString_PlayTime(datum.timeleft, true) + " left",

                    Style = Resources["Right3ColStyle"] as Style
                });

                if (++i < len)
                {
                    CSGOMapsStack.Children.Add(new BoxView
                    {
                        Style = Resources["MapSeparatorStyle"] as Style
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
                if (string.IsNullOrEmpty(BaseViewModel.propertiesDict_getSteamID()))
                {
                    // Set it to default (protect from just switching tabs to PlayerPage without saving settings edits)
                    if (!App.Current.Properties.ContainsKey("steamid")) App.Current.Properties.Add("steamid", BaseViewModel.DEFAULT_ME_STEAM_ID);
                    else App.Current.Properties["steamid"] = BaseViewModel.DEFAULT_ME_STEAM_ID;
                    App.Current.SavePropertiesAsync(); // NO AWAIT

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
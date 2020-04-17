﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Essentials;

using KSF_Surf.Models;
using KSF_Surf.ViewModels;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class LivePage : ContentPage
    {
        private LiveViewModel liveViewModel;

        private ObservableCollection<KSFServerDatum> css_serverData;
        private ObservableCollection<KSFServerDatum> css100t_serverData;
        private ObservableCollection<KSFServerDatum> csgo_serverData;
        private ObservableCollection<TwitchDatum> streamData;

        private bool allowVibrate = true;

        public LivePage()
        {
            liveViewModel = new LiveViewModel();

            InitializeComponent();
            StreamsRefreshView.Command = new Command(StreamsRefresh);

            LayoutDesign();
        }

        private async void LayoutDesign()
        {
            List<ServersList> servers = LoadServers();
            if (servers.Count != 0)
            {
                ServersCarousel.ItemsSource = servers;
            }
            else
            {
                bool tryAgain = await DisplayAlert("Could not connect to KSF servers :(", "Would you like to retry?", "Retry", "Cancel");
                if (tryAgain)
                {
                    liveViewModel.ksfRefresh();
                    liveViewModel.twitchRefresh();
                    LayoutDesign();
                }
                else
                {
                    System.Environment.Exit(0); // exit the app (no WIFI)
                }
            }
            LoadStreams();
        }

        // Event Handlers --------------------------------------------------------------------------------------------------------------------------

        private void ServersCarousel_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            if (ServersCarousel.Position == 0)
            {
                CSSServersLabel.TextColor = Xamarin.Forms.Color.FromHex("147efb");
                CSGOServersLabel.TextColor = Xamarin.Forms.Color.Gray;
            }
            else
            {
                CSSServersLabel.TextColor = Xamarin.Forms.Color.Gray;
                CSGOServersLabel.TextColor = Xamarin.Forms.Color.FromHex("147efb");
            }
        }

        private void CSSLabel_Tapped(object sender, EventArgs e)
        {
            if (ServersCarousel.Position != 0)
            {
                BaseViewModel.vibrate(allowVibrate);
                ServersCarousel.ScrollTo(0);
            }
        }

        private void CSGOLabel_Tapped(object sender, EventArgs e)
        {
            if (ServersCarousel.Position != 1)
            {
                BaseViewModel.vibrate(allowVibrate);
                ServersCarousel.ScrollTo(1);
            }
        }

        private async void Stream_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            TwitchDatum datum = (TwitchDatum)StreamsCollectionView.SelectedItem;
            StreamsCollectionView.SelectedItem = null;

            if (BaseViewModel.hasConnection())
            {
                Uri link = new Uri("https://www.twitch.tv/" + datum.user_name);
                if (await Launcher.CanOpenAsync(link)) await Launcher.OpenAsync(link);
            }
        }

        private async void StreamsRefresh()
        {
            if (BaseViewModel.hasConnection())
            {
                liveViewModel.twitchRefresh();
                LoadStreams();
            }
            else
            {
                await DisplayAlert("Could not connect to Twitch", "Please connect to the Internet.", "OK");
            }
            StreamsRefreshView.IsRefreshing = false;
        }

        // Loading KSF Server List ---------------------------------------------------------------------------------------------------------------------

        private ObservableCollection<KSFServerDatum> CSS_Servers { get { return css_serverData; } }
        private ObservableCollection<KSFServerDatum> CSS100T_Servers { get { return css100t_serverData; } }
        private ObservableCollection<KSFServerDatum> CSGO_Servers { get { return csgo_serverData; } }

        private List<ServersList> LoadServers()
        {
            List<ServersList> servers = new List<ServersList>();
            try
            {
                css_serverData = new ObservableCollection<KSFServerDatum>(liveViewModel.css_servers.data);
                css100t_serverData = new ObservableCollection<KSFServerDatum>(liveViewModel.css100t_servers.data);
                csgo_serverData = new ObservableCollection<KSFServerDatum>(liveViewModel.csgo_servers.data);
            }
            catch (NullReferenceException)
            {
                // no handling (query failed)
                Console.WriteLine("KSF Server Request returned NULL (LivePage)");
                return servers;
            }

            string cssServerString = "";
            string cssMapString = "";
            foreach (KSFServerDatum datum in css_serverData) // Adding CSS servers to list of Strings
            {
                if (datum.surftimer_servername.Contains("test")) continue;
                cssServerString += datum.surftimer_servername + "\n";

                string map = datum.currentmap;
                if (map.Length > 22)
                {
                    map = map.Substring(0, 19) + "...";
                }
                cssMapString += map + "\n";
            }
                
            foreach (KSFServerDatum datum in css100t_serverData) // Adding CSS100T servers to list of Strings
            {
                if (datum.surftimer_servername.Contains("TEST")) continue;
                cssServerString += datum.surftimer_servername + "\n";

                string map = datum.currentmap;
                if (map.Length > 22)
                {
                    map = map.Substring(0, 19) + "...";
                }
                cssMapString += map + "\n";
            }
            ServersList cssServersList = new ServersList { game = "css" };
            cssServersList.server_names = cssServerString.Trim();
            cssServersList.maps = cssMapString.Trim();

            string csgoServerString = "";
            string csgoMapString = "";
            foreach (KSFServerDatum datum in csgo_serverData) // Adding CSGO servers to list of Strings
            {
                if (datum.surftimer_servername.Contains("TEST")) continue;
                if (datum.surftimer_servername.Contains("Europe")) // "EasySurf Europe" is too long
                {
                    csgoServerString += "Europe\n";
                }
                else
                {
                    csgoServerString += datum.surftimer_servername + "\n";
                }

                string map = datum.currentmap;
                if (map.Length > 22)
                {
                    map = map.Substring(0, 19) + "...";
                }
                csgoMapString += map + "\n";
            }
            ServersList csgoServersList = new ServersList { game = "csgo" };
            csgoServersList.server_names = csgoServerString.Trim();
            csgoServersList.maps = csgoMapString.Trim();

            servers.Add(cssServersList);
            servers.Add(csgoServersList);

            return servers;
        }

        // Loading Twitch Streams ----------------------------------------------------------------------------------------------------------------------

        private ObservableCollection<TwitchDatum> Streams { get { return streamData; } }

        private void LoadStreams()
        {
            TwitchRootObject tro = liveViewModel.streams;
            if (tro == null)
            {
                // no handling (no streams online or Twitch query failed)
                Console.WriteLine("Twitch Request returned NULL (LivePage)");
                StreamsCollectionView.ItemsSource = new ObservableCollection<TwitchDatum>();
                return;    
            }
            streamData = new ObservableCollection<TwitchDatum>(tro.data);

            if (streamData.Count > 0)
            {
                foreach (TwitchDatum datum in streamData) // applying image sizes to stream thumbnails
                {
                    if (datum.thumbnail_url != null)
                    {
                        datum.thumbnail_url = datum.thumbnail_url.Replace("{height}", "72");
                        datum.thumbnail_url = datum.thumbnail_url.Replace("{width}", "128");
                    }
                }
                StreamsCollectionView.ItemsSource = streamData;
            } 
        }
    }
}
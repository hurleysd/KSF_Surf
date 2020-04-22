using System;
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
        private readonly LiveViewModel liveViewModel;

        // objects for "KSFClan Servers"
        private List<KSFServerDatum> css_serverData;
        private List<KSFServerDatum> css100t_serverData;
        private List<KSFServerDatum> csgo_serverData;

        private List<Label> css_serverLabels = new List<Label>();
        private List<Label> css_mapLabels = new List<Label>();
        private List<Label> csgo_serverLabels = new List<Label>();
        private List<Label> csgo_mapLabels = new List<Label>();

        // object for "Surfer Streams"
        private ObservableCollection<TwitchDatum> streamData;

        // variable for current filter
        private EFilter_Game game = EFilter_Game.none;

        // vibration
        private bool allowVibrate = false;

        public LivePage()
        {
            liveViewModel = new LiveViewModel();

            InitializeComponent();
            StreamsRefreshView.Command = new Command(StreamsRefresh);

            LayoutDesign();
        }

        // UI -------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private async void LayoutDesign()
        {
            if (BaseViewModel.hasConnection())
            {
                LoadServers();
                LoadStreams();
            }
            else
            {
                bool tryAgain = await DisplayAlert("Could not connect to KSF servers :(", "Would you like to retry?", "Retry", "Cancel");
                if (tryAgain)
                {
                    liveViewModel.twitchRefresh();
                    LayoutDesign();
                }
                else
                {
                    System.Environment.Exit(0); // exit the app (no WIFI)
                }
            }
        }

        private void ChangeGame(EFilter_Game newGame)
        {
            if (newGame == game) return;
            BaseViewModel.vibrate(allowVibrate);

            Color GrayTextColor = (Color)App.Current.Resources["GrayTextColor"];
            Color TappedTextColor = (Color)App.Current.Resources["TappedTextColor"];

            ServerStack.Children.Clear();
            MapsStack.Children.Clear();

            if (newGame == EFilter_Game.css)
            {
                CSSServersLabel.TextColor = TappedTextColor;
                CSGOServersLabel.TextColor = GrayTextColor;

                foreach (Label ServerLabel in css_serverLabels)
                {
                    ServerStack.Children.Add(ServerLabel);
                }
                foreach (Label MapLabel in css_mapLabels)
                {
                    MapsStack.Children.Add(MapLabel);
                }
            }
            else
            {
                CSSServersLabel.TextColor = GrayTextColor;
                CSGOServersLabel.TextColor = TappedTextColor;

                foreach (Label ServerLabel in csgo_serverLabels)
                {
                    ServerStack.Children.Add(ServerLabel);
                }
                foreach (Label MapLabel in csgo_mapLabels)
                {
                    MapsStack.Children.Add(MapLabel);
                }
            }
            game = newGame;
        }


        #endregion
        // Event Handlers -------------------------------------------------------------------------------------------------------------------------
        #region events

        private void CSSLabel_Tapped(object sender, EventArgs e) => ChangeGame(EFilter_Game.css);

        private void CSGOLabel_Tapped(object sender, EventArgs e) => ChangeGame(EFilter_Game.csgo);


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

        #endregion
        // Loading KSF Server List ----------------------------------------------------------------------------------------------------------------
        #region ksf

        private void LoadServers()
        {
            css_serverData = LiveViewModel.GetServers(EFilter_Game.css)?.data;
            css100t_serverData = LiveViewModel.GetServers(EFilter_Game.css100t)?.data;
            csgo_serverData = LiveViewModel.GetServers(EFilter_Game.csgo)?.data;

            if (css_serverData is null || css100t_serverData is null || csgo_serverData is null) return;

            foreach (KSFServerDatum datum in css_serverData)
            {
                if (datum.surftimer_servername.Contains("test")) continue;

                Label ServerLabel = new Label
                {
                    Text = datum.surftimer_servername,
                    Style = Resources["ServerLabelStyle"] as Style
                };
                css_serverLabels.Add(ServerLabel);

                Label MapLabel = new Label
                {
                    Text = datum.currentmap,
                    Style = Resources["ServerLabelStyle"] as Style
                };
                css_mapLabels.Add(MapLabel);
            }

            foreach (KSFServerDatum datum in css100t_serverData)
            {
                if (datum.surftimer_servername.Contains("TEST")) continue;

                Label ServerLabel = new Label
                {
                    Text = datum.surftimer_servername,
                    Style = Resources["ServerLabelStyle"] as Style
                };
                css_serverLabels.Add(ServerLabel);

                Label MapLabel = new Label
                {
                    Text = datum.currentmap,
                    Style = Resources["ServerLabelStyle"] as Style
                };
                css_mapLabels.Add(MapLabel);
            }

            foreach (KSFServerDatum datum in csgo_serverData)
            {
                if (datum.surftimer_servername.Contains("TEST")) continue;
                if (datum.surftimer_servername == "EasySurf Europe")
                {
                    datum.surftimer_servername = "EasySurf EU";
                }

                Label ServerLabel = new Label
                {
                    Text = datum.surftimer_servername,
                    Style = Resources["ServerLabelStyle"] as Style
                };
                csgo_serverLabels.Add(ServerLabel);

                Label MapLabel = new Label
                {
                    Text = datum.currentmap,
                    Style = Resources["ServerLabelStyle"] as Style
                };
                csgo_mapLabels.Add(MapLabel);
            }

            ChangeGame(EFilter_Game.css);
            allowVibrate = true;
        }

        #endregion
        // Loading Twitch Streams -----------------------------------------------------------------------------------------------------------------
        #region twitch

        private ObservableCollection<TwitchDatum> Streams { get { return streamData; } }

        private void LoadStreams()
        {
            TwitchRootObject tro = liveViewModel.streams;
            if (tro == null)
            {
                // no handling (no streams online or Twitch query failed)
                Console.WriteLine("Twitch Request returned NULL");
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

    #endregion
}
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

        // objects for "KSFClan Servers"
        private List<KSFServerDatum> css_serverData;
        private List<KSFServerDatum> css100t_serverData;
        private List<KSFServerDatum> csgo_serverData;

        // object for "Surfer Streams"
        private ObservableCollection<TwitchDatum> streamData;

        public LivePage()
        {
            liveViewModel = new LiveViewModel();

            InitializeComponent();
            LiveRefreshView.Command = new Command(LiveRefresh);
        }

        // UI -------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private async Task LayoutDesign()
        {
            if (BaseViewModel.hasConnection())
            {
                await LoadServers();
                await LoadStreams();
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


        private async Task LoadServers()
        {
            var css_ServerDatum = await liveViewModel.GetServers(EFilter_Game.css);
            var css100t_ServerDatum = await liveViewModel.GetServers(EFilter_Game.css100t);
            var csgos_ServerDatum = await liveViewModel.GetServers(EFilter_Game.csgo);
            
            css_serverData = css_ServerDatum?.data;
            css100t_serverData = css100t_ServerDatum?.data;
            csgo_serverData = csgos_ServerDatum?.data;

            if (css_serverData is null || css100t_serverData is null || csgo_serverData is null) return;
            ChangeServers();
        }

        private void ChangeServers()
        {
            CSSServerStack.Children.Clear();
            CSSMapsStack.Children.Clear();
            CSS100TServerStack.Children.Clear();
            CSS100TMapsStack.Children.Clear();
            CSGOServerStack.Children.Clear();
            CSGOMapsStack.Children.Clear();

            foreach (KSFServerDatum datum in css_serverData)
            {
                if (datum.surftimer_servername.Contains("test")) continue;

                CSSServerStack.Children.Add(new Label
                {
                    Text = datum.surftimer_servername,
                    Style = App.Current.Resources["LeftColStyle"] as Style,
                });
                CSSMapsStack.Children.Add(new Label
                {
                    Text = datum.currentmap,
                    Style = Resources["Right2ColStyle"] as Style
                });
            }

            foreach (KSFServerDatum datum in css100t_serverData)
            {
                if (datum.surftimer_servername.Contains("TEST")) continue;

                CSS100TServerStack.Children.Add(new Label
                {
                    Text = datum.surftimer_servername,
                    Style = App.Current.Resources["LeftColStyle"] as Style,
                });
                CSS100TMapsStack.Children.Add(new Label
                {
                    Text = datum.currentmap,
                    Style = Resources["Right2ColStyle"] as Style
                });
            }

            foreach (KSFServerDatum datum in csgo_serverData)
            {
                if (datum.surftimer_servername.Contains("TEST")) continue;
                if (datum.surftimer_servername == "EasySurf Europe")
                {
                    datum.surftimer_servername = "EasySurf EU";
                }

                CSGOServerStack.Children.Add(new Label
                {
                    Text = datum.surftimer_servername,
                    Style = App.Current.Resources["LeftColStyle"] as Style,
                });
                CSGOMapsStack.Children.Add(new Label
                {
                    Text = datum.currentmap,
                    Style = Resources["Right2ColStyle"] as Style
                });
            }
        }

        #endregion
        // Loading Twitch Streams -----------------------------------------------------------------------------------------------------------------
        #region twitch

        private ObservableCollection<TwitchDatum> Streams { get { return streamData; } }

        private async Task LoadStreams()
        {
            await liveViewModel.twitchRefresh();

            TwitchRootObject tro = liveViewModel.streams;
            if (tro == null)
            {
                // no handling (no streams online or Twitch query failed)
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

        #endregion
        // Event Handlers -------------------------------------------------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                await LayoutDesign();

                LoadingAnimation.IsRunning = false;
                LiveRefreshView.IsVisible = true;
                hasLoaded = true;
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

        private async void LiveRefresh()
        {
            if (BaseViewModel.hasConnection())
            {
                await LoadStreams();
                await LoadServers();
            }
            else
            {
                await DisplayAlert("Unable to refresh", "Please connect to the Internet.", "OK");
            }
            LiveRefreshView.IsRefreshing = false;
        }

        private async void Settings_Pressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        #endregion
    }
}
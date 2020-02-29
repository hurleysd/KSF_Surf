using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using KSF_Surf.Models;
using KSF_Surf.Views;
using KSF_Surf.ViewModels;
using System.Collections.ObjectModel;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class LivePage : ContentPage
    {
        private LiveViewModel liveModel;

        public LivePage()
        {
            liveModel = new LiveViewModel();

            InitializeComponent();
            LayoutDesign(); 
        }

        private void LayoutDesign()
        {
            List<ServersList> servers = LoadServers();
            ServersCarousel.ItemsSource = servers;

            LoadStreams();
        }

        private void ServersCarousel_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            if (ServersCarousel.Position == 0)
            {
                CSSServersLabel.TextColor = Xamarin.Forms.Color.Black;
                CSGOServersLabel.TextColor = Xamarin.Forms.Color.Gray;
            }
            else
            {
                CSSServersLabel.TextColor = Xamarin.Forms.Color.Gray;
                CSGOServersLabel.TextColor = Xamarin.Forms.Color.Black;
            }
        }

        private void CSSLabel_Tapped(object sender, EventArgs e)
        {
            ServersCarousel.Position = 0;
        }

        private void CSGOLabel_Tapped(object sender, EventArgs e)
        {
            ServersCarousel.Position = 1;
        }


        // Loading KSF Server List ---------------------------------------------------------------------------------------------------------------------

        ObservableCollection<KSFDatum> css_serverData;
        ObservableCollection<KSFDatum> css100t_serverData;
        ObservableCollection<KSFDatum> csgo_serverData;
        private ObservableCollection<KSFDatum> CSS_Servers { get { return css_serverData; } }
        private ObservableCollection<KSFDatum> CSS100T_Servers { get { return css100t_serverData; } }
        private ObservableCollection<KSFDatum> CSGO_Servers { get { return csgo_serverData; } }

        private List<ServersList> LoadServers()
        {
            List<ServersList> servers= new List<ServersList>();
            try
            {
                css_serverData = new ObservableCollection<KSFDatum>(liveModel.css_servers.data);
                css100t_serverData = new ObservableCollection<KSFDatum>(liveModel.css100t_servers.data);
                csgo_serverData = new ObservableCollection<KSFDatum>(liveModel.csgo_servers.data);
            }
            catch (NullReferenceException nullref)
            {
                // no handling (query failed)
                Console.Error.WriteLine("KSF Server Request returned null");
            }
            finally
            {

                string cssServerString = "";
                string cssMapString = "";
                foreach (KSFDatum datum in css_serverData) // Adding CSS servers to list of Strings
                {
                    if (datum.surftimer_servername.Contains("test")) continue;
                    cssServerString += datum.surftimer_servername + "\n";

                    string map = datum.currentmap;
                    if (map.Length > 25)
                    {
                        map = map.Substring(0, 22) + "...";
                    }
                    cssMapString += map + "\n";
                }
                
                foreach (KSFDatum datum in css100t_serverData) // Adding CSS100T servers to list of Strings
                {
                    if (datum.surftimer_servername.Contains("TEST")) continue;
                    cssServerString += datum.surftimer_servername + "\n";

                    string map = datum.currentmap;
                    if (map.Length > 25)
                    {
                        map = map.Substring(0, 22) + "...";
                    }
                    cssMapString += map + "\n";
                }
                ServersList cssServersList = new ServersList { game = "css" };
                cssServersList.server_names = cssServerString;
                cssServersList.maps = cssMapString;

                string csgoServerString = "";
                string csgoMapString = "";
                foreach (KSFDatum datum in csgo_serverData) // Adding CSGO servers to list of Strings
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
                    if (map.Length > 25)
                    {
                        map = map.Substring(0, 22) + "...";
                    }
                    csgoMapString += map + "\n";
                }
                ServersList csgoServersList = new ServersList { game = "csgo" };
                csgoServersList.server_names = csgoServerString;
                csgoServersList.maps = csgoMapString;

                servers.Add(cssServersList);
                servers.Add(csgoServersList);
            }
            return servers;
        }

        // Loading Twitch Streams ----------------------------------------------------------------------------------------------------------------------

        ObservableCollection<TwitchDatum> streamData;
        private ObservableCollection<TwitchDatum> Streams { get { return streamData; } }

        private void LoadStreams()
        {
            try
            {
                streamData = new ObservableCollection<TwitchDatum>(liveModel.streams.data);
            }
            catch (NullReferenceException nullref)
            {
                // no handling (no streams online or Twitch query failed)
                streamData = new ObservableCollection<TwitchDatum>() { new TwitchDatum { user_name = "(No streams online)" } };
            }
            finally
            {
                foreach (TwitchDatum datum in streamData) // applying image heights to stream thumbnails
                {
                    if (datum.thumbnail_url != null)
                    {
                        datum.thumbnail_url = datum.thumbnail_url.Replace("{height}", "75");
                        datum.thumbnail_url = datum.thumbnail_url.Replace("{width}", "125");
                    }
                }
                StreamsList.ItemsSource = streamData;
                StreamsList.HeightRequest = (StreamsList.RowHeight * streamData.Count) + 1; // adding 1 disables scrolling within this ListView
            }
        }

        
    }

}
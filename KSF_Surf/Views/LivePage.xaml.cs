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
            string cssServers = " Surf\t\t\t\t\t\t" + "map1" + "\n Europe\t\t\t\t\t" + "map2" + "\n Australia\t\t\t\t\t" + "map3" + "\n EUTop500\t\t\t\t" + "map4"
                                + "\n Expert\t\t\t\t\t" + "map5" + "\n Veteran\t\t\t\t\t" + "map6" + "\n 100T US\t\t\t\t\t" + "map7" + "\n 100T EU\t\t\t\t\t" + "map8";

            string csgoServers = " EasySurf\t\t\t\t\t" + "map1" + "\n AllSurf\t\t\t\t\t" + "map2" + "\n ExpertSurf\t\t\t\t" + "map3" + "\n Europe\t\t\t\t\t" + "map4";

            var servers = new List<string> { cssServers, csgoServers };

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

        ObservableCollection<LiveViewModel.Datum> streamData;
        private ObservableCollection<LiveViewModel.Datum> Streams { get { return streamData; } }

        private void LoadStreams()
        {
            try
            {
                streamData = new ObservableCollection<LiveViewModel.Datum>(liveModel.streams.data);
            }
            catch (NullReferenceException nullref)
            {
                // no handling (no streams online or Twitch query failed)
                streamData = new ObservableCollection<LiveViewModel.Datum>() { new LiveViewModel.Datum { user_name = "(No streams online)" } };
            }
            finally
            {
                foreach (LiveViewModel.Datum datum in streamData) // applying image heights to stream thumbnails
                {
                    if (datum.thumbnail_url != null)
                    {
                        datum.thumbnail_url = datum.thumbnail_url.Replace("{height}", "75");
                        datum.thumbnail_url = datum.thumbnail_url.Replace("{width}", "75");
                    }
                }
                StreamsList.ItemsSource = streamData;
                StreamsList.HeightRequest = (StreamsList.RowHeight * streamData.Count) + 1; // adding 1 disables scrolling within this ListView
            }
        }

        
    }

}
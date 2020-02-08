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

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class LivePage : ContentPage
    {
        public LivePage() 
        {
            InitializeComponent();
            LayoutDesign();
        }

        private void LayoutDesign()
        {
            

            String cssServers = " Surf\t\t\t\t\t\t" + "map1" + "\n Europe\t\t\t\t\t" + "map2" + "\n Australia\t\t\t\t\t" + "map3" + "\n EUTop500\t\t\t\t" + "map4"
                                + "\n Expert\t\t\t\t\t" + "map5" + "\n Veteran\t\t\t\t\t" + "map6" + "\n 100T US\t\t\t\t\t" + "map7" + "\n 100T EU\t\t\t\t\t" + "map8";

            String csgoServers = " EasySurf\t\t\t\t\t" + "map1" + "\n AllSurf\t\t\t\t\t" + "map2" + "\n ExpertSurf\t\t\t\t" + "map3" + "\n Europe\t\t\t\t\t" + "map4";

            var servers = new List<String> { cssServers, csgoServers };

            ServersCarousel.ItemsSource = servers;
            
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

        private async void StartLoadingMaps()
        {
            //ClientWebSocket ws = new ClientWebSocket();
        }
    }
}
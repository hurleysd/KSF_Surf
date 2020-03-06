using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    public partial class MapsPage : ContentPage
    {
        private MapsViewModel mapsViewModel;
        List<string> maps_list = new List<string>();
        private string[] games = { "css", "csgo" }; // used by filter to change game
        private string filteredGame;
        
        public MapsPage()
        {
            mapsViewModel = new MapsViewModel();

            InitializeComponent();
            LayoutDesign();
        }

        private void LayoutDesign()
        {
            maps_list = LoadMaps();
            MapsListView.ItemsSource = maps_list;
        }

        // Event Handlers --------------------------------------------------------------------------------------------------------------------------

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null) // cancel button pressed on iOS
            {
                MapsListView.ItemsSource = maps_list;
            }
            else
            {
                var keyword = MapsSearchBar.Text;
                var suggestions = maps_list.Where(m => m.Contains(keyword.ToLower()));
                MapsListView.ItemsSource = suggestions;
            }
        }

        // Loading KSF map list ---------------------------------------------------------------------------------------------------------------------

        private List<string> LoadMaps()
        {
            try
            {
                maps_list = mapsViewModel.css_maps.data;
            }
            catch (NullReferenceException nullref)
            {
                // no handling (query failed)
                Console.WriteLine("KSF Server Request returned NULL (MapsPage)");
                maps_list.Add("No maps found (error in KSF request)");
            }
            return maps_list;
        }
    }
}
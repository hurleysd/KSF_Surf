using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    public partial class MapsPage : ContentPage
    {
        private MapsViewModel mapsViewModel;
        List<string> maps_list = new List<string>();
        private string[] games = { "CS:S", "CS:GO" }; // used by filter to change game
        private string filteredGame;
        
        public MapsPage()
        {
            mapsViewModel = new MapsViewModel();

            InitializeComponent();
            LayoutDesign();
        }

        private void LayoutDesign()
        {
            maps_list = LoadMaps("CS:S");
            MapsListView.ItemsSource = maps_list;
        }

        // Event Handlers --------------------------------------------------------------------------------------------------------------------------

        private async void FilterPressed(object sender, EventArgs e)
        {
            // NOTE: DisplayActionSheet method causes unsatisfiable layout constraints -- this is a Xamarin issue
            string game = await DisplayActionSheet("Game", "Cancel", null, games);
            if (game != "cancel") MapsListView.ItemsSource = LoadMaps(game);
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null) // "cancel" button pressed on iOS
            {
                MapsListView.ItemsSource = maps_list;
            }
            else if (e.NewTextValue == "")
            {
                MapsListView.ItemsSource = new List<string>();
            }
            else
            {
                var keyword = MapsSearchBar.Text;
                var suggestions = maps_list.Where(m => m.Contains(keyword.ToLower()));
                MapsListView.ItemsSource = suggestions;
            }
        }

        public void SearchBarFocused(object sender, EventArgs e)
        {
            if (MapsSearchBar.Text == null) MapsListView.ItemsSource = new List<string>();
        }

        public void SearchBarUnfocused(object sender, EventArgs e)
        {
            MapsListView.ItemsSource = maps_list;
        }

        // Loading KSF map list ---------------------------------------------------------------------------------------------------------------------

        private List<string> LoadMaps(string game)
        {
            try
            {
                switch (game)
                {
                    case "CS:S": maps_list = mapsViewModel.css_maps.data; break;
                    case "CS:GO": maps_list = mapsViewModel.csgo_maps.data; break;
                }
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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using Xamarin.Forms;

using KSF_Surf.Models;
using KSF_Surf.ViewModels;


namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsPage : ContentPage
    {
        private MapsViewModel mapsViewModel;
        private List<string> maps_list = new List<string>();

        private ObservableCollection<DetailedMapDatum> detailed_mapData;

        // search filter vars used in API call used to minimize new calls
        private EFilter_Game currentGame = EFilter_Game.none;
        private EFilter_Sort currentSort = EFilter_Sort.none;
        // other search filter vars
        private int currentMinTier = 1;
        private int currentMaxTier = 8;
        private EFilter_MapType currentMapType = EFilter_MapType.any;

        public MapsPage()
        {
            mapsViewModel = new MapsViewModel();

            InitializeComponent();
            ChangeDisplayList(LoadMaps(EFilter_Game.css, EFilter_Sort.name, currentMinTier, currentMaxTier, currentMapType)); //initial load of maps
            MapsCollectionView.MinimumHeightRequest = 600;
        }

        // Loading KSF map list --------------------------------------------------------------------------------------------------------------------
        #region ksf

        private List<string> LoadMaps(EFilter_Game game, EFilter_Sort sort, int minTier, int maxTier, EFilter_MapType mapType)
        {
            // minTier options: 1-8
            // maxTier options: 1-8

            maps_list = new List<string>();

            try
            {
                if (game != currentGame || sort != currentSort)
                {
                    DetailedMapsRootObject dmro = MapsViewModel.GetDetailedMapsList(game, sort);
                    if (dmro == null)
                    {
                        Console.WriteLine("KSF Server Request returned NULL (MapsPage)");

                        MapsCollectionEmptyViewLabel.Text = "Could not reach KSF servers :(";
                        return maps_list;
                    }
                    MapsCollectionEmptyViewLabel.Text = "No maps matched your filter";
                    MapsTab.Title = "Maps [" + EFilter_ToString.toString2(game) + "]";

                    detailed_mapData = new ObservableCollection<DetailedMapDatum>(dmro.data);
                    currentGame = game;
                    currentSort = sort;
                }

                currentMinTier = minTier;
                currentMaxTier = maxTier;
                currentMapType = mapType;

                foreach (DetailedMapDatum datum in detailed_mapData)
                {
                    int tier = int.Parse(datum.tier);
                    int type = int.Parse(datum.maptype);

                    if (mapType != EFilter_MapType.any && type != (int)mapType)
                    {
                        continue;
                    }
                    else if (tier < minTier)
                    {
                        continue;
                    }
                    else if (tier > maxTier)
                    {
                        continue;
                    }
                    maps_list.Add(datum.name);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Problem parsing KSFDetailedMapDatum.data field (MapsPage)");
            }

            return maps_list;
        }

        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (BaseViewModel.hasConnection())
            {
                if (currentGame == EFilter_Game.none)
                {
                    ChangeDisplayList(LoadMaps(EFilter_Game.css, EFilter_Sort.name, currentMinTier, currentMaxTier, currentMapType)); //initial load of maps
                }

                await Navigation.PushAsync(new MapsFilterPage(ApplyFilters,
                    currentGame, currentSort, currentMinTier, currentMaxTier, currentMapType));
            }
            else
            {
                await DisplayAlert("Could not connect to KSF", "Please connect to the Internet.", "OK");
            }
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
            {
                ChangeDisplayList(new List<string>());
            }
            else
            {
                var keyword = MapsSearchBar.Text;
                var suggestions = maps_list.Where(m => m.Contains(keyword.ToLower()));
                ChangeDisplayList(suggestions.ToList());
            }
        }

        private void SearchBarFocused(object sender, EventArgs e)
        {
            if (MapsSearchBar.Text == null || MapsSearchBar.Text == "") ChangeDisplayList(new List<string>());
        }

        private void SearchBarUnfocused(object sender, EventArgs e)
        {
            
            if (MapsSearchBar.Text == null || MapsSearchBar.Text == "") ChangeDisplayList(maps_list);
        }

        private async void Map_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MapsCollectionView.SelectedItem == null) return;
            MapsCollectionView.SelectedItem = null;

            string selectedMap = (string)MapsCollectionView.SelectedItem;
            
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new MapsMapPage(selectedMap, currentGame));
            }
            else
            {
                await DisplayAlert("Could not connect to KSF servers :(", "Please connect to the Internet.", "OK");
            }

        }

        internal void ApplyFilters(EFilter_Game game, EFilter_Sort sort, int minTier, int maxTier, EFilter_MapType mapType)
        {
            MapsSearchBar.Text = "";
            ChangeDisplayList(LoadMaps(game, sort, minTier, maxTier, mapType));
            MapsCollectionView.ScrollTo(0);
        }

        private void ChangeDisplayList(List<string> list)
        {
            MapsCollectionView.ItemsSource = list;
        }

        #endregion
    }
}
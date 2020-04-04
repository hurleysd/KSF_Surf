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
        private List<string> maps_list = new List<string>();

        public MapsPage()
        {
            mapsViewModel = new MapsViewModel();

            InitializeComponent();
            LayoutDesign();
        }


        private void LayoutDesign() => ChangeDisplayList(LoadMaps(EFilter_Game.css, EFilter_Sort.name, 1, 8, EFilter_MapType.any)); //initial load of maps

        // Event Handlers --------------------------------------------------------------------------------------------------------------------------

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MapsFilterPage(ApplyFilters,
                currentGame, currentSort, currentMinTier, currentMaxTier,  currentMapType));
        }

        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            //if (e.NewTextValue == null) // "cancel" button pressed on iOS
            //{
            //    MapsListView.ItemsSource = maps_list;
            //} 
            //else 
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

        private void MapItem_Tapped(object sender, ItemTappedEventArgs e)
        {
            MapsSearchBar.Unfocus();
        }

        private void MapItem_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            // TODO: go to map page
        }


        // Loading KSF map list ---------------------------------------------------------------------------------------------------------------------

        private ObservableCollection<KSFDetailedMapDatum> detailed_mapData;

        // search filter vars used in API call used to minimize new calls
        private EFilter_Game currentGame = EFilter_Game.none;
        private EFilter_Sort currentSort = EFilter_Sort.none;
        // other search filter vars
        private int currentMinTier;
        private int currentMaxTier;
        private EFilter_MapType currentMapType;

        private List<string> LoadMaps(EFilter_Game game, EFilter_Sort sort, int minTier, int maxTier, EFilter_MapType mapType)
        {
            // minTier options: 1-8
            // maxTier options: 1-8
            maps_list = new List<string>();
            try
            {
                if (game != currentGame || sort != currentSort)
                {
                    detailed_mapData = new ObservableCollection<KSFDetailedMapDatum>(MapsViewModel.getDetailedMapsList(game, sort).data);
                    currentGame = game;
                    currentSort = sort;
                }

                currentMinTier = minTier;
                currentMaxTier = maxTier;
                currentMapType = mapType;


                foreach (KSFDetailedMapDatum datum in detailed_mapData)
                {
                    if (minTier != 1 || maxTier != 8 || mapType != 0)
                    {
                        if (Int32.Parse(datum.tier) < minTier)
                        {
                            continue;
                        }
                        else if (Int32.Parse(datum.tier) > maxTier)
                        {
                            continue;
                        }
                        else if (Int32.Parse(datum.maptype) != (int) mapType)
                        {
                            continue;
                        }
                        else
                        {
                            maps_list.Add(datum.name);
                        }
                    }
                    else
                    {
                        maps_list.Add(datum.name);
                    }
                }
            }
            catch (NullReferenceException)
            {
                // no handling (query failed)
                Console.WriteLine("KSF Server Request returned NULL (MapsPage)");
                maps_list.Add("No maps found (error in KSF request)");
            }
            catch (FormatException)
            {
                Console.WriteLine("Problem parsing KSFDetailedMapDatum.data field (MapsPage)");
            }

            return maps_list;
        }

        internal void ApplyFilters(EFilter_Game game, EFilter_Sort sort, int minTier, int maxTier, EFilter_MapType mapType)
        {
            ChangeDisplayList(LoadMaps(game, sort, minTier, maxTier, mapType));
        }


        private void ChangeDisplayList(List<string> list)
        {
            MapsCollectionView.ItemsSource = list;
        }
    }
}
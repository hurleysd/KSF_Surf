using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using KSF_Surf.Models;
using KSF_Surf.ViewModels;
using static Xamarin.Forms.Internals.Profile;


namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsPage : ContentPage
    {
        private readonly MapsViewModel mapsViewModel;
        private bool hasLoaded = false;

        // objects used in "Maps" call
        private List<string> mapsList = new List<string>();
        private List<DetailedMapDatum> detailedMapData;

        // search filter vars used in API call used to minimize new calls
        private GameEnum currentGame = GameEnum.NONE;
        private readonly GameEnum defaultGame;
        private SortEnum currentSort = SortEnum.NONE;
        // other search filter vars
        private int currentMinTier = 1;
        private int currentMaxTier = 8;
        private MapTypeEnum currentMapType = MapTypeEnum.ANY;

        public MapsPage()
        {
            mapsViewModel = new MapsViewModel();
            defaultGame = PropertiesDict.GetGame();

            InitializeComponent();
        }

        // Loading KSF map list --------------------------------------------------------------------------------------------------------------------
        #region ksf

        private async Task<List<string>> LoadMaps(GameEnum game, SortEnum sort, int minTier, int maxTier, MapTypeEnum mapType)
        {
            // minTier options: 1-8
            // maxTier options: 1-8

            mapsList = new List<string>();

            try
            {
                if (game != currentGame || sort != currentSort)
                {
                    DetailedMapsRoot dmro = await mapsViewModel.GetDetailedMapsList(game, sort);
                    if (dmro == null)
                    {
                        MapsCollectionEmptyViewLabel.Text = "Could not reach KSF servers :(";
                        return mapsList;
                    }
                    MapsCollectionEmptyViewLabel.Text = "No maps matched your filter";
                    MapsTab.Title = "Maps [" + EnumToString.NameString(game) + "]";

                    detailedMapData = new List<DetailedMapDatum>(dmro.data);
                    currentGame = game;
                    currentSort = sort;
                }

                currentMinTier = minTier;
                currentMaxTier = maxTier;
                currentMapType = mapType;

                foreach (DetailedMapDatum datum in detailedMapData)
                {
                    int tier = int.Parse(datum.tier);
                    int type = int.Parse(datum.maptype);

                    if (mapType != MapTypeEnum.ANY && type != (int)mapType) continue;
                    else if (tier < minTier) continue;
                    else if (tier > maxTier) continue;

                    mapsList.Add(datum.name);
                }
            }
            catch (FormatException)
            {
            }

            return mapsList;
        }

        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        internal async Task OnChangedTabAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;

                MapsCollectionView.VerticalOptions = LayoutOptions.FillAndExpand;
                ChangeDisplayList(await LoadMaps(defaultGame, SortEnum.NAME, currentMinTier, currentMaxTier, currentMapType)); // initial load of maps

                LoadingAnimation.IsRunning = false;
                MapsStack.IsVisible = true;
            }
        }

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (hasLoaded && BaseViewModel.HasConnection())
            {
                if (currentGame == GameEnum.NONE)
                {
                    ChangeDisplayList(await LoadMaps(defaultGame, SortEnum.NAME, currentMinTier, currentMaxTier, currentMapType)); // initial load of maps
                }

                await Navigation.PushAsync(new MapsFilterPage(ApplyFilters,
                    currentGame, currentSort, currentMinTier, currentMaxTier, currentMapType, defaultGame));
            }
            else await DisplayNoConnectionAlert();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
            {
                ChangeDisplayList(new List<string>());
            }
            else if (e.NewTextValue != null)
            {
                var keyword = MapsSearchBar.Text;
                var suggestions = mapsList.Where(m => m.Contains(keyword.ToLower()));
                ChangeDisplayList(suggestions.ToList());
            }
        }

        private void SearchBar_Focused(object sender, EventArgs e)
        {
            if (MapsSearchBar.Text == null || MapsSearchBar.Text == "") ChangeDisplayList(new List<string>());
        }

        private void SearchBar_Unfocused(object sender, EventArgs e)
        {
            if (MapsSearchBar.Text == null || MapsSearchBar.Text == "") ChangeDisplayList(mapsList);
        }

        private async void Map_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            string selectedMap = (string)MapsCollectionView.SelectedItem;
            MapsCollectionView.SelectedItem = null;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new MapsMapPage(selectedMap, currentGame));
            }
            else await DisplayNoConnectionAlert();
        }

        internal async void ApplyFilters(GameEnum game, SortEnum sort, int minTier, int maxTier, MapTypeEnum mapType)
        {
            if (BaseViewModel.HasConnection())
            {
                MapsSearchBar.Text = "";

                LoadingAnimation.IsRunning = true;
                ChangeDisplayList(await LoadMaps(game, sort, minTier, maxTier, mapType));
                LoadingAnimation.IsRunning = false;

                MapsCollectionView.ScrollTo(0);
            }
            else await DisplayNoConnectionAlert();
        }

        private void ChangeDisplayList(List<string> list)
        {
            MapsCollectionView.ItemsSource = list;
        }

        private async void RetryLabel_Tapped(object sender, EventArgs e)
        {
            LoadingAnimation.IsRunning = true;
            ChangeDisplayList(await LoadMaps(defaultGame, SortEnum.NAME, currentMinTier, currentMaxTier, currentMapType)); // initial load of maps
            LoadingAnimation.IsRunning = false;
        }

        private async Task DisplayNoConnectionAlert()
        {
            await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }

        #endregion
    }
}
﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsCountryTopPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;

        // objects used by "SurfTop" call
        private List<CountryTopDatum> countryTopData;
        private int listIndex = 1;

        // variables for filters
        private readonly GameEnum defaultGame;
        private readonly ModeEnum defaultMode;
        private GameEnum game;
        private ModeEnum mode;
        private string country;

        // collection view
        public ObservableCollection<Tuple<string, string, string>> recordsCountryTopCollectionViewItemsSource { get; }
                = new ObservableCollection<Tuple<string, string, string>>();

        public RecordsCountryTopPage(GameEnum game, ModeEnum mode, GameEnum defaultGame, ModeEnum defaultMode)
        {
            this.game = game;
            this.mode = mode;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;

            recordsViewModel = new RecordsViewModel();

            InitializeComponent();
            ChangeTitle(game, mode);
            CountryPicker.ItemsSource = StringFormatter.CountryTopCountries;
            RecordsCountryTopCollectionView.ItemsSource = recordsCountryTopCollectionViewItemsSource;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private void ChangeTitle(GameEnum game, ModeEnum mode)
        {
            Title = "[" + EnumToString.NameString(game) + "]";
            if (mode != ModeEnum.FW) Title += "[" + EnumToString.NameString(mode) + "]";
            Title += " Records";
        }

        private async Task ChangeCountryTop(bool clearPrev)
        {
            var countryTopDatum = await recordsViewModel.GetCountryTop(game, mode, country, listIndex);
            countryTopData = countryTopDatum?.data;
            if (countryTopData is null) return;

            if (clearPrev) recordsCountryTopCollectionViewItemsSource.Clear();
            LayoutCountryTop();
            CountryOptionButton.Text = StringFormatter.CountryEmoji(country);
            ChangeTitle(game, mode);
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutCountryTop()
        {
            foreach (CountryTopDatum datum in countryTopData)
            {
                string nameString = listIndex + ". " + datum.playerName;
                string pointsString = StringFormatter.PointsString(datum.points);

                recordsCountryTopCollectionViewItemsSource.Add(new Tuple<string, string, string>(
                    nameString, pointsString, datum.steamID));

                listIndex++;
            }
        }

        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;
                country = await recordsViewModel.GetTopCountry(game, mode);
                if (country is null || country == "")
                {
                    country = StringFormatter.CountryTopCountries[60]; // USA
                }

                await ChangeCountryTop(false);

                LoadingAnimation.IsRunning = false;
                RecordsCountryTopStack.IsVisible = true;
            }
        }

        private void CountryOptionButton_Clicked(object sender, EventArgs e)
        {
            CountryPicker.SelectedItem = country;
            CountryPicker.Focus();
        }

        private async void CountryPicker_Unfocused(object sender, FocusEventArgs e)
        {
            string selected = (string)CountryPicker.SelectedItem;
            if (selected == country) return;

            country = selected;
            listIndex = 1;

            LoadingAnimation.IsRunning = true;
            isLoading = true;

            await ChangeCountryTop(true);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void CountryRecordsTop_ThresholdReached(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.HasConnection()) return;
            if (((listIndex - 1) % RecordsViewModel.COUNTRY_TOP_QLIMIT) != 0) return; // didn't get full results
            if (listIndex >= RecordsViewModel.COUNTRY_TOP_CLIMIT) return; // at call limit

            isLoading = true;
            LoadingAnimation.IsRunning = true;

            await ChangeCountryTop(false);

            LoadingAnimation.IsRunning = false;
            isLoading = false;
        }

        private async void CountryRecordsTop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0) return;

            Tuple<string, string, string> selectedPlayer = (Tuple<string, string, string>)RecordsCountryTopCollectionView.SelectedItem;
            RecordsCountryTopCollectionView.SelectedItem = null;

            string playerSteamID = selectedPlayer.Item3;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsPlayerPage(game, mode, playerSteamID));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);
        }

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (hasLoaded && BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new RecordsFilterPage(ApplyFilters, game, mode, defaultGame, defaultMode));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);
        }

        internal async void ApplyFilters(GameEnum newGame, ModeEnum newMode)
        {
            if (newGame == game && newMode == mode) return;

            if (BaseViewModel.HasConnection())
            {
                game = newGame;
                mode = newMode;
                listIndex = 1;

                LoadingAnimation.IsRunning = true;
                isLoading = true;

                await ChangeCountryTop(true);

                LoadingAnimation.IsRunning = false;
                isLoading = false;
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);
        }

        #endregion
    }
}
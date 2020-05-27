using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.ViewModels;
using KSF_Surf.Models;
using System.Runtime.CompilerServices;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsCountryTopPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;
        private readonly int LIST_LIMIT = 25;
        private readonly int CALL_LIMIT = 250;

        // objects used by "SurfTop" call
        private List<CountryPlayer> countryTopData;
        private int list_index = 1;
        private bool moreRecords = false;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private string country;

        public RecordsCountryTopPage(string title, RecordsViewModel recordsViewModel, EFilter_Game game, EFilter_Mode mode)
        {
            this.recordsViewModel = recordsViewModel;
            this.game = game;
            this.mode = mode;

            InitializeComponent();
            Title = title;
            CountryPicker.ItemsSource = EFilter_ToString.ctopCountries;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeCountryTop(string country, bool clearGrid)
        {
            var countryTopDatum = await recordsViewModel.GetCountryTop(game, mode, country, list_index);
            countryTopData = countryTopDatum?.data;
            if (countryTopData is null) return;

            if (clearGrid) ClearCountryTopGrid();

            this.country = country;
            CountryOptionLabel.Text = "Country: " + String_Formatter.toEmoji_Country(country);
            LayoutCountryTop();
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutCountryTop()
        {
            foreach (CountryPlayer datum in countryTopData)
            {
                RankStack.Children.Add(new Label
                {
                    Text = list_index + ". " + datum.playerName,
                    Style = App.Current.Resources["GridLabelStyle"] as Style
                });
                PointsStack.Children.Add(new Label
                {
                    Text = String_Formatter.toString_Points(datum.points),
                    Style = App.Current.Resources["GridLabelStyle"] as Style
                });

                list_index++;
            }

            moreRecords = (((list_index - 1) % LIST_LIMIT == 0) && ((list_index - 1) < CALL_LIMIT));
            MoreFrame.IsVisible = moreRecords;
        }

        private void ClearCountryTopGrid()
        {
            RankStack.Children.Clear();
            RankStack.Children.Add(RankLabel);
            PointsStack.Children.Clear();
            PointsStack.Children.Add(PointsLabel);
        }

        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                country = await recordsViewModel.GetTopCountry(game, mode);
                if (country is null || country == "")
                {
                    country = EFilter_ToString.ctopCountries[58]; //USA
                }

                await ChangeCountryTop(country, false);

                LoadingAnimation.IsRunning = false;
                RecordsCountryTopPageScrollView.IsVisible = true;
                hasLoaded = true;
            }
        }

        private void CountryOptionLabel_Tapped(object sender, EventArgs e)
        {
            CountryPicker.SelectedItem = country;
            CountryPicker.Focus();
        }

        private async void CountryPicker_Unfocused(object sender, FocusEventArgs e)
        {
            string selected = (string)CountryPicker.SelectedItem;
            if (selected == country) return;

            country = selected;
            list_index = 1;

            LoadingAnimation.IsRunning = true;
            await ChangeCountryTop(country, true);
            LoadingAnimation.IsRunning = false;
        }

        private async void MoreButton_Tapped(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection()) return;
            isLoading = true;

            MoreButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            MoreLabel.IsVisible = false;
            MoreLoadingAnimation.IsRunning = true;

            var countryTopDatum = await recordsViewModel.GetCountryTop(game, mode, country, list_index);
            countryTopData = countryTopDatum?.data;

            if (countryTopData is null || countryTopData.Count < 1)
            {
                MoreFrame.IsVisible = false;
                return;
            }

            LayoutCountryTop();

            MoreButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
            MoreLoadingAnimation.IsRunning = false;
            MoreLabel.IsVisible = true;
            isLoading = false;
        }

        #endregion
    }
}
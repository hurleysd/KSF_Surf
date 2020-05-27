using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsTopCountriesPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;
        private readonly int LIST_LIMIT = 25;
        private readonly int CALL_LIMIT = 250;

        // objects used by "SurfTop" call
        private List<CountryPoints> topCountriesData;
        private int list_index = 1;
        private bool moreRecords = false;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;

        public RecordsTopCountriesPage(string title, RecordsViewModel recordsViewModel, EFilter_Game game, EFilter_Mode mode)
        {
            this.recordsViewModel = recordsViewModel;
            this.game = game;
            this.mode = mode;

            InitializeComponent();
            Title = title;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeTopCountries()
        {
            var topCountriesDatum = await recordsViewModel.GetTopCountries(game, mode, list_index);
            topCountriesData = topCountriesDatum?.data;
            if (topCountriesData is null) return;

            LayoutTopCountries();
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutTopCountries()
        {
            foreach (CountryPoints datum in topCountriesData)
            {
                RankStack.Children.Add(new Label
                {
                    Text = list_index + ". " + String_Formatter.toEmoji_Country(datum.country) + " " + datum.country,
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

        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                await ChangeTopCountries();

                LoadingAnimation.IsRunning = false;
                RecordsTopCountriesPageScrollView.IsVisible = true;
                hasLoaded = true;
            }
        }

        private async void MoreButton_Tapped(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection()) return;
            isLoading = true;

            MoreButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            MoreLabel.IsVisible = false;
            MoreLoadingAnimation.IsRunning = true;

            var topCountriesDatum = await recordsViewModel.GetTopCountries(game, mode, list_index);
            topCountriesData = topCountriesDatum?.data;

            if (topCountriesData is null || topCountriesData.Count < 1)
            {
                MoreFrame.IsVisible = false;
                return;
            }

            LayoutTopCountries();

            MoreButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
            MoreLoadingAnimation.IsRunning = false;
            MoreLabel.IsVisible = true;
            isLoading = false;
        }

        #endregion
    }
}
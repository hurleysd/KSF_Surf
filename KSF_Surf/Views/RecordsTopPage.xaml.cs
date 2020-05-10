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
    public partial class RecordsTopPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private readonly int LIST_LIMIT = 25;
        private readonly int CALL_LIMIT = 250;

        // objects used by "SurfTop" call
        private List<SurfTopDatum> surfTopData;
        private int list_index = 1;
        private bool moreRecords = false;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;

        public RecordsTopPage(string title, RecordsViewModel recordsViewModel, EFilter_Game game, EFilter_Mode mode)
        {
            this.recordsViewModel = recordsViewModel;
            this.game = game;
            this.mode = mode;

            InitializeComponent();
            Title = title;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords()
        {
            var surfTopDatum = await recordsViewModel.GetSurfTop(game, mode, list_index);
            surfTopData = surfTopDatum?.data;
            if (surfTopData is null) return;

            LayoutSurfTop();
        }

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutSurfTop()
        {
            foreach (SurfTopDatum datum in surfTopData)
            {
                TopRankStack.Children.Add(new Label
                {
                    Text = list_index + ". " + String_Formatter.toEmoji_Country(datum.country) + " " + datum.name,
                    Style = App.Current.Resources["GridLabelStyle"] as Style
                });
                TopPointsStack.Children.Add(new Label
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
                await ChangeRecords();

                LoadingAnimation.IsRunning = false;
                RecordsTopPageScrollView.IsVisible = true;
                hasLoaded = true;
            }
        }

        private async void MoreButton_Tapped(object sender, EventArgs e)
        {
            if (!BaseViewModel.hasConnection()) return;

            MoreButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            MoreLabel.IsVisible = false;
            MoreLoadingAnimation.IsRunning = true;

            var surfTopDatum = await recordsViewModel.GetSurfTop(game, mode, list_index);
            surfTopData = surfTopDatum?.data;

            if (surfTopData is null || surfTopData.Count < 1)
            {
                MoreFrame.IsVisible = false;
                return;
            }

            LayoutSurfTop();

            MoreButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
            MoreLoadingAnimation.IsRunning = false;
            MoreLabel.IsVisible = true;
        }

        #endregion
    }
}
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
    public partial class PlayerMapsCompletionPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;
        private readonly int LIST_LIMIT = 10;

        // objects used by "(In)Complete Maps" call
        private List<PlayerCompletionRecord> recordsData;
        private int list_index = 1;
        private bool moreRecords = false;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly EFilter_PlayerType playerType;
        private readonly string playerValue;
        private readonly EFilter_PlayerCompletionType completionType;

        public PlayerMapsCompletionPage(string title, PlayerViewModel playerViewModel, EFilter_Game game, EFilter_Mode mode,
            EFilter_PlayerCompletionType completionType, EFilter_PlayerType playerType, string playerValue)
        {
            this.playerViewModel = playerViewModel;
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;
            this.completionType = completionType;

            InitializeComponent();
            Title = title;
            HeaderLabel.Text = EFilter_ToString.toString2(completionType);
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeCompletion()
        {
            var completionDatum = await playerViewModel.GetPlayerMapsCompletion(game, mode, completionType, playerType, playerValue, list_index);
            recordsData = completionDatum?.data.records;
            if (recordsData is null) return;

            LayoutRecords();
        }

        // Dispaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            if (list_index != 1)
            {
                CompletionStack.Children.Add(new BoxView
                {
                    Style = App.Current.Resources["SeparatorStyle"] as Style
                });
            }

            int i = 0;
            int length = recordsData.Count;
            foreach (PlayerCompletionRecord datum in recordsData)
            {
                if (datum.completedZones is null) datum.completedZones = "0";
                CompletionStack.Children.Add(new Label
                {
                    Text = datum.mapName + " (" + datum.completedZones + "/" + datum.totalZones + ")",
                    Style = App.Current.Resources["RRLabelStyle"] as Style

                });

                EFilter_MapType mapType = (EFilter_MapType)int.Parse(datum.mapType);
                string cptype = (mapType == EFilter_MapType.linear) ? "CPs" : "Stages";
                string rrinfo = datum.cp_count + " " + cptype + ", " + datum.b_count + " Bonuses";

                CompletionStack.Children.Add(new Label
                {
                    Text = "Tier " + datum.tier + " " + EFilter_ToString.toString(mapType) + " - " + rrinfo,
                    Style = App.Current.Resources["TimeLabelStyle"] as Style
                });

                if (++i != length)
                {
                    CompletionStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["SeparatorStyle"] as Style
                    });
                }
            }

            moreRecords = (i == LIST_LIMIT);
            MoreFrame.IsVisible = moreRecords;

            if (i == 0) // no (in)complete maps
            {
                string text = "None ! " + ((completionType == EFilter_PlayerCompletionType.complete) ? ":(" :  ":)");
                CompletionStack.Children.Add(new Label
                {
                    Text = text,
                    Style = App.Current.Resources["LeftColStyle"] as Style,
                    HorizontalOptions = LayoutOptions.Center
                });
            }
        }


        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                await ChangeCompletion();

                LoadingAnimation.IsRunning = false;
                PlayerMapsCompletionScrollView.IsVisible = true;
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

            list_index += LIST_LIMIT;

            var completionDatum = await playerViewModel.GetPlayerMapsCompletion(game, mode, completionType, playerType, playerValue, list_index);
            recordsData = completionDatum?.data.records;

            MoreButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;

            if (recordsData is null || recordsData.Count < 1)
            {
                MoreFrame.IsVisible = false;
                return;
            }

            LayoutRecords();
            MoreLoadingAnimation.IsRunning = false;
            MoreLabel.IsVisible = true;
            isLoading = false;
        }

        #endregion
    }
}
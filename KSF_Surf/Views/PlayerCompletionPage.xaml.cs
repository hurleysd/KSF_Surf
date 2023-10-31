using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class PlayerCompletionPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;

        // objects used by "Completion By Tier" call
        private List<PlayerTierCompletionDatum> recordsData;

        // variables for filters
        private readonly GameEnum game;
        private readonly ModeEnum mode;
        private readonly PlayerTypeEnum playerType;
        private readonly string playerValue;

        public PlayerCompletionPage(string title, GameEnum game, ModeEnum mode, 
            PlayerTypeEnum playerType, string playerValue)
        {
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;

            playerViewModel = new PlayerViewModel();

            InitializeComponent();
            Title = title;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeTierCompletion()
        {
            var completionDatum = await playerViewModel.GetPlayerTierCompletion(game, mode, playerType, playerValue);
            recordsData = completionDatum?.data.records;
            if (recordsData is null) return;

            LayoutTiers();
        }

        

        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutTiers()
        {
            string[] labelTexts = new string[48];
            int i = 0;
            foreach (PlayerTierCompletionDatum datum in recordsData)
            {
                labelTexts[i++] = datum.map + "/" + datum.mapTotal + " (" + StringFormatter.CompletionPercentString(datum.map, datum.mapTotal) + ")";
                labelTexts[i++] = StringFormatter.PointsString(datum.stage) + "/" + StringFormatter.PointsString(datum.stageTotal) 
                    + " (" + StringFormatter.CompletionPercentString(datum.stage, datum.stageTotal) + ")";
                labelTexts[i++] = StringFormatter.PointsString(datum.bonus) + "/" + StringFormatter.PointsString(datum.bonusTotal) 
                    + " (" + StringFormatter.CompletionPercentString(datum.bonus, datum.bonusTotal) + ")";
            }

            applyLabelTexts(labelTexts);
        }

        private void applyLabelTexts(string[] texts)
        {
            int i = 0;

            T1MapsValueLabel.Text = texts[i++];
            T1StagesValueLabel.Text = texts[i++];
            T1BonusesValueLabel.Text = texts[i++];
            
            T2MapsValueLabel.Text = texts[i++];
            T2StagesValueLabel.Text = texts[i++];
            T2BonusesValueLabel.Text = texts[i++];
            
            T3MapsValueLabel.Text = texts[i++];
            T3StagesValueLabel.Text = texts[i++];
            T3BonusesValueLabel.Text = texts[i++];

            T4MapsValueLabel.Text = texts[i++];
            T4StagesValueLabel.Text = texts[i++];
            T4BonusesValueLabel.Text = texts[i++];

            T5MapsValueLabel.Text = texts[i++];
            T5StagesValueLabel.Text = texts[i++];
            T5BonusesValueLabel.Text = texts[i++];

            T6MapsValueLabel.Text = texts[i++];
            T6StagesValueLabel.Text = texts[i++];
            T6BonusesValueLabel.Text = texts[i++];

            T7MapsValueLabel.Text = texts[i++];
            T7StagesValueLabel.Text = texts[i++];
            T7BonusesValueLabel.Text = texts[i++];

            T8MapsValueLabel.Text = texts[i++];
            T8StagesValueLabel.Text = texts[i++];
            T8BonusesValueLabel.Text = texts[i++];
        }

        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;
                await ChangeTierCompletion();

                LoadingAnimation.IsRunning = false;
                PlayerCompletionScrollView.IsVisible = true;
            }
        }

        #endregion
    }
}
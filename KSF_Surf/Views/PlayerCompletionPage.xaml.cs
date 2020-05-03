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
    public partial class PlayerCompletionPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;

        // objects used by "Completion By Tier" call
        private List<PlayerTierCompletion> recordsData;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly EFilter_PlayerType playerType;
        private readonly string playerValue;

        public PlayerCompletionPage(string title, PlayerViewModel playerViewModel, EFilter_Game game, EFilter_Mode mode, 
            EFilter_PlayerType playerType, string playerValue)
        {
            this.playerViewModel = playerViewModel;
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;

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

        

        // Dispaying Changes -------------------------------------------------------------------------------

        private void LayoutTiers()
        {
            string[] labelTexts = new string[48];
            int i = 0;
            foreach (PlayerTierCompletion datum in recordsData)
            {
                labelTexts[i++] = datum.map + "/" + datum.mapTotal + " (" + String_Formatter.toString_CompletionPercent(datum.map, datum.mapTotal) + ")";
                labelTexts[i++] = String_Formatter.toString_Points(datum.stage) + "/" + String_Formatter.toString_Points(datum.stageTotal) 
                    + " (" + String_Formatter.toString_CompletionPercent(datum.stage, datum.stageTotal) + ")";
                labelTexts[i++] = String_Formatter.toString_Points(datum.bonus) + "/" + String_Formatter.toString_Points(datum.bonusTotal) 
                    + " (" + String_Formatter.toString_CompletionPercent(datum.bonus, datum.bonusTotal) + ")";
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
                await ChangeTierCompletion();
                hasLoaded = true;
            }
        }


        #endregion
    }
}
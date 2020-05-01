using KSF_Surf.Models;
using KSF_Surf.ViewModels;

using System;
using System.ComponentModel;

using Xamarin.Forms;


namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsFilterPage : ContentPage
    {
        // variables for filters
        private EFilter_Game game = EFilter_Game.none;
        private readonly EFilter_Game defaultGame;
        private EFilter_Mode mode = EFilter_Mode.none;
        private readonly EFilter_Mode defaultMode;

        // method to apply filters
        private readonly Action<EFilter_Game, EFilter_Mode> FilterApplier;

        // booleans for reset
        private bool resetGame = false;
        private bool resetMode = false;

        // vibration
        private bool allowVibrate = false;

        // colors
        private readonly Color untappedTextColor = (Color)App.Current.Resources["UntappedTextColor"];
        private readonly Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];


        public RecordsFilterPage(Action<EFilter_Game, EFilter_Mode> FilterApplier,
            EFilter_Game currentGame, EFilter_Mode currentMode,
            EFilter_Game defaultGame, EFilter_Mode defaultMode)
        {
            this.FilterApplier = FilterApplier;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;

            InitializeComponent();

            ChangeGameFilter(currentGame);
            ChangeModeFilter(currentMode);

            allowVibrate = true;
        }

        // UI -------------------------------------------------------------------------------------------------------------------------------------
        #region UI


        private void ChangeGameFilter(EFilter_Game newGame)
        {
            if (game == newGame) return;

            switch (game)
            {
                case EFilter_Game.css: GameCSSLabel.TextColor = untappedTextColor; break;
                case EFilter_Game.css100t: GameCSS100TLabel.TextColor = untappedTextColor; break;
                case EFilter_Game.csgo: GameCSGOLabel.TextColor = untappedTextColor; break;
                default: break;
            }

            switch (newGame)
            {
                case EFilter_Game.css: GameCSSLabel.TextColor = tappedTextColor; break;
                case EFilter_Game.css100t: GameCSS100TLabel.TextColor = tappedTextColor; break;
                case EFilter_Game.csgo:GameCSGOLabel.TextColor = tappedTextColor; break;
            }

            resetGame = (newGame != defaultGame);
            checkReset();

            BaseViewModel.vibrate(allowVibrate);
            game = newGame;
        }

        private void ChangeModeFilter(EFilter_Mode newMode)
        {
            if(mode == newMode) return;           

            switch (mode)
            {
                case EFilter_Mode.fw: ModeFWLabel.TextColor = untappedTextColor; break;
                case EFilter_Mode.hsw: ModeHSWLabel.TextColor = untappedTextColor; break;
                case EFilter_Mode.sw: ModeSWLabel.TextColor = untappedTextColor; break;
                case EFilter_Mode.bw: ModeBWLabel.TextColor = untappedTextColor; break;
                default: break;
            }

            switch (newMode)
            {
                case EFilter_Mode.fw: ModeFWLabel.TextColor = tappedTextColor; break;
                case EFilter_Mode.hsw: ModeHSWLabel.TextColor = tappedTextColor; break;
                case EFilter_Mode.sw: ModeSWLabel.TextColor = tappedTextColor; break;
                case EFilter_Mode.bw: ModeBWLabel.TextColor = tappedTextColor; break;
                default: break;
            }

            resetMode = (newMode != defaultMode);
            checkReset();

            BaseViewModel.vibrate(allowVibrate);
            mode = newMode;
        }

        private void checkReset()
        {
            ResetLabel.IsVisible = resetGame || resetMode;
        }


        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        private async void Apply_Clicked(object sender, System.EventArgs e)
        {
            FilterApplier(game, mode);
            await Navigation.PopAsync();
        }

        private void CSSGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.css);
        private void CSS100TGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.css100t);
        private void CSGOGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.csgo);

        private void FWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.fw);
        private void HSWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.hsw);
        private void SWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.sw);
        private void BWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.bw);

        private void ResetLabel_Tapped(object sender, EventArgs e)
        {
            bool oldAllowVibrate = allowVibrate;
            allowVibrate = false;

            if (game != defaultGame)
            {
                ChangeGameFilter(defaultGame);
            }
            if (mode != defaultMode)
            {
                ChangeModeFilter(defaultMode);
            }
  
            allowVibrate = oldAllowVibrate;
            BaseViewModel.vibrate(allowVibrate);

            ResetLabel.IsVisible = false;
        }
    }

    #endregion
}
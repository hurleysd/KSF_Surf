using System;
using System.ComponentModel;
using Xamarin.Forms;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsFilterPage : ContentPage
    {
        // variables for filters
        private GameEnum game = GameEnum.NONE;
        private readonly GameEnum defaultGame;
        private ModeEnum mode = ModeEnum.NONE;
        private readonly ModeEnum defaultMode;

        // method to apply filters
        private readonly Action<GameEnum, ModeEnum> FilterApplier;

        // booleans for reset
        private bool resetGame = false;
        private bool resetMode = false;

        // colors
        private readonly Color untappedTextColor = (Color)App.Current.Resources["UntappedTextColor"];
        private readonly Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];

        public RecordsFilterPage(Action<GameEnum, ModeEnum> FilterApplier,
            GameEnum currentGame, ModeEnum currentMode,
            GameEnum defaultGame, ModeEnum defaultMode)
        {
            this.FilterApplier = FilterApplier;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;

            InitializeComponent();

            ChangeGameFilter(currentGame);
            ChangeModeFilter(currentMode);
        }

        // UI -------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private void ChangeGameFilter(GameEnum newGame)
        {
            if (game == newGame) return;

            switch (game)
            {
                case GameEnum.CSS: GameCSSLabel.TextColor = untappedTextColor; break;
                case GameEnum.CSS100T: GameCSS100TLabel.TextColor = untappedTextColor; break;
                case GameEnum.CSGO: GameCSGOLabel.TextColor = untappedTextColor; break;
                default: break;
            }

            switch (newGame)
            {
                case GameEnum.CSS: GameCSSLabel.TextColor = tappedTextColor; break;
                case GameEnum.CSS100T: GameCSS100TLabel.TextColor = tappedTextColor; break;
                case GameEnum.CSGO:GameCSGOLabel.TextColor = tappedTextColor; break;
            }

            resetGame = (newGame != defaultGame);
            checkReset();

            game = newGame;
        }

        private void ChangeModeFilter(ModeEnum newMode)
        {
            if(mode == newMode) return;           

            switch (mode)
            {
                case ModeEnum.FW: ModeFWLabel.TextColor = untappedTextColor; break;
                case ModeEnum.HSW: ModeHSWLabel.TextColor = untappedTextColor; break;
                case ModeEnum.SW: ModeSWLabel.TextColor = untappedTextColor; break;
                case ModeEnum.BW: ModeBWLabel.TextColor = untappedTextColor; break;
                default: break;
            }

            switch (newMode)
            {
                case ModeEnum.FW: ModeFWLabel.TextColor = tappedTextColor; break;
                case ModeEnum.HSW: ModeHSWLabel.TextColor = tappedTextColor; break;
                case ModeEnum.SW: ModeSWLabel.TextColor = tappedTextColor; break;
                case ModeEnum.BW: ModeBWLabel.TextColor = tappedTextColor; break;
                default: break;
            }

            resetMode = (newMode != defaultMode);
            checkReset();

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

        private void CSSGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSS);
        private void CSS100TGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSS100T);
        private void CSGOGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSGO);

        private void FWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.FW);
        private void HSWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.HSW);
        private void SWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.SW);
        private void BWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.BW);

        private void ResetLabel_Tapped(object sender, EventArgs e)
        {
            if (game != defaultGame) ChangeGameFilter(defaultGame);
            if (mode != defaultMode) ChangeModeFilter(defaultMode);

            ResetLabel.IsVisible = false;
        }
    }

    #endregion
}
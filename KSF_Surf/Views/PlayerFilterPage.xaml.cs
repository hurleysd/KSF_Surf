using KSF_Surf.Models;
using KSF_Surf.ViewModels;

using System;
using System.ComponentModel;

using Xamarin.Forms;


namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class PlayerFilterPage : ContentPage
    {
        // variables for filters
        private EFilter_Game game = EFilter_Game.none;
        private EFilter_Mode mode = EFilter_Mode.none;
        private EFilter_PlayerType playerType = EFilter_PlayerType.none;
        private string playerSteamId;
        private string playerRank;

        // method to apply filters
        private Action<EFilter_Game, EFilter_Mode, EFilter_PlayerType, string> FilterApplier;

        // vibration
        private bool allowVibrate = false;


        public PlayerFilterPage(Action<EFilter_Game, EFilter_Mode, EFilter_PlayerType, string> FilterApplier,
            EFilter_Game currentGame, EFilter_Mode currentMode, EFilter_PlayerType currentPlayerType, string currentPlayerSteamId, string currentPlayerRank)
        {
            this.FilterApplier = FilterApplier;

            InitializeComponent();

            ChangeGameFilter(currentGame);
            ChangeModeFilter(currentMode);
            ChangePlayerFilter(currentPlayerType);

            playerRank = currentPlayerRank;
            RankEntry.Text = playerRank;

            playerSteamId = currentPlayerSteamId;
            SteamIdEntry.Text = playerSteamId;

            allowVibrate = true;
        }

        // UI -------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private void ChangePlayerFilter(EFilter_PlayerType newPlayerType)
        {
            if (playerType == newPlayerType) return;
            Color GrayTextColor = (Color)App.Current.Resources["GrayTextColor"];
            Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];

            switch (playerType)
            {
                case EFilter_PlayerType.rank: PlayerRankLabel.TextColor = GrayTextColor; break;
                case EFilter_PlayerType.steamid: PlayerSteamLabel.TextColor = GrayTextColor; break;
                default: break;
            }

            switch (newPlayerType)
            {
                case EFilter_PlayerType.rank: PlayerRankLabel.TextColor = tappedTextColor; break;
                case EFilter_PlayerType.steamid: PlayerSteamLabel.TextColor = tappedTextColor; break;
                default: break;
            }

            BaseViewModel.vibrate(allowVibrate);
            playerType = newPlayerType;
        }

        private void ChangeGameFilter(EFilter_Game newGame)
        {
            if (game == newGame) return;
            Color GrayTextColor = (Color)App.Current.Resources["GrayTextColor"];
            Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];

            switch (game)
            {
                case EFilter_Game.css: GameCSSLabel.TextColor = GrayTextColor; break;
                case EFilter_Game.css100t: GameCSS100TLabel.TextColor = GrayTextColor; break;
                case EFilter_Game.csgo: GameCSGOLabel.TextColor = GrayTextColor; break;
                default: break;
            }

            switch (newGame)
            {
                case EFilter_Game.css: GameCSSLabel.TextColor = tappedTextColor; break;
                case EFilter_Game.css100t: GameCSS100TLabel.TextColor = tappedTextColor; break;
                case EFilter_Game.csgo:GameCSGOLabel.TextColor = tappedTextColor; break;
            }

            BaseViewModel.vibrate(allowVibrate);
            game = newGame;
        }

        private void ChangeModeFilter(EFilter_Mode newMode)
        {
            if(mode == newMode) return;
            Color GrayTextColor = (Color)App.Current.Resources["GrayTextColor"];
            Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];

            switch (mode)
            {
                case EFilter_Mode.fw: ModeFWLabel.TextColor = GrayTextColor; break;
                case EFilter_Mode.hsw: ModeHSWLabel.TextColor = GrayTextColor; break;
                case EFilter_Mode.sw: ModeSWLabel.TextColor = GrayTextColor; break;
                case EFilter_Mode.bw: ModeBWLabel.TextColor = GrayTextColor; break;
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

            BaseViewModel.vibrate(allowVibrate);
            mode = newMode;
        }


        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        private async void Apply_Clicked(object sender, System.EventArgs e)
        {
            if (playerType == EFilter_PlayerType.rank)
            {
                playerRank = RankEntry.Text;
                FilterApplier(game, mode, playerType, playerRank);
            }
            else
            {
                playerSteamId = SteamIdEntry.Text;
                FilterApplier(game, mode, playerType, playerSteamId);
            }
            await Navigation.PopAsync();
        }

        private void PlayerRankLabel_Tapped(object sender, EventArgs e) => ChangePlayerFilter(EFilter_PlayerType.rank);
        private void PlayerSteamLabel_Tapped(object sender, EventArgs e) => ChangePlayerFilter(EFilter_PlayerType.steamid);

        private void CSSGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.css);
        private void CSS100TGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.css100t);
        private void CSGOGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.csgo);

        private void FWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.fw);
        private void HSWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.hsw);
        private void SWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.sw);
        private void BWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.bw);

        private void RankEntry_Focused(object sender, FocusEventArgs e) => ChangePlayerFilter(EFilter_PlayerType.rank);
        private void SteamIdEntry_Focused(object sender, FocusEventArgs e) => ChangePlayerFilter(EFilter_PlayerType.steamid);
    }

    #endregion
}
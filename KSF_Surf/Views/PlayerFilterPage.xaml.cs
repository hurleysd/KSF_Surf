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
        private readonly EFilter_Game defaultGame;

        private EFilter_Mode mode = EFilter_Mode.none;
        private readonly EFilter_Mode defaultMode;

        private EFilter_PlayerType playerType = EFilter_PlayerType.none;
        private readonly string meSteamId;
        private string playerSteamId;
        private string playerRank;

        // method to apply filters
        private readonly Action<EFilter_Game, EFilter_Mode, EFilter_PlayerType, string> FilterApplier;

        // booleans for reset
        private bool resetGame = false;
        private bool resetMode = false;
        private bool resetPlayer = false;

        // colors
        private readonly Color untappedTextColor = (Color)App.Current.Resources["UntappedTextColor"];
        private readonly Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];


        public PlayerFilterPage(Action<EFilter_Game, EFilter_Mode, EFilter_PlayerType, string> FilterApplier,
            EFilter_Game currentGame, EFilter_Mode currentMode, EFilter_PlayerType currentPlayerType, string currentPlayerSteamId, string currentPlayerRank,
            EFilter_Game defaultGame, EFilter_Mode defaultMode, string meSteamId)
        {
            this.FilterApplier = FilterApplier;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;
            this.meSteamId = meSteamId;

            InitializeComponent();

            ChangeGameFilter(currentGame);
            ChangeModeFilter(currentMode);
            ChangePlayerFilter(currentPlayerType);

            PlayerMeIDLabel.Text = "  " + meSteamId;

            playerRank = currentPlayerRank;
            RankEntry.Text = playerRank;

            playerSteamId = currentPlayerSteamId;
            SteamIdEntry.Text = playerSteamId;
        }

        // UI -------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private void ChangePlayerFilter(EFilter_PlayerType newPlayerType)
        {
            if (playerType == newPlayerType) return;

            switch (playerType)
            {
                case EFilter_PlayerType.me: PlayerMeLabel.TextColor = untappedTextColor; break;
                case EFilter_PlayerType.rank: PlayerRankLabel.TextColor = untappedTextColor; break;
                case EFilter_PlayerType.steamid: PlayerSteamLabel.TextColor = untappedTextColor; break;
                default: break;
            }

            switch (newPlayerType)
            {
                case EFilter_PlayerType.me: PlayerMeLabel.TextColor = tappedTextColor; break;
                case EFilter_PlayerType.rank: PlayerRankLabel.TextColor = tappedTextColor; break;
                case EFilter_PlayerType.steamid: PlayerSteamLabel.TextColor = tappedTextColor; break;
                default: break;
            }

            resetPlayer = (newPlayerType != EFilter_PlayerType.me) ;
            checkReset();

            playerType = newPlayerType;
        }

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

            mode = newMode;
        }

        private void checkReset()
        {
            ResetLabel.IsVisible = resetPlayer || resetGame || resetMode;
        }


        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        private async void Apply_Clicked(object sender, System.EventArgs e)
        {
            string playerValue = meSteamId;
            if (playerType == EFilter_PlayerType.rank)
            {
                playerValue = RankEntry.Text;
            }
            else if (playerType == EFilter_PlayerType.steamid)
            {
                playerValue = SteamIdEntry.Text;
            }

            FilterApplier(game, mode, playerType, playerValue);
            await Navigation.PopAsync();
        }

        private void PlayerMeLabel_Tapped(object sender, EventArgs e) => ChangePlayerFilter(EFilter_PlayerType.me);
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

        private void ResetLabel_Tapped(object sender, EventArgs e)
        {
            if (playerType != EFilter_PlayerType.me)
            {
                ChangePlayerFilter(EFilter_PlayerType.me);
            }
            if (game != defaultGame)
            {
                ChangeGameFilter(defaultGame);
            }
            if (mode != defaultMode)
            {
                ChangeModeFilter(defaultMode);
            }

            ResetLabel.IsVisible = false;
        }
    }

    #endregion
}
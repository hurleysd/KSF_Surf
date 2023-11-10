using System;
using System.ComponentModel;
using Xamarin.Forms;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class PlayerFilterPage : ContentPage
    {
        // variables for filters
        private GameEnum game = GameEnum.NONE;
        private readonly GameEnum defaultGame;

        private ModeEnum mode = ModeEnum.NONE;
        private readonly ModeEnum defaultMode;

        private PlayerTypeEnum playerType = PlayerTypeEnum.NONE;
        private readonly string meSteamID;
        private string playerSteamID;
        private string playerRank;

        // method to apply filters
        private readonly Action<GameEnum, ModeEnum, PlayerTypeEnum, string> FilterApplier;

        // booleans for reset
        private bool resetGame = false;
        private bool resetMode = false;
        private bool resetPlayer = false;

        // colors
        private readonly Color untappedTextColor = (Color)App.Current.Resources["UntappedTextColor"];
        private readonly Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];


        public PlayerFilterPage(Action<GameEnum, ModeEnum, PlayerTypeEnum, string> FilterApplier,
            GameEnum currentGame, ModeEnum currentMode, PlayerTypeEnum currentPlayerType, string currentPlayerSteamID, string currentPlayerRank,
            GameEnum defaultGame, ModeEnum defaultMode, string meSteamID)
        {
            this.FilterApplier = FilterApplier;
            this.defaultGame = defaultGame;
            this.defaultMode = defaultMode;
            this.meSteamID = meSteamID;

            InitializeComponent();

            ChangeGameFilter(currentGame);
            ChangeModeFilter(currentMode);
            ChangePlayerFilter(currentPlayerType);

            PlayerMeIDLabel.Text = " " + meSteamID;

            playerRank = currentPlayerRank;
            RankEntry.Text = playerRank;

            playerSteamID = currentPlayerSteamID;
            SteamIDEntry.Text = playerSteamID;
        }

        // UI -------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private void ChangePlayerFilter(PlayerTypeEnum newPlayerType)
        {
            if (playerType == newPlayerType) return;

            switch (playerType)
            {
                case PlayerTypeEnum.ME: PlayerMeLabel.TextColor = untappedTextColor; break;
                case PlayerTypeEnum.RANK: PlayerRankLabel.TextColor = untappedTextColor; break;
                case PlayerTypeEnum.STEAM_ID: PlayerSteamLabel.TextColor = untappedTextColor; break;
                default: break;
            }

            switch (newPlayerType)
            {
                case PlayerTypeEnum.ME: PlayerMeLabel.TextColor = tappedTextColor; break;
                case PlayerTypeEnum.RANK: PlayerRankLabel.TextColor = tappedTextColor; break;
                case PlayerTypeEnum.STEAM_ID: PlayerSteamLabel.TextColor = tappedTextColor; break;
                default: break;
            }

            resetPlayer = (newPlayerType != PlayerTypeEnum.ME) ;
            checkReset();

            playerType = newPlayerType;
        }

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
            ResetLabel.IsVisible = resetPlayer || resetGame || resetMode;
        }


        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        private async void Apply_Clicked(object sender, System.EventArgs e)
        {
            string playerValue = meSteamID;
            if (playerType == PlayerTypeEnum.RANK) playerValue = RankEntry.Text;
            else if (playerType == PlayerTypeEnum.STEAM_ID) playerValue = SteamIDEntry.Text;

            FilterApplier(game, mode, playerType, playerValue);
            await Navigation.PopAsync();
        }

        private void PlayerMeLabel_Tapped(object sender, EventArgs e) => ChangePlayerFilter(PlayerTypeEnum.ME);
        private void PlayerRankLabel_Tapped(object sender, EventArgs e) => ChangePlayerFilter(PlayerTypeEnum.RANK);
        private void PlayerSteamLabel_Tapped(object sender, EventArgs e) => ChangePlayerFilter(PlayerTypeEnum.STEAM_ID);

        private void CSSGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSS);
        private void CSS100TGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSS100T);
        private void CSGOGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSGO);

        private void FWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.FW);
        private void HSWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.HSW);
        private void SWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.SW);
        private void BWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.BW);

        private void RankEntry_Focused(object sender, FocusEventArgs e) => ChangePlayerFilter(PlayerTypeEnum.RANK);
        private void SteamIDEntry_Focused(object sender, FocusEventArgs e) => ChangePlayerFilter(PlayerTypeEnum.STEAM_ID);

        private void ResetLabel_Tapped(object sender, EventArgs e)
        {
            if (playerType != PlayerTypeEnum.ME) ChangePlayerFilter(PlayerTypeEnum.ME);
            if (game != defaultGame) ChangeGameFilter(defaultGame);
            if (mode != defaultMode) ChangeModeFilter(defaultMode);

            ResetLabel.IsVisible = false;
        }
    }

    #endregion
}
using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using KSF_Surf.Models;
using KSF_Surf.ViewModels;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class SettingsPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;

        // variables for filters
        private GameEnum game = GameEnum.NONE;
        private ModeEnum mode = ModeEnum.NONE;
        private string playerSteamID;

        // colors
        private readonly Color untappedTextColor = (Color)App.Current.Resources["UntappedTextColor"];
        private readonly Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];


        public SettingsPage()
        {
            playerViewModel = new PlayerViewModel();

            InitializeComponent();

            GameEnum currentGame = PropertiesDict.GetGame();
            ModeEnum currentMode = PropertiesDict.GetMode();
            playerSteamID = PropertiesDict.GetSteamID();

            ChangeGameFilter(currentGame);
            ChangeModeFilter(currentMode);
            SteamIdEntry.Text = playerSteamID;
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

            game = newGame;
        }

        private void ChangeModeFilter(ModeEnum newMode)
        {
            if (mode == newMode) return;           

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

            mode = newMode;
        }

        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        private async void Apply_Clicked(object sender, EventArgs e)
        {
            if (BaseViewModel.HasConnection())
            {
                if (playerSteamID != SteamIdEntry.Text)
                {
                    // Check new Steam ID is valid
                    var steamProfileDatum = await playerViewModel.GetPlayerSteamProfile(SteamIdEntry.Text);
                    if (steamProfileDatum is null || steamProfileDatum?.response.players.Count == 0)
                    {
                        await DisplayAlert("Could not find player profile!", "Invalid SteamID.", "OK");
                        return;
                    }
                }

                await PropertiesDict.SetAll(SteamIdEntry.Text, game, mode);
                await Navigation.PopAsync();
            }
            else await DisplayAlert("Could not connect to Steam!", "Please connect to the Internet.", "OK"); 
        }

        private void CSSGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSS);
        private void CSS100TGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSS100T);
        private void CSGOGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSGO);

        private void FWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.FW);
        private void HSWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.HSW);
        private void SWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.SW);
        private void BWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(ModeEnum.BW);

        private async void Info_Tapped(object sender, EventArgs e)
        {
            InfoButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            await Navigation.PushAsync(new InfoPage());

            InfoButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void Donate_Tapped(object sender, EventArgs e)
        {
            DonateButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                Uri link = new Uri("https://paypal.me/ksfmobiledev");
                if (await Launcher.CanOpenAsync(link)) await Launcher.OpenAsync(link);
            }
            else await DisplayAlert("Could not open web page", "Please connect to the Internet.", "OK");

            DonateButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }
    }

    #endregion
}
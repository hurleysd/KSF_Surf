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
        private readonly PlayerViewModel playerViewModel = new PlayerViewModel();

        // variables for filters
        private EFilter_Game game = EFilter_Game.none;
        private EFilter_Mode mode = EFilter_Mode.none;
        private string playerSteamID;

        // vibration
        private bool allowVibrate = false;

        // colors
        private readonly Color untappedTextColor = (Color)App.Current.Resources["UntappedTextColor"];
        private readonly Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];


        public SettingsPage()
        {
            InitializeComponent();

            EFilter_Game currentGame = BaseViewModel.propertiesDict_getGame();
            EFilter_Mode currentMode = BaseViewModel.propertiesDict_getMode();
            playerSteamID = BaseViewModel.propertiesDict_getSteamID();

            ChangeGameFilter(currentGame);
            ChangeModeFilter(currentMode);
            SteamIdEntry.Text = playerSteamID;

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

            BaseViewModel.vibrate(allowVibrate);
            mode = newMode;
        }

        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        private async void Apply_Clicked(object sender, System.EventArgs e)
        {
            if (BaseViewModel.hasConnection())
            {
                if (playerSteamID != SteamIdEntry.Text)
                {
                    var steamProfileDatum = await playerViewModel.GetPlayerSteamProfile(SteamIdEntry.Text);
                    if (steamProfileDatum?.response.players.Count == 0)
                    {
                        await DisplayAlert("Could not find player profile!", "Invalid SteamID.", "OK");
                        return;
                    }
                }

                App.Current.Properties["steamid"] = SteamIdEntry.Text;
                App.Current.Properties["game"] = EFilter_ToString.toString(game);
                App.Current.Properties["mode"] = EFilter_ToString.toString(mode);
                await App.Current.SavePropertiesAsync();

                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Could not connect to Steam!", "Please connect to the Internet.", "OK");
            }
            
        }

        private void CSSGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.css);
        private void CSS100TGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.css100t);
        private void CSGOGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.csgo);

        private void FWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.fw);
        private void HSWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.hsw);
        private void SWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.sw);
        private void BWModeFilter_Tapped(object sender, EventArgs e) => ChangeModeFilter(EFilter_Mode.bw);

        private async void Info_Tapped(object sender, EventArgs e)
        {
            InfoButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            await Navigation.PushAsync(new InfoPage());
            InfoButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void Donate_Tapped(object sender, EventArgs e)
        {
            DonateButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                Uri link = new Uri("https://paypal.me/ksfmobiledev");
                if (await Launcher.CanOpenAsync(link)) await Launcher.OpenAsync(link);
            }
            else
            {
                await DisplayAlert("Could not open web page", "Please connect to the Internet.", "OK");
            }
            DonateButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }
    }

    #endregion
}
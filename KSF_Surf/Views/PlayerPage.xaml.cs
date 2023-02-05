using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class PlayerPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;

        // object used by "Info" call
        private SteamProfile playerSteamProfile;
        private PlayerInfoDatum playerInfoData;

        // variables for current filters
        private EFilter_Game game = EFilter_Game.none;
        private readonly EFilter_Game defaultGame = BaseViewModel.propertiesDict_getGame();

        private EFilter_Mode mode = EFilter_Mode.none;
        private readonly EFilter_Mode defaultMode = BaseViewModel.propertiesDict_getMode();

        private readonly string meSteamId = BaseViewModel.propertiesDict_getSteamID();
        private EFilter_PlayerType playerType = EFilter_PlayerType.none;
        private string playerValue = "";
        private string playerSteamId;
        private string playerRank;
        private EFilter_PlayerWRsType wrsType;
        private bool hasTop;

        // date of last refresh
        private DateTime lastRefresh;

        public PlayerPage()
        {
            playerViewModel = new PlayerViewModel();
            InitializeComponent();

            // Refresh command
            PlayerRefreshView.Command = new Command(async () =>
            {
                if (hasLoaded)
                {
                    if (BaseViewModel.hasConnection())
                    {
                        TimeSpan sinceRefresh = DateTime.Now - lastRefresh;
                        bool tooSoon = sinceRefresh.TotalSeconds < 10;

                        if (tooSoon)
                        {
                            await Task.Delay(500); // 0.5 seconds
                        }
                        else
                        {
                            await ChangePlayerInfo(game, mode, playerType, playerValue);
                            lastRefresh = DateTime.Now;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Unable to refresh", "Please connect to the Internet.", "OK");
                    }
                }

                PlayerRefreshView.IsRefreshing = false;
            });
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangePlayerInfo(EFilter_Game newGame, EFilter_Mode newMode, EFilter_PlayerType newPlayerType, string newPlayerValue)
        {
            var playerInfoDatum = await playerViewModel.GetPlayerInfo(newGame, newMode, newPlayerType, newPlayerValue);
            playerInfoData = playerInfoDatum?.data;
            if (playerInfoData is null || playerInfoData.basicInfo is null)
            {
                await DisplayAlert("Could not find player profile!", "Invalid SteamID or rank.", "OK");
                return;
            }

            playerType = newPlayerType;
            playerValue = newPlayerValue;
            game = newGame;
            mode = newMode;
            playerSteamId = playerInfoData.basicInfo.steamID;
            playerRank = playerInfoData.SurfRank;

            string playerName = playerInfoData.basicInfo.name;
            if (playerName.Length > 18)
            {
                playerName = playerName.Substring(0, 13) + "...";
            }
            Title = playerName + " [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";

            var PlayerSteamDatum = await playerViewModel.GetPlayerSteamProfile(playerSteamId);
            playerSteamProfile = PlayerSteamDatum?.response.players[0];

            wrsType = EFilter_PlayerWRsType.none;
            LayoutPlayerInfo();
            LayoutPlayerProfile();
        }


        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutPlayerProfile()
        {
            if (playerSteamProfile is null) PlayerImage.Source = "failed_steam_profile.png";
            else PlayerImage.Source = playerSteamProfile.avatarfull;

            PlayerNameLabel.Text = playerInfoData.basicInfo.name;
            PlayerCountryLabel.Text = String_Formatter.toEmoji_Country(playerInfoData.basicInfo.country) + " " + playerInfoData.basicInfo.country;

            List<string> attributes = new List<string>();
            if (!(playerInfoData.banStatus is bool))
            {
                attributes.Add("BANNED");
            }
            if (playerInfoData.KSFStatus != null)
            {
                attributes.Add("KSF");
            }
            if (playerInfoData.vipStatus != null)
            {
                attributes.Add("VIP");
            }
            if (playerInfoData.adminStatus != null)
            {
                attributes.Add("Admin");
            }
            if (playerInfoData.mapperID != null)
            {
                attributes.Add("Mapper");
            }
            PlayerAttributesLabel.Text = string.Join(" | ", attributes);

            string rankTitle = EFilter_ToString.getRankTitle(playerInfoData.SurfRank, playerInfoData.playerPoints.points);
            Color rankColor = new Color();
            switch (rankTitle)
            {
                case "MASTER": rankColor = EFilter_ToString.rankColors[0]; break;
                case "ELITE": rankColor = EFilter_ToString.rankColors[1]; break;
                case "VETERAN": rankColor = EFilter_ToString.rankColors[2]; break;
                case "PRO": rankColor = EFilter_ToString.rankColors[3]; break;
                case "EXPERT": rankColor = EFilter_ToString.rankColors[4]; break;
                case "HOTSHOT": rankColor = EFilter_ToString.rankColors[5]; break;
                case "EXCEPTIONAL": rankColor = EFilter_ToString.rankColors[6]; break;
                case "SEASONED": rankColor = EFilter_ToString.rankColors[7]; break;
                case "EXPERIENCED": rankColor = EFilter_ToString.rankColors[8]; break;
                case "ACCOMPLISHED": rankColor = EFilter_ToString.rankColors[9]; break;
                case "ADEPT": rankColor = EFilter_ToString.rankColors[10]; break;
                case "PROFICIENT": rankColor = EFilter_ToString.rankColors[11]; break;
                case "SKILLED": rankColor = EFilter_ToString.rankColors[12]; break;
                case "CASUAL": rankColor = EFilter_ToString.rankColors[13]; break;
                case "BEGINNER": rankColor = EFilter_ToString.rankColors[14]; break;
                case "ROOKIE": rankColor = EFilter_ToString.rankColors[15]; break;
            }
            RankTitleLabel.Text = rankTitle;
            RankTitleLabel.TextColor = rankColor;
            PlayerImageFrame.BorderColor = rankColor;
        }

        private void LayoutPlayerInfo()
        {
            // Info -----------------------------------------------------------
            RankLabel.Text = String_Formatter.toString_Int(playerInfoData.SurfRank);
            PointsLabel.Text = String_Formatter.toString_Points(playerInfoData.playerPoints.points);
            CompletionLabel.Text = playerInfoData.percentCompletion + "%";
            
            WRsLabel.Text = playerInfoData.WRZones.wr;
            WRCPsLabel.Text = String_Formatter.toString_Int(playerInfoData.WRZones.wrcp);
            WRBsLabel.Text = String_Formatter.toString_Points(playerInfoData.WRZones.wrb);

            if (int.Parse(playerInfoData.WRZones.wr) > 0)
            {
                wrsType = EFilter_PlayerWRsType.wr;
            }
            else if (int.Parse(playerInfoData.WRZones.wrcp) > 0)
            {
                wrsType = EFilter_PlayerWRsType.wrcp;
            }
            else if (int.Parse(playerInfoData.WRZones.wrb) > 0)
            {
                wrsType = EFilter_PlayerWRsType.wrb;
            }
            WRsFrame.IsVisible = (wrsType != EFilter_PlayerWRsType.none);

            FirstOnlineLabel.Text = String_Formatter.toString_KSFDate(playerInfoData.basicInfo.firstOnline);
            LastSeenLabel.Text = String_Formatter.toString_LastOnline(playerInfoData.basicInfo.lastOnline);


            SurfTimeLabel.Text = String_Formatter.toString_PlayTime(playerInfoData.basicInfo.aliveTime, true);
            SpecTimeLabel.Text = String_Formatter.toString_PlayTime(playerInfoData.basicInfo.deadTime, true);

            // Completion -----------------------------------------------------
            MapsValueLabel.Text = playerInfoData.CompletedZones.map + "/" + playerInfoData.TotalZones.TotalMaps;
            MapsValueLabel.Text += " (" + String_Formatter.toString_CompletionPercent(playerInfoData.CompletedZones.map, playerInfoData.TotalZones.TotalMaps) + ")";

            StagesValueLabel.Text = String_Formatter.toString_Points(playerInfoData.CompletedZones.stage) 
                + "/" + String_Formatter.toString_Points(playerInfoData.TotalZones.TotalStages);
            StagesValueLabel.Text += " (" + String_Formatter.toString_CompletionPercent(playerInfoData.CompletedZones.stage, playerInfoData.TotalZones.TotalStages) + ")";

            BonusesValueLabel.Text = String_Formatter.toString_Points(playerInfoData.CompletedZones.bonus) 
                + "/" + String_Formatter.toString_Points(playerInfoData.TotalZones.TotalBonuses);
            BonusesValueLabel.Text += " (" + String_Formatter.toString_CompletionPercent(playerInfoData.CompletedZones.bonus, playerInfoData.TotalZones.TotalBonuses) + ")";

            // Groups ---------------------------------------------------------
            Top10sLabel.Text = playerInfoData.Top10Groups.top10;
            hasTop = (playerInfoData.Top10Groups.top10 != "0");
            if (hasTop)
            {
                if (playerInfoData.Top10Groups.rank1 != "0")
                {
                    R1GroupLabel.IsVisible = true;
                    R1ValueLabel.IsVisible = true;
                    R1ValueLabel.Text = playerInfoData.Top10Groups.rank1;
                }
                else
                {
                    R1GroupLabel.IsVisible = false;
                    R1ValueLabel.IsVisible = false;
                }

                if (playerInfoData.Top10Groups.rank2 != "0")
                {
                    R2GroupLabel.IsVisible = true;
                    R2ValueLabel.IsVisible = true;
                    R2ValueLabel.Text = playerInfoData.Top10Groups.rank2;
                }
                else
                {
                    R2GroupLabel.IsVisible = false;
                    R2ValueLabel.IsVisible = false;
                }

                if (playerInfoData.Top10Groups.rank3 != "0")
                {
                    R3GroupLabel.IsVisible = true;
                    R3ValueLabel.IsVisible = true;
                    R3ValueLabel.Text = playerInfoData.Top10Groups.rank3;
                }
                else
                {
                    R3GroupLabel.IsVisible = false;
                    R3ValueLabel.IsVisible = false;
                }

                if (playerInfoData.Top10Groups.rank4 != "0")
                {
                    R4GroupLabel.IsVisible = true;
                    R4ValueLabel.IsVisible = true;
                    R4ValueLabel.Text = playerInfoData.Top10Groups.rank4;
                }
                else
                {
                    R4GroupLabel.IsVisible = false;
                    R4ValueLabel.IsVisible = false;
                }

                if (playerInfoData.Top10Groups.rank5 != "0")
                {
                    R5GroupLabel.IsVisible = true;
                    R5ValueLabel.IsVisible = true;
                    R5ValueLabel.Text = playerInfoData.Top10Groups.rank5;
                }
                else
                {
                    R5GroupLabel.IsVisible = false;
                    R5ValueLabel.IsVisible = false;
                }

                if (playerInfoData.Top10Groups.rank6 != "0")
                {
                    R6GroupLabel.IsVisible = true;
                    R6ValueLabel.IsVisible = true;
                    R6ValueLabel.Text = playerInfoData.Top10Groups.rank6;
                }
                else
                {
                    R6GroupLabel.IsVisible = false;
                    R6ValueLabel.IsVisible = false;
                }

                if (playerInfoData.Top10Groups.rank7 != "0")
                {
                    R7GroupLabel.IsVisible = true;
                    R7ValueLabel.IsVisible = true;
                    R7ValueLabel.Text = playerInfoData.Top10Groups.rank7;
                }
                else
                {
                    R7GroupLabel.IsVisible = false;
                    R7ValueLabel.IsVisible = false;
                }

                if (playerInfoData.Top10Groups.rank8 != "0")
                {
                    R8GroupLabel.IsVisible = true;
                    R8ValueLabel.IsVisible = true;
                    R8ValueLabel.Text = playerInfoData.Top10Groups.rank8;
                }
                else
                {
                    R8GroupLabel.IsVisible = false;
                    R8ValueLabel.IsVisible = false;
                }

                if (playerInfoData.Top10Groups.rank9 != "0")
                {
                    R9GroupLabel.IsVisible = true;
                    R9ValueLabel.IsVisible = true;
                    R9ValueLabel.Text = playerInfoData.Top10Groups.rank9;
                }
                else
                {
                    R9GroupLabel.IsVisible = false;
                    R9ValueLabel.IsVisible = false;
                }

                if (playerInfoData.Top10Groups.rank10 != "0")
                {
                    R10GroupLabel.IsVisible = true;
                    R10ValueLabel.IsVisible = true;
                    R10ValueLabel.Text = playerInfoData.Top10Groups.rank10;
                }
                else
                {
                    R10GroupLabel.IsVisible = false;
                    R10ValueLabel.IsVisible = false;
                }
            }
            else
            {
                R1GroupLabel.IsVisible = false;
                R1ValueLabel.IsVisible = false;
                R2GroupLabel.IsVisible = false;
                R2ValueLabel.IsVisible = false;
                R3GroupLabel.IsVisible = false;
                R3ValueLabel.IsVisible = false;
                R4GroupLabel.IsVisible = false;
                R4ValueLabel.IsVisible = false;
                R5GroupLabel.IsVisible = false;
                R5ValueLabel.IsVisible = false;
                R6GroupLabel.IsVisible = false;
                R6ValueLabel.IsVisible = false;
                R7GroupLabel.IsVisible = false;
                R7ValueLabel.IsVisible = false;
                R8GroupLabel.IsVisible = false;
                R8ValueLabel.IsVisible = false;
                R9GroupLabel.IsVisible = false;
                R9ValueLabel.IsVisible = false;
                R10GroupLabel.IsVisible = false;
                R10ValueLabel.IsVisible = false;
            }

            G1sLabel.Text = playerInfoData.Top10Groups.g1;
            G2sLabel.Text = playerInfoData.Top10Groups.g2;
            G3sLabel.Text = playerInfoData.Top10Groups.g3;
            G4sLabel.Text = playerInfoData.Top10Groups.g4;
            G5sLabel.Text = playerInfoData.Top10Groups.g5;
            G6sLabel.Text = playerInfoData.Top10Groups.g6;
            GroupsLabel.Text = playerInfoData.Top10Groups.groups;

            // Points Stack ------------------------------------------------------
            Top10PtsLabel.Text = String_Formatter.toString_Points(playerInfoData.playerPoints.top10);
            GroupsPtsLabel.Text = String_Formatter.toString_Points(playerInfoData.playerPoints.groups);
            MapsPtsLabel.Text = String_Formatter.toString_Points(playerInfoData.playerPoints.map);

            string wrcpPoints = "";
            if (playerInfoData.playerPoints.wrcp != "0")
            {
                wrcpPoints = "[+" + String_Formatter.toString_Points(playerInfoData.playerPoints.wrcp) + "] ";
            }
            StagesPtsLabel.Text = wrcpPoints + String_Formatter.toString_Points(playerInfoData.playerPoints.stage);

            string wrbPoints = "";
            if (playerInfoData.playerPoints.wrb != "0")
            {
                wrbPoints = "[+" + String_Formatter.toString_Points(playerInfoData.playerPoints.wrb) + "] ";
            }
            BonusesPtsLabel.Text = wrbPoints + String_Formatter.toString_Points(playerInfoData.playerPoints.bonus);
        }


        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        internal async Task OnChangedTabAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;
                await ChangePlayerInfo(defaultGame, defaultMode, EFilter_PlayerType.me, meSteamId);

                LoadingAnimation.IsRunning = false;
                PlayerPageScrollView.IsVisible = true;
            }
        }

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (hasLoaded && BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new PlayerFilterPage(ApplyFilters,
                    game, mode, playerType, playerSteamId, playerRank,
                    defaultGame, defaultMode, meSteamId));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
        }

        internal async void ApplyFilters(EFilter_Game newGame, EFilter_Mode newMode, EFilter_PlayerType newPlayerType, string newPlayerValue)
        {
            if (BaseViewModel.hasConnection())
            {
                LoadingAnimation.IsRunning = true;
                await ChangePlayerInfo(newGame, newMode, newPlayerType, newPlayerValue);
                LoadingAnimation.IsRunning = false;
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            await PlayerPageScrollView.ScrollToAsync(0, 0, true);
        }

        private async void RecentRecordsSet_Tapped(object sender, EventArgs e)
        {
            RecentRecordsSetButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new PlayerRecentSetRecordsPage(Title, game, mode, playerType, playerValue));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            RecentRecordsSetButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void RecentRecordsBroken_Tapped(object sender, EventArgs e)
        {
            RecentRecordsBrokenButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new PlayerRecentBrokenRecordsPage(Title, game, mode, playerType, playerValue));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            RecentRecordsBrokenButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void OldestRecords_Tapped(object sender, EventArgs e)
        {
            OldestRecordsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new PlayerOldestRecordsPage(Title, game, mode, playerType, playerValue, wrsType, hasTop));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            OldestRecordsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void WorldRecords_Tapped(object sender, EventArgs e)
        {
            WRsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new PlayerWorldRecordsPage(Title, game, mode, playerType, playerValue, wrsType));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            WRsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void TierCompletion_Tapped(object sender, EventArgs e)
        {
            TierCompletionButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new PlayerCompletionPage(Title, game, mode, playerType, playerValue));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            TierCompletionButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void CompleteMaps_Tapped(object sender, EventArgs e)
        {
            CompleteMapsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new PlayerMapsCompletionPage(Title, game, mode, EFilter_PlayerCompletionType.complete, 
                    playerType, playerValue));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            CompleteMapsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void IncompleteMaps_Tapped(object sender, EventArgs e)
        {
            IncompleteMapsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new PlayerMapsCompletionPage(Title, game, mode, EFilter_PlayerCompletionType.incomplete,
                    playerType, playerValue));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            IncompleteMapsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void Settings_Pressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        private async Task DisplayNoConnectionAlert()
        {
            await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }
        #endregion
    }
}
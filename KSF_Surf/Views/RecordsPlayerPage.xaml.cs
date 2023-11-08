using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsPlayerPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;

        // object used by "Info" call
        private SteamProfileDatum playerSteamProfile;
        private PlayerInfoDatum playerInfoData;

        // variables for current filters
        private GameEnum game = GameEnum.NONE;
        private ModeEnum mode = ModeEnum.NONE;
        private PlayerTypeEnum playerType = PlayerTypeEnum.STEAM_ID;
        private string playerValue = "";
        private string playerSteamID;
        private PlayerWorldRecordsTypeEnum wrsType;
        private bool hasTop;

        // date of last refresh
        private DateTime lastRefresh;

        public RecordsPlayerPage(GameEnum game, ModeEnum mode, string playerSteamID)
        {
            this.game = game;
            this.mode = mode;
            this.playerSteamID = playerSteamID;

            playerViewModel = new PlayerViewModel();

            InitializeComponent();

            // Refresh command
            PlayerRefreshView.Command = new Command(async () =>
            {
                if (hasLoaded)
                {
                    if (BaseViewModel.HasConnection())
                    {
                        TimeSpan sinceRefresh = DateTime.Now - lastRefresh;
                        if (sinceRefresh.TotalSeconds < 10) await Task.Delay(500); // 0.5 seconds
                        else
                        {
                            await ChangePlayerInfo(game, mode, playerType, playerValue);
                            lastRefresh = DateTime.Now;
                        }
                    }
                    else await DisplayAlert("Unable to refresh", "Please connect to the Internet.", "OK");
                }

                PlayerRefreshView.IsRefreshing = false;
            });
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangePlayerInfo(GameEnum newGame, ModeEnum newMode, PlayerTypeEnum newPlayerType, string newPlayerValue)
        {
            var playerInfoDatum = await playerViewModel.GetPlayerInfo(newGame, newMode, newPlayerType, newPlayerValue);
            playerInfoData = playerInfoDatum?.data;
            if (playerInfoData is null || playerInfoData.basicInfo is null)
            {
                await DisplayAlert("Could not find player profile!", "Invalid SteamID", "OK");
                return;
            }

            playerType = newPlayerType;
            playerValue = newPlayerValue;
            game = newGame;
            mode = newMode;
            playerSteamID = playerInfoData.basicInfo.steamID;

            string playerName = playerInfoData.basicInfo.name;
            if (playerName.Length > 18) playerName = playerName.Substring(0, 15) + "...";
            Title = playerName + " [" + EnumToString.NameString(game) + ", " + EnumToString.NameString(mode) + "]";

            wrsType = PlayerWorldRecordsTypeEnum.NONE;
            LayoutPlayerInfo();
            LayoutPlayerProfile();
        }

        private async Task ChangePlayerImage()
        {
            PlayerImage.Source = "";

            var PlayerSteamDatum = await playerViewModel.GetPlayerSteamProfile(playerSteamID);
            playerSteamProfile = PlayerSteamDatum?.response.players[0];

            if (playerSteamProfile is null) return;
            PlayerImage.Source = playerSteamProfile.avatarfull;
        }


        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutPlayerProfile()
        {
            if (playerSteamProfile is null) PlayerImage.Source = "failed_steam_profile.png";
            else PlayerImage.Source = playerSteamProfile.avatarfull;

            PlayerNameLabel.Text = playerInfoData.basicInfo.name;
            PlayerCountryLabel.Text = StringFormatter.CountryEmoji(playerInfoData.basicInfo.country) + " " + playerInfoData.basicInfo.country;

            List<string> attributes = new List<string>();
            if (playerInfoData.banStatus) attributes.Add("BANNED");
            if (playerInfoData.muteStatus) attributes.Add("MUTED");
            if (playerInfoData.KSFStatus != null) attributes.Add("KSF");
            if (playerInfoData.vipStatus != null) attributes.Add("VIP");
            if (playerInfoData.adminStatus != null) attributes.Add("Admin");
            if (playerInfoData.mapperID != null) attributes.Add("Mapper");
            PlayerAttributesLabel.Text = string.Join(" | ", attributes);

            string rankTitle = StringFormatter.RankTitleString(playerInfoData.SurfRank, playerInfoData.playerPoints.points);
            Color rankColor = new Color();
            switch (rankTitle)
            {
                case "MASTER": rankColor = StringFormatter.RankColors[0]; break;
                case "ELITE": rankColor = StringFormatter.RankColors[1]; break;
                case "VETERAN": rankColor = StringFormatter.RankColors[2]; break;
                case "PRO": rankColor = StringFormatter.RankColors[3]; break;
                case "EXPERT": rankColor = StringFormatter.RankColors[4]; break;
                case "HOTSHOT": rankColor = StringFormatter.RankColors[5]; break;
                case "EXCEPTIONAL": rankColor = StringFormatter.RankColors[6]; break;
                case "SEASONED": rankColor = StringFormatter.RankColors[7]; break;
                case "EXPERIENCED": rankColor = StringFormatter.RankColors[8]; break;
                case "ACCOMPLISHED": rankColor = StringFormatter.RankColors[9]; break;
                case "ADEPT": rankColor = StringFormatter.RankColors[10]; break;
                case "PROFICIENT": rankColor = StringFormatter.RankColors[11]; break;
                case "SKILLED": rankColor = StringFormatter.RankColors[12]; break;
                case "CASUAL": rankColor = StringFormatter.RankColors[13]; break;
                case "BEGINNER": rankColor = StringFormatter.RankColors[14]; break;
                case "ROOKIE": rankColor = StringFormatter.RankColors[15]; break;
            }

            RankTitleLabel.Text = rankTitle;
            RankTitleLabel.TextColor = rankColor;
            PlayerImageFrame.BackgroundColor = rankColor;
        }

        private void LayoutPlayerInfo()
        {
            // Info -----------------------------------------------------------
            RankLabel.Text = StringFormatter.IntString(playerInfoData.SurfRank);
            PointsLabel.Text = StringFormatter.PointsString(playerInfoData.playerPoints.points);
            CompletionLabel.Text = playerInfoData.percentCompletion + "%";

            WRsLabel.Text = playerInfoData.WRZones.wr;
            WRCPsLabel.Text = StringFormatter.IntString(playerInfoData.WRZones.wrcp);
            WRBsLabel.Text = StringFormatter.PointsString(playerInfoData.WRZones.wrb);

            if (int.Parse(playerInfoData.WRZones.wr) > 0) wrsType = PlayerWorldRecordsTypeEnum.WR;
            else if (int.Parse(playerInfoData.WRZones.wrcp) > 0) wrsType = PlayerWorldRecordsTypeEnum.WRCP;
            else if (int.Parse(playerInfoData.WRZones.wrb) > 0) wrsType = PlayerWorldRecordsTypeEnum.WRB;
            WRsFrame.IsVisible = (wrsType != PlayerWorldRecordsTypeEnum.NONE);

            FirstOnlineLabel.Text = StringFormatter.KSFDateString(playerInfoData.basicInfo.firstOnline);
            LastSeenLabel.Text = StringFormatter.LastOnlineString(playerInfoData.basicInfo.lastOnline);

            SurfTimeLabel.Text = StringFormatter.PlayTimeString(playerInfoData.basicInfo.aliveTime, true);
            SpecTimeLabel.Text = StringFormatter.PlayTimeString(playerInfoData.basicInfo.deadTime, true);

            // Completion -----------------------------------------------------
            MapsValueLabel.Text = playerInfoData.CompletedZones.map + "/" + playerInfoData.TotalZones.TotalMaps;
            MapsValueLabel.Text += " (" + StringFormatter.CompletionPercentString(playerInfoData.CompletedZones.map, playerInfoData.TotalZones.TotalMaps) + ")";

            StagesValueLabel.Text = StringFormatter.PointsString(playerInfoData.CompletedZones.stage)
                + "/" + StringFormatter.PointsString(playerInfoData.TotalZones.TotalStages);
            StagesValueLabel.Text += " (" + StringFormatter.CompletionPercentString(playerInfoData.CompletedZones.stage, playerInfoData.TotalZones.TotalStages) + ")";

            BonusesValueLabel.Text = StringFormatter.PointsString(playerInfoData.CompletedZones.bonus)
                + "/" + StringFormatter.PointsString(playerInfoData.TotalZones.TotalBonuses);
            BonusesValueLabel.Text += " (" + StringFormatter.CompletionPercentString(playerInfoData.CompletedZones.bonus, playerInfoData.TotalZones.TotalBonuses) + ")";

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
            Top10PtsLabel.Text = StringFormatter.PointsString(playerInfoData.playerPoints.top10);
            GroupsPtsLabel.Text = StringFormatter.PointsString(playerInfoData.playerPoints.groups);
            MapsPtsLabel.Text = StringFormatter.PointsString(playerInfoData.playerPoints.map);

            string wrcpPoints = "";
            if (playerInfoData.playerPoints.wrcp != "0")
            {
                wrcpPoints = "[+" + StringFormatter.PointsString(playerInfoData.playerPoints.wrcp) + "] ";
            }
            StagesPtsLabel.Text = wrcpPoints + StringFormatter.PointsString(playerInfoData.playerPoints.stage);

            string wrbPoints = "";
            if (playerInfoData.playerPoints.wrb != "0")
            {
                wrbPoints = "[+" + StringFormatter.PointsString(playerInfoData.playerPoints.wrb) + "] ";
            }
            BonusesPtsLabel.Text = wrbPoints + StringFormatter.PointsString(playerInfoData.playerPoints.bonus);
        }


        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;
            
                await ChangePlayerInfo(game, mode, playerType, playerSteamID);

                LoadingAnimation.IsRunning = false;
                RecordsPlayerPageScrollView.IsVisible = true;

                await ChangePlayerImage();
                PlayerImageLoadingAnimation.IsRunning = false;
            }
        }

        private async void Copy_Pressed(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(playerSteamID);
            await DisplayAlert("Steam ID copied to clipboard.", "", "OK");
        }

        private async void RecentRecordsSet_Tapped(object sender, EventArgs e)
        {
            RecentRecordsSetButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerRecentSetRecordsPage(Title, game, mode, playerType, playerValue));
            }
            else await DisplayNoConnectionAlert();

            RecentRecordsSetButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void RecentRecordsBroken_Tapped(object sender, EventArgs e)
        {
            RecentRecordsBrokenButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerRecentBrokenRecordsPage(Title, game, mode, playerType, playerValue));
            }
            else await DisplayNoConnectionAlert();

            RecentRecordsBrokenButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void OldestRecords_Tapped(object sender, EventArgs e)
        {
            OldestRecordsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerOldestRecordsPage(Title, game, mode, playerType, playerValue, wrsType, hasTop));
            }
            else await DisplayNoConnectionAlert();

            OldestRecordsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void WorldRecords_Tapped(object sender, EventArgs e)
        {
            WRsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerWorldRecordsPage(Title, game, mode, playerType, playerValue, wrsType));
            }
            else await DisplayNoConnectionAlert();

            WRsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void TierCompletion_Tapped(object sender, EventArgs e)
        {
            TierCompletionButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerCompletionPage(Title, game, mode, playerType, playerValue));
            }
            else await DisplayNoConnectionAlert();

            TierCompletionButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void CompleteMaps_Tapped(object sender, EventArgs e)
        {
            CompleteMapsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerMapsCompletionPage(Title, game, mode, PlayerCompletionTypeEnum.COMPLETE,
                    playerType, playerValue));
            }
            else await DisplayNoConnectionAlert();

            CompleteMapsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void IncompleteMaps_Tapped(object sender, EventArgs e)
        {
            IncompleteMapsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerMapsCompletionPage(Title, game, mode, PlayerCompletionTypeEnum.INCOMPLETE,
                    playerType, playerValue));
            }
            else await DisplayNoConnectionAlert();

            IncompleteMapsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async Task DisplayNoConnectionAlert()
        {
            await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }

        #endregion
    }
}
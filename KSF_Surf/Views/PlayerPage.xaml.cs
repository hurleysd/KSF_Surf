using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using KSF_Surf.ViewModels;
using KSF_Surf.Models;
using P42.Utils;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class PlayerPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;

        // object used by "Info" call
        private PlayerInfoDatum playerInfoData;

        // variables for current filters
        private GameEnum game = GameEnum.NONE;
        private readonly GameEnum defaultGame = PropertiesDict.GetGame();

        private ModeEnum mode = ModeEnum.NONE;
        private readonly ModeEnum defaultMode = PropertiesDict.GetMode();

        private string meSteamID = PropertiesDict.GetSteamID();
        private PlayerTypeEnum playerType = PlayerTypeEnum.NONE;
        private string playerValue = "";
        private string playerSteamID = "";
        private string playerRank;
        private PlayerWorldRecordsTypeEnum wrsType;
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
                    else await ViewsCommon.DisplayNoConnectionAlert(this);
                }

                PlayerRefreshView.IsRefreshing = false;
            });
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private void ChangeTitle(GameEnum game, ModeEnum mode, string playerName)
        {
            Title = "[" + EnumToString.NameString(game) + "]";
            if (mode != ModeEnum.FW) Title += "[" + EnumToString.NameString(mode) + "]";
            Title += " " + playerName;
        }

        private async Task ChangePlayerInfo(GameEnum newGame, ModeEnum newMode, PlayerTypeEnum newPlayerType, string newPlayerValue)
        {
            var playerInfoDatum = await playerViewModel.GetPlayerInfo(newGame, newMode, newPlayerType, newPlayerValue);
            playerInfoData = playerInfoDatum?.data;
            if (playerInfoData is null || playerInfoData.basicInfo is null)
            {
                await ViewsCommon.DisplayProfileFailureAlert(this, true);
                return;
            }

            playerType = newPlayerType;
            playerValue = newPlayerValue;
            game = newGame;
            mode = newMode;
            playerSteamID = playerInfoData.basicInfo.steamID;
            playerRank = playerInfoData.SurfRank;

            ChangeTitle(game, mode, playerInfoData.basicInfo.name);

            wrsType = PlayerWorldRecordsTypeEnum.NONE;
            LayoutPlayerInfo();
            LayoutPlayerProfile();
        }

        private async Task ChangePlayerImage()
        {
            PlayerImage.Source = "";

            var playerSteamDatum = await playerViewModel.GetPlayerSteamProfile(playerSteamID);
            var playerSteamResponse = playerSteamDatum?.response;
            if (playerSteamResponse is null || playerSteamResponse.players.IsNullOrEmpty()) return;
            var playerSteamProfile = playerSteamResponse.players[0];
            if (playerSteamProfile is null) return;
            PlayerImage.Source = playerSteamProfile.avatarfull;
        }


        // Displaying Changes -------------------------------------------------------------------------------

        private void LayoutPlayerProfile()
        {
            PlayerNameLabel.Text = playerInfoData.basicInfo.name;
            PlayerCountryLabel.Text = StringFormatter.CountryEmoji(playerInfoData.basicInfo.country) + " " + playerInfoData.basicInfo.country;

            List<string> attributes = new List<string>();
            if (playerInfoData.banStatus.ToString() != "False") attributes.Add("BANNED");
            if (playerInfoData.muteStatus.ToString() != "False") attributes.Add("MUTED");
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

        internal async Task OnChangedTabAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;

                meSteamID = PropertiesDict.GetSteamID(); // get the player steam ID again in case they set it for the first time
                await ChangePlayerInfo(defaultGame, defaultMode, PlayerTypeEnum.ME, meSteamID);

                LoadingAnimation.IsRunning = false;
                PlayerPageScrollView.IsVisible = true;

                await ChangePlayerImage();
                PlayerImageLoadingAnimation.IsRunning = false;
            }
        }

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (hasLoaded && BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerFilterPage(ApplyFilters,
                    game, mode, playerType, playerSteamID, playerRank,
                    defaultGame, defaultMode, meSteamID));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);
        }

        internal async void ApplyFilters(GameEnum newGame, ModeEnum newMode, PlayerTypeEnum newPlayerType, string newPlayerValue)
        {
            if (BaseViewModel.HasConnection())
            {
                LoadingAnimation.IsRunning = true;
                await ChangePlayerInfo(newGame, newMode, newPlayerType, newPlayerValue);
                LoadingAnimation.IsRunning = false;

                PlayerImageLoadingAnimation.IsRunning = true;
                await ChangePlayerImage();
                PlayerImageLoadingAnimation.IsRunning = false;
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            await PlayerPageScrollView.ScrollToAsync(0, 0, true);
        }

        private async void RecentRecordsSet_Tapped(object sender, EventArgs e)
        {
            RecentRecordsSetButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerRecentSetRecordsPage(Title, game, mode, playerType, playerValue));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            RecentRecordsSetButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void RecentRecordsBroken_Tapped(object sender, EventArgs e)
        {
            RecentRecordsBrokenButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerRecentBrokenRecordsPage(Title, game, mode, playerType, playerValue));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            RecentRecordsBrokenButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void OldestRecords_Tapped(object sender, EventArgs e)
        {
            OldestRecordsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerOldestRecordsPage(Title, game, mode, playerType, playerValue, wrsType, hasTop));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            OldestRecordsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void WorldRecords_Tapped(object sender, EventArgs e)
        {
            WRsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerWorldRecordsPage(Title, game, mode, playerType, playerValue, wrsType));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            WRsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void TierCompletion_Tapped(object sender, EventArgs e)
        {
            TierCompletionButton.Style = App.Current.Resources["TappedStackStyle"] as Style;

            if (BaseViewModel.HasConnection())
            {
                await Navigation.PushAsync(new PlayerCompletionPage(Title, game, mode, playerType, playerValue));
            }
            else await ViewsCommon.DisplayNoConnectionAlert(this);

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
            else await ViewsCommon.DisplayNoConnectionAlert(this);

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
            else await ViewsCommon.DisplayNoConnectionAlert(this);

            IncompleteMapsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void Settings_Pressed(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        #endregion
    }
}
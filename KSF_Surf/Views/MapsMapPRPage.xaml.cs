using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsMapPRPage : ContentPage
    {
        private readonly MapsViewModel mapsViewModel;
        private readonly string mapsMapTitle;
        private bool hasLoaded = false;

        // objects used by "Personal Record" call
        private MapPRInfoDatum prInfoData;

        // variables for filters
        private readonly EFilter_Game game;
        private EFilter_Mode currentMode = EFilter_Mode.none;
        private readonly EFilter_Mode defaultMode = BaseViewModel.propertiesDict_getMode();
        private readonly string map;
        
        private EFilter_PlayerType playerType;
        private string playerValue;
        private string playerSteamID;
        private string playerRank;
        private readonly string meSteamID  = BaseViewModel.propertiesDict_getSteamID();

        public MapsMapPRPage(string title, MapsViewModel mapsViewModel, EFilter_Game game, string map, bool hasZones)
        {
            mapsMapTitle = title;
            this.mapsViewModel = mapsViewModel;
            this.game = game;
            this.map = map;
            playerValue = meSteamID;
            playerSteamID = meSteamID;
            playerType = EFilter_PlayerType.me;

            InitializeComponent();
            Title = mapsMapTitle + " " + EFilter_ToString.toString(defaultMode) + "]";
            if (!hasZones) ZoneRecordsOption.IsVisible = false;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangePR(EFilter_Mode newMode, EFilter_PlayerType newPlayerType, string newPlayerValue)
        {
            if (currentMode == newMode && newPlayerType == playerType && newPlayerValue == playerValue) return;

            var prInfoDatum = await mapsViewModel.GetMapPRInfo(game, newMode, map, newPlayerType, newPlayerValue);
            prInfoData = prInfoDatum?.data;
            if (prInfoData is null || prInfoData.basicInfo is null)
            {
                hidePR();
                await DisplayAlert("Could not find player profile!", "Invalid SteamID or rank.", "OK");
                return;
            }

            if (prInfoData.time is null || prInfoData.time == "0")
            {
                hidePR();
                await DisplayAlert("Could not find personal record!", "Player has not completed the map.", "OK");
                return;
            }
            displayPR();

            currentMode = newMode;
            playerType = newPlayerType;
            playerValue = newPlayerValue;
            Title = mapsMapTitle + " " + EFilter_ToString.toString(currentMode) + "]";

            PRTitleLabel.Text = String_Formatter.toEmoji_Country(prInfoData.basicInfo.country) + prInfoData.basicInfo.name + "'s";
            playerRank = prInfoData.rank.ToString();
            playerSteamID = prInfoData.basicInfo.steamID;
            LayoutPRInfo();
        }

        

        // Dispaying Changes -------------------------------------------------------------------------------

        private void LayoutPRInfo()
        {
            // Info -----------------------------------------------------------------
            string time = String_Formatter.toString_RankTime(prInfoData.time) + " (WR";
            if (prInfoData.rank == 1)
            {
                if (prInfoData.r2Diff is null || prInfoData.r2Diff == "0")
                {
                    time += " N/A";
                }
                else
                {
                    time += "-" + String_Formatter.toString_RankTime(prInfoData.r2Diff.Substring(1));
                }
            }
            else
            {
                time += "+" + String_Formatter.toString_RankTime(prInfoData.wrDiff);
            }
            TimeLabel.Text = time + ")";

            string rank = prInfoData.rank + "/" + prInfoData.totalRanks;
            if (!(prInfoData.group is null))
            {
                if (prInfoData.rank == 1)
                {
                    rank = "[WR] " + rank;
                }
                else if (prInfoData.rank <= 10)
                {
                    rank = "[Top10] " + rank;
                }
                else
                {
                    rank = "[G" + prInfoData.group + "] " + rank;
                } 
            }
            RankLabel.Text = rank;

            CompletionsLabel.Text = prInfoData.count;
            if (!(prInfoData.attempts is null))
            {
                string percent = String_Formatter.toString_CompletionPercent(prInfoData.count, prInfoData.attempts);
                CompletionsLabel.Text += "/" + prInfoData.attempts + " (" + percent + ")";
            }

            if (!(prInfoData.total_time is null))
            {
                ZoneTimeLabel.Text = String_Formatter.toString_PlayTime(prInfoData.total_time, true);
            }
            else 
            {
                ZoneTimeLabel.Text = "";
            }

            DateSetLabel.Text = String_Formatter.toString_KSFDate(prInfoData.date);
            if (!(prInfoData.date_lastplayed is null))
            {
                LastPlayedLabel.Text = String_Formatter.toString_LastOnline(prInfoData.date_lastplayed);
            }
            else
            {
                LastPlayedLabel.Text = "";
            }

            // Velocity -------------------------------------------------------------
            string units = " u/s";
            AvgVelLabel.Text = ((int)double.Parse(prInfoData.avgvel)) + units;
            StartVelLabel.Text = ((int)double.Parse(prInfoData.startvel)) + units;
            EndVelLabel.Text = ((int)double.Parse(prInfoData.endvel)) + units;

            // First Completion  -----------------------------------------------------
            if (!(prInfoData.first_date is null))
            {
                FirstDateLabel.Text = String_Formatter.toString_KSFDate(prInfoData.first_date);
                FirstTimeLabel.Text = String_Formatter.toString_PlayTime(prInfoData.first_timetaken, true);
                FirstAttemptsLabel.Text = prInfoData.first_attempts;
            }
            else
            {
                FirstDateLabel.Text = "";
                FirstTimeLabel.Text = "";
                FirstAttemptsLabel.Text = "";
            }
        }

        private void hidePR()
        {
            if (hasLoaded) return;

            PRTitleLabel.Text = "";
            PRStack.IsVisible = false;
            PagesStack.IsVisible = false;
            NoPRLabel.IsVisible = true;
        }

        private void displayPR()
        {
            PRStack.IsVisible = true;
            PagesStack.IsVisible = true;
            NoPRLabel.IsVisible = false;
        }


        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                await ChangePR(defaultMode, playerType, playerValue);
                hasLoaded = true;
            }
        }

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new MapsMapPRFilterPage(ApplyFilters, currentMode, playerType, 
                    playerSteamID, playerRank, defaultMode, meSteamID));
            }
            else
            {
                await DisplayAlert("Could not connect to KSF", "Please connect to the Internet.", "OK");
            }
            await MapsMapPRScollView.ScrollToAsync(0, 0, true);
        }

        internal async void ApplyFilters(EFilter_Mode newMode, EFilter_PlayerType newPlayerType, string newPlayerValue)
        {
            await ChangePR(newMode, newPlayerType, newPlayerValue);
        }

        private async void PR_Tapped(object sender, EventArgs e)
        {
            PRButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new MapsMapPRDetailsPage(Title,
                    mapsViewModel, game, currentMode, map, playerSteamID));
            }
            else
            {
                await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
            }
            PRButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void CPR_Tapped(object sender, EventArgs e)
        {
            CPRButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                
            }
            else
            {
                await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
            }
            CPRButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void CCP_Tapped(object sender, EventArgs e)
        {
            CCPButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                
            }
            else
            {
                await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
            }
            CCPButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        #endregion
    }
}
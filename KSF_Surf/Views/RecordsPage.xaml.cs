using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.Models;
using KSF_Surf.ViewModels;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class RecordsPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private bool isRefreshing = false;

        // objects used by "Recent" call
        private List<RRDatum> recentRecordsData;
        private List<RR10Datum> recentRecords10Data;

        // variables for current filters
        private EFilter_Game game;
        private readonly EFilter_Game defaultGame;
        private EFilter_Mode mode;
        private readonly EFilter_Mode defaultMode;
        private EFilter_RRType recentRecordsType;

        // Date of last refresh
        private DateTime lastRefresh;

        public RecordsPage()
        {
            recordsViewModel = new RecordsViewModel();
            InitializeComponent();

            game = BaseViewModel.propertiesDict_getGame();
            defaultGame = game;
            mode = BaseViewModel.propertiesDict_getMode();
            defaultMode = mode;
            recentRecordsType = EFilter_RRType.map;
            
        }

        // UI ---------------------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecentRecords(EFilter_Game game, EFilter_RRType type, EFilter_Mode mode)
        {
            if (type == EFilter_RRType.top)
            {
                var recentRecords10Datum = await recordsViewModel.GetRecentRecords10(game, mode);
                recentRecords10Data = recentRecords10Datum?.data;
                if (recentRecords10Data is null) return;
                LayoutRecentRecords10();
            }
            else
            {
                var recentRecordsDatum = await recordsViewModel.GetRecentRecords(game, type, mode);
                recentRecordsData = recentRecordsDatum?.data;
                if (recentRecordsData is null) return;
                LayoutRecentRecords(EFilter_ToString.toString2(type));
            }
            lastRefresh = DateTime.Now;
        }

        // Displaying Changes --------------------------------------------------------------------------

        private void LayoutRecentRecords(string typeString)
        {
            Title = "Records [" + EFilter_ToString.toString2(game) + ", " + EFilter_ToString.toString(mode) + "]";
            RRTypeOptionLabel.Text = "Type: " + typeString;

            RecordsStack.Children.Clear();

            int i = 0;
            int length = recentRecordsData.Count;
            foreach (RRDatum datum in recentRecordsData)
            {
                RecordsStack.Children.Add(new Label
                {
                    Text = datum.mapName + " " + EFilter_ToString.zoneFormatter(datum.zoneID, false, false),
                    Style = App.Current.Resources["RRLabelStyle"] as Style

                });
                RecordsStack.Children.Add(new Label
                {
                    Text = String_Formatter.toEmoji_Country(datum.country) + " " + datum.playerName + " on " + datum.server + " server",
                    Style = App.Current.Resources["RR2LabelStyle"] as Style
                });

                string rrtime = "in " + String_Formatter.toString_RankTime(datum.surfTime) + " (";
                if (datum.wrDiff == "0")
                {
                    if (datum.r2Diff is null)
                    {
                        rrtime += "WR N/A";
                    }
                    else
                    {
                        rrtime += "WR-" + String_Formatter.toString_RankTime(datum.r2Diff.Substring(1));
                    }
                }
                else
                {
                    rrtime += "now WR+" + String_Formatter.toString_RankTime(datum.wrDiff);
                }
                rrtime += ") (" + String_Formatter.toString_LastOnline(datum.date) + ")";
                RecordsStack.Children.Add(new Label
                {
                    Text = rrtime,
                    Style = App.Current.Resources["TimeLabelStyle"] as Style
                });

                if (++i != length)
                {
                    RecordsStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["SeparatorStyle"] as Style
                    });
                }
            }
        }

        private void LayoutRecentRecords10()
        {
            RRTypeOptionLabel.Text = "Type: Top10";

            RecordsStack.Children.Clear();

            int i = 0;
            int length = recentRecords10Data.Count;
            foreach (RR10Datum datum in recentRecords10Data)
            {
                int rank = int.Parse(datum.newRank);
                RecordsStack.Children.Add(new Label
                {
                    Text = datum.mapName + " [R" + rank + "]",
                    Style = App.Current.Resources["RRLabelStyle"] as Style

                });
                RecordsStack.Children.Add(new Label
                {
                    Text = String_Formatter.toEmoji_Country(datum.country) + " " + datum.playerName + " on " + datum.server + " server",
                    Style = App.Current.Resources["RR2LabelStyle"] as Style,
                });

                string rrtime = "in " + String_Formatter.toString_RankTime(datum.surfTime) + " (";
                if (datum.wrDiff == "0")
                {
                    rrtime += "WR";
                }
                else
                {
                    rrtime += "WR+" + String_Formatter.toString_RankTime(datum.wrDiff);
                }
                rrtime += ") (" + String_Formatter.toString_LastOnline(datum.date) + ")";
                RecordsStack.Children.Add(new Label
                {
                    Text = rrtime,
                    Style = App.Current.Resources["TimeLabelStyle"] as Style
                });

                if (++i != length)
                {
                    RecordsStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["SeparatorStyle"] as Style
                    });
                }
            }
        }

        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        internal async Task OnChangedTabAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;
                await ChangeRecentRecords(game, recentRecordsType, mode);

                LoadingAnimation.IsRunning = false;
                RecordsPageScrollView.IsVisible = true;
            }
        }

        private async void RRTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EFilter_ToString.toString2(recentRecordsType);
            foreach (string type in EFilter_ToString.rrtype_arr)
            {
                if (type != currentTypeString)
                {
                    types.Add(type);
                }
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());

            EFilter_RRType newType = EFilter_RRType.map;
            switch (newTypeString)
            {
                case "Cancel": return;
                case "Top10": newType = EFilter_RRType.top; break;
                case "Stage": newType = EFilter_RRType.stage; break;
                case "Bonus": newType = EFilter_RRType.bonus; break;
                case "All WRs": newType = EFilter_RRType.all; break;
            }

            recentRecordsType = newType;

            LoadingAnimation.IsRunning = true;
            await ChangeRecentRecords(game, recentRecordsType, mode);
            LoadingAnimation.IsRunning = false;
        }

        

        private async void Filter_Pressed(object sender, EventArgs e)
        {
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new RecordsFilterPage(ApplyFilters, game, mode, defaultGame, defaultMode));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
        }

        private async void Refresh_Pressed(object sender, EventArgs e)
        {
            if (isRefreshing) return;

            TimeSpan sinceRefresh = DateTime.Now - lastRefresh;
            bool tooSoon = sinceRefresh.TotalSeconds < 10;

            if (BaseViewModel.hasConnection())
            {
                isRefreshing = true;
                BaseViewModel.vibrate(true);
                LoadingAnimation.IsRunning = true;

                if (tooSoon)
                {
                    await Task.Delay(500); // 0.5 seconds
                }
                else
                {
                    await ChangeRecentRecords(game, recentRecordsType, mode);
                }

                LoadingAnimation.IsRunning = false;
                BaseViewModel.vibrate(true);
                isRefreshing = false;
            }
            else
            {
                await DisplayAlert("Unable to refresh", "Please connect to the Internet.", "OK");
            }
        }

        internal async void ApplyFilters(EFilter_Game newGame, EFilter_Mode newMode)
        {
            if (BaseViewModel.hasConnection())
            {
                if (newGame == game && newMode == mode) return;

                game = newGame;
                mode = newMode;

                LoadingAnimation.IsRunning = true;
                await ChangeRecentRecords(game, recentRecordsType, mode);
                LoadingAnimation.IsRunning = false;
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            await RecordsPageScrollView.ScrollToAsync(0, 0, true);
        }

        private async void SurfTop_Tapped(object sender, EventArgs e)
        {
            SurfTopButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new RecordsTopPage(Title, recordsViewModel, game, mode));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            SurfTopButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void CountryTop_Tapped(object sender, EventArgs e)
        {
            CountryTopButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new RecordsCountryTopPage(Title, recordsViewModel, game, mode));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            CountryTopButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void TopCountries_Tapped(object sender, EventArgs e)
        {
            TopCountriesButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new RecordsTopCountriesPage(Title, recordsViewModel, game, mode));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            TopCountriesButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void MostByType_Tapped(object sender, EventArgs e)
        {
            MostButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new RecordsMostPage(Title, recordsViewModel, game, mode));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            MostButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async void OldestRecords_Tapped(object sender, EventArgs e)
        {
            OldestRecordsButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            if (BaseViewModel.hasConnection())
            {
                await Navigation.PushAsync(new RecordsOldestPage(Title, recordsViewModel, game, mode));
            }
            else
            {
                await DisplayNoConnectionAlert();
            }
            OldestRecordsButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;
        }

        private async Task DisplayNoConnectionAlert()
        {
            await DisplayAlert("Could not connect to KSF!", "Please connect to the Internet.", "OK");
        }
    }

    #endregion
}
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using Xamarin.Forms;

using KSF_Surf.ViewModels;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class PlayerRecentRecordsPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;

        // objects used by "Records Set" and "Records Broken" calls
        private List<RecentPlayerRecords> recordsSetData;
        private List<RecentPlayerRecords> recordsBrokenData;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly EFilter_PlayerType playerType;
        private readonly string playerValue;

        public PlayerRecentRecordsPage(string title, PlayerViewModel playerViewModel, EFilter_Game game, EFilter_Mode mode, EFilter_PlayerType playerType, string playerValue)
        {
            this.playerViewModel = playerViewModel;
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;

            InitializeComponent();
            Title = title;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords()
        {
            var recordsSetDatum = await playerViewModel.GetPlayerRecords(game, mode, EFilter_PlayerRecordsType.set, playerType, playerValue);
            recordsSetData = recordsSetDatum?.data.recentRecords;

            var recordsBrokenDatum = await playerViewModel.GetPlayerRecords(game, mode, EFilter_PlayerRecordsType.broken, playerType, playerValue);
            recordsBrokenData = recordsBrokenDatum?.data.recentRecords;
            
            if (recordsSetData is null || recordsBrokenData is null) return;
            LayoutRecords();
        }

        // Dispaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            ClearRecordsStacks();

            int i = 0;
            foreach (RecentPlayerRecords datum in recordsBrokenData)
            {
                string rrstring = datum.mapName + " ";
                if (datum.zoneID != "0")
                {
                    rrstring += EFilter_ToString.zoneFormatter(datum.zoneID, false, false) + " ";
                }
                RecordsBrokenStack.Children.Add(new Label
                {
                    Text = rrstring,
                    Style = App.Current.Resources["RRLabelStyle"] as Style
                });
                RecordsBrokenStack.Children.Add(new Label
                {
                    Text = datum.recordType + " lost on " + datum.server + " server",
                    Style = App.Current.Resources["RR2LabelStyle"] as Style,
                });

                string rrtime = "now [R" + datum.newRank + "] (";
                if (datum.wrDiff == "0")
                {
                    rrtime += "RETAKEN";
                }
                else
                {
                    rrtime += "WR+" + String_Formatter.toString_RankTime(datum.wrDiff);
                }
                rrtime += ") (" + String_Formatter.toString_LastOnline(datum.date) + ")";

                RecordsBrokenStack.Children.Add(new Label
                {
                    Text = rrtime,
                    Style = App.Current.Resources["TimeLabelStyle"] as Style
                });

                if (++i != recordsBrokenData.Count)
                {
                    RecordsBrokenStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["SeparatorStyle"] as Style
                    });
                }
            }
            if (i == 0) // no recently broken records
            {
                RecordsBrokenStack.Children.Add(new Label
                {
                    Text = "None! :)",
                    Style = App.Current.Resources["LeftColStyle"] as Style,
                    HorizontalOptions = LayoutOptions.Center
                });
            }
                
            i = 0;
            foreach (RecentPlayerRecords datum in recordsSetData)
            {
                string rrstring = datum.mapName + " ";
                if (datum.zoneID != "0")
                {
                    rrstring += EFilter_ToString.zoneFormatter(datum.zoneID, false, false) + " ";
                }
                RecordsSetStack.Children.Add(new Label
                {
                    Text = rrstring,
                    Style = App.Current.Resources["RRLabelStyle"] as Style
                });
                RecordsSetStack.Children.Add(new Label
                {
                    Text = datum.recordType + " set on " + datum.server + " server",
                    Style = App.Current.Resources["RR2LabelStyle"] as Style
                });

                string rrtime = "[R" + datum.newRank + "] in " + String_Formatter.toString_RankTime(datum.surfTime) + " (";
                if (datum.wrDiff != "0")
                {
                    if (datum.newRank == "1")
                    {
                        rrtime += "now ";
                    }
                    rrtime += "WR+" + String_Formatter.toString_RankTime(datum.wrDiff) + ") (";
                }
                rrtime += String_Formatter.toString_LastOnline(datum.date) + ")";

                RecordsSetStack.Children.Add(new Label
                {
                    Text = rrtime,
                    Style = App.Current.Resources["TimeLabelStyle"] as Style
                });

                if (++i != recordsSetData.Count)
                {
                    RecordsSetStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["SeparatorStyle"] as Style
                    });
                }
               
            }
            if (i == 0) // no recently set records
            {
                RecordsSetStack.Children.Add(new Label
                {
                    Text = "None! :(",
                    Style = App.Current.Resources["LeftColStyle"] as Style,
                    HorizontalOptions = LayoutOptions.Center
                });
            }
        }

        private void ClearRecordsStacks()
        {
            RecordsSetStack.Children.Clear();
            RecordsBrokenStack.Children.Clear();
        }


        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                await ChangeRecords();

                LoadingAnimation.IsRunning = false;
                RRPageScrollView.IsVisible = true;
                hasLoaded = true;
            }
        }

        #endregion
    }
}
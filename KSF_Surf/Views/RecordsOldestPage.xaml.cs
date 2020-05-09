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
    public partial class RecordsOldestPage : ContentPage
    {
        private readonly RecordsViewModel recordsViewModel;
        private bool hasLoaded = false;
        private readonly int LIST_LIMIT = 10;
        private readonly int CALL_LIMIT = 50;

        // objects used by "Oldest Records" call
        private List<OldRecord> oldRecordData;
        private int list_index = 1;
        private bool moreRecords = false;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private EFilter_ORType oldestType;

        public RecordsOldestPage(string title, RecordsViewModel recordsViewModel, EFilter_Game game, EFilter_Mode mode)
        {
            this.recordsViewModel = recordsViewModel;
            this.game = game;
            this.mode = mode;
            this.oldestType = EFilter_ORType.map;

            InitializeComponent();
            Title = title;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords(EFilter_ORType type)
        {
            var oldRecordDatum = await recordsViewModel.GetOldestRecords(game, type, mode, list_index);
            oldRecordData = oldRecordDatum?.data;
            if (oldRecordData is null) return;

            ORTypeOptionLabel.Text = "Type: " + EFilter_ToString.toString2(type);
            if (list_index == 1) ORStack.Children.Clear();
            LayoutRecords();
        }

        // Dispaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            if (list_index != 1)
            {
                ORStack.Children.Add(new BoxView
                {
                    Style = App.Current.Resources["SeparatorStyle"] as Style
                });
            }

            int i = 0;
            int length = oldRecordData.Count;
            foreach (OldRecord datum in oldRecordData)
            {
                string rrstring = datum.mapName;
                if (datum.zoneID != null)
                {
                    rrstring += " " + EFilter_ToString.zoneFormatter(datum.zoneID, false, false);
                }
                ORStack.Children.Add(new Label
                {
                    Text = rrstring,
                    Style = App.Current.Resources["RRLabelStyle"] as Style

                });

                ORStack.Children.Add(new Label
                {
                    Text = String_Formatter.toEmoji_Country(datum.country) + " " + datum.playerName,
                    Style = App.Current.Resources["RR2LabelStyle"] as Style
                });

                string rrtime = "in " + String_Formatter.toString_RankTime(datum.surfTime);
                if (!(datum.r2Diff is null))
                {
                    if (datum.r2Diff != "0")
                    {
                        rrtime += " (WR-" + String_Formatter.toString_RankTime(datum.r2Diff.Substring(1)) + ")";
                    }
                    else
                    {
                        rrtime += " (RETAKEN)";
                    }
                }
                else
                {
                    rrtime += " (WR N/A)";
                }
                rrtime += " (" + String_Formatter.toString_LastOnline(datum.date) + ")";
                ORStack.Children.Add(new Label
                {
                    Text = rrtime,
                    Style = App.Current.Resources["TimeLabelStyle"] as Style
                });

                if (++i != length)
                {
                    ORStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["SeparatorStyle"] as Style
                    });
                }
            }

            moreRecords = ((i == LIST_LIMIT) && list_index < CALL_LIMIT);
            MoreFrame.IsVisible = moreRecords;
        }


        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                await ChangeRecords(oldestType);

                LoadingAnimation.IsRunning = false;
                RecordsOldestPageScrollView.IsVisible = true;
                hasLoaded = true;
            }
        }

        private async void ORTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EFilter_ToString.toString2(oldestType);
            foreach (string type in EFilter_ToString.otype_arr)
            {
                if (type != currentTypeString)
                {
                    types.Add(type);
                }
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());

            EFilter_ORType newType = EFilter_ORType.map;
            switch (newTypeString)
            {
                case "Cancel": return;
                case "Stage": newType = EFilter_ORType.stage; break;
                case "Bonus": newType = EFilter_ORType.bonus; break;
            }

            oldestType = newType;
            list_index = 1;

            LoadingAnimation.IsRunning = true;
            await ChangeRecords(newType);
            LoadingAnimation.IsRunning = false;
        }

        private async void MoreButton_Tapped(object sender, EventArgs e)
        {
            if (!BaseViewModel.hasConnection()) return;

            MoreButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            list_index += LIST_LIMIT;

            var oldRecordDatum = await recordsViewModel.GetOldestRecords(game, oldestType, mode, list_index);
            oldRecordData = oldRecordDatum?.data;

            MoreButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;

            if (oldRecordData is null || oldRecordData.Count < 1)
            {
                MoreFrame.IsVisible = false;
                return;
            }

            LayoutRecords();
        }

        #endregion
    }
}
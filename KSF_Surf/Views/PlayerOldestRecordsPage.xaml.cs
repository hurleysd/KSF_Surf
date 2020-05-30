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
    public partial class PlayerOldestRecordsPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;
        private bool isLoading = false;
        private readonly int LIST_LIMIT = 10;

        // objects used by "Oldest Records" call
        private List<PlayerOldRecord> oldRecordData;
        private List<string> oldRecordsOptionStrings;
        private int list_index = 1;
        private bool moreRecords = false;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly EFilter_PlayerType playerType;
        private readonly string playerValue;
        private EFilter_PlayerWRsType wrsType;
        private bool hasTop;
        private EFilter_PlayerOldestType oldestType;

        public PlayerOldestRecordsPage(string title, PlayerViewModel playerViewModel, EFilter_Game game, EFilter_Mode mode, 
            EFilter_PlayerType playerType, string playerValue, EFilter_PlayerWRsType wrsType, bool hasTop)
        {
            this.playerViewModel = playerViewModel;
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;
            this.wrsType = wrsType;
            this.hasTop = hasTop;

            oldRecordsOptionStrings = new List<string>(EFilter_ToString.ortype_arr);

            InitializeComponent();
            Title = title;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords(EFilter_PlayerOldestType type)
        {
            var oldRecordDatum = await playerViewModel.GetPlayerOldestRecords(game, mode, type, playerType, playerValue, list_index);
            oldRecordData = oldRecordDatum?.data.records;
            if (oldRecordData is null) return;

            ORTypeOptionLabel.Text = "Type: " + EFilter_ToString.toString2(type);
            if (list_index == 1) ORStack.Children.Clear();
            LayoutRecords();
        }

        // Displaying Changes -------------------------------------------------------------------------------

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
            foreach (PlayerOldRecord datum in oldRecordData)
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
                
                string rrtime = "";
                string rrdiff = "";
                if (oldestType == EFilter_PlayerOldestType.top10)
                {
                    rrtime += "[R" + datum.rank + "] ";
                    if (datum.wrdiff == "0")
                    {
                        rrdiff += " (WR)";
                    }
                    else
                    {
                        rrdiff += " (WR+" + String_Formatter.toString_RankTime(datum.wrdiff) + ")";
                    }
                }
                else if (oldestType == EFilter_PlayerOldestType.map)
                {
                    if (datum.top10Group != "0") rrtime += "[G" + datum.top10Group.Substring(1) + "] ";
                }
                else if (oldestType == EFilter_PlayerOldestType.wr ||
                    oldestType == EFilter_PlayerOldestType.wrcp ||
                    oldestType == EFilter_PlayerOldestType.wrb)
                {
                    if (!(datum.r2Diff is null))
                    {
                        if (datum.r2Diff != "0")
                        {
                            rrdiff += " (WR-" + String_Formatter.toString_RankTime(datum.r2Diff.Substring(1)) + ")";
                        }
                        else
                        {
                            rrdiff += " (RETAKEN)";
                        }
                    }
                    else
                    {
                        rrdiff += " (WR N/A)";
                    }
                }

                rrtime += "in " + String_Formatter.toString_RankTime(datum.surfTime) + rrdiff;
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

            moreRecords = (i == LIST_LIMIT);
            MoreFrame.IsVisible = moreRecords;

            if (i == 0) // no recently broken records
            {
                ORStack.Children.Add(new Label
                {
                    Text = "None! :/",
                    Style = App.Current.Resources["LeftColStyle"] as Style,
                    HorizontalOptions = LayoutOptions.Center
                });
            }
        }


        #endregion
        // Event Handlers ----------------------------------------------------------------------------------
        #region events

        protected override async void OnAppearing()
        {
            if (!hasLoaded)
            {
                hasLoaded = true;

                oldestType = EFilter_PlayerOldestType.map;
                switch (wrsType)
                {
                    case EFilter_PlayerWRsType.wr: oldestType = EFilter_PlayerOldestType.wr; break;
                    case EFilter_PlayerWRsType.wrcp:oldestType = EFilter_PlayerOldestType.wrcp;break;
                    case EFilter_PlayerWRsType.wrb: oldestType = EFilter_PlayerOldestType.wrb; break;
                    default: 
                        {
                            oldRecordsOptionStrings.RemoveRange(0, 3);
                            if (hasTop)
                            {
                                oldestType = EFilter_PlayerOldestType.top10;
                            }
                            else
                            {
                                oldRecordsOptionStrings.Remove("Top10");
                            }
                            break; 
                        }
                }
                await ChangeRecords(oldestType);

                LoadingAnimation.IsRunning = false;
                PlayerOldestRecordsScrollView.IsVisible = true;
            }
        }

        private async void ORTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EFilter_ToString.toString2(oldestType);
            foreach (string type in oldRecordsOptionStrings)
            {
                if (type != currentTypeString)
                {
                    types.Add(type);
                }
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());

            EFilter_PlayerOldestType newType = EFilter_PlayerOldestType.map;
            switch (newTypeString)
            {
                case "Cancel": return;
                case "Stage": newType = EFilter_PlayerOldestType.stage; break;
                case "Bonus": newType = EFilter_PlayerOldestType.bonus; break;
                case "WR": newType = EFilter_PlayerOldestType.wr; break;
                case "WRCP": newType = EFilter_PlayerOldestType.wrcp; break;
                case "WRB": newType = EFilter_PlayerOldestType.wrb; break;
                case "Top10": newType = EFilter_PlayerOldestType.top10; break;
            }

            oldestType = newType;
            list_index = 1;

            LoadingAnimation.IsRunning = true;
            await ChangeRecords(newType);
            LoadingAnimation.IsRunning = false;
        }

        private async void MoreButton_Tapped(object sender, EventArgs e)
        {
            if (isLoading || !BaseViewModel.hasConnection()) return;
            isLoading = true;

            MoreButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            MoreLabel.IsVisible = false;
            MoreLoadingAnimation.IsRunning = true;

            list_index += LIST_LIMIT;

            var oldRecordDatum = await playerViewModel.GetPlayerOldestRecords(game, mode, oldestType, playerType, playerValue, list_index);
            oldRecordData = oldRecordDatum?.data.records;

            MoreButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;

            if (oldRecordData is null || oldRecordData.Count < 1)
            {
                MoreFrame.IsVisible = false;
                return;
            }

            LayoutRecords();
            MoreLoadingAnimation.IsRunning = false;
            MoreLabel.IsVisible = true;
            isLoading = false;
        }

        #endregion
    }
}
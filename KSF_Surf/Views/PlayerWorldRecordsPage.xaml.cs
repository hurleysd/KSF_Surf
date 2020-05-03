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
    public partial class PlayerWorldRecordsPage : ContentPage
    {
        private readonly PlayerViewModel playerViewModel;
        private bool hasLoaded = false;
        private readonly int LIST_LIMIT = 10;

        // objects used by "World Records" call
        private List<PlayerWorldRecords> worldRecordsData;
        private int list_index = 1;
        private bool moreRecords = false;

        // variables for filters
        private readonly EFilter_Game game;
        private readonly EFilter_Mode mode;
        private readonly EFilter_PlayerType playerType;
        private readonly string playerValue;
        private EFilter_PlayerWRsType wrsType;

        public PlayerWorldRecordsPage(string title, PlayerViewModel playerViewModel, EFilter_Game game, EFilter_Mode mode, 
            EFilter_PlayerType playerType, string playerValue, EFilter_PlayerWRsType wrsType)
        {
            this.playerViewModel = playerViewModel;
            this.game = game;
            this.mode = mode;
            this.playerType = playerType;
            this.playerValue = playerValue;
            this.wrsType = wrsType;

            InitializeComponent();
            Title = title;
        }

        // UI -----------------------------------------------------------------------------------------------
        #region UI

        private async Task ChangeRecords(EFilter_PlayerWRsType type)
        {
            var worldRecordsDatum = await playerViewModel.GetPlayerWRs(game, mode, type, playerType, playerValue, list_index);
            worldRecordsData = worldRecordsDatum?.data.records;
            if (worldRecordsData is null) return;

            WRTypeOptionLabel.Text = "[Type: " + EFilter_ToString.toString2(type) + "]";
            if (list_index == 1) WRsStack.Children.Clear();
            LayoutRecords();
        }

        // Dispaying Changes -------------------------------------------------------------------------------

        private void LayoutRecords()
        {
            if (list_index != 1)
            {
                WRsStack.Children.Add(new BoxView
                {
                    Style = App.Current.Resources["SeparatorStyle"] as Style
                });
            }

            int i = 0;
            int length = worldRecordsData.Count;
            foreach (PlayerWorldRecords datum in worldRecordsData)
            {
                string rrstring = datum.mapName;
                if (wrsType != EFilter_PlayerWRsType.wr)
                {
                    rrstring += " " + EFilter_ToString.zoneFormatter(datum.zoneID, false);
                }
                WRsStack.Children.Add(new Label
                {
                    Text = rrstring,
                    Style = App.Current.Resources["RRLabelStyle"] as Style

                });

                string rrtime = "in " + String_Formatter.toString_RankTime(datum.surfTime) + " (";
                if (datum.r2Diff is null)
                {
                    rrtime += "WR N/A";
                }
                else
                {
                    rrtime += "WR-" + String_Formatter.toString_RankTime(datum.r2Diff.Substring(1));
                }
                rrtime += ") (" + String_Formatter.toString_LastOnline(datum.date) + ")";
                WRsStack.Children.Add(new Label
                {
                    Text = rrtime,
                    Style = App.Current.Resources["TimeLabelStyle"] as Style
                });

                if (++i != length)
                {
                    WRsStack.Children.Add(new BoxView
                    {
                        Style = App.Current.Resources["SeparatorStyle"] as Style
                    });
                }
            }

            moreRecords = (i == LIST_LIMIT);
            MoreFrame.IsVisible = moreRecords;

            if (i == 0) // no recently broken records
            {
                WRsStack.Children.Add(new Label
                {
                    Text = "None! :(",
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
                await ChangeRecords(wrsType);
                hasLoaded = true;
            }
        }

        private async void WRTypeOptionLabel_Tapped(object sender, EventArgs e)
        {
            List<string> types = new List<string>();
            string currentTypeString = EFilter_ToString.toString2(wrsType);
            foreach (string type in EFilter_ToString.wrtype_arr2)
            {
                if (type != currentTypeString)
                {
                    types.Add(type);
                }
            }

            string newTypeString = await DisplayActionSheet("Choose a different type", "Cancel", null, types.ToArray());

            EFilter_PlayerWRsType newType = EFilter_PlayerWRsType.wr;
            switch (newTypeString)
            {
                case "Cancel": return;
                case "WRCP": newType = EFilter_PlayerWRsType.wrcp; break;
                case "WRB": newType = EFilter_PlayerWRsType.wrb; break;
            }

            wrsType = newType;
            list_index = 1;
            await ChangeRecords(newType);
        }

        private async void MoreButton_Tapped(object sender, EventArgs e)
        {
            if (!BaseViewModel.hasConnection()) return;

            MoreButton.Style = App.Current.Resources["TappedStackStyle"] as Style;
            list_index += LIST_LIMIT;

            var worldRecordsDatum = await playerViewModel.GetPlayerWRs(game, mode, wrsType, playerType, playerValue, list_index);
            worldRecordsData = worldRecordsDatum?.data.records;
            
            MoreButton.Style = App.Current.Resources["UntappedStackStyle"] as Style;

            if (worldRecordsData is null || worldRecordsData.Count < 1)
            {
                MoreFrame.IsVisible = false;
                return;
            }

            LayoutRecords();
        }

        #endregion
    }
}
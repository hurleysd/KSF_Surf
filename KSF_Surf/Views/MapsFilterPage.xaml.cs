﻿using KSF_Surf.Models;
using KSF_Surf.ViewModels;

using System;
using System.ComponentModel;

using Xamarin.Forms;


namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsFilterPage : ContentPage
    {
        // variables for filters
        private EFilter_Game game = EFilter_Game.none;
        private EFilter_Sort sort = EFilter_Sort.none;
        private int minTier = -1;
        private int maxTier = -1;
        private EFilter_MapType mapType = EFilter_MapType.none;

        // method to apply filters
        private Action<EFilter_Game, EFilter_Sort, int, int, EFilter_MapType> FilterApplier;

        // vibration
        private bool allowVibrate = false;


        public MapsFilterPage(Action<EFilter_Game, EFilter_Sort, int, int, EFilter_MapType> FilterApplier,
            EFilter_Game currentGame, EFilter_Sort currentSort, int currentMinTier, int currentMaxTier, EFilter_MapType currentMapType)
        {
            this.FilterApplier = FilterApplier;

            InitializeComponent();

            ChangeGameFilter(currentGame);
            ChangeSortFilter(currentSort);
            ChangeMapTypeFilter(currentMapType);
            allowVibrate = true;

            minTier = currentMinTier;
            maxTier = currentMaxTier;
            MaxTierSlider.Value = maxTier;
            MinTierSlider.Value = minTier;

            if (minTier != 1 || maxTier != 8)
            {
                ResetLabel.IsVisible = true;
            }
        }

        // UI -------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private void ChangeGameFilter(EFilter_Game newGame)
        {
            if (game == newGame) return;
            Color GrayTextColor = (Color)App.Current.Resources["GrayTextColor"];
            Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];

            switch (game)
            {
                case EFilter_Game.css: GameCSSLabel.TextColor = GrayTextColor; break;
                case EFilter_Game.css100t: GameCSS100TLabel.TextColor = GrayTextColor; break;
                case EFilter_Game.csgo: GameCSGOLabel.TextColor = GrayTextColor; break;
                default: break;
            }

            switch (newGame)
            {
                case EFilter_Game.css: GameCSSLabel.TextColor = tappedTextColor; break;
                case EFilter_Game.css100t:
                    {
                        GameCSS100TLabel.TextColor = tappedTextColor;
                        ResetLabel.IsVisible = true;
                        break;
                    }
                case EFilter_Game.csgo:
                    {
                        GameCSGOLabel.TextColor = tappedTextColor;
                        ResetLabel.IsVisible = true;
                        break;
                    }
            }

            BaseViewModel.vibrate(allowVibrate);
            game = newGame;
        }

        private void ChangeSortFilter(EFilter_Sort newSort)
        {
            if (sort == newSort) return;
            Color GrayTextColor = (Color)App.Current.Resources["GrayTextColor"];
            Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];

            switch (sort)
            {
                case EFilter_Sort.name: SortNameLabel.TextColor = GrayTextColor; break;
                case EFilter_Sort.created: SortCreateLabel.TextColor = GrayTextColor; break;
                case EFilter_Sort.lastplayed: SortLastLabel.TextColor = GrayTextColor; break;
                case EFilter_Sort.playtime: SortPlayLabel.TextColor = GrayTextColor; break;
                case EFilter_Sort.popularity: SortPopLabel.TextColor = GrayTextColor; break;
                default: break;
            }

            switch (newSort)
            {
                case EFilter_Sort.name: SortNameLabel.TextColor = tappedTextColor; break;
                case EFilter_Sort.created:
                    {
                        SortCreateLabel.TextColor = tappedTextColor;
                        ResetLabel.IsVisible = true;
                        break;
                    }
                case EFilter_Sort.lastplayed:
                    {
                        SortLastLabel.TextColor = tappedTextColor;
                        ResetLabel.IsVisible = true;
                        break;
                    }
                case EFilter_Sort.playtime:
                    {
                        SortPlayLabel.TextColor = tappedTextColor;
                        ResetLabel.IsVisible = true;
                        break;
                    }
                case EFilter_Sort.popularity:
                    {
                        SortPopLabel.TextColor = tappedTextColor;
                        ResetLabel.IsVisible = true;
                        break;
                    }
            }

            BaseViewModel.vibrate(allowVibrate);
            sort = newSort;
        }

        private void ChangeMapTypeFilter(EFilter_MapType newMapType)
        {
            if (mapType == newMapType) return;
            Color GrayTextColor = (Color)App.Current.Resources["GrayTextColor"];
            Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];

            switch (mapType)
            {
                case EFilter_MapType.any: TypeAnyLabel.TextColor = GrayTextColor; break;
                case EFilter_MapType.linear: TypeLinearLabel.TextColor = GrayTextColor; break;
                case EFilter_MapType.staged: TypeStagedLabel.TextColor = GrayTextColor; break;
                default: break;
            }

            switch (newMapType)
            {
                case EFilter_MapType.any: TypeAnyLabel.TextColor = tappedTextColor; break;
                case EFilter_MapType.linear:
                    {
                        TypeLinearLabel.TextColor = tappedTextColor;
                        ResetLabel.IsVisible = true;
                        break;
                    }
                case EFilter_MapType.staged:
                    {
                        TypeStagedLabel.TextColor = tappedTextColor;
                        ResetLabel.IsVisible = true;
                        break;
                    }
            }

            BaseViewModel.vibrate(allowVibrate);
            mapType = newMapType;
        }

        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        private async void Apply_Clicked(object sender, System.EventArgs e)
        {
            FilterApplier(game, sort, minTier, maxTier, mapType);
            await Navigation.PopAsync();
        }

        private void CSSGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.css);
        private void CSS100TGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.css100t);
        private void CSGOGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(EFilter_Game.csgo);

        private void NameSortFilter_Tapped(object sender, EventArgs e) => ChangeSortFilter(EFilter_Sort.name);
        private void CreateSortFilter_Tapped(object sender, EventArgs e) => ChangeSortFilter(EFilter_Sort.created);
        private void LastSortFilter_Tapped(object sender, EventArgs e) => ChangeSortFilter(EFilter_Sort.lastplayed);
        private void PlaySortFilter_Tapped(object sender, EventArgs e) => ChangeSortFilter(EFilter_Sort.playtime);
        private void PopSortFilter_Tapped(object sender, EventArgs e) => ChangeSortFilter(EFilter_Sort.popularity);

        private void AnyMapTypeFilter_Tapped(object sender, EventArgs e) => ChangeMapTypeFilter(EFilter_MapType.any);
        private void LinearMapTypeFilter_Tapped(object sender, EventArgs e) => ChangeMapTypeFilter(EFilter_MapType.linear);
        private void StagedMapTypeFilter_Tapped(object sender, EventArgs e) => ChangeMapTypeFilter(EFilter_MapType.staged);

        private void MinTierSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int newValue = (int)Math.Round(e.NewValue);
            MinTierSlider.Value = newValue;
            MinTierLabel.Text = newValue.ToString();
            if (newValue > MaxTierSlider.Value)
            {
                MaxTierSlider.Value = newValue;
            }
            minTier = newValue;

            if (minTier != 1)
            {
                ResetLabel.IsVisible = true;
            }
        }

        private void MaxTierSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int newValue = (int)Math.Round(e.NewValue);
            MaxTierSlider.Value = newValue;
            MaxTierLabel.Text = newValue.ToString();
            if (newValue < MinTierSlider.Value)
            {
                MinTierSlider.Value = newValue;
            }
            maxTier = newValue;

            if (maxTier != 8)
            {
                ResetLabel.IsVisible = true;
            }
        }

        private void ResetLabel_Tapped(object sender, EventArgs e)
        {
            bool oldAllowVibrate = allowVibrate;
            allowVibrate = false;

            if (game != EFilter_Game.css)
            {
                ChangeGameFilter(EFilter_Game.css);
            }
            if (sort != EFilter_Sort.name)
            {
                ChangeSortFilter(EFilter_Sort.name);
            }
            if (mapType != EFilter_MapType.any)
            {
                ChangeMapTypeFilter(EFilter_MapType.any);
            }
            if (minTier != 1)
            {
                minTier = 1;
                MinTierSlider.Value = minTier;
            }
            if (maxTier != 8)
            {
                maxTier = 8;
                MaxTierSlider.Value = maxTier;
            }

            allowVibrate = oldAllowVibrate;
            BaseViewModel.vibrate(allowVibrate);

            ResetLabel.IsVisible = false;
        }
    }

    #endregion
}
using System;
using System.ComponentModel;
using Xamarin.Forms;
using KSF_Surf.Models;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MapsFilterPage : ContentPage
    {
        // variables for filters
        private GameEnum game = GameEnum.NONE;
        private readonly GameEnum defaultGame;

        private SortEnum sort = SortEnum.NONE;
        private int minTier = -1;
        private int maxTier = -1;
        private MapTypeEnum mapType = MapTypeEnum.NODE;

        // booleans for reset
        private bool resetGame = false;
        private bool resetSort = false;
        private bool resetMin = false;
        private bool resetMax = false;
        private bool resetType = false;

        // method to apply filters
        private readonly Action<GameEnum, SortEnum, int, int, MapTypeEnum> FilterApplier;

        // colors
        private readonly Color untappedTextColor = (Color)App.Current.Resources["UntappedTextColor"];
        private readonly Color tappedTextColor = (Color)App.Current.Resources["TappedTextColor"];

        public MapsFilterPage(Action<GameEnum, SortEnum, int, int, MapTypeEnum> FilterApplier,
            GameEnum currentGame, SortEnum currentSort, 
            int currentMinTier, int currentMaxTier, MapTypeEnum currentMapType,
            GameEnum defaultGame)
        {
            this.FilterApplier = FilterApplier;
            this.defaultGame = defaultGame;

            InitializeComponent();

            ChangeGameFilter(currentGame);
            ChangeSortFilter(currentSort);
            ChangeMapTypeFilter(currentMapType);

            minTier = currentMinTier;
            maxTier = currentMaxTier;
            TierRangeSlider.UpperValue = maxTier;
            TierRangeSlider.LowerValue = minTier;
        }

        // UI -------------------------------------------------------------------------------------------------------------------------------------
        #region UI

        private void ChangeGameFilter(GameEnum newGame)
        {
            if (game == newGame) return;

            switch (game)
            {
                case GameEnum.CSS: GameCSSLabel.TextColor = untappedTextColor; break;
                case GameEnum.CSS100T: GameCSS100TLabel.TextColor = untappedTextColor; break;
                case GameEnum.CSGO: GameCSGOLabel.TextColor = untappedTextColor; break;
                default: break;
            }

            switch (newGame)
            {
                case GameEnum.CSS: GameCSSLabel.TextColor = tappedTextColor; break;
                case GameEnum.CSS100T: GameCSS100TLabel.TextColor = tappedTextColor; break;
                case GameEnum.CSGO: GameCSGOLabel.TextColor = tappedTextColor; break;
            }

            resetGame = (newGame != defaultGame);
            checkReset();

            game = newGame;
        }

        private void ChangeSortFilter(SortEnum newSort)
        {
            if (sort == newSort) return;

            switch (sort)
            {
                case SortEnum.NAME: SortNameLabel.TextColor = untappedTextColor; break;
                case SortEnum.CREATED: SortCreateLabel.TextColor = untappedTextColor; break;
                case SortEnum.LAST_PLAYED: SortLastLabel.TextColor = untappedTextColor; break;
                case SortEnum.PLAYTIME: SortPlayLabel.TextColor = untappedTextColor; break;
                case SortEnum.POPULARITY: SortPopLabel.TextColor = untappedTextColor; break;
                default: break;
            }

            switch (newSort)
            {
                case SortEnum.NAME: SortNameLabel.TextColor = tappedTextColor; break;
                case SortEnum.CREATED: SortCreateLabel.TextColor = tappedTextColor; break;
                case SortEnum.LAST_PLAYED: SortLastLabel.TextColor = tappedTextColor; break;
                case SortEnum.PLAYTIME: SortPlayLabel.TextColor = tappedTextColor; break;
                case SortEnum.POPULARITY: SortPopLabel.TextColor = tappedTextColor; break;
            }

            resetSort = newSort != (SortEnum.NAME);
            checkReset();

            sort = newSort;
        }

        private void ChangeMapTypeFilter(MapTypeEnum newMapType)
        {
            if (mapType == newMapType) return;

            switch (mapType)
            {
                case MapTypeEnum.ANY: TypeAnyLabel.TextColor = untappedTextColor; break;
                case MapTypeEnum.LINEAR: TypeLinearLabel.TextColor = untappedTextColor; break;
                case MapTypeEnum.STAGED: TypeStagedLabel.TextColor = untappedTextColor; break;
                default: break;
            }

            switch (newMapType)
            {
                case MapTypeEnum.ANY: TypeAnyLabel.TextColor = tappedTextColor; break;
                case MapTypeEnum.LINEAR: TypeLinearLabel.TextColor = tappedTextColor; break;
                case MapTypeEnum.STAGED: TypeStagedLabel.TextColor = tappedTextColor; break;
            }

            resetType = (newMapType != MapTypeEnum.ANY);
            checkReset();

            mapType = newMapType;
        }

        private void checkReset()
        {
            ResetLabel.IsVisible = resetGame || resetSort || resetMin || resetMax || resetType;
        }

        #endregion
        // Event Handlers --------------------------------------------------------------------------------------------------------------------------
        #region events

        private async void Apply_Clicked(object sender, System.EventArgs e)
        {
            FilterApplier(game, sort, minTier, maxTier, mapType);
            await Navigation.PopAsync();
        }

        private void CSSGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSS);
        private void CSS100TGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSS100T);
        private void CSGOGameFilter_Tapped(object sender, EventArgs e) => ChangeGameFilter(GameEnum.CSGO);

        private void NameSortFilter_Tapped(object sender, EventArgs e) => ChangeSortFilter(SortEnum.NAME);
        private void CreateSortFilter_Tapped(object sender, EventArgs e) => ChangeSortFilter(SortEnum.CREATED);
        private void LastSortFilter_Tapped(object sender, EventArgs e) => ChangeSortFilter(SortEnum.LAST_PLAYED);
        private void PlaySortFilter_Tapped(object sender, EventArgs e) => ChangeSortFilter(SortEnum.PLAYTIME);
        private void PopSortFilter_Tapped(object sender, EventArgs e) => ChangeSortFilter(SortEnum.POPULARITY);

        private void AnyMapTypeFilter_Tapped(object sender, EventArgs e) => ChangeMapTypeFilter(MapTypeEnum.ANY);
        private void LinearMapTypeFilter_Tapped(object sender, EventArgs e) => ChangeMapTypeFilter(MapTypeEnum.LINEAR);
        private void StagedMapTypeFilter_Tapped(object sender, EventArgs e) => ChangeMapTypeFilter(MapTypeEnum.STAGED);

        private void TierRangeSlider_LowerValueChanged(object sender, EventArgs e)
        {
            minTier = (int)TierRangeSlider.LowerValue;
            resetMin = (minTier != 1);
            checkReset();
        }

        private void TierRangeSlider_UpperValueChanged(object sender, EventArgs e)
        {
            maxTier = (int)TierRangeSlider.UpperValue;
            resetMax = (maxTier != 8);
            checkReset();
        }

        private void ResetLabel_Tapped(object sender, EventArgs e)
        {
            if (game != defaultGame)
            {
                ChangeGameFilter(defaultGame);
            }
            if (sort != SortEnum.NAME)
            {
                ChangeSortFilter(SortEnum.NAME);
            }
            if (mapType != MapTypeEnum.ANY)
            {
                ChangeMapTypeFilter(MapTypeEnum.ANY);
            }
            if (minTier != 1)
            {
                minTier = 1;
                TierRangeSlider.LowerValue = minTier;
            }
            if (maxTier != 8)
            {
                maxTier = 8;
                TierRangeSlider.UpperValue = maxTier;
            }
        }
    }

    #endregion
}
using System.ComponentModel;

using Xamarin.Forms;

namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Event Handler for the initial (lazy) load of tabs
        protected override async void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            if (CurrentPage is NavigationPage navpage)
            {
                if (navpage.CurrentPage is MapsPage mapspage)
                {
                    await mapspage.OnChangedTabAppearing();
                }
                else if (navpage.CurrentPage is PlayerPage playerpage)
                {
                    await playerpage.OnChangedTabAppearing();
                }
            }
        }
    }
}
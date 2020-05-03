using System.ComponentModel;

using Xamarin.Forms;


namespace KSF_Surf.Views
{
    [DesignTimeVisible(false)]
    public partial class InfoPage : ContentPage
    {
        public InfoPage()
        {
            InitializeComponent();

            IconThanksLabel.Text = "In-app icons courtesy of Phillip Reilly ().";
            KSFThanksLabel.Text = "Thanks to Hardex for the app icon inspiration, Sam for the API help, unt0uch4bl3 for letting me make this app, and KSF for the many hours of joy!";         
            SignOffLabel.Text = "\U00002764 hesuka (Sean Hurley)";
        }
    }
}
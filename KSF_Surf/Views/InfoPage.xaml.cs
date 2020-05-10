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

            IconThanksLabel.Text = "In-app icons courtesy of Phillip Reilly (reillypd@dukes.jmu.edu).";
            KSFThanksLabel.Text = "Thanks to Hardex for the app icon inspiration, Sam for being Sam, unt0uch4bl3 for letting me make this app, and KSF for its dedication to surf!";         
        }
    }
}
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
            
            CopyrightsLabel.Text = "KSF data provided with permission from unt0uch4bl3. Twitch data provided in accordance with the Twitch Developer Services Agreement." +
                " Steam data provided in accordance with the Steam Web API Terms of Use.";
            PrivacyLabel.Text = "Privacy Policy: No user data is collected, stored, or shared, nor do users rely on an account to use this software. Software preferences such as " +
                "user Steam ID are stored locally (on device) and not on a server. Data from KSF, Steam, and Twitch is requested on behalf of the user by this software and no user information is shared" +
                " with these services by this software. If you have any questions, please email seandhurley@live.com. Changes to this policy will be shown here.";
            CopyrightLabel.Text = "Intellectual Property: The \"KSF Surf\" mobile application is the property of Sean Hurley, and subject to his intellectual property rights. " +
                "Copyright \U000000A9 2020 Sean Hurley. All rights reserved.";
        }
    }
}
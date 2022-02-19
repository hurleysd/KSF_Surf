using KSF_Surf.ViewModels;
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

            KSFThanksLabel.Text = "Thanks to Hardex for the app icon inspiration, Sam for being Sam, unt0uch4bl3 for letting me make this app, and KSF for its dedication to surf!";
            IconThanksLabel.Text = "In-app icons courtesy of Phillip Reilly (reillypd@dukes.jmu.edu).";

            CopyrightLabel.Text = "Intellectual Property: The \"KSF Surf\" mobile application is the property of Sean Hurley, and subject to his intellectual property rights. " +
                "Copyright \U000000A9 2022 Sean Hurley. All rights reserved.";
            VersionLabel.Text = "Version " + BaseViewModel.appVersionString;

            CopyrightsLabel.Text = "KSF data provided with permission from the owner, unt0uch4bl3. Steam data provided in accordance with the Steam Web API Terms of Use.";
            PrivacyLabel.Text = "Privacy Policy: No user data is collected, stored, or shared, nor do users rely on an account to use this software. Software preferences such as " +
                "user Steam ID are stored locally (on device) and not on a server. Data from KSF and Steam is requested on behalf of the user by this software and no user information is shared" +
                " with these services by this software, with the exception of device information for logging purposes. If you have any questions, please email seandhurley@live.com. Changes to this policy will be shown here.";

            const string MITcontent = "Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the \"Software\"), to deal " +
                "in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/ or sell copies of the Software, " +
                "and to permit persons to whom the Software is furnished to do so, subject to the following conditions:\n\nThe above copyright notice and this permission notice shall be included in all " +
                "copies or substantial portions of the Software.\n\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF " +
                "MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER " +
                "IN AN ACTION OF CONTRACT,TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.";

            Forms9PatchLabel.Text = "Forms9Patch\n\nThe MIT License (MIT)\n\nCopyright(c) 42nd Parallel\n\nAll rights reserved.\n\n" + MITcontent;
            NETStandardLibraryLabel.Text = ".NET Runtime\n\nThe MIT License (MIT)\n\nCopyright(c) .NET Foundation and Contributors\n\nAll rights reserved.\n\n" + MITcontent;
            NewtonsoftJsonLabel.Text = "Netwonsoft.json\n\nThe MIT License (MIT)\n\nCopyright(c) 2007 James Newton-King\n\nAll rights reserved.\n\n" + MITcontent;
            RestSharpLabel.Text = "Restsharp\n\nCopyright 2009-2021 John Sheehan, Andrew Young, Alexey Zimarev and RestSharp community\n\nSPDX - License - Identifier: Apache - 2.0";
            XamarinAndroidXAppCombatResources.Text = "Xamarin.AndroidX.AppCombat.Resources\n\nThe MIT License (MIT)\n\nCopyright(c) .NET Foundation Contributors\n\n" + MITcontent;
            XamarinAndroidXBrowserLabel.Text = "Xamarin.AndroidX.Browser\n\nThe MIT License (MIT)\n\nCopyright(c) .NET Foundation Contributors\n\n" + MITcontent;
            XamarinAndroidXLegacyLabel.Text = "Xamarin.AndroidX.Legacy.Support.V4\n\nThe MIT License (MIT)\n\nCopyright(c) .NET Foundation Contributors\n\n" + MITcontent;
            XamarinAndroidXLifecycleLabel.Text = "Xamarin.AndroidX.LifeCycle.LiveData\n\nThe MIT License (MIT)\n\nCopyright(c) .NET Foundation Contributors\n\n" + MITcontent;
            XamarinAndroidXMediaRouterLabel.Text = "Xamarin.AndroidX.MediaRouter\n\nThe MIT License (MIT)\n\nCopyright(c) .NET Foundation Contributors\n\n" + MITcontent;
            XamarinEssentialsLabel.Text = "Xamarin.Essentials\n\nThe MIT License (MIT)\n\nCopyright(c) Microsoft Corporation\n\nAll rights reserved.\n\n" + MITcontent;
            XamarinFormsLabel.Text = "Xamarin.Forms\n\nThe MIT License (MIT)\n\nCopyright(c) Microsoft Corporation\n\nAll rights reserved.\n\n" + MITcontent;
            XamarinGoogleAndroidMaterialLabel.Text = "Xamarin.Google.Android.Material\n\nThe MIT License (MIT)\n\nCopyright(c) .NET Foundation Contributors\n\n" + MITcontent;
        }
    }
}
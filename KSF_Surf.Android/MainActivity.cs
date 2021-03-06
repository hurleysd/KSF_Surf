﻿using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Runtime;
using Android.OS;

using Xamarin.Forms;

namespace KSF_Surf.Droid
{
    [Activity(Label = "KSF Surf", Icon = "@mipmap/icon", Theme = "@style/Theme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.MainTheme); // change theme from splash screen

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            
            Forms.SetFlags("CollectionView_Experimental"); // needed for CollectionView

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Forms9Patch.Droid.Settings.Initialize(this);  // needed for Forms9Patch

            LoadApplication(new App());
            App.ApplyDarkTheme();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override Resources Resources
        {
            get
            {
                var config = new Configuration();
                config.SetToDefaults();

                return CreateConfigurationContext(config).Resources;
            }
        }

    }
}
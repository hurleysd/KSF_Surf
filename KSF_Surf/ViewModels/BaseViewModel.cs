using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Essentials;

using UIKit;

using KSF_Surf.Models;

namespace KSF_Surf.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        readonly static string deviceString = Device.RuntimePlatform;

        #region autogen
        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #endregion


        // System wide static methods ----------------------------------------------------------
        #region system
        internal static void vibrate(bool allowVibrate)
        {
            if (!allowVibrate) return;

            if (deviceString == Device.iOS)
            {
                if (Device.Idiom != TargetIdiom.Phone || !UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    return;
                }
                var impact = new UIImpactFeedbackGenerator(UIImpactFeedbackStyle.Light);
                impact.Prepare();
                impact.ImpactOccurred();
            }
        }

        internal static bool hasConnection()
        {
            var current = Connectivity.NetworkAccess;
            return (current == NetworkAccess.Internet);
        }

        // Properties Dicitonary --------------------------------------------------------------------------

        internal static string propertiesDict_getSteamID()
        {
            string id = "STEAM_0:0:47620794";  // Sean's steam ID
            if (App.Current.Properties.ContainsKey("steamid"))
            {
                id = App.Current.Properties["steamid"] as string;
            }
            else
            {
                App.Current.Properties.Add("steamid", id);
            }
            return id;
        }

        internal static EFilter_Game propertiesDict_getGame()
        {
            EFilter_Game game = EFilter_Game.css;

            if (App.Current.Properties.ContainsKey("game"))
            {
                string gameString = App.Current.Properties["game"] as string;

                switch (gameString)
                {
                    case "css": game = EFilter_Game.css; break;
                    case "css100t": game = EFilter_Game.css100t; break;
                    case "csgo": game = EFilter_Game.csgo; break;
                    default: goto case "css";
                }
            }
            else
            {
                App.Current.Properties.Add("game", EFilter_ToString.toString(game));
            }

            return game;
        }

        internal static EFilter_Mode propertiesDict_getMode()
        {
            EFilter_Mode mode = EFilter_Mode.fw;

            if (App.Current.Properties.ContainsKey("mode"))
            {
                string modeString = App.Current.Properties["mode"] as string;

                switch (modeString)
                {
                    case "FW": mode = EFilter_Mode.fw; break;
                    case "HSW": mode = EFilter_Mode.hsw; break;
                    case "SW": mode = EFilter_Mode.sw; break;
                    case "BW": mode = EFilter_Mode.bw; break;
                    default: goto case "FW";
                }
            }
            else
            {
                App.Current.Properties.Add("mode", EFilter_ToString.toString(mode));
            }

            return mode;

        }
    }
    #endregion
}

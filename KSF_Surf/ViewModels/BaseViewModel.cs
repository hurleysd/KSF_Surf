using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Essentials;

using UIKit;


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
                if (Device.Idiom != TargetIdiom.Phone || !UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
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
        #endregion
    }
}

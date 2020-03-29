using System;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace KSF_Surf.ViewModels
{
    public class PlayersViewModel : BaseViewModel
    {
        public PlayersViewModel()
        {
            Title = "Players";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public ICommand OpenWebCommand { get; }
    }
}
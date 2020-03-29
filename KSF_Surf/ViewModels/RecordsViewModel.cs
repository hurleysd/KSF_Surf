using System;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace KSF_Surf.ViewModels
{
    public class RecordsViewModel : BaseViewModel
    {
        public RecordsViewModel()
        {
            Title = "Records";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public ICommand OpenWebCommand { get; }
    }
}
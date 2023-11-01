using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(KSF_Surf.iOS.Renderers.ExtendedSearchBarRenderer))]
namespace KSF_Surf.iOS.Renderers
{
    // Custom searchbar renderer that hides the cancel button
    public class ExtendedSearchBarRenderer : SearchBarRenderer
	{
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == "Text") Control.ShowsCancelButton = false;
		}
	}
}


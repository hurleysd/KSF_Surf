using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(SearchBar), typeof(KSF_Surf.iOS.Renderers.ExtendedSearchBarRenderer))]

// Custom search bar renderer that removes the "cancel" button
//
// Taken from : https://gist.github.com/xleon/9f94a8482162460ceaf9
// Thanks to xleon
namespace KSF_Surf.iOS.Renderers
{
	public class ExtendedSearchBarRenderer : SearchBarRenderer
	{
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == "Text")
			{
				Control.ShowsCancelButton = false;
			}
		}
	}
}


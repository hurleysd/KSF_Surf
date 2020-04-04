using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using UIKit;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(SearchBar), typeof(Namespace.iOS.Renderers.ExtendedSearchBarRenderer))]


//Taken from : https://gist.github.com/xleon/9f94a8482162460ceaf9
//Thanks to xleon
namespace Namespace.iOS.Renderers
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


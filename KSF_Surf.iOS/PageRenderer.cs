﻿using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(KSF_Surf.iOS.Renderers.PageRenderer))]
namespace KSF_Surf.iOS.Renderers
{
    // Custom page renderer that responds to device theme changes
    public class PageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        { 
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null) return;

            try
            {
                App.ApplyTheme();
            }
            catch (Exception)
            {
            }
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);
            App.ApplyTheme();
        }
    }
}
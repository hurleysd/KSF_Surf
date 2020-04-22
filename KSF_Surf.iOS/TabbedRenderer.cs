using System;
using UIKit;

using Xamarin.Forms;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(KSF_Surf.iOS.Renderers.TabbedRenderer))]
namespace KSF_Surf.iOS.Renderers
{
    public class TabbedRenderer : Xamarin.Forms.Platform.iOS.TabbedRenderer
    {
        readonly nfloat imageYOffset = 7.0f;

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (TabBar.Items != null)
            {
                foreach (var item in TabBar.Items)
                {
                    item.Title = null;
                    item.ImageInsets = new UIEdgeInsets(imageYOffset, 0, -imageYOffset, 0);
                }
            }
        }
    }
}
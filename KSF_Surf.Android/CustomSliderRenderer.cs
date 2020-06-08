using Android.Content;
using Android.Graphics;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Slider), typeof(KSF_Surf.Droid.Renderers.CustomSliderRenderer))]
namespace KSF_Surf.Droid.Renderers
{
    // Custom  renderer to set the tint color of sliders
    public class CustomSliderRenderer : SliderRenderer
    {
        public CustomSliderRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.ProgressBackgroundTintMode = PorterDuff.Mode.Src;
            }
        }
    }
}
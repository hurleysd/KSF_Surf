using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Widget;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.SearchBar), typeof(KSF_Surf.Droid.Renderers.CustomIconColorSearchBarRenderer))]
namespace KSF_Surf.Droid.Renderers
{
    // Custom  renderer to set the color of the search bar icon
    public class CustomIconColorSearchBarRenderer : SearchBarRenderer
    {
        public CustomIconColorSearchBarRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            var icon = Control?.FindViewById(Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null));
            (icon as ImageView)?.SetColorFilter(Android.Graphics.Color.WhiteSmoke);
        }
    }
}
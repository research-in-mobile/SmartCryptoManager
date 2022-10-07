using System.ComponentModel;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using SmartCryptoManager.Controls;
using SmartCryptoManager.Droid.Renderers;

[assembly: ExportRenderer(typeof(ExtendedEntryControl), typeof(ExtendedEntryRenderer))]
namespace SmartCryptoManager.Droid.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        public ExtendedEntryControl ExtendedEntryElement => Element as ExtendedEntryControl;

        public ExtendedEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.InputType |= Android.Text.InputTypes.TextFlagNoSuggestions;
                UpdateLineColor();
            }

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals(nameof(ExtendedEntryControl.LineColor)))
            {
                UpdateLineColor();
            }
        }

        private void UpdateLineColor()
        {
            Control?.Background?.SetColorFilter(ExtendedEntryElement.LineColor.ToAndroid(), Android.Graphics.PorterDuff.Mode.SrcAtop);
        }
    }
}
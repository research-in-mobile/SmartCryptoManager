using Xamarin.Forms;

namespace SmartCryptoManager.Controls
{
    public class ExtendedEntryControl : Entry
    {
        public static readonly BindableProperty LineColorProperty =
            BindableProperty.Create("LineColor", typeof(Color), typeof(ExtendedEntryControl), Color.Default);

        public Color LineColor
        {
            get => (Color)GetValue(LineColorProperty);
            set => SetValue(LineColorProperty, value);
        }

    }
}
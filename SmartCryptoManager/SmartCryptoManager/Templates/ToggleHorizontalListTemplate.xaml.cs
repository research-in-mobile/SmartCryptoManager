using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartCryptoManager.Templates
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ToggleHorizontalListTemplate : ContentView
	{
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(ToggleHorizontalListTemplate),
                default(IEnumerable<object>),
                BindingMode.TwoWay);

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public ToggleHorizontalListTemplate ()
		{
			InitializeComponent ();
		}
    }
}
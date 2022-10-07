using Prism.Navigation;
using Xamarin.Forms;

namespace SmartCryptoManager.Views
{
    public partial class TabbedNavigationPage : TabbedPage, INavigatingAware
    {
        public TabbedNavigationPage()
        {
            InitializeComponent();
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            foreach (var child in Children)
            {
                (child as INavigatingAware)?.OnNavigatingTo(parameters);
                (child?.BindingContext as INavigatingAware)?.OnNavigatingTo(parameters);
            }
        }
    }
}

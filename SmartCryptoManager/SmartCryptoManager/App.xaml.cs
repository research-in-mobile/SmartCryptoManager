using Prism;
using Prism.Ioc;
using SmartCryptoManager.Services;
using SmartCryptoManager.ViewModels;
using SmartCryptoManager.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SmartCryptoManager
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("SlidingPanelPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterNavigation(containerRegistry);
            RegisterServices(containerRegistry);
            
        }

        private void RegisterNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<TabbedNavigationPage, TabbedNavigationPageViewModel>();
            containerRegistry.RegisterForNavigation<WatchPage, WatchPageViewModel>();
            containerRegistry.RegisterForNavigation<ManagePage, ManagePageViewModel>();
            containerRegistry.RegisterForNavigation<CoinDetailPage, CoinDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<SlidingPanelPage, SlidingPanelPageViewModel>();
        }

        private void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IRequestService, RequestService>();
        }

    }
}

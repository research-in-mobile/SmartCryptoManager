using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SmartCryptoManager.ViewModels
{
    public class WatchPageViewModel : BaseViewModel
    {
        public WatchPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Watch";
        }
    }
}

using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartCryptoManager.ViewModels
{
	public class ManagePageViewModel : BaseViewModel
	{
        public ManagePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Manage";

        }
	}
}

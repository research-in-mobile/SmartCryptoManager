using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartCryptoManager.ViewModels
{
	public class SlidingPanelPageViewModel : BaseViewModel
	{
        public SlidingPanelPageViewModel(INavigationService navigationService) 
            :base(navigationService)
        {

        }
	}
}

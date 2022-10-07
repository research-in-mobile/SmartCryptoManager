using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartCryptoManager.ViewModels
{
    public class BaseViewModel : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }

        protected CancellationTokenSource CTS = new CancellationTokenSource();
        protected CancellationToken CT { get; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public BaseViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            CT = CTS.Token;
        }
        ~BaseViewModel()
        {
            CancelAllTasks();
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatingTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {
            CancelAllTasks();
        }

        protected void CancelAllTasks()
        {
            if (!CTS.IsCancellationRequested) CTS.Cancel();
        }

        #region Busy Mechanism
        private static readonly Guid _defaultTracker = new Guid("A7848922-C10A-4D6A-9D82-4987F638F718");
        private readonly IList<Guid> _busyLocks = new List<Guid>();

        public bool IsBusy
        {
            get => _busyLocks.Any();
            set
            {
                if (value && !_busyLocks.Contains(_defaultTracker))
                {
                    _busyLocks.Add(_defaultTracker);
                    RaisePropertyChanged(nameof(IsBusy));
                }

                if (!value && _busyLocks.Contains(_defaultTracker))
                {
                    _busyLocks.Remove(_defaultTracker);
                    RaisePropertyChanged(nameof(IsBusy));
                }
            }
        }

        protected BusyHelper Busy() => new BusyHelper(this);

        public Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        protected sealed class BusyHelper : IDisposable
        {
            private readonly BaseViewModel _viewModel;
            private readonly Guid _tracker;

            public BusyHelper(BaseViewModel viewModel)
            {
                _viewModel = viewModel;
                _tracker = new Guid();
                _viewModel._busyLocks.Add(_tracker);
                _viewModel.RaisePropertyChanged(nameof(IsBusy));
            }

            public void Dispose()
            {
                _viewModel._busyLocks.Remove(_tracker);
                _viewModel.RaisePropertyChanged(nameof(IsBusy));
            }
        }
        #endregion
    }
}

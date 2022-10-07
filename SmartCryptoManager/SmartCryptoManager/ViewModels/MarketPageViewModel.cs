using CryptoCompare;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SmartCryptoManager.Models;

namespace SmartCryptoManager.ViewModels
{
    public class MarketPageViewModel : BaseViewModel
    {
        private const int PageSize = 100;
        private int pageNumber = 0;

        private bool _isLoadingData;
        private ObservableCollection<TopMarketCapInfo> _topCoins;
        private ObservableCollection<CoinCapInfo> _coinCapInfo;

        public DelegateCommand LoadMoreCoinsCommand { get; set; }
        public DelegateCommand RefreshPageCommand { get; set; }
        public DelegateCommand<object> ItemTappedCommand { get; set; }

        public ObservableCollection<TopMarketCapInfo> TopCoins
        {
            get => _topCoins;
            set => SetProperty(ref _topCoins, value);
        }

        public ObservableCollection<CoinCapInfo> CoinCapInfo
        {
            get => _coinCapInfo;
            set => SetProperty(ref _coinCapInfo, value);
        }

        public bool IsLoadingData
        {
            get => _isLoadingData;
            set => SetProperty(ref _isLoadingData, value);
        }

        public bool UseAdaptiveLoading { get; set; }

        public MarketPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Market";

            ItemTappedCommand = new DelegateCommand<object>(async (item) =>
            {
                var coin = item as TopMarketCapInfo;

                var parameters = new NavigationParameters();
                parameters.Add("Symbol", coin.CoinInfo.Name);
                parameters.Add("CoinTopMarketCapInfo", coin);

                await NavigationService.NavigateAsync("CoinDetailPage", parameters);
            });

            LoadMoreCoinsCommand = new DelegateCommand(async () => await LoadNextPage());
            RefreshPageCommand = new DelegateCommand(async () => await RefreshPage());
            RefreshPageCommand.Execute();
        }

        private async Task LoadNextPage()
        {
            if (IsBusy) return;

            using (Busy())
            {
                IsLoadingData = true;

                var result = await CryptoCompareClient.Instance.Tops.CoinFullDataByMarketCap("USD", PageSize, pageNumber);

                foreach (var item in result.Data.ToList())
                {
                    TopCoins.Add(item);
                }

                pageNumber++;

                IsLoadingData = false;
            }
        }

        private async Task RefreshPage()
        {
            pageNumber = 0;
            TopCoins = new ObservableCollection<TopMarketCapInfo>();
            await LoadNextPage();
            CoinCapInfo = new ObservableCollection<CoinCapInfo>(await GetMarketCapPercentage(TopCoins, 5));
        }

        private Task<IList<CoinCapInfo>> GetMarketCapPercentage(IList<TopMarketCapInfo> topMarketCapCoins, int numberOfTopCoinReturned)
        {
            var result = new List<CoinCapInfo>();
            if (topMarketCapCoins == null) return Task.FromResult<IList<CoinCapInfo>>(result);

            for (int i = 0; i < numberOfTopCoinReturned; i++)
            {

                CoinFullAggregatedData rawData;
                topMarketCapCoins[i].Raw.TryGetValue("USD", out rawData);

                var coin = new CoinCapInfo()
                {
                    Name = topMarketCapCoins[i].CoinInfo.FullName,
                    Symbol = topMarketCapCoins[i].CoinInfo.Name,
                    MarketCap = (double)rawData.MarketCap
                };

                result.Add(coin);
            }

            return Task.FromResult<IList<CoinCapInfo>>(result);

        }
    }
}

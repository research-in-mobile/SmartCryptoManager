using CryptoCompare;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartCryptoManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartCryptoManager.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private const int PageSize = 100;
        private int page = 0;


        private ObservableCollection<TopMarketCapInfo> _topCoins;
        public ObservableCollection<TopMarketCapInfo> TopCoins
        {
            get => _topCoins;
            set
            {
                if (value != _topCoins)
                {
                    _topCoins = value;
                    RaisePropertyChanged();
                }
            }
        }

        //public bool UseAdaptiveLoading { get; set; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            //UseAdaptiveLoading = true;

            TopCoins = new ObservableCollection<TopMarketCapInfo>();

            Task.Run(async () => await Load());

            LoadMore = new DelegateCommand(async () => await Load());
        }

        private async Task Load()
        {
            var result = await CryptoCompareClient.Instance.Tops.CoinFullDataByMarketCap("USD", PageSize, page);

            foreach (var item in result.Data.ToList())
            {
                TopCoins.Add(item);
            }

            page++;
        }


        public ICommand LoadMore
        {
            get;
            set;
        }




        //public InfiniteScrollCollection<TopMarketCapInfo> Items { get; set; }

        ////int _preLoad = 10;
        ////public int PreLoad
        ////{
        ////    get => _preLoad;
        ////    set
        ////    {
        ////        _preLoad = value;
        ////        RaisePropertyChanged();
        ////    }
        ////}

        //public ICommand RefreshCommand { get; }
        //public ICommand ItemAppearingCommand { get; }

        //private const int PageSize = 50;

        //private bool _isWorking;

        //public bool IsWorking
        //{
        //    get => _isWorking;
        //    set
        //    {
        //        _isWorking = value;
        //        RaisePropertyChanged();
        //    }
        //}

        //public MainPageViewModel(INavigationService navigationService, IRequestService requestService)
        //    : base(navigationService)
        //{
        //    _requestService = requestService;

        //    Title = "Main Page";
        //    TopCoins = new List<TopMarketCapInfo>();


        //    Task.Run(async () =>
        //    {
        //        //TopCoins = new List<TopMarketCapInfo>();
        //        //var result = await _requestService.GetAsync<TopMarketCapResponse>("https://min-api.cryptocompare.com/data/top/mktcapfull?limit=100&tsym=USD", IsMocked: true);





        //        Device.BeginInvokeOnMainThread(async () =>
        //        {

        //            var result = await _requestService.GetAsync<TopMarketCapResponse>("https://min-api.cryptocompare.com/data/top/mktcapfull?limit=100&tsym=USD", IsMocked: true);
        //            //LoadNext(result.Data.ToList());


        //            foreach (var item in result.Data.ToList())
        //            {
        //                TopCoins.Add(item);
        //            }
        //        });

        //        //await LoadNext();
        //    });





        //    //Task.Run(async () =>
        //    //{
        //    //    //var request = await requestService.GetAsync<TopMarketCapResponse>("https://min-api.cryptocompare.com/data/top/mktcapfull?limit=10&tsym=USD");
        //    //    //var CCC = new CryptoCompareClient("4f4f38012ecfcd9d7ad0c0e22b7e45f040956a861d3db1d3f68b36256c1c138e");

        //    //    var result = await CryptoCompareClient.Instance.Tops.CoinFullDataByMarketCap("USD", 100, 0);
        //    //    //var result = await requestService.GetAsync<TopMarketCapResponse>("https://min-api.cryptocompare.com/data/top/mktcapfull?limit=100&tsym=USD", IsMocked: true);
        //    //    var result1 = await CryptoCompareClient.Instance.Tops.CoinFullDataByMarketCap("USD", 100, 1);
        //    //    //var result2 = await CryptoCompareClient.Instance.Tops.CoinFullDataByMarketCap("USD", 100, 2);



        //    //    TopCoins.AddRange(result.Data.ToList());
        //    //    TopCoins.AddRange(result1.Data.ToList());
        //    //    //TopCoins.AddRange(result2.Data.ToList());
        //    //    RaisePropertyChanged("");

        //    //});


        //    //Items = new InfiniteScrollCollection<TopMarketCapInfo>
        //    //{
        //    //    OnLoadMore = async () =>
        //    //    {
        //    //        // load the next page
        //    //        var page = Items.Count / PageSize;

        //    //        var result = await CryptoCompareClient.Instance.Tops.CoinFullDataByMarketCap("USD", PageSize, page + 1);
        //    //        //TopCoins = result.Data.ToList();

        //    //        var items = result.Data.ToList();

        //    //        //CoinFullAggregatedDataDisplay coinFullAggregatedDataDisplay;
        //    //        //var ss = items[0].Display.Values.ToList();

        //    //        return items;
        //    //    }
        //    //};

        //    RefreshCommand = new Command(() =>
        //   {
        //       //var result = await requestService.GetAsync<TopMarketCapResponse>("https://min-api.cryptocompare.com/data/top/mktcapfull?limit=100&tsym=USD", IsMocked: true);

        //       //TopCoins = new List<TopMarketCapInfo>(result.Data.ToList());

        //       //// clear and start again
        //       //Items.Clear();
        //       //Items.LoadMoreAsync();


        //       Device.BeginInvokeOnMainThread(async () =>
        //       {

        //           var result = await _requestService.GetAsync<TopMarketCapResponse>("https://min-api.cryptocompare.com/data/top/mktcapfull?limit=100&tsym=USD", IsMocked: true);
        //           //LoadNext(result.Data.ToList());


        //           foreach (var item in result.Data.ToList())
        //           {
        //               TopCoins.Add(item);
        //           }
        //       });

        //       IsWorking = false;
        //   });



        //    //ItemAppearingCommand = new Command(() =>
        //    //{

        //    //});

        //    //// load the initial data
        //    //Items.LoadMoreAsync();


        //}

        //private void LoadNext(IEnumerable<TopMarketCapInfo> list)
        //{
        //    Device.BeginInvokeOnMainThread(() => {

        //        foreach (var item in list)
        //        {
        //            TopCoins.Add(item);
        //        }

        //        //TopCoins.AddRange(list);
        //        //RaisePropertyChanged(nameof(TopCoins));
        //    });

        //}

    }
}

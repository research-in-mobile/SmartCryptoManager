using CryptoCompare;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SmartCryptoManager.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UltimateXF.Widget.Charts.Models.CandleStickChart;

namespace SmartCryptoManager.ViewModels
{
    public enum SpanType
    {
        Minute,
        Hour,
        Day,
        Week
    }

    public class ChartDataPointSpan : BindableBase
    {
        public string Name
        {
            get
            {
                return Points.ToString();
            }
        }

        public int Points { get; private set; }

        private bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public ChartDataPointSpan(int points)
        {
            Points = points;
        }

    }

    public class CandleSpan : BindableBase
    {
        public string SpanString
        {
            get
            {
                return string.Join(string.Empty, Span.ToString(), " ", CandleSpanType, Span > 1 ? "s" : "");
            }
        }
        public SpanType CandleSpanType { get; private set; }
        public int Span { get; private set; }

        private bool _isActive = false;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public CandleSpan(int span, SpanType intervalSpan)
        {
            Span = span;
            CandleSpanType = intervalSpan;
        }

    }

    public class CoinDetailPageViewModel : BaseViewModel
    {
        private List<ICandleStickDataSet> _candleStrickChartData;
        public List<ICandleStickDataSet> CandleStickData
        {
            get => _candleStrickChartData;
            set => SetProperty(ref _candleStrickChartData, value);
        }

        private List<string> _labels;
        public List<string> Labels
        {
            get => _labels;
            set => SetProperty(ref _labels, value);
        }

        public List<CandleSpan> CandleSpans => CandleSpanList;
        public List<ChartDataPointSpan> ChartDataPointSpans => ChartDataPointSpanList;

        private int _selectedCandleSpansIndex;
        public int SelectedCandleSpansIndex
        {
            get => _selectedCandleSpansIndex;
            set
            {
                if (_selectedCandleSpansIndex != value)
                {
                    _selectedCandleSpansIndex = value;

                    RaisePropertyChanged(nameof(SelectedCandleSpansIndex));

                    CandleSpanSelected = CandleSpanList[_selectedCandleSpansIndex];
                }
            }
        }

        private CandleSpan _candleSpanSelected;
        public CandleSpan CandleSpanSelected { get; set; }


        private ChartDataPointSpan _chartDataPointSpanSelected;
        public ChartDataPointSpan ChartDataPointSpanSelected { get; set; }

        private bool _canChangeChart = true;
        public bool CanChangeChart
        {
            get => _canChangeChart;
            set => SetProperty(ref _canChangeChart, value);
        }

        public ICommand CandleSpansChangedCommand { get; set; }
        public ICommand ChartDataPointSpanChangedCommand { get; set; }
        public ICommand ToggleChartOptionsCommand { get; set; }


        public CoinDetailPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            CandleSpansChangedCommand = new DelegateCommand(() =>
            {
                if (CandleSpanSelected == _candleSpanSelected || CandleSpanSelected == null)
                    return;

                CandleSpans.ForEach(c => c.IsActive = false);
                CandleSpanSelected.IsActive = true;
                _candleSpanSelected = CandleSpanSelected;
            });

            ChartDataPointSpanChangedCommand = new DelegateCommand(() =>
            {
                if (ChartDataPointSpanSelected == _chartDataPointSpanSelected || ChartDataPointSpanSelected == null)
                    return;

                ChartDataPointSpans.ForEach(c => c.IsActive = false);
                ChartDataPointSpanSelected.IsActive = true;
                _chartDataPointSpanSelected = ChartDataPointSpanSelected;
            });

            ToggleChartOptionsCommand = new DelegateCommand(() =>
            {
                CanChangeChart = !CanChangeChart;
            });
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            var hasCoinSymbol = parameters.TryGetValue("Symbol", out string coinSymbol);
            var hasCoinTopMarketCapInfo = parameters.TryGetValue("CoinTopMarketCapInfo", out TopMarketCapInfo coinTopMarketCapInfo);

            if (hasCoinSymbol)
                Task.Run(async () => await LoadCoinDetails(coinSymbol), CT);
            else
                Task.Run(async () => await LoadCoinDetails(""), CT);

        }

        private async Task LoadCoinDetails(string symbol)
        {

            var result = await CryptoCompareClient.Instance.History.DailyAsync("BTC", "USD", 365);
            int count = 0;

            var labels = new List<string>();
            var candleSticks = new List<CandleStickEntry>();
            string dateTime = string.Empty;

            foreach (var candle in result.Data)
            {
                candleSticks.Add(new CandleStickEntry(count, (double)candle.High, (double)candle.Low, (double)candle.Open, (double)candle.Close));

                MonthToString.TryGetValue(candle.Time.Month, out dateTime);


                labels.Add(string.Join(".", dateTime, candle.Time.Day));
                count++;
            }

            var dataSet = new CandleStickDataSet(candleSticks, "")
            {
                DecreasingColor = Color.Red,
                IncreasingColor = Color.Green,
                DecreasingPaintStyle = PaintStyle.FILL_AND_STROKE,
                IncreasingPaintStyle = PaintStyle.FILL_AND_STROKE,
                ShadowColorSameAsCandle = true,
                BarSpace = 0.05f,
                DrawValues = false,
                ShowCandleBar = true,
                HighlightEnabled = true
            };

            Labels = new List<string>(labels);
            CandleStickData = new List<ICandleStickDataSet>() { dataSet };

        }

        public static Dictionary<int, string> MonthToString = new Dictionary<int, string>()
        {
            {1,"Jan" },
            {2, "Feb"},
            {3, "Mar"},
            {4, "Apr"},
            {5, "May"},
            {6, "Jun"},
            {7, "Jul"},
            {8, "Aug"},
            {9, "Sep"},
            {10, "Oct"},
            {11, "Nov"},
            {12, "Dec"}
        };

        public static List<CandleSpan> CandleSpanList = new List<CandleSpan>()
        {
            new CandleSpan(5, SpanType.Minute),
            new CandleSpan(15, SpanType.Minute),
            new CandleSpan(30, SpanType.Minute),
            new CandleSpan(1, SpanType.Hour),
            new CandleSpan(2, SpanType.Hour),
            new CandleSpan(4, SpanType.Hour),
            new CandleSpan(6, SpanType.Hour),
            new CandleSpan(12, SpanType.Hour),
            new CandleSpan(1, SpanType.Day),
            new CandleSpan(1, SpanType.Week),
        };

        public static List<ChartDataPointSpan> ChartDataPointSpanList = new List<ChartDataPointSpan>()
        {
            new ChartDataPointSpan(7),
            new ChartDataPointSpan(10),
            new ChartDataPointSpan(30),
            new ChartDataPointSpan(60),
            new ChartDataPointSpan(90),
            new ChartDataPointSpan(183),
            new ChartDataPointSpan(250),
            new ChartDataPointSpan(365),
            new ChartDataPointSpan(500),
            new ChartDataPointSpan(1000),
            new ChartDataPointSpan(1500),
            new ChartDataPointSpan(2000),
            new ChartDataPointSpan(4000),
            new ChartDataPointSpan(8000),
            new ChartDataPointSpan(10000),

        };
    }
}

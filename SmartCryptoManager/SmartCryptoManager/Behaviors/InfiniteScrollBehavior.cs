using SmartCryptoManager.Services;
using System;
using System.Collections;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartCryptoManager.Behaviors
{
    public class InfiniteScrollBehavior : BaseBehavior<ListView>
    {
        public static readonly BindableProperty LoadMoreCommandProperty =
            BindableProperty.Create(
                nameof(LoadMoreCommand),
                typeof(ICommand),
                typeof(InfiniteScrollBehavior),
                null);

        public static readonly BindableProperty UseAdaptiveLoadingProperty =
            BindableProperty.Create(
                nameof(UseAdaptiveLoading),
                typeof(bool),
                typeof(InfiniteScrollBehavior),
                false);

        public ICommand LoadMoreCommand
        {
            get
            {
                return (ICommand)GetValue(LoadMoreCommandProperty);
            }
            set
            {
                SetValue(LoadMoreCommandProperty, value);
            }
        }


        public bool UseAdaptiveLoading
        {
            get
            {
                return (bool)GetValue(UseAdaptiveLoadingProperty);
            }
            set
            {
                SetValue(UseAdaptiveLoadingProperty, value);
            }
        }

        private Timer _timer;
        private DateTime _previousDateTime;
        private int _itemsScrolled;
        private int _itemScrollRate;

        public InfiniteScrollBehavior()
        {
            //InititilizeAdaptiveLoading();
            //_timer.Start();
        }

        public void InititilizeAdaptiveLoading()
        {
            _timer = new Timer { Interval = 250 };
            _previousDateTime = DateTime.UtcNow;
            _itemsScrolled = 0;

            _timer.Elapsed += ElapsedTime;
        }

        public void RemoveAdaptiveLoading()
        {
            _timer.Elapsed -= ElapsedTime;
        }


        private void ElapsedTime(object sender, ElapsedEventArgs e)
        {
            //var span = _previousDateTime - DateTime.UtcNow;
            //_previousDateTime = DateTime.UtcNow;
            _itemScrollRate = (int)Math.Round((_itemsScrolled / 0.25), 0);
            _itemsScrolled = 0;
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ItemAppearing += InfiniteListView_ItemAppearing;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemAppearing -= InfiniteListView_ItemAppearing;
        }

        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = AssociatedObject.ItemsSource as IList;
            if (items == null) return;

            _itemsScrolled++;

            if (UseAdaptiveLoading)
            {
                if (e.Item == items[items.Count - 1 - 25])
                {
                    if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                    {
                        LoadMoreCommand.Execute(null);
                    }
                }
            }
            else
            {
                if (e.Item == items[items.Count - 1])
                {
                    if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null)) LoadMoreCommand.Execute(null);
                }
            }

        }
    }
}
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartCryptoManager.Controls
{
    public class ExtendedListViewControl : ListView
    {
        public static readonly BindableProperty LoadMoreCommandProperty =
            BindableProperty.Create(nameof(LoadMoreCommand), typeof(ICommand), typeof(ExtendedListViewControl), default(ICommand));

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        public ExtendedListViewControl()
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }

        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = ItemsSource as IList;

            if (items != null && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                    LoadMoreCommand.Execute(null);
            }
        }
    }
}

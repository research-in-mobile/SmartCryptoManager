using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartCryptoManager.Controls
{
    public class HorizontalListControl : Grid
    {
        ICommand innerSelectedCommand;
        readonly ScrollView scrollView;
        readonly StackLayout itemsStackLayout;

        public event EventHandler SelectedItemChanged;
        public StackOrientation Orientation { get; set; }
        public bool ShowScrollBar { get; set; }
        public double Spacing { get; set; }

        public static readonly BindableProperty SelectedCommandProperty =
            BindableProperty.Create(nameof(SelectedCommand),
                typeof(ICommand),
                typeof(HorizontalListControl), null);

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource),
                typeof(IEnumerable),
                typeof(HorizontalListControl),
                default(IEnumerable<object>),
                BindingMode.TwoWay, propertyChanged: ItemsSourceChanged);

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem),
                typeof(object),
                typeof(HorizontalListControl),
                null,
                BindingMode.TwoWay,
                propertyChanged: OnSelectedItemChanged);

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(HorizontalListControl),
                default(DataTemplate));

        public ICommand SelectedCommand
        {
            get => (ICommand)GetValue(SelectedCommandProperty);
            set => SetValue(SelectedCommandProperty, value);
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public object SelectedItem
        {
            get => (object)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemsLayout = (HorizontalListControl)bindable;
            itemsLayout.SetItems();
        }

        public HorizontalListControl()
        {
            scrollView = new ScrollView()
            {
                HorizontalScrollBarVisibility = (ShowScrollBar) ? ScrollBarVisibility.Always : ScrollBarVisibility.Never
            };

            itemsStackLayout = new StackLayout
            {
                BackgroundColor = BackgroundColor,
                Padding = Padding,
                Spacing = Spacing,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            scrollView.BackgroundColor = BackgroundColor;
            scrollView.Content = itemsStackLayout;
            Children.Add(scrollView);
        }

        protected virtual void SetItems()
        {
            itemsStackLayout.Children.Clear();
            itemsStackLayout.Spacing = Spacing;

            innerSelectedCommand = new Command<View>(view =>
            {
                SelectedItem = view.BindingContext;
                SelectedItem = null; // Allowing item second time selection
            });

            itemsStackLayout.Orientation = Orientation;
            scrollView.Orientation = Orientation == StackOrientation.Horizontal
                ? ScrollOrientation.Horizontal
                : ScrollOrientation.Vertical;

            if (ItemsSource == null)
            {
                return;
            }

            foreach (var item in ItemsSource)
            {
                itemsStackLayout.Children.Add(GetItemView(item));
            }

            itemsStackLayout.BackgroundColor = BackgroundColor;
            SelectedItem = null;
        }

        protected virtual View GetItemView(object item)
        {
            var content = ItemTemplate.CreateContent();

            if (!(content is View view))
            {
                return null;
            }

            view.BindingContext = item;

            var gesture = new TapGestureRecognizer
            {
                Command = innerSelectedCommand,
                CommandParameter = view
            };

            AddGesture(view, gesture);

            return view;
        }

        private void AddGesture(View view, TapGestureRecognizer gesture)
        {
            view.GestureRecognizers.Add(gesture);

            if (!(view is Layout<View> layout))
            {
                return;
            }

            foreach (var child in layout.Children)
            {
                AddGesture(child, gesture);
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemsView = (HorizontalListControl)bindable;
            if (newValue == oldValue && newValue != null)
            {
                return;
            }

            itemsView.SelectedItemChanged?.Invoke(itemsView, EventArgs.Empty);

            if (itemsView.SelectedCommand?.CanExecute(newValue) ?? false)
            {
                itemsView.SelectedCommand?.Execute(newValue);
            }
        }
    }
}
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    partial class Carousel
    {
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Carousel), new PropertyMetadata(-1, SelectedIndexChanged));
        public static readonly DependencyProperty MaxItemsProperty = DependencyProperty.Register("MaxItems", typeof(int), typeof(Carousel), new PropertyMetadata(3, MaxItemsChanged));
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(Carousel), new PropertyMetadata(null));
        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(Carousel), new PropertyMetadata(1.6, OnInvalidate));
        public static readonly DependencyProperty AlignmentXProperty = DependencyProperty.Register("AlignmentX", typeof(AlignmentX), typeof(Carousel), new PropertyMetadata(AlignmentX.Left, OnInvalidate));
        public static readonly DependencyProperty GradientOpacityProperty = DependencyProperty.Register("GradientOpacity", typeof(double), typeof(Carousel), new PropertyMetadata(0.0));
        public static readonly DependencyProperty ItemClickCommandProperty = DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(Carousel), new PropertyMetadata(null, ItemClickCommandChanged));

        private bool _disableSelectedIndexCallback = false;

        public IList<object> Items => _items;

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set
            {
                // NOTE: Avoid external exceptions when this property is binded
                try
                {
                    SetValue(SelectedIndexProperty, value);
                }
                catch { }
            }
        }


        public int MaxItems
        {
            get { return (int)GetValue(MaxItemsProperty); }
            set { SetValue(MaxItemsProperty, value); }
        }


        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }


        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }


        public AlignmentX AlignmentX
        {
            get { return (AlignmentX)GetValue(AlignmentXProperty); }
            set { SetValue(AlignmentXProperty, value); }
        }


        public double GradientOpacity
        {
            get { return (double)GetValue(GradientOpacityProperty); }
            set { SetValue(GradientOpacityProperty, value); }
        }


        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        private static void ItemClickCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            control.SetItemClickCommand(e.NewValue as ICommand);
        }


        private void SetItemClickCommand(ICommand command)
        {
            if (_container != null)
            {
                foreach (CarouselSlot item in _container.Children)
                {
                    item.ItemClickCommand = command;
                }
            }
        }

        private static void OnInvalidate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            control.InvalidateMeasure();
        }

        private static void MaxItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            control.BuildSlots();
            control.ArrangeItems();
            control.InvalidateMeasure();
        }

        private static void SelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            if (control._disableSelectedIndexCallback)
            {
                return;
            }

            if ((int)e.NewValue > -1)
            {
                control.ArrangeItems();
            }
        }
    }
}

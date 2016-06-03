using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    partial class Carousel
    {
        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Carousel), new PropertyMetadata(-1, SelectedIndexChanged));
        /// <summary>
        /// Identifies the <see cref="MaxItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxItemsProperty = DependencyProperty.Register("MaxItems", typeof(int), typeof(Carousel), new PropertyMetadata(3, MaxItemsChanged));
        /// <summary>
        /// Identifies the <see cref="ContentTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(Carousel), new PropertyMetadata(null));
        /// <summary>
        /// Identifies the <see cref="AspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(Carousel), new PropertyMetadata(1.6, OnInvalidate));
        /// <summary>
        /// Identifies the <see cref="AlignmentX"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AlignmentXProperty = DependencyProperty.Register("AlignmentX", typeof(AlignmentX), typeof(Carousel), new PropertyMetadata(AlignmentX.Left, OnInvalidate));
        /// <summary>
        /// Identifies the <see cref="GradientOpacity"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GradientOpacityProperty = DependencyProperty.Register("GradientOpacity", typeof(double), typeof(Carousel), new PropertyMetadata(0.0));
        /// <summary>
        /// Identifies the <see cref="ItemClickCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemClickCommandProperty = DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(Carousel), new PropertyMetadata(null, ItemClickCommandChanged));

        private bool _disableSelectedIndexCallback;

        /// <summary>
        /// Gets the list of items associated with the control.
        /// </summary>
        public IList<object> Items => _items;

        /// <summary>
        /// Gets or sets the index of the selected option in a select object.
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of items.
        /// </summary>
        public int MaxItems
        {
            get { return (int)GetValue(MaxItemsProperty); }
            set { SetValue(MaxItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> associated with content.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the aspect ratio to use.
        /// </summary>
        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="AlignmentX"/>.
        /// This value is used to align items when all displayble items do not fit into the screen.
        /// </summary>
        public AlignmentX AlignmentX
        {
            get { return (AlignmentX)GetValue(AlignmentXProperty); }
            set { SetValue(AlignmentXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the gradient used for the opcaity on X axis.
        /// </summary>
        public double GradientOpacity
        {
            get { return (double)GetValue(GradientOpacityProperty); }
            set { SetValue(GradientOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command associated with a click on an item.
        /// </summary>
        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        private static void ItemClickCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            control?.SetItemClickCommand(e.NewValue as ICommand);
        }


        private void SetItemClickCommand(ICommand command)
        {
            if (_container == null)
            {
                return;
            }

            foreach (var uiElement in _container.Children)
            {
                var item = uiElement as CarouselSlot;
                if (item != null)
                {
                    item.ItemClickCommand = command;
                }
            }
        }

        private static void OnInvalidate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            control?.InvalidateMeasure();
        }

        private static void MaxItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            control?.BuildSlots();
            control?.ArrangeItems();
            control?.InvalidateMeasure();
        }

        private static void SelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Carousel;
            if (control != null && control._disableSelectedIndexCallback)
            {
                return;
            }

            if ((int)e.NewValue > -1)
            {
                control?.ArrangeItems();
            }
        }
    }
}

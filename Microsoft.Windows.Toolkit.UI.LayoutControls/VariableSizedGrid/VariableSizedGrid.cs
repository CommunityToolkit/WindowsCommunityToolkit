using Microsoft.Windows.Toolkit.UI.Controls.Primitives;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// The VariableSizedGrid control allows to display items from a list using different values 
    /// for Width and Height item properties. You can control the number of rows and columns to be
    /// displayed as well as the items orientation in the panel. Finally, the AspectRatio property
    /// allow us to control the relation between Width and Height.
    /// </summary>
    public class VariableSizedGrid : ListViewBase
    {
        private ScrollViewer _scrollViewer = null;
        private VariableSizedGridPanel _panel = null;

        private bool _isInitialized = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSizedGrid"/> class.
        /// </summary>
        public VariableSizedGrid()
        {
            this.DefaultStyleKey = typeof(VariableSizedGrid);
            this.LayoutUpdated += OnLayoutUpdated;
        }

        #region ItemMargin
        /// <summary>
        /// Gets or sets the margin for each item.
        /// </summary>
        /// <value>The item margin.</value>
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(nameof(ItemMargin), typeof(Thickness), typeof(VariableSizedGrid), new PropertyMetadata(new Thickness(2)));
        #endregion

        #region ItemPadding
        public Thickness ItemPadding
        {
            get { return (Thickness)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemPadding"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(nameof(ItemPadding), typeof(Thickness), typeof(VariableSizedGrid), new PropertyMetadata(new Thickness(2)));
        #endregion

        #region MaximumRowsOrColumns
        public int MaximumRowsOrColumns
        {
            get { return (int)GetValue(MaximumRowsOrColumnsProperty); }
            set { SetValue(MaximumRowsOrColumnsProperty, value); }
        }

        private static void MaximumRowsOrColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGrid;
            control.InvalidateMeasure();
        }

        /// <summary>
        /// Identifies the <see cref="MaximumRowsOrColumns"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumRowsOrColumnsProperty = DependencyProperty.Register(nameof(MaximumRowsOrColumns), typeof(int), typeof(VariableSizedGrid), new PropertyMetadata(4, MaximumRowsOrColumnsChanged));
        #endregion

        #region AspectRatio
        /// <summary>
        /// Gets or sets the height-to-width aspect ratio for each tile.
        /// </summary>
        /// <value>The aspect ratio.</value>
        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="AspectRatio"/> dependency property.
        /// </summary>
        private static void AspectRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGrid;
            control.InvalidateMeasure();
        }

        /// <summary>
        /// Identifies the <see cref="AspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register(nameof(AspectRatio), typeof(double), typeof(VariableSizedGrid), new PropertyMetadata(1.0, AspectRatioChanged));
        #endregion

        #region Orientation        
        /// <summary>
        /// Gets or sets the dimension by which child elements are stacked.
        /// </summary>
        /// <value>One of the enumeration values that specifies the orientation of child elements.</value>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGrid;
            control.SetOrientation((Orientation)e.NewValue);
        }

        /// <summary>
        /// Identifies the <see cref="Orientation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(VariableSizedGrid), new PropertyMetadata(Orientation.Horizontal, OrientationChanged));
        #endregion

        /// <summary>
        /// Creates or identifies the element that is used to display the given item.
        /// </summary>
        /// <returns>The element that is used to display the given item.</returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ListViewItem();
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">The element that's used to display the specified item.</param>
        /// <param name="item">The item to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var container = element as ListViewItem;
            container.Margin = this.ItemMargin;
            container.Padding = this.ItemPadding;
            base.PrepareContainerForItemOverride(element, item);
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass)
        /// call ApplyTemplate. In simplest terms, this means the method is called just before a UI
        /// element displays in your app. Override this method to influence the default post-template 
        /// logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            _scrollViewer = base.GetTemplateChild("scrollViewer") as ScrollViewer;

            _isInitialized = true;

            SetOrientation(this.Orientation);

            base.OnApplyTemplate();
        }

        private void SetOrientation(Orientation orientation)
        {
            if (_isInitialized)
            {
                if (orientation == Orientation.Horizontal)
                {
                    _scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    _scrollViewer.HorizontalScrollMode = ScrollMode.Disabled;
                    _scrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)this.GetValue(ScrollViewer.VerticalScrollBarVisibilityProperty);
                    _scrollViewer.VerticalScrollMode = ScrollMode.Auto;
                }
                else
                {
                    _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    _scrollViewer.VerticalScrollMode = ScrollMode.Disabled;
                    if ((ScrollBarVisibility)this.GetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty) == ScrollBarVisibility.Disabled)
                    {
                        _scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    }
                    else
                    {
                        _scrollViewer.HorizontalScrollBarVisibility = (ScrollBarVisibility)this.GetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty);
                    }
                    _scrollViewer.HorizontalScrollMode = ScrollMode.Auto;
                }
            }
        }

        private void OnLayoutUpdated(object sender, object e)
        {
            if (_panel == null)
            {
                _panel = base.ItemsPanelRoot as VariableSizedGridPanel;
                if (_panel != null)
                {
                    _panel.IsReady = true;
                    _panel.SetBinding(VariableSizedGridPanel.OrientationProperty, new Binding { Source = this, Path = new PropertyPath("Orientation") });
                    _panel.SetBinding(VariableSizedGridPanel.AspectRatioProperty, new Binding { Source = this, Path = new PropertyPath("AspectRatio") });
                    _panel.SetBinding(VariableSizedGridPanel.MaximumRowsOrColumnsProperty, new Binding { Source = this, Path = new PropertyPath("MaximumRowsOrColumns") });
                }
            }
        }
    }
}

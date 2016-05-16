using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// The ResponsiveGridView control allows to present information within a Grid View perfectly adjusting the
    /// total display available space. It reacts to changes in the layout as well as the content so it can adapt
    /// to different form factors automatically.
    /// </summary>
    /// <remarks>
    /// The number and the width of items are calculated based on the
    /// screen resolution in order to fully leverage the available screen space. The property ItemsHeight define
    /// the items fixed height and the property DesiredWidth sets the minimum width for the elements to add a 
    /// new column.</remarks>
    [TemplatePart(Name = "ListView", Type = typeof(ListViewBase))]
    public sealed partial class ResponsiveGridView : Control
    {
        private int _columns;
        private bool _isInitialized;
        private ListViewBase _listView;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponsiveGridView"/> class.
        /// </summary>
        public ResponsiveGridView()
        {
            this.IsTabStop = false;
            this.DefaultStyleKey = typeof(ResponsiveGridView);
        }
        
        private void RecalculateLayout(double containerWidth)
        {
            if (containerWidth == 0 || DesiredWidth == 0)
            {
                return;
            }
            if (_columns == 0)
            {
                _columns = CalculateColumns(containerWidth, DesiredWidth);
            }
            else
            {
                var desiredColumns = CalculateColumns(containerWidth, DesiredWidth);
                if (desiredColumns != _columns)
                {
                    _columns = desiredColumns;
                }
            }
            ItemWidth = (containerWidth / _columns) - 5;
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call 
        /// ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays
        /// in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_listView != null)
            {
                _listView.SizeChanged -= ListView_SizeChanged;
                _listView.ItemClick -= ListView_ItemClick;  
                _listView = null;
            }

            _listView = GetTemplateChild("ListView") as ListViewBase;
            if (_listView != null)
            {
                _listView.SizeChanged += ListView_SizeChanged;
                _listView.ItemClick += ListView_ItemClick;
            }
            _isInitialized = true;
            OnOneRowModeEnabledChanged(this, OneRowModeEnabled);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var cmd = ItemClickCommand;
            if (cmd != null)
            {
                if (cmd.CanExecute(e.ClickedItem))
                {
                    cmd.Execute(e.ClickedItem);
                }
            }
        }

        private static int CalculateColumns(double containerWidth, double itemWidth)
        {
            var columns = (int)(containerWidth / itemWidth);
            if (columns == 0)
            {
                columns = 1;
            }
            return columns;
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //If the width of the internal list view changes, check if more or less columns needs to be rendered.
            if (e.PreviousSize.Width != e.NewSize.Width)
            {
                RecalculateLayout(e.NewSize.Width);
            }
        }
    }
}

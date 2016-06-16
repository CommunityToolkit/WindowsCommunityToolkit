// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
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
    public partial class VariableSizedGridView : ListViewBase
    {
        private ScrollViewer _scrollViewer;
        private VariableSizedGridViewPanel _viewPanel;

        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSizedGridView"/> class.
        /// </summary>
        public VariableSizedGridView()
        {
            DefaultStyleKey = typeof(VariableSizedGridView);
            LayoutUpdated += OnLayoutUpdated;
        }

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
            container.Margin = ItemMargin;
            container.Padding = ItemPadding;
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
            _scrollViewer = GetTemplateChild("scrollViewer") as ScrollViewer;

            _isInitialized = true;

            SetOrientation(Orientation);

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
                    _scrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)GetValue(ScrollViewer.VerticalScrollBarVisibilityProperty);
                    _scrollViewer.VerticalScrollMode = ScrollMode.Auto;
                }
                else
                {
                    _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    _scrollViewer.VerticalScrollMode = ScrollMode.Disabled;
                    if ((ScrollBarVisibility)GetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty) == ScrollBarVisibility.Disabled)
                    {
                        _scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    }
                    else
                    {
                        _scrollViewer.HorizontalScrollBarVisibility = (ScrollBarVisibility)GetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty);
                    }

                    _scrollViewer.HorizontalScrollMode = ScrollMode.Auto;
                }
            }
        }

        private void OnLayoutUpdated(object sender, object e)
        {
            if (_viewPanel == null)
            {
                _viewPanel = ItemsPanelRoot as VariableSizedGridViewPanel;
                if (_viewPanel != null)
                {
                    _viewPanel.IsReady = true;
                    _viewPanel.SetBinding(VariableSizedGridViewPanel.OrientationProperty, new Binding { Source = this, Path = new PropertyPath(nameof(Orientation)) });
                    _viewPanel.SetBinding(VariableSizedGridViewPanel.AspectRatioProperty, new Binding { Source = this, Path = new PropertyPath(nameof(AspectRatio)) });
                    _viewPanel.SetBinding(VariableSizedGridViewPanel.MaximumRowsOrColumnsProperty, new Binding { Source = this, Path = new PropertyPath(nameof(MaximumRowsOrColumns)) });
                }
            }
        }
    }
}

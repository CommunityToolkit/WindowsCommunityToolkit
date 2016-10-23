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

using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A container that hosts <see cref="BladeItem"/> controls in a horizontal scrolling list
    /// Based on the Azure portal UI
    /// </summary>
    public partial class BladeControl : ItemsControl
    {
        private ScrollViewer _scrollViewer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BladeControl"/> class.
        /// </summary>
        public BladeControl()
        {
            DefaultStyleKey = typeof(BladeControl);
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture child controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CycleBlades();
        }

        /// <summary>
        /// Creates or identifies the element that is used to display the given item.
        /// </summary>
        /// <returns>
        /// The element that is used to display the given item.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new BladeItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var blade = element as BladeItem;
            if (blade != null)
            {
                blade.VisibilityChanged += BladeOnVisibilityChanged;
            }

            base.PrepareContainerForItemOverride(element, item);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            var blade = element as BladeItem;
            if (blade != null)
            {
                blade.VisibilityChanged -= BladeOnVisibilityChanged;
            }

            base.ClearContainerForItemOverride(element, item);
        }

        private void CycleBlades()
        {
            ActiveBlades = new ObservableCollection<BladeItem>();
            foreach (var blade in Items.OfType<BladeItem>())
            {
                if (blade.IsOpen)
                {
                    ActiveBlades.Add(blade);
                }
            }
        }

        private void BladeOnVisibilityChanged(object sender, Visibility visibility)
        {
            var blade = sender as BladeItem;

            if (visibility == Visibility.Visible)
            {
                Items.Remove(blade);
                Items.Add(blade);
                BladeOpened?.Invoke(this, blade);
                ActiveBlades.Add(blade);
                UpdateLayout();
                GetScrollViewer()?.ChangeView(_scrollViewer.ScrollableWidth, null, null);
                return;
            }

            BladeClosed?.Invoke(this, blade);
            ActiveBlades.Remove(blade);
        }

        private ScrollViewer GetScrollViewer()
        {
            return _scrollViewer ?? (_scrollViewer = this.FindDescendant<ScrollViewer>());
        }
    }
}
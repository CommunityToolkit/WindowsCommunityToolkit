// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A container that hosts <see cref="BladeItem"/> controls in a horizontal scrolling list
    /// Based on the Azure portal UI
    /// </summary>
    public partial class BladeView : ItemsControl
    {
        private ScrollViewer _scrollViewer;

        private Dictionary<BladeItem, Size> _cachedBladeItemSizes = new Dictionary<BladeItem, Size>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BladeView"/> class.
        /// </summary>
        public BladeView()
        {
            DefaultStyleKey = typeof(BladeView);

            Items.VectorChanged += ItemsVectorChanged;

            Loaded += (sender, e) => AdjustBladeItemSize();
            SizeChanged += (sender, e) => AdjustBladeItemSize();
        }

        /// <summary>
        /// Override default OnApplyTemplate to capture child controls
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CycleBlades();
            AdjustBladeItemSize();
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

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">The element that's used to display the specified item.</param>
        /// <param name="item">The item to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var blade = element as BladeItem;
            if (blade != null)
            {
                blade.VisibilityChanged += BladeOnVisibilityChanged;
            }

            base.PrepareContainerForItemOverride(element, item);
            CycleBlades();
        }

        /// <summary>
        /// Undoes the effects of the PrepareContainerForItemOverride method.
        /// </summary>
        /// <param name="element">The container element.</param>
        /// <param name="item">The item.</param>
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

            // For now we skip this feature when blade mode is set to fullscreen
            if (AutoCollapseCountThreshold > 0 && BladeMode != BladeMode.Fullscreen && ActiveBlades.Any())
            {
                var openBlades = ActiveBlades.Where(item => item.TitleBarVisibility == Visibility.Visible).ToList();
                if (openBlades.Count > AutoCollapseCountThreshold)
                {
                    for (int i = 0; i < openBlades.Count - 1; i++)
                    {
                        openBlades[i].IsExpanded = false;
                    }
                }
            }
        }

        private async void BladeOnVisibilityChanged(object sender, Visibility visibility)
        {
            var blade = sender as BladeItem;

            if (visibility == Visibility.Visible)
            {
                if (Items == null)
                {
                    return;
                }

                Items.Remove(blade);
                Items.Add(blade);
                BladeOpened?.Invoke(this, blade);
                ActiveBlades.Add(blade);
                UpdateLayout();

                // Need to do this because of touch. See more information here: https://github.com/Microsoft/WindowsCommunityToolkit/issues/760#issuecomment-276466464
                await Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
                {
                    GetScrollViewer()?.ChangeView(_scrollViewer.ScrollableWidth, null, null);
                });

                return;
            }

            BladeClosed?.Invoke(this, blade);
            ActiveBlades.Remove(blade);

            var lastBlade = ActiveBlades.LastOrDefault();
            if (lastBlade != null && lastBlade.TitleBarVisibility == Visibility.Visible)
            {
                lastBlade.IsExpanded = true;
            }
        }

        private ScrollViewer GetScrollViewer()
        {
            return _scrollViewer ?? (_scrollViewer = this.FindDescendant<ScrollViewer>());
        }

        private void AdjustBladeItemSize()
        {
            // Adjust blade items to be full screen
            if (BladeMode == BladeMode.Fullscreen && GetScrollViewer() != null)
            {
                foreach (BladeItem blade in Items)
                {
                    blade.Width = _scrollViewer.ActualWidth;
                    blade.Height = _scrollViewer.ActualHeight;
                }
            }
        }

        private void ItemsVectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs e)
        {
            if (BladeMode == BladeMode.Fullscreen)
            {
                var bladeItem = (BladeItem)sender[(int)e.Index];
                if (bladeItem != null)
                {
                    if (!_cachedBladeItemSizes.ContainsKey(bladeItem))
                    {
                        // Execute change of blade item size when a blade item is added in Fullscreen mode
                        _cachedBladeItemSizes.Add(bladeItem, new Size(bladeItem.Width, bladeItem.Height));
                        AdjustBladeItemSize();
                    }
                }
            }
            else if (e.CollectionChange == CollectionChange.ItemInserted)
            {
                UpdateLayout();
                GetScrollViewer()?.ChangeView(_scrollViewer.ScrollableWidth, null, null);
            }
        }
    }
}
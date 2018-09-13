using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// TabView methods related to tracking Items and ItemsSource changes.
    /// </summary>
    public partial class TabView
    {
        // Temporary tracking of previous collections for removing events.
        private ItemCollection _previousItems;
        private INotifyCollectionChanged _previousItemsSource;
        private MethodInfo _removeItemsSourceMethod;

        /// <inheritdoc/>
        protected override void OnItemsChanged(object e)
        {
            // Un/Register for Items changes
            if (_previousItems != null)
            {
                _previousItems.VectorChanged -= Items_VectorChanged;
            }

            _previousItems = Items;

            if (Items != null)
            {
                Items.VectorChanged += Items_VectorChanged;
            }

            // Update Sizing (in case there are less items now)
            TabView_SizeChanged(this, null);

            base.OnItemsChanged(e);
        }

        private void Items_VectorChanged(Windows.Foundation.Collections.IObservableVector<object> sender, Windows.Foundation.Collections.IVectorChangedEventArgs @event)
        {
            if (@event.CollectionChange == Windows.Foundation.Collections.CollectionChange.ItemRemoved)
            {
                TabView_SizeChanged(this, null);
            }
        }

        private void ItemsSource_PropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (_previousItemsSource != null)
            {
                _previousItemsSource.CollectionChanged -= ItemsSource_CollectionChanged;
            }

            if (ItemsSource != null && ItemsSource is INotifyCollectionChanged obs)
            {
                _previousItemsSource = obs;

                obs.CollectionChanged += ItemsSource_CollectionChanged;
            }
            else
            {
                _previousItemsSource = null;
            }

            // Use reflection to store a 'Remove' method of any possible collection in ItemsSource
            // Cache for efficiency later.
            if (ItemsSource != null)
            {
                _removeItemsSourceMethod = ItemsSource.GetType().GetMethod("Remove");
            }
            else
            {
                _removeItemsSourceMethod = null;
            }
        }

        private void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                TabView_SizeChanged(this, null);
            }
        }

        private object GetTabSource()
        {
            if (ItemsSource != null)
            {
                return ItemsSource;
            }
            else if (Items != null)
            {
                return Items;
            }

            return null;
        }
    }
}

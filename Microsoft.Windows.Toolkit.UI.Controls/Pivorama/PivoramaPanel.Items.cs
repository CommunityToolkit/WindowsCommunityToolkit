using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// Defines the functionality for handling the items of the <see cref="PivoramaPanel"/> control.
    /// </summary>
    public partial class PivoramaPanel
    {
        /// <summary>
        /// Defines the <see cref="Index"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register(
            nameof(Index),
            typeof(int),
            typeof(PivoramaPanel),
            new PropertyMetadata(0, (d, e) => { ((PivoramaPanel)d).InvalidateMeasure(); }));

        /// <summary>
        /// Defines the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource),
            typeof(object),
            typeof(PivoramaPanel),
            new PropertyMetadata(null, ItemsSourceChanged));

        protected List<object> _items = new List<object>();

        /// <summary>
        /// Gets or sets the current index.
        /// </summary>
        public int Index
        {
            get
            {
                return (int)GetValue(IndexProperty);
            }
            set
            {
                SetValue(IndexProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        public object ItemsSource
        {
            get
            {
                return GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is IEnumerable))
            {
                throw new ArgumentException("ItemsSource");
            }

            var control = d as PivoramaPanel;
            if (control != null)
            {
                control.DetachNotificationEvents(e.OldValue as INotifyCollectionChanged);
                control.AttachNotificationEvents(e.NewValue as INotifyCollectionChanged);

                control.ItemsSourceChanged(e.NewValue as IEnumerable);
            }
        }

        private void AttachNotificationEvents(INotifyCollectionChanged notifyCollection)
        {
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += OnCollectionChanged;
            }
        }

        private void DetachNotificationEvents(INotifyCollectionChanged notifyCollection)
        {
            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged -= OnCollectionChanged;
            }
        }

        internal int GetIndexOf(object content)
        {
            return _items.IndexOf(content);
        }

        private void ItemsSourceChanged(IEnumerable items)
        {
            if (items != null)
            {
                _items.Clear();
                foreach (var item in items)
                {
                    _items.Add(item);
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    ClearChildren();
                    break;
                case NotifyCollectionChangedAction.Add:
                    int index = e.NewStartingIndex;
                    foreach (var item in e.NewItems)
                    {
                        AddItem(item, index++);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        RemoveItem(item);
                    }
                    break;
            }

            InvalidateMeasure();
        }

        private void ClearChildren()
        {
            _items.Clear();
        }

        private void AddItem(object item, int index = -1)
        {
            index = index < 0 ? _items.Count : index;
            _items.Insert(index, item);
        }

        private void RemoveItem(object item)
        {
            _items.Remove(item);
        }
    }
}
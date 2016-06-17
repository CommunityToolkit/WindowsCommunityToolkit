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
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// The Carousel offer an alternative to items visualization adding horizontal scroll to a set of items.
    /// The Carousel control is responsive by design, optimizing the visualization in the different form factors.
    /// You can control properties like the AspectRatio, MaxItems, MinHeight, MaxHeight, GradientOpacity and AlignmentX to properly behave depending on the resolution and space available.
    /// </summary>
    public partial class Carousel
    {
        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(Carousel), new PropertyMetadata(null, ItemsSourceChanged));

        /// <summary>
        /// Gets or sets an object source used to generate the content of the <see cref="Carousel"/>.
        /// </summary>
        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is IEnumerable))
            {
                throw new ArgumentException("ItemsSource");
            }

            var control = d as Carousel;

            control?.DetachNotificationEvents(e.OldValue as INotifyCollectionChanged);
            control?.AttachNotificationEvents(e.NewValue as INotifyCollectionChanged);

            control?.ItemsSourceChanged(e.NewValue as IEnumerable);
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

        private void ItemsSourceChanged(IEnumerable items)
        {
            if (_container != null)
            {
                int index = -1;
                ClearChildren();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        AddItem(item);
                        index = 0;
                    }
                }

                SelectedIndex = index;
                ArrangeItems();
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_container != null)
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

                ArrangeItems();
            }
        }

        private void ClearChildren()
        {
            SelectedIndex = -1;
            _items.Clear();
        }

        private void AddItem(object item, int index = -1)
        {
            index = index < 0 ? _items.Count : index;
            _items.Insert(index, item);
            SelectedIndex = Math.Max(0, SelectedIndex);
        }

        private void RemoveItem(object item)
        {
            _items.Remove(item);
            SelectedIndex = Math.Min(_items.Count - 1, SelectedIndex);
        }

        private void ArrangeItems()
        {
            if (_container != null)
            {
                var slots = _container.Children.Cast<ContentControl>().OrderBy(r => r.GetTranslateX()).ToArray();
                for (int n = 0; n < slots.Length; n++)
                {
                    if (_items.Count > 0)
                    {
                        int index = GetItemIndex(n);
                        var item = _items[index];
                        if (slots[n].Content != item)
                        {
                            slots[n].Content = item;
                        }
                    }
                    else
                    {
                        slots[n].Content = null;
                    }
                }
            }
        }

        private int GetItemIndex(int n)
        {
            int index = SelectedIndex + n - 1;
            return index.Mod(_items.Count);
        }
    }
}

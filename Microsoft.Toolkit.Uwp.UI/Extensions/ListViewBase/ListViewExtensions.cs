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

using System.Collections.Generic;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
    /// </summary>
    public static class ListViewExtensions
    {
        private static Dictionary<IObservableVector<object>, Windows.UI.Xaml.Controls.ListViewBase> _itemsForList = new Dictionary<IObservableVector<object>, Windows.UI.Xaml.Controls.ListViewBase>();

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding a <see cref="Brush"/> as an alternate background color to a <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty AlternateColorProperty = DependencyProperty.RegisterAttached("AlternateColor", typeof(Brush), typeof(ListViewExtensions), new PropertyMetadata(null, OnAlternateColorPropertyChanged));

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding a <see cref="DataTemplate"/> as an alternate template to a <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty AlternateItemTemplateProperty = DependencyProperty.RegisterAttached("AlternateItemTemplate", typeof(DataTemplate), typeof(ListViewExtensions), new PropertyMetadata(null, OnAlternateItemTemplatePropertyChanged));

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for setting the container content stretch direction on the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty StretchItemContainerDirectionProperty = DependencyProperty.RegisterAttached("StretchItemContainerDirection", typeof(StretchDirection), typeof(ListViewExtensions), new PropertyMetadata(null, OnStretchItemContainerDirectionPropertyChanged));

        /// <summary>
        /// Gets the alternate <see cref="Brush"/> associated with the specified <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/> to get the associated <see cref="Brush"/> from</param>
        /// <returns>The <see cref="Brush"/> associated with the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></returns>
        public static Brush GetAlternateColor(Windows.UI.Xaml.Controls.ListViewBase obj)
        {
            return (Brush)obj.GetValue(AlternateColorProperty);
        }

        /// <summary>
        /// Sets the alternate <see cref="Brush"/> associated with the specified <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/> to associate the <see cref="Brush"/> with</param>
        /// <param name="value">The <see cref="Brush"/> for binding to the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></param>
        public static void SetAlternateColor(Windows.UI.Xaml.Controls.ListViewBase obj, Brush value)
        {
            obj.SetValue(AlternateColorProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="DataTemplate"/> associated with the specified <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/> to get the associated <see cref="DataTemplate"/> from</param>
        /// <returns>The <see cref="DataTemplate"/> associated with the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></returns>
        public static DataTemplate GetAlternateItemTemplate(Windows.UI.Xaml.Controls.ListViewBase obj)
        {
            return (DataTemplate)obj.GetValue(AlternateItemTemplateProperty);
        }

        /// <summary>
        /// Sets the <see cref="DataTemplate"/> associated with the specified <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/> to associate the <see cref="DataTemplate"/> with</param>
        /// <param name="value">The <see cref="DataTemplate"/> for binding to the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></param>
        public static void SetAlternateItemTemplate(Windows.UI.Xaml.Controls.ListViewBase obj, DataTemplate value)
        {
            obj.SetValue(AlternateItemTemplateProperty, value);
        }

        /// <summary>
        /// Gets the stretch <see cref="StretchDirection"/> associated with the specified <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/> to get the associated <see cref="StretchDirection"/> from</param>
        /// <returns>The <see cref="StretchDirection"/> associated with the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></returns>
        public static StretchDirection GetStretchItemContainerDirection(Windows.UI.Xaml.Controls.ListViewBase obj)
        {
            return (StretchDirection)obj.GetValue(StretchItemContainerDirectionProperty);
        }

        /// <summary>
        /// Sets the stretch <see cref="StretchDirection"/> associated with the specified <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Controls.ListViewBase"/> to associate the <see cref="StretchDirection"/> with</param>
        /// <param name="value">The <see cref="StretchDirection"/> for binding to the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/></param>
        public static void SetStretchItemContainerDirection(Windows.UI.Xaml.Controls.ListViewBase obj, StretchDirection value)
        {
            obj.SetValue(StretchItemContainerDirectionProperty, value);
        }

        private static void OnAlternateColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Windows.UI.Xaml.Controls.ListViewBase listViewBase = sender as Windows.UI.Xaml.Controls.ListViewBase;

            if (listViewBase == null)
            {
                return;
            }

            listViewBase.ContainerContentChanging -= ColorContainerContentChanging;
            listViewBase.Items.VectorChanged -= ColorItemsVectorChanged;
            listViewBase.Unloaded -= OnListViewBaseUnloaded;

            _itemsForList[listViewBase.Items] = listViewBase;
            if (AlternateColorProperty != null)
            {
                listViewBase.ContainerContentChanging += ColorContainerContentChanging;
                listViewBase.Items.VectorChanged += ColorItemsVectorChanged;
                listViewBase.Unloaded += OnListViewBaseUnloaded;
            }
        }

        private static void ColorContainerContentChanging(Windows.UI.Xaml.Controls.ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var itemContainer = args.ItemContainer as Control;
            var itemIndex = sender.IndexFromContainer(itemContainer);

            SetItemContainerBackground(sender, itemContainer, itemIndex);
        }

        private static void OnAlternateItemTemplatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Windows.UI.Xaml.Controls.ListViewBase listViewBase = sender as Windows.UI.Xaml.Controls.ListViewBase;

            if (listViewBase == null)
            {
                return;
            }

            listViewBase.ContainerContentChanging -= ItemTemplateContainerContentChanging;

            if (AlternateItemTemplateProperty != null)
            {
                listViewBase.ContainerContentChanging += ItemTemplateContainerContentChanging;
            }
        }

        private static void ItemTemplateContainerContentChanging(Windows.UI.Xaml.Controls.ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var itemContainer = args.ItemContainer as SelectorItem;
            var itemIndex = sender.IndexFromContainer(itemContainer);

            if (itemIndex % 2 == 0)
            {
                itemContainer.ContentTemplate = GetAlternateItemTemplate(sender);
            }
            else
            {
                itemContainer.ContentTemplate = sender.ItemTemplate;
            }
        }

        private static void OnStretchItemContainerDirectionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Windows.UI.Xaml.Controls.ListViewBase listViewBase = sender as Windows.UI.Xaml.Controls.ListViewBase;

            if (listViewBase == null)
            {
                return;
            }

            listViewBase.ContainerContentChanging -= StretchItemContainerDirectionChanging;

            if (StretchItemContainerDirectionProperty != null)
            {
                listViewBase.ContainerContentChanging += StretchItemContainerDirectionChanging;
            }
        }

        private static void StretchItemContainerDirectionChanging(Windows.UI.Xaml.Controls.ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var itemContainer = args.ItemContainer as SelectorItem;
            var stretchDirection = GetStretchItemContainerDirection(sender);

            if (stretchDirection == StretchDirection.Vertical || stretchDirection == StretchDirection.Both)
            {
                itemContainer.VerticalContentAlignment = VerticalAlignment.Stretch;
            }

            if (stretchDirection == StretchDirection.Horizontal || stretchDirection == StretchDirection.Both)
            {
                itemContainer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            }
        }

        private static void OnListViewBaseUnloaded(object sender, RoutedEventArgs e)
        {
            Windows.UI.Xaml.Controls.ListViewBase listViewBase = sender as Windows.UI.Xaml.Controls.ListViewBase;
            _itemsForList.Remove(listViewBase.Items);

            listViewBase.ContainerContentChanging -= ColorContainerContentChanging;
            listViewBase.Items.VectorChanged -= ColorItemsVectorChanged;
            listViewBase.Unloaded -= OnListViewBaseUnloaded;
        }

        private static void ColorItemsVectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs args)
        {
            // If the index is at the end we can ignore
            if (args.Index == (sender.Count - 1))
            {
                return;
            }

            // Only need to handle Inserted and Removed because we'll handle everything else in the
            // ColorContainerContentChanging method
            if ((args.CollectionChange == CollectionChange.ItemInserted) || (args.CollectionChange == CollectionChange.ItemRemoved))
            {
                _itemsForList.TryGetValue(sender, out Windows.UI.Xaml.Controls.ListViewBase listViewBase);
                if (listViewBase == null)
                {
                    return;
                }

                int index = (int)args.Index;
                for (int i = index; i < sender.Count; i++)
                {
                    var itemContainer = listViewBase.ContainerFromIndex(i) as Control;
                    if (itemContainer != null)
                    {
                        SetItemContainerBackground(listViewBase, itemContainer, i);
                    }
                }
            }
        }

        private static void SetItemContainerBackground(Windows.UI.Xaml.Controls.ListViewBase sender, Control itemContainer, int itemIndex)
        {
            if (itemIndex % 2 == 0)
            {
                itemContainer.Background = GetAlternateColor(sender);
            }
            else
            {
                itemContainer.Background = null;
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides the Command attached dependency property for the <see cref="ListViewBase"/>
    /// </summary>
    public static partial class ListViewExtensions
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding an <see cref="ICommand"/> to handle ListViewBase Item interaction by means of <see cref="ListViewBase"/> ItemClick event. ListViewBase IsItemClickEnabled must be set to true.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(ListViewExtensions),
            new PropertyMetadata(null, OnCommandPropertyChanged));

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> to discard the null clicked items returned by <see cref="ListViewBase.ItemClick"/>.
        /// If we click on a <see cref="ListViewBase"/> item while the list is reordered, we may receive a <see cref="ListViewBase.ItemClick"/>
        /// event with a null <see cref="ItemClickEventArgs.ClickedItem"/> even if there is no null item within the list.
        /// Setting to true this property will prevent the associated command from being executed when the clicked item is null.
        /// Default value is false.
        /// </summary>
        public static readonly DependencyProperty DiscardNullClickedItemsProperty = DependencyProperty.RegisterAttached(
            "DiscardNullClickedItems",
            typeof(bool),
            typeof(ListViewExtensions),
            new PropertyMetadata(false));

        /// <summary>
        /// Gets the <see cref="ICommand"/> associated with the specified <see cref="ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> to get the associated <see cref="ICommand"/> from</param>
        /// <returns>The <see cref="ICommand"/> associated with the <see cref="ListViewBase"/></returns>
        public static ICommand GetCommand(ListViewBase obj) => (ICommand)obj.GetValue(CommandProperty);

        /// <summary>
        /// Sets the <see cref="ICommand"/> associated with the specified <see cref="ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> to associate the <see cref="ICommand"/> with</param>
        /// <param name="value">The <see cref="ICommand"/> for binding to the <see cref="ListViewBase"/></param>
        public static void SetCommand(ListViewBase obj, ICommand value) => obj.SetValue(CommandProperty, value);

        /// <summary>
        /// Gets the <see cref="DiscardNullClickedItemsProperty"/> value associated with the specified <see cref="ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> to get the associated <see cref="DiscardNullClickedItemsProperty"/> from</param>
        /// <returns>The <see cref="DiscardNullClickedItemsProperty"/> value associated with the <see cref="ListViewBase"/></returns>
        public static bool GetDiscardNullClickedItems(ListViewBase obj) => (bool)obj.GetValue(DiscardNullClickedItemsProperty);

        /// <summary>
        /// Sets the <see cref="DiscardNullClickedItemsProperty"/> value associated with the specified <see cref="ListViewBase"/>
        /// </summary>
        /// <param name="obj">The <see cref="ListViewBase"/> to associate the <see cref="ICommand"/> with</param>
        /// <param name="value">True to discard null clicked items.</param>
        public static void SetDiscardNullClickedItems(ListViewBase obj, bool value) => obj.SetValue(DiscardNullClickedItemsProperty, value);

        private static void OnCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var listViewBase = sender as ListViewBase;

            if (listViewBase == null)
            {
                return;
            }

            if (args.OldValue is ICommand oldCommand)
            {
                listViewBase.ItemClick -= OnListViewBaseItemClick;
            }

            if (args.NewValue is ICommand newCommand)
            {
                listViewBase.ItemClick += OnListViewBaseItemClick;
            }
        }

        private static void OnListViewBaseItemClick(object sender, ItemClickEventArgs e)
        {
            var listViewBase = sender as ListViewBase;
            if (listViewBase == null)
            {
                return;
            }

            if (GetDiscardNullClickedItems(listViewBase) && e.ClickedItem == null)
            {
                return;
            }

            var command = GetCommand(listViewBase);
            if (command?.CanExecute(e.ClickedItem) == true)
            {
                command.Execute(e.ClickedItem);
            }
        }
    }
}
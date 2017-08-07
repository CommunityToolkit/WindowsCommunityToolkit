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

using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="Windows.UI.Xaml.Controls.ListViewBase"/>
    /// </summary>
    public partial class ListViewBase
    {
        private static void OnCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Windows.UI.Xaml.Controls.ListViewBase listViewBase = sender as Windows.UI.Xaml.Controls.ListViewBase;

            if (listViewBase != null)
            {
                listViewBase.ItemClick -= OnItemClicked;

                ICommand command = args.NewValue as ICommand;

                if (command != null)
                {
                    listViewBase.ItemClick += OnItemClicked;
                }
            }
        }

        private static void OnItemClicked(object sender, ItemClickEventArgs args)
        {
            Windows.UI.Xaml.Controls.ListViewBase listViewBase = sender as Windows.UI.Xaml.Controls.ListViewBase;

            if (listViewBase == null)
            {
                return;
            }

            ICommand command = GetCommand(listViewBase);
            if (command != null && command.CanExecute(args.ClickedItem))
            {
                command.Execute(args.ClickedItem);
            }
        }

        private static void OnAlternateColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Windows.UI.Xaml.Controls.ListViewBase listViewBase = sender as Windows.UI.Xaml.Controls.ListViewBase;

            if (listViewBase == null)
            {
                return;
            }

            listViewBase.ContainerContentChanging -= ColorContainerContentChanging;

            if (AlternateColorProperty != null)
            {
                listViewBase.ContainerContentChanging += ColorContainerContentChanging;
            }
        }

        private static void ColorContainerContentChanging(Windows.UI.Xaml.Controls.ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var itemContainer = args.ItemContainer as SelectorItem;
            var itemIndex = sender.IndexFromContainer(itemContainer);

            if (itemIndex % 2 == 0)
            {
                itemContainer.Background = GetAlternateColor(sender);
            }
            else
            {
                itemContainer.Background = null;
            }
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
    }
}

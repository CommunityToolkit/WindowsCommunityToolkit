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

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="ListViewBase"/> that allows attaching an <see cref="ICommand"/>.
    /// This ICommand is executed when ListViewBase Item receives interaction by means of ItemClick. This requires IsItemClickEnabled to set to true.
    /// The ICommand is passed the Item that received interaction as a parameter
    /// </summary>
    public static class ListViewBaseExtensions
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding an <see cref="ICommand"/> instance to a <see cref="ListViewBase"/>
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(ListViewBaseExtensions),
            new PropertyMetadata(null, OnCommandPropertyChanged));

        /// <summary>
        /// Gets the <see cref="ICommand"/> instance assocaited with the specified <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> from which to get the associated <see cref="ICommand"/> instance</param>
        /// <returns>The <see cref="ICommand"/> instance associated with the the <see cref="DependencyObject"/> or null</returns>
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets the <see cref="ICommand"/> instance assocaited with the specified <see cref="DependencyObject"/>
        /// </summary>
        /// <param name="obj">The <see cref="DependencyObject"/> to associated the <see cref="ICommand"/> instance to</param>
        /// <param name="value">The <see cref="ICommand"/> instance to bind to the <see cref="DependencyObject"/></param>
        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        private static void OnCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            ListViewBase listViewBase = sender as ListViewBase;

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
            ListViewBase listViewBase = sender as ListViewBase;

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
    }
}

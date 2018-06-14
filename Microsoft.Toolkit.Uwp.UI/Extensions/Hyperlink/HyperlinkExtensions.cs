// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="Windows.UI.Xaml.Documents.Hyperlink"/> content element that allows
    /// it to invoke a <see cref="ICommand"/> when clicked
    /// </summary>
    public static class HyperlinkExtensions
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding an <see cref="ICommand"/> instance to a <see cref="Windows.UI.Xaml.Documents.Hyperlink"/>
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(Hyperlink), new PropertyMetadata(null, OnCommandPropertyChanged));

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding a command parameter to a <see cref="Windows.UI.Xaml.Documents.Hyperlink"/>
        /// </summary>
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(Hyperlink), new PropertyMetadata(null));

        /// <summary>
        /// Gets the <see cref="ICommand"/> instance assocaited with the specified <see cref="Windows.UI.Xaml.Documents.Hyperlink"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Documents.Hyperlink"/> from which to get the associated <see cref="ICommand"/> instance</param>
        /// <returns>The <see cref="ICommand"/> instance associated with the the <see cref="Windows.UI.Xaml.Documents.Hyperlink"/> or null</returns>
        public static ICommand GetCommand(Windows.UI.Xaml.Documents.Hyperlink obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets the <see cref="ICommand"/> instance assocaited with the specified <see cref="Windows.UI.Xaml.Documents.Hyperlink"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Documents.Hyperlink"/> to associated the <see cref="ICommand"/> instance to</param>
        /// <param name="value">The <see cref="ICommand"/> instance to bind to the <see cref="Windows.UI.Xaml.Documents.Hyperlink"/></param>
        public static void SetCommand(Windows.UI.Xaml.Documents.Hyperlink obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets the <see cref="CommandProperty"/> instance assocaited with the specified <see cref="Windows.UI.Xaml.Documents.Hyperlink"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Documents.Hyperlink"/> from which to get the associated <see cref="CommandProperty"/> value</param>
        /// <returns>The <see cref="CommandProperty"/> value associated with the the <see cref="Windows.UI.Xaml.Documents.Hyperlink"/> or null</returns>
        public static object GetCommandParameter(Windows.UI.Xaml.Documents.Hyperlink obj)
        {
            return obj.GetValue(CommandParameterProperty);
        }

        /// <summary>
        /// Sets the <see cref="CommandProperty"/> assocaited with the specified <see cref="Windows.UI.Xaml.Documents.Hyperlink"/>
        /// </summary>
        /// <param name="obj">The <see cref="Windows.UI.Xaml.Documents.Hyperlink"/> to associated the <see cref="CommandProperty"/> instance to</param>
        /// <param name="value">The <see cref="object"/> to set the <see cref="CommandProperty"/> to</param>
        public static void SetCommandParameter(Windows.UI.Xaml.Documents.Hyperlink obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        private static void OnCommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Windows.UI.Xaml.Documents.Hyperlink hyperlink = sender as Windows.UI.Xaml.Documents.Hyperlink;

            if (hyperlink != null)
            {
                hyperlink.Click -= OnHyperlinkClicked;

                ICommand command = args.NewValue as ICommand;

                if (command != null)
                {
                    hyperlink.Click += OnHyperlinkClicked;
                }
            }
        }

        private static void OnHyperlinkClicked(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            ICommand command = GetCommand(sender);
            object parameter = GetCommandParameter(sender);

            command?.Execute(parameter);
        }
    }
}

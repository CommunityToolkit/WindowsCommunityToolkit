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

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="Windows.UI.Xaml.Documents.Hyperlink"/> content element that allows
    /// it to invoke a <see cref="ICommand"/> when clicked
    /// </summary>
    public partial class Hyperlink
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
    }
}

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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Primitives
{
    /// <summary>
    /// Represents a non-scrollable grid that contains <see cref="DataGrid"/> row headers.
    /// </summary>
    public class DataGridFrozenGrid : Grid
    {
        /// <summary>
        /// A dependency property that indicates whether the grid is frozen.
        /// </summary>
        public static readonly DependencyProperty IsFrozenProperty = DependencyProperty.RegisterAttached(
            "IsFrozen",
            typeof(bool),
            typeof(DataGridFrozenGrid),
            null);

        /// <summary>
        /// Gets a value that indicates whether the grid is frozen.
        /// </summary>
        /// <param name="element">
        /// The object to get the IsFrozen value from.
        /// </param>
        /// <returns>true if the grid is frozen; otherwise, false. The default is true.</returns>
        public static bool GetIsFrozen(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (bool)element.GetValue(IsFrozenProperty);
        }

        /// <summary>
        /// Sets a value that indicates whether the grid is frozen.
        /// </summary>
        /// <param name="element">The object to set the IsFrozen value on.</param>
        /// <param name="value">true if <paramref name="element"/> is frozen; otherwise, false.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="element"/> is null.</exception>
        public static void SetIsFrozen(DependencyObject element, bool value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(IsFrozenProperty, value);
        }
   }
}

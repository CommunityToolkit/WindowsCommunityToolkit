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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents a control that displays a header that has a collapsible window that displays content.
    /// </summary>
    public partial class Expander : ContentControl
    {

        /// <summary>
        /// Event raised when the expander is expanded
        /// </summary>
        public event RoutedEventHandler ExpanderExpanded;

        /// <summary>
        /// Event raised when the expander is collapsed
        /// </summary>
        public event RoutedEventHandler ExpanderCollapsed;

        private void ExpanderButton_Clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}

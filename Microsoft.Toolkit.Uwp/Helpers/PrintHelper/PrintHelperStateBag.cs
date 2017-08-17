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

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// Internal class used to store values updated by the PrintHelper
    /// </summary>
    internal class PrintHelperStateBag
    {
        /// <summary>
        /// Gets or sets the stored horizontal alignment
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the stored vertical alignment
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        ///  Gets or sets the stored width
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        ///  Gets or sets the stored height
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        ///  Gets or sets the stored margin
        /// </summary>
        public Thickness Margin { get; set; }

        /// <summary>
        /// Capture the current element state
        /// </summary>
        /// <param name="element">Element to capture state from</param>
        public void Capture(FrameworkElement element)
        {
            HorizontalAlignment = element.HorizontalAlignment;
            VerticalAlignment = element.VerticalAlignment;
            Width = element.Width;
            Height = element.Height;
            Margin = element.Margin;
        }

        /// <summary>
        /// Restore stored state to given element
        /// </summary>
        /// <param name="element">Element to restore state to</param>
        public void Restore(FrameworkElement element)
        {
            element.HorizontalAlignment = HorizontalAlignment;
            element.VerticalAlignment = VerticalAlignment;
            element.Width = Width;
            element.Height = Height;
            element.Margin = Margin;
        }
    }
}

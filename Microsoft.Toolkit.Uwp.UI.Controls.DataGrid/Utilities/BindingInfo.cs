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
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Data.Utilities
{
    /// <summary>
    /// Stores information about a Binding, including the BindingExpression, BindingTarget and associated Element.
    /// </summary>
    internal class BindingInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingInfo"/> class.
        /// </summary>
        public BindingInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingInfo"/> class
        /// with the specified BindingExpression, DependencyProperty and FrameworkElement.
        /// </summary>
        /// <param name="bindingExpression">BindingExpression</param>
        /// <param name="bindingTarget">BindingTarget</param>
        /// <param name="element">Element</param>
        public BindingInfo(BindingExpression bindingExpression, DependencyProperty bindingTarget, FrameworkElement element)
        {
            this.BindingExpression = bindingExpression;
            this.BindingTarget = bindingTarget;
            this.Element = element;
        }

        /// <summary>
        /// Gets or sets the BindingExpression.
        /// </summary>
        public BindingExpression BindingExpression { get; set; }

        /// <summary>
        /// Gets or sets the BindingTarget.
        /// </summary>
        public DependencyProperty BindingTarget { get; set; }

        /// <summary>
        /// Gets or sets the Element.
        /// </summary>
        public FrameworkElement Element { get; set; }
    }
}
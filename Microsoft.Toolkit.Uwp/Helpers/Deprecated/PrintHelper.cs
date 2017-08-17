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
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// Helper class used to simplify document printing.
    /// Based on https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/Printing/cs/PrintHelper.cs />
    /// It allows you to render a framework element per page.
    /// </summary>
    [Obsolete("This class is being deprecated. Please use the Microsoft.Toolkit.Uwp.Helpers counterpart.")]
    public class PrintHelper : Helpers.PrintHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintHelper"/> class.
        /// </summary>
        /// <param name="canvasContainer">XAML panel used to attach printing canvas. Can be hidden in your UI with Opacity = 0 for instance</param>
        public PrintHelper(Panel canvasContainer)
            : base(canvasContainer)
        {
        }
    }
}
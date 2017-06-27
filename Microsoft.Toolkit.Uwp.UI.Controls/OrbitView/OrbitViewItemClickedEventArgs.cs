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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A class used by the <see cref="OrbitView"/> ItemClicked Event
    /// </summary>
    public class OrbitViewItemClickedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrbitViewItemClickedEventArgs"/> class.
        /// </summary>
        /// <param name="item">data context of element clicked</param>
        public OrbitViewItemClickedEventArgs(object item)
        {
            Item = item;
        }

        /// <summary>
        /// Gets or sets the Item/Data Context of the clicked item
        /// </summary>
        public object Item { get; set; }
    }
}

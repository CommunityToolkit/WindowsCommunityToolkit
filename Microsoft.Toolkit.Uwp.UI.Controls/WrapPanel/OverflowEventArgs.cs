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

namespace Microsoft.Toolkit.Uwp.UI.Controls.WrapPanel
{
    /// <summary>
    /// Event argument raised when the wrap panel's current row or column overflows.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class OverflowEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OverflowEventArgs" /> class.
        /// </summary>
        /// <param name="newIndex">The new index.</param>
        internal OverflowEventArgs(int newIndex)
        {
            this.NewIndex = newIndex;
        }

        /// <summary>
        /// Gets the index of the new row or column.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int NewIndex { get; private set; }
    }
}
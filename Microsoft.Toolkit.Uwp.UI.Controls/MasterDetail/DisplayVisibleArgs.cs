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
    ///     The event args for DisplayVisibleChanged event
    /// </summary>
    public class DisplayVisibleArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DisplayVisibleArgs" /> class.
        /// </summary>
        /// <param name="displayVisible">The visible display.</param>
        internal DisplayVisibleArgs(MasterDetailDisplayVisible displayVisible)
        {
            DisplayVisible = displayVisible;
        }

        /// <summary>
        ///     Gets or sets the visible display.
        /// </summary>
        /// <value>
        ///     The visible display.
        /// </value>
        public MasterDetailDisplayVisible DisplayVisible { get; set; }
    }
}
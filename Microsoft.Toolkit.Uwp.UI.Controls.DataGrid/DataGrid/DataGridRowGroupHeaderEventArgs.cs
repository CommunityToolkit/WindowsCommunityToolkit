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
    /// EventArgs used for the DataGrid's LoadingRowGroup and UnloadingRowGroup events
    /// </summary>
    public class DataGridRowGroupHeaderEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridRowGroupHeaderEventArgs"/> class.
        /// </summary>
        /// <param name="rowGroupHeader">The row group header that the event occurs for.</param>
        public DataGridRowGroupHeaderEventArgs(DataGridRowGroupHeader rowGroupHeader)
        {
            this.RowGroupHeader = rowGroupHeader;
        }

        /// <summary>
        /// Gets the <see cref="DataGridRowGroupHeader"/> associated with this instance.
        /// </summary>
        public DataGridRowGroupHeader RowGroupHeader
        {
            get;
            private set;
        }
    }
}

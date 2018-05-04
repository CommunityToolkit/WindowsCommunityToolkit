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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Provides data for the <see cref="E:Microsoft.Toolkit.Uwp.UI.Controls.DataGrid.LoadingRowDetails"/>, <see cref="E:Microsoft.Toolkit.Uwp.UI.Controls.DataGrid.UnloadingRowDetails"/>,
    /// and <see cref="E:Microsoft.Toolkit.Uwp.UI.Controls.DataGrid.RowDetailsVisibilityChanged"/> events.
    /// </summary>
    public class DataGridRowDetailsEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridRowDetailsEventArgs"/> class.
        /// </summary>
        /// <param name="row">The row that the event occurs for.</param>
        /// <param name="detailsElement">The row details section as a framework element.</param>
        public DataGridRowDetailsEventArgs(DataGridRow row, FrameworkElement detailsElement)
        {
            this.Row = row;
            this.DetailsElement = detailsElement;
        }

        /// <summary>
        /// Gets the row details section as a framework element.
        /// </summary>
        public FrameworkElement DetailsElement
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the row that the event occurs for.
        /// </summary>
        public DataGridRow Row
        {
            get;
            private set;
        }
    }
}

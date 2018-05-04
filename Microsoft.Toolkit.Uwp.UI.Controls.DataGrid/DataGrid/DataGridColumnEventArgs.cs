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
    /// Provides data for <see cref="DataGrid"/> column-related events.
    /// </summary>
    public class DataGridColumnEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridColumnEventArgs"/> class.
        /// </summary>
        /// <param name="column">The column that the event occurs for.</param>
        public DataGridColumnEventArgs(DataGridColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }

            this.Column = column;
        }

        /// <summary>
        /// Gets the column that the event occurs for.
        /// </summary>
        public DataGridColumn Column
        {
            get;
            private set;
        }
    }
}

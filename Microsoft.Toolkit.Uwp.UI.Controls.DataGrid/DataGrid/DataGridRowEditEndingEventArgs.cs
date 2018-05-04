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

using System.ComponentModel;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Provides information just before a row exits editing mode.
    /// </summary>
    public class DataGridRowEditEndingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridRowEditEndingEventArgs"/> class.
        /// </summary>
        /// <param name="row">The row container of the cell container that is about to exit edit mode.</param>
        /// <param name="editAction">The editing action that will be taken.</param>
        public DataGridRowEditEndingEventArgs(DataGridRow row, DataGridEditAction editAction)
        {
            this.Row = row;
            this.EditAction = editAction;
        }

        /// <summary>
        /// Gets the editing action that will be taken.
        /// </summary>
        public DataGridEditAction EditAction
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the row container of the cell container that is about to exit edit mode.
        /// </summary>
        public DataGridRow Row
        {
            get;
            private set;
        }
    }
}
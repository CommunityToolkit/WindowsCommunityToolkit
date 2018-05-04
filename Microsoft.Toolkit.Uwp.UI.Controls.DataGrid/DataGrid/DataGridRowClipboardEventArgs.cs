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
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// This class encapsulates a selected row's information necessary for the CopyingRowClipboardContent event.
    /// </summary>
    public class DataGridRowClipboardEventArgs : EventArgs
    {
        private List<DataGridClipboardCellContent> _clipboardRowContent;
        private bool _isColumnHeadersRow;
        private object _item;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridRowClipboardEventArgs"/> class.
        /// </summary>
        /// <param name="item">The row's associated data item.</param>
        /// <param name="isColumnHeadersRow">Whether or not this EventArgs is for the column headers.</param>
        internal DataGridRowClipboardEventArgs(object item, bool isColumnHeadersRow)
        {
            _isColumnHeadersRow = isColumnHeadersRow;
            _item = item;
        }

        /// <summary>
        /// Gets a list used to modify, add or remove a cell content before it gets stored into the clipboard.
        /// </summary>
        public List<DataGridClipboardCellContent> ClipboardRowContent
        {
            get
            {
                if (_clipboardRowContent == null)
                {
                    _clipboardRowContent = new List<DataGridClipboardCellContent>();
                }

                return _clipboardRowContent;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this property is true when the ClipboardRowContent represents column headers, in which case the Item is null.
        /// </summary>
        public bool IsColumnHeadersRow
        {
            get
            {
                return _isColumnHeadersRow;
            }
        }

        /// <summary>
        /// Gets the <see cref="DataGrid"/> row item used for preparing the ClipboardRowContent.
        /// </summary>
        public object Item
        {
            get
            {
                return _item;
            }
        }
    }
}
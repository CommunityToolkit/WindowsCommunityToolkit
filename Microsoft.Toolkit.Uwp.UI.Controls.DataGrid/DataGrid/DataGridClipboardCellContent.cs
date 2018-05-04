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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// This structure encapsulate the cell information necessary when clipboard content is prepared.
    /// </summary>
    public struct DataGridClipboardCellContent
    {
        private DataGridColumn _column;
        private object _content;
        private object _item;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataGridClipboardCellContent"/> struct.
        /// </summary>
        /// <param name="item">DataGrid row item containing the cell.</param>
        /// <param name="column">DataGridColumn containing the cell.</param>
        /// <param name="content">DataGrid cell value.</param>
        public DataGridClipboardCellContent(object item, DataGridColumn column, object content)
        {
            _item = item;
            _column = column;
            _content = content;
        }

        /// <summary>
        /// Gets the <see cref="DataGridColumn"/> column containing the cell.
        /// </summary>
        public DataGridColumn Column
        {
            get
            {
                return _column;
            }
        }

        /// <summary>
        /// Gets the <see cref="DataGridCell"/> cell content.
        /// </summary>
        public object Content
        {
            get
            {
                return _content;
            }
        }

        /// <summary>
        /// Gets the <see cref="DataGridRow"/> row item containing the cell.
        /// </summary>
        public object Item
        {
            get
            {
                return _item;
            }
        }

        /// <summary>
        /// Field-by-field comparison to avoid reflection-based ValueType.Equals.
        /// </summary>
        /// <param name="obj">DataGridClipboardCellContent to compare.</param>
        /// <returns>True iff this and data are equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is DataGridClipboardCellContent))
            {
                return false;
            }

            DataGridClipboardCellContent clipboardCellContent = (DataGridClipboardCellContent)obj;
            return _column == clipboardCellContent._column && _content == clipboardCellContent._content && _item == clipboardCellContent._item;
        }

        /// <summary>
        /// Returns a deterministic hash code.
        /// </summary>
        /// <returns>Hash value.</returns>
        public override int GetHashCode()
        {
            return (_column.GetHashCode() ^ _content.GetHashCode()) ^ _item.GetHashCode();
        }

        /// <summary>
        /// Field-by-field comparison to avoid reflection-based ValueType.Equals.
        /// </summary>
        /// <param name="clipboardCellContent1">The first DataGridClipboardCellContent.</param>
        /// <param name="clipboardCellContent2">The second DataGridClipboardCellContent.</param>
        /// <returns>True if and only if clipboardCellContent1 and clipboardCellContent2 are equal.</returns>
        public static bool operator ==(DataGridClipboardCellContent clipboardCellContent1, DataGridClipboardCellContent clipboardCellContent2)
        {
            return clipboardCellContent1._column == clipboardCellContent2._column && clipboardCellContent1._content == clipboardCellContent2._content && clipboardCellContent1._item == clipboardCellContent2._item;
        }

        /// <summary>
        /// Field-by-field comparison to avoid reflection-based ValueType.Equals.
        /// </summary>
        /// <param name="clipboardCellContent1">The first DataGridClipboardCellContent.</param>
        /// <param name="clipboardCellContent2">The second DataGridClipboardCellContent.</param>
        /// <returns>True iff clipboardCellContent1 and clipboardCellContent2 are NOT equal.</returns>
        public static bool operator !=(DataGridClipboardCellContent clipboardCellContent1, DataGridClipboardCellContent clipboardCellContent2)
        {
            if (clipboardCellContent1._column == clipboardCellContent2._column && clipboardCellContent1._content == clipboardCellContent2._content)
            {
                return clipboardCellContent1._item != clipboardCellContent2._item;
            }

            return true;
        }
    }
}
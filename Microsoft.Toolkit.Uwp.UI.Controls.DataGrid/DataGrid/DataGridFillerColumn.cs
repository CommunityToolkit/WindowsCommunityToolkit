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

using System.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Controls.Primitives;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class DataGridFillerColumn : DataGridColumn
    {
        public DataGridFillerColumn(DataGrid owningGrid)
        {
            this.IsReadOnly = true;
            this.OwningGrid = owningGrid;
            this.MinWidth = 0;
            this.MaxWidth = int.MaxValue;
        }

        internal double FillerWidth
        {
            get;
            set;
        }

        // True if there is room for the filler column; otherwise, false
        internal bool IsActive
        {
            get
            {
                return this.FillerWidth > 0;
            }
        }

        // True if the FillerColumn's header cell is contained in the visual tree
        internal bool IsRepresented
        {
            get;
            set;
        }

        internal override DataGridColumnHeader CreateHeader()
        {
            DataGridColumnHeader headerCell = base.CreateHeader();
            if (headerCell != null)
            {
                headerCell.IsEnabled = false;
            }

            return headerCell;
        }

        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            return null;
        }

        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            return null;
        }

        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            Debug.Assert(false, "Unexpected call to DataGridFillerColumn.PrepareCellForEdit.");

            return null;
        }
    }
}

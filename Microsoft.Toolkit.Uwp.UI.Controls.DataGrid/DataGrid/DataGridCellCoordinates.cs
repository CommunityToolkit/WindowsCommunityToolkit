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

using System.Globalization;

namespace Microsoft.Toolkit.Uwp.UI.Controls.DataGridInternals
{
    internal class DataGridCellCoordinates
    {
        public DataGridCellCoordinates(int columnIndex, int slot)
        {
            this.ColumnIndex = columnIndex;
            this.Slot = slot;
        }

        public DataGridCellCoordinates(DataGridCellCoordinates dataGridCellCoordinates)
            : this(dataGridCellCoordinates.ColumnIndex, dataGridCellCoordinates.Slot)
        {
        }

        public int ColumnIndex
        {
            get;
            set;
        }

        public int Slot
        {
            get;
            set;
        }

        public override bool Equals(object o)
        {
            DataGridCellCoordinates dataGridCellCoordinates = o as DataGridCellCoordinates;
            if (dataGridCellCoordinates != null)
            {
                return dataGridCellCoordinates.ColumnIndex == this.ColumnIndex && dataGridCellCoordinates.Slot == this.Slot;
            }

            return false;
        }

        // Avoiding build warning CS0659: 'DataGridCellCoordinates' overrides Object.Equals(object o) but does not override Object.GetHashCode()
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

#if DEBUG
        public override string ToString()
        {
            return "DataGridCellCoordinates {ColumnIndex = " + this.ColumnIndex.ToString(CultureInfo.CurrentCulture) +
                   ", Slot = " + this.Slot.ToString(CultureInfo.CurrentCulture) + "}";
        }
#endif
    }
}

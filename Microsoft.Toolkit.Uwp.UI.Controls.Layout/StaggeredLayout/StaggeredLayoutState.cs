// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class StaggeredLayoutState
    {
        private List<StaggeredItem> _items = new List<StaggeredItem>();
        private VirtualizingLayoutContext _context;
        private Dictionary<int, StaggeredColumnLayout> _columnLayout = new Dictionary<int, StaggeredColumnLayout>();

        public StaggeredLayoutState(VirtualizingLayoutContext context)
        {
            _context = context;
        }

        public double ColumnWidth { get; internal set; }

        internal void AddItemToColumn(StaggeredItem item, int columnIndex)
        {
            if (_columnLayout.TryGetValue(columnIndex, out StaggeredColumnLayout columnLayout) == false)
            {
                columnLayout = new StaggeredColumnLayout();
                _columnLayout[columnIndex] = columnLayout;
            }
            if (columnLayout.Contains(item) == false)
            {
                columnLayout.Add(item);
            }
        }

        internal StaggeredItem GetItemAt(int index)
        {
            if (index < 0) throw new IndexOutOfRangeException();

            if (index <= (_items.Count - 1))
            {
                return _items[index];
            }
            else
            {
                StaggeredItem item = new StaggeredItem(_context, index);
                _items.Add(item);
                return item;
            }
        }

        internal StaggeredColumnLayout GetColumnLayout(int columnIndex)
        {
            _columnLayout.TryGetValue(columnIndex, out StaggeredColumnLayout columnLayout);
            return columnLayout;
        }
    }
}

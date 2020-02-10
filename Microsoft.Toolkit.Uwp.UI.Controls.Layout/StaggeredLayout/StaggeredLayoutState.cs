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

        public StaggeredLayoutState(VirtualizingLayoutContext context)
        {
            _context = context;
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

    }
}

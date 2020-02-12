// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class WrapLayoutState
    {
        private List<WrapItem> _items = new List<WrapItem>();

        public WrapLayoutState()
        {
        }

        internal WrapItem GetItemAt(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException();
            }

            if (index <= (_items.Count - 1))
            {
                return _items[index];
            }
            else
            {
                WrapItem item = new WrapItem(index);
                _items.Add(item);
                return item;
            }
        }

    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class WrapLayoutState
    {
        private List<WrapItem> _items = new List<WrapItem>();

        public WrapLayoutState()
        {
        }

        public Orientation Orientation { get; private set; }
        public double AvailableU { get; internal set; }

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

        internal void Clear()
        {
            _items.Clear();
        }

        internal void RemoveFromIndex(int index)
        {
            if (index > _items.Count)
            {
                // Item was added/removed but we haven't realized that far yet
                return;
            }

            int numToRemove = _items.Count - index;
            _items.RemoveRange(index, numToRemove);
        }

        internal void SetOrientation(Orientation orientation)
        {
            foreach (var item in _items.Where(i => i.Measure.HasValue))
            {
                UvMeasure measure = item.Measure.Value;
                double v = measure.V;
                measure.V = measure.U;
                measure.U = v;
                item.Measure = measure;
            }

            Orientation = orientation;
            AvailableU = 0;
        }

        internal void ClearPositions()
        {
            foreach (var item in _items)
            {
                item.Position = null;
            }
        }
    }
}
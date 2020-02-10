// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    internal class StaggeredItem
    {
        private VirtualizingLayoutContext _context;
        private int _index;
        private Size? _size;

        public StaggeredItem(VirtualizingLayoutContext context, int index)
        {
            _context = context;
            this._index = index;
        }

        public double Top { get; internal set; }

        internal Size Measure(double columnWidth, double availableHeight)
        {
            if (_size == null)
            {
                UIElement element = GetElement();

                element.Measure(new Size(columnWidth, availableHeight));
                _size = element.DesiredSize;
            }

            return _size.Value;
        }

        private UIElement GetElement()
        {
            return _context.GetOrCreateElementAt(_index);
        }
    }
}
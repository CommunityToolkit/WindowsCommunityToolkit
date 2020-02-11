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
        private Size? _size;

        public StaggeredItem(VirtualizingLayoutContext context, int index)
        {
            _context = context;
            this.Index = index;
        }

        public double Top { get; internal set; }

        public double Height { get; internal set; }

        public int Index { get; }

        internal Size Measure(double columnWidth, double availableHeight)
        {
            if (_size == null)
            {
                UIElement element = GetElement();

                element.Measure(new Size(columnWidth, availableHeight));
                _size = element.DesiredSize;
                Height = _size.Value.Height;
            }

            return _size.Value;
        }

        private UIElement GetElement()
        {
            return _context.GetOrCreateElementAt(Index);
        }

        internal void Arrange(Rect bounds)
        {
            UIElement element = GetElement();
            element.Arrange(bounds);
        }

        internal void RecycleElement()
        {
            UIElement element = GetElement();
            _context.RecycleElement(element);
        }
    }
}
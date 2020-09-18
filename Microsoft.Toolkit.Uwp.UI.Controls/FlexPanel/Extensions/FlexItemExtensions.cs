// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See the LICENSE file in the project root
// for the license information.
//
// Author(s):
//  - Laurent Sansonetti (native Xamarin flex https://github.com/xamarin/flex)
//  - Stephane Delcroix (.NET port)
//  - Ben Askren (UWP/Uno port)
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Microsoft.Toolkit.Uwp.UI.Controls.FlexPanel;

namespace Microsoft.Toolkit.Uwp.UI.Controls.FlexPanelExtensions
{
    internal static class FlexItemExtensions
    {
        public static int IndexOf(this FlexItem parent, FlexItem child)
        {
            var index = -1;
            foreach (var it in parent)
            {
                index++;
                if (it == child)
                {
                    return index;
                }
            }

            return -1;
        }

        public static void Remove(this FlexItem parent, FlexItem child)
        {
            var index = parent.IndexOf(child);
            if (index < 0)
            {
                return;
            }

            parent.RemoveAt((uint)index);
        }

        public static Rect GetFrame(this FlexItem item)
        {
            return new Rect(item.Frame[0], item.Frame[1], Math.Max(item.Frame[2], 0), Math.Max(item.Frame[3], 0));
        }

        public static Size GetConstraints(this FlexItem item)
        {
            var widthConstraint = -1d;
            var heightConstraint = -1d;
            var parent = item.Parent;
            do
            {
                if (parent == null)
                {
                    break;
                }

                if (widthConstraint < 0 && !double.IsNaN(parent.Width))
                {
                    widthConstraint = (double)parent.Width;
                }

                if (heightConstraint < 0 && !double.IsNaN(parent.Height))
                {
                    heightConstraint = (double)parent.Height;
                }

                parent = parent.Parent;
            }
            while (widthConstraint < 0 || heightConstraint < 0);
            return new Size(widthConstraint, heightConstraint);
        }

        public static void SetPadding(this FlexItem item, Thickness padding)
        {
            item.PaddingLeft = (double)padding.Left;
            item.PaddingTop = (double)padding.Top;
            item.PaddingRight = (double)padding.Right;
            item.PaddingBottom = (double)padding.Bottom;
        }
    }
}
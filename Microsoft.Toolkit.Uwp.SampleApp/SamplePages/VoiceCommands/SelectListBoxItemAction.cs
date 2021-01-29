// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public class SelectListBoxItemAction : DependencyObject, IAction
    {
        public Direction MoveDirection
        {
            get => (Direction)GetValue(MoveDirectionProperty);
            set => SetValue(MoveDirectionProperty, value);
        }

        public static readonly DependencyProperty MoveDirectionProperty = DependencyProperty.Register(nameof(MoveDirection), typeof(Direction), typeof(SelectListBoxItemAction), new PropertyMetadata(default(Direction)));

        public object Execute(object sender, object parameter)
        {
            if (sender is ListBox lb)
            {
                var selectedIndex = lb.SelectedIndex;
                switch (MoveDirection)
                {
                    case Direction.Up when selectedIndex > 0:
                        lb.SelectedIndex--;
                        break;
                    case Direction.Down when selectedIndex < lb.Items.Count - 1:
                        lb.SelectedIndex++;
                        break;
                    case Direction.First when lb.Items.Count > 0:
                        lb.SelectedIndex = 0;
                        break;
                    case Direction.Last when lb.Items.Count > 0:
                        lb.SelectedIndex = lb.Items.Count - 1;
                        break;
                    default:
                        break;
                }
            }

            return null;
        }

        public enum Direction
        {
            Up,
            Down,
            First,
            Last
        }
    }
}
